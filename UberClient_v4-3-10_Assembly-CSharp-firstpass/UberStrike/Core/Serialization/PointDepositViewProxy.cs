// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PointDepositViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class PointDepositViewProxy
  {
    public static void Serialize(Stream stream, PointDepositView instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        DateTimeProxy.Serialize((Stream) bytes, instance.DepositDate);
        EnumProxy<PointsDepositType>.Serialize((Stream) bytes, instance.DepositType);
        BooleanProxy.Serialize((Stream) bytes, instance.IsAdminAction);
        Int32Proxy.Serialize((Stream) bytes, instance.PointDepositId);
        Int32Proxy.Serialize((Stream) bytes, instance.Points);
        bytes.WriteTo(stream);
      }
    }

    public static PointDepositView Deserialize(Stream bytes) => new PointDepositView()
    {
      Cmid = Int32Proxy.Deserialize(bytes),
      DepositDate = DateTimeProxy.Deserialize(bytes),
      DepositType = EnumProxy<PointsDepositType>.Deserialize(bytes),
      IsAdminAction = BooleanProxy.Deserialize(bytes),
      PointDepositId = Int32Proxy.Deserialize(bytes),
      Points = Int32Proxy.Deserialize(bytes)
    };
  }
}
