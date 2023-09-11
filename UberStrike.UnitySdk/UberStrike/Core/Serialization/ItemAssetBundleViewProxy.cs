
using System.IO;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.Serialization
{
  public static class ItemAssetBundleViewProxy
  {
    public static void Serialize(Stream stream, ItemAssetBundleView instance)
    {
      int num = 0;
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ItemAssetBundleView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ItemAssetBundleView itemAssetBundleView = (ItemAssetBundleView) null;
      if (num != 0)
      {
        itemAssetBundleView = new ItemAssetBundleView();
        if ((num & 1) != 0)
          itemAssetBundleView.Url = StringProxy.Deserialize(bytes);
      }
      return itemAssetBundleView;
    }
  }
}
