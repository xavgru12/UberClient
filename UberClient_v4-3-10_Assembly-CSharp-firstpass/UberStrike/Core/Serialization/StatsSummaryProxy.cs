// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.StatsSummaryProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization
{
  public static class StatsSummaryProxy
  {
    public static void Serialize(Stream stream, StatsSummary instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.Achievements != null)
          DictionaryProxy<byte, ushort>.Serialize((Stream) bytes, instance.Achievements, new DictionaryProxy<byte, ushort>.Serializer<byte>(ByteProxy.Serialize), new DictionaryProxy<byte, ushort>.Serializer<ushort>(UInt16Proxy.Serialize));
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        Int32Proxy.Serialize((Stream) bytes, instance.Deaths);
        Int32Proxy.Serialize((Stream) bytes, instance.Kills);
        Int32Proxy.Serialize((Stream) bytes, instance.Level);
        if (instance.Name != null)
          StringProxy.Serialize((Stream) bytes, instance.Name);
        else
          num |= 2;
        EnumProxy<TeamID>.Serialize((Stream) bytes, instance.Team);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static StatsSummary Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      StatsSummary statsSummary = new StatsSummary();
      if ((num & 1) != 0)
        statsSummary.Achievements = DictionaryProxy<byte, ushort>.Deserialize(bytes, new DictionaryProxy<byte, ushort>.Deserializer<byte>(ByteProxy.Deserialize), new DictionaryProxy<byte, ushort>.Deserializer<ushort>(UInt16Proxy.Deserialize));
      statsSummary.Cmid = Int32Proxy.Deserialize(bytes);
      statsSummary.Deaths = Int32Proxy.Deserialize(bytes);
      statsSummary.Kills = Int32Proxy.Deserialize(bytes);
      statsSummary.Level = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        statsSummary.Name = StringProxy.Deserialize(bytes);
      statsSummary.Team = EnumProxy<TeamID>.Deserialize(bytes);
      return statsSummary;
    }
  }
}
