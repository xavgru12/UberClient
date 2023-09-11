// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.AuthenticateApplicationViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
