// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.AuthenticateApplicationViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.Core.Models.Views;
using System.Collections.Generic;
using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class AuthenticateApplicationViewProxy
  {
    public static void Serialize(Stream stream, AuthenticateApplicationView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.CommServer != null)
            PhotonViewProxy.Serialize((Stream) bytes, instance.CommServer);
          else
            num |= 1;
          if (instance.EncryptionInitVector != null)
            StringProxy.Serialize((Stream) bytes, instance.EncryptionInitVector);
          else
            num |= 2;
          if (instance.EncryptionPassPhrase != null)
            StringProxy.Serialize((Stream) bytes, instance.EncryptionPassPhrase);
          else
            num |= 4;
          if (instance.GameServers != null)
            ListProxy<PhotonView>.Serialize((Stream) bytes, (ICollection<PhotonView>) instance.GameServers, new ListProxy<PhotonView>.Serializer<PhotonView>(PhotonViewProxy.Serialize));
          else
            num |= 8;
          BooleanProxy.Serialize((Stream) bytes, instance.IsEnabled);
          BooleanProxy.Serialize((Stream) bytes, instance.WarnPlayer);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static AuthenticateApplicationView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      AuthenticateApplicationView authenticateApplicationView = (AuthenticateApplicationView) null;
      if (num != 0)
      {
        authenticateApplicationView = new AuthenticateApplicationView();
        if ((num & 1) != 0)
          authenticateApplicationView.CommServer = PhotonViewProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          authenticateApplicationView.EncryptionInitVector = StringProxy.Deserialize(bytes);
        if ((num & 4) != 0)
          authenticateApplicationView.EncryptionPassPhrase = StringProxy.Deserialize(bytes);
        if ((num & 8) != 0)
          authenticateApplicationView.GameServers = ListProxy<PhotonView>.Deserialize(bytes, new ListProxy<PhotonView>.Deserializer<PhotonView>(PhotonViewProxy.Deserialize));
        authenticateApplicationView.IsEnabled = BooleanProxy.Deserialize(bytes);
        authenticateApplicationView.WarnPlayer = BooleanProxy.Deserialize(bytes);
      }
      return authenticateApplicationView;
    }
  }
}
