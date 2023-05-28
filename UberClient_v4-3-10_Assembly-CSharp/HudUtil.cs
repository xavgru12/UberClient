// Decompiled with JetBrains decompiler
// Type: HudUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class HudUtil : Singleton<HudUtil>
{
  private int _lastScreenWidth;
  private int _lastScreenHeight;

  private HudUtil()
  {
  }

  public void Update()
  {
    if (!this.IsScreenResolutionChanged())
      return;
    CmuneEventHandler.Route((object) new ScreenResolutionEvent());
  }

  public void ClearAllHud()
  {
    this.CleanAllTemporaryHud();
    Singleton<ScreenshotHud>.Instance.Enable = false;
    Singleton<FrameRateHud>.Instance.Enable = false;
  }

  public void ClearAllFeedbackHud()
  {
    Singleton<EventStreamHud>.Instance.ClearAllEvents();
    Singleton<EventFeedbackHud>.Instance.ClearAll();
    Singleton<InGameFeatHud>.Instance.AnimationScheduler.ClearAll();
    Singleton<DamageFeedbackHud>.Instance.ClearAll();
    Singleton<PlayerStateMsgHud>.Instance.DisplayNone();
  }

  public void SetPlayerTeam(TeamID teamId) => CmuneEventHandler.Route((object) new OnSetPlayerTeamEvent()
  {
    TeamId = teamId
  });

  public void ShowContinueButton()
  {
    Singleton<PlayerStateMsgHud>.Instance.ButtonEnabled = true;
    Singleton<PlayerStateMsgHud>.Instance.ButtonCaption = LocalizedStrings.Continue;
    Singleton<PlayerStateMsgHud>.Instance.OnButtonClicked = new PlayerStateMsgHud.OnButtonClickedDelegate(this.OnContinueButtonClicked);
  }

  public void ShowRespawnButton()
  {
    Singleton<PlayerStateMsgHud>.Instance.ButtonEnabled = true;
    Singleton<PlayerStateMsgHud>.Instance.TemporaryMsgEnabled = false;
    Singleton<PlayerStateMsgHud>.Instance.ButtonCaption = LocalizedStrings.Respawn;
    Singleton<PlayerStateMsgHud>.Instance.OnButtonClicked = new PlayerStateMsgHud.OnButtonClickedDelegate(this.OnRespawnButtonClicked);
  }

  public void ShowClickToRespawnText(FpsGameMode fpsGameMode)
  {
    Singleton<PlayerStateMsgHud>.Instance.DisplayClickToRespawnMsg();
    if (!Input.GetMouseButtonDown(0))
      return;
    fpsGameMode.RespawnPlayer();
  }

  public void ShowRespawnFrozenTimeText(int spawnFrozenTime) => Singleton<PlayerStateMsgHud>.Instance.DisplayRespawnTimeMsg(spawnFrozenTime);

  public void ShowTimeOutText(FpsGameMode fpsGameMode, int timeout) => Singleton<PlayerStateMsgHud>.Instance.DisplayDisconnectionTimeoutMsg(timeout);

  public void SetTeamScore(int blueScore, int redScore)
  {
    Singleton<MatchStatusHud>.Instance.BlueTeamScore = blueScore;
    Singleton<MatchStatusHud>.Instance.RedTeamScore = redScore;
    TabScreenPanelGUI.Instance.SetTeamSplats(blueScore, redScore);
  }

  public void AddInGameEvent(
    string subjective,
    string objective,
    UberstrikeItemClass weaponClass,
    InGameEventFeedbackType eventType,
    TeamID sourceTeam,
    TeamID destinationTeam)
  {
    Singleton<EventStreamHud>.Instance.AddEventText(subjective, sourceTeam, this.GetEventTypeMessage(eventType), objective, destinationTeam);
  }

  public void AddInGameEvent(string sourcePlayer, string message) => this.AddInGameEvent(sourcePlayer, message, UberstrikeItemClass.FunctionalGeneral, InGameEventFeedbackType.CustomMessage, TeamID.NONE, TeamID.NONE);

  private void OnRespawnButtonClicked()
  {
    GamePageManager.Instance.UnloadCurrentPage();
    GameState.LocalPlayer.UnPausePlayer();
    GameState.CurrentGame.RespawnPlayer();
  }

  private void OnContinueButtonClicked()
  {
    GamePageManager.Instance.UnloadCurrentPage();
    GameState.LocalPlayer.UnPausePlayer();
  }

  private void CleanAllTemporaryHud()
  {
    Singleton<InGameChatHud>.Instance.ClearAll();
    this.ClearAllFeedbackHud();
  }

  private string GetEventTypeMessage(InGameEventFeedbackType eventType)
  {
    switch (eventType)
    {
      case InGameEventFeedbackType.None:
        return "killed";
      case InGameEventFeedbackType.HeadShot:
        return "headshot";
      case InGameEventFeedbackType.NutShot:
        return "nutshot";
      case InGameEventFeedbackType.Humiliation:
        return "smacked";
      default:
        return string.Empty;
    }
  }

  private bool IsScreenResolutionChanged()
  {
    if (Screen.width == this._lastScreenWidth && Screen.height == this._lastScreenHeight)
      return false;
    this._lastScreenWidth = Screen.width;
    this._lastScreenHeight = Screen.height;
    return true;
  }
}
