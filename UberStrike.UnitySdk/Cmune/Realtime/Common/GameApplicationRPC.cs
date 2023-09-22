
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(21, 50)]
  public class GameApplicationRPC : ApplicationRPC
  {
    public const byte RoomRequest = 21;
    public const byte BanPlayer = 22;
    public const byte CustomMessage = 23;
    public const byte MutePlayer = 24;
    public const byte UnmutePlayer = 25;
    public const byte GhostPlayer = 26;
  }
}
