// Decompiled with JetBrains decompiler
// Type: TrainingState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;

internal class TrainingState : IState
{
  private const HudDrawFlags _hudDrawFlag = HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.StateMsg;
  private TrainingFpsMode _trainingGameMode;
  private StateMachine _stateMachine;

  public TrainingState()
  {
    this._stateMachine = new StateMachine();
    this._stateMachine.RegisterState(19, (IState) new InGamePregameLoadoutState());
    this._stateMachine.RegisterState(18, (IState) new InGamePlayingState(this._stateMachine, HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.StateMsg));
    this._stateMachine.RegisterState(23, (IState) new InGamePlayerKilledState(this._stateMachine, HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.StateMsg, false));
    this._stateMachine.RegisterState(26, (IState) new InGamePlayerPausedState(this._stateMachine));
  }

  public int MapId { get; set; }

  public void OnEnter()
  {
    this._trainingGameMode = new TrainingFpsMode(GameConnectionManager.Rmi, this.MapId);
    GameState.CurrentGame = (FpsGameMode) this._trainingGameMode;
    LevelCamera.Instance.SetLevelCamera(GameState.CurrentSpace.Camera, GameState.CurrentSpace.DefaultViewPoint.position, GameState.CurrentSpace.DefaultViewPoint.rotation);
    GameState.LocalPlayer.SetPlayerControlState(LocalPlayer.PlayerState.None);
    GameState.LocalPlayer.SetEnabled(true);
    CmuneEventHandler.AddListener<OnModeInitializedEvent>(new Action<OnModeInitializedEvent>(this.OnModeStart));
    Singleton<HudUtil>.Instance.SetPlayerTeam(TeamID.NONE);
    Singleton<QuickItemController>.Instance.IsConsumptionEnabled = false;
    Singleton<QuickItemController>.Instance.Restriction.IsEnabled = false;
    ProjectileManager.CreateContainer();
    this._stateMachine.SetState(19);
  }

  public void OnExit()
  {
    this._stateMachine.PopAllStates();
    CmuneEventHandler.RemoveListener<OnModeInitializedEvent>(new Action<OnModeInitializedEvent>(this.OnModeStart));
    GameModeUtil.OnExitGameMode();
    this._trainingGameMode = (TrainingFpsMode) null;
  }

  public void OnUpdate()
  {
    if (!this._trainingGameMode.IsMatchRunning)
      return;
    Singleton<QuickItemController>.Instance.Update();
    if (this._trainingGameMode.IsWaitingForSpawn)
      GameModeUtil.UpdateWaitingForSpawnMsg((FpsGameMode) this._trainingGameMode, false);
    this._stateMachine.Update();
  }

  public void OnGUI()
  {
    if (!this._trainingGameMode.IsMatchRunning)
      return;
    this._stateMachine.OnGUI();
  }

  private void OnModeStart(OnModeInitializedEvent ev)
  {
    this._stateMachine.SetState(18);
    GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Playing);
    Singleton<HudUtil>.Instance.ClearAllFeedbackHud();
    Singleton<FrameRateHud>.Instance.Enable = true;
    this.ShowTrainingGameMessages();
    Singleton<HudUtil>.Instance.AddInGameEvent(GameState.LocalCharacter.PlayerName, LocalizedStrings.EnteredTrainingMode);
  }

  private void ShowTrainingGameMessages()
  {
    if (ApplicationDataManager.IsMobile)
      return;
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Empty);
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, LocalizedStrings.TrainingTutorialMsg01);
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, LocalizedStrings.MessageQuickItemsTry);
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, LocalizedStrings.TrainingTutorialMsg03);
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, LocalizedStrings.TrainingTutorialMsg04);
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Format(LocalizedStrings.TrainingTutorialMsg05, (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.Forward), (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.Left), (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.Backward), (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.Right)));
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Format(LocalizedStrings.TrainingTutorialMsg06, (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.PrimaryFire)));
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Format(LocalizedStrings.TrainingTutorialMsg07, (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.NextWeapon), (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.PrevWeapon)));
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Format(LocalizedStrings.TrainingTutorialMsg08, (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.WeaponMelee), (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.Weapon1), (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.Weapon2), (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.Weapon3), (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.Weapon4)));
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Format(LocalizedStrings.TrainingTutorialMsg09, (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.Crouch)));
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Format(LocalizedStrings.TrainingTutorialMsg10, (object) AutoMonoBehaviour<InputManager>.Instance.InputChannelForSlot(GameInputKey.Fullscreen)));
    Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, LocalizedStrings.TrainingTutorialMsg11);
  }
}
