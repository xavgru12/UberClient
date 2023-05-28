// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MatchViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class MatchViewProxy
  {
    public static void Serialize(Stream stream, MatchView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<GameModeType>.Serialize((Stream) bytes, instance.GameModeId);
        Int32Proxy.Serialize((Stream) bytes, instance.MapId);
        if (instance.PlayersCompleted != null)
          ListProxy<PlayerStatisticsView>.Serialize((Stream) bytes, (ICollection<PlayerStatisticsView>) instance.PlayersCompleted, new ListProxy<PlayerStatisticsView>.Serializer<PlayerStatisticsView>(PlayerStatisticsViewProxy.Serialize));
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.PlayersLimit);
        if (instance.PlayersNonCompleted != null)
          ListProxy<PlayerStatisticsView>.Serialize((Stream) bytes, (ICollection<PlayerStatisticsView>) instance.PlayersNonCompleted, new ListProxy<PlayerStatisticsView>.Serializer<PlayerStatisticsView>(PlayerStatisticsViewProxy.Serialize));
        else
          num |= 2;
        Int32Proxy.Serialize((Stream) bytes, instance.TimeLimit);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static MatchView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MatchView matchView = new MatchView();
      matchView.GameModeId = EnumProxy<GameModeType>.Deserialize(bytes);
      matchView.MapId = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        matchView.PlayersCompleted = ListProxy<PlayerStatisticsView>.Deserialize(bytes, new ListProxy<PlayerStatisticsView>.Deserializer<PlayerStatisticsView>(PlayerStatisticsViewProxy.Deserialize));
      matchView.PlayersLimit = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        matchView.PlayersNonCompleted = ListProxy<PlayerStatisticsView>.Deserialize(bytes, new ListProxy<PlayerStatisticsView>.Deserializer<PlayerStatisticsView>(PlayerStatisticsViewProxy.Deserialize));
      matchView.TimeLimit = Int32Proxy.Deserialize(bytes);
      return matchView;
    }
  }
}
