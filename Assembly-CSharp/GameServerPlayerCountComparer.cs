// Decompiled with JetBrains decompiler
// Type: GameServerPlayerCountComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

public class GameServerPlayerCountComparer : IComparer<GameServerView>
{
  public int Compare(GameServerView a, GameServerView b) => GameServerPlayerCountComparer.StaticCompare(a, b);

  public static int StaticCompare(GameServerView a, GameServerView b)
  {
    int num = 1;
    if (a.Data.PlayersConnected == b.Data.PlayersConnected)
      return string.Compare(b.Name, a.Name);
    return (a.Data.State != ServerLoadData.Status.Alive ? 1000 : a.Data.PlayersConnected) > (b.Data.State != ServerLoadData.Status.Alive ? 1000 : b.Data.PlayersConnected) ? num : num * -1;
  }
}
