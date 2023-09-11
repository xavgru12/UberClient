
using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class BugViewProxy
  {
    public static void Serialize(Stream stream, BugView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.Content != null)
            StringProxy.Serialize((Stream) bytes, instance.Content);
          else
            num |= 1;
          if (instance.Subject != null)
            StringProxy.Serialize((Stream) bytes, instance.Subject);
          else
            num |= 2;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static BugView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      BugView bugView = (BugView) null;
      if (num != 0)
      {
        bugView = new BugView();
        if ((num & 1) != 0)
          bugView.Content = StringProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          bugView.Subject = StringProxy.Deserialize(bytes);
      }
      return bugView;
    }
  }
}
