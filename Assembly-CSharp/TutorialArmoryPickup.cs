// Decompiled with JetBrains decompiler
// Type: TutorialArmoryPickup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class TutorialArmoryPickup : PickupItem
{
  private string _weaponName = string.Empty;
  [SerializeField]
  private UberstrikeItemClass _weaponType;
  [SerializeField]
  private TutorialWaypoint _waypoint;

  private void Start()
  {
    IUnityItem unityItem;
    if (Singleton<ItemManager>.Instance.TryGetDefaultItem(this._weaponType, out unityItem))
    {
      this._weaponName = unityItem.Name;
      BaseWeaponDecorator component = Singleton<ItemManager>.Instance.Instantiate(unityItem.ItemId).GetComponent<BaseWeaponDecorator>();
      component.transform.parent = this._pickupItem;
      component.transform.localPosition = new Vector3(0.0f, 0.0f, -0.3f);
      component.transform.localRotation = Quaternion.identity;
      component.transform.localScale = Vector3.one;
      LayerUtil.SetLayerRecursively(component.transform, UberstrikeLayer.GloballyLit);
      this._renderers = component.GetComponentsInChildren<MeshRenderer>(true);
    }
    else
      Debug.LogError((object) ("No Default Weapon found for Class " + (object) this._weaponType));
  }

  protected override bool OnPlayerPickup()
  {
    this.IsAvailable = false;
    bool flag1 = false;
    IUnityItem unityItem;
    if (Singleton<ItemManager>.Instance.TryGetDefaultItem(this._weaponType, out unityItem))
    {
      flag1 = true;
      bool flag2 = Singleton<WeaponController>.Instance.HasWeaponOfClass(this._weaponType);
      Singleton<WeaponController>.Instance.SetPickupWeapon(unityItem.ItemId);
      if ((bool) (UnityEngine.Object) this._pickupItem)
        this._pickupItem.gameObject.SetActive(false);
      if (GameState.HasCurrentGame && GameState.CurrentGame is TutorialGameMode)
      {
        (GameState.CurrentGame as TutorialGameMode).OnArmoryPickupMG();
        ParticleEffectController.ShowPickUpEffect(this._pickupItem.position, 100);
      }
      if (GameState.HasCurrentGame)
      {
        GameState.CurrentGame.PickupPowerup(this.PickupID, PickupItemType.Weapon, 0);
        if (flag2)
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
        }
        else
        {
          Singleton<PickupNameHud>.Instance.DisplayPickupName(this._weaponName, PickUpMessageType.ChangeWeapon);
          Singleton<WeaponController>.Instance.ResetPickupWeaponSlotInSeconds(0);
        }
      }
    }
    else
      Debug.LogError((object) ("Cannot get default item of type: " + ((Enum) this._weaponType).ToString()));
    return flag1;
  }

  public void Show(bool show)
  {
    this.SetItemAvailable(show);
    if ((bool) (UnityEngine.Object) this._waypoint)
      this._waypoint.CanShow = show;
    if (!show || !(bool) (UnityEngine.Object) this._pickupItem)
      return;
    this._pickupItem.gameObject.SetActive(true);
  }
}
