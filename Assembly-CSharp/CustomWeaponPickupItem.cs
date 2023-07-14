// Decompiled with JetBrains decompiler
// Type: CustomWeaponPickupItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class CustomWeaponPickupItem : PickupItem
{
  private string _weaponName = string.Empty;
  [SerializeField]
  private int _weaponId;

  private void Start()
  {
    WeaponItem weaponItemInShop = Singleton<ItemManager>.Instance.GetWeaponItemInShop(this._weaponId);
    if (!((Object) weaponItemInShop != (Object) null))
      return;
    this._weaponName = weaponItemInShop.Name;
    BaseWeaponDecorator component = Singleton<ItemManager>.Instance.Instantiate(weaponItemInShop.ItemId).GetComponent<BaseWeaponDecorator>();
    component.transform.parent = this._pickupItem;
    component.transform.localPosition = new Vector3(0.0f, 0.0f, -0.3f);
    component.transform.localRotation = Quaternion.identity;
    component.transform.localScale = Vector3.one;
    LayerUtil.SetLayerRecursively(component.transform, UberstrikeLayer.Default);
    this._renderers = component.GetComponentsInChildren<MeshRenderer>(true);
  }

  protected override bool OnPlayerPickup()
  {
    if (!ApplicationDataManager.ApplicationOptions.GameplayAutoPickupEnabled)
      return false;
    Singleton<WeaponController>.Instance.SetPickupWeapon(this._weaponId);
    if (GameState.HasCurrentGame)
    {
      GameState.CurrentGame.PickupPowerup(this.PickupID, PickupItemType.Weapon, 0);
      Singleton<PickupNameHud>.Instance.DisplayPickupName(this._weaponName, PickUpMessageType.ChangeWeapon);
      Singleton<WeaponController>.Instance.ResetPickupWeaponSlotInSeconds(0);
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
