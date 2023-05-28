// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MemberViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MemberViewProxy
  {
    public static void Serialize(Stream stream, MemberView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.MemberItems != null)
            ListProxy<int>.Serialize((Stream) bytes, (ICollection<int>) instance.MemberItems, new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
          else
            num |= 1;
          if (instance.MemberWallet != null)
            MemberWalletViewProxy.Serialize((Stream) bytes, instance.MemberWallet);
          else
            num |= 2;
          if (instance.PublicProfile != null)
            PublicProfileViewProxy.Serialize((Stream) bytes, instance.PublicProfile);
          else
            num |= 4;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static MemberView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberView memberView = (MemberView) null;
      if (num != 0)
      {
        memberView = new MemberView();
        if ((num & 1) != 0)
          memberView.MemberItems = ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
        if ((num & 2) != 0)
          memberView.MemberWallet = MemberWalletViewProxy.Deserialize(bytes);
        if ((num & 4) != 0)
          memberView.PublicProfile = PublicProfileViewProxy.Deserialize(bytes);
      }
      return memberView;
    }
  }
}
