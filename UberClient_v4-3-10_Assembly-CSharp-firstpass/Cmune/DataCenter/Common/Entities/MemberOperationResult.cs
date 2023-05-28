// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MemberOperationResult
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public enum MemberOperationResult
  {
    Ok = 0,
    DuplicateEmail = 2,
    DuplicateName = 3,
    DuplicateHandle = 4,
    DuplicateEmailName = 5,
    MemberNotFound = 6,
    InvalidData = 9,
    InvalidHandle = 10, // 0x0000000A
    InvalidEsns = 11, // 0x0000000B
    InvalidCmid = 12, // 0x0000000C
    InvalidName = 13, // 0x0000000D
    InvalidEmail = 14, // 0x0000000E
    InvalidPassword = 15, // 0x0000000F
    OffensiveName = 16, // 0x00000010
    NameChangeNotInInventory = 17, // 0x00000011
    AlreadyHasAnESNSAccountOfThisTypeAttached = 18, // 0x00000012
  }
}
