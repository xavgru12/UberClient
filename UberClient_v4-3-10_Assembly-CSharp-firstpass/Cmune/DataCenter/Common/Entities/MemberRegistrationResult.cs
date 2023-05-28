// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MemberRegistrationResult
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Cmune.DataCenter.Common.Entities
{
  public enum MemberRegistrationResult
  {
    Ok,
    InvalidEmail,
    InvalidName,
    InvalidPassword,
    DuplicateEmail,
    DuplicateName,
    DuplicateEmailName,
    InvalidData,
    InvalidHandle,
    DuplicateHandle,
    InvalidEsns,
    MemberNotFound,
    OffensiveName,
    IsIpBanned,
    EmailAlreadyLinkedToActualEsns,
    Error_MemberNotCreated,
  }
}
