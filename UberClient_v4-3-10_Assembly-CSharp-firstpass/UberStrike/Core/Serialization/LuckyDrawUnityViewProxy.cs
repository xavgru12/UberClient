// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.LuckyDrawUnityViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public static LuckyDrawUnityView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      LuckyDrawUnityView luckyDrawUnityView = new LuckyDrawUnityView();
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
      return luckyDrawUnityView;
    }
  }
}
