// Decompiled with JetBrains decompiler
// Type: AmmoDepot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Core.Types;
using UnityEngine;

public static class AmmoDepot
{
  private static Dictionary<AmmoType, FastSecureInteger> _currentAmmo = new Dictionary<AmmoType, FastSecureInteger>(7);
  private static Dictionary<AmmoType, FastSecureInteger> _startAmmo;
  private static Dictionary<AmmoType, FastSecureInteger> _maxAmmo = new Dictionary<AmmoType, FastSecureInteger>(7);

  static AmmoDepot()
  {
    AmmoDepot._startAmmo = new Dictionary<AmmoType, FastSecureInteger>(7);
    foreach (int num in Enum.GetValues(typeof (AmmoType)))
    {
      AmmoType key = (AmmoType) num;
      AmmoDepot._startAmmo.Add(key, new FastSecureInteger(100));
      AmmoDepot._currentAmmo.Add(key, new FastSecureInteger(0));
      AmmoDepot._maxAmmo.Add(key, new FastSecureInteger(200));
    }
  }

  public static void Reset()
  {
    AmmoDepot._currentAmmo[AmmoType.Handgun].Value = AmmoDepot._startAmmo[AmmoType.Handgun].Value;
    AmmoDepot._currentAmmo[AmmoType.Machinegun].Value = AmmoDepot._startAmmo[AmmoType.Machinegun].Value;
    AmmoDepot._currentAmmo[AmmoType.Launcher].Value = AmmoDepot._startAmmo[AmmoType.Launcher].Value;
    AmmoDepot._currentAmmo[AmmoType.Shotgun].Value = AmmoDepot._startAmmo[AmmoType.Shotgun].Value;
    AmmoDepot._currentAmmo[AmmoType.Cannon].Value = AmmoDepot._startAmmo[AmmoType.Cannon].Value;
    AmmoDepot._currentAmmo[AmmoType.Splattergun].Value = AmmoDepot._startAmmo[AmmoType.Splattergun].Value;
    AmmoDepot._currentAmmo[AmmoType.Snipergun].Value = AmmoDepot._startAmmo[AmmoType.Snipergun].Value;
  }

  public static void SetMaxAmmoForType(WeaponItem t, int maxAmmoCount)
  {
    if (!PlayerDataManager.IsPlayerLoggedIn)
      return;
    switch (t.ItemClass)
    {
      case UberstrikeItemClass.WeaponHandgun:
        AmmoDepot._maxAmmo[AmmoType.Handgun].Value = maxAmmoCount;
        break;
      case UberstrikeItemClass.WeaponMachinegun:
        AmmoDepot._maxAmmo[AmmoType.Machinegun].Value = maxAmmoCount;
        break;
      case UberstrikeItemClass.WeaponShotgun:
        AmmoDepot._maxAmmo[AmmoType.Shotgun].Value = maxAmmoCount;
        break;
      case UberstrikeItemClass.WeaponSniperRifle:
        AmmoDepot._maxAmmo[AmmoType.Snipergun].Value = maxAmmoCount;
        break;
      case UberstrikeItemClass.WeaponCannon:
        AmmoDepot._maxAmmo[AmmoType.Cannon].Value = maxAmmoCount;
        break;
      case UberstrikeItemClass.WeaponSplattergun:
        AmmoDepot._maxAmmo[AmmoType.Splattergun].Value = maxAmmoCount;
        break;
      case UberstrikeItemClass.WeaponLauncher:
        AmmoDepot._maxAmmo[AmmoType.Launcher].Value = maxAmmoCount;
        break;
    }
  }

  public static void SetStartAmmoForType(WeaponItem t, int startAmmoCount)
  {
    if (!PlayerDataManager.IsPlayerLoggedIn)
      return;
    switch (t.ItemClass)
    {
      case UberstrikeItemClass.WeaponHandgun:
        AmmoDepot._startAmmo[AmmoType.Handgun].Value = startAmmoCount;
        break;
      case UberstrikeItemClass.WeaponMachinegun:
        AmmoDepot._startAmmo[AmmoType.Machinegun].Value = startAmmoCount;
        break;
      case UberstrikeItemClass.WeaponShotgun:
        AmmoDepot._startAmmo[AmmoType.Shotgun].Value = startAmmoCount;
        break;
      case UberstrikeItemClass.WeaponSniperRifle:
        AmmoDepot._startAmmo[AmmoType.Snipergun].Value = startAmmoCount;
        break;
      case UberstrikeItemClass.WeaponCannon:
        AmmoDepot._startAmmo[AmmoType.Cannon].Value = startAmmoCount;
        break;
      case UberstrikeItemClass.WeaponSplattergun:
        AmmoDepot._startAmmo[AmmoType.Splattergun].Value = startAmmoCount;
        break;
      case UberstrikeItemClass.WeaponLauncher:
        AmmoDepot._startAmmo[AmmoType.Launcher].Value = startAmmoCount;
        break;
    }
  }

  public static bool CanAddAmmo(AmmoType t)
  {
    UberstrikeItemClass itemClass;
    return AmmoDepot.TryGetAmmoTypeFromItemClass(t, out itemClass) && Singleton<WeaponController>.Instance.HasWeaponOfClass(itemClass) && AmmoDepot._currentAmmo[t].Value < AmmoDepot._maxAmmo[t].Value;
  }

