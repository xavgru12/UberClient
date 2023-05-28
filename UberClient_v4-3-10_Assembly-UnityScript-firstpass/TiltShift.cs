// Decompiled with JetBrains decompiler
// Type: TiltShift
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Tilt shift")]
[Serializable]
public class TiltShift : PostEffectsBase
{
  public Shader tiltShiftShader;
  private Material tiltShiftMaterial;
  public int renderTextureDivider;
  public int blurIterations;
  public bool enableForegroundBlur;
  public int foregroundBlurIterations;
  public float maxBlurSpread;
  public float focalPoint;
  public float smoothness;
  public bool visualizeCoc;
  private float start01;
  private float distance01;
  private float end01;
  private float curve;

  public TiltShift()
  {
    this.renderTextureDivider = 2;
    this.blurIterations = 2;
    this.enableForegroundBlur = true;
    this.foregroundBlurIterations = 2;
    this.maxBlurSpread = 1.5f;
    this.focalPoint = 30f;
    this.smoothness = 1.65f;
    this.distance01 = 0.2f;
    this.end01 = 1f;
    this.curve = 1f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.tiltShiftMaterial = this.CheckShaderAndCreateMaterial(this.tiltShiftShader, this.tiltShiftMaterial);
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
      float num1 = (float) (1.0 * (double) source.width / (1.0 * (double) source.height));
      float num2 = 1f / 512f;
      this.renderTextureDivider = this.renderTextureDivider >= 1 ? this.renderTextureDivider : 1;
      this.renderTextureDivider = this.renderTextureDivider <= 4 ? this.renderTextureDivider : 4;
      this.blurIterations = this.blurIterations >= 1 ? this.blurIterations : 0;
      this.blurIterations = this.blurIterations <= 4 ? this.blurIterations : 4;
      float num3 = this.camera.WorldToViewportPoint(this.focalPoint * this.camera.transform.forward + this.camera.transform.position).z / this.camera.farClipPlane;
      this.distance01 = num3;
      this.start01 = 0.0f;
      this.end01 = 1f;
      this.start01 = Mathf.Min(num3 - float.Epsilon, this.start01);
      this.end01 = Mathf.Max(num3 + float.Epsilon, this.end01);
      this.curve = this.smoothness * this.distance01;
      RenderTexture temporary1 = RenderTexture.GetTemporary(source.width, source.height, 0);
      RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
      RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / this.renderTextureDivider, source.height / this.renderTextureDivider, 0);
      RenderTexture temporary4 = RenderTexture.GetTemporary(source.width / this.renderTextureDivider, source.height / this.renderTextureDivider, 0);
      this.tiltShiftMaterial.SetVector("_SimpleDofParams", new Vector4(this.start01, this.distance01, this.end01, this.curve));
      this.tiltShiftMaterial.SetTexture("_Coc", (Texture) temporary1);
      if (this.enableForegroundBlur)
      {
        Graphics.Blit((Texture) source, temporary1, this.tiltShiftMaterial, 0);
        Graphics.Blit((Texture) temporary1, temporary3);
        for (int index = 0; index < this.foregroundBlurIterations; ++index)
        {
          this.tiltShiftMaterial.SetVector("offsets", new Vector4(0.0f, this.maxBlurSpread * 0.75f * num2, 0.0f, 0.0f));
          Graphics.Blit((Texture) temporary3, temporary4, this.tiltShiftMaterial, 3);
          this.tiltShiftMaterial.SetVector("offsets", new Vector4(this.maxBlurSpread * 0.75f / num1 * num2, 0.0f, 0.0f, 0.0f));
          Graphics.Blit((Texture) temporary4, temporary3, this.tiltShiftMaterial, 3);
        }
        Graphics.Blit((Texture) temporary3, temporary2, this.tiltShiftMaterial, 7);
        this.tiltShiftMaterial.SetTexture("_Coc", (Texture) temporary2);
      }
      else
      {
        RenderTexture.active = temporary1;
        GL.Clear(false, true, Color.black);
      }
      Graphics.Blit((Texture) source, temporary1, this.tiltShiftMaterial, 5);
      this.tiltShiftMaterial.SetTexture("_Coc", (Texture) temporary1);
      Graphics.Blit((Texture) source, temporary4);
      for (int index = 0; index < this.blurIterations; ++index)
      {
        this.tiltShiftMaterial.SetVector("offsets", new Vector4(0.0f, this.maxBlurSpread * 1f * num2, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary4, temporary3, this.tiltShiftMaterial, 6);
        this.tiltShiftMaterial.SetVector("offsets", new Vector4(this.maxBlurSpread * 1f / num1 * num2, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary3, temporary4, this.tiltShiftMaterial, 6);
      }
      this.tiltShiftMaterial.SetTexture("_Blurred", (Texture) temporary4);
      Graphics.Blit((Texture) source, destination, this.tiltShiftMaterial, !this.visualizeCoc ? 1 : 4);
      RenderTexture.ReleaseTemporary(temporary1);
      RenderTexture.ReleaseTemporary(temporary2);
      RenderTexture.ReleaseTemporary(temporary3);
      RenderTexture.ReleaseTemporary(temporary4);
    }
  }

  public override void Main()
  {
  }
}
