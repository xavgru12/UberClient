// Decompiled with JetBrains decompiler
// Type: JoinNonTeamGameGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class JoinNonTeamGameGUI : BaseJoinGameGUI
{
  private DeathMatchGameMode _gameMode;

  public JoinNonTeamGameGUI(DeathMatchGameMode gameMode) => this._gameMode = gameMode;

  public override void Draw(Rect rect)
  {
    int connectedPlayers = this._gameMode.GameData.ConnectedPlayers;
    int num = Mathf.Min(8, this._gameMode.GameData.MaxPlayers);
    int maxPlayerCount = this._gameMode.GameData.MaxPlayers - num;
    this.DrawPlayers(new Rect((float) (((double) rect.width - 130.0) / 2.0), 194f, 130f, 24f), Mathf.Min(num, connectedPlayers), num, StormFront.DotBlue);
    if (maxPlayerCount > 0)
      this.DrawPlayers(new Rect((float) (((double) rect.width - 130.0) / 2.0), 212f, 130f, 24f), Mathf.Max(0, connectedPlayers - num), maxPlayerCount, StormFront.DotBlue);
    GUITools.PushGUIState();
    GUI.enabled = JoinGameUtil.CanJoinGame((FpsGameMode) this._gameMode);
    if (GUITools.Button(new Rect((float) (((double) rect.width - 130.0) / 2.0), 64f, 130f, 130f), GUIContent.none, StormFront.ButtonJoinGray))
    {
      GameState.LocalDecorator.HideWeapons();
      GameState.LocalDecorator.MeshRenderer.enabled = false;
      GamePageManager.Instance.UnloadCurrentPage();
      this._gameMode.InitializeMode();
    }
    GUITools.PopGUIState();
    this.DrawSpectateButton(new Rect(rect.width - 33f, rect.height - 60f, 33f, 33f));
  }
}
