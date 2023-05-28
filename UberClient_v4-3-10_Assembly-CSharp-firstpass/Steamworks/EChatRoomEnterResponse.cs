// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatRoomEnterResponse
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public enum EChatRoomEnterResponse
  {
    k_EChatRoomEnterResponseSuccess = 1,
    k_EChatRoomEnterResponseDoesntExist = 2,
    k_EChatRoomEnterResponseNotAllowed = 3,
    k_EChatRoomEnterResponseFull = 4,
    k_EChatRoomEnterResponseError = 5,
    k_EChatRoomEnterResponseBanned = 6,
    k_EChatRoomEnterResponseLimited = 7,
    k_EChatRoomEnterResponseClanDisabled = 8,
    k_EChatRoomEnterResponseCommunityBan = 9,
    k_EChatRoomEnterResponseMemberBlockedYou = 10, // 0x0000000A
    k_EChatRoomEnterResponseYouBlockedMember = 11, // 0x0000000B
  }
}
