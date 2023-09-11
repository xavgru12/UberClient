// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.BundleItemViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
