// Decompiled with JetBrains decompiler
// Type: Steamworks.EUserRestriction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public enum EUserRestriction
  {
    k_nUserRestrictionNone = 0,
    k_nUserRestrictionUnknown = 1,
    k_nUserRestrictionAnyChat = 2,
    k_nUserRestrictionVoiceChat = 4,
    k_nUserRestrictionGroupChat = 8,
    k_nUserRestrictionRating = 16, // 0x00000010
    k_nUserRestrictionGameInvites = 32, // 0x00000020
    k_nUserRestrictionTrading = 64, // 0x00000040
  }
}
