// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.BundleItemViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class BundleItemViewProxy
  {
    public static void Serialize(Stream stream, BundleItemView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.Amount);
          Int32Proxy.Serialize((Stream) bytes, instance.BundleId);
          EnumProxy<BuyingDurationType>.Serialize((Stream) bytes, instance.Duration);
          Int32Proxy.Serialize((Stream) bytes, instance.ItemId);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static BundleItemView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      BundleItemView bundleItemView = (BundleItemView) null;
      if (num != 0)
      {
        bundleItemView = new BundleItemView();
        bundleItemView.Amount = Int32Proxy.Deserialize(bytes);
        bundleItemView.BundleId = Int32Proxy.Deserialize(bytes);
        bundleItemView.Duration = EnumProxy<BuyingDurationType>.Deserialize(bytes);
        bundleItemView.ItemId = Int32Proxy.Deserialize(bytes);
      }
      return bundleItemView;
    }
  }
}
