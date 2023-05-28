// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.ILobbyRoomOperationsType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace UberStrike.Realtime.Client
{
  public enum ILobbyRoomOperationsType
  {
    FullPlayerListUpdate = 1,
    UpdatePlayerRoom = 2,
    ResetPlayerRoom = 3,
    UpdateFriendsList = 4,
    UpdateClanData = 5,
    UpdateInboxMessages = 6,
    UpdateInboxRequests = 7,
    UpdateClanMembers = 8,
    GetPlayersWithMatchingName = 9,
    ChatMessageToAll = 10, // 0x0000000A
    ChatMessageToPlayer = 11, // 0x0000000B
    ChatMessageToClan = 12, // 0x0000000C
    ModerationMutePlayer = 13, // 0x0000000D
    ModerationPermanentBan = 14, // 0x0000000E
    ModerationBanPlayer = 15, // 0x0000000F
    ModerationKickGame = 16, // 0x00000010
    ModerationUnbanPlayer = 17, // 0x00000011
    ModerationCustomMessage = 18, // 0x00000012
    SpeedhackDetection = 19, // 0x00000013
    SpeedhackDetectionNew = 20, // 0x00000014
    PlayersReported = 21, // 0x00000015
    UpdateNaughtyList = 22, // 0x00000016
    ClearModeratorFlags = 23, // 0x00000017
    SetContactList = 24, // 0x00000018
    UpdateAllActors = 25, // 0x00000019
    UpdateContacts = 26, // 0x0000001A
  }
}
