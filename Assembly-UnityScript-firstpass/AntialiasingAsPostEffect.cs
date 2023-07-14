// Decompiled with JetBrains decompiler
// Type: AntialiasingAsPostEffect
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Antialiasing (Fullscreen)")]
[Serializable]
public class AntialiasingAsPostEffect : PostEffectsBase
{
  public AAMode mode;
  public bool showGeneratedNormals;
  public float offsetScale;
  public float blurRadius;
  public float edgeThresholdMin;
  public float edgeThreshold;
  public float edgeSharpness;
  public bool dlaaSharp;
  public Shader ssaaShader;
  private Material ssaa;
  public Shader dlaaShader;
  private Material dlaa;
  public Shader nfaaShader;
  private Material nfaa;
  public Shader shaderFXAAPreset2;
  private Material materialFXAAPreset2;
  public Shader shaderFXAAPreset3;
  private Material materialFXAAPreset3;
  public Shader shaderFXAAII;
  private Material materialFXAAII;
  public Shader shaderFXAAIII;
  private Material materialFXAAIII;

  public AntialiasingAsPostEffect()
  {
    this.mode = AAMode.FXAA3Console;
    this.offsetScale = 0.2f;
    this.blurRadius = 18f;
    this.edgeThresholdMin = 0.05f;
    this.edgeThreshold = 0.2f;
    this.edgeSharpness = 4f;
  }

  public virtual Material CurrentAAMaterial()
  {
    switch (this.mode)
    {
      case AAMode.FXAA2:
        return this.materialFXAAII;
      case AAMode.FXAA3Console:
        return this.materialFXAAIII;
      case AAMode.FXAA1PresetA:
        return this.materialFXAAPreset2;
      case AAMode.FXAA1PresetB:
        return this.materialFXAAPreset3;
      case AAMode.NFAA:
        return this.nfaa;
      case AAMode.SSAA:
        return this.ssaa;
      case AAMode.DLAA:
        return this.dlaa;
      default:
        return (Material) null;
    }
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.materialFXAAPreset2 = this.CreateMaterial(this.shaderFXAAPreset2, this.materialFXAAPreset2);
    this.materialFXAAPreset3 = this.CreateMaterial(this.shaderFXAAPreset3, this.materialFXAAPreset3);
    this.materialFXAAII = this.CreateMaterial(this.shaderFXAAII, this.materialFXAAII);
    this.materialFXAAIII = this.CreateMaterial(this.shaderFXAAIII, this.materialFXAAIII);
    this.nfaa = this.CreateMaterial(this.nfaaShader, this.nfaa);
    this.ssaa = this.CreateMaterial(this.ssaaShader, this.ssaa);
    this.dlaa = this.CreateMaterial(this.dlaaShader, this.dlaa);
    if (!this.ssaaShader.isSupported)
    {
      this.NotSupported();
      this.ReportAutoDisable();
    }
    return this.isSupported;
  }

  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
      Graphics.Blit((Texture) source, destination);
    else if (this.mode == AAMode.FXAA3Console && (UnityEngine.Object) this.materialFXAAIII != (UnityEngine.Object) null)
    {
      this.materialFXAAIII.SetFloat("_EdgeThresholdMin", this.edgeThresholdMin);
      this.materialFXAAIII.SetFloat("_EdgeThreshold", this.edgeThreshold);
      this.materialFXAAIII.SetFloat("_EdgeSharpness", this.edgeSharpness);
      Graphics.Blit((Texture) source, destination, this.materialFXAAIII);
    }
    else if (this.mode == AAMode.FXAA1PresetB && (UnityEngine.Object) this.materialFXAAPreset3 != (UnityEngine.Object) null)
      Graphics.Blit((Texture) source, destination, this.materialFXAAPreset3);
    else if (this.mode == AAMode.FXAA1PresetA && (UnityEngine.Object) this.materialFXAAPreset2 != (UnityEngine.Object) null)
    {
      source.anisoLevel = 4;
      Graphics.Blit((Texture) source, destination, this.materialFXAAPreset2);
      source.anisoLevel = 0;
    }
    else if (this.mode == AAMode.FXAA2 && (UnityEngine.Object) this.materialFXAAII != (UnityEngine.Object) null)
      Graphics.Blit((Texture) source, destination, this.materialFXAAII);
    else if (this.mode == AAMode.SSAA && (UnityEngine.Object) this.ssaa != (UnityEngine.Object) null)
      Graphics.Blit((Texture) source, destination, this.ssaa);
    else if (this.mode == AAMode.DLAA && (UnityEngine.Object) this.dlaa != (UnityEngine.Object) null)
    {
      source.anisoLevel = 0;
      RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
      Graphics.Blit((Texture) source, temporary, this.dlaa, 0);
      Graphics.Blit((Texture) temporary, destination, this.dlaa, !this.dlaaSharp ? 1 : 2);
      RenderTexture.ReleaseTemporary(temporary);
    }
    else if (this.mode == AAMode.NFAA && (UnityEngine.Object) this.nfaa != (UnityEngine.Object) null)
    {
      source.anisoLevel = 0;
      this.nfaa.SetFloat("_OffsetScale", this.offsetScale);
      this.nfaa.SetFloat("_BlurRadius", this.blurRadius);
      Graphics.Blit((Texture) source, destination, this.nfaa, !this.showGeneratedNormals ? 0 : 1);
    }
    else
      Graphics.Blit((Texture) source, destination);
  }

  public override void Main()
  {
  }
}
