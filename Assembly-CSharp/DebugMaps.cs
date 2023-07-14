// Decompiled with JetBrains decompiler
// Type: DebugMaps
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DebugMaps : IDebugPage
{
  private Vector2 scroll;

  public string Title => "Maps";

  public void Draw()
  {
    this.scroll = GUILayout.BeginScrollView(this.scroll);
    foreach (UberstrikeMap allMap in Singleton<MapManager>.Instance.AllMaps)
      GUILayout.Label(allMap.Id.ToString() + ", Modes: " + (object) allMap.View.SupportedGameModes + ", Item: " + (object) allMap.View.RecommendedItemId + ", Scene: " + allMap.SceneName + ", File: " + allMap.View.FileName);
    GUILayout.EndScrollView();
  }
}
