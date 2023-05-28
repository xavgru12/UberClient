// Decompiled with JetBrains decompiler
// Type: DepthOfField34
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Depth of Field (3.4)")]
[Serializable]
public class DepthOfField34 : PostEffectsBase
{
  [NonSerialized]
  private static int SMOOTH_DOWNSAMPLE_PASS = 6;
  [NonSerialized]
  private static float BOKEH_EXTRA_BLUR = 2f;
  public Dof34QualitySetting quality;
  public DofResolution resolution;
  public bool simpleTweakMode;
  public float focalPoint;
  public float smoothness;
  public float focalZDistance;
  public float focalZStartCurve;
  public float focalZEndCurve;
  private float focalStartCurve;
  private float focalEndCurve;
  private float focalDistance01;
  public Transform objectFocus;
  public float focalSize;
  public DofBlurriness bluriness;
  public float maxBlurSpread;
  public float foregroundBlurExtrude;
  public Shader dofBlurShader;
  private Material dofBlurMaterial;
  public Shader dofShader;
  private Material dofMaterial;
  public bool visualize;
  public BokehDestination bokehDestination;
  private float widthOverHeight;
  private float oneOverBaseSize;
  public bool bokeh;
  public bool bokehSupport;
  public Shader bokehShader;
  public Texture2D bokehTexture;
  public float bokehScale;
  public float bokehIntensity;
  public float bokehThreshholdContrast;
  public float bokehThreshholdLuminance;
  public int bokehDownsample;
  private Material bokehMaterial;
  private RenderTexture foregroundTexture;
  private RenderTexture mediumRezWorkTexture;
  private RenderTexture finalDefocus;
  private RenderTexture lowRezWorkTexture;
  private RenderTexture bokehSource;
  private RenderTexture bokehSource2;

  public DepthOfField34()
  {
    this.quality = Dof34QualitySetting.OnlyBackground;
    this.resolution = DofResolution.Low;
    this.simpleTweakMode = true;
    this.focalPoint = 1f;
    this.smoothness = 0.5f;
    this.focalZStartCurve = 1f;
    this.focalZEndCurve = 1f;
    this.focalStartCurve = 2f;
    this.focalEndCurve = 2f;
    this.focalDistance01 = 0.1f;
    this.bluriness = DofBlurriness.High;
    this.maxBlurSpread = 1.75f;
    this.foregroundBlurExtrude = 1.15f;
    this.bokehDestination = BokehDestination.Background;
    this.widthOverHeight = 1.25f;
    this.oneOverBaseSize = 1f / 512f;
    this.bokehSupport = true;
    this.bokehScale = 2.4f;
    this.bokehIntensity = 0.15f;
    this.bokehThreshholdContrast = 0.1f;
    this.bokehThreshholdLuminance = 0.55f;
    this.bokehDownsample = 1;
  }

