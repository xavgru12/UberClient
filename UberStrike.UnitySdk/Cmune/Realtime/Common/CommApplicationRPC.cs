
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(21, 50)]
  public class CommApplicationRPC : ApplicationRPC
  {
    public const byte ProfanityCheck = 21;
  }
}
