// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.EmailNotificationType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Cmune.DataCenter.Common.Entities
{
  public enum EmailNotificationType
  {
    DeleteMember = 1,
    BanMemberPermanent = 2,
    MergeMembers = 3,
    ChangeMemberName = 8,
    ChangeMemberPassword = 9,
    ChangeMemberEmail = 11, // 0x0000000B
    BanMemberTemporary = 12, // 0x0000000C
    UnbanMember = 13, // 0x0000000D
    BanMemberChatPermanent = 14, // 0x0000000E
    BanMemberChatTemporary = 15, // 0x0000000F
    UnbanMemberChat = 16, // 0x00000010
    ChangeClanTag = 17, // 0x00000011
    ChangeClanName = 18, // 0x00000012
    ChangeClanMotto = 19, // 0x00000013
  }
}
