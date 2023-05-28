// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ItemAssetBundleViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.Serialization
{
  public static class ItemAssetBundleViewProxy
  {
    public static void Serialize(Stream stream, ItemAssetBundleView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.Url != null)
          StringProxy.Serialize((Stream) bytes, instance.Url);
        else
          num |= 1;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static ItemAssetBundleView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ItemAssetBundleView itemAssetBundleView = new ItemAssetBundleView();
      if ((num & 1) != 0)
        itemAssetBundleView.Url = StringProxy.Deserialize(bytes);
      return itemAssetBundleView;
    }
  }
}
