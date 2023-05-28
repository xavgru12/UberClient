// Decompiled with JetBrains decompiler
// Type: Bloom
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Bloom (4.0, HDR, Lens Flares)")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[Serializable]
public class Bloom : PostEffectsBase
{
  public Bloom.TweakMode tweakMode;
  public Bloom.BloomScreenBlendMode screenBlendMode;
  public Bloom.HDRBloomMode hdr;
  private bool doHdr;
  public float sepBlurSpread;
  public Bloom.BloomQuality quality;
  public float bloomIntensity;
  public float bloomThreshhold;
  public Color bloomThreshholdColor;
  public int bloomBlurIterations;
  public int hollywoodFlareBlurIterations;
  public float flareRotation;
  public Bloom.LensFlareStyle lensflareMode;
  public float hollyStretchWidth;
  public float lensflareIntensity;
  public float lensflareThreshhold;
  public float lensFlareSaturation;
  public Color flareColorA;
  public Color flareColorB;
  public Color flareColorC;
  public Color flareColorD;
  public float blurWidth;
  public Texture2D lensFlareVignetteMask;
  public Shader lensFlareShader;
  private Material lensFlareMaterial;
  public Shader screenBlendShader;
  private Material screenBlend;
  public Shader blurAndFlaresShader;
  private Material blurAndFlaresMaterial;
  public Shader brightPassFilterShader;
  private Material brightPassFilterMaterial;

