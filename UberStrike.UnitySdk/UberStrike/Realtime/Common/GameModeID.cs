
using Cmune.Core.Types.Attributes;

namespace UberStrike.Realtime.Common
{
  [ExtendableEnumBounds(100, 150)]
  public class GameModeID : UberstrikeClassID
  {
    public const short TeamDeathMatch = 100;
    public const short DeathMatch = 101;
    public const short FunMode = 102;
    public const short CooperationMode = 103;
    public const short CaptureTheFlag = 104;
    public const short LastManStanding = 105;
    public const short EliminationMode = 106;
    public const short ModerationMode = 107;
  }
}
