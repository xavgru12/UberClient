// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PointDepositViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class PointDepositViewProxy
  {
    public static void Serialize(Stream stream, PointDepositView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
          DateTimeProxy.Serialize((Stream) bytes, instance.DepositDate);
          EnumProxy<PointsDepositType>.Serialize((Stream) bytes, instance.DepositType);
          BooleanProxy.Serialize((Stream) bytes, instance.IsAdminAction);
          Int32Proxy.Serialize((Stream) bytes, instance.PointDepositId);
          Int32Proxy.Serialize((Stream) bytes, instance.Points);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PointDepositView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PointDepositView pointDepositView = (PointDepositView) null;
      if (num != 0)
      {
        pointDepositView = new PointDepositView();
        pointDepositView.Cmid = Int32Proxy.Deserialize(bytes);
        pointDepositView.DepositDate = DateTimeProxy.Deserialize(bytes);
        pointDepositView.DepositType = EnumProxy<PointsDepositType>.Deserialize(bytes);
        pointDepositView.IsAdminAction = BooleanProxy.Deserialize(bytes);
        pointDepositView.PointDepositId = Int32Proxy.Deserialize(bytes);
        pointDepositView.Points = Int32Proxy.Deserialize(bytes);
      }
      return pointDepositView;
    }
  }
}
