// Decompiled with JetBrains decompiler
// Type: VortexEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Vortex")]
public class VortexEffect : ImageEffectBase
{
  public Vector2 radius = new Vector2(0.4f, 0.4f);
  public float angle = 50f;
  public Vector2 center = new Vector2(0.5f, 0.5f);

  private void OnRenderImage(RenderTexture source, RenderTexture destination) => ImageEffects.RenderDistortion(this.material, source, destination, this.angle, this.center, this.radius);
}
