// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MemberViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public static MemberView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberView memberView = new MemberView();
      if ((num & 1) != 0)
        memberView.MemberItems = ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
      if ((num & 2) != 0)
        memberView.MemberWallet = MemberWalletViewProxy.Deserialize(bytes);
      if ((num & 4) != 0)
        memberView.PublicProfile = PublicProfileViewProxy.Deserialize(bytes);
      return memberView;
    }
  }
}
