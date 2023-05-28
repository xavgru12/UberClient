// Decompiled with JetBrains decompiler
// Type: Steamworks.EFriendFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EFriendFlags
  {
    k_EFriendFlagNone = 0,
    k_EFriendFlagBlocked = 1,
    k_EFriendFlagFriendshipRequested = 2,
    k_EFriendFlagImmediate = 4,
    k_EFriendFlagClanMember = 8,
    k_EFriendFlagOnGameServer = 16, // 0x00000010
    k_EFriendFlagRequestingFriendship = 128, // 0x00000080
    k_EFriendFlagRequestingInfo = 256, // 0x00000100
    k_EFriendFlagIgnored = 512, // 0x00000200
    k_EFriendFlagIgnoredFriend = 1024, // 0x00000400
    k_EFriendFlagSuggested = 2048, // 0x00000800
    k_EFriendFlagAll = 65535, // 0x0000FFFF
  }
}
