// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.CurrencyDepositsViewModelProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class CurrencyDepositsViewModelProxy
  {
    public static void Serialize(Stream stream, CurrencyDepositsViewModel instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.CurrencyDeposits != null)
            ListProxy<CurrencyDepositView>.Serialize((Stream) bytes, (ICollection<CurrencyDepositView>) instance.CurrencyDeposits, new ListProxy<CurrencyDepositView>.Serializer<CurrencyDepositView>(CurrencyDepositViewProxy.Serialize));
          else
            num |= 1;
          Int32Proxy.Serialize((Stream) bytes, instance.TotalCount);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static CurrencyDepositsViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      CurrencyDepositsViewModel depositsViewModel = (CurrencyDepositsViewModel) null;
      if (num != 0)
      {
        depositsViewModel = new CurrencyDepositsViewModel();
        if ((num & 1) != 0)
          depositsViewModel.CurrencyDeposits = ListProxy<CurrencyDepositView>.Deserialize(bytes, new ListProxy<CurrencyDepositView>.Deserializer<CurrencyDepositView>(CurrencyDepositViewProxy.Deserialize));
        depositsViewModel.TotalCount = Int32Proxy.Deserialize(bytes);
      }
      return depositsViewModel;
    }
  }
}
