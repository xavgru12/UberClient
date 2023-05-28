// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ItemTransactionsViewModelProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class ItemTransactionsViewModelProxy
  {
    public static void Serialize(Stream stream, ItemTransactionsViewModel instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.ItemTransactions != null)
          ListProxy<ItemTransactionView>.Serialize((Stream) bytes, (ICollection<ItemTransactionView>) instance.ItemTransactions, new ListProxy<ItemTransactionView>.Serializer<ItemTransactionView>(ItemTransactionViewProxy.Serialize));
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.TotalCount);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static ItemTransactionsViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ItemTransactionsViewModel transactionsViewModel = new ItemTransactionsViewModel();
      if ((num & 1) != 0)
        transactionsViewModel.ItemTransactions = ListProxy<ItemTransactionView>.Deserialize(bytes, new ListProxy<ItemTransactionView>.Deserializer<ItemTransactionView>(ItemTransactionViewProxy.Deserialize));
      transactionsViewModel.TotalCount = Int32Proxy.Deserialize(bytes);
      return transactionsViewModel;
    }
  }
}
