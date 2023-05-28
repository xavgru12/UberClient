// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberstrikeLevelViewModelProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public static UberstrikeLevelViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberstrikeLevelViewModel uberstrikeLevelViewModel = new UberstrikeLevelViewModel();
      if ((num & 1) != 0)
        uberstrikeLevelViewModel.Maps = ListProxy<MapView>.Deserialize(bytes, new ListProxy<MapView>.Deserializer<MapView>(MapViewProxy.Deserialize));
      return uberstrikeLevelViewModel;
    }
  }
}
