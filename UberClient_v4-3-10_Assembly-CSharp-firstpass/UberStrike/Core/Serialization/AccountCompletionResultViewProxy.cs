// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.AccountCompletionResultViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class AccountCompletionResultViewProxy
  {
    public static void Serialize(Stream stream, AccountCompletionResultView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.ItemsAttributed != null)
          DictionaryProxy<int, int>.Serialize((Stream) bytes, instance.ItemsAttributed, new DictionaryProxy<int, int>.Serializer<int>(Int32Proxy.Serialize), new DictionaryProxy<int, int>.Serializer<int>(Int32Proxy.Serialize));
        else
          num |= 1;
        if (instance.NonDuplicateNames != null)
          ListProxy<string>.Serialize((Stream) bytes, (ICollection<string>) instance.NonDuplicateNames, new ListProxy<string>.Serializer<string>(StringProxy.Serialize));
        else
          num |= 2;
        Int32Proxy.Serialize((Stream) bytes, instance.Result);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static AccountCompletionResultView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      AccountCompletionResultView completionResultView = new AccountCompletionResultView();
      if ((num & 1) != 0)
        completionResultView.ItemsAttributed = DictionaryProxy<int, int>.Deserialize(bytes, new DictionaryProxy<int, int>.Deserializer<int>(Int32Proxy.Deserialize), new DictionaryProxy<int, int>.Deserializer<int>(Int32Proxy.Deserialize));
      if ((num & 2) != 0)
        completionResultView.NonDuplicateNames = ListProxy<string>.Deserialize(bytes, new ListProxy<string>.Deserializer<string>(StringProxy.Deserialize));
      completionResultView.Result = Int32Proxy.Deserialize(bytes);
      return completionResultView;
    }
  }
}
