// Decompiled with JetBrains decompiler
// Type: Vignetting
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Vignette and Chromatic Aberration")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[Serializable]
public class Vignetting : PostEffectsBase
{
  public Vignetting.AberrationMode mode;
  public float intensity;
  public float chromaticAberration;
  public float axialAberration;
  public float blur;
  public float blurSpread;
  public float luminanceDependency;
  public Shader vignetteShader;
  private Material vignetteMaterial;
  public Shader separableBlurShader;
  private Material separableBlurMaterial;
  public Shader chromAberrationShader;
  private Material chromAberrationMaterial;

  public Vignetting()
  {
    this.mode = Vignetting.AberrationMode.Simple;
    this.intensity = 0.375f;
    this.chromaticAberration = 0.2f;
    this.axialAberration = 0.5f;
    this.blurSpread = 0.75f;
    this.luminanceDependency = 0.25f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.vignetteMaterial = this.CheckShaderAndCreateMaterial(this.vignetteShader, this.vignetteMaterial);
    this.separableBlurMaterial = this.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
    this.chromAberrationMaterial = this.CheckShaderAndCreateMaterial(this.chromAberrationShader, this.chromAberrationMaterial);
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
      int num1 = (double) Mathf.Abs(this.blur) > 0.0 ? 1 : 0;
      if (num1 == 0)
        num1 = (double) Mathf.Abs(this.intensity) > 0.0 ? 1 : 0;
      bool flag = num1 != 0;
      float num2 = (float) (1.0 * (double) source.width / (1.0 * (double) source.height));
      float num3 = 1f / 512f;
      RenderTexture renderTexture1 = (RenderTexture) null;
      RenderTexture renderTexture2 = (RenderTexture) null;
      RenderTexture renderTexture3 = (RenderTexture) null;
      if (flag)
      {
        renderTexture1 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
        if ((double) Mathf.Abs(this.blur) > 0.0)
        {
          renderTexture2 = RenderTexture.GetTemporary((int) ((double) source.width / 2.0), (int) ((double) source.height / 2.0), 0, source.format);
          renderTexture3 = RenderTexture.GetTemporary((int) ((double) source.width / 2.0), (int) ((double) source.height / 2.0), 0, source.format);
          Graphics.Blit((Texture) source, renderTexture2, this.chromAberrationMaterial, 0);
          for (int index = 0; index < 2; ++index)
          {
            this.separableBlurMaterial.SetVector("offsets", new Vector4(0.0f, this.blurSpread * num3, 0.0f, 0.0f));
            Graphics.Blit((Texture) renderTexture2, renderTexture3, this.separableBlurMaterial);
            this.separableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * num3 / num2, 0.0f, 0.0f, 0.0f));
            Graphics.Blit((Texture) renderTexture3, renderTexture2, this.separableBlurMaterial);
          }
        }
        this.vignetteMaterial.SetFloat("_Intensity", this.intensity);
        this.vignetteMaterial.SetFloat("_Blur", this.blur);
        this.vignetteMaterial.SetTexture("_VignetteTex", (Texture) renderTexture2);
        Graphics.Blit((Texture) source, renderTexture1, this.vignetteMaterial, 0);
      }
      this.chromAberrationMaterial.SetFloat("_ChromaticAberration", this.chromaticAberration);
      this.chromAberrationMaterial.SetFloat("_AxialAberration", this.axialAberration);
      this.chromAberrationMaterial.SetFloat("_Luminance", (float) (1.0 / (1.4012984643248171E-45 + (double) this.luminanceDependency)));
      if (flag)
        renderTexture1.wrapMode = TextureWrapMode.Clamp;
      else
        source.wrapMode = TextureWrapMode.Clamp;
      Graphics.Blit(!flag ? (Texture) source : (Texture) renderTexture1, destination, this.chromAberrationMaterial, this.mode != Vignetting.AberrationMode.Advanced ? 1 : 2);
      if ((bool) (UnityEngine.Object) renderTexture1)
        RenderTexture.ReleaseTemporary(renderTexture1);
      if ((bool) (UnityEngine.Object) renderTexture2)
        RenderTexture.ReleaseTemporary(renderTexture2);
      if (!(bool) (UnityEngine.Object) renderTexture3)
        return;
      RenderTexture.ReleaseTemporary(renderTexture3);
    }
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum AberrationMode
  {
    Simple,
    Advanced,
  }
}
