// Decompiled with JetBrains decompiler
// Type: ScreenOverlay
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Screen Overlay")]
[Serializable]
public class ScreenOverlay : PostEffectsBase
{
  public ScreenOverlay.OverlayBlendMode blendMode;
  public float intensity;
  public Texture2D texture;
  public Shader overlayShader;
  private Material overlayMaterial;

  public ScreenOverlay()
  {
    this.blendMode = ScreenOverlay.OverlayBlendMode.Overlay;
    this.intensity = 1f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.overlayMaterial = this.CheckShaderAndCreateMaterial(this.overlayShader, this.overlayMaterial);
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
      this.overlayMaterial.SetFloat("_Intensity", this.intensity);
      this.overlayMaterial.SetTexture("_Overlay", (Texture) this.texture);
      Graphics.Blit((Texture) source, destination, this.overlayMaterial, (int) this.blendMode);
    }
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum OverlayBlendMode
  {
    Additive,
    ScreenBlend,
    Multiply,
    Overlay,
    AlphaBlend,
  }
}
