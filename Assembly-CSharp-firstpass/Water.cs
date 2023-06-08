using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class Water : MonoBehaviour
{
	public enum WaterMode
	{
		Simple,
		Reflective,
		Refractive
	}

	public WaterMode m_WaterMode = WaterMode.Refractive;

	public bool m_DisablePixelLights = true;

	public int m_TextureSize = 256;

	public float m_ClipPlaneOffset = 0.07f;

	public LayerMask m_ReflectLayers = -1;

	public LayerMask m_RefractLayers = -1;

	private Hashtable m_ReflectionCameras = new Hashtable();

	private Hashtable m_RefractionCameras = new Hashtable();

	private RenderTexture m_ReflectionTexture;

	private RenderTexture m_RefractionTexture;

	private WaterMode m_HardwareWaterSupport = WaterMode.Refractive;

	private int m_OldReflectionTextureSize;

	private int m_OldRefractionTextureSize;

	private static bool s_InsideWater;

	public void OnWillRenderObject()
	{
		if (!base.enabled || !base.renderer || !base.renderer.sharedMaterial || !base.renderer.enabled)
		{
			return;
		}
		Camera current = Camera.current;
		if ((bool)current && !s_InsideWater)
		{
			s_InsideWater = true;
			m_HardwareWaterSupport = FindHardwareWaterSupport();
			WaterMode waterMode = GetWaterMode();
			CreateWaterObjects(current, out Camera reflectionCamera, out Camera refractionCamera);
			Vector3 position = base.transform.position;
			Vector3 up = base.transform.up;
			int pixelLightCount = QualitySettings.pixelLightCount;
			if (m_DisablePixelLights)
			{
				QualitySettings.pixelLightCount = 0;
			}
			UpdateCameraModes(current, reflectionCamera);
			UpdateCameraModes(current, refractionCamera);
			if (waterMode >= WaterMode.Reflective)
			{
				float w = 0f - Vector3.Dot(up, position) - m_ClipPlaneOffset;
				Vector4 plane = new Vector4(up.x, up.y, up.z, w);
				Matrix4x4 reflectionMat = Matrix4x4.zero;
				CalculateReflectionMatrix(ref reflectionMat, plane);
				Vector3 position2 = current.transform.position;
				Vector3 position3 = reflectionMat.MultiplyPoint(position2);
				reflectionCamera.worldToCameraMatrix = current.worldToCameraMatrix * reflectionMat;
				Vector4 clipPlane = CameraSpacePlane(reflectionCamera, position, up, 1f);
				Matrix4x4 projection = current.projectionMatrix;
				CalculateObliqueMatrix(ref projection, clipPlane);
				reflectionCamera.projectionMatrix = projection;
				reflectionCamera.cullingMask = (-17 & m_ReflectLayers.value);
				reflectionCamera.targetTexture = m_ReflectionTexture;
				GL.SetRevertBackfacing(revertBackFaces: true);
				reflectionCamera.transform.position = position3;
				Vector3 eulerAngles = current.transform.eulerAngles;
				reflectionCamera.transform.eulerAngles = new Vector3(0f - eulerAngles.x, eulerAngles.y, eulerAngles.z);
				reflectionCamera.Render();
				reflectionCamera.transform.position = position2;
				GL.SetRevertBackfacing(revertBackFaces: false);
				base.renderer.sharedMaterial.SetTexture("_ReflectionTex", m_ReflectionTexture);
			}
			if (waterMode >= WaterMode.Refractive)
			{
				refractionCamera.worldToCameraMatrix = current.worldToCameraMatrix;
				Vector4 clipPlane2 = CameraSpacePlane(refractionCamera, position, up, -1f);
				Matrix4x4 projection2 = current.projectionMatrix;
				CalculateObliqueMatrix(ref projection2, clipPlane2);
				refractionCamera.projectionMatrix = projection2;
				refractionCamera.cullingMask = (-17 & m_RefractLayers.value);
				refractionCamera.targetTexture = m_RefractionTexture;
				refractionCamera.transform.position = current.transform.position;
				refractionCamera.transform.rotation = current.transform.rotation;
				refractionCamera.Render();
				base.renderer.sharedMaterial.SetTexture("_RefractionTex", m_RefractionTexture);
			}
			if (m_DisablePixelLights)
			{
				QualitySettings.pixelLightCount = pixelLightCount;
			}
			switch (waterMode)
			{
			case WaterMode.Simple:
				Shader.EnableKeyword("WATER_SIMPLE");
				Shader.DisableKeyword("WATER_REFLECTIVE");
				Shader.DisableKeyword("WATER_REFRACTIVE");
				break;
			case WaterMode.Reflective:
				Shader.DisableKeyword("WATER_SIMPLE");
				Shader.EnableKeyword("WATER_REFLECTIVE");
				Shader.DisableKeyword("WATER_REFRACTIVE");
				break;
			case WaterMode.Refractive:
				Shader.DisableKeyword("WATER_SIMPLE");
				Shader.DisableKeyword("WATER_REFLECTIVE");
				Shader.EnableKeyword("WATER_REFRACTIVE");
				break;
			}
			s_InsideWater = false;
		}
	}

	private void OnDisable()
	{
		if ((bool)m_ReflectionTexture)
		{
			UnityEngine.Object.DestroyImmediate(m_ReflectionTexture);
			m_ReflectionTexture = null;
		}
		if ((bool)m_RefractionTexture)
		{
			UnityEngine.Object.DestroyImmediate(m_RefractionTexture);
			m_RefractionTexture = null;
		}
		foreach (DictionaryEntry reflectionCamera in m_ReflectionCameras)
		{
			UnityEngine.Object.DestroyImmediate(((Camera)reflectionCamera.Value).gameObject);
		}
		m_ReflectionCameras.Clear();
		foreach (DictionaryEntry refractionCamera in m_RefractionCameras)
		{
			UnityEngine.Object.DestroyImmediate(((Camera)refractionCamera.Value).gameObject);
		}
		m_RefractionCameras.Clear();
	}

	private void Update()
	{
		if ((bool)base.renderer)
		{
			Material sharedMaterial = base.renderer.sharedMaterial;
			if ((bool)sharedMaterial)
			{
				Vector4 vector = sharedMaterial.GetVector("WaveSpeed");
				float @float = sharedMaterial.GetFloat("_WaveScale");
				Vector4 vector2 = new Vector4(@float, @float, @float * 0.4f, @float * 0.45f);
				double num = (double)Time.timeSinceLevelLoad / 20.0;
				Vector4 vector3 = new Vector4((float)Math.IEEERemainder((double)(vector.x * vector2.x) * num, 1.0), (float)Math.IEEERemainder((double)(vector.y * vector2.y) * num, 1.0), (float)Math.IEEERemainder((double)(vector.z * vector2.z) * num, 1.0), (float)Math.IEEERemainder((double)(vector.w * vector2.w) * num, 1.0));
				sharedMaterial.SetVector("_WaveOffset", vector3);
				sharedMaterial.SetVector("_WaveScale4", vector2);
				Vector3 size = base.renderer.bounds.size;
				Matrix4x4 matrix = Matrix4x4.TRS(s: new Vector3(size.x * vector2.x, size.z * vector2.y, 1f), pos: new Vector3(vector3.x, vector3.y, 0f), q: Quaternion.identity);
				sharedMaterial.SetMatrix("_WaveMatrix", matrix);
				matrix = Matrix4x4.TRS(s: new Vector3(size.x * vector2.z, size.z * vector2.w, 1f), pos: new Vector3(vector3.z, vector3.w, 0f), q: Quaternion.identity);
				sharedMaterial.SetMatrix("_WaveMatrix2", matrix);
			}
		}
	}

	private void UpdateCameraModes(Camera src, Camera dest)
	{
		if (dest == null)
		{
			return;
		}
		dest.clearFlags = src.clearFlags;
		dest.backgroundColor = src.backgroundColor;
		if (src.clearFlags == CameraClearFlags.Skybox)
		{
			Skybox skybox = src.GetComponent(typeof(Skybox)) as Skybox;
			Skybox skybox2 = dest.GetComponent(typeof(Skybox)) as Skybox;
			if (!skybox || !skybox.material)
			{
				skybox2.enabled = false;
			}
			else
			{
				skybox2.enabled = true;
				skybox2.material = skybox.material;
			}
		}
		dest.farClipPlane = src.farClipPlane;
		dest.nearClipPlane = src.nearClipPlane;
		dest.orthographic = src.orthographic;
		dest.fieldOfView = src.fieldOfView;
		dest.aspect = src.aspect;
		dest.orthographicSize = src.orthographicSize;
	}

	private void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
	{
		WaterMode waterMode = GetWaterMode();
		reflectionCamera = null;
		refractionCamera = null;
		if (waterMode >= WaterMode.Reflective)
		{
			if (!m_ReflectionTexture || m_OldReflectionTextureSize != m_TextureSize)
			{
				if ((bool)m_ReflectionTexture)
				{
					UnityEngine.Object.DestroyImmediate(m_ReflectionTexture);
				}
				m_ReflectionTexture = new RenderTexture(m_TextureSize, m_TextureSize, 16);
				m_ReflectionTexture.name = "__WaterReflection" + GetInstanceID().ToString();
				m_ReflectionTexture.isPowerOfTwo = true;
				m_ReflectionTexture.hideFlags = HideFlags.DontSave;
				m_OldReflectionTextureSize = m_TextureSize;
			}
			reflectionCamera = (m_ReflectionCameras[currentCamera] as Camera);
			if (!reflectionCamera)
			{
				GameObject gameObject = new GameObject("Water Refl Camera id" + GetInstanceID().ToString() + " for " + currentCamera.GetInstanceID().ToString(), typeof(Camera), typeof(Skybox));
				reflectionCamera = gameObject.camera;
				reflectionCamera.enabled = false;
				reflectionCamera.transform.position = base.transform.position;
				reflectionCamera.transform.rotation = base.transform.rotation;
				reflectionCamera.gameObject.AddComponent("FlareLayer");
				gameObject.hideFlags = HideFlags.HideAndDontSave;
				m_ReflectionCameras[currentCamera] = reflectionCamera;
			}
		}
		if (waterMode < WaterMode.Refractive)
		{
			return;
		}
		if (!m_RefractionTexture || m_OldRefractionTextureSize != m_TextureSize)
		{
			if ((bool)m_RefractionTexture)
			{
				UnityEngine.Object.DestroyImmediate(m_RefractionTexture);
			}
			m_RefractionTexture = new RenderTexture(m_TextureSize, m_TextureSize, 16);
			m_RefractionTexture.name = "__WaterRefraction" + GetInstanceID().ToString();
			m_RefractionTexture.isPowerOfTwo = true;
			m_RefractionTexture.hideFlags = HideFlags.DontSave;
			m_OldRefractionTextureSize = m_TextureSize;
		}
		refractionCamera = (m_RefractionCameras[currentCamera] as Camera);
		if (!refractionCamera)
		{
			GameObject gameObject2 = new GameObject("Water Refr Camera id" + GetInstanceID().ToString() + " for " + currentCamera.GetInstanceID().ToString(), typeof(Camera), typeof(Skybox));
			refractionCamera = gameObject2.camera;
			refractionCamera.enabled = false;
			refractionCamera.transform.position = base.transform.position;
			refractionCamera.transform.rotation = base.transform.rotation;
			refractionCamera.gameObject.AddComponent("FlareLayer");
			gameObject2.hideFlags = HideFlags.HideAndDontSave;
			m_RefractionCameras[currentCamera] = refractionCamera;
		}
	}

	private WaterMode GetWaterMode()
	{
		if (m_HardwareWaterSupport < m_WaterMode)
		{
			return m_HardwareWaterSupport;
		}
		return m_WaterMode;
	}

	private WaterMode FindHardwareWaterSupport()
	{
		if (!SystemInfo.supportsRenderTextures || !base.renderer)
		{
			return WaterMode.Simple;
		}
		Material sharedMaterial = base.renderer.sharedMaterial;
		if (!sharedMaterial)
		{
			return WaterMode.Simple;
		}
		string tag = sharedMaterial.GetTag("WATERMODE", searchFallbacks: false);
		if (tag == "Refractive")
		{
			return WaterMode.Refractive;
		}
		if (tag == "Reflective")
		{
			return WaterMode.Reflective;
		}
		return WaterMode.Simple;
	}

	private static float sgn(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 v = pos + normal * m_ClipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(v);
		Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(rhs.x, rhs.y, rhs.z, 0f - Vector3.Dot(lhs, rhs));
	}

	private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 b = projection.inverse * new Vector4(sgn(clipPlane.x), sgn(clipPlane.y), 1f, 1f);
		Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
		projection[2] = vector.x - projection[3];
		projection[6] = vector.y - projection[7];
		projection[10] = vector.z - projection[11];
		projection[14] = vector.w - projection[15];
	}

	private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
	}
}
