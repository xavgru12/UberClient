
using Cmune.Realtime.Common;
using Cmune.Realtime.Common.Utils;
using System.Collections.Generic;

namespace Cmune.Realtime.Photon.Client.Events
{
  public class MemberListUpdatedEvent
  {
    public ActorInfo[] Players;

    public MemberListUpdatedEvent(ICollection<ActorInfo> info) => this.Players = Conversion.ToArray<ActorInfo>(info);
  }
}
