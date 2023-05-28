// Decompiled with JetBrains decompiler
// Type: AmmoPickupItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class AmmoPickupItem : PickupItem
{
  [SerializeField]
  private AmmoType _ammoType;

  protected override bool OnPlayerPickup()
  {
    bool flag = AmmoDepot.CanAddAmmo(this._ammoType);
    if (flag)
    {
      AmmoDepot.AddDefaultAmmoOfType(this._ammoType);
      Singleton<WeaponController>.Instance.UpdateAmmoHUD();
      switch (this._ammoType)
      {
        case AmmoType.Cannon:
          Singleton<PickupNameHud>.Instance.DisplayPickupName("Cannon Rockets", PickUpMessageType.AmmoCannon);
          break;
        case AmmoType.Handgun:
          Singleton<PickupNameHud>.Instance.DisplayPickupName("Handgun Rounds", PickUpMessageType.AmmoHandgun);
          break;
        case AmmoType.Launcher:
          Singleton<PickupNameHud>.Instance.DisplayPickupName("Launcher Grenades", PickUpMessageType.AmmoLauncher);
          break;
        case AmmoType.Machinegun:
          Singleton<PickupNameHud>.Instance.DisplayPickupName("Machinegun Ammo", PickUpMessageType.AmmoMachinegun);
          break;
        case AmmoType.Shotgun:
          Singleton<PickupNameHud>.Instance.DisplayPickupName("Shotgun Shells", PickUpMessageType.AmmoShotgun);
          break;
        case AmmoType.Snipergun:
          Singleton<PickupNameHud>.Instance.DisplayPickupName("Sniper Bullets", PickUpMessageType.AmmoSnipergun);
          break;
        case AmmoType.Splattergun:
          Singleton<PickupNameHud>.Instance.DisplayPickupName("Splattergun Cells", PickUpMessageType.AmmoSplattergun);
          break;
      }
      this.PlayLocalPickupSound(GameAudio.AmmoPickup2D);
      if (GameState.HasCurrentGame)
      {
        GameState.CurrentGame.PickupPowerup(this.PickupID, PickupItemType.Ammo, 0);
        if (GameState.IsSinglePlayer)
          this.StartCoroutine(this.StartHidingPickupForSeconds(this._respawnTime));
      }
    }
    return flag;
  }

  protected override void OnRemotePickup() => this.PlayRemotePickupSound(GameAudio.AmmoPickup, this.transform.position);

  protected override bool CanPlayerPickup => AmmoDepot.CanAddAmmo(this._ammoType);
}
