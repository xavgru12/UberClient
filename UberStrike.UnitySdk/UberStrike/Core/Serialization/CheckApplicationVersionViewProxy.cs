﻿
using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class CheckApplicationVersionViewProxy
  {
    public static void Serialize(Stream stream, CheckApplicationVersionView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          if (instance.ClientVersion != null)
            ApplicationViewProxy.Serialize((Stream) memoryStream, instance.ClientVersion);
          else
            num |= 1;
          if (instance.CurrentVersion != null)
            ApplicationViewProxy.Serialize((Stream) memoryStream, instance.CurrentVersion);
          else
            num |= 2;
          Int32Proxy.Serialize(stream, ~num);
          memoryStream.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static CheckApplicationVersionView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      CheckApplicationVersionView applicationVersionView = (CheckApplicationVersionView) null;
      if (num != 0)
      {
        applicationVersionView = new CheckApplicationVersionView();
        if ((num & 1) != 0)
          applicationVersionView.ClientVersion = ApplicationViewProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          applicationVersionView.CurrentVersion = ApplicationViewProxy.Deserialize(bytes);
      }
      return applicationVersionView;
    }
  }
}
