// Decompiled with JetBrains decompiler
// Type: QuickItemController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class QuickItemController : Singleton<QuickItemController>
{
  private const float CooldownTime = 0.5f;
  private QuickItem[] _quickItems;
  private bool _isEnabled;
  public bool IsQuickItemMobilePushed;

  private QuickItemController()
  {
    this._quickItems = new QuickItem[LoadoutManager.QuickSlots.Length];
    this.Restriction = new QuickItemRestriction();
    Singleton<QuickItemEventListener>.Instance.Initialize();
    CmuneEventHandler.AddListener<OnSetPlayerTeamEvent>((Action<OnSetPlayerTeamEvent>) (ev => this.UpdateHudSlot(ev.TeamId)));
    CmuneEventHandler.AddListener<InputChangeEvent>(new Action<InputChangeEvent>(this.OnInputChanged));
  }

  public bool IsEnabled
  {
    get => this._isEnabled && !GameState.CurrentGame.IsWaitingForPlayers && GameState.LocalCharacter.IsAlive;
    set => this._isEnabled = value;
  }

  public bool IsCharging { get; set; }

  public bool IsConsumptionEnabled { get; set; }

  public int CurrentSlotIndex { get; private set; }

  public float NextCooldownFinishTime { get; set; }

  public QuickItemRestriction Restriction { get; private set; }

  public void Initialize()
  {
    this.Clear();
    for (int index1 = 0; index1 < LoadoutManager.QuickSlots.Length; ++index1)
    {
      LoadoutSlotType quickSlot = LoadoutManager.QuickSlots[index1];
      InventoryItem inventoryItem;
      if (Singleton<LoadoutManager>.Instance.TryGetItemInSlot(quickSlot, out inventoryItem) && inventoryItem.Item is QuickItem)
      {
        QuickItem original = inventoryItem.Item as QuickItem;
        if ((bool) (UnityEngine.Object) original)
        {
          this._quickItems[index1] = UnityEngine.Object.Instantiate((UnityEngine.Object) original) as QuickItem;
          this._quickItems[index1].Configuration = original.Configuration;
          this._quickItems[index1].gameObject.SetActive(true);
          for (int index2 = 0; index2 < this._quickItems[index1].gameObject.transform.childCount; ++index2)
            this._quickItems[index1].gameObject.transform.GetChild(index2).gameObject.SetActive(false);
          this._quickItems[index1].gameObject.name = "QI - " + original.Configuration.Name;
          this._quickItems[index1].transform.parent = GameState.LocalPlayer.WeaponAttachPoint;
        }
        if ((UnityEngine.Object) this._quickItems[index1] != (UnityEngine.Object) null)
        {
          if (this._quickItems[index1].Configuration.RechargeTime <= 0)
          {
            int index = index1;
            this._quickItems[index1].Behaviour.OnActivated += (Action) (() =>
            {
              this.UseConsumableItem(inventoryItem);
              this.Restriction.DecreaseUse(index);
              this.NextCooldownFinishTime = Time.time + 0.5f;
            });
            this.Restriction.InitializeSlot(index1, this._quickItems[index1], inventoryItem.AmountRemaining);
          }
          else
            this._quickItems[index1].Behaviour.CurrentAmount = this._quickItems[index1].Configuration.AmountRemaining;
          this._quickItems[index1].Behaviour.FocusKey = this.GetFocusKey(quickSlot);
          Singleton<WeaponsHud>.Instance.SetQuickItemCurrentAmount(index1, this._quickItems[index1].Behaviour.CurrentAmount);
          Singleton<WeaponsHud>.Instance.SetQuickItemCooldownMax(index1, this._quickItems[index1].Behaviour.CoolDownTimeTotal);
          Singleton<WeaponsHud>.Instance.SetQuickItemRechargingMax(index1, this._quickItems[index1].Behaviour.ChargingTimeTotal);
        }
        if (this._quickItems[index1] is IGrenadeProjectile quickItem)
          quickItem.OnProjectileEmitted += (Action<IGrenadeProjectile>) (p =>
          {
            Singleton<ProjectileManager>.Instance.AddProjectile((IProjectile) p, Singleton<WeaponController>.Instance.NextProjectileId());
            GameState.CurrentGame.EmitQuickItem(p.Position, p.Velocity, inventoryItem.Item.ItemId, GameState.LocalCharacter.PlayerNumber, p.ID);
          });
      }
      else
        this.Restriction.InitializeSlot(index1);
    }
    this.UpdateHudSlot(GameState.LocalCharacter.TeamID);
    this.ResetSlotSelection();
    Singleton<WeaponsHud>.Instance.QuickItems.Collapse();
  }

  public void ResetSlotSelection()
  {
    if (this._quickItems.Length > 0)
    {
      this.CurrentSlotIndex = 0;
      if (!this.IsSlotAvailable(this.CurrentSlotIndex))
        this.CurrentSlotIndex = this.GetNextAvailableSlotIndex(this.CurrentSlotIndex);
    }
    Singleton<WeaponsHud>.Instance.QuickItems.SetSelected(this.CurrentSlotIndex);
  }

  public void UpdateQuickSlotAmount()
  {
    for (int slot = 0; slot < this._quickItems.Length; ++slot)
    {
      if ((UnityEngine.Object) this._quickItems[slot] != (UnityEngine.Object) null)
        Singleton<WeaponsHud>.Instance.SetQuickItemCurrentAmount(slot, this._quickItems[slot].Behaviour.CurrentAmount);
    }
  }

  public void UseQuickItem(LoadoutSlotType slot) => this.UseQuickItem(this.GetSlotIndex(slot));

  private void UseQuickItem(int index)
  {
    if (!this.IsEnabled || this.IsCharging || (double) Time.time < (double) this.NextCooldownFinishTime)
      return;
    if (this._quickItems != null && index >= 0 && (UnityEngine.Object) this._quickItems[index] != (UnityEngine.Object) null)
    {
      if (!this._quickItems[index].Behaviour.Run() || !((UnityEngine.Object) GameState.LocalPlayer.Character != (UnityEngine.Object) null))
        return;
      SfxManager.Play2dAudioClip(GameAudio.WeaponSwitch);
    }
    else
      Debug.LogError((object) ("The QuickItem has no Behaviour: " + (object) index));
  }

  public void Update()
  {
    if (this._quickItems == null)
      return;
    for (int slot = 0; slot < this._quickItems.Length; ++slot)
    {
      if ((UnityEngine.Object) this._quickItems[slot] != (UnityEngine.Object) null)
      {
        Singleton<WeaponsHud>.Instance.SetQuickItemCooldown(slot, this._quickItems[slot].Behaviour.CoolDownTimeRemaining);
        Singleton<WeaponsHud>.Instance.SetQuickItemRecharging(slot, this._quickItems[slot].Behaviour.ChargingTimeRemaining);
      }
    }
  }

  private void OnInputChanged(InputChangeEvent ev)
  {
    if (ev.IsDown && !LevelCamera.Instance.IsZoomedIn && this.IsEnabled)
    {
      GameInputKey key = ev.Key;
      switch (key)
      {
        case GameInputKey.UseQuickItem:
          this.UseQuickItem(this.CurrentSlotIndex);
          break;
        case GameInputKey.NextQuickItem:
          if (this._quickItems.Length > 0)
          {
            this.CurrentSlotIndex = this.GetNextAvailableSlotIndex(this.CurrentSlotIndex);
            Singleton<WeaponsHud>.Instance.QuickItems.SetSelected(this.CurrentSlotIndex);
            break;
          }
          break;
        case GameInputKey.PrevQuickItem:
          if (this._quickItems.Length > 0)
          {
            this.CurrentSlotIndex = this.GetPrevAvailableSlotIndex(this.CurrentSlotIndex);
            Singleton<WeaponsHud>.Instance.QuickItems.SetSelected(this.CurrentSlotIndex, false);
            break;
          }
          break;
        default:
          switch (key - 16)
          {
            case GameInputKey.None:
              this.UseQuickItem(LoadoutSlotType.QuickUseItem1);
              break;
            case GameInputKey.HorizontalLook:
              this.UseQuickItem(LoadoutSlotType.QuickUseItem2);
              break;
            case GameInputKey.VerticalLook:
              this.UseQuickItem(LoadoutSlotType.QuickUseItem3);
              break;
          }
          break;
      }
    }
    if (ev.Key != GameInputKey.UseQuickItem || LevelCamera.Instance.IsZoomedIn || !this.IsEnabled)
      return;
    this.IsQuickItemMobilePushed = ev.IsDown;
  }

  private int GetNextAvailableSlotIndex(int currentSlot)
  {
    for (int slot = (currentSlot + 1) % this._quickItems.Length; slot != currentSlot; slot = (slot + 1) % this._quickItems.Length)
    {
      if (!Singleton<WeaponsHud>.Instance.QuickItems.GetLoadoutQuickItemHud(slot).IsEmpty)
        return slot;
    }
    return currentSlot;
  }

  private int GetPrevAvailableSlotIndex(int currentSlot)
  {
    for (int slot = (currentSlot - 1) % this._quickItems.Length; slot != currentSlot; slot = (slot - 1) % this._quickItems.Length)
    {
      if (slot < 0)
        slot = this._quickItems.Length - 1;
      if (!Singleton<WeaponsHud>.Instance.QuickItems.GetLoadoutQuickItemHud(slot).IsEmpty)
        return slot;
    }
    return currentSlot;
  }

  private void UpdateHudSlot(TeamID teamId)
  {
    for (int slot = 0; slot < this._quickItems.Length; ++slot)
    {
      QuickItem quickItem = this._quickItems[slot];
      Singleton<WeaponsHud>.Instance.QuickItems.ConfigureQuickItem(slot, quickItem, teamId);
    }
    Singleton<WeaponsHud>.Instance.QuickItems.SetSelected(this.CurrentSlotIndex);
  }

  private bool IsSlotAvailable(int slotIndex) => slotIndex >= 0 && slotIndex < this._quickItems.Length && (UnityEngine.Object) this._quickItems[slotIndex] != (UnityEngine.Object) null;

  private void UseConsumableItem(InventoryItem inventoryItem)
  {
    if (!this.IsConsumptionEnabled)
      return;
    ShopWebServiceClient.UseConsumableItem(PlayerDataManager.CmidSecure, inventoryItem.Item.ItemId, (Action<bool>) null, (Action<Exception>) null);
    --inventoryItem.AmountRemaining;
    if (inventoryItem.AmountRemaining != 0)
      return;
    MonoRoutine.Start(Singleton<ItemManager>.Instance.StartGetInventory(false));
  }

  private LoadoutSlotType GetSlotType(int index) => (LoadoutSlotType) (12 + index);

  private GameInputKey GetFocusKey(LoadoutSlotType slot)
  {
    switch (slot)
    {
      case LoadoutSlotType.QuickUseItem1:
        return GameInputKey.QuickItem1;
      case LoadoutSlotType.QuickUseItem2:
        return GameInputKey.QuickItem2;
      case LoadoutSlotType.QuickUseItem3:
        return GameInputKey.QuickItem3;
      default:
        return GameInputKey.None;
    }
  }

  private int GetSlotIndex(LoadoutSlotType slot)
  {
    switch (slot)
    {
      case LoadoutSlotType.QuickUseItem1:
        return 0;
      case LoadoutSlotType.QuickUseItem2:
        return 1;
      case LoadoutSlotType.QuickUseItem3:
        return 2;
      default:
        return -1;
    }
  }

  internal void Reset()
  {
  }

  internal void Clear()
  {
    for (int index = 0; index < this._quickItems.Length; ++index)
    {
      if ((UnityEngine.Object) this._quickItems[index] != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this._quickItems[index]);
      this._quickItems[index] = (QuickItem) null;
    }
  }
}
