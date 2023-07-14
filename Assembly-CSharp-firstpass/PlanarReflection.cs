// Decompiled with JetBrains decompiler
// Type: PlanarReflection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21AF7BBC-70B8-4BE8-9CDE-C2EC2144EAE4
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (WaterBase))]
public class PlanarReflection : MonoBehaviour
{
  public LayerMask reflectionMask;
  public bool reflectSkybox;
  public Color clearColor = Color.grey;
  public string reflectionSampler = "_ReflectionTex";
  public float clipPlaneOffset = 0.07f;
  private Vector3 oldpos = Vector3.zero;
  private Camera reflectionCamera;
  private Material sharedMaterial;
  private Dictionary<Camera, bool> helperCameras;

  public void Start() => this.sharedMaterial = ((WaterBase) this.gameObject.GetComponent(typeof (WaterBase))).sharedMaterial;

  private Camera CreateReflectionCameraFor(Camera cam)
  {
    string name = this.gameObject.name + "Reflection" + cam.name;
    GameObject gameObject = GameObject.Find(name);
    if (!(bool) (UnityEngine.Object) gameObject)
      gameObject = new GameObject(name, new System.Type[1]
      {
        typeof (Camera)
      });
    if (!(bool) (UnityEngine.Object) gameObject.GetComponent(typeof (Camera)))
      gameObject.AddComponent(typeof (Camera));
    Camera camera = gameObject.camera;
    camera.backgroundColor = this.clearColor;
    camera.clearFlags = !this.reflectSkybox ? CameraClearFlags.Color : CameraClearFlags.Skybox;
    this.SetStandardCameraParameter(camera, this.reflectionMask);
    if (!(bool) (UnityEngine.Object) camera.targetTexture)
      camera.targetTexture = this.CreateTextureFor(cam);
    return camera;
  }

  private void SetStandardCameraParameter(Camera cam, LayerMask mask)
  {
    cam.cullingMask = (int) mask & ~(1 << LayerMask.NameToLayer("Water"));
    cam.backgroundColor = Color.black;
    cam.enabled = false;
  }

  private RenderTexture CreateTextureFor(Camera cam)
  {
    RenderTexture textureFor = new RenderTexture(Mathf.FloorToInt(cam.pixelWidth * 0.5f), Mathf.FloorToInt(cam.pixelHeight * 0.5f), 24);
    textureFor.hideFlags = HideFlags.DontSave;
    return textureFor;
  }

  public void RenderHelpCameras(Camera currentCam)
  {
    if (this.helperCameras == null)
      this.helperCameras = new Dictionary<Camera, bool>();
    if (!this.helperCameras.ContainsKey(currentCam))
      this.helperCameras.Add(currentCam, false);
    if (this.helperCameras[currentCam])
      return;
    if (!(bool) (UnityEngine.Object) this.reflectionCamera)
      this.reflectionCamera = this.CreateReflectionCameraFor(currentCam);
    this.RenderReflectionFor(currentCam, this.reflectionCamera);
    this.helperCameras[currentCam] = true;
  }

  public void LateUpdate()
  {
    if (this.helperCameras == null)
      return;
    this.helperCameras.Clear();
  }

  public void WaterTileBeingRendered(Transform tr, Camera currentCam)
  {
    this.RenderHelpCameras(currentCam);
    if (!(bool) (UnityEngine.Object) this.reflectionCamera || !(bool) (UnityEngine.Object) this.sharedMaterial)
      return;
    this.sharedMaterial.SetTexture(this.reflectionSampler, (Texture) this.reflectionCamera.targetTexture);
  }

  public void OnEnable()
  {
    Shader.EnableKeyword("WATER_REFLECTIVE");
    Shader.DisableKeyword("WATER_SIMPLE");
  }

  public void OnDisable()
  {
    Shader.EnableKeyword("WATER_SIMPLE");
    Shader.DisableKeyword("WATER_REFLECTIVE");
  }

