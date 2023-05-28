// Decompiled with JetBrains decompiler
// Type: DebugNetworkTraffic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugNetworkTraffic : IDebugPage
{
  private Vector2 scroller;

  public string Title => "Network";

  public void Draw()
  {
    this.scroller = GUILayout.BeginScrollView(this.scroller);
    GUILayout.BeginHorizontal();
    GUILayout.BeginVertical();
    GUILayout.Label("IN (" + (object) NetworkStatistics.TotalBytesIn + ")");
    foreach (KeyValuePair<string, NetworkStatistics.Statistics> keyValuePair in (Dictionary<string, NetworkStatistics.Statistics>) NetworkStatistics.Incoming)
      GUILayout.Label(keyValuePair.Key + ": " + (object) keyValuePair.Value);
    GUILayout.EndVertical();
    GUILayout.BeginVertical();
    GUILayout.Label("OUT (" + (object) NetworkStatistics.TotalBytesOut + ")");
    foreach (KeyValuePair<string, NetworkStatistics.Statistics> keyValuePair in (Dictionary<string, NetworkStatistics.Statistics>) NetworkStatistics.Outgoing)
      GUILayout.Label(keyValuePair.Key + ": " + (object) keyValuePair.Value);
    GUILayout.EndVertical();
    GUILayout.EndHorizontal();
    GUILayout.EndScrollView();
  }
}
