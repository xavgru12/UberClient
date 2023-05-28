// Decompiled with JetBrains decompiler
// Type: EndOfRoundPageScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class EndOfRoundPageScene : PageScene
{
  private int _endOfMatchCountdown;
  private MeshGUIText _redTeamText;
  private MeshGUIText _blueTeamText;
  private MeshGUIText _redTeamSplats;
  private MeshGUIText _blueTeamSplats;
  private Animatable2DGroup _scoreGroup = new Animatable2DGroup();

  public override PageType PageType => PageType.EndOfMatch;

  protected override void OnLoad()
  {
    CmuneEventHandler.AddListener<TeamGameEndEvent>(new Action<TeamGameEndEvent>(this.OnTeamGameEnd));
    CmuneEventHandler.AddListener<EndOfMatchCountdownEvent>(new Action<EndOfMatchCountdownEvent>(this.OnEndOfMatchCountdown));
  }

  protected override void OnUnload()
  {
    this._scoreGroup.Hide();
    GameState.LocalDecorator.SetLayers(UberstrikeLayer.LocalPlayer);
    CmuneEventHandler.RemoveListener<TeamGameEndEvent>(new Action<TeamGameEndEvent>(this.OnTeamGameEnd));
    CmuneEventHandler.RemoveListener<EndOfMatchCountdownEvent>(new Action<EndOfMatchCountdownEvent>(this.OnEndOfMatchCountdown));
  }

  private void OnGUI()
  {
    GUI.depth = 100;
    if (!GameState.HasCurrentGame)
      return;
    float num = (float) Screen.height * 0.2f;
    Rect pixelRect = GameState.CurrentSpace.Camera.pixelRect;
    if (GameState.CurrentGameMode == GameMode.TeamDeathMatch || GameState.CurrentGameMode == GameMode.TeamElimination)
    {
      Rect position = new Rect((float) (((double) pixelRect.width - (double) num * 2.0 - 40.0) / 2.0), (float) (GlobalUIRibbon.Instance.Height() + 30), (float) ((double) num * 2.0 + 40.0), num);
      GUI.BeginGroup(position);
      GUI.Label(new Rect(0.0f, 0.0f, num, num), GUIContent.none, StormFront.BlueBox);
      GUI.Label(new Rect(position.width - num, 0.0f, num, num), GUIContent.none, StormFront.RedBox);
      if (this._redTeamSplats != null)
      {
        this._redTeamSplats.Position = new Vector2((float) (Screen.width / 2) + (float) (((double) position.width - (double) num) / 2.0), (float) ((double) position.y + (double) num / 2.0 - (double) position.height * 0.25));
        this._redTeamSplats.Draw(0.0f, 0.0f);
      }
      if (this._blueTeamSplats != null)
      {
        this._blueTeamSplats.Position = new Vector2((float) (Screen.width / 2) - (float) (((double) position.width - (double) num) / 2.0), (float) ((double) position.y + (double) num / 2.0 - (double) position.height * 0.25));
        this._blueTeamSplats.Draw(0.0f, 0.0f);
      }
      if (this._redTeamText != null)
      {
        this._redTeamText.Position = new Vector2((float) (Screen.width / 2) + (float) (((double) position.width - (double) num) / 2.0), (float) ((double) position.y + (double) num / 2.0 + (double) position.height * 0.30000001192092896));
        this._redTeamText.Draw(0.0f, 0.0f);
      }
      if (this._blueTeamText != null)
      {
        this._blueTeamText.Position = new Vector2((float) (Screen.width / 2) - (float) (((double) position.width - (double) num) / 2.0), (float) ((double) position.y + (double) num / 2.0 + (double) position.height * 0.30000001192092896));
        this._blueTeamText.Draw(0.0f, 0.0f);
      }
      GUI.EndGroup();
    }
    this.DrawReadyButton(new Rect((float) (((double) pixelRect.width - (double) num * 2.0 - 60.0) / 2.0), (float) ((double) GlobalUIRibbon.Instance.Height() + (double) num + 40.0), (float) ((double) num * 2.0 + 60.0), 50f));
  }

  private void OnTeamGameEnd(TeamGameEndEvent ev)
  {
    if (this._redTeamText == null)
    {
      this._redTeamText = new MeshGUIText(LocalizedStrings.RedCaps, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
      Singleton<HudStyleUtility>.Instance.SetRedStyle(this._redTeamText);
      this._redTeamText.Scale = new Vector2(0.5f, 0.5f);
      this._scoreGroup.Group.Add((IAnimatable2D) this._redTeamText);
    }
    if (this._blueTeamText == null)
    {
      this._blueTeamText = new MeshGUIText(LocalizedStrings.BlueCaps, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
      Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._blueTeamText);
      this._blueTeamText.Scale = new Vector2(0.5f, 0.5f);
      this._scoreGroup.Group.Add((IAnimatable2D) this._blueTeamText);
    }
    if (this._redTeamSplats == null)
    {
      this._redTeamSplats = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
      Singleton<HudStyleUtility>.Instance.SetRedStyle(this._redTeamSplats);
      this._redTeamSplats.Scale = new Vector2(0.8f, 0.8f);
      this._scoreGroup.Group.Add((IAnimatable2D) this._redTeamSplats);
    }
    if (this._blueTeamSplats == null)
    {
      this._blueTeamSplats = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
      Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._blueTeamSplats);
      this._blueTeamSplats.Scale = new Vector2(0.8f, 0.8f);
      this._scoreGroup.Group.Add((IAnimatable2D) this._blueTeamSplats);
    }
    this._redTeamSplats.Text = ev.RedTeamSplats.ToString();
    this._blueTeamSplats.Text = ev.BlueTeamSplats.ToString();
    this._scoreGroup.Show();
  }

  private void DrawReadyButton(Rect rect)
  {
    if (!GameState.HasCurrentGame)
      return;
    GUI.BeginGroup(rect);
    string text = string.Format("{0} {1}/{2}", (object) LocalizedStrings.ReadyCaps, (object) GameState.CurrentGame.PlayerReadyForNextRound, (object) GameState.CurrentGame.Players.Count);
    GUITools.PushGUIState();
    GUI.enabled = !GameState.IsReadyForNextGame;
    bool flag = GUI.Toggle(new Rect((float) ((double) rect.width / 2.0 - 70.0), (float) ((double) rect.height / 2.0 - 23.0), 140f, 45f), GameState.IsReadyForNextGame, text, StormFront.ButtonGray);
    GUITools.PopGUIState();
    GUI.Label(new Rect((float) ((double) rect.width / 2.0 + 80.0), 0.0f, 60f, rect.height), this._endOfMatchCountdown.ToString(), BlueStonez.label_interparkbold_48pt_left);
    if (flag && !GameState.IsReadyForNextGame)
    {
      GameState.IsReadyForNextGame = true;
      GUITools.Clicked();
      GameState.CurrentGame.SetReadyForNextMatch(true);
      SfxManager.Play2dAudioClip(GameAudio.ClickReady);
    }
    else if (!flag && GameState.IsReadyForNextGame)
      SfxManager.Play2dAudioClip(GameAudio.ClickUnready);
    GUI.EndGroup();
  }

  private void OnEndOfMatchCountdown(EndOfMatchCountdownEvent ev) => this._endOfMatchCountdown = ev.Countdown;
}
