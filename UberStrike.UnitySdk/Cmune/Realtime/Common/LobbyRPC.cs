
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(21, 50)]
  public class LobbyRPC : RPC
  {
    public const byte RegisterToLobbyUpdates = 21;
  }
}