  public virtual void CreateMaterials()
  {
    this.dofBlurMaterial = this.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
    this.dofMaterial = this.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
    this.bokehSupport = this.bokehShader.isSupported;
    if (!this.bokeh || !this.bokehSupport || !(bool) (UnityEngine.Object) this.bokehShader)
      return;
    this.bokehMaterial = this.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.dofBlurMaterial = this.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
    this.dofMaterial = this.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
    this.bokehSupport = this.bokehShader.isSupported;
    if (this.bokeh && this.bokehSupport && (bool) (UnityEngine.Object) this.bokehShader)
      this.bokehMaterial = this.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public virtual void OnDisable() => Quads.Cleanup();

  public override void OnEnable() => this.camera.depthTextureMode |= DepthTextureMode.Depth;

  public virtual float FocalDistance01(float worldDist) => this.camera.WorldToViewportPoint((worldDist - this.camera.nearClipPlane) * this.camera.transform.forward + this.camera.transform.position).z / (this.camera.farClipPlane - this.camera.nearClipPlane);

  public virtual int GetDividerBasedOnQuality()
  {
    int dividerBasedOnQuality = 1;
    if (this.resolution == DofResolution.Medium)
      dividerBasedOnQuality = 2;
    else if (this.resolution == DofResolution.Low)
      dividerBasedOnQuality = 2;
    return dividerBasedOnQuality;
  }

  public virtual int GetLowResolutionDividerBasedOnQuality(int baseDivider)
  {
    int dividerBasedOnQuality = baseDivider;
    if (this.resolution == DofResolution.High)
      dividerBasedOnQuality *= 2;
    if (this.resolution == DofResolution.Low)
      dividerBasedOnQuality *= 2;
    return dividerBasedOnQuality;
  }

  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if ((double) this.smoothness < 0.10000000149011612)
        this.smoothness = 0.1f;
      int num1 = this.bokeh ? 1 : 0;
      if (num1 != 0)
        num1 = this.bokehSupport ? 1 : 0;
      this.bokeh = num1 != 0;
      float num2 = !this.bokeh ? 1f : DepthOfField34.BOKEH_EXTRA_BLUR;
      bool flag = this.quality > Dof34QualitySetting.OnlyBackground;
      float num3 = this.focalSize / (this.camera.farClipPlane - this.camera.nearClipPlane);
      bool blurForeground;
      if (this.simpleTweakMode)
      {
        this.focalDistance01 = !(bool) (UnityEngine.Object) this.objectFocus ? this.FocalDistance01(this.focalPoint) : this.camera.WorldToViewportPoint(this.objectFocus.position).z / this.camera.farClipPlane;
        this.focalStartCurve = this.focalDistance01 * this.smoothness;
        this.focalEndCurve = this.focalStartCurve;
        int num4 = flag ? 1 : 0;
        if (num4 != 0)
          num4 = (double) this.focalPoint > (double) this.camera.nearClipPlane + 1.4012984643248171E-45 ? 1 : 0;
        blurForeground = num4 != 0;
      }
      else
      {
        if ((bool) (UnityEngine.Object) this.objectFocus)
        {
          Vector3 viewportPoint = this.camera.WorldToViewportPoint(this.objectFocus.position);
          viewportPoint.z /= this.camera.farClipPlane;
          this.focalDistance01 = viewportPoint.z;
        }
        else
          this.focalDistance01 = this.FocalDistance01(this.focalZDistance);
        this.focalStartCurve = this.focalZStartCurve;
        this.focalEndCurve = this.focalZEndCurve;
        int num5 = flag ? 1 : 0;
        if (num5 != 0)
          num5 = (double) this.focalPoint > (double) this.camera.nearClipPlane + 1.4012984643248171E-45 ? 1 : 0;
        blurForeground = num5 != 0;
      }
      this.widthOverHeight = (float) (1.0 * (double) source.width / (1.0 * (double) source.height));
      this.oneOverBaseSize = 1f / 512f;
      this.dofMaterial.SetFloat("_ForegroundBlurExtrude", this.foregroundBlurExtrude);
      this.dofMaterial.SetVector("_CurveParams", new Vector4(!this.simpleTweakMode ? this.focalStartCurve : 1f / this.focalStartCurve, !this.simpleTweakMode ? this.focalEndCurve : 1f / this.focalEndCurve, num3 * 0.5f, this.focalDistance01));
      this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4((float) (1.0 / (1.0 * (double) source.width)), (float) (1.0 / (1.0 * (double) source.height)), 0.0f, 0.0f));
      int dividerBasedOnQuality1 = this.GetDividerBasedOnQuality();
      int dividerBasedOnQuality2 = this.GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality1);
      this.AllocateTextures(blurForeground, source, dividerBasedOnQuality1, dividerBasedOnQuality2);
      Graphics.Blit((Texture) source, source, this.dofMaterial, 3);
      this.Downsample(source, this.mediumRezWorkTexture);
      this.Blur(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DofBlurriness.Low, 4, this.maxBlurSpread);
      if (this.bokeh && (this.bokehDestination & BokehDestination.Background) != (BokehDestination) 0)
      {
        this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThreshholdContrast, this.bokehThreshholdLuminance, 0.95f, 0.0f));
        Graphics.Blit((Texture) this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
        Graphics.Blit((Texture) this.mediumRezWorkTexture, this.lowRezWorkTexture);
        this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread * num2);
      }
      else
      {
        this.Downsample(this.mediumRezWorkTexture, this.lowRezWorkTexture);
        this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread);
      }
      this.dofBlurMaterial.SetTexture("_TapLow", (Texture) this.lowRezWorkTexture);
      this.dofBlurMaterial.SetTexture("_TapMedium", (Texture) this.mediumRezWorkTexture);
      Graphics.Blit((Texture) null, this.finalDefocus, this.dofBlurMaterial, 3);
      if (this.bokeh && (this.bokehDestination & BokehDestination.Background) != (BokehDestination) 0)
        this.AddBokeh(this.bokehSource2, this.bokehSource, this.finalDefocus);
      this.dofMaterial.SetTexture("_TapLowBackground", (Texture) this.finalDefocus);
      this.dofMaterial.SetTexture("_TapMedium", (Texture) this.mediumRezWorkTexture);
      Graphics.Blit((Texture) source, !blurForeground ? destination : this.foregroundTexture, this.dofMaterial, !this.visualize ? 0 : 2);
      if (blurForeground)
      {
        Graphics.Blit((Texture) this.foregroundTexture, source, this.dofMaterial, 5);
        this.Downsample(source, this.mediumRezWorkTexture);
        this.BlurFg(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DofBlurriness.Low, 2, this.maxBlurSpread);
        if (this.bokeh && (this.bokehDestination & BokehDestination.Foreground) != (BokehDestination) 0)
        {
          this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThreshholdContrast * 0.5f, this.bokehThreshholdLuminance, 0.0f, 0.0f));
          Graphics.Blit((Texture) this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
          Graphics.Blit((Texture) this.mediumRezWorkTexture, this.lowRezWorkTexture);
          this.BlurFg(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread * num2);
        }
        else
          this.BlurFg(this.mediumRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread);
        Graphics.Blit((Texture) this.lowRezWorkTexture, this.finalDefocus);
        this.dofMaterial.SetTexture("_TapLowForeground", (Texture) this.finalDefocus);
        Graphics.Blit((Texture) source, destination, this.dofMaterial, !this.visualize ? 4 : 1);
        if (this.bokeh && (this.bokehDestination & BokehDestination.Foreground) != (BokehDestination) 0)
          this.AddBokeh(this.bokehSource2, this.bokehSource, destination);
      }
      this.ReleaseTextures();
    }
  }

  public virtual void Blur(
    RenderTexture from,
    RenderTexture to,
    DofBlurriness iterations,
    int blurPass,
    float spread)
  {
    RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
    if (iterations > DofBlurriness.Low)
    {
      this.BlurHex(from, to, blurPass, spread, temporary);
      if (iterations > DofBlurriness.High)
      {
        this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
        Graphics.Blit((Texture) to, temporary, this.dofBlurMaterial, blurPass);
        this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
      }
    }
    else
    {
      this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
      Graphics.Blit((Texture) from, temporary, this.dofBlurMaterial, blurPass);
      this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
      Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
    }
    RenderTexture.ReleaseTemporary(temporary);
  }

  public virtual void BlurFg(
    RenderTexture from,
    RenderTexture to,
    DofBlurriness iterations,
    int blurPass,
    float spread)
  {
    this.dofBlurMaterial.SetTexture("_TapHigh", (Texture) from);
    RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
    if (iterations > DofBlurriness.Low)
    {
      this.BlurHex(from, to, blurPass, spread, temporary);
      if (iterations > DofBlurriness.High)
      {
        this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
        Graphics.Blit((Texture) to, temporary, this.dofBlurMaterial, blurPass);
        this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
      }
    }
    else
    {
      this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
      Graphics.Blit((Texture) from, temporary, this.dofBlurMaterial, blurPass);
      this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
      Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
    }
    RenderTexture.ReleaseTemporary(temporary);
  }

  public virtual void BlurHex(
    RenderTexture from,
    RenderTexture to,
    int blurPass,
    float spread,
    RenderTexture tmp)
  {
    this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
    Graphics.Blit((Texture) from, tmp, this.dofBlurMaterial, blurPass);
    this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
    Graphics.Blit((Texture) tmp, to, this.dofBlurMaterial, blurPass);
    this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, spread * this.oneOverBaseSize, 0.0f, 0.0f));
    Graphics.Blit((Texture) to, tmp, this.dofBlurMaterial, blurPass);
    this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, -spread * this.oneOverBaseSize, 0.0f, 0.0f));
    Graphics.Blit((Texture) tmp, to, this.dofBlurMaterial, blurPass);
  }

  public virtual void Downsample(RenderTexture from, RenderTexture to)
  {
    this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4((float) (1.0 / (1.0 * (double) to.width)), (float) (1.0 / (1.0 * (double) to.height)), 0.0f, 0.0f));
    Graphics.Blit((Texture) from, to, this.dofMaterial, DepthOfField34.SMOOTH_DOWNSAMPLE_PASS);
  }

  public virtual void AddBokeh(
    RenderTexture bokehInfo,
    RenderTexture tempTex,
    RenderTexture finalTarget)
  {
    if (!(bool) (UnityEngine.Object) this.bokehMaterial)
      return;
    Mesh[] meshes = Quads.GetMeshes(tempTex.width, tempTex.height);
    RenderTexture.active = tempTex;
    GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    GL.PushMatrix();
    GL.LoadIdentity();
    bokehInfo.filterMode = FilterMode.Point;
    float num = (float) ((double) bokehInfo.width * 1.0 / ((double) bokehInfo.height * 1.0));
    float x = (float) (2.0 / (1.0 * (double) bokehInfo.width)) + this.bokehScale * this.maxBlurSpread * DepthOfField34.BOKEH_EXTRA_BLUR * this.oneOverBaseSize;
    this.bokehMaterial.SetTexture("_Source", (Texture) bokehInfo);
    this.bokehMaterial.SetTexture("_MainTex", (Texture) this.bokehTexture);
    this.bokehMaterial.SetVector("_ArScale", new Vector4(x, x * num, 0.5f, 0.5f * num));
    this.bokehMaterial.SetFloat("_Intensity", this.bokehIntensity);
    this.bokehMaterial.SetPass(0);
    int index = 0;
    Mesh[] meshArray = meshes;
    for (int length = meshArray.Length; index < length; ++index)
    {
      if ((bool) (UnityEngine.Object) meshArray[index])
        Graphics.DrawMeshNow(meshArray[index], Matrix4x4.identity);
    }
    GL.PopMatrix();
    Graphics.Blit((Texture) tempTex, finalTarget, this.dofMaterial, 8);
    bokehInfo.filterMode = FilterMode.Bilinear;
  }

  public virtual void ReleaseTextures()
  {
    if ((bool) (UnityEngine.Object) this.foregroundTexture)
      RenderTexture.ReleaseTemporary(this.foregroundTexture);
    if ((bool) (UnityEngine.Object) this.finalDefocus)
      RenderTexture.ReleaseTemporary(this.finalDefocus);
    if ((bool) (UnityEngine.Object) this.mediumRezWorkTexture)
      RenderTexture.ReleaseTemporary(this.mediumRezWorkTexture);
    if ((bool) (UnityEngine.Object) this.lowRezWorkTexture)
      RenderTexture.ReleaseTemporary(this.lowRezWorkTexture);
    if ((bool) (UnityEngine.Object) this.bokehSource)
      RenderTexture.ReleaseTemporary(this.bokehSource);
    if (!(bool) (UnityEngine.Object) this.bokehSource2)
      return;
    RenderTexture.ReleaseTemporary(this.bokehSource2);
  }

  public virtual void AllocateTextures(
    bool blurForeground,
    RenderTexture source,
    int divider,
    int lowTexDivider)
  {
    this.foregroundTexture = (RenderTexture) null;
    if (blurForeground)
      this.foregroundTexture = RenderTexture.GetTemporary(source.width, source.height, 0);
    this.mediumRezWorkTexture = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
    this.finalDefocus = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
    this.lowRezWorkTexture = RenderTexture.GetTemporary(source.width / lowTexDivider, source.height / lowTexDivider, 0);
    this.bokehSource = (RenderTexture) null;
    this.bokehSource2 = (RenderTexture) null;
    if (this.bokeh)
    {
      this.bokehSource = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
      this.bokehSource2 = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
      this.bokehSource.filterMode = FilterMode.Bilinear;
      this.bokehSource2.filterMode = FilterMode.Bilinear;
      RenderTexture.active = this.bokehSource2;
      GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    }
    source.filterMode = FilterMode.Bilinear;
    this.finalDefocus.filterMode = FilterMode.Bilinear;
    this.mediumRezWorkTexture.filterMode = FilterMode.Bilinear;
    this.lowRezWorkTexture.filterMode = FilterMode.Bilinear;
    if (!(bool) (UnityEngine.Object) this.foregroundTexture)
      return;
    this.foregroundTexture.filterMode = FilterMode.Bilinear;
  }

  public override void Main()
  {
  }
}
