﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.ESNetSocketState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public enum ESNetSocketState
  {
    k_ESNetSocketStateInvalid = 0,
    k_ESNetSocketStateConnected = 1,
    k_ESNetSocketStateInitiated = 10, // 0x0000000A
    k_ESNetSocketStateLocalCandidatesFound = 11, // 0x0000000B
    k_ESNetSocketStateReceivedRemoteCandidates = 12, // 0x0000000C
    k_ESNetSocketStateChallengeHandshake = 15, // 0x0000000F
    k_ESNetSocketStateDisconnecting = 21, // 0x00000015
    k_ESNetSocketStateLocalDisconnect = 22, // 0x00000016
    k_ESNetSocketStateTimeoutDuringConnect = 23, // 0x00000017
    k_ESNetSocketStateRemoteEndDisconnected = 24, // 0x00000018
    k_ESNetSocketStateConnectionBroken = 25, // 0x00000019
  }
}
