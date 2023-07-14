// Decompiled with JetBrains decompiler
// Type: GameDataAccessComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

public class GameDataAccessComparer : IComparer<GameMetaData>
{
  public int Compare(GameMetaData a, GameMetaData b)
  {
    int num = 0;
    if (GameDataComparer.SortAscending)
    {
      if (a.IsPublic && b.IsPublic)
        num = 2;
      else if (a.IsPublic)
        num = 1;
      else if (b.IsPublic)
        num = -1;
    }
    else if (!a.IsPublic && !b.IsPublic)
      num = 2;
    else if (!a.IsPublic)
      num = 1;
    else if (!b.IsPublic)
      num = -1;
    return num;
  }
}
