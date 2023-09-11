// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ShortProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ShortProxy
  {
    public static void Serialize(Stream bytes, short instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(instance);
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static short Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[2];
      bytes.Read(buffer, 0, 2);
      return BitConverter.ToInt16(buffer, 0);
    }
  }
}
