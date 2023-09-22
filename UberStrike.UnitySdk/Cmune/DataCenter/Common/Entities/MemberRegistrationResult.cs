﻿
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
  }
}
