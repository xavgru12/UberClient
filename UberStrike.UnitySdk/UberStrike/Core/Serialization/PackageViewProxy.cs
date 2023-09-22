
using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class PackageViewProxy
  {
    public static void Serialize(Stream stream, PackageView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.Bonus);
          if (instance.Items != null)
            ListProxy<int>.Serialize((Stream) bytes, (ICollection<int>) instance.Items, new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
          else
            num |= 1;
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 2;
          DecimalProxy.Serialize((Stream) bytes, instance.Price);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PackageView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PackageView packageView = (PackageView) null;
      if (num != 0)
      {
        packageView = new PackageView();
        packageView.Bonus = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          packageView.Items = ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
        if ((num & 2) != 0)
          packageView.Name = StringProxy.Deserialize(bytes);
        packageView.Price = DecimalProxy.Deserialize(bytes);
      }
      return packageView;
    }
  }
}
