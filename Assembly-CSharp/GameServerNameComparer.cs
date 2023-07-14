// Decompiled with JetBrains decompiler
// Type: GameServerNameComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class GameServerNameComparer : IComparer<GameServerView>
{
  public int Compare(GameServerView a, GameServerView b) => GameServerNameComparer.StaticCompare(a, b);

  public static int StaticCompare(GameServerView a, GameServerView b) => string.Compare(b.Name, a.Name);
}
