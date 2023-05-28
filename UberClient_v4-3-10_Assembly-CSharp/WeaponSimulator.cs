// Decompiled with JetBrains decompiler
// Type: WeaponSimulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Core.Models.Views;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class WeaponSimulator : IWeaponController
{
  private bool _isFullSimulation = true;
  private AvatarDecorator _avatar;
  private CharacterConfig _config;
  private float _nextShootTime;
  private WeaponSlot _currentSlot;
  private WeaponSlot[] _weaponSlots;
  private int _projectileId;

  public WeaponSimulator(CharacterConfig config)
  {
    this._config = config;
    this._weaponSlots = new WeaponSlot[5];
    this.CurrentSlotIndex = -1;
  }

  public void Update(UberStrike.Realtime.UnitySdk.CharacterInfo state, bool isLocal)
  {
    if (!((Object) this._avatar != (Object) null) || state == null || !state.IsAlive || isLocal || !state.IsFiring)
      return;
    this.Shoot(state);
  }

  public void Shoot(UberStrike.Realtime.UnitySdk.CharacterInfo state)
  {
    if (state == null || (double) this._nextShootTime >= (double) Time.time)
      return;
    if (this._currentSlot != null)
    {
      this._nextShootTime = Time.time + WeaponConfigurationHelper.GetRateOfFire((UberStrikeItemWeaponView) this._currentSlot.Logic.Config);
      if (!this._isFullSimulation)
        return;
      this.BeginShooting();
      this._currentSlot.Logic.Shoot(new Ray(state.ShootingPoint + LocalPlayer.EyePosition, state.ShootingDirection), out CmunePairList<BaseGameProp, ShotPoint> _);
      this.EndShooting();
    }
    else
      Debug.LogWarning((object) "Current weapon is null!");
  }

  public IProjectile EmitProjectile(
    int actorID,
    byte playerNumber,
    Vector3 origin,
    Vector3 direction,
    LoadoutSlotType slot,
    int projectileId,
    bool explode)
  {
    IProjectile projectile = (IProjectile) null;
    if (this._isFullSimulation)
    {
      this.BeginShooting();
      switch (slot)
      {
        case LoadoutSlotType.WeaponPrimary:
          projectile = this.ShootProjectileFromSlot(1, origin, direction, projectileId, explode, actorID);
          break;
        case LoadoutSlotType.WeaponSecondary:
          projectile = this.ShootProjectileFromSlot(2, origin, direction, projectileId, explode, actorID);
          break;
        case LoadoutSlotType.WeaponTertiary:
          projectile = this.ShootProjectileFromSlot(3, origin, direction, projectileId, explode, actorID);
          break;
        case LoadoutSlotType.WeaponPickup:
          projectile = this.ShootProjectileFromSlot(4, origin, direction, projectileId, explode, actorID);
          break;
      }
      this.EndShooting();
    }
    return projectile;
  }

  private void BeginShooting()
  {
    foreach (Component hitArea in this._avatar.HitAreas)
      hitArea.gameObject.layer = 2;
  }

  private void EndShooting()
  {
    foreach (Component hitArea in this._avatar.HitAreas)
      hitArea.gameObject.layer = this._avatar.gameObject.layer;
  }

  private IProjectile ShootProjectileFromSlot(
    int slot,
    Vector3 origin,
    Vector3 direction,
    int projectileID,
    bool explode,
    int actorID)
  {
    if (this._weaponSlots.Length > slot && this._weaponSlots[slot] != null && this._weaponSlots[slot].Logic is ProjectileWeapon logic)
    {
      logic.Decorator.PlayShootSound();
      if (!explode)
        return (IProjectile) logic.EmitProjectile(new Ray(origin, direction), projectileID, actorID);
      logic.ShowExplosionEffect(origin, Vector3.up, direction, projectileID);
    }
    return (IProjectile) null;
  }

  public int CurrentSlotIndex { get; private set; }

  public void UpdateWeaponSlot(int slotIndex, bool isLocal)
  {
    this.CurrentSlotIndex = slotIndex;
    switch (slotIndex)
    {
      case 0:
        this._currentSlot = this._weaponSlots[0];
        if (isLocal)
          break;
        this._avatar.ShowWeapon(LoadoutSlotType.WeaponMelee);
        break;
      case 1:
        this._currentSlot = this._weaponSlots[1];
        if (isLocal)
          break;
        this._avatar.ShowWeapon(LoadoutSlotType.WeaponPrimary);
        break;
      case 2:
        this._currentSlot = this._weaponSlots[2];
        if (isLocal)
          break;
        this._avatar.ShowWeapon(LoadoutSlotType.WeaponSecondary);
        break;
      case 3:
        this._currentSlot = this._weaponSlots[3];
        if (isLocal)
          break;
        this._avatar.ShowWeapon(LoadoutSlotType.WeaponTertiary);
        break;
      case 4:
        this._currentSlot = this._weaponSlots[4];
        if (isLocal)
          break;
        this._avatar.ShowWeapon(LoadoutSlotType.WeaponPickup);
        break;
    }
  }

  public void UpdateWeapons(
    int currentWeaponSlot,
    IList<int> weaponItemIds,
    IList<int> quickItemIds)
  {
    if (!((Object) this._avatar != (Object) null))
      return;
    WeaponItem[] weaponItemArray = new WeaponItem[5]
    {
      Singleton<ItemManager>.Instance.GetWeaponItemInShop(weaponItemIds[0]),
      Singleton<ItemManager>.Instance.GetWeaponItemInShop(weaponItemIds[1]),
      Singleton<ItemManager>.Instance.GetWeaponItemInShop(weaponItemIds[2]),
      Singleton<ItemManager>.Instance.GetWeaponItemInShop(weaponItemIds[3]),
      Singleton<ItemManager>.Instance.GetWeaponItemInShop(weaponItemIds[4])
    };
    LoadoutSlotType[] loadoutSlotTypeArray = new LoadoutSlotType[5]
    {
      LoadoutSlotType.WeaponMelee,
      LoadoutSlotType.WeaponPrimary,
      LoadoutSlotType.WeaponSecondary,
      LoadoutSlotType.WeaponTertiary,
      LoadoutSlotType.WeaponPickup
    };
    int num = -1;
    for (int index = 0; index < this._weaponSlots.Length; ++index)
    {
      if (this._weaponSlots[index] != null && (Object) this._weaponSlots[index].Decorator != (Object) null)
        Object.Destroy((Object) this._weaponSlots[index].Decorator.gameObject);
      if ((Object) weaponItemArray[index] != (Object) null && (bool) (Object) this._avatar.WeaponAttachPoint)
      {
        WeaponSlot weaponSlot = new WeaponSlot(loadoutSlotTypeArray[index], weaponItemArray[index], this._avatar.WeaponAttachPoint, (IWeaponController) this);
        if ((bool) (Object) weaponSlot.Decorator)
        {
          if (num < 0)
            num = index;
          weaponSlot.Decorator.EnableShootAnimation = false;
          weaponSlot.Decorator.DefaultPosition = Vector3.zero;
          this._avatar.AssignWeapon(loadoutSlotTypeArray[index], weaponSlot.Decorator);
        }
        else
          Debug.LogError((object) "WeaponDecorator is NULL!");
        this._weaponSlots[index] = weaponSlot;
      }
      else
        this._weaponSlots[index] = (WeaponSlot) null;
    }
    if (this.CurrentSlotIndex < 0 || this._weaponSlots[this.CurrentSlotIndex] == null || !((Object) this._weaponSlots[this.CurrentSlotIndex].Decorator != (Object) null))
      return;
    this._weaponSlots[this.CurrentSlotIndex].Decorator.IsEnabled = true;
  }

  public void SetAvatarDecorator(AvatarDecorator decorator) => this._avatar = decorator;

  public int NextProjectileId() => ProjectileManager.CreateGlobalProjectileID(this.PlayerNumber, ++this._projectileId);

  public byte PlayerNumber => this._config.State.PlayerNumber;

  public Vector3 ShootingPoint => this._config.State.ShootingPoint;

  public Vector3 ShootingDirection => this._config.State.ShootingDirection;

  public bool IsLocal => false;
}
