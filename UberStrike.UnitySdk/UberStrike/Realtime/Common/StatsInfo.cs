// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.StatsInfo
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common;
using Cmune.Realtime.Common.IO;
using System.Collections.Generic;

namespace UberStrike.Realtime.Common
{
  public class StatsInfo : IByteArray
  {
    public short Kills { get; set; }

    public short Deaths { get; set; }

    public ushort XP { get; set; }

    public ushort Points { get; set; }

    public StatsInfo()
    {
    }

    public StatsInfo(byte[] bytes, ref int index)
      : this()
    {
      index = this.FromBytes(bytes, index);
    }

    public byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>(16);
      DefaultByteConverter.FromShort(this.Kills, ref bytes);
      DefaultByteConverter.FromShort(this.Deaths, ref bytes);
      DefaultByteConverter.FromUShort(this.XP, ref bytes);
      DefaultByteConverter.FromUShort(this.Points, ref bytes);
      return bytes.ToArray();
    }

    public int FromBytes(byte[] bytes, int idx)
    {
      this.Kills = DefaultByteConverter.ToShort(bytes, ref idx);
      this.Deaths = DefaultByteConverter.ToShort(bytes, ref idx);
      this.XP = DefaultByteConverter.ToUShort(bytes, ref idx);
      this.Points = DefaultByteConverter.ToUShort(bytes, ref idx);
      return idx;
    }

    public override int GetHashCode() => (int) this.Kills + (int) this.Deaths + (int) this.XP + (int) this.Points;

    public override bool Equals(object obj) => !object.ReferenceEquals(obj, (object) null) && this.GetHashCode() == obj.GetHashCode();

    public override string ToString() => string.Format("{0}/{1} {2}/{3}%", (object) this.Kills, (object) this.Deaths, (object) this.XP, (object) this.Points);
  }
}
