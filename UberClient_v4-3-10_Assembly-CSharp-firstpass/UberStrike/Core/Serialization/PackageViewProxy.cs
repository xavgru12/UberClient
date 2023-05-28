// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PackageViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public static PackageView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PackageView packageView = new PackageView();
      packageView.Bonus = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        packageView.Items = ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
      if ((num & 2) != 0)
        packageView.Name = StringProxy.Deserialize(bytes);
      packageView.Price = DecimalProxy.Deserialize(bytes);
      return packageView;
    }
  }
}
