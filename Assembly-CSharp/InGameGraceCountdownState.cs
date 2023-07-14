// Decompiled with JetBrains decompiler
// Type: InGameGraceCountdownState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

internal class InGameGraceCountdownState : IState
{
  public void OnEnter()
  {
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag = HudDrawFlags.Score | HudDrawFlags.HealthArmor | HudDrawFlags.Ammo | HudDrawFlags.Weapons | HudDrawFlags.RoundTime | HudDrawFlags.XpPoints | HudDrawFlags.EventStream | HudDrawFlags.RemainingKill | HudDrawFlags.InGameChat | HudDrawFlags.StateMsg;
    Singleton<HudUtil>.Instance.ClearAllFeedbackHud();
    Singleton<PlayerStateMsgHud>.Instance.DisplayNone();
    Singleton<PopupHud>.Instance.PopupRoundStart();
  }

  public void OnExit()
  {
  }

  public void OnUpdate()
  {
  }

  public void OnGUI()
  {
  }
}
