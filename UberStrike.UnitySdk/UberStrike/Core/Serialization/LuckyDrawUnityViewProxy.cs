// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.LuckyDrawUnityViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class LuckyDrawUnityViewProxy
  {
    public static void Serialize(Stream stream, LuckyDrawUnityView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          EnumProxy<BundleCategoryType>.Serialize((Stream) bytes, instance.Category);
          if (instance.Description != null)
            StringProxy.Serialize((Stream) bytes, instance.Description);
          else
            num |= 1;
          if (instance.IconUrl != null)
            StringProxy.Serialize((Stream) bytes, instance.IconUrl);
          else
            num |= 2;
          Int32Proxy.Serialize((Stream) bytes, instance.Id);
          BooleanProxy.Serialize((Stream) bytes, instance.IsAvailableInShop);
          if (instance.LuckyDrawSets != null)
            ListProxy<LuckyDrawSetUnityView>.Serialize((Stream) bytes, (ICollection<LuckyDrawSetUnityView>) instance.LuckyDrawSets, new ListProxy<LuckyDrawSetUnityView>.Serializer<LuckyDrawSetUnityView>(LuckyDrawSetUnityViewProxy.Serialize));
          else
            num |= 4;
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 8;
          Int32Proxy.Serialize((Stream) bytes, instance.Price);
          EnumProxy<UberStrikeCurrencyType>.Serialize((Stream) bytes, instance.UberStrikeCurrencyType);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static LuckyDrawUnityView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      LuckyDrawUnityView luckyDrawUnityView = (LuckyDrawUnityView) null;
      if (num != 0)
      {
        luckyDrawUnityView = new LuckyDrawUnityView();
        luckyDrawUnityView.Category = EnumProxy<BundleCategoryType>.Deserialize(bytes);
        if ((num & 1) != 0)
          luckyDrawUnityView.Description = StringProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          luckyDrawUnityView.IconUrl = StringProxy.Deserialize(bytes);
        luckyDrawUnityView.Id = Int32Proxy.Deserialize(bytes);
        luckyDrawUnityView.IsAvailableInShop = BooleanProxy.Deserialize(bytes);
        if ((num & 4) != 0)
          luckyDrawUnityView.LuckyDrawSets = ListProxy<LuckyDrawSetUnityView>.Deserialize(bytes, new ListProxy<LuckyDrawSetUnityView>.Deserializer<LuckyDrawSetUnityView>(LuckyDrawSetUnityViewProxy.Deserialize));
        if ((num & 8) != 0)
          luckyDrawUnityView.Name = StringProxy.Deserialize(bytes);
        luckyDrawUnityView.Price = Int32Proxy.Deserialize(bytes);
        luckyDrawUnityView.UberStrikeCurrencyType = EnumProxy<UberStrikeCurrencyType>.Deserialize(bytes);
      }
      return luckyDrawUnityView;
    }
  }
}
