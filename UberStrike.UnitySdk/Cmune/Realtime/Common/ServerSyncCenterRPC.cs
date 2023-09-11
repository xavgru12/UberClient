// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.ServerSyncCenterRPC
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types;
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(1, 20)]
  public class ServerSyncCenterRPC : ExtendableEnum<byte>
  {
    public const byte UnregisterNetworkClass = 1;
    public const byte RegisterMonoNetworkGameObject = 2;
    public const byte RegisterStaticNetworkClass = 3;
    public const byte InitializeRoomOnClient = 4;
    public const byte SynchronizeProperties = 5;
    public const byte NetworkPhysicsUpdate = 6;
    public const byte MailNotification = 7;
  }
}
