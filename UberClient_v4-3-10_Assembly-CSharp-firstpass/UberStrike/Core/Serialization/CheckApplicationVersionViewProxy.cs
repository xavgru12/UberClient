// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.CheckApplicationVersionViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class CheckApplicationVersionViewProxy
  {
    public static void Serialize(Stream stream, CheckApplicationVersionView instance)
    {
      int num = 0;
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

    public static CheckApplicationVersionView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      CheckApplicationVersionView applicationVersionView = new CheckApplicationVersionView();
      if ((num & 1) != 0)
        applicationVersionView.ClientVersion = ApplicationViewProxy.Deserialize(bytes);
      if ((num & 2) != 0)
        applicationVersionView.CurrentVersion = ApplicationViewProxy.Deserialize(bytes);
      return applicationVersionView;
    }
  }
}
