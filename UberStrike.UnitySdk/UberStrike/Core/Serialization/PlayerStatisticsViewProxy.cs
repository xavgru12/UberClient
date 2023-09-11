// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerStatisticsViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerStatisticsViewProxy
  {
    public static void Serialize(Stream stream, PlayerStatisticsView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
          Int32Proxy.Serialize((Stream) bytes, instance.Headshots);
          Int64Proxy.Serialize((Stream) bytes, instance.Hits);
          Int32Proxy.Serialize((Stream) bytes, instance.Level);
          Int32Proxy.Serialize((Stream) bytes, instance.Nutshots);
          if (instance.PersonalRecord != null)
            PlayerPersonalRecordStatisticsViewProxy.Serialize((Stream) bytes, instance.PersonalRecord);
          else
            num |= 1;
          Int32Proxy.Serialize((Stream) bytes, instance.Points);
          Int64Proxy.Serialize((Stream) bytes, instance.Shots);
          Int32Proxy.Serialize((Stream) bytes, instance.Splats);
          Int32Proxy.Serialize((Stream) bytes, instance.Splatted);
          Int32Proxy.Serialize((Stream) bytes, instance.TimeSpentInGame);
          if (instance.WeaponStatistics != null)
            PlayerWeaponStatisticsViewProxy.Serialize((Stream) bytes, instance.WeaponStatistics);
          else
            num |= 2;
          Int32Proxy.Serialize((Stream) bytes, instance.Xp);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PlayerStatisticsView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlayerStatisticsView playerStatisticsView = (PlayerStatisticsView) null;
      if (num != 0)
      {
        playerStatisticsView = new PlayerStatisticsView();
        playerStatisticsView.Cmid = Int32Proxy.Deserialize(bytes);
        playerStatisticsView.Headshots = Int32Proxy.Deserialize(bytes);
        playerStatisticsView.Hits = Int64Proxy.Deserialize(bytes);
        playerStatisticsView.Level = Int32Proxy.Deserialize(bytes);
        playerStatisticsView.Nutshots = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          playerStatisticsView.PersonalRecord = PlayerPersonalRecordStatisticsViewProxy.Deserialize(bytes);
        playerStatisticsView.Points = Int32Proxy.Deserialize(bytes);
        playerStatisticsView.Shots = Int64Proxy.Deserialize(bytes);
        playerStatisticsView.Splats = Int32Proxy.Deserialize(bytes);
        playerStatisticsView.Splatted = Int32Proxy.Deserialize(bytes);
        playerStatisticsView.TimeSpentInGame = Int32Proxy.Deserialize(bytes);
        if ((num & 2) != 0)
          playerStatisticsView.WeaponStatistics = PlayerWeaponStatisticsViewProxy.Deserialize(bytes);
        playerStatisticsView.Xp = Int32Proxy.Deserialize(bytes);
      }
      return playerStatisticsView;
    }
  }
}