  public static void AddAmmoOfClass(UberstrikeItemClass c)
  {
    AmmoType t;
    if (!AmmoDepot.TryGetAmmoType(c, out t))
      return;
    AmmoDepot.AddDefaultAmmoOfType(t);
  }

  public static void AddDefaultAmmoOfType(AmmoType t) => AmmoDepot.AddAmmoOfType(t, AmmoDepot._startAmmo[t].Value);

  public static void AddAmmoOfType(AmmoType t, int bullets) => AmmoDepot._currentAmmo[t].Value = Mathf.Clamp(AmmoDepot._currentAmmo[t].Value + bullets, 0, AmmoDepot._maxAmmo[t].Value);

  public static void AddStartAmmoOfType(AmmoType t, float percentage = 1)
  {
    int num = Mathf.CeilToInt((float) AmmoDepot._startAmmo[t].Value * percentage);
    AmmoDepot._currentAmmo[t].Value = Mathf.Clamp(AmmoDepot._currentAmmo[t].Value + num, 0, AmmoDepot._maxAmmo[t].Value);
  }

  public static void AddMaxAmmoOfType(AmmoType t, float percentage = 1)
  {
    int num = Mathf.CeilToInt((float) AmmoDepot._maxAmmo[t].Value * percentage);
    AmmoDepot._currentAmmo[t].Value = Mathf.Clamp(AmmoDepot._currentAmmo[t].Value + num, 0, AmmoDepot._maxAmmo[t].Value);
  }

  public static bool HasAmmoOfType(AmmoType t) => AmmoDepot._currentAmmo[t].Value > 0;

  public static bool HasAmmoOfClass(UberstrikeItemClass t)
  {
    if (t == UberstrikeItemClass.WeaponMelee)
      return true;
    AmmoType t1;
    return AmmoDepot.TryGetAmmoType(t, out t1) && AmmoDepot.HasAmmoOfType(t1);
  }

  public static int AmmoOfType(AmmoType t) => AmmoDepot._currentAmmo[t].Value;

  public static int AmmoOfClass(UberstrikeItemClass t)
  {
    AmmoType t1;
    return AmmoDepot.TryGetAmmoType(t, out t1) ? AmmoDepot.AmmoOfType(t1) : 0;
  }

  private static bool TryGetAmmoType(UberstrikeItemClass item, out AmmoType t)
  {
    switch (item)
    {
      case UberstrikeItemClass.WeaponHandgun:
        t = AmmoType.Handgun;
        return true;
      case UberstrikeItemClass.WeaponMachinegun:
        t = AmmoType.Machinegun;
        return true;
      case UberstrikeItemClass.WeaponShotgun:
        t = AmmoType.Shotgun;
        return true;
      case UberstrikeItemClass.WeaponSniperRifle:
        t = AmmoType.Snipergun;
        return true;
      case UberstrikeItemClass.WeaponCannon:
        t = AmmoType.Cannon;
        return true;
      case UberstrikeItemClass.WeaponSplattergun:
        t = AmmoType.Splattergun;
        return true;
      case UberstrikeItemClass.WeaponLauncher:
        t = AmmoType.Launcher;
        return true;
      default:
        t = AmmoType.Handgun;
        return false;
    }
  }

  private static bool TryGetAmmoTypeFromItemClass(AmmoType t, out UberstrikeItemClass itemClass)
  {
    switch (t)
    {
      case AmmoType.Cannon:
        itemClass = UberstrikeItemClass.WeaponCannon;
        return true;
      case AmmoType.Handgun:
        itemClass = UberstrikeItemClass.WeaponHandgun;
        return true;
      case AmmoType.Launcher:
        itemClass = UberstrikeItemClass.WeaponLauncher;
        return true;
      case AmmoType.Machinegun:
        itemClass = UberstrikeItemClass.WeaponMachinegun;
        return true;
      case AmmoType.Shotgun:
        itemClass = UberstrikeItemClass.WeaponShotgun;
        return true;
      case AmmoType.Snipergun:
        itemClass = UberstrikeItemClass.WeaponSniperRifle;
        return true;
      case AmmoType.Splattergun:
        itemClass = UberstrikeItemClass.WeaponSplattergun;
        return true;
      default:
        itemClass = UberstrikeItemClass.WeaponHandgun;
        return false;
    }
  }

  public static bool UseAmmoOfType(AmmoType t)
  {
    if (AmmoDepot._currentAmmo[t].Value <= 0)
      return false;
    AmmoDepot._currentAmmo[t].Decrement(1);
    return true;
  }

  public static bool UseAmmoOfClass(UberstrikeItemClass t)
  {
    AmmoType t1;
    return AmmoDepot.TryGetAmmoType(t, out t1) && AmmoDepot.UseAmmoOfType(t1);
  }

  public static string ToString(AmmoType t) => AmmoDepot._currentAmmo[t].ToString();

  public static void RemoveExtraAmmoOfType(UberstrikeItemClass t)
  {
    AmmoType t1;
    if (!AmmoDepot.TryGetAmmoType(t, out t1) || AmmoDepot._currentAmmo[t1].Value <= AmmoDepot._maxAmmo[t1].Value)
      return;
    AmmoDepot._currentAmmo[t1].Value = AmmoDepot._maxAmmo[t1].Value;
  }
}
