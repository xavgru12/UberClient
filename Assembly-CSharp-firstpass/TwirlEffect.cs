// Decompiled with JetBrains decompiler
// Type: TwirlEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21AF7BBC-70B8-4BE8-9CDE-C2EC2144EAE4
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[AddComponentMenu("Image Effects/Twirl")]
[ExecuteInEditMode]
public class TwirlEffect : ImageEffectBase
{
  public Vector2 radius = new Vector2(0.3f, 0.3f);
  public float angle = 50f;
  public Vector2 center = new Vector2(0.5f, 0.5f);

  private void OnRenderImage(RenderTexture source, RenderTexture destination) => ImageEffects.RenderDistortion(this.material, source, destination, this.angle, this.center, this.radius);
}