  private void RenderReflectionFor(Camera cam, Camera reflectCamera)
  {
    if (!(bool) (UnityEngine.Object) reflectCamera || (bool) (UnityEngine.Object) this.sharedMaterial && !this.sharedMaterial.HasProperty(this.reflectionSampler))
      return;
    reflectCamera.cullingMask = (int) this.reflectionMask & ~(1 << LayerMask.NameToLayer("Water"));
    this.SaneCameraSettings(reflectCamera);
    reflectCamera.backgroundColor = this.clearColor;
    reflectCamera.clearFlags = !this.reflectSkybox ? CameraClearFlags.Color : CameraClearFlags.Skybox;
    if (this.reflectSkybox && (bool) (UnityEngine.Object) cam.gameObject.GetComponent(typeof (Skybox)))
    {
      Skybox skybox = (Skybox) reflectCamera.gameObject.GetComponent(typeof (Skybox));
      if (!(bool) (UnityEngine.Object) skybox)
        skybox = (Skybox) reflectCamera.gameObject.AddComponent(typeof (Skybox));
      skybox.material = ((Skybox) cam.GetComponent(typeof (Skybox))).material;
    }
    GL.SetRevertBackfacing(true);
    Transform transform = this.transform;
    Vector3 eulerAngles1 = cam.transform.eulerAngles;
    reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles1.x, eulerAngles1.y, eulerAngles1.z);
    reflectCamera.transform.position = cam.transform.position;
    Vector3 position = transform.transform.position with
    {
      y = transform.position.y
    };
    Vector3 up = transform.transform.up;
    float w = -Vector3.Dot(up, position) - this.clipPlaneOffset;
    Matrix4x4 reflectionMatrix = PlanarReflection.CalculateReflectionMatrix(Matrix4x4.zero, new Vector4(up.x, up.y, up.z, w));
    this.oldpos = cam.transform.position;
    Vector3 vector3 = reflectionMatrix.MultiplyPoint(this.oldpos);
    reflectCamera.worldToCameraMatrix = cam.worldToCameraMatrix * reflectionMatrix;
    Vector4 clipPlane = this.CameraSpacePlane(reflectCamera, position, up, 1f);
    Matrix4x4 obliqueMatrix = PlanarReflection.CalculateObliqueMatrix(cam.projectionMatrix, clipPlane);
    reflectCamera.projectionMatrix = obliqueMatrix;
    reflectCamera.transform.position = vector3;
    Vector3 eulerAngles2 = cam.transform.eulerAngles;
    reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles2.x, eulerAngles2.y, eulerAngles2.z);
    reflectCamera.Render();
    GL.SetRevertBackfacing(false);
  }

  private void SaneCameraSettings(Camera helperCam)
  {
    helperCam.depthTextureMode = DepthTextureMode.None;
    helperCam.backgroundColor = Color.black;
    helperCam.clearFlags = CameraClearFlags.Color;
    helperCam.renderingPath = RenderingPath.Forward;
  }

  private static Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
  {
    Vector4 b = projection.inverse * new Vector4(PlanarReflection.sgn(clipPlane.x), PlanarReflection.sgn(clipPlane.y), 1f, 1f);
    Vector4 vector4 = clipPlane * (2f / Vector4.Dot(clipPlane, b));
    projection[2] = vector4.x - projection[3];
    projection[6] = vector4.y - projection[7];
    projection[10] = vector4.z - projection[11];
    projection[14] = vector4.w - projection[15];
    return projection;
  }

  private static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane)
  {
    reflectionMat.m00 = (float) (1.0 - 2.0 * (double) plane[0] * (double) plane[0]);
    reflectionMat.m01 = -2f * plane[0] * plane[1];
    reflectionMat.m02 = -2f * plane[0] * plane[2];
    reflectionMat.m03 = -2f * plane[3] * plane[0];
    reflectionMat.m10 = -2f * plane[1] * plane[0];
    reflectionMat.m11 = (float) (1.0 - 2.0 * (double) plane[1] * (double) plane[1]);
    reflectionMat.m12 = -2f * plane[1] * plane[2];
    reflectionMat.m13 = -2f * plane[3] * plane[1];
    reflectionMat.m20 = -2f * plane[2] * plane[0];
    reflectionMat.m21 = -2f * plane[2] * plane[1];
    reflectionMat.m22 = (float) (1.0 - 2.0 * (double) plane[2] * (double) plane[2]);
    reflectionMat.m23 = -2f * plane[3] * plane[2];
    reflectionMat.m30 = 0.0f;
    reflectionMat.m31 = 0.0f;
    reflectionMat.m32 = 0.0f;
    reflectionMat.m33 = 1f;
    return reflectionMat;
  }

  private static float sgn(float a)
  {
    if ((double) a > 0.0)
      return 1f;
    return (double) a < 0.0 ? -1f : 0.0f;
  }

  private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
  {
    Vector3 v = pos + normal * this.clipPlaneOffset;
    Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
    Vector3 lhs = worldToCameraMatrix.MultiplyPoint(v);
    Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
    return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
  }
}
