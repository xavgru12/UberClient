
using Cmune.Core.Types;
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(1, 20)]
  public class ClientSyncCenterRPC : ExtendableEnum<byte>
  {
    public const byte RecieveNetworkID = 1;
    public const byte LoadAssetWithID = 2;
    public const byte RemoveInstanceWithID = 3;
    public const byte RecieveStaticClassRegistration = 4;
    public const byte SynchronizeServerTime = 5;
    public const byte ClearRoom = 6;
    public const byte RoomInitialized = 7;
    public const byte RefreshMemberInfo = 8;
    public const byte KickPlayer = 9;
  }
}
