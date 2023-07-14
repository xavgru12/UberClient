// Decompiled with JetBrains decompiler
// Type: JoinTrainingGameGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class JoinTrainingGameGUI : BaseJoinGameGUI
{
  private TrainingFpsMode _gameMode;

  public JoinTrainingGameGUI(TrainingFpsMode gameMode) => this._gameMode = gameMode;

  public override void Draw(Rect rect)
  {
    if (!GUITools.Button(new Rect(0.0f, 45f, 130f, 130f), GUIContent.none, StormFront.ButtonJoinGray))
      return;
    GamePageManager.Instance.UnloadCurrentPage();
    this._gameMode.InitializeMode();
  }
}
