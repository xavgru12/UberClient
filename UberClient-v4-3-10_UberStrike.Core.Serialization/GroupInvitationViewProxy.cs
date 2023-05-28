// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.GroupInvitationViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class GroupInvitationViewProxy
  {
    public static void Serialize(Stream stream, GroupInvitationView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.GroupId);
          Int32Proxy.Serialize((Stream) bytes, instance.GroupInvitationId);
          if (instance.GroupName != null)
            StringProxy.Serialize((Stream) bytes, instance.GroupName);
          else
            num |= 1;
          if (instance.GroupTag != null)
            StringProxy.Serialize((Stream) bytes, instance.GroupTag);
          else
            num |= 2;
          Int32Proxy.Serialize((Stream) bytes, instance.InviteeCmid);
          if (instance.InviteeName != null)
            StringProxy.Serialize((Stream) bytes, instance.InviteeName);
          else
            num |= 4;
          Int32Proxy.Serialize((Stream) bytes, instance.InviterCmid);
          if (instance.InviterName != null)
            StringProxy.Serialize((Stream) bytes, instance.InviterName);
          else
            num |= 8;
          if (instance.Message != null)
            StringProxy.Serialize((Stream) bytes, instance.Message);
          else
            num |= 16;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static GroupInvitationView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      GroupInvitationView groupInvitationView = (GroupInvitationView) null;
      if (num != 0)
      {
        groupInvitationView = new GroupInvitationView();
        groupInvitationView.GroupId = Int32Proxy.Deserialize(bytes);
        groupInvitationView.GroupInvitationId = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          groupInvitationView.GroupName = StringProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          groupInvitationView.GroupTag = StringProxy.Deserialize(bytes);
        groupInvitationView.InviteeCmid = Int32Proxy.Deserialize(bytes);
        if ((num & 4) != 0)
          groupInvitationView.InviteeName = StringProxy.Deserialize(bytes);
        groupInvitationView.InviterCmid = Int32Proxy.Deserialize(bytes);
        if ((num & 8) != 0)
          groupInvitationView.InviterName = StringProxy.Deserialize(bytes);
        if ((num & 16) != 0)
          groupInvitationView.Message = StringProxy.Deserialize(bytes);
      }
      return groupInvitationView;
    }
  }
}
