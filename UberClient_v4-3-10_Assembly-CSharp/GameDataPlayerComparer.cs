// Decompiled with JetBrains decompiler
// Type: GameDataPlayerComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

public class GameDataPlayerComparer : IComparer<GameMetaData>
{
  public int Compare(GameMetaData a, GameMetaData b)
  {
    int num = a.ConnectedPlayers - b.ConnectedPlayers;
    if (num == 0)
      return GameDataNameComparer.StaticCompare(a, b);
    return GameDataComparer.SortAscending ? num : -num;
  }
}
