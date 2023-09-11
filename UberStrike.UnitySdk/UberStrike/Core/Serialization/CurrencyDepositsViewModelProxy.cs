﻿// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.CurrencyDepositsViewModelProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
