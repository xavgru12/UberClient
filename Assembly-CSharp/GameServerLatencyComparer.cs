// Decompiled with JetBrains decompiler
// Type: GameServerLatencyComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

public class GameServerLatencyComparer : IComparer<GameServerView>
{
  public int Compare(GameServerView a, GameServerView b) => GameServerLatencyComparer.StaticCompare(a, b);

  public static int StaticCompare(GameServerView a, GameServerView b)
  {
    int num1 = 1;
    int num2 = a.Data.State != ServerLoadData.Status.Alive ? 1000 : a.Latency;
    int num3 = b.Data.State != ServerLoadData.Status.Alive ? 1000 : b.Latency;
    if (a.Latency == b.Latency)
      return string.Compare(b.Name, a.Name);
    return num2 > num3 ? num1 : num1 * -1;
  }
}
