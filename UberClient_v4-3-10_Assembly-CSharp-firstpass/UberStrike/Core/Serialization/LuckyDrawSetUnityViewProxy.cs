// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.LuckyDrawSetUnityViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class LuckyDrawSetUnityViewProxy
  {
    public static void Serialize(Stream stream, LuckyDrawSetUnityView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.CreditsAttributed);
        BooleanProxy.Serialize((Stream) bytes, instance.ExposeItemsToPlayers);
        Int32Proxy.Serialize((Stream) bytes, instance.Id);
        if (instance.ImageUrl != null)
          StringProxy.Serialize((Stream) bytes, instance.ImageUrl);
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.LuckyDrawId);
        if (instance.LuckyDrawSetItems != null)
          ListProxy<BundleItemView>.Serialize((Stream) bytes, (ICollection<BundleItemView>) instance.LuckyDrawSetItems, new ListProxy<BundleItemView>.Serializer<BundleItemView>(BundleItemViewProxy.Serialize));
        else
          num |= 2;
        Int32Proxy.Serialize((Stream) bytes, instance.PointsAttributed);
        Int32Proxy.Serialize((Stream) bytes, instance.SetWeight);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static LuckyDrawSetUnityView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      LuckyDrawSetUnityView drawSetUnityView = new LuckyDrawSetUnityView();
      drawSetUnityView.CreditsAttributed = Int32Proxy.Deserialize(bytes);
      drawSetUnityView.ExposeItemsToPlayers = BooleanProxy.Deserialize(bytes);
      drawSetUnityView.Id = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        drawSetUnityView.ImageUrl = StringProxy.Deserialize(bytes);
      drawSetUnityView.LuckyDrawId = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        drawSetUnityView.LuckyDrawSetItems = ListProxy<BundleItemView>.Deserialize(bytes, new ListProxy<BundleItemView>.Deserializer<BundleItemView>(BundleItemViewProxy.Deserialize));
      drawSetUnityView.PointsAttributed = Int32Proxy.Deserialize(bytes);
      drawSetUnityView.SetWeight = Int32Proxy.Deserialize(bytes);
      return drawSetUnityView;
    }
  }
}
