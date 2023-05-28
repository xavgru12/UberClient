// Decompiled with JetBrains decompiler
// Type: Crease
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Crease")]
[Serializable]
public class Crease : PostEffectsBase
{
  public float intensity;
  public int softness;
  public float spread;
  public Shader blurShader;
  private Material blurMaterial;
  public Shader depthFetchShader;
  private Material depthFetchMaterial;
  public Shader creaseApplyShader;
  private Material creaseApplyMaterial;

  public Crease()
  {
    this.intensity = 0.5f;
    this.softness = 1;
    this.spread = 1f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.blurMaterial = this.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
    this.depthFetchMaterial = this.CheckShaderAndCreateMaterial(this.depthFetchShader, this.depthFetchMaterial);
    this.creaseApplyMaterial = this.CheckShaderAndCreateMaterial(this.creaseApplyShader, this.creaseApplyMaterial);
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
      RenderTexture temporary1 = RenderTexture.GetTemporary(source.width, source.height, 0);
      RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0);
      RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0);
      Graphics.Blit((Texture) source, temporary1, this.depthFetchMaterial);
      Graphics.Blit((Texture) temporary1, temporary2);
      for (int index = 0; index < this.softness; ++index)
      {
        this.blurMaterial.SetVector("offsets", new Vector4(0.0f, this.spread * num2, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary2, temporary3, this.blurMaterial);
        this.blurMaterial.SetVector("offsets", new Vector4(this.spread * num2 / num1, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary3, temporary2, this.blurMaterial);
      }
      this.creaseApplyMaterial.SetTexture("_HrDepthTex", (Texture) temporary1);
      this.creaseApplyMaterial.SetTexture("_LrDepthTex", (Texture) temporary2);
      this.creaseApplyMaterial.SetFloat("intensity", this.intensity);
      Graphics.Blit((Texture) source, destination, this.creaseApplyMaterial);
      RenderTexture.ReleaseTemporary(temporary1);
      RenderTexture.ReleaseTemporary(temporary2);
      RenderTexture.ReleaseTemporary(temporary3);
    }
  }

  public override void Main()
  {
  }
}
