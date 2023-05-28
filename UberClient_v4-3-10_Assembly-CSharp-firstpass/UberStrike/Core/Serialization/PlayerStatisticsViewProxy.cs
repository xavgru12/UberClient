// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerStatisticsViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerStatisticsViewProxy
  {
    public static void Serialize(Stream stream, PlayerStatisticsView instance)
    {
      int num = 0;
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

    public static PlayerStatisticsView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlayerStatisticsView playerStatisticsView = new PlayerStatisticsView();
      playerStatisticsView.Cmid = Int32Proxy.Deserialize(bytes);
      playerStatisticsView.Headshots = Int32Proxy.Deserialize(bytes);
      playerStatisticsView.Hits = Int64Proxy.Deserialize(bytes);
      playerStatisticsView.Level = Int32Proxy.Deserialize(bytes);
      playerStatisticsView.Nutshots = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        playerStatisticsView.PersonalRecord = PlayerPersonalRecordStatisticsViewProxy.Deserialize(bytes);
      playerStatisticsView.Shots = Int64Proxy.Deserialize(bytes);
      playerStatisticsView.Splats = Int32Proxy.Deserialize(bytes);
      playerStatisticsView.Splatted = Int32Proxy.Deserialize(bytes);
      playerStatisticsView.TimeSpentInGame = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        playerStatisticsView.WeaponStatistics = PlayerWeaponStatisticsViewProxy.Deserialize(bytes);
      playerStatisticsView.Xp = Int32Proxy.Deserialize(bytes);
      return playerStatisticsView;
    }
  }
}
