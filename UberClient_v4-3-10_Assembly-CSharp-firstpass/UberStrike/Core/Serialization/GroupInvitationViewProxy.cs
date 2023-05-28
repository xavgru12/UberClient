// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.GroupInvitationViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class GroupInvitationViewProxy
  {
    public static void Serialize(Stream stream, GroupInvitationView instance)
    {
      int num = 0;
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

    public static GroupInvitationView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      GroupInvitationView groupInvitationView = new GroupInvitationView();
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
      return groupInvitationView;
    }
  }
}
