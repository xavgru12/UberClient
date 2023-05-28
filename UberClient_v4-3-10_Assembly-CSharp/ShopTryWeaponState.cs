// Decompiled with JetBrains decompiler
// Type: ShopTryWeaponState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.Realtime.UnitySdk;

internal class ShopTryWeaponState : IState
{
  private ShopWeaponMode _shopGameMode;

  public int ItemId { get; set; }

  public void OnEnter()
  {
    CmuneEventHandler.AddListener<OnPlayerRespawnEvent>(new Action<OnPlayerRespawnEvent>(this.OnPlayerRespawn));
    CmuneEventHandler.AddListener<OnMobileBackPressedEvent>(new Action<OnMobileBackPressedEvent>(this.OnMobileBackPressed));
    CmuneEventHandler.AddListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPause));
    this._shopGameMode = new ShopWeaponMode(GameConnectionManager.Rmi);
    GameState.CurrentGame = (FpsGameMode) this._shopGameMode;
    Singleton<SpawnPointManager>.Instance.ConfigureSpawnPoints(GameState.CurrentSpace.SpawnPoints.GetComponentsInChildren<SpawnPoint>(true));
    LevelCamera.Instance.SetLevelCamera(GameState.CurrentSpace.Camera, GameState.CurrentSpace.DefaultViewPoint.position, GameState.CurrentSpace.DefaultViewPoint.rotation);
    GameState.LocalPlayer.SetPlayerControlState(LocalPlayer.PlayerState.None);
    GameState.LocalPlayer.SetEnabled(true);
    Singleton<QuickItemController>.Instance.IsEnabled = true;
    Singleton<QuickItemController>.Instance.IsConsumptionEnabled = false;
    Singleton<QuickItemController>.Instance.Restriction.IsEnabled = false;
    ProjectileManager.CreateContainer();
    this._shopGameMode.InitializeMode();
    MenuPageManager.Instance.UnloadCurrentPage();
    HudController.Instance.XpPtsHud.OnGameStart();
    Singleton<HudUtil>.Instance.SetPlayerTeam(TeamID.NONE);
    Singleton<PlayerStateMsgHud>.Instance.DisplayNone();
    Singleton<FrameRateHud>.Instance.Enable = true;
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag = HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.XpPoints | HudDrawFlags.StateMsg;
    this.ShowShopMessages();
  }

  public void OnExit()
  {
    CmuneEventHandler.RemoveListener<OnPlayerRespawnEvent>(new Action<OnPlayerRespawnEvent>(this.OnPlayerRespawn));
    CmuneEventHandler.RemoveListener<OnMobileBackPressedEvent>(new Action<OnMobileBackPressedEvent>(this.OnMobileBackPressed));
    CmuneEventHandler.RemoveListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPause));
    GameModeUtil.OnExitGameMode();
    this._shopGameMode = (ShopWeaponMode) null;
  }

  public void OnUpdate() => Singleton<QuickItemController>.Instance.Update();

  private void Unload()
  {
    MenuPageManager.Instance.LoadPage(PageType.Shop, true);
    Singleton<GameStateController>.Instance.UnloadGameMode();
  }

  public void OnGUI()
  {
  }

  private void OnPlayerRespawn(OnPlayerRespawnEvent ev) => Singleton<WeaponController>.Instance.SetPickupWeapon(this.ItemId, false, true);

  private void ShowShopMessages()
  {
    if (ApplicationDataManager.IsMobile)
      return;
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Empty);
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, LocalizedStrings.ShopTutorialMsg01);
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, LocalizedStrings.MessageQuickItemsTry);
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, LocalizedStrings.ShopTutorialMsg02, 20f);
  }

  private void OnMobileBackPressed(OnMobileBackPressedEvent ev) => this.QuitTryWeapon();

  private void OnPlayerPause(OnPlayerPauseEvent ev) => this.QuitTryWeapon();

  private void QuitTryWeapon()
  {
    MenuPageManager.Instance.LoadPage(PageType.Shop, true);
    Singleton<GameStateController>.Instance.UnloadGameMode();
    if (this.ItemId > 0)
    {
      BuyPanelGUI buyPanelGui = PanelManager.Instance.OpenPanel(PanelType.BuyItem) as BuyPanelGUI;
      if ((bool) (UnityEngine.Object) buyPanelGui)
        buyPanelGui.SetItem(Singleton<ItemManager>.Instance.GetItemInShop(this.ItemId), BuyingLocationType.Shop, BuyingRecommendationType.None);
    }
    AutoMonoBehaviour<BackgroundMusicPlayer>.Instance.Play(GameAudio.SeletronRadioShort);
  }
}
