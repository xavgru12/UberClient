// Decompiled with JetBrains decompiler
// Type: EnumConversion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Types;

public static class EnumConversion
{
  public static short GetGameModeID(this GameModeType mode)
  {
    switch (mode)
    {
      case GameModeType.DeathMatch:
        return 101;
      case GameModeType.TeamDeathMatch:
        return 100;
      case GameModeType.EliminationMode:
        return 106;
      default:
        return 109;
    }
  }
}
