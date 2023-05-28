// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerMatchStatsProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerMatchStatsProxy
  {
    public static void Serialize(Stream stream, PlayerMatchStats instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        Int32Proxy.Serialize((Stream) bytes, instance.Death);
        BooleanProxy.Serialize((Stream) bytes, instance.HasFinishedMatch);
        BooleanProxy.Serialize((Stream) bytes, instance.HasWonMatch);
        Int32Proxy.Serialize((Stream) bytes, instance.Headshots);
        Int64Proxy.Serialize((Stream) bytes, instance.Hits);
        Int32Proxy.Serialize((Stream) bytes, instance.Kills);
        Int32Proxy.Serialize((Stream) bytes, instance.Nutshots);
        if (instance.PersonalRecord != null)
          PlayerPersonalRecordStatisticsViewProxy.Serialize((Stream) bytes, instance.PersonalRecord);
        else
          num |= 1;
        Int64Proxy.Serialize((Stream) bytes, instance.Shots);
        Int32Proxy.Serialize((Stream) bytes, instance.Smackdowns);
        Int32Proxy.Serialize((Stream) bytes, instance.TimeSpentInGame);
        if (instance.WeaponStatistics != null)
          PlayerWeaponStatisticsViewProxy.Serialize((Stream) bytes, instance.WeaponStatistics);
        else
          num |= 2;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static PlayerMatchStats Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlayerMatchStats playerMatchStats = new PlayerMatchStats();
      playerMatchStats.Cmid = Int32Proxy.Deserialize(bytes);
      playerMatchStats.Death = Int32Proxy.Deserialize(bytes);
      playerMatchStats.HasFinishedMatch = BooleanProxy.Deserialize(bytes);
      playerMatchStats.HasWonMatch = BooleanProxy.Deserialize(bytes);
      playerMatchStats.Headshots = Int32Proxy.Deserialize(bytes);
      playerMatchStats.Hits = Int64Proxy.Deserialize(bytes);
      playerMatchStats.Kills = Int32Proxy.Deserialize(bytes);
      playerMatchStats.Nutshots = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        playerMatchStats.PersonalRecord = PlayerPersonalRecordStatisticsViewProxy.Deserialize(bytes);
      playerMatchStats.Shots = Int64Proxy.Deserialize(bytes);
      playerMatchStats.Smackdowns = Int32Proxy.Deserialize(bytes);
      playerMatchStats.TimeSpentInGame = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        playerMatchStats.WeaponStatistics = PlayerWeaponStatisticsViewProxy.Deserialize(bytes);
      return playerMatchStats;
    }
  }
}
