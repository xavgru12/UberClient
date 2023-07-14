// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ApplicationViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ApplicationViewProxy
  {
    public static void Serialize(Stream stream, ApplicationView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.ApplicationVersionId);
          EnumProxy<BuildType>.Serialize((Stream) bytes, instance.Build);
          EnumProxy<ChannelType>.Serialize((Stream) bytes, instance.Channel);
          if (instance.ExpirationDate.HasValue)
            DateTimeProxy.Serialize((Stream) bytes, instance.ExpirationDate ?? new DateTime());
          else
            num |= 1;
          if (instance.FileName != null)
            StringProxy.Serialize((Stream) bytes, instance.FileName);
          else
            num |= 2;
          BooleanProxy.Serialize((Stream) bytes, instance.IsCurrent);
          Int32Proxy.Serialize((Stream) bytes, instance.PhotonGroupId);
          if (instance.PhotonGroupName != null)
            StringProxy.Serialize((Stream) bytes, instance.PhotonGroupName);
          else
            num |= 4;
          DateTimeProxy.Serialize((Stream) bytes, instance.ReleaseDate);
          Int32Proxy.Serialize((Stream) bytes, instance.RemainingTime);
          if (instance.Servers != null)
            ListProxy<PhotonView>.Serialize((Stream) bytes, (ICollection<PhotonView>) instance.Servers, new ListProxy<PhotonView>.Serializer<PhotonView>(PhotonViewProxy.Serialize));
          else
            num |= 8;
          if (instance.SupportUrl != null)
            StringProxy.Serialize((Stream) bytes, instance.SupportUrl);
          else
            num |= 16;
          if (instance.Version != null)
            StringProxy.Serialize((Stream) bytes, instance.Version);
          else
            num |= 32;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ApplicationView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ApplicationView applicationView = (ApplicationView) null;
      if (num != 0)
      {
        applicationView = new ApplicationView();
        applicationView.ApplicationVersionId = Int32Proxy.Deserialize(bytes);
        applicationView.Build = EnumProxy<BuildType>.Deserialize(bytes);
        applicationView.Channel = EnumProxy<ChannelType>.Deserialize(bytes);
        if ((num & 1) != 0)
          applicationView.ExpirationDate = new DateTime?(DateTimeProxy.Deserialize(bytes));
        if ((num & 2) != 0)
          applicationView.FileName = StringProxy.Deserialize(bytes);
        applicationView.IsCurrent = BooleanProxy.Deserialize(bytes);
        applicationView.PhotonGroupId = Int32Proxy.Deserialize(bytes);
        if ((num & 4) != 0)
          applicationView.PhotonGroupName = StringProxy.Deserialize(bytes);
        applicationView.ReleaseDate = DateTimeProxy.Deserialize(bytes);
        applicationView.RemainingTime = Int32Proxy.Deserialize(bytes);
        if ((num & 8) != 0)
          applicationView.Servers = ListProxy<PhotonView>.Deserialize(bytes, new ListProxy<PhotonView>.Deserializer<PhotonView>(PhotonViewProxy.Deserialize));
        if ((num & 16) != 0)
          applicationView.SupportUrl = StringProxy.Deserialize(bytes);
        if ((num & 32) != 0)
          applicationView.Version = StringProxy.Deserialize(bytes);
      }
      return applicationView;
    }
  }
}
