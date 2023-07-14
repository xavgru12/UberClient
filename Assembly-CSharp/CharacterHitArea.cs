// Decompiled with JetBrains decompiler
// Type: CharacterHitArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class CharacterHitArea : BaseGameProp
{
  [SerializeField]
  private BodyPart _part;

  public override void ApplyDamage(DamageInfo shot)
  {
    shot.BodyPart = this._part;
    if (this.Shootable != null)
    {
      if (!this.Shootable.IsVulnerable)
        return;
      if (this._part == BodyPart.Head || this._part == BodyPart.Nuts)
        shot.Damage += (short) ((double) shot.Damage * (double) shot.CriticalStrikeBonus);
      this.Shootable.ApplyDamage(shot);
    }
    else
      Debug.LogError((object) "No character set to the body part!");
  }

  public override bool IsLocal => this.Shootable != null && this.Shootable.IsLocal;

  public IShootable Shootable { get; set; }

  public BodyPart CharacterBodyPart => this._part;
}
