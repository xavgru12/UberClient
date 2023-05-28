// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PointDepositsViewModelProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class PointDepositsViewModelProxy
  {
    public static void Serialize(Stream stream, PointDepositsViewModel instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.PointDeposits != null)
          ListProxy<PointDepositView>.Serialize((Stream) bytes, (ICollection<PointDepositView>) instance.PointDeposits, new ListProxy<PointDepositView>.Serializer<PointDepositView>(PointDepositViewProxy.Serialize));
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.TotalCount);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static PointDepositsViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PointDepositsViewModel depositsViewModel = new PointDepositsViewModel();
      if ((num & 1) != 0)
        depositsViewModel.PointDeposits = ListProxy<PointDepositView>.Deserialize(bytes, new ListProxy<PointDepositView>.Deserializer<PointDepositView>(PointDepositViewProxy.Deserialize));
      depositsViewModel.TotalCount = Int32Proxy.Deserialize(bytes);
      return depositsViewModel;
    }
  }
}
