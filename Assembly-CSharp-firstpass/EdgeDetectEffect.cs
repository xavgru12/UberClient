// Decompiled with JetBrains decompiler
// Type: EdgeDetectEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21AF7BBC-70B8-4BE8-9CDE-C2EC2144EAE4
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
