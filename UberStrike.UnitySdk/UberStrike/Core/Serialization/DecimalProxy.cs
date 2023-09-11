
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
