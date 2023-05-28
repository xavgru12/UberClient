// Decompiled with JetBrains decompiler
// Type: InGamePlayerKilledState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class InGamePlayerKilledState : IState
{
  private HudDrawFlags _gameModeFlag;
  private bool _showInGameHelp;
  private StateMachine _stateMachine;

  public InGamePlayerKilledState(
    StateMachine stateMachine,
    HudDrawFlags gameModeFlag,
    bool showInGameHelp)
  {
    this._gameModeFlag = gameModeFlag;
    this._showInGameHelp = showInGameHelp;
    this._stateMachine = stateMachine;
  }

  public void OnEnter()
  {
    Singleton<GameModeObjectiveHud>.Instance.Clear();
    Singleton<InGameFeatHud>.Instance.AnimationScheduler.ClearAll();
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag &= ~(HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.Reticle | HudDrawFlags.EventStream);
    if (this._showInGameHelp)
      Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag |= HudDrawFlags.InGameHelp;
    Screen.lockCursor = false;
    Singleton<QuickItemController>.Instance.IsEnabled = false;
    CmuneEventHandler.AddListener<OnPlayerRespawnEvent>(new Action<OnPlayerRespawnEvent>(this.OnPlayerRespawn));
  }

  public void OnExit()
  {
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag = this._gameModeFlag;
    GamePageManager.Instance.UnloadCurrentPage();
    Singleton<HudUtil>.Instance.ClearAllFeedbackHud();
    Singleton<PlayerStateMsgHud>.Instance.ButtonEnabled = false;
    Singleton<QuickItemController>.Instance.IsEnabled = true;
    CmuneEventHandler.RemoveListener<OnPlayerRespawnEvent>(new Action<OnPlayerRespawnEvent>(this.OnPlayerRespawn));
  }

  public void OnUpdate()
  {
  }

  public void OnGUI()
  {
  }

  private void OnPlayerRespawn(OnPlayerRespawnEvent ev) => this._stateMachine.PopState();
}