  public Bloom()
  {
    this.screenBlendMode = Bloom.BloomScreenBlendMode.Add;
    this.hdr = Bloom.HDRBloomMode.Auto;
    this.sepBlurSpread = 2.5f;
    this.quality = Bloom.BloomQuality.High;
    this.bloomIntensity = 0.5f;
    this.bloomThreshhold = 0.5f;
    this.bloomThreshholdColor = Color.white;
    this.bloomBlurIterations = 2;
    this.hollywoodFlareBlurIterations = 2;
    this.lensflareMode = Bloom.LensFlareStyle.Anamorphic;
    this.hollyStretchWidth = 2.5f;
    this.lensflareThreshhold = 0.3f;
    this.lensFlareSaturation = 0.75f;
    this.flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);
    this.flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);
    this.flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);
    this.flareColorD = new Color(0.8f, 0.4f, 0.0f, 0.75f);
    this.blurWidth = 1f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.screenBlend = this.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
    this.lensFlareMaterial = this.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
    this.blurAndFlaresMaterial = this.CheckShaderAndCreateMaterial(this.blurAndFlaresShader, this.blurAndFlaresMaterial);
    this.brightPassFilterMaterial = this.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
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
      this.doHdr = false;
      if (this.hdr == Bloom.HDRBloomMode.Auto)
      {
        int num = source.format == RenderTextureFormat.ARGBHalf ? 1 : 0;
        if (num != 0)
          num = this.camera.hdr ? 1 : 0;
        this.doHdr = num != 0;
      }
      else
        this.doHdr = this.hdr == Bloom.HDRBloomMode.On;
      int num1 = this.doHdr ? 1 : 0;
      if (num1 != 0)
        num1 = this.supportHDRTextures ? 1 : 0;
      this.doHdr = num1 != 0;
      Bloom.BloomScreenBlendMode bloomScreenBlendMode = this.screenBlendMode;
      if (this.doHdr)
        bloomScreenBlendMode = Bloom.BloomScreenBlendMode.Add;
      RenderTextureFormat format = !this.doHdr ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf;
      RenderTexture temporary1 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, format);
      RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
      RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
      RenderTexture temporary4 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
      float num2 = (float) (1.0 * (double) source.width / (1.0 * (double) source.height));
      float num3 = 1f / 512f;
      if (this.quality > Bloom.BloomQuality.Cheap)
      {
        Graphics.Blit((Texture) source, temporary1, this.screenBlend, 2);
        Graphics.Blit((Texture) temporary1, temporary3, this.screenBlend, 2);
        Graphics.Blit((Texture) temporary3, temporary2, this.screenBlend, 6);
      }
      else
      {
        Graphics.Blit((Texture) source, temporary1);
        Graphics.Blit((Texture) temporary1, temporary2, this.screenBlend, 6);
      }
      this.BrightFilter(this.bloomThreshhold * this.bloomThreshholdColor, temporary2, temporary3);
      if (this.bloomBlurIterations < 1)
        this.bloomBlurIterations = 1;
      else if (this.bloomBlurIterations > 10)
        this.bloomBlurIterations = 10;
      for (int index = 0; index < this.bloomBlurIterations; ++index)
      {
        float num4 = (float) (1.0 + (double) index * 0.25) * this.sepBlurSpread;
        this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0.0f, num4 * num3, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary3, temporary4, this.blurAndFlaresMaterial, 4);
        if (this.quality > Bloom.BloomQuality.Cheap)
        {
          this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num4 / num2 * num3, 0.0f, 0.0f, 0.0f));
          Graphics.Blit((Texture) temporary4, temporary3, this.blurAndFlaresMaterial, 4);
          if (index == 0)
            Graphics.Blit((Texture) temporary3, temporary2);
          else
            Graphics.Blit((Texture) temporary3, temporary2, this.screenBlend, 10);
        }
        else
        {
          this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num4 / num2 * num3, 0.0f, 0.0f, 0.0f));
          Graphics.Blit((Texture) temporary4, temporary3, this.blurAndFlaresMaterial, 4);
        }
      }
      if (this.quality > Bloom.BloomQuality.Cheap)
        Graphics.Blit((Texture) temporary2, temporary3, this.screenBlend, 6);
      if ((double) this.lensflareIntensity > 1.4012984643248171E-45)
      {
        if (this.lensflareMode == Bloom.LensFlareStyle.Ghosting)
        {
          this.BrightFilter(this.lensflareThreshhold, temporary3, temporary4);
          if (this.quality > Bloom.BloomQuality.Cheap)
          {
            this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0.0f, (float) (1.5 / (1.0 * (double) temporary2.height)), 0.0f, 0.0f));
            Graphics.Blit((Texture) temporary4, temporary2, this.blurAndFlaresMaterial, 4);
            this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4((float) (1.5 / (1.0 * (double) temporary2.width)), 0.0f, 0.0f, 0.0f));
            Graphics.Blit((Texture) temporary2, temporary4, this.blurAndFlaresMaterial, 4);
          }
          this.Vignette(0.975f, temporary4, temporary4);
          this.BlendFlares(temporary4, temporary3);
        }
        else
        {
          float x = 1f * Mathf.Cos(this.flareRotation);
          float y = 1f * Mathf.Sin(this.flareRotation);
          float num5 = this.hollyStretchWidth * 1f / num2 * num3;
          float num6 = this.hollyStretchWidth * num3;
          this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(x, y, 0.0f, 0.0f));
          this.blurAndFlaresMaterial.SetVector("_Threshhold", new Vector4(this.lensflareThreshhold, 1f, 0.0f, 0.0f));
          this.blurAndFlaresMaterial.SetVector("_TintColor", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a * this.lensflareIntensity);
          this.blurAndFlaresMaterial.SetFloat("_Saturation", this.lensFlareSaturation);
          Graphics.Blit((Texture) temporary4, temporary2, this.blurAndFlaresMaterial, 2);
          Graphics.Blit((Texture) temporary2, temporary4, this.blurAndFlaresMaterial, 3);
          this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(x * num5, y * num5, 0.0f, 0.0f));
          this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth);
          Graphics.Blit((Texture) temporary4, temporary2, this.blurAndFlaresMaterial, 1);
          this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 2f);
          Graphics.Blit((Texture) temporary2, temporary4, this.blurAndFlaresMaterial, 1);
          this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 4f);
          Graphics.Blit((Texture) temporary4, temporary2, this.blurAndFlaresMaterial, 1);
          for (int index = 0; index < this.hollywoodFlareBlurIterations; ++index)
          {
            float num7 = this.hollyStretchWidth * 2f / num2 * num3;
            this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num7 * x, num7 * y, 0.0f, 0.0f));
            Graphics.Blit((Texture) temporary2, temporary4, this.blurAndFlaresMaterial, 4);
            this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num7 * x, num7 * y, 0.0f, 0.0f));
            Graphics.Blit((Texture) temporary4, temporary2, this.blurAndFlaresMaterial, 4);
          }
          if (this.lensflareMode == Bloom.LensFlareStyle.Anamorphic)
          {
            this.AddTo(1f, temporary2, temporary3);
          }
          else
          {
            this.Vignette(1f, temporary2, temporary4);
            this.BlendFlares(temporary4, temporary2);
            this.AddTo(1f, temporary2, temporary3);
          }
        }
      }
      int pass = (int) bloomScreenBlendMode;
      this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
      this.screenBlend.SetTexture("_ColorBuffer", (Texture) source);
      if (this.quality > Bloom.BloomQuality.Cheap)
      {
        Graphics.Blit((Texture) temporary3, temporary1);
        Graphics.Blit((Texture) temporary1, destination, this.screenBlend, pass);
      }
      else
        Graphics.Blit((Texture) temporary3, destination, this.screenBlend, pass);
      RenderTexture.ReleaseTemporary(temporary1);
      RenderTexture.ReleaseTemporary(temporary2);
      RenderTexture.ReleaseTemporary(temporary3);
      RenderTexture.ReleaseTemporary(temporary4);
    }
  }

  private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
  {
    this.screenBlend.SetFloat("_Intensity", intensity_);
    Graphics.Blit((Texture) from, to, this.screenBlend, 9);
  }

  private void BlendFlares(RenderTexture from, RenderTexture to)
  {
    this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
    this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
    this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
    this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
    Graphics.Blit((Texture) from, to, this.lensFlareMaterial);
  }

  private void BrightFilter(float thresh, RenderTexture from, RenderTexture to)
  {
    this.brightPassFilterMaterial.SetVector("_Threshhold", new Vector4(thresh, thresh, thresh, thresh));
    Graphics.Blit((Texture) from, to, this.brightPassFilterMaterial, 0);
  }

  private void BrightFilter(Color threshColor, RenderTexture from, RenderTexture to)
  {
    this.brightPassFilterMaterial.SetVector("_Threshhold", (Vector4) threshColor);
    Graphics.Blit((Texture) from, to, this.brightPassFilterMaterial, 1);
  }

  private void Vignette(float amount, RenderTexture from, RenderTexture to)
  {
    if ((bool) (UnityEngine.Object) this.lensFlareVignetteMask)
    {
      this.screenBlend.SetTexture("_ColorBuffer", (Texture) this.lensFlareVignetteMask);
      Graphics.Blit(!((UnityEngine.Object) from == (UnityEngine.Object) to) ? (Texture) from : (Texture) null, to, this.screenBlend, !((UnityEngine.Object) from == (UnityEngine.Object) to) ? 3 : 7);
    }
    else
    {
      if (!((UnityEngine.Object) from != (UnityEngine.Object) to))
        return;
      Graphics.Blit((Texture) from, to);
    }
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum LensFlareStyle
  {
    Ghosting,
    Anamorphic,
    Combined,
  }

  [Serializable]
  public enum TweakMode
  {
    Basic,
    Complex,
  }

  [Serializable]
  public enum HDRBloomMode
  {
    Auto,
    On,
    Off,
  }

  [Serializable]
  public enum BloomScreenBlendMode
  {
    Screen,
    Add,
  }

  [Serializable]
  public enum BloomQuality
  {
    Cheap,
    High,
  }
}
