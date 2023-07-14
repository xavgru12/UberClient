// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.Lite.LitePeer
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System.Collections;
using System.Collections.Generic;

namespace ExitGames.Client.Photon.Lite
{
  public class LitePeer : PhotonPeer
  {
    public LitePeer(IPhotonPeerListener listener)
      : base(listener, ConnectionProtocol.Udp)
    {
    }

    protected LitePeer()
      : base(ConnectionProtocol.Udp)
    {
    }

    protected LitePeer(ConnectionProtocol protocolType)
      : base(protocolType)
    {
    }

    public LitePeer(IPhotonPeerListener listener, ConnectionProtocol protocolType)
      : base(listener, protocolType)
    {
    }

    public LitePeer(IPhotonPeerListener listener, bool useTcp)
      : base(listener, useTcp)
    {
    }

    public virtual bool OpRaiseEvent(
      byte eventCode,
      Hashtable customEventContent,
      bool sendReliable)
    {
      return this.OpRaiseEvent(eventCode, customEventContent, sendReliable, (byte) 0);
    }

    public virtual bool OpRaiseEvent(
      byte eventCode,
      Hashtable customEventContent,
      bool sendReliable,
      byte channelId)
    {
      return this.OpCustom((byte) 253, new Dictionary<byte, object>()
      {
        [(byte) 245] = (object) customEventContent,
        [(byte) 244] = (object) eventCode
      }, sendReliable, channelId);
    }

    public virtual bool OpRaiseEvent(
      byte eventCode,
      Hashtable customEventContent,
      bool sendReliable,
      byte channelId,
      int[] targetActors)
    {
      Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>();
      customOpParameters[(byte) 245] = (object) customEventContent;
      customOpParameters[(byte) 244] = (object) eventCode;
      if (targetActors != null)
        customOpParameters[(byte) 252] = (object) targetActors;
      return this.OpCustom((byte) 253, customOpParameters, sendReliable, channelId);
    }

    public virtual bool OpRaiseEvent(
      byte eventCode,
      Hashtable customEventContent,
      bool sendReliable,
      byte channelId,
      EventCaching cache,
      ReceiverGroup receivers)
    {
      Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>();
      customOpParameters[(byte) 245] = (object) customEventContent;
      customOpParameters[(byte) 244] = (object) eventCode;
      if (cache != EventCaching.DoNotCache)
        customOpParameters[(byte) 247] = (object) (byte) cache;
      if (receivers != ReceiverGroup.Others)
        customOpParameters[(byte) 246] = (object) (byte) receivers;
      return this.OpCustom((byte) 253, customOpParameters, sendReliable, channelId, false);
    }

    public virtual bool OpSetPropertiesOfActor(
      int actorNr,
      Hashtable properties,
      bool broadcast,
      byte channelId)
    {
      Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>();
      customOpParameters.Add((byte) 251, (object) properties);
      customOpParameters.Add((byte) 254, (object) actorNr);
      if (broadcast)
        customOpParameters.Add((byte) 250, (object) broadcast);
      return this.OpCustom((byte) 252, customOpParameters, true, channelId);
    }

    public virtual bool OpSetPropertiesOfGame(Hashtable properties, bool broadcast, byte channelId)
    {
      Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>();
      customOpParameters.Add((byte) 251, (object) properties);
      if (broadcast)
        customOpParameters.Add((byte) 250, (object) broadcast);
      return this.OpCustom((byte) 252, customOpParameters, true, channelId);
    }

    public virtual bool OpGetProperties(byte channelId) => this.OpCustom((byte) 251, new Dictionary<byte, object>()
    {
      {
        (byte) 251,
        (object) (byte) 3
      }
    }, true, channelId);

    public virtual bool OpGetPropertiesOfActor(
      int[] actorNrList,
      string[] properties,
      byte channelId)
    {
      Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>();
      customOpParameters.Add((byte) 251, (object) LitePropertyTypes.Actor);
      if (properties != null)
        customOpParameters.Add((byte) 249, (object) properties);
      if (actorNrList != null)
        customOpParameters.Add((byte) 252, (object) actorNrList);
      return this.OpCustom((byte) 251, customOpParameters, true, channelId);
    }

    public virtual bool OpGetPropertiesOfActor(
      int[] actorNrList,
      byte[] properties,
      byte channelId)
    {
      Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>();
      customOpParameters.Add((byte) 251, (object) LitePropertyTypes.Actor);
      if (properties != null)
        customOpParameters.Add((byte) 249, (object) properties);
      if (actorNrList != null)
        customOpParameters.Add((byte) 252, (object) actorNrList);
      return this.OpCustom((byte) 251, customOpParameters, true, channelId);
    }

    public virtual bool OpGetPropertiesOfGame(string[] properties, byte channelId)
    {
      Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>();
      customOpParameters.Add((byte) 251, (object) LitePropertyTypes.Game);
      if (properties != null)
        customOpParameters.Add((byte) 248, (object) properties);
      return this.OpCustom((byte) 251, customOpParameters, true, channelId);
    }

    public virtual bool OpGetPropertiesOfGame(byte[] properties, byte channelId)
    {
      Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>();
      customOpParameters.Add((byte) 251, (object) LitePropertyTypes.Game);
      if (properties != null)
        customOpParameters.Add((byte) 248, (object) properties);
      return this.OpCustom((byte) 251, customOpParameters, true, channelId);
    }

    public virtual bool OpJoin(string gameName) => this.OpJoin(gameName, (Hashtable) null, (Hashtable) null, false);

    public virtual bool OpJoin(
      string gameName,
      Hashtable gameProperties,
      Hashtable actorProperties,
      bool broadcastActorProperties)
    {
      if (this.DebugOut >= DebugLevel.ALL)
        this.Listener.DebugReturn(DebugLevel.ALL, "OpJoin(" + gameName + ")");
      Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>();
      customOpParameters[byte.MaxValue] = (object) gameName;
      if (actorProperties != null)
        customOpParameters[(byte) 249] = (object) actorProperties;
      if (gameProperties != null)
        customOpParameters[(byte) 248] = (object) gameProperties;
      if (broadcastActorProperties)
        customOpParameters[(byte) 250] = (object) broadcastActorProperties;
      return this.OpCustom(byte.MaxValue, customOpParameters, true, (byte) 0, false);
    }

    public virtual bool OpLeave()
    {
      if (this.DebugOut >= DebugLevel.ALL)
        this.Listener.DebugReturn(DebugLevel.ALL, "OpLeave()");
      return this.OpCustom((byte) 254, (Dictionary<byte, object>) null, true, (byte) 0);
    }
  }
}
