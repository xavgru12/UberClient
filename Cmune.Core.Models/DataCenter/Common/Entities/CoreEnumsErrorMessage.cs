// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.CoreEnumsErrorMessage
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

namespace Cmune.DataCenter.Common.Entities
{
  public static class CoreEnumsErrorMessage
  {
    public static string GetMemberRegistrationErrorMessage(
      this MemberAuthenticationResult memberRegistrationResult)
    {
      string registrationErrorMessage = string.Empty;
      switch (memberRegistrationResult)
      {
        case MemberAuthenticationResult.Ok:
          return registrationErrorMessage;
        case MemberAuthenticationResult.InvalidData:
          registrationErrorMessage = "Your e-mail address or password is incorrect. Double check you typed them in correctly and try again!";
          goto case MemberAuthenticationResult.Ok;
        case MemberAuthenticationResult.InvalidEmail:
          registrationErrorMessage = "Your e-mail address or password is incorrect. Double check you typed them in correctly and try again!";
          goto case MemberAuthenticationResult.Ok;
        case MemberAuthenticationResult.InvalidPassword:
          registrationErrorMessage = "Your e-mail address or password is incorrect. Double check you typed them in correctly and try again!";
          goto case MemberAuthenticationResult.Ok;
        case MemberAuthenticationResult.IsBanned:
          registrationErrorMessage = "";
          goto case MemberAuthenticationResult.Ok;
        default:
          registrationErrorMessage = "Sorry, our systems are having trouble logging you in. Please try again in a few minutes.";
          goto case MemberAuthenticationResult.Ok;
      }
    }

    public static string GetMemberOperationErrorMessage(
      this MemberOperationResult memberOperationResult)
    {
      string operationErrorMessage = string.Empty;
      switch (memberOperationResult)
      {
        case MemberOperationResult.Ok:
          return operationErrorMessage;
        case MemberOperationResult.DuplicateEmail:
          operationErrorMessage = "Your email address is already in use by another account.";
          goto case MemberOperationResult.Ok;
        case MemberOperationResult.MemberNotFound:
          operationErrorMessage = "That e-mail address doesn't exist in our systems!";
          goto case MemberOperationResult.Ok;
        case MemberOperationResult.InvalidEmail:
          operationErrorMessage = "That doesn't look like a valid e-mail address to me!";
          goto case MemberOperationResult.Ok;
        case MemberOperationResult.InvalidPassword:
          operationErrorMessage = "That doesn't look like a valid password to me!";
          goto case MemberOperationResult.Ok;
        default:
          operationErrorMessage = "Sorry, our system encountered a problem. Please try again and if the problem continues visit http://support.uberstrike.com/.";
          goto case MemberOperationResult.Ok;
      }
    }

    public static string GetBanModeErrorMessage(this BanMode banMode, string info)
    {
      string modeErrorMessage = string.Empty;
      switch (banMode)
      {
        case BanMode.Temporary:
          modeErrorMessage = "Your account has been banned until " + info + ". If you want to dispute the ban please visit http://support.uberstrike.com/.";
          break;
        case BanMode.Permanent:
          modeErrorMessage = "Your account has been permanently banned. Common ban offences include cheating and in-game abuse. If you want to dispute the ban please visit http://support.uberstrike.com/.";
          break;
      }
      return modeErrorMessage;
    }
  }
}
