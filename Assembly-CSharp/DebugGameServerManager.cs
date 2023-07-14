// Decompiled with JetBrains decompiler
// Type: DebugGameServerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DebugGameServerManager : IDebugPage
{
  public string Title => "Requests";

  public void Draw()
  {
    foreach (ServerLoadRequest serverRequest in Singleton<GameServerManager>.Instance.ServerRequests)
    {
      GUILayout.Label(serverRequest.Server.Name + " " + serverRequest.Server.ConnectionString + ", Latency: " + (object) serverRequest.Server.Latency + " - " + (object) serverRequest.Server.IsValid);
      GUILayout.Label("States: " + (object) serverRequest.RequestState + " " + (object) serverRequest.Server.Data.State + ", Connection: " + (object) serverRequest.ConnectionState + " with Peer: " + (object) serverRequest.PeerState);
      GUILayout.Space(10f);
    }
  }
}
