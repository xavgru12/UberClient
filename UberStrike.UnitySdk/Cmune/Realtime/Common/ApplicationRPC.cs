// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.ApplicationRPC
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types;
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(1, 20)]
  public class ApplicationRPC : ExtendableEnum<byte>
  {
    public const byte PeerSpecification = 1;
    public const byte QueryServerLoad = 2;
    public const byte QueryPerfCounters = 3;
    public const byte GeoLocalization = 4;
  }
}
