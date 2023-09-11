// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.DateTimeProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
