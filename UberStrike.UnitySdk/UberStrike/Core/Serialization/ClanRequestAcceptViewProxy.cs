
using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ClanRequestAcceptViewProxy
  {
    public static void Serialize(Stream stream, ClanRequestAcceptView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.ActionResult);
          Int32Proxy.Serialize((Stream) bytes, instance.ClanRequestId);
          if (instance.ClanView != null)
            ClanViewProxy.Serialize((Stream) bytes, instance.ClanView);
          else
            num |= 1;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ClanRequestAcceptView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ClanRequestAcceptView requestAcceptView = (ClanRequestAcceptView) null;
      if (num != 0)
      {
        requestAcceptView = new ClanRequestAcceptView();
        requestAcceptView.ActionResult = Int32Proxy.Deserialize(bytes);
        requestAcceptView.ClanRequestId = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          requestAcceptView.ClanView = ClanViewProxy.Deserialize(bytes);
      }
      return requestAcceptView;
    }
  }
}
