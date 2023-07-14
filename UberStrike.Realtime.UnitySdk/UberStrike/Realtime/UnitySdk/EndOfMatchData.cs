// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.EndOfMatchData
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  [Serializable]
  public class EndOfMatchData : IByteArray
  {
    public int RoundNumber { get; set; }

    public List<StatsSummary> MostValuablePlayers { get; set; }

    public int MostEffecientWeaponId { get; set; }

    public StatsCollection PlayerStatsTotal { get; set; }

    public StatsCollection PlayerStatsBestPerLife { get; set; }

    public Dictionary<byte, ushort> PlayerXpEarned { get; set; }

    public EndOfMatchData()
    {
    }

    public EndOfMatchData(byte[] bytes, ref int idx) => idx = this.FromBytes(bytes, idx);

    public byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>();
      DefaultByteConverter.FromInt(this.MostEffecientWeaponId, ref bytes);
      DefaultByteConverter.FromByte((byte) this.MostValuablePlayers.Count, ref bytes);
      foreach (StatsSummary mostValuablePlayer in this.MostValuablePlayers)
      {
        DefaultByteConverter.FromString(mostValuablePlayer.Name, ref bytes);
        DefaultByteConverter.FromInt(mostValuablePlayer.Kills, ref bytes);
        DefaultByteConverter.FromInt(mostValuablePlayer.Deaths, ref bytes);
        DefaultByteConverter.FromByte((byte) mostValuablePlayer.Level, ref bytes);
        DefaultByteConverter.FromByte((byte) mostValuablePlayer.Team, ref bytes);
        DefaultByteConverter.FromInt(mostValuablePlayer.Cmid, ref bytes);
        DefaultByteConverter.FromByteCollection((ICollection<byte>) mostValuablePlayer.Achievements.Keys, ref bytes);
        DefaultByteConverter.FromUShortCollection((ICollection<ushort>) mostValuablePlayer.Achievements.Values, ref bytes);
      }
      bytes.AddRange((IEnumerable<byte>) this.PlayerStatsTotal.GetBytes());
      bytes.AddRange((IEnumerable<byte>) this.PlayerStatsBestPerLife.GetBytes());
      DefaultByteConverter.FromByteCollection((ICollection<byte>) this.PlayerXpEarned.Keys, ref bytes);
      DefaultByteConverter.FromUShortCollection((ICollection<ushort>) this.PlayerXpEarned.Values, ref bytes);
      return bytes.ToArray();
    }

    public int FromBytes(byte[] bytes, int idx)
    {
      this.MostEffecientWeaponId = DefaultByteConverter.ToInt(bytes, ref idx);
      int capacity = (int) DefaultByteConverter.ToByte(bytes, ref idx);
      this.MostValuablePlayers = new List<StatsSummary>(capacity);
      for (int index1 = 0; index1 < capacity; ++index1)
      {
        StatsSummary statsSummary = new StatsSummary();
        statsSummary.Name = DefaultByteConverter.ToString(bytes, ref idx);
        statsSummary.Kills = DefaultByteConverter.ToInt(bytes, ref idx);
        statsSummary.Deaths = DefaultByteConverter.ToInt(bytes, ref idx);
        statsSummary.Level = (int) DefaultByteConverter.ToByte(bytes, ref idx);
        statsSummary.Team = (TeamID) DefaultByteConverter.ToByte(bytes, ref idx);
        statsSummary.Cmid = DefaultByteConverter.ToInt(bytes, ref idx);
        List<byte> byteCollection = DefaultByteConverter.ToByteCollection(bytes, ref idx);
        List<ushort> ushortCollection = DefaultByteConverter.ToUShortCollection(bytes, ref idx);
        statsSummary.Achievements = new Dictionary<byte, ushort>();
        for (int index2 = 0; index2 < byteCollection.Count && index2 < ushortCollection.Count; ++index2)
          statsSummary.Achievements.Add(byteCollection[index2], ushortCollection[index2]);
        this.MostValuablePlayers.Add(statsSummary);
      }
      this.PlayerStatsTotal = new StatsCollection(bytes, ref idx);
      this.PlayerStatsBestPerLife = new StatsCollection(bytes, ref idx);
      List<byte> byteCollection1 = DefaultByteConverter.ToByteCollection(bytes, ref idx);
      List<ushort> ushortCollection1 = DefaultByteConverter.ToUShortCollection(bytes, ref idx);
      this.PlayerXpEarned = new Dictionary<byte, ushort>();
      for (int index = 0; index < byteCollection1.Count && index < ushortCollection1.Count; ++index)
        this.PlayerXpEarned.Add(byteCollection1[index], ushortCollection1[index]);
      return idx;
    }
  }
}
