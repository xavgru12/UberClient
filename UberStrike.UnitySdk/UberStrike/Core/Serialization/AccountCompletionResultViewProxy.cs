
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
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static AccountCompletionResultView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      AccountCompletionResultView completionResultView = (AccountCompletionResultView) null;
      if (num != 0)
      {
        completionResultView = new AccountCompletionResultView();
        if ((num & 1) != 0)
          completionResultView.ItemsAttributed = DictionaryProxy<int, int>.Deserialize(bytes, new DictionaryProxy<int, int>.Deserializer<int>(Int32Proxy.Deserialize), new DictionaryProxy<int, int>.Deserializer<int>(Int32Proxy.Deserialize));
        if ((num & 2) != 0)
          completionResultView.NonDuplicateNames = ListProxy<string>.Deserialize(bytes, new ListProxy<string>.Deserializer<string>(StringProxy.Deserialize));
        completionResultView.Result = Int32Proxy.Deserialize(bytes);
      }
      return completionResultView;
    }
  }
}
