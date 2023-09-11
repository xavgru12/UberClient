
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
  }
}
