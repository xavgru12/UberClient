// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.DecimalProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
