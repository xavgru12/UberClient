// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberstrikeLevelViewModelProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models.Views;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class UberstrikeLevelViewModelProxy
  {
    public static void Serialize(Stream stream, UberstrikeLevelViewModel instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.Maps != null)
            ListProxy<MapView>.Serialize((Stream) bytes, (ICollection<MapView>) instance.Maps, new ListProxy<MapView>.Serializer<MapView>(MapViewProxy.Serialize));
          else
            num |= 1;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static UberstrikeLevelViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberstrikeLevelViewModel uberstrikeLevelViewModel = (UberstrikeLevelViewModel) null;
      if (num != 0)
      {
        uberstrikeLevelViewModel = new UberstrikeLevelViewModel();
        if ((num & 1) != 0)
          uberstrikeLevelViewModel.Maps = ListProxy<MapView>.Deserialize(bytes, new ListProxy<MapView>.Deserializer<MapView>(MapViewProxy.Deserialize));
      }
      return uberstrikeLevelViewModel;
    }
  }
}
