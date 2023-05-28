// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MemberPositionUpdateViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MemberPositionUpdateViewProxy
  {
    public static void Serialize(Stream stream, MemberPositionUpdateView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.AuthToken != null)
          StringProxy.Serialize((Stream) bytes, instance.AuthToken);
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.GroupId);
        Int32Proxy.Serialize((Stream) bytes, instance.MemberCmid);
        EnumProxy<GroupPosition>.Serialize((Stream) bytes, instance.Position);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static MemberPositionUpdateView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberPositionUpdateView positionUpdateView = new MemberPositionUpdateView();
      if ((num & 1) != 0)
        positionUpdateView.AuthToken = StringProxy.Deserialize(bytes);
      positionUpdateView.GroupId = Int32Proxy.Deserialize(bytes);
      positionUpdateView.MemberCmid = Int32Proxy.Deserialize(bytes);
      positionUpdateView.Position = EnumProxy<GroupPosition>.Deserialize(bytes);
      return positionUpdateView;
    }
  }
}
