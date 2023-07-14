// Decompiled with JetBrains decompiler
// Type: JoinTeamGameGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class JoinTeamGameGUI : BaseJoinGameGUI
{
  private TeamDeathMatchGameMode _gameMode;

  public JoinTeamGameGUI(TeamDeathMatchGameMode gameMode) => this._gameMode = gameMode;

  public override void Draw(Rect rect)
  {
    float num = Mathf.Min(400f, rect.width);
    Vector2 zero = Vector2.zero with
    {
      x = (float) (((double) rect.width - (double) num) / 2.0)
    };
    zero.y = zero.x + num;
    float left = zero.y - 130f;
    int maxPlayerCount = Mathf.CeilToInt((float) this._gameMode.GameData.MaxPlayers / 2f);
    this.DrawPlayers(new Rect(zero.x, 194f, 130f, 24f), this._gameMode.BlueTeamPlayerCount, maxPlayerCount, StormFront.DotBlue);
    GUITools.PushGUIState();
    GUI.enabled = JoinGameUtil.CanJoinBlueTeam(this._gameMode);
    if (GUITools.Button(new Rect(zero.x, 64f, 130f, 130f), GUIContent.none, StormFront.ButtonJoinBlue))
    {
      GameState.LocalDecorator.HideWeapons();
      GameState.LocalDecorator.MeshRenderer.enabled = false;
      GamePageManager.Instance.UnloadCurrentPage();
      this._gameMode.InitializeMode(TeamID.BLUE);
    }
    GUITools.PopGUIState();
    GUITools.PushGUIState();
    GUI.enabled = JoinGameUtil.CanJoinRedTeam(this._gameMode);
    this.DrawPlayers(new Rect(left, 194f, 130f, 24f), this._gameMode.RedTeamPlayerCount, maxPlayerCount, StormFront.DotRed);
    if (GUITools.Button(new Rect(left, 64f, 130f, 130f), GUIContent.none, StormFront.ButtonJoinRed))
    {
      GameState.LocalDecorator.HideWeapons();
      GameState.LocalDecorator.MeshRenderer.enabled = false;
      GamePageManager.Instance.UnloadCurrentPage();
      this._gameMode.InitializeMode(TeamID.RED);
    }
    GUITools.PopGUIState();
    this.DrawSpectateButton(new Rect(rect.width - 33f, rect.height - 60f, 33f, 33f));
  }
}
