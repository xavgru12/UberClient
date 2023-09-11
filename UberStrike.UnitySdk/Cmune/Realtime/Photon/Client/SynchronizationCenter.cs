
using Cmune.Realtime.Common;
using Cmune.Realtime.Photon.Client.Events;
using Cmune.Util;

namespace Cmune.Realtime.Photon.Client
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
