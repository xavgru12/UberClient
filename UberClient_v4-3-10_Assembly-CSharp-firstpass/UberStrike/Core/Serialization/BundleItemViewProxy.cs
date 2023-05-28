// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.BundleItemViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class BundleItemViewProxy
  {
    public static void Serialize(Stream stream, BundleItemView instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.Amount);
        Int32Proxy.Serialize((Stream) bytes, instance.BundleId);
        EnumProxy<BuyingDurationType>.Serialize((Stream) bytes, instance.Duration);
        Int32Proxy.Serialize((Stream) bytes, instance.ItemId);
        bytes.WriteTo(stream);
      }
    }

    public static BundleItemView Deserialize(Stream bytes) => new BundleItemView()
    {
      Amount = Int32Proxy.Deserialize(bytes),
      BundleId = Int32Proxy.Deserialize(bytes),
      Duration = EnumProxy<BuyingDurationType>.Deserialize(bytes),
      ItemId = Int32Proxy.Deserialize(bytes)
    };
  }
}
