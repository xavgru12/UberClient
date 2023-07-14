// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.DecimalProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

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
