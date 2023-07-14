// Decompiled with JetBrains decompiler
// Type: GameDataLatencyComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

public class GameDataLatencyComparer : IComparer<GameMetaData>
{
  public int Compare(GameMetaData a, GameMetaData b)
  {
    int num = a.Latency - b.Latency;
    return GameDataComparer.SortAscending ? num : -num;
  }
}
