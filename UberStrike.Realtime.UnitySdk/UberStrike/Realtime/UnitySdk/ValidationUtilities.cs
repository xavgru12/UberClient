// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ValidationUtilities
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System.Text.RegularExpressions;

namespace UberStrike.Realtime.UnitySdk
{
  public static class ValidationUtilities
  {
    public static string StandardizeMemberName(string memberName) => TextUtilities.CompleteTrim(memberName);

    public static bool IsValidEmailAddress(string email)
    {
      if (TextUtilities.IsNullOrEmpty(email) || email.Length > 100)
        return false;
      int num1 = email.IndexOf('@');
      int num2 = email.LastIndexOf('@');
      return num1 > 0 && num2 == num1 && num1 < email.Length - 1 && Regex.IsMatch(email, "^([a-zA-Z0-9_'+*$%\\^&!\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9:]{2,4})+$");
    }

    public static string StandardizeEmail(string email)
    {
      if (email != null)
        email = TextUtilities.CompleteTrim(email).ToLower();
      return email;
    }

    public static bool IsValidPassword(string password)
    {
      bool flag = false;
      if (!TextUtilities.IsNullOrEmpty(password) && password.Length > 3 && password.Length < 64)
        flag = true;
      return flag;
    }

    public static bool IsValidCategoryName(string categoryName)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(categoryName))
        flag = Regex.IsMatch(categoryName, "^[a-zA-Z0-9]{" + (object) 3 + "," + (object) 20 + "}$");
      return flag;
    }

    public static bool IsValidMemberName(string memberName) => ValidationUtilities.IsValidMemberName(memberName, "en-US");

    public static bool IsValidMemberName(string memberName, string locale)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(memberName))
      {
        memberName = memberName.Trim();
        if (memberName.Equals(TextUtilities.CompleteTrim(memberName)))
        {
          string empty = string.Empty;
          string str;
          switch (locale)
          {
            case "ko-KR":
              str = "\\p{IsHangulSyllables}";
              break;
            default:
              str = string.Empty;
              break;
          }
          flag = Regex.IsMatch(memberName, "^[a-zA-Z0-9 .!_\\-<>{}~@#$%^&*()=+|:?" + str + "]{" + (object) 3 + "," + (object) 18 + "}$");
        }
        int num1 = memberName.ToLower().IndexOf("admin");
        int num2;
        if (num1.Equals(-1))
        {
          num1 = memberName.ToLower().IndexOf("cmune");
          num2 = num1.Equals(-1) ? 1 : 0;
        }
        else
          num2 = 0;
        if (num2 == 0)
          flag = false;
      }
      return flag;
    }

    public static bool IsValidEsndId(EsnsType esnsType, string esnsMemberId)
    {
      bool flag = false;
      switch (esnsType)
      {
        case EsnsType.Facebook:
          long result1;
          if (long.TryParse(esnsMemberId, out result1) && result1 > 0L)
          {
            flag = true;
            break;
          }
          break;
        case EsnsType.MySpace:
          int result2;
          if (int.TryParse(esnsMemberId, out result2) && result2 > 0)
          {
            flag = true;
            break;
          }
          break;
        case EsnsType.Cyworld:
          int result3;
          if (int.TryParse(esnsMemberId, out result3) && result3 > 0)
          {
            flag = true;
            break;
          }
          break;
        case EsnsType.Kongregate:
          long result4;
          if (long.TryParse(esnsMemberId, out result4) && result4 > 0L)
          {
            flag = true;
            break;
          }
          break;
      }
      return flag;
    }

    public static string StandardizeContactGroupName(string contactGroupName) => TextUtilities.CompleteTrim(contactGroupName);

    public static bool IsValidContactGroupName(string contactGroupName) => ValidationUtilities.IsValidContactGroupName(contactGroupName, true);

    public static bool IsValidContactGroupName(string contactGroupName, bool checkDefaultName)
    {
      bool flag1 = false;
      if (!string.IsNullOrEmpty(contactGroupName))
      {
        contactGroupName = contactGroupName.Trim();
        if (contactGroupName.Equals(TextUtilities.CompleteTrim(contactGroupName)))
        {
          flag1 = Regex.IsMatch(contactGroupName, "^[a-zA-Z0-9]{" + (object) 3 + "," + (object) 15 + "}$");
          bool flag2 = false;
          if (checkDefaultName)
            flag2 = contactGroupName.Equals("Default");
          bool flag3 = false;
          int num1 = contactGroupName.ToLower().IndexOf("admin");
          int num2;
          if (num1.Equals(-1))
          {
            num1 = contactGroupName.ToLower().IndexOf("cmune");
            num2 = num1.Equals(-1) ? 1 : 0;
          }
          else
            num2 = 0;
          if (num2 == 0)
            flag3 = true;
          if (flag1 && !flag2 && !flag3)
            flag1 = true;
        }
      }
      return flag1;
    }

    public static string StandardizeClanName(string clanName) => TextUtilities.CompleteTrim(clanName);

    public static string StandardizeClanTag(string tagName) => TextUtilities.CompleteTrim(tagName);

    public static bool IsValidClanName(string clanName, string locale)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(clanName))
      {
        clanName = clanName.Trim();
        if (clanName.Equals(TextUtilities.CompleteTrim(clanName)))
        {
          string empty = string.Empty;
          string str;
          switch (locale)
          {
            case "ko-KR":
              str = "\\p{IsHangulSyllables}";
              break;
            default:
              str = string.Empty;
              break;
          }
          flag = Regex.IsMatch(clanName, "^[a-zA-Z0-9 \\-" + str + "]{" + (object) 3 + "," + (object) 25 + "}$");
        }
        int num1 = clanName.ToLower().IndexOf("admin");
        int num2;
        if (num1.Equals(-1))
        {
          num1 = clanName.ToLower().IndexOf("cmune");
          num2 = num1.Equals(-1) ? 1 : 0;
        }
        else
          num2 = 0;
        if (num2 == 0)
          flag = false;
      }
      return flag;
    }

    public static bool IsValidClanTag(string clanTag, string locale)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(clanTag))
      {
        clanTag = clanTag.Trim();
        if (clanTag.Equals(TextUtilities.CompleteTrim(clanTag)))
        {
          string empty = string.Empty;
          string str;
          switch (locale)
          {
            case "ko-KR":
              str = "\\p{IsHangulSyllables}";
              break;
            default:
              str = string.Empty;
              break;
          }
          flag = Regex.IsMatch(clanTag, "^[a-zA-Z0-9 .!_\\-<>[\\]{}~@#$%^&*()=+|:?" + str + "]{" + (object) 2 + "," + (object) 5 + "}$");
        }
        int num1 = clanTag.ToLower().IndexOf("admin");
        int num2;
        if (num1.Equals(-1))
        {
          num1 = clanTag.ToLower().IndexOf("cmune");
          num2 = num1.Equals(-1) ? 1 : 0;
        }
        else
          num2 = 0;
        if (num2 == 0)
          flag = false;
      }
      return flag;
    }

    public static bool IsValidClanMotto(string clanMotto)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(clanMotto) && clanMotto.Length >= 1 && clanMotto.Length <= 25)
        flag = true;
      return flag;
    }

    public static bool IsValidClanDesciption(string clanDescription)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(clanDescription) && clanDescription.Length >= 1 && clanDescription.Length <= 200)
        flag = true;
      return flag;
    }

    public static bool IsValidPhotonsGroupName(string photonsGroupsName)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(photonsGroupsName))
      {
        photonsGroupsName = photonsGroupsName.Trim();
        if (photonsGroupsName.Equals(TextUtilities.CompleteTrim(photonsGroupsName)))
          flag = photonsGroupsName.Length >= 3 && photonsGroupsName.Length <= 50;
      }
      return flag;
    }

    public static string StandardizePhotonsGroupName(string photonsGroupsName) => TextUtilities.CompleteTrim(photonsGroupsName);

    public static bool IsValidPhotonServerName(string name)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(name))
      {
        name = name.Trim();
        if (name.Equals(TextUtilities.CompleteTrim(name)))
          flag = name.Length >= 3 && name.Length <= (int) byte.MaxValue;
      }
      return flag;
    }

    public static string StandardizePhotonServerName(string name) => TextUtilities.CompleteTrim(name);

    public static bool IsValidManagedServerName(string serverName)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(serverName))
      {
        serverName = serverName.Trim();
        if (serverName.Equals(TextUtilities.CompleteTrim(serverName)))
          flag = serverName.Length >= 3 && serverName.Length <= 50;
      }
      return flag;
    }

    public static string StandardizeManagedServerName(string serverName) => TextUtilities.CompleteTrim(serverName);

    public static bool IsValidManagedServerTestName(string testName)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(testName))
      {
        testName = testName.Trim();
        if (testName.Equals(TextUtilities.CompleteTrim(testName)))
          flag = testName.Length >= 3 && testName.Length <= 50;
      }
      return flag;
    }

    public static string StandardizeManagedServerTestName(string serverName) => TextUtilities.CompleteTrim(serverName);

    public static bool IsValidRotationMemberName(string memberName)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(memberName))
      {
        memberName = memberName.Trim();
        if (memberName.Equals(TextUtilities.CompleteTrim(memberName)))
          flag = memberName.Length >= 3 && memberName.Length <= 50;
      }
      return flag;
    }

    public static string StandardizeRotationMemberName(string memberName) => TextUtilities.CompleteTrim(memberName);

    public static bool IsValidPortNumber(int portNumber)
    {
      bool flag = false;
      if (portNumber >= 1 && portNumber <= (int) ushort.MaxValue)
        flag = true;
      return flag;
    }

    public static bool IsValidPortNumber(string portNumber)
    {
      bool flag = false;
      int result = 0;
      if (int.TryParse(portNumber, out result))
        flag = ValidationUtilities.IsValidPortNumber(result);
      return flag;
    }

    public static bool IsValidIPAddress(string ipAddress)
    {
      bool flag1 = false;
      if (!string.IsNullOrEmpty(ipAddress))
      {
        string[] strArray = ipAddress.Split('.');
        if (strArray.Length == 4)
        {
          bool flag2 = byte.TryParse(strArray[0], out byte _);
          bool flag3 = byte.TryParse(strArray[1], out byte _);
          bool flag4 = byte.TryParse(strArray[2], out byte _);
          bool flag5 = byte.TryParse(strArray[3], out byte _);
          if (flag2 && flag3 && flag4 && flag5)
            flag1 = true;
        }
      }
      return flag1;
    }

    public static bool IsValidSocket(string socket)
    {
      bool flag = false;
      string[] strArray = socket.Split(':');
      if (strArray.Length == 2)
        flag = ValidationUtilities.IsValidIPAddress(strArray[0]) && ValidationUtilities.IsValidPortNumber(strArray[1]);
      return flag;
    }
  }
}
