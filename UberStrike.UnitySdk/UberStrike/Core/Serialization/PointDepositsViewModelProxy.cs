
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
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PointDepositsViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PointDepositsViewModel depositsViewModel = (PointDepositsViewModel) null;
      if (num != 0)
      {
        depositsViewModel = new PointDepositsViewModel();
        if ((num & 1) != 0)
          depositsViewModel.PointDeposits = ListProxy<PointDepositView>.Deserialize(bytes, new ListProxy<PointDepositView>.Deserializer<PointDepositView>(PointDepositViewProxy.Deserialize));
        depositsViewModel.TotalCount = Int32Proxy.Deserialize(bytes);
      }
      return depositsViewModel;
    }
  }
}
