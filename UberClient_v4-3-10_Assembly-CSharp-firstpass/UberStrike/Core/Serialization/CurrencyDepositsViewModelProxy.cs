// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.CurrencyDepositsViewModelProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public static CurrencyDepositsViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      CurrencyDepositsViewModel depositsViewModel = new CurrencyDepositsViewModel();
      if ((num & 1) != 0)
        depositsViewModel.CurrencyDeposits = ListProxy<CurrencyDepositView>.Deserialize(bytes, new ListProxy<CurrencyDepositView>.Deserializer<CurrencyDepositView>(CurrencyDepositViewProxy.Deserialize));
      depositsViewModel.TotalCount = Int32Proxy.Deserialize(bytes);
      return depositsViewModel;
    }
  }
}
