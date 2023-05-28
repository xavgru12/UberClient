// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PhotonViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class PhotonViewProxy
  {
    public static void Serialize(Stream stream, PhotonView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.IP != null)
          StringProxy.Serialize((Stream) bytes, instance.IP);
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.MinLatency);
        if (instance.Name != null)
          StringProxy.Serialize((Stream) bytes, instance.Name);
        else
          num |= 2;
        Int32Proxy.Serialize((Stream) bytes, instance.PhotonId);
        Int32Proxy.Serialize((Stream) bytes, instance.Port);
        EnumProxy<RegionType>.Serialize((Stream) bytes, instance.Region);
        EnumProxy<PhotonUsageType>.Serialize((Stream) bytes, instance.UsageType);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static PhotonView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PhotonView photonView = new PhotonView();
      if ((num & 1) != 0)
        photonView.IP = StringProxy.Deserialize(bytes);
      photonView.MinLatency = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        photonView.Name = StringProxy.Deserialize(bytes);
      photonView.PhotonId = Int32Proxy.Deserialize(bytes);
      photonView.Port = Int32Proxy.Deserialize(bytes);
      photonView.Region = EnumProxy<RegionType>.Deserialize(bytes);
      photonView.UsageType = EnumProxy<PhotonUsageType>.Deserialize(bytes);
      return photonView;
    }
  }
}
