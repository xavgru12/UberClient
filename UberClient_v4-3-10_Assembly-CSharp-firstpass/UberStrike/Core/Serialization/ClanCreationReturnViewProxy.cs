// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClanCreationReturnViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ClanCreationReturnViewProxy
  {
    public static void Serialize(Stream stream, ClanCreationReturnView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.ClanView != null)
          ClanViewProxy.Serialize((Stream) bytes, instance.ClanView);
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.ResultCode);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static ClanCreationReturnView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ClanCreationReturnView creationReturnView = new ClanCreationReturnView();
      if ((num & 1) != 0)
        creationReturnView.ClanView = ClanViewProxy.Deserialize(bytes);
      creationReturnView.ResultCode = Int32Proxy.Deserialize(bytes);
      return creationReturnView;
    }
  }
}
