﻿
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
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PhotonView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PhotonView photonView = (PhotonView) null;
      if (num != 0)
      {
        photonView = new PhotonView();
        if ((num & 1) != 0)
          photonView.IP = StringProxy.Deserialize(bytes);
        photonView.MinLatency = Int32Proxy.Deserialize(bytes);
        if ((num & 2) != 0)
          photonView.Name = StringProxy.Deserialize(bytes);
        photonView.PhotonId = Int32Proxy.Deserialize(bytes);
        photonView.Port = Int32Proxy.Deserialize(bytes);
        photonView.Region = EnumProxy<RegionType>.Deserialize(bytes);
        photonView.UsageType = EnumProxy<PhotonUsageType>.Deserialize(bytes);
      }
      return photonView;
    }
  }
}
