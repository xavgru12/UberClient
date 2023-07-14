// Decompiled with JetBrains decompiler
// Type: AnimateTextureUV
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class AnimateTextureUV : MonoBehaviour
{
  public int uvAnimationTileX = 1;
  public int uvAnimationTileY = 1;
  public int framesPerSecond = 10;

  private void Update()
  {
    int num1 = Mathf.RoundToInt(Time.time * (float) this.framesPerSecond) % (this.uvAnimationTileX * this.uvAnimationTileY);
    Vector2 scale = new Vector2(1f / (float) this.uvAnimationTileX, 1f / (float) this.uvAnimationTileY);
    int num2 = num1 % this.uvAnimationTileX;
    int num3 = num1 / this.uvAnimationTileX;
    this.renderer.material.SetTextureOffset("_MainTex", new Vector2((float) num2 * scale.x, (float) (1.0 - (double) scale.y - (double) num3 * (double) scale.y)));
    this.renderer.material.SetTextureScale("_MainTex", scale);
  }
}
