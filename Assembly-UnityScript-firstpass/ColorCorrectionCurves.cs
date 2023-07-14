// Decompiled with JetBrains decompiler
// Type: ColorCorrectionCurves
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Correction (Curves, Saturation)")]
[Serializable]
public class ColorCorrectionCurves : PostEffectsBase
{
  public AnimationCurve redChannel;
  public AnimationCurve greenChannel;
  public AnimationCurve blueChannel;
  public bool useDepthCorrection;
  public AnimationCurve zCurve;
  public AnimationCurve depthRedChannel;
  public AnimationCurve depthGreenChannel;
  public AnimationCurve depthBlueChannel;
  private Material ccMaterial;
  private Material ccDepthMaterial;
  private Material selectiveCcMaterial;
  private Texture2D rgbChannelTex;
  private Texture2D rgbDepthChannelTex;
  private Texture2D zCurveTex;
  public float saturation;
  public bool selectiveCc;
  public Color selectiveFromColor;
  public Color selectiveToColor;
  public ColorCorrectionMode mode;
  public bool updateTextures;
  public Shader colorCorrectionCurvesShader;
  public Shader simpleColorCorrectionCurvesShader;
  public Shader colorCorrectionSelectiveShader;
  private bool updateTexturesOnStartup;

  public ColorCorrectionCurves()
  {
    this.saturation = 1f;
    this.selectiveFromColor = Color.white;
    this.selectiveToColor = Color.white;
    this.updateTextures = true;
    this.updateTexturesOnStartup = true;
  }

  public override void Start()
  {
    base.Start();
    this.updateTexturesOnStartup = true;
  }

  public virtual void Awake()
  {
  }

  public override bool CheckResources()
  {
    this.CheckSupport(this.mode == ColorCorrectionMode.Advanced);
    this.ccMaterial = this.CheckShaderAndCreateMaterial(this.simpleColorCorrectionCurvesShader, this.ccMaterial);
    this.ccDepthMaterial = this.CheckShaderAndCreateMaterial(this.colorCorrectionCurvesShader, this.ccDepthMaterial);
    this.selectiveCcMaterial = this.CheckShaderAndCreateMaterial(this.colorCorrectionSelectiveShader, this.selectiveCcMaterial);
    if (!(bool) (UnityEngine.Object) this.rgbChannelTex)
      this.rgbChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
    if (!(bool) (UnityEngine.Object) this.rgbDepthChannelTex)
      this.rgbDepthChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
    if (!(bool) (UnityEngine.Object) this.zCurveTex)
      this.zCurveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
    this.rgbChannelTex.hideFlags = HideFlags.DontSave;
    this.rgbDepthChannelTex.hideFlags = HideFlags.DontSave;
    this.zCurveTex.hideFlags = HideFlags.DontSave;
    this.rgbChannelTex.wrapMode = TextureWrapMode.Clamp;
    this.rgbDepthChannelTex.wrapMode = TextureWrapMode.Clamp;
    this.zCurveTex.wrapMode = TextureWrapMode.Clamp;
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public virtual void UpdateParameters()
  {
    if (this.redChannel == null || this.greenChannel == null || this.blueChannel == null)
      return;
    for (float time = 0.0f; (double) time <= 1.0; time += 0.003921569f)
    {
      float num1 = Mathf.Clamp(this.redChannel.Evaluate(time), 0.0f, 1f);
      float num2 = Mathf.Clamp(this.greenChannel.Evaluate(time), 0.0f, 1f);
      float num3 = Mathf.Clamp(this.blueChannel.Evaluate(time), 0.0f, 1f);
      this.rgbChannelTex.SetPixel((int) Mathf.Floor(time * (float) byte.MaxValue), 0, new Color(num1, num1, num1));
      this.rgbChannelTex.SetPixel((int) Mathf.Floor(time * (float) byte.MaxValue), 1, new Color(num2, num2, num2));
      this.rgbChannelTex.SetPixel((int) Mathf.Floor(time * (float) byte.MaxValue), 2, new Color(num3, num3, num3));
      float num4 = Mathf.Clamp(this.zCurve.Evaluate(time), 0.0f, 1f);
      this.zCurveTex.SetPixel((int) Mathf.Floor(time * (float) byte.MaxValue), 0, new Color(num4, num4, num4));
      float num5 = Mathf.Clamp(this.depthRedChannel.Evaluate(time), 0.0f, 1f);
      float num6 = Mathf.Clamp(this.depthGreenChannel.Evaluate(time), 0.0f, 1f);
      float num7 = Mathf.Clamp(this.depthBlueChannel.Evaluate(time), 0.0f, 1f);
      this.rgbDepthChannelTex.SetPixel((int) Mathf.Floor(time * (float) byte.MaxValue), 0, new Color(num5, num5, num5));
      this.rgbDepthChannelTex.SetPixel((int) Mathf.Floor(time * (float) byte.MaxValue), 1, new Color(num6, num6, num6));
      this.rgbDepthChannelTex.SetPixel((int) Mathf.Floor(time * (float) byte.MaxValue), 2, new Color(num7, num7, num7));
    }
    this.rgbChannelTex.Apply();
    this.rgbDepthChannelTex.Apply();
    this.zCurveTex.Apply();
  }

  public virtual void UpdateTextures() => this.UpdateParameters();

  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if (this.updateTexturesOnStartup)
      {
        this.UpdateParameters();
        this.updateTexturesOnStartup = false;
      }
      if (this.useDepthCorrection)
        this.camera.depthTextureMode |= DepthTextureMode.Depth;
      RenderTexture renderTexture = destination;
      if (this.selectiveCc)
        renderTexture = RenderTexture.GetTemporary(source.width, source.height);
      if (this.useDepthCorrection)
      {
        this.ccDepthMaterial.SetTexture("_RgbTex", (Texture) this.rgbChannelTex);
        this.ccDepthMaterial.SetTexture("_ZCurve", (Texture) this.zCurveTex);
        this.ccDepthMaterial.SetTexture("_RgbDepthTex", (Texture) this.rgbDepthChannelTex);
        this.ccDepthMaterial.SetFloat("_Saturation", this.saturation);
        Graphics.Blit((Texture) source, renderTexture, this.ccDepthMaterial);
      }
      else
      {
        this.ccMaterial.SetTexture("_RgbTex", (Texture) this.rgbChannelTex);
        this.ccMaterial.SetFloat("_Saturation", this.saturation);
        Graphics.Blit((Texture) source, renderTexture, this.ccMaterial);
      }
      if (!this.selectiveCc)
        return;
      this.selectiveCcMaterial.SetColor("selColor", this.selectiveFromColor);
      this.selectiveCcMaterial.SetColor("targetColor", this.selectiveToColor);
      Graphics.Blit((Texture) renderTexture, destination, this.selectiveCcMaterial);
      RenderTexture.ReleaseTemporary(renderTexture);
    }
  }

  public override void Main()
  {
  }
}
