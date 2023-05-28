// Decompiled with JetBrains decompiler
// Type: WeaponShotCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Core.Types;

public class WeaponShotCounter
{
  private Dictionary<UberstrikeItemClass, int> _shotCountPerWeaponClass;
  private UberstrikeItemClass[] _allWeaponClasses = new UberstrikeItemClass[8]
  {
    UberstrikeItemClass.WeaponCannon,
    UberstrikeItemClass.WeaponHandgun,
    UberstrikeItemClass.WeaponLauncher,
    UberstrikeItemClass.WeaponMachinegun,
    UberstrikeItemClass.WeaponMelee,
    UberstrikeItemClass.WeaponShotgun,
    UberstrikeItemClass.WeaponSniperRifle,
    UberstrikeItemClass.WeaponSplattergun
  };

  public WeaponShotCounter()
  {
    this._shotCountPerWeaponClass = new Dictionary<UberstrikeItemClass, int>();
    this.Reset();
  }

  public void Reset()
  {
    foreach (UberstrikeItemClass allWeaponClass in this._allWeaponClasses)
      this._shotCountPerWeaponClass[allWeaponClass] = 0;
  }

  public int GetShotCount(UberstrikeItemClass weaponClass) => this._shotCountPerWeaponClass[weaponClass];

  public void IncreaseShotCount(UberstrikeItemClass weaponClass)
  {
    Dictionary<UberstrikeItemClass, int> countPerWeaponClass;
    UberstrikeItemClass key;
    (countPerWeaponClass = this._shotCountPerWeaponClass)[key = weaponClass] = countPerWeaponClass[key] + 1;
  }
}
