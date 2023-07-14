// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ConvertEntities
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

namespace Cmune.DataCenter.Common.Entities
{
  public class ConvertEntities
  {
    public static MemberOperationResult ConvertMemberRegistration(
      MemberRegistrationResult memberRegistration)
    {
      MemberOperationResult memberOperationResult = MemberOperationResult.Ok;
      switch (memberRegistration)
      {
        case MemberRegistrationResult.Ok:
          memberOperationResult = MemberOperationResult.Ok;
          break;
        case MemberRegistrationResult.InvalidName:
          memberOperationResult = MemberOperationResult.InvalidName;
          break;
        case MemberRegistrationResult.DuplicateName:
          memberOperationResult = MemberOperationResult.DuplicateName;
          break;
        case MemberRegistrationResult.InvalidHandle:
          memberOperationResult = MemberOperationResult.InvalidHandle;
          break;
        case MemberRegistrationResult.DuplicateHandle:
          memberOperationResult = MemberOperationResult.DuplicateHandle;
          break;
        case MemberRegistrationResult.InvalidEsns:
          memberOperationResult = MemberOperationResult.InvalidEsns;
          break;
      }
      return memberOperationResult;
    }

    public static MemberRegistrationResult ConvertMemberOperation(
      MemberOperationResult memberOperation)
    {
      MemberRegistrationResult registrationResult = MemberRegistrationResult.Ok;
      switch (memberOperation)
      {
        case MemberOperationResult.Ok:
          registrationResult = MemberRegistrationResult.Ok;
          break;
        case MemberOperationResult.DuplicateEmail:
          registrationResult = MemberRegistrationResult.DuplicateEmail;
          break;
        case MemberOperationResult.DuplicateName:
          registrationResult = MemberRegistrationResult.DuplicateName;
          break;
        case MemberOperationResult.DuplicateEmailName:
          registrationResult = MemberRegistrationResult.DuplicateEmailName;
          break;
        case MemberOperationResult.InvalidName:
          registrationResult = MemberRegistrationResult.InvalidName;
          break;
        case MemberOperationResult.OffensiveName:
          registrationResult = MemberRegistrationResult.OffensiveName;
          break;
      }
      return registrationResult;
    }
  }
}
