// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MatchStatsProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class MatchStatsProxy
  {
    public static void Serialize(Stream stream, MatchStats instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<GameModeType>.Serialize((Stream) bytes, instance.GameModeId);
        Int32Proxy.Serialize((Stream) bytes, instance.MapId);
        if (instance.Players != null)
          ListProxy<PlayerMatchStats>.Serialize((Stream) bytes, (ICollection<PlayerMatchStats>) instance.Players, new ListProxy<PlayerMatchStats>.Serializer<PlayerMatchStats>(PlayerMatchStatsProxy.Serialize));
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.PlayersLimit);
        Int32Proxy.Serialize((Stream) bytes, instance.TimeLimit);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static MatchStats Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MatchStats matchStats = new MatchStats();
      matchStats.GameModeId = EnumProxy<GameModeType>.Deserialize(bytes);
      matchStats.MapId = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        matchStats.Players = ListProxy<PlayerMatchStats>.Deserialize(bytes, new ListProxy<PlayerMatchStats>.Deserializer<PlayerMatchStats>(PlayerMatchStatsProxy.Deserialize));
      matchStats.PlayersLimit = Int32Proxy.Deserialize(bytes);
      matchStats.TimeLimit = Int32Proxy.Deserialize(bytes);
      return matchStats;
    }
  }
}
