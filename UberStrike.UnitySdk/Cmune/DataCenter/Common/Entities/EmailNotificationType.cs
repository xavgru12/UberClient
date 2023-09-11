// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.EmailNotificationType
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
