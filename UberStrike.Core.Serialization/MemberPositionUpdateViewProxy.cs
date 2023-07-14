// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MemberPositionUpdateViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MemberPositionUpdateViewProxy
  {
    public static void Serialize(Stream stream, MemberPositionUpdateView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.CmidMakingAction);
          Int32Proxy.Serialize((Stream) bytes, instance.GroupId);
          Int32Proxy.Serialize((Stream) bytes, instance.MemberCmid);
          EnumProxy<GroupPosition>.Serialize((Stream) bytes, instance.Position);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static MemberPositionUpdateView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberPositionUpdateView positionUpdateView = (MemberPositionUpdateView) null;
      if (num != 0)
      {
        positionUpdateView = new MemberPositionUpdateView();
        positionUpdateView.CmidMakingAction = Int32Proxy.Deserialize(bytes);
        positionUpdateView.GroupId = Int32Proxy.Deserialize(bytes);
        positionUpdateView.MemberCmid = Int32Proxy.Deserialize(bytes);
        positionUpdateView.Position = EnumProxy<GroupPosition>.Deserialize(bytes);
      }
      return positionUpdateView;
    }
  }
}
