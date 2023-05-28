// Decompiled with JetBrains decompiler
// Type: WeaponPickupItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class WeaponPickupItem : PickupItem
{
  private string _weaponName = string.Empty;
  [SerializeField]
  private UberstrikeItemClass _weaponType;

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
      LayerUtil.SetLayerRecursively(component.transform, UberstrikeLayer.Default);
      this._renderers = component.GetComponentsInChildren<MeshRenderer>(true);
    }
    else
      Debug.LogError((object) ("No Default Weapon found for Class " + (object) this._weaponType));
  }

  protected override bool OnPlayerPickup()
  {
    if (!ApplicationDataManager.ApplicationOptions.GameplayAutoPickupEnabled)
      return false;
    IUnityItem unityItem;
    if (!Singleton<ItemManager>.Instance.TryGetDefaultItem(this._weaponType, out unityItem))
      return false;
    bool flag = Singleton<WeaponController>.Instance.HasWeaponOfClass(this._weaponType);
    Singleton<WeaponController>.Instance.SetPickupWeapon(unityItem.ItemId);
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
      }
      else
      {
        Singleton<PickupNameHud>.Instance.DisplayPickupName(this._weaponName, PickUpMessageType.ChangeWeapon);
        Singleton<WeaponController>.Instance.ResetPickupWeaponSlotInSeconds(0);
      }
      this.PlayLocalPickupSound(GameAudio.WeaponPickup2D);
      if (GameState.IsSinglePlayer)
        this.StartCoroutine(this.StartHidingPickupForSeconds(this._respawnTime));
    }
    return true;
  }

  protected override void OnRemotePickup() => this.PlayRemotePickupSound(GameAudio.WeaponPickup, this.transform.position);

  private void Update()
  {
    if (!(bool) (Object) this._pickupItem)
      return;
    this._pickupItem.Rotate(Vector3.up, 150f * Time.deltaTime, Space.Self);
  }
}
