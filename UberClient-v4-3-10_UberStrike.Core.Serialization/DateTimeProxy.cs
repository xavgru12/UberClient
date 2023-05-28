// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.DateTimeProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class DateTimeProxy
  {
    public static void Serialize(Stream bytes, DateTime instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(instance.Ticks);
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static DateTime Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[8];
      bytes.Read(buffer, 0, 8);
      return new DateTime(BitConverter.ToInt64(buffer, 0));
    }
  }
}
