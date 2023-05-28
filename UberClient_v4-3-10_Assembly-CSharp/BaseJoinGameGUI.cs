// Decompiled with JetBrains decompiler
// Type: BaseJoinGameGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public abstract class BaseJoinGameGUI
{
  public abstract void Draw(Rect rect);

  protected bool DrawJoinButton(Rect rect, string content, GUIStyle style)
  {
    GUI.BeginGroup(rect);
    bool flag = GUI.Button(new Rect(0.0f, 0.0f, rect.width, rect.height), GUIContent.none, style);
    GUI.Label(new Rect(0.0f, (float) ((double) rect.height / 2.0 - 35.0), rect.width, 30f), "JOIN", BlueStonez.label_interparkbold_32pt);
    GUI.Label(new Rect(0.0f, (float) ((double) rect.height / 2.0 + 5.0), rect.width, 30f), content, BlueStonez.label_interparkbold_32pt);
    GUI.EndGroup();
    return flag;
  }

  protected void DrawPlayers(Rect rect, int playerCount, int maxPlayerCount, GUIStyle style)
  {
    GUI.BeginGroup(new Rect(rect.x - 1f, rect.y, rect.width, rect.height));
    float num1 = 24f;
    float num2 = -8.857f;
    for (int index = 0; index < maxPlayerCount; ++index)
    {
      GUIStyle style1 = index >= playerCount ? StormFront.DotGray : style;
      GUI.Label(new Rect((float) index * (num1 + num2), 0.0f, num1, num1), GUIContent.none, style1);
    }
    GUI.EndGroup();
  }

  protected void DrawSpectateButton(Rect position)
  {
    if (PlayerDataManager.AccessLevel < MemberAccessLevel.SeniorModerator || !GUITools.Button(position, GUIContent.none, StormFront.ButtonCam))
      return;
    GamePageManager.Instance.UnloadCurrentPage();
    Singleton<GameStateController>.Instance.SpectateCurrentGame();
    GameState.LocalDecorator.MeshRenderer.enabled = false;
  }
}
