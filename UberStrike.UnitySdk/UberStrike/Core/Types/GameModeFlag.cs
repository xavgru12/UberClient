
using System;

namespace UberStrike.Core.Types
{
  [Flags]
  public enum GameModeFlag
  {
    None = 0,
    All = -1, // 0xFFFFFFFF
    DeathMatch = 1,
    TeamDeathMatch = 2,
    EliminationMode = 4,
  }
}
