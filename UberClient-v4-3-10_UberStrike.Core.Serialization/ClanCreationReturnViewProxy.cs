// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClanCreationReturnViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ClanCreationReturnViewProxy
  {
    public static void Serialize(Stream stream, ClanCreationReturnView instance)
    {
      int num = 0;
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ClanCreationReturnView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ClanCreationReturnView creationReturnView = (ClanCreationReturnView) null;
      if (num != 0)
      {
        creationReturnView = new ClanCreationReturnView();
        if ((num & 1) != 0)
          creationReturnView.ClanView = ClanViewProxy.Deserialize(bytes);
        creationReturnView.ResultCode = Int32Proxy.Deserialize(bytes);
      }
      return creationReturnView;
    }
  }
}
