// Decompiled with JetBrains decompiler
// Type: DebugApplication
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public class DebugApplication : IDebugPage
{
  public string Title => "App";

  public void Draw()
  {
    GUILayout.Label("Channel: " + (object) ApplicationDataManager.Channel);
    GUILayout.Label("Version: 4.3.10");
    GUILayout.Label("BuildNumber: " + ApplicationDataManager.BuildNumber);
    GUILayout.Label("BuildType: " + (object) ApplicationDataManager.BuildType);
    GUILayout.Label("Source: " + Application.srcValue);
    GUILayout.Label("WS API: " + UberStrike.DataCenter.UnitySdk.ApiVersion.Current);
    GUILayout.Label("RT API: " + UberStrike.Realtime.UnitySdk.ApiVersion.Current);
    if (PlayerDataManager.AccessLevel <= MemberAccessLevel.Default)
      return;
    GUILayout.Label("Time: " + (object) CheatDetection.GameTime + " " + (object) CheatDetection.RealTime + " (Dif: " + (object) (CheatDetection.GameTime - CheatDetection.RealTime) + ")");
    GUILayout.Label("Member Name: " + PlayerDataManager.Name);
    GUILayout.Label("Member Cmid: " + (object) PlayerDataManager.Cmid);
    GUILayout.Label("Member Access: " + (object) PlayerDataManager.AccessLevel);
    GUILayout.Label("Member Tag: " + PlayerDataManager.GroupTag);
    foreach (GameServerView photonServer in Singleton<GameServerManager>.Instance.PhotonServerList)
      GUILayout.Label("Game Server: " + photonServer.Name + " [" + (object) photonServer.MinLatency + "] " + (object) photonServer.Data.PeersConnected + "/" + (object) photonServer.Data.PlayersConnected);
  }
}
