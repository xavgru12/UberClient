// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.DecimalProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class DecimalProxy
  {
    public static void Serialize(Stream bytes, Decimal instance)
    {
      int[] bits = Decimal.GetBits(instance);
      Int32Proxy.Serialize(bytes, bits[0]);
      Int32Proxy.Serialize(bytes, bits[1]);
      Int32Proxy.Serialize(bytes, bits[2]);
      Int32Proxy.Serialize(bytes, bits[3]);
    }

    public static Decimal Deserialize(Stream bytes) => new Decimal(new int[4]
    {
      Int32Proxy.Deserialize(bytes),
      Int32Proxy.Deserialize(bytes),
      Int32Proxy.Deserialize(bytes),
      Int32Proxy.Deserialize(bytes)
    });
  }
}
