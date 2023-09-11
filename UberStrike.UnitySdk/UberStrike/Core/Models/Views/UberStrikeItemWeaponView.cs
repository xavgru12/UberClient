// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.UberStrikeItemWeaponView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using UberStrike.Core.Types;

namespace UberStrike.Core.Models.Views
{
  [Serializable]
  public class UberStrikeItemWeaponView : BaseUberStrikeItemView
  {
    public override UberstrikeItemType ItemType => UberstrikeItemType.Weapon;

    public int DamageKnockback { get; set; }

    public int DamagePerProjectile { get; set; }

    public int AccuracySpread { get; set; }

    public int RecoilKickback { get; set; }

    public int StartAmmo { get; set; }

    public int MaxAmmo { get; set; }

    public int MissileTimeToDetonate { get; set; }

    public int MissileForceImpulse { get; set; }

    public int MissileBounciness { get; set; }

    public int SplashRadius { get; set; }

    public int ProjectilesPerShot { get; set; }

    public int ProjectileSpeed { get; set; }

    public int RateOfFire { get; set; }

    public int RecoilMovement { get; set; }
  }
}
