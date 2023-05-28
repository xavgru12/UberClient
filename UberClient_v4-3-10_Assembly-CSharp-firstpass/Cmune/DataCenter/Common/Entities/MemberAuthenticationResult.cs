// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.MemberAuthenticationResult
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Cmune.DataCenter.Common.Entities
{
  public enum MemberAuthenticationResult
  {
    Ok,
    InvalidData,
    InvalidName,
    InvalidEmail,
    InvalidPassword,
    IsBanned,
    InvalidHandle,
    InvalidEsns,
    InvalidCookie,
    IsIpBanned,
    UnknownError,
    NewUpdate,
  }
}
