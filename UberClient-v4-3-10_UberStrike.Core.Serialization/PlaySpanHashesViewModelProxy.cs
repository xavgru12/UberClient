// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlaySpanHashesViewModelProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

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
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PlaySpanHashesViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlaySpanHashesViewModel spanHashesViewModel = (PlaySpanHashesViewModel) null;
      if (num != 0)
      {
        spanHashesViewModel = new PlaySpanHashesViewModel();
        if ((num & 1) != 0)
          spanHashesViewModel.Hashes = DictionaryProxy<Decimal, string>.Deserialize(bytes, new DictionaryProxy<Decimal, string>.Deserializer<Decimal>(DecimalProxy.Deserialize), new DictionaryProxy<Decimal, string>.Deserializer<string>(StringProxy.Deserialize));
        if ((num & 2) != 0)
          spanHashesViewModel.MerchTrans = StringProxy.Deserialize(bytes);
      }
      return spanHashesViewModel;
    }
  }
}
