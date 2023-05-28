// Decompiled with JetBrains decompiler
// Type: PhotonServerConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PhotonServerConfiguration : MonoBehaviour
{
  [SerializeField]
  private PhotonServerConfiguration.LocalRealtimeServer _localGameServer = new PhotonServerConfiguration.LocalRealtimeServer()
  {
    Ip = "127.0.0.1",
    Port = 5155
  };
  [SerializeField]
  private PhotonServerConfiguration.LocalRealtimeServer _localCommServer = new PhotonServerConfiguration.LocalRealtimeServer()
  {
    Ip = "127.0.0.1",
    Port = 5055
  };

  public PhotonServerConfiguration.LocalRealtimeServer CustomGameServer => this._localGameServer;

  public PhotonServerConfiguration.LocalRealtimeServer CustomCommServer => this._localCommServer;

  private void Awake()
  {
    CmuneDebug.AddDebugChannel((ICmuneDebug) new UnityDebug());
    if (this.CustomGameServer.IsEnabled)
    {
      for (int index = 0; index < 20; index += 5)
        Singleton<GameServerManager>.Instance.AddGameServer(new PhotonView()
        {
          IP = this.CustomGameServer.Ip,
          Port = this.CustomGameServer.Port,
          Name = "CUSTOM GAME SERVER",
          PhotonId = UnityEngine.Random.Range(-1, -100),
          Region = RegionType.AsiaPacific,
          UsageType = PhotonUsageType.All,
          MinLatency = index
        });
    }
    if (!this._localCommServer.IsEnabled)
      return;
    CmuneNetworkManager.CurrentCommServer = new GameServerView(this._localCommServer.Address, PhotonUsageType.CommServer);
    CmuneNetworkManager.UseLocalCommServer = this._localCommServer.IsEnabled;
  }

  [Serializable]
  public class LocalRealtimeServer
  {
    public string Ip = string.Empty;
    public int Port;
    public bool IsEnabled;

    public string Address => this.Ip + ":" + (object) this.Port;
  }
}
