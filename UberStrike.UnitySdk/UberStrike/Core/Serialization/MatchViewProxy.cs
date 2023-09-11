
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
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static MatchView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MatchView matchView = (MatchView) null;
      if (num != 0)
      {
        matchView = new MatchView();
        matchView.GameModeId = EnumProxy<GameModeType>.Deserialize(bytes);
        matchView.MapId = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          matchView.PlayersCompleted = ListProxy<PlayerStatisticsView>.Deserialize(bytes, new ListProxy<PlayerStatisticsView>.Deserializer<PlayerStatisticsView>(PlayerStatisticsViewProxy.Deserialize));
        matchView.PlayersLimit = Int32Proxy.Deserialize(bytes);
        if ((num & 2) != 0)
          matchView.PlayersNonCompleted = ListProxy<PlayerStatisticsView>.Deserialize(bytes, new ListProxy<PlayerStatisticsView>.Deserializer<PlayerStatisticsView>(PlayerStatisticsViewProxy.Deserialize));
        matchView.TimeLimit = Int32Proxy.Deserialize(bytes);
      }
      return matchView;
    }
  }
}
