// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlaySpanHashesViewModelProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class PlaySpanHashesViewModelProxy
  {
    public static void Serialize(Stream stream, PlaySpanHashesViewModel instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.Hashes != null)
          DictionaryProxy<Decimal, string>.Serialize((Stream) bytes, instance.Hashes, new DictionaryProxy<Decimal, string>.Serializer<Decimal>(DecimalProxy.Serialize), new DictionaryProxy<Decimal, string>.Serializer<string>(StringProxy.Serialize));
        else
          num |= 1;
        if (instance.MerchTrans != null)
          StringProxy.Serialize((Stream) bytes, instance.MerchTrans);
        else
          num |= 2;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static PlaySpanHashesViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlaySpanHashesViewModel spanHashesViewModel = new PlaySpanHashesViewModel();
      if ((num & 1) != 0)
        spanHashesViewModel.Hashes = DictionaryProxy<Decimal, string>.Deserialize(bytes, new DictionaryProxy<Decimal, string>.Deserializer<Decimal>(DecimalProxy.Deserialize), new DictionaryProxy<Decimal, string>.Deserializer<string>(StringProxy.Deserialize));
      if ((num & 2) != 0)
        spanHashesViewModel.MerchTrans = StringProxy.Deserialize(bytes);
      return spanHashesViewModel;
    }
  }
}
