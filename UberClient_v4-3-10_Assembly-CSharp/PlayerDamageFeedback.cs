// Decompiled with JetBrains decompiler
// Type: PlayerDamageFeedback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PlayerDamageFeedback : MonoBehaviour
{
  private Material damageSplat;
  public Color[] DamageColors;
  public float Factor;
  private int colorIndex;

  private void Awake() => this.damageSplat = this.renderer.material;

  public void RandomizeDamageFeedbackcolor() => this.colorIndex = Random.Range(0, 5);

  public void ShowDamageFeedback(float damage)
  {
    this.DamageColors[this.colorIndex].a = damage * this.Factor;
    this.damageSplat.color = this.DamageColors[this.colorIndex];
  }
}
