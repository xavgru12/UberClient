// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.Captions
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  public static class Captions
  {
    private static readonly string[] _gameModes = new string[7]
    {
      "Free for All",
      "Team VS Team",
      "Capture The Flag",
      "Just For Fun",
      "Cooperation",
      "Elimination",
      "Last Man Standing"
    };

    public static string GetGameMode(short mode)
    {
      switch (mode)
      {
        case 100:
          return Captions._gameModes[1];
        case 101:
          return Captions._gameModes[0];
        case 102:
          return Captions._gameModes[3];
        case 103:
          return Captions._gameModes[4];
        case 104:
          return Captions._gameModes[2];
        case 105:
          return Captions._gameModes[6];
        case 106:
          return Captions._gameModes[5];
        default:
          return "<Unknown GameMode>";
      }
    }
  }
}
