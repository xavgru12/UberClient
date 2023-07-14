// Decompiled with JetBrains decompiler
// Type: GameDataNameComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

public class GameDataNameComparer : IComparer<GameMetaData>
{
  public int Compare(GameMetaData a, GameMetaData b) => GameDataNameComparer.StaticCompare(a, b);

  public static int StaticCompare(GameMetaData a, GameMetaData b) => GameDataComparer.SortAscending ? string.Compare(b.RoomName, a.RoomName) : string.Compare(a.RoomName, b.RoomName);
}
