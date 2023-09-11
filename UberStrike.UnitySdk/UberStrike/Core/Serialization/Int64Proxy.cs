// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.Int64Proxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class Int64Proxy
  {
    public static void Serialize(Stream bytes, long instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(instance);
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static long Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[8];
      bytes.Read(buffer, 0, 8);
      return BitConverter.ToInt64(buffer, 0);
    }
  }
}
