// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.GameApplicationViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Models.Views;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class GameApplicationViewProxy
  {
    public static void Serialize(Stream stream, GameApplicationView instance)
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

    public static GameApplicationView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      GameApplicationView gameApplicationView = (GameApplicationView) null;
      if (num != 0)
      {
        gameApplicationView = new GameApplicationView();
        if ((num & 1) != 0)
          gameApplicationView.CommServer = PhotonViewProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          gameApplicationView.EncryptionInitVector = StringProxy.Deserialize(bytes);
        if ((num & 4) != 0)
          gameApplicationView.EncryptionPassPhrase = StringProxy.Deserialize(bytes);
        if ((num & 8) != 0)
          gameApplicationView.GameServers = ListProxy<PhotonView>.Deserialize(bytes, new ListProxy<PhotonView>.Deserializer<PhotonView>(PhotonViewProxy.Deserialize));
        if ((num & 16) != 0)
          gameApplicationView.SupportUrl = StringProxy.Deserialize(bytes);
        if ((num & 32) != 0)
          gameApplicationView.Version = StringProxy.Deserialize(bytes);
      }
      return gameApplicationView;
    }
  }
}
