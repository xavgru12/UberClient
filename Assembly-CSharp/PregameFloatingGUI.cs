// Decompiled with JetBrains decompiler
// Type: PregameFloatingGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PregameFloatingGUI : MonoBehaviour
{
  private BaseJoinGameGUI _joinGameGUI;
  private MeshGUIText _gameModeText;

  private void OnEnable() => this.InitializeJoinGameGUI();

  private void OnDisable()
  {
    if (this._gameModeText == null)
      return;
    this._gameModeText.Hide();
  }

  private void OnGUI()
  {
    GUI.depth = 100;
    if (!GameState.HasCurrentSpace)
      return;
    Rect rect = new Rect(30f, 80f, GameState.CurrentSpace.Camera.pixelRect.width - 60f, (float) (Screen.height - 80));
    GUI.BeginGroup(rect, string.Empty);
    this.DrawJoinArea(rect);
    GUI.EndGroup();
  }

  private void InitializeJoinGameGUI()
  {
    if (!GameState.HasCurrentGame)
      return;
    GameMode currentGameMode = GameState.CurrentGameMode;
    switch (currentGameMode)
    {
      case GameMode.TeamElimination:
        this._joinGameGUI = (BaseJoinGameGUI) new JoinTeamGameGUI(GameState.CurrentGame as TeamDeathMatchGameMode);
        break;
      case GameMode.Training:
        this._joinGameGUI = (BaseJoinGameGUI) new JoinTrainingGameGUI(GameState.CurrentGame as TrainingFpsMode);
        break;
      default:
        if (currentGameMode != GameMode.TeamDeathMatch)
        {
          if (currentGameMode == GameMode.DeathMatch)
          {
            this._joinGameGUI = (BaseJoinGameGUI) new JoinNonTeamGameGUI(GameState.CurrentGame as DeathMatchGameMode);
            break;
          }
          break;
        }
        goto case GameMode.TeamElimination;
    }
    string text = string.Empty;
    switch (GameState.CurrentGameMode)
    {
      case GameMode.TeamDeathMatch:
        text = LocalizedStrings.TeamDeathMatch.ToUpper();
        break;
      case GameMode.DeathMatch:
        text = LocalizedStrings.DeathMatch.ToUpper();
        break;
      case GameMode.TeamElimination:
        text = LocalizedStrings.TeamElimination.ToUpper();
        break;
    }
    if (this._gameModeText != null)
    {
      this._gameModeText.Text = text;
      this._gameModeText.Show();
    }
    else
    {
      this._gameModeText = new MeshGUIText(text, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
      Singleton<HudStyleUtility>.Instance.SetBlackStyle(this._gameModeText);
      this._gameModeText.Scale = new Vector2(0.4f, 0.4f);
    }
  }

  private void DrawJoinArea(Rect rect)
  {
    this._gameModeText.Position = new Vector2((float) (Screen.width / 2), 120f);
    this._gameModeText.Draw(0.0f, 0.0f);
    if (this._joinGameGUI == null)
      return;
    this._joinGameGUI.Draw(rect);
  }
}
