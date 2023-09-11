// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.NetworkClassID
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types;
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(1, 10)]
  public class NetworkClassID : ExtendableEnum<short>
  {
    public const short ClientSyncCenter = 1;
    public const short ServerSyncCenter = 2;
    public const short LobbyCenter = 3;
    public const short CommCenter = 4;
  }
}
