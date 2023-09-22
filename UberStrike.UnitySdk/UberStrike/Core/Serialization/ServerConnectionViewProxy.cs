
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
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.ApiVersion);
          EnumProxy<ChannelType>.Serialize((Stream) bytes, instance.Channel);
          Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ServerConnectionView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ServerConnectionView serverConnectionView = (ServerConnectionView) null;
      if (num != 0)
      {
        serverConnectionView = new ServerConnectionView();
        serverConnectionView.ApiVersion = Int32Proxy.Deserialize(bytes);
        serverConnectionView.Channel = EnumProxy<ChannelType>.Deserialize(bytes);
        serverConnectionView.Cmid = Int32Proxy.Deserialize(bytes);
      }
      return serverConnectionView;
    }
  }
}
