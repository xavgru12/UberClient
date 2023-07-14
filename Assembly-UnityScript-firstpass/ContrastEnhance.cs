// Decompiled with JetBrains decompiler
// Type: ContrastEnhance
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Contrast Enhance (Unsharp Mask)")]
[Serializable]
public class ContrastEnhance : PostEffectsBase
{
  public float intensity;
  public float threshhold;
  private Material separableBlurMaterial;
  private Material contrastCompositeMaterial;
  public float blurSpread;
  public Shader separableBlurShader;
  public Shader contrastCompositeShader;

  public ContrastEnhance()
  {
    this.intensity = 0.5f;
    this.blurSpread = 1f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.contrastCompositeMaterial = this.CheckShaderAndCreateMaterial(this.contrastCompositeShader, this.contrastCompositeMaterial);
    this.separableBlurMaterial = this.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
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
      RenderTexture temporary1 = RenderTexture.GetTemporary((int) ((double) source.width / 2.0), (int) ((double) source.height / 2.0), 0);
      RenderTexture temporary2 = RenderTexture.GetTemporary((int) ((double) source.width / 4.0), (int) ((double) source.height / 4.0), 0);
      RenderTexture temporary3 = RenderTexture.GetTemporary((int) ((double) source.width / 4.0), (int) ((double) source.height / 4.0), 0);
      Graphics.Blit((Texture) source, temporary1);
      Graphics.Blit((Texture) temporary1, temporary2);
      this.separableBlurMaterial.SetVector("offsets", new Vector4(0.0f, this.blurSpread * 1f / (float) temporary2.height, 0.0f, 0.0f));
      Graphics.Blit((Texture) temporary2, temporary3, this.separableBlurMaterial);
      this.separableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 1f / (float) temporary2.width, 0.0f, 0.0f, 0.0f));
      Graphics.Blit((Texture) temporary3, temporary2, this.separableBlurMaterial);
      this.contrastCompositeMaterial.SetTexture("_MainTexBlurred", (Texture) temporary2);
      this.contrastCompositeMaterial.SetFloat("intensity", this.intensity);
      this.contrastCompositeMaterial.SetFloat("threshhold", this.threshhold);
      Graphics.Blit((Texture) source, destination, this.contrastCompositeMaterial);
      RenderTexture.ReleaseTemporary(temporary1);
      RenderTexture.ReleaseTemporary(temporary2);
      RenderTexture.ReleaseTemporary(temporary3);
    }
  }

  public override void Main()
  {
  }
}
