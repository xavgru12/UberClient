
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Cmune.DataCenter.Common.Entities
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct CommonConfig
  {
    public const string ContactGroupDefaultName = "Default";
    public const int MemberMergePointsPenalizeDefault = 50;
    public const string SmtpIP = "67.228.44.4";
    public const string SmtpUser = "cmuneMailer";
    public const string SmtpPassword = "cmune$1";
    public const string CmuneSupportEmail = "support@cmune.com";
    public const string CmuneSupportEmailName = "The Cmune Team";
    public const string CmuneNoReplyEmail = "noreply@cmune.com";
    public const string CmuneNoReplyEmailName = "The Cmune Team";
    public const string CmuneSupportCenterUrl = "http://support.uberstrike.com/";
    public const string CmuneDevteamEmail = "devteam@cmune.com";
    public const string CmuneDevteamEmailName = "The Cmune Devteam";
    public const int IdentityValidationLifetimeInDays = 30;
    public const int AdminCmid = 767;
    public const int CommunityManagerCmid = 598916;
    public const int PointsAttributedOnRegistration = 1500;
    public const int PointsAttributedOnEmailValidation = 1000;
    public const int GroupMottoMinLength = 1;
    public const int GroupMottoMaxLength = 25;
    public const int GroupDescriptionMinLength = 1;
    public const int GroupDescriptionMaxLength = 200;
    public const int ItemCategoryNameMinLength = 3;
    public const int ItemCategoryNameMaxLength = 20;
    public const int MemberNameMinLength = 3;
    public const int MemberNameMaxLength = 18;
    public const int MemberEmailMaxLength = 100;
    public const int MemberPasswordMinLength = 3;
    public const int MemberPasswordMaxLength = 64;
    public const int ContacGroupNameMinLength = 3;
    public const int ContactGroupNameMaxLength = 15;
    public const int GroupNameMinLength = 3;
    public const int GroupNameMaxLength = 25;
    public const int GroupTagMinLength = 2;
    public const int GroupTagMaxLenght = 5;
    public const int PhotonsGroupNameMinLenght = 3;
    public const int PhotonsGroupNameMaxLenght = 50;
    public const int PhotonsServerNameMinLenght = 3;
    public const int PhotonsServerNameMaxLenght = 255;
    public const int PhotonsGroupDescriptionMaxLength = 100;
    public const int MemberReportReasonMaxLength = 500;
    public const int MemberReportContextMaxLength = 4000;
    public const int ManagedServerNameMinLenght = 3;
    public const int ManagedServerNameMaxLenght = 50;
    public const int ManagedServerTestNameMinLenght = 3;
    public const int ManagedServerTestNameMaxLenght = 50;
    public const int RotationMemberNameMinLenght = 3;
    public const int RotationMemberNameMaxLenght = 50;
    public const int PortMinNumber = 1;
    public const int PortMaxNumber = 65535;
    public const int ApplicationIdUberstrike = 1;
    public const int StoredProcedureSuccess = 0;
    public const int StoredProcedureFailure = 1;
    public const int ItemMallFieldDisable = -1;
    public const BuyingDurationType PackDuration = BuyingDurationType.Permanent;
    public const int ItemMinimumDurationInDays = 1;
    public const int ItemMaximumDurationInDays = 90;
    public const int ItemMaximumOwnableAmount = 1000;
    public const char PlaySpanDelimiter = '|';
    public const int NewItemMallItemIdStart = 1000;
    public const int DotoriToSouthKoreanWon = 100;
    public const int UsdToFacebookCredit = 10;
    public const int UsdToKreds = 10;
    public const int NameChangeItem = 1294;
    public const int DiscountPointsSevenDays = 20;
    public const int DiscountPointsThirtyDays = 25;
    public const int DiscountPointsNinetyDays = 30;
    public const int DiscountCreditsSevenDays = 40;
    public const int DiscountCreditsThirtyDays = 75;
    public const int DiscountCreditsNinetyDays = 85;
    public const int DiscountPackThree = 20;
    public const int LuckyDrawSetCount = 3;
    public const int LuckyDrawSetItemMaxCount = 12;
    public const int MysteryBoxItemMaxCount = 12;
    public const int GroupMembersLimitCount = 12;
    public const int PrivateMessagesInboxPageSize = 30;
    public const int PrivateMessagesThreadPageSize = 30;
    public const int PrivateerUberStrikeLicenseId = 1094;
    public const int ClanLeaderUberStrikeLicenseId = 1234;
    public const int MacAppStoreUberStrikeLicenseId = 1265;
    public static readonly string[] DefaultContactGroupToBeCreated = new string[1]
    {
      "Default"
    };
    public static readonly Dictionary<int, string> ApplicationsName = new Dictionary<int, string>()
    {
      {
        1,
        "Uberstrike"
      }
    };
    public static readonly Dictionary<long, int> FacebookApplicationsId = new Dictionary<long, int>()
    {
      {
        24509077139L,
        1
      }
    };
    public static readonly List<int> ApplicationsHavingPayingClient = new List<int>()
    {
      1
    };
    public static readonly List<ChannelType> ActiveChannels = new List<ChannelType>()
    {
      ChannelType.MacAppStore,
      ChannelType.OSXStandalone,
      ChannelType.WebFacebook,
      ChannelType.WebPortal,
      ChannelType.WindowsStandalone,
      ChannelType.Kongregate,
      ChannelType.IPad,
      ChannelType.IPhone,
      ChannelType.Android
    };
    public static readonly List<ChannelType> WebChannels = new List<ChannelType>()
    {
      ChannelType.WebFacebook,
      ChannelType.WebPortal,
      ChannelType.Kongregate
    };
    public static readonly List<ChannelType> StandaloneChannels = new List<ChannelType>()
    {
      ChannelType.WindowsStandalone
    };
    public static readonly Dictionary<EsnsType, string> EsnsProfilesUrl = new Dictionary<EsnsType, string>()
    {
      {
        EsnsType.Aol,
        string.Empty
      },
      {
        EsnsType.Facebook,
        "http://www.facebook.com/profile.php?id="
      },
      {
        EsnsType.Gmail,
        string.Empty
      },
      {
        EsnsType.LinkedIn,
        string.Empty
      },
      {
        EsnsType.MySpace,
        "http://www.myspace.com/"
      },
      {
        EsnsType.None,
        string.Empty
      },
      {
        EsnsType.WindowsLive,
        string.Empty
      },
      {
        EsnsType.Yahoo,
        string.Empty
      },
      {
        EsnsType.Cyworld,
        string.Empty
      }
    };
    public static readonly Dictionary<PaymentProviderType, string> PaymentProviderName = new Dictionary<PaymentProviderType, string>()
    {
      {
        PaymentProviderType.Cmune,
        "Cmune"
      },
      {
        PaymentProviderType.Offerpal,
        "Offerpal"
      },
      {
        PaymentProviderType.PlaySpan,
        "PlaySpan"
      },
      {
        PaymentProviderType.SixWaves,
        "Six Waves"
      },
      {
        PaymentProviderType.Zong,
        "Zong"
      },
      {
        PaymentProviderType.SuperRewards,
        "Super Rewards"
      },
      {
        PaymentProviderType.Dotori,
        "Cyworld"
      },
      {
        PaymentProviderType.FacebookCredits,
        "Facebook"
      },
      {
        PaymentProviderType.GameSultan,
        "Game Sultan"
      },
      {
        PaymentProviderType.Apple,
        "Apple"
      },
      {
        PaymentProviderType.KongregateKreds,
        "Kongregate Kreds"
      },
      {
        PaymentProviderType.CherryCredits,
        "Cherry Pins"
      }
    };
    public static readonly Dictionary<ReferrerPartnerType, string> ReferrerPartnerName = new Dictionary<ReferrerPartnerType, string>()
    {
      {
        ReferrerPartnerType.None,
        "None"
      },
      {
        ReferrerPartnerType.AppleWidget,
        "Apple Widget"
      },
      {
        ReferrerPartnerType.MySpace,
        "MySpace"
      },
      {
        ReferrerPartnerType.SixWaves,
        "6waves"
      },
      {
        ReferrerPartnerType.Applifier,
        "Applifier"
      }
    };
    public static readonly Dictionary<BuyingDurationType, string> BuyingDurationName = new Dictionary<BuyingDurationType, string>()
    {
      {
        BuyingDurationType.None,
        "None"
      },
      {
        BuyingDurationType.OneDay,
        "1"
      },
      {
        BuyingDurationType.SevenDays,
        "7"
      },
      {
        BuyingDurationType.ThirtyDays,
        "30"
      },
      {
        BuyingDurationType.NinetyDays,
        "90"
      },
      {
        BuyingDurationType.Permanent,
        "Permanent"
      }
    };
    public static readonly DateTime UberStrikeStartingDate = new DateTime(2010, 9, 15, 0, 0, 0);
    public static readonly Dictionary<string, string> AcceptedCurrencies = new Dictionary<string, string>()
    {
      {
        "USD",
        "USD"
      },
      {
        "KRW",
        "KRW"
      }
    };
    public static readonly Dictionary<string, int> CurrenciesToCreditsConversionRate = new Dictionary<string, int>()
    {
      {
        "USD",
        650
      },
      {
        "KRW",
        1
      }
    };
    public static readonly int LoginMinPointsPerDay = 500;
    public static readonly int LoginMaxPointsPerDay = 1500;
    public static readonly int LoginDailyGrowth = 100;
    public static readonly int LoginDailyDecay = 200;
  }
}
