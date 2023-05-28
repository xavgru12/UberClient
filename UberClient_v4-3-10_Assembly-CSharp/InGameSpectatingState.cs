// Decompiled with JetBrains decompiler
// Type: InGameSpectatingState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;

internal class InGameSpectatingState : IState
{
  private HudDrawFlags _gameModeFlag;
  private StateMachine _stateMachine;

  public InGameSpectatingState(StateMachine stateMachine, HudDrawFlags gameModeFlag)
  {
    this._gameModeFlag = gameModeFlag;
    this._stateMachine = stateMachine;
  }

  public void OnEnter()
  {
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag &= ~(HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle);
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag |= HudDrawFlags.InGameHelp;
    CmuneEventHandler.AddListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPaused));
    Singleton<QuickItemController>.Instance.IsEnabled = false;
    if (!GameState.LocalPlayer.IsGamePaused)
      return;
    this._stateMachine.PushState(26);
  }

  public void OnExit()
  {
    GamePageManager.Instance.UnloadCurrentPage();
    CmuneEventHandler.RemoveListener<OnPlayerPauseEvent>(new Action<OnPlayerPauseEvent>(this.OnPlayerPaused));
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag = this._gameModeFlag;
  }

  public void OnUpdate()
  {
  }

  public void OnGUI()
  {
  }

  private void OnPlayerPaused(OnPlayerPauseEvent ev) => this._stateMachine.PushState(26);
}
