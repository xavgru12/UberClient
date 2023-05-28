// Decompiled with JetBrains decompiler
// Type: InGamePlayerPausedState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;

internal class InGamePlayerPausedState : IState
{
  private const HudDrawFlags pauseDrawFlagTuning = ~(HudDrawFlags.Weapons | HudDrawFlags.Reticle);
  private StateMachine _stateMachine;

  public InGamePlayerPausedState(StateMachine stateMachine) => this._stateMachine = stateMachine;

  public void OnEnter()
  {
    Singleton<HudDrawFlagGroup>.Instance.AddFlag(~(HudDrawFlags.Weapons | HudDrawFlags.Reticle));
    Singleton<HudUtil>.Instance.ShowContinueButton();
    if (!ApplicationDataManager.IsMobile)
      Singleton<WeaponsHud>.Instance.QuickItems.Expand();
    CmuneEventHandler.AddListener<OnPlayerUnpauseEvent>(new Action<OnPlayerUnpauseEvent>(this.OnPlayerUnpaused));
  }

  public void OnExit()
  {
    Singleton<PlayerStateMsgHud>.Instance.ButtonEnabled = false;
    Singleton<HudDrawFlagGroup>.Instance.RemoveFlag(~(HudDrawFlags.Weapons | HudDrawFlags.Reticle));
    if (!ApplicationDataManager.IsMobile)
      Singleton<WeaponsHud>.Instance.QuickItems.Collapse();
    CmuneEventHandler.RemoveListener<OnPlayerUnpauseEvent>(new Action<OnPlayerUnpauseEvent>(this.OnPlayerUnpaused));
  }

  public void OnUpdate()
  {
  }

  public void OnGUI()
  {
  }

  private void OnPlayerUnpaused(OnPlayerUnpauseEvent ev) => this._stateMachine.PopState();
}
