
using Cmune.Core.Types.Attributes;
using Cmune.Realtime.Common;

namespace UberStrike.Realtime.Common
{
  [ExtendableEnumBounds(11, 30)]
  public class UberstrikeClassID : NetworkClassID
  {
    public const short GameAICenter = 12;
  }
}
