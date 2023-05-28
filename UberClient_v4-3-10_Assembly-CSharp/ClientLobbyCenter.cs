// Decompiled with JetBrains decompiler
// Type: ClientLobbyCenter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

[NetworkClass(3)]
public class ClientLobbyCenter : ClientNetworkClass
{
  private ActorInfo _myInfo;

  public ClientLobbyCenter(RemoteMethodInterface rmi)
    : base(rmi)
  {
    this._myInfo = (ActorInfo) new CommActorInfo(string.Empty, 0, ChannelType.WebPortal);
  }

  protected override void OnInitialized()
  {
    base.OnInitialized();
    ((Dictionary<int, int>) this.MyInfo.Cache).Clear();
    this.MyInfo.ActorId = this._rmi.Messenger.PeerListener.ActorIdSecure;
    this.MyInfo.PlayerName = string.Empty;
    this.SendMethodToServer((byte) 1, (object) this.MyInfo);
  }

  public ActorInfo MyInfo => this._myInfo;
}
