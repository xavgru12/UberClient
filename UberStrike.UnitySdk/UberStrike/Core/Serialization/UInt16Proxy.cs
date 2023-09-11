// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UInt16Proxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class UInt16Proxy
  {
    public static void Serialize(Stream bytes, ushort instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(instance);
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static ushort Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[2];
      bytes.Read(buffer, 0, 2);
      return BitConverter.ToUInt16(buffer, 0);
    }
  }
}
