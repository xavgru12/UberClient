// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.ILobbyRoomEventsType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace UberStrike.Realtime.Client
{
  public enum ILobbyRoomEventsType
  {
    PlayerHide = 5,
    PlayerLeft = 6,
    PlayerUpdate = 7,
    UpdateContacts = 8,
    FullPlayerListUpdate = 9,
    PlayerJoined = 10, // 0x0000000A
    ClanChatMessage = 11, // 0x0000000B
    InGameChatMessage = 12, // 0x0000000C
    LobbyChatMessage = 13, // 0x0000000D
    PrivateChatMessage = 14, // 0x0000000E
    UpdateInboxRequests = 15, // 0x0000000F
    UpdateFriendsList = 16, // 0x00000010
    UpdateInboxMessages = 17, // 0x00000011
    UpdateClanMembers = 18, // 0x00000012
    UpdateClanData = 19, // 0x00000013
    UpdateActorsForModeration = 20, // 0x00000014
    ModerationCustomMessage = 21, // 0x00000015
    ModerationMutePlayer = 22, // 0x00000016
    ModerationKickGame = 23, // 0x00000017
  }
}
