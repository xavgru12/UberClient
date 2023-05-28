// Decompiled with JetBrains decompiler
// Type: SunShafts
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Sun Shafts")]
[Serializable]
public class SunShafts : PostEffectsBase
{
  public SunShaftsResolution resolution;
  public ShaftsScreenBlendMode screenBlendMode;
  public Transform sunTransform;
  public int radialBlurIterations;
  public Color sunColor;
  public float sunShaftBlurRadius;
  public float sunShaftIntensity;
  public float useSkyBoxAlpha;
  public float maxRadius;
  public bool useDepthTexture;
  public Shader sunShaftsShader;
  private Material sunShaftsMaterial;
  public Shader simpleClearShader;
  private Material simpleClearMaterial;

  public SunShafts()
  {
    this.resolution = SunShaftsResolution.Normal;
    this.screenBlendMode = ShaftsScreenBlendMode.Screen;
    this.radialBlurIterations = 2;
    this.sunColor = Color.white;
    this.sunShaftBlurRadius = 2.5f;
    this.sunShaftIntensity = 1.15f;
    this.useSkyBoxAlpha = 0.75f;
    this.maxRadius = 0.75f;
    this.useDepthTexture = true;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(this.useDepthTexture);
    this.sunShaftsMaterial = this.CheckShaderAndCreateMaterial(this.sunShaftsShader, this.sunShaftsMaterial);
    this.simpleClearMaterial = this.CheckShaderAndCreateMaterial(this.simpleClearShader, this.simpleClearMaterial);
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
      if (this.useDepthTexture)
        this.camera.depthTextureMode |= DepthTextureMode.Depth;
      float num1 = 4f;
      if (this.resolution == SunShaftsResolution.Normal)
        num1 = 2f;
      else if (this.resolution == SunShaftsResolution.High)
        num1 = 1f;
      Vector3 vector3_1 = Vector3.one * 0.5f;
      Vector3 vector3_2 = !(bool) (UnityEngine.Object) this.sunTransform ? new Vector3(0.5f, 0.5f, 0.0f) : this.camera.WorldToViewportPoint(this.sunTransform.position);
      RenderTexture temporary1 = RenderTexture.GetTemporary((int) ((double) source.width / (double) num1), (int) ((double) source.height / (double) num1), 0);
      RenderTexture temporary2 = RenderTexture.GetTemporary((int) ((double) source.width / (double) num1), (int) ((double) source.height / (double) num1), 0);
      this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0.0f, 0.0f) * this.sunShaftBlurRadius);
      this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector3_2.x, vector3_2.y, vector3_2.z, this.maxRadius));
      this.sunShaftsMaterial.SetFloat("_NoSkyBoxMask", 1f - this.useSkyBoxAlpha);
      if (!this.useDepthTexture)
      {
        RenderTexture temporary3 = RenderTexture.GetTemporary(source.width, source.height, 0);
        RenderTexture.active = temporary3;
        GL.ClearWithSkybox(false, this.camera);
        this.sunShaftsMaterial.SetTexture("_Skybox", (Texture) temporary3);
        Graphics.Blit((Texture) source, temporary2, this.sunShaftsMaterial, 3);
        RenderTexture.ReleaseTemporary(temporary3);
      }
      else
        Graphics.Blit((Texture) source, temporary2, this.sunShaftsMaterial, 2);
      this.DrawBorder(temporary2, this.simpleClearMaterial);
      this.radialBlurIterations = this.ClampBlurIterationsToSomethingThatMakesSense(this.radialBlurIterations);
      float num2 = this.sunShaftBlurRadius * (1f / 768f);
      this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0.0f, 0.0f));
      this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector3_2.x, vector3_2.y, vector3_2.z, this.maxRadius));
      for (int index = 0; index < this.radialBlurIterations; ++index)
      {
        Graphics.Blit((Texture) temporary2, temporary1, this.sunShaftsMaterial, 1);
        float num3 = (float) ((double) this.sunShaftBlurRadius * (((double) index * 2.0 + 1.0) * 6.0) / 768.0);
        this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num3, num3, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary1, temporary2, this.sunShaftsMaterial, 1);
        float num4 = (float) ((double) this.sunShaftBlurRadius * (((double) index * 2.0 + 2.0) * 6.0) / 768.0);
        this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num4, num4, 0.0f, 0.0f));
      }
      if ((double) vector3_2.z >= 0.0)
        this.sunShaftsMaterial.SetVector("_SunColor", new Vector4(this.sunColor.r, this.sunColor.g, this.sunColor.b, this.sunColor.a) * this.sunShaftIntensity);
      else
        this.sunShaftsMaterial.SetVector("_SunColor", Vector4.zero);
      this.sunShaftsMaterial.SetTexture("_ColorBuffer", (Texture) temporary2);
      Graphics.Blit((Texture) source, destination, this.sunShaftsMaterial, this.screenBlendMode != ShaftsScreenBlendMode.Screen ? 4 : 0);
      RenderTexture.ReleaseTemporary(temporary2);
      RenderTexture.ReleaseTemporary(temporary1);
    }
  }

  private int ClampBlurIterationsToSomethingThatMakesSense(int its)
  {
    if (its < 1)
      return 1;
    return its > 4 ? 4 : its;
  }

  public override void Main()
  {
  }
}
