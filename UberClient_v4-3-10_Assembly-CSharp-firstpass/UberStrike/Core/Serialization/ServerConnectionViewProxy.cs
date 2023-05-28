// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ServerConnectionViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class ServerConnectionViewProxy
  {
    public static void Serialize(Stream stream, ServerConnectionView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<MemberAccessLevel>.Serialize((Stream) bytes, instance.AccessLevel);
        if (instance.ApiVersion != null)
          StringProxy.Serialize((Stream) bytes, instance.ApiVersion);
        else
          num |= 1;
        EnumProxy<ChannelType>.Serialize((Stream) bytes, instance.Channel);
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static ServerConnectionView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ServerConnectionView serverConnectionView = new ServerConnectionView();
      serverConnectionView.AccessLevel = EnumProxy<MemberAccessLevel>.Deserialize(bytes);
      if ((num & 1) != 0)
        serverConnectionView.ApiVersion = StringProxy.Deserialize(bytes);
      serverConnectionView.Channel = EnumProxy<ChannelType>.Deserialize(bytes);
      serverConnectionView.Cmid = Int32Proxy.Deserialize(bytes);
      return serverConnectionView;
    }
  }
}
