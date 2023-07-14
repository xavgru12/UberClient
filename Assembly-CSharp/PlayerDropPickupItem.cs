// Decompiled with JetBrains decompiler
// Type: PlayerDropPickupItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PlayerDropPickupItem : PickupItem
{
  public int WeaponItemId;
  private UberstrikeItemClass _weaponType = UberstrikeItemClass.WeaponMelee;
  private float _timeout;

  [DebuggerHidden]
  private IEnumerator Start() => (IEnumerator) new PlayerDropPickupItem.\u003CStart\u003Ec__Iterator3A()
  {
    \u003C\u003Ef__this = this
  };

  private void Update()
  {
    if (!(bool) (Object) this._pickupItem)
      return;
    this._pickupItem.Rotate(Vector3.up, 150f * Time.deltaTime, Space.Self);
  }

  protected override bool OnPlayerPickup()
  {
    if (!ApplicationDataManager.ApplicationOptions.GameplayAutoPickupEnabled)
      return false;
    bool flag = Singleton<WeaponController>.Instance.HasWeaponOfClass(this._weaponType);
    Singleton<WeaponController>.Instance.SetPickupWeapon(this.WeaponItemId, false, false);
    if (Singleton<WeaponController>.Instance.CheckPlayerWeaponInPickupSlot(this.WeaponItemId))
      Singleton<WeaponController>.Instance.ResetPickupWeaponSlotInSeconds(10);
    if (GameState.HasCurrentGame)
    {
      GameState.CurrentGame.PickupPowerup(this.PickupID, PickupItemType.Weapon, 0);
      if (flag)
      {
        switch (this._weaponType)
        {
          case UberstrikeItemClass.WeaponHandgun:
            Singleton<PickupNameHud>.Instance.DisplayPickupName("Handgun Rounds", PickUpMessageType.AmmoHandgun);
            break;
          case UberstrikeItemClass.WeaponMachinegun:
            Singleton<PickupNameHud>.Instance.DisplayPickupName("Machinegun Ammo", PickUpMessageType.AmmoMachinegun);
            break;
          case UberstrikeItemClass.WeaponShotgun:
            Singleton<PickupNameHud>.Instance.DisplayPickupName("Shotgun Shells", PickUpMessageType.AmmoShotgun);
            break;
          case UberstrikeItemClass.WeaponSniperRifle:
            Singleton<PickupNameHud>.Instance.DisplayPickupName("Sniper Bullets", PickUpMessageType.AmmoSnipergun);
            break;
          case UberstrikeItemClass.WeaponCannon:
            Singleton<PickupNameHud>.Instance.DisplayPickupName("Cannon Rockets", PickUpMessageType.AmmoCannon);
            break;
          case UberstrikeItemClass.WeaponSplattergun:
            Singleton<PickupNameHud>.Instance.DisplayPickupName("Splattergun Cells", PickUpMessageType.AmmoSplattergun);
            break;
          case UberstrikeItemClass.WeaponLauncher:
            Singleton<PickupNameHud>.Instance.DisplayPickupName("Launcher Grenades", PickUpMessageType.AmmoLauncher);
            break;
        }
        this.PlayLocalPickupSound(GameAudio.WeaponPickup2D);
        this.StartCoroutine(this.StartHidingPickupForSeconds(0));
      }
    }
    return true;
  }

  protected override void OnRemotePickup() => this.PlayRemotePickupSound(GameAudio.WeaponPickup, this.transform.position);
}
