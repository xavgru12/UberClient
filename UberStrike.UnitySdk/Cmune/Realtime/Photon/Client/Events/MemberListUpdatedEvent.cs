// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.Events.MemberListUpdatedEvent
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
