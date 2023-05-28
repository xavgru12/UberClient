// Decompiled with JetBrains decompiler
// Type: InGameEndOfMatchState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class InGameEndOfMatchState : IState
{
  private float _nextRoundStartTime;
  private int _endOfMatchCountdown;

  public void OnEnter()
  {
    Singleton<QuickItemController>.Instance.Restriction.RenewGameUses();
    Singleton<QuickItemController>.Instance.IsEnabled = false;
    GamePageManager.Instance.LoadPage(PageType.EndOfMatch);
    SfxManager.Play2dAudioClip(GameAudio.EndOfRound);
    Singleton<PopupHud>.Instance.PopupMatchOver();
    Singleton<PlayerStateMsgHud>.Instance.ButtonEnabled = false;
    Screen.lockCursor = false;
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag = HudDrawFlags.XpPoints | HudDrawFlags.InGameChat;
    HudController.Instance.XpPtsHud.DisplayPermanently();
    CmuneEventHandler.AddListener<OnSetEndOfMatchCountdownEvent>(new Action<OnSetEndOfMatchCountdownEvent>(this.OnStartCountdown));
  }

  public void OnExit()
  {
    this._endOfMatchCountdown = 0;
    SfxManager.Play2dAudioClip(GameAudio.CountdownTonal2);
    SfxManager.Play2dAudioClip(GameAudio.Fight, 0.5f);
    GamePageManager.Instance.UnloadCurrentPage();
    CmuneEventHandler.RemoveListener<OnSetEndOfMatchCountdownEvent>(new Action<OnSetEndOfMatchCountdownEvent>(this.OnStartCountdown));
  }

  public void OnUpdate()
  {
    if ((double) this._nextRoundStartTime < (double) Time.time)
      return;
    int num = Mathf.CeilToInt(Mathf.Max(this._nextRoundStartTime - Time.time, 0.0f));
    if (this._endOfMatchCountdown == num)
      return;
    if (num <= 3 && num >= 1)
      SfxManager.Play2dAudioClip(GameAudio.CountdownTonal1);
    GameState.CurrentGame.UpdatePlayerReadyForNextRound();
    this._endOfMatchCountdown = num;
    CmuneEventHandler.Route((object) new EndOfMatchCountdownEvent()
    {
      Countdown = this._endOfMatchCountdown
    });
  }

  public void OnGUI()
  {
  }

  private void OnStartCountdown(OnSetEndOfMatchCountdownEvent ev)
  {
    this._nextRoundStartTime = Time.time + (float) ev.SecondsUntilNextMatch;
    this._endOfMatchCountdown = 0;
  }
}
