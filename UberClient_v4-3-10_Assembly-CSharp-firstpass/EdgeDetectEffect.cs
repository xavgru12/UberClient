// Decompiled with JetBrains decompiler
// Type: EdgeDetectEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[AddComponentMenu("Image Effects/Edge Detection (Color)")]
[ExecuteInEditMode]
public class EdgeDetectEffect : ImageEffectBase
{
  public float threshold = 0.2f;

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.material.SetFloat("_Treshold", this.threshold * this.threshold);
    Graphics.Blit((Texture) source, destination, this.material);
  }
}
