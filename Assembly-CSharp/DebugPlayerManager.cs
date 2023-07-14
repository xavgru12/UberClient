// Decompiled with JetBrains decompiler
// Type: DebugPlayerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DebugPlayerManager : IDebugPage
{
  private Vector2 v1;

  public string Title => "Players";

  public void Draw()
  {
    if (!GameState.HasCurrentGame)
      return;
    this.v1 = GUILayout.BeginScrollView(this.v1, GUILayout.MinHeight(200f));
    GUILayout.BeginHorizontal();
    foreach (UberStrike.Realtime.UnitySdk.CharacterInfo characterInfo in GameState.CurrentGame.Players.Values)
      GUILayout.Label(characterInfo.PlayerName + " " + (object) characterInfo.Ping);
    GUILayout.EndHorizontal();
    GUILayout.EndScrollView();
  }
}
