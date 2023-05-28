// Decompiled with JetBrains decompiler
// Type: GrayscaleEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Grayscale")]
public class GrayscaleEffect : ImageEffectBase
{
  public Texture textureRamp;
  public float rampOffset;

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.material.SetTexture("_RampTex", this.textureRamp);
    this.material.SetFloat("_RampOffset", this.rampOffset);
    Graphics.Blit((Texture) source, destination, this.material);
  }
}
