// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.SynchronizationCenter
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  [NetworkClass(1)]
  public class SynchronizationCenter : ClientNetworkClass
  {
    public SynchronizationCenter(RemoteMethodInterface rmi)
      : base(rmi)
    {
    }

    [NetworkMethod(1)]
    protected void OnRecieveNetworkID(int instanceID, short networkID) => this._rmi.RecieveRegistrationConfirmation(instanceID, networkID);

    [NetworkMethod(8)]
    protected void OnRefreshMemberInfo()
    {
    }

    [NetworkMethod(9)]
    protected void OnKickPlayer(string message)
    {
      CmuneDebug.LogError("Kick Reason: " + message, new object[0]);
      CmuneEventHandler.Route((object) new PlayerBanEvent(message));
    }
  }
}
