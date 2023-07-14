// Decompiled with JetBrains decompiler
// Type: GlobalFog
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Global Fog")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[Serializable]
public class GlobalFog : PostEffectsBase
{
  public GlobalFog.FogMode fogMode;
  private float CAMERA_NEAR;
  private float CAMERA_FAR;
  private float CAMERA_FOV;
  private float CAMERA_ASPECT_RATIO;
  public float startDistance;
  public float globalDensity;
  public float heightScale;
  public float height;
  public Color globalFogColor;
  public Shader fogShader;
  private Material fogMaterial;

  public GlobalFog()
  {
    this.fogMode = GlobalFog.FogMode.AbsoluteYAndDistance;
    this.CAMERA_NEAR = 0.5f;
    this.CAMERA_FAR = 50f;
    this.CAMERA_FOV = 60f;
    this.CAMERA_ASPECT_RATIO = 1.333333f;
    this.startDistance = 200f;
    this.globalDensity = 1f;
    this.heightScale = 100f;
    this.globalFogColor = Color.grey;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.fogMaterial = this.CheckShaderAndCreateMaterial(this.fogShader, this.fogMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      this.CAMERA_NEAR = this.camera.nearClipPlane;
      this.CAMERA_FAR = this.camera.farClipPlane;
      this.CAMERA_FOV = this.camera.fieldOfView;
      this.CAMERA_ASPECT_RATIO = this.camera.aspect;
      Matrix4x4 identity = Matrix4x4.identity;
      Vector4 vector4 = new Vector4();
      Vector3 vector3_1 = new Vector3();
      float num1 = this.CAMERA_FOV * 0.5f;
      Vector3 vector3_2 = this.camera.transform.right * this.CAMERA_NEAR * Mathf.Tan(num1 * ((float) Math.PI / 180f)) * this.CAMERA_ASPECT_RATIO;
      Vector3 vector3_3 = this.camera.transform.up * this.CAMERA_NEAR * Mathf.Tan(num1 * ((float) Math.PI / 180f));
      Vector3 vector3_4 = this.camera.transform.forward * this.CAMERA_NEAR - vector3_2 + vector3_3;
      float num2 = vector3_4.magnitude * this.CAMERA_FAR / this.CAMERA_NEAR;
      vector3_4.Normalize();
      Vector3 v1 = vector3_4 * num2;
      Vector3 vector3_5 = this.camera.transform.forward * this.CAMERA_NEAR + vector3_2 + vector3_3;
      vector3_5.Normalize();
      Vector3 v2 = vector3_5 * num2;
      Vector3 vector3_6 = this.camera.transform.forward * this.CAMERA_NEAR + vector3_2 - vector3_3;
      vector3_6.Normalize();
      Vector3 v3 = vector3_6 * num2;
      Vector3 vector3_7 = this.camera.transform.forward * this.CAMERA_NEAR - vector3_2 - vector3_3;
      vector3_7.Normalize();
      Vector3 v4 = vector3_7 * num2;
      identity.SetRow(0, (Vector4) v1);
      identity.SetRow(1, (Vector4) v2);
      identity.SetRow(2, (Vector4) v3);
      identity.SetRow(3, (Vector4) v4);
      this.fogMaterial.SetMatrix("_FrustumCornersWS", identity);
      this.fogMaterial.SetVector("_CameraWS", (Vector4) this.camera.transform.position);
      this.fogMaterial.SetVector("_StartDistance", new Vector4(1f / this.startDistance, num2 - this.startDistance));
      this.fogMaterial.SetVector("_Y", new Vector4(this.height, 1f / this.heightScale));
      this.fogMaterial.SetFloat("_GlobalDensity", this.globalDensity * 0.01f);
      this.fogMaterial.SetColor("_FogColor", this.globalFogColor);
      GlobalFog.CustomGraphicsBlit(source, destination, this.fogMaterial, (int) this.fogMode);
    }
  }

  public static void CustomGraphicsBlit(
    RenderTexture source,
    RenderTexture dest,
    Material fxMaterial,
    int passNr)
  {
    RenderTexture.active = dest;
    fxMaterial.SetTexture("_MainTex", (Texture) source);
    GL.PushMatrix();
    GL.LoadOrtho();
    fxMaterial.SetPass(passNr);
    GL.Begin(7);
    GL.MultiTexCoord2(0, 0.0f, 0.0f);
    GL.Vertex3(0.0f, 0.0f, 3f);
    GL.MultiTexCoord2(0, 1f, 0.0f);
    GL.Vertex3(1f, 0.0f, 2f);
    GL.MultiTexCoord2(0, 1f, 1f);
    GL.Vertex3(1f, 1f, 1f);
    GL.MultiTexCoord2(0, 0.0f, 1f);
    GL.Vertex3(0.0f, 1f, 0.0f);
    GL.End();
    GL.PopMatrix();
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum FogMode
  {
    AbsoluteYAndDistance,
    AbsoluteY,
    Distance,
    RelativeYAndDistance,
  }
}
