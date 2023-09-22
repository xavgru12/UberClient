
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
