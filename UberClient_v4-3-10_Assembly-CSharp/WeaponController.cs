// Decompiled with JetBrains decompiler
// Type: WeaponController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class WeaponController : Singleton<WeaponController>, IWeaponController
{
  private WeaponSlot[] _weapons;
  private WeaponSlot _weapon;
  private CircularInteger _currentSlotID;
  private bool _isWeaponControlEnabled = true;
  private float _weaponSwitchTimeout;
  private int _pickupWeaponEventCount;
  private float _pickUpWeaponAutoRemovalTime;
  private int _projectileId;
  private LoadoutSlotType _lastLoadoutType = LoadoutSlotType.WeaponPrimary;
  private readonly LoadoutSlotType[] _slotTypes = new LoadoutSlotType[5]
  {
    LoadoutSlotType.WeaponMelee,
    LoadoutSlotType.WeaponPrimary,
    LoadoutSlotType.WeaponSecondary,
    LoadoutSlotType.WeaponTertiary,
    LoadoutSlotType.WeaponPickup
  };
  private Dictionary<GameInputKey, WeaponController.InputEventHandler> _gameInputHandlers = new Dictionary<GameInputKey, WeaponController.InputEventHandler>();
  private WeaponShotCounter _shotCounter;

  private WeaponController()
  {
    this._weapons = new WeaponSlot[5];
    this._shotCounter = new WeaponShotCounter();
    this._currentSlotID = new CircularInteger(0, 3);
    this.InitInputEventHandlers();
    CmuneEventHandler.AddListener<InputChangeEvent>(new Action<InputChangeEvent>(this.OnInputChanged));
  }

  public void NextWeapon()
  {
    if (!this.HasAnyWeapon)
      return;
    if (this._weapon != null && this._weapon.InputHandler != null)
    {
      this._weapon.InputHandler.Stop();
      this._lastLoadoutType = this._weapon.Slot;
      this._weapon = (WeaponSlot) null;
    }
    int next = this._currentSlotID.Next;
    while (this._weapons[next] == null)
      next = this._currentSlotID.Next;
    this.ShowWeapon(this._slotTypes[next]);
  }

  public void PrevWeapon()
  {
    if (!this.HasAnyWeapon)
      return;
    if (this._weapon != null && this._weapon.InputHandler != null)
    {
      this._weapon.InputHandler.Stop();
      this._lastLoadoutType = this._weapon.Slot;
      this._weapon = (WeaponSlot) null;
    }
    int prev = this._currentSlotID.Prev;
    while (this._weapons[prev] == null)
      prev = this._currentSlotID.Prev;
    this.ShowWeapon(this._slotTypes[prev]);
  }

  public void ShowFirstWeapon()
  {
    this._currentSlotID.Reset();
    this.NextWeapon();
  }

  public bool ShowWeapon(LoadoutSlotType slot) => this.ShowWeapon(slot, false);

  public bool ShowWeapon(LoadoutSlotType slot, bool force)
  {
    if (HudAssets.Exists)
      Singleton<TemporaryWeaponHud>.Instance.Enabled = false;
    if (!force && this._weapon != null && this._weapon.Slot == slot)
      return false;
    WeaponSlot weapon;
    switch (slot)
    {
      case LoadoutSlotType.WeaponMelee:
        weapon = this._weapons[0];
        if (weapon != null)
        {
          this._currentSlotID.Current = 0;
          Singleton<WeaponsHud>.Instance.SetActiveLoadout(LoadoutSlotType.WeaponMelee);
          if (GameState.HasCurrentPlayer)
          {
            GameState.LocalCharacter.CurrentWeaponSlot = (byte) 0;
            break;
          }
          break;
        }
        break;
      case LoadoutSlotType.WeaponPrimary:
        weapon = this._weapons[1];
        if (weapon != null)
        {
          this._currentSlotID.Current = 1;
          Singleton<WeaponsHud>.Instance.SetActiveLoadout(LoadoutSlotType.WeaponPrimary);
          if (GameState.HasCurrentPlayer)
          {
            GameState.LocalCharacter.CurrentWeaponSlot = (byte) 1;
            break;
          }
          break;
        }
        break;
      case LoadoutSlotType.WeaponSecondary:
        weapon = this._weapons[2];
        if (weapon != null)
        {
          this._currentSlotID.Current = 2;
          Singleton<WeaponsHud>.Instance.SetActiveLoadout(LoadoutSlotType.WeaponSecondary);
          if (GameState.HasCurrentPlayer)
          {
            GameState.LocalCharacter.CurrentWeaponSlot = (byte) 2;
            break;
          }
          break;
        }
        break;
      case LoadoutSlotType.WeaponTertiary:
        weapon = this._weapons[3];
        if (weapon != null)
        {
          this._currentSlotID.Current = 3;
          Singleton<WeaponsHud>.Instance.SetActiveLoadout(LoadoutSlotType.WeaponTertiary);
          if (GameState.HasCurrentPlayer)
          {
            GameState.LocalCharacter.CurrentWeaponSlot = (byte) 3;
            break;
          }
          break;
        }
        break;
      case LoadoutSlotType.WeaponPickup:
        weapon = this._weapons[4];
        if (weapon != null)
        {
          this._currentSlotID.Current = 4;
          Singleton<WeaponsHud>.Instance.SetActiveLoadout(LoadoutSlotType.WeaponPickup);
          if (GameState.HasCurrentPlayer)
            GameState.LocalCharacter.CurrentWeaponSlot = (byte) 4;
          if (this.TimeLeftForPickUpWeapon > 0 && HudAssets.Exists)
          {
            Singleton<TemporaryWeaponHud>.Instance.Enabled = true;
            Singleton<TemporaryWeaponHud>.Instance.StartCounting(30);
            Singleton<TemporaryWeaponHud>.Instance.RemainingSeconds = this.TimeLeftForPickUpWeapon;
            break;
          }
          break;
        }
        break;
      default:
        weapon = this._weapons[1];
        if (weapon != null)
        {
          this._currentSlotID.Current = 1;
          Singleton<WeaponsHud>.Instance.SetActiveLoadout(LoadoutSlotType.WeaponPrimary);
          if (GameState.HasCurrentPlayer)
          {
            GameState.LocalCharacter.CurrentWeaponSlot = (byte) 1;
            break;
          }
          break;
        }
        break;
    }
    if (weapon != null)
    {
      this._weaponSwitchTimeout = Time.time + 0.2f;
      this._weapon = weapon;
      this.UpdateAmmoHUD();
      if (this._weapon.Logic != null && (UnityEngine.Object) this._weapon.Decorator != (UnityEngine.Object) null)
      {
        WeaponFeedbackManager.Instance.PickUp(this._weapon.Logic, this._weapon.Decorator);
        this._weapon.Decorator.PlayEquipSound();
      }
      else
        UnityEngine.Debug.LogError((object) ("Failed to show weapon: logic is null = " + (object) (this._weapon.Logic == null) + " decorator is null = " + (object) ((UnityEngine.Object) this._weapon.Decorator == (UnityEngine.Object) null)));
      return true;
    }
    return !this.HasAnyWeapon && false;
  }

  public void UpdateAmmoHUD()
  {
    if (this._weapon == null || !HudAssets.Exists)
      return;
    Singleton<AmmoHud>.Instance.Ammo = AmmoDepot.AmmoOfClass(this._weapon.Item.ItemClass);
  }

  public void PutdownCurrentWeapon() => WeaponFeedbackManager.Instance.PutDown();

  public void PickupCurrentWeapon()
  {
    if (this._weapon == null)
      return;
    WeaponFeedbackManager.Instance.PickUp(this._weapon.Logic, this._weapon.Decorator);
  }

  public bool Shoot()
  {
    bool flag = false;
    if (this.IsWeaponReady && GameState.HasCurrentPlayer)
    {
      this._weapon.NextShootTime = Time.time + WeaponConfigurationHelper.GetRateOfFire((UberStrikeItemWeaponView) this._weapon.Logic.Config);
      if (AmmoDepot.HasAmmoOfClass(this._weapon.Item.ItemClass))
      {
        Ray ray = new Ray(GameState.LocalCharacter.ShootingPoint + LocalPlayer.EyePosition, GameState.LocalCharacter.ShootingDirection);
        this._shotCounter.IncreaseShotCount(this._weapon.Item.ItemClass);
        this._weapon.Logic.Shoot(ray, out CmunePairList<BaseGameProp, ShotPoint> _);
        if (!this._weapon.Decorator.HasShootAnimation)
          WeaponFeedbackManager.Instance.Fire();
        AmmoDepot.UseAmmoOfClass(this._weapon.Item.ItemClass);
        this.UpdateAmmoHUD();
        if (HudAssets.Exists)
          Singleton<ReticleHud>.Instance.TriggerReticle(this._weapon.Item.ItemClass);
        flag = true;
      }
      else
      {
        this._weapon.Decorator.PlayOutOfAmmoSound();
        GameState.LocalCharacter.IsFiring = false;
      }
    }
    return flag;
  }

  public WeaponSlot GetPrimaryWeapon() => this._weapons[1];

  public WeaponSlot GetCurrentWeapon() => this._weapon;

  public WeaponSlot GetPickupWeapon() => this._weapons[4];

  public void InitializeAllWeapons(Transform _weaponAttachPoint)
  {
    for (int index = 0; index < this._weapons.Length; ++index)
    {
      if (this._weapons[index] != null && (UnityEngine.Object) this._weapons[index].Decorator != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this._weapons[index].Decorator.gameObject);
      this._weapons[index] = (WeaponSlot) null;
    }
    WeaponInfo.SlotType[] slotTypeArray = new WeaponInfo.SlotType[4]
    {
      WeaponInfo.SlotType.Melee,
      WeaponInfo.SlotType.Primary,
      WeaponInfo.SlotType.Secondary,
      WeaponInfo.SlotType.Tertiary
    };
    for (int index = 0; index < LoadoutManager.WeaponSlots.Length; ++index)
    {
      LoadoutSlotType weaponSlot = LoadoutManager.WeaponSlots[index];
      InventoryItem inventoryItem;
      if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(weaponSlot, out inventoryItem))
      {
        WeaponItem t = inventoryItem.Item as WeaponItem;
        WeaponSlot weapon = new WeaponSlot(weaponSlot, t, _weaponAttachPoint, (IWeaponController) this);
        this.AddGameLogicToWeapon(weapon);
        if ((bool) (UnityEngine.Object) weapon.Decorator)
        {
          weapon.Decorator.EnableShootAnimation = true;
          weapon.Decorator.IronSightPosition = t.Configuration.IronSightPosition;
        }
        this._weapons[index] = weapon;
        AmmoDepot.SetMaxAmmoForType(t, t.Configuration.MaxAmmo);
        AmmoDepot.SetStartAmmoForType(t, t.Configuration.StartAmmo);
        if (GameState.HasCurrentPlayer)
          GameState.LocalCharacter.Weapons.SetWeaponSlot(slotTypeArray[index], t.ItemId, t.ItemClass);
        if (HudAssets.Exists)
          Singleton<WeaponsHud>.Instance.Weapons.SetSlotWeapon(weaponSlot, inventoryItem.Item as WeaponItem);
      }
      else if (GameState.HasCurrentPlayer)
      {
        GameState.LocalCharacter.Weapons.SetWeaponSlot(slotTypeArray[index], 0, (UberstrikeItemClass) 0);
        if (HudAssets.Exists)
          Singleton<WeaponsHud>.Instance.Weapons.SetSlotWeapon(weaponSlot, (WeaponItem) null);
      }
    }
    if (GameState.HasCurrentPlayer)
      GameState.LocalCharacter.Weapons.SetWeaponSlot(WeaponInfo.SlotType.Pickup, 0, (UberstrikeItemClass) 0);
    Singleton<QuickItemController>.Instance.Initialize();
    if (HudAssets.Exists)
      Singleton<WeaponsHud>.Instance.Weapons.SetSlotWeapon(LoadoutSlotType.WeaponPickup, (WeaponItem) null);
    this.Reset();
  }

  public void ResetPickupWeaponSlotInSeconds(int seconds)
  {
    if (seconds <= 0)
      this._pickUpWeaponAutoRemovalTime = 0.0f;
    else
      this._pickUpWeaponAutoRemovalTime = Time.time + (float) seconds;
  }

  public void Reset()
  {
    AmmoDepot.Reset();
    this._currentSlotID.SetRange(0, 3);
    this._weapon = (WeaponSlot) null;
    this._shotCounter.Reset();
    this.ShowFirstWeapon();
    this.ResetPickupWeaponSlotInSeconds(0);
  }

  public void SetPickupWeapon(int weaponID) => this.SetPickupWeapon(weaponID, true, false);

  public void SetPickupWeapon(int weaponID, bool uniqueWeaponClass, bool forceAutoEquip)
  {
    ++this._pickupWeaponEventCount;
    WeaponItem weaponItemInShop = Singleton<ItemManager>.Instance.GetWeaponItemInShop(weaponID);
    if ((UnityEngine.Object) weaponItemInShop != (UnityEngine.Object) null)
    {
      if (GameState.HasCurrentPlayer)
      {
        if (!GameState.LocalCharacter.Weapons.ItemIDs.Contains(weaponItemInShop.ItemId))
        {
          bool flag = true;
          for (int index = 0; index < 4; ++index)
          {
            if (this._weapons[index] != null && this._weapons[index].Item.ItemClass == weaponItemInShop.ItemClass)
              flag = false;
          }
          if (flag || !uniqueWeaponClass)
          {
            if (this._weapons[4] != null && (UnityEngine.Object) this._weapons[4].Decorator != (UnityEngine.Object) null)
            {
              this._weapons[4].InputHandler.Stop();
              UnityEngine.Object.Destroy((UnityEngine.Object) this._weapons[4].Decorator.gameObject);
            }
            WeaponSlot weapon = new WeaponSlot(LoadoutSlotType.WeaponPickup, weaponItemInShop, GameState.LocalPlayer.WeaponAttachPoint, (IWeaponController) this);
            this.AddGameLogicToWeapon(weapon);
            if ((bool) (UnityEngine.Object) weapon.Decorator)
            {
              weapon.Decorator.EnableShootAnimation = true;
              weapon.Decorator.IronSightPosition = weaponItemInShop.Configuration.IronSightPosition;
            }
            int current = this._currentSlotID.Current;
            this._currentSlotID.SetRange(0, 4);
            this._currentSlotID.Current = current;
            if (HudAssets.Exists)
            {
              Singleton<WeaponsHud>.Instance.Weapons.SetSlotWeapon(LoadoutSlotType.WeaponPickup, weapon.Item);
              Singleton<AmmoHud>.Instance.Ammo = AmmoDepot.AmmoOfClass(weaponItemInShop.ItemClass);
            }
            Singleton<LoadoutManager>.Instance.EquipWeapon(LoadoutSlotType.WeaponPickup, weaponItemInShop);
            GameState.LocalCharacter.Weapons.SetWeaponSlot(WeaponInfo.SlotType.Pickup, weaponItemInShop.ItemId, weaponItemInShop.ItemClass);
            AmmoDepot.SetMaxAmmoForType(weapon.Item, weapon.Item.Configuration.MaxAmmo);
            AmmoDepot.SetStartAmmoForType(weapon.Item, weapon.Item.Configuration.StartAmmo);
            this._weapons[4] = weapon;
            if (this._weapon == null || forceAutoEquip || ApplicationDataManager.ApplicationOptions.GameplayAutoEquipEnabled || this._currentSlotID.Current == 4)
              this.ShowWeapon(LoadoutSlotType.WeaponPickup, true);
          }
        }
      }
      else
        UnityEngine.Debug.LogError((object) "SetPickupWeapon failed because no player defined yet");
      AmmoDepot.AddAmmoOfClass(weaponItemInShop.ItemClass);
      this.UpdateAmmoHUD();
    }
    else
      this.ResetPickupSlot();
  }

  public void SetPickupWeapon(WeaponItem item)
  {
    WeaponSlot weapon = new WeaponSlot(LoadoutSlotType.WeaponPickup, item, GameState.LocalPlayer.WeaponAttachPoint, (IWeaponController) this);
    this.AddGameLogicToWeapon(weapon);
    if ((bool) (UnityEngine.Object) weapon.Decorator)
    {
      weapon.Decorator.EnableShootAnimation = true;
      weapon.Decorator.IronSightPosition = item.Configuration.IronSightPosition;
    }
    int current = this._currentSlotID.Current;
    this._currentSlotID.SetRange(0, 4);
    this._currentSlotID.Current = current;
    if (HudAssets.Exists)
    {
      Singleton<WeaponsHud>.Instance.Weapons.SetSlotWeapon(LoadoutSlotType.WeaponPickup, weapon.Item);
      Singleton<AmmoHud>.Instance.Ammo = AmmoDepot.AmmoOfClass(item.ItemClass);
    }
    Singleton<LoadoutManager>.Instance.EquipWeapon(LoadoutSlotType.WeaponPickup, item);
    GameState.LocalCharacter.Weapons.SetWeaponSlot(WeaponInfo.SlotType.Pickup, item.ItemId, item.ItemClass);
    AmmoDepot.SetMaxAmmoForType(weapon.Item, weapon.Item.Configuration.MaxAmmo);
    AmmoDepot.SetStartAmmoForType(weapon.Item, weapon.Item.Configuration.StartAmmo);
    this._weapons[4] = weapon;
    this.ShowWeapon(LoadoutSlotType.WeaponPickup, true);
  }

  public void ResetPickupSlot()
  {
    if (HudAssets.Exists)
      Singleton<TemporaryWeaponHud>.Instance.Enabled = false;
    if (this._weapons[4] == null || !((UnityEngine.Object) this._weapons[4].Decorator != (UnityEngine.Object) null))
      return;
    this._weapons[4].InputHandler.Stop();
    WeaponItem sameClassWeapon = (WeaponItem) null;
    if (this.GetPlayerWeaponOfPickupClass(out sameClassWeapon))
    {
      AmmoDepot.SetMaxAmmoForType(sameClassWeapon, sameClassWeapon.Configuration.MaxAmmo);
      AmmoDepot.RemoveExtraAmmoOfType(sameClassWeapon.ItemClass);
      this.UpdateAmmoHUD();
    }
    MonoRoutine.Start(this.StartHidingWeapon(this._weapons[4].Decorator.gameObject, true));
    if (this._weapon != null && this._weapon.Slot == LoadoutSlotType.WeaponPickup)
      WeaponFeedbackManager.Instance.PutDown();
    int current = this._currentSlotID.Current;
    this._currentSlotID.SetRange(0, 3);
    if (current != 4)
      this._currentSlotID.Current = current;
    this._weapons[4] = (WeaponSlot) null;
    if (HudAssets.Exists)
      Singleton<WeaponsHud>.Instance.Weapons.SetSlotWeapon(LoadoutSlotType.WeaponPickup, (WeaponItem) null);
    if (current != 4)
      return;
    if (HudAssets.Exists)
      Singleton<WeaponsHud>.Instance.ResetActiveWeapon();
    this.ShowWeapon(this._lastLoadoutType);
  }

  public bool HasWeaponOfClass(UberstrikeItemClass itemClass)
  {
    for (int index = 0; index < 5; ++index)
    {
      WeaponSlot weapon = this._weapons[index];
      if (weapon != null && weapon.HasWeapon && weapon.Item.ItemClass == itemClass)
        return true;
    }
    return false;
  }

  public bool CheckPlayerWeaponInPickupSlot(int id) => this._weapons[4] != null && this._weapons[4].HasWeapon && this._weapons[4].Item.ItemId == id;

  public void StopInputHandler()
  {
    if (this._weapon == null)
      return;
    this._weapon.InputHandler.Stop();
  }

  public int NextProjectileId() => ProjectileManager.CreateGlobalProjectileID(this.PlayerNumber, ++this._projectileId);

  public WeaponShotCounter ShotCounter => this._shotCounter;

  public byte PlayerNumber => GameState.LocalCharacter.PlayerNumber;

  public bool IsLocal => true;

  public Vector3 ShootingPoint => GameState.LocalCharacter.ShootingPoint;

  public Vector3 ShootingDirection => GameState.LocalCharacter.ShootingDirection;

  public bool HasAnyWeapon
  {
    get
    {
      foreach (WeaponSlot weapon in this._weapons)
      {
        if (weapon != null)
          return true;
      }
      return false;
    }
  }

  public BaseWeaponDecorator CurrentDecorator => this.IsWeaponValid ? this._weapon.Decorator : (BaseWeaponDecorator) null;

  public bool IsWeaponValid => this._weapon != null && this._weapon.Logic != null && (UnityEngine.Object) this._weapon.Decorator != (UnityEngine.Object) null;

  public bool IsWeaponReady => this.IsWeaponValid && (double) this._weapon.NextShootTime < (double) Time.time && this._weapon.Logic.IsWeaponActive;

  public bool IsSecondaryAction => this._weapon != null && !this._weapon.InputHandler.CanChangeWeapon();

  public bool IsEnabled
  {
    get => this._isWeaponControlEnabled && AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled;
    set => this._isWeaponControlEnabled = value;
  }

  public int TimeLeftForPickUpWeapon => (double) this._pickUpWeaponAutoRemovalTime > (double) Time.time ? Mathf.RoundToInt(this._pickUpWeaponAutoRemovalTime - Time.time) : -1;

  public LoadoutSlotType CurrentSlot => this._weapon != null ? this._weapon.Slot : LoadoutSlotType.None;

  private void OnInputChanged(InputChangeEvent ev)
  {
    WeaponController.InputEventHandler inputEventHandler;
    if (!GameState.HasCurrentPlayer || !this.IsEnabled || !GameState.LocalCharacter.IsAlive || !this._gameInputHandlers.TryGetValue(ev.Key, out inputEventHandler))
      return;
    inputEventHandler.Callback(ev, inputEventHandler.SlotType);
  }

  private void InitInputEventHandlers()
  {
    this._gameInputHandlers.Add(GameInputKey.WeaponMelee, new WeaponController.InputEventHandler(LoadoutSlotType.WeaponMelee, new Action<InputChangeEvent, LoadoutSlotType>(this.SelectWeaponCallback)));
    this._gameInputHandlers.Add(GameInputKey.Weapon1, new WeaponController.InputEventHandler(LoadoutSlotType.WeaponPrimary, new Action<InputChangeEvent, LoadoutSlotType>(this.SelectWeaponCallback)));
    this._gameInputHandlers.Add(GameInputKey.Weapon2, new WeaponController.InputEventHandler(LoadoutSlotType.WeaponSecondary, new Action<InputChangeEvent, LoadoutSlotType>(this.SelectWeaponCallback)));
    this._gameInputHandlers.Add(GameInputKey.Weapon3, new WeaponController.InputEventHandler(LoadoutSlotType.WeaponTertiary, new Action<InputChangeEvent, LoadoutSlotType>(this.SelectWeaponCallback)));
    this._gameInputHandlers.Add(GameInputKey.Weapon4, new WeaponController.InputEventHandler(LoadoutSlotType.WeaponPickup, new Action<InputChangeEvent, LoadoutSlotType>(this.SelectWeaponCallback)));
    this._gameInputHandlers.Add(GameInputKey.PrevWeapon, new WeaponController.InputEventHandler(LoadoutSlotType.None, new Action<InputChangeEvent, LoadoutSlotType>(this.PrevWeaponCallback)));
    this._gameInputHandlers.Add(GameInputKey.NextWeapon, new WeaponController.InputEventHandler(LoadoutSlotType.None, new Action<InputChangeEvent, LoadoutSlotType>(this.NextWeaponCallback)));
    this._gameInputHandlers.Add(GameInputKey.PrimaryFire, new WeaponController.InputEventHandler(LoadoutSlotType.None, new Action<InputChangeEvent, LoadoutSlotType>(this.PrimaryFireCallback)));
    this._gameInputHandlers.Add(GameInputKey.SecondaryFire, new WeaponController.InputEventHandler(LoadoutSlotType.None, new Action<InputChangeEvent, LoadoutSlotType>(this.SecondaryFireCallback)));
  }

  private void SelectWeaponCallback(InputChangeEvent ev, LoadoutSlotType slotType)
  {
    if (!ev.IsDown || LevelCamera.Instance.IsZoomedIn)
      return;
    this.ShowWeapon(slotType);
  }

  private void PrevWeaponCallback(InputChangeEvent ev, LoadoutSlotType slotType)
  {
    if ((this._weapon == null || ev.IsDown && this._weapon.InputHandler.CanChangeWeapon()) && GUITools.SaveClickIn(0.2f))
    {
      GUITools.Clicked();
      this.NextWeapon();
    }
    else
    {
      if (this._weapon == null || !ev.IsDown)
        return;
      this._weapon.InputHandler.OnPrevWeapon();
    }
  }

  private void NextWeaponCallback(InputChangeEvent ev, LoadoutSlotType slotType)
  {
    if ((this._weapon == null || ev.IsDown && this._weapon.InputHandler.CanChangeWeapon()) && GUITools.SaveClickIn(0.2f))
    {
      GUITools.Clicked();
      this.PrevWeapon();
    }
    else
    {
      if (this._weapon == null || !ev.IsDown)
        return;
      this._weapon.InputHandler.OnNextWeapon();
    }
  }

  private void PrimaryFireCallback(InputChangeEvent ev, LoadoutSlotType slotType)
  {
    if (ev.IsDown && this.CanPlayerShoot)
    {
      if (this._weapon == null || !this._weapon.HasWeapon)
        return;
      this._weapon.InputHandler.OnPrimaryFire(true);
    }
    else
    {
      if (this._weapon == null)
        return;
      GameState.LocalCharacter.IsFiring = false;
      this._weapon.InputHandler.OnPrimaryFire(false);
    }
  }

  private void SecondaryFireCallback(InputChangeEvent ev, LoadoutSlotType slotType)
  {
    if (!GameState.HasCurrentPlayer || !GameState.LocalCharacter.IsAlive || !this.IsEnabled || this._weapon == null || !this._weapon.HasWeapon)
      return;
    this._weapon.InputHandler.OnSecondaryFire(ev.IsDown);
  }

  private bool CanPlayerShoot => GameState.HasCurrentPlayer && this.IsEnabled && GameState.LocalCharacter.IsAlive && (double) this._weaponSwitchTimeout < (double) Time.time;

  public void LateUpdate()
  {
    if (this.CanPlayerShoot)
    {
      if (this._weapon != null && this._weapon.HasWeapon && (double) this._weaponSwitchTimeout < (double) Time.time)
        this._weapon.InputHandler.Update();
      if ((double) this._pickUpWeaponAutoRemovalTime > 0.0 && (double) this._pickUpWeaponAutoRemovalTime < (double) Time.time)
      {
        this._pickUpWeaponAutoRemovalTime = 0.0f;
        if (GameState.HasCurrentPlayer)
          GameState.LocalCharacter.Weapons.SetWeaponSlot(WeaponInfo.SlotType.Pickup, 0, (UberstrikeItemClass) 0);
        this.ResetPickupSlot();
      }
      if (!HudAssets.Exists)
        return;
      Singleton<TemporaryWeaponHud>.Instance.RemainingSeconds = this.TimeLeftForPickUpWeapon;
    }
    else
    {
      if (GameState.HasCurrentPlayer)
        GameState.LocalCharacter.IsFiring = false;
      if (this._weapon == null || this._weapon.InputHandler == null)
        return;
      this._weapon.InputHandler.Stop();
    }
  }

  [DebuggerHidden]
  private IEnumerator StartHidingWeapon(GameObject weapon, bool destroy) => (IEnumerator) new WeaponController.\u003CStartHidingWeapon\u003Ec__Iterator8B()
  {
    destroy = destroy,
    weapon = weapon,
    \u003C\u0024\u003Edestroy = destroy,
    \u003C\u0024\u003Eweapon = weapon
  };

  [DebuggerHidden]
  private IEnumerator StartApplyDamage(float delay, CmunePairList<BaseGameProp, ShotPoint> hits) => (IEnumerator) new WeaponController.\u003CStartApplyDamage\u003Ec__Iterator8C()
  {
    delay = delay,
    hits = hits,
    \u003C\u0024\u003Edelay = delay,
    \u003C\u0024\u003Ehits = hits,
    \u003C\u003Ef__this = this
  };

  private void ApplyDamage(CmunePairList<BaseGameProp, ShotPoint> hits)
  {
    foreach (KeyValuePair<BaseGameProp, ShotPoint> hit in (List<KeyValuePair<BaseGameProp, ShotPoint>>) hits)
    {
      int shotCount = this._shotCounter.GetShotCount(this._weapon.Item.ItemClass);
      DamageInfo shot = new DamageInfo((short) (this._weapon.Logic.Config.DamagePerProjectile * hit.Value.Count))
      {
        Force = GameState.LocalPlayer.WeaponCamera.transform.forward * (float) this._weapon.Logic.Config.DamageKnockback,
        Hitpoint = hit.Value.MidPoint,
        ProjectileID = hit.Value.ProjectileId,
        ShotCount = shotCount,
        WeaponID = this._weapon.Logic.Config.ID,
        WeaponClass = this._weapon.Logic.Config.ItemClass,
        DamageEffectFlag = this._weapon.Logic.Config.DamageEffectFlag,
        DamageEffectValue = this._weapon.Logic.Config.DamageEffectValue,
        CriticalStrikeBonus = WeaponConfigurationHelper.GetCriticalStrikeBonus(this._weapon.Logic.Config)
      };
      switch (shot.WeaponClass)
      {
        case UberstrikeItemClass.WeaponHandgun:
        case UberstrikeItemClass.WeaponSniperRifle:
          if ((double) shot.CriticalStrikeBonus == 0.0)
          {
            shot.CriticalStrikeBonus = 0.5f;
            break;
          }
          break;
      }
      if ((UnityEngine.Object) hit.Key != (UnityEngine.Object) null)
        hit.Key.ApplyDamage(shot);
    }
  }

  private void AddGameLogicToWeapon(WeaponSlot weapon)
  {
    float movement = WeaponConfigurationHelper.GetRecoilMovement((UberStrikeItemWeaponView) weapon.Item.Configuration);
    float kickback = WeaponConfigurationHelper.GetRecoilKickback((UberStrikeItemWeaponView) weapon.Item.Configuration);
    LoadoutSlotType slot = weapon.Slot;
    if (weapon.Logic is ProjectileWeapon)
    {
      ProjectileWeapon w = weapon.Logic as ProjectileWeapon;
      w.OnProjectileShoot += (Action<ProjectileInfo>) (p =>
      {
        ProjectileDetonator projectileDetonator = new ProjectileDetonator(WeaponConfigurationHelper.GetSplashRadius((UberStrikeItemWeaponView) w.Config), (float) w.Config.DamagePerProjectile, w.Config.DamageKnockback, p.Direction, p.Id, w.Config.ID, w.Config.ItemClass, w.Config.DamageEffectFlag, w.Config.DamageEffectValue);
        if ((UnityEngine.Object) p.Projectile != (UnityEngine.Object) null)
        {
          p.Projectile.Detonator = projectileDetonator;
          if (w.Config.ItemClass != UberstrikeItemClass.WeaponSplattergun)
            GameState.CurrentGame.EmitProjectile(p.Position, p.Direction, slot, p.Id, false);
        }
        else
        {
          projectileDetonator.Explode(p.Position);
          GameState.CurrentGame.EmitProjectile(p.Position, p.Direction, slot, p.Id, true);
        }
        if (w.Config.ItemClass == UberstrikeItemClass.WeaponSplattergun)
          GameState.LocalCharacter.IsFiring = true;
        else if (w.HasProjectileLimit)
          Singleton<ProjectileManager>.Instance.AddLimitedProjectile((IProjectile) p.Projectile, p.Id, w.MaxConcurrentProjectiles);
        else
          Singleton<ProjectileManager>.Instance.AddProjectile((IProjectile) p.Projectile, p.Id);
        LevelCamera.Instance.DoFeedback(LevelCamera.FeedbackType.ShootWeapon, Vector3.back, 0.0f, movement / 8f, 0.1f, 0.3f, kickback / 3f, Vector3.left);
      });
    }
    else if (weapon.Logic is MeleeWeapon)
    {
      float delay = weapon.Logic.HitDelay;
      weapon.Logic.OnTargetHit += (Action<CmunePairList<BaseGameProp, ShotPoint>>) (h =>
      {
        if (GameState.LocalCharacter != null)
        {
          if (weapon.Item.Configuration.HasAutomaticFire)
            GameState.LocalCharacter.IsFiring = true;
          else
            GameState.CurrentGame.SingleBulletFire();
        }
        if (h != null)
          MonoRoutine.Start(this.StartApplyDamage(delay, h));
        LevelCamera.Instance.DoFeedback(LevelCamera.FeedbackType.ShootWeapon, Vector3.back, 0.0f, movement / 8f, 0.1f, 0.3f, kickback / 3f, Vector3.left);
      });
    }
    else
      weapon.Logic.OnTargetHit += (Action<CmunePairList<BaseGameProp, ShotPoint>>) (h =>
      {
        if (GameState.LocalCharacter != null)
        {
          if (weapon.Item.Configuration.HasAutomaticFire)
            GameState.LocalCharacter.IsFiring = true;
          else
            GameState.CurrentGame.SingleBulletFire();
        }
        if (h != null)
          this.ApplyDamage(h);
        LevelCamera.Instance.DoFeedback(LevelCamera.FeedbackType.ShootWeapon, Vector3.back, 0.0f, movement / 8f, 0.1f, 0.3f, kickback / 3f, Vector3.left);
      });
  }

  private bool GetPlayerWeaponOfPickupClass(out WeaponItem sameClassWeapon)
  {
    sameClassWeapon = (WeaponItem) null;
    if (this._weapons[4] != null && this._weapons[4].HasWeapon)
    {
      for (int index = 0; index < 4; ++index)
      {
        WeaponSlot weapon = this._weapons[index];
        if (weapon != null && weapon.HasWeapon && weapon.Item.ItemClass == this._weapons[4].Item.ItemClass)
        {
          sameClassWeapon = weapon.Item;
          return true;
        }
      }
    }
    return false;
  }

  private class InputEventHandler
  {
    public InputEventHandler(
      LoadoutSlotType slotType,
      Action<InputChangeEvent, LoadoutSlotType> callback)
    {
      this.SlotType = slotType;
      this.Callback = callback;
    }

    public LoadoutSlotType SlotType { get; private set; }

    public Action<InputChangeEvent, LoadoutSlotType> Callback { get; private set; }
  }
}
