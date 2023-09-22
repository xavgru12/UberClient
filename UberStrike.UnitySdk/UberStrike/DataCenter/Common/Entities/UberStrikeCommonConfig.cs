
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UberStrike.Core.Types;

namespace UberStrike.DataCenter.Common.Entities
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct UberStrikeCommonConfig
  {
    public const int ApplicationId = 1;
    public const int DefaultLevel = 1;
    public const int MaxPlayers = 16;
    public const int LevelCap = 40;
    public const int DefaultMeleeWeapon = 1000;
    public const int DefaultWeapon = 1002;
    public const int DefaultHead = 1084;
    public const int DefaultFace = 1085;
    public const int DefaultGloves = 1086;
    public const int DefaultUpperBody = 1087;
    public const int DefaultLowerBody = 1088;
    public const int DefaultBoots = 1089;
    public const string DefaultSkinColor = "c69c6d";
    public const AvatarType DefaultAvatarType = AvatarType.LutzRavinoff;
    public const int ClanLeaderMinLevel = 4;
    public const int ClanLeaderMinContactsCount = 1;
    public const int LiveFeedCriticalPriority = 0;
    public const int LiveFeedImportantPriority = 1;
    public const int LiveFeedNormalPriority = 2;
    public const int LiveFeedDescriptionMinLength = 1;
    public const int LiveFeedDescriptionMaxLength = 140;
    public const int LiveFeedUrlMaxLength = 255;
    public const int MapDisplayNameMinLength = 1;
    public const int MapDisplayNameMaxLength = 20;
    public const int MapDescriptionMinLength = 1;
    public const int MapDescriptionMaxLength = 500;
    public const int MapSceneNameMinLength = 1;
    public const int MapSceneNameMaxLength = 50;
    public const int PrivateerLicenseId = 1094;
    public const int ClanLeaderLicenseId = 1234;
    public const int MacAppStoreLicenseId = 1265;
    public const int XpAttributedOnTutorialCompletion = 80;
    public const string WelcomeMessage = "\r\nWelcome UberStriker,\r\n\r\nTo make your gaming experience as enjoyable as possible, we have written out some guidelines that you need to follow while playing our game. \r\nKeep in mind that these guidelines are here to make sure everyone in the community enjoys the content! \r\n\r\nWe hope that you have a pleasant stay!\r\n\r\nIn-game Rules:\r\n \r\nChatting -\r\n• No swearing or inappropriate content.\r\n• No \"Caps lock\" (using it for emphasis is okay).\r\n• No spamming.\r\n• Do not personally attack any person(s).\r\n• No backseat moderating. Please use the report button.\r\n• Do not discuss topics that involve race, color, creed, religion, sex, or politics.\r\n\r\n \r\nGeneral -\r\n• Alternate or \"Second\" Accounts in-game ARE allowed.\r\n• No account sharing! Your account is yours, and if another player is caught using it, all parties will get banned.\r\n• Exploiting of glitches will not be tolerated. Cheating of any kind will result in a permanent ban.\r\n• Be respectful to the Administrators/Moderators/QAs. These people work hard for you, so please show them respect.\r\n• Advertising of any content unrelated to UberStrike is not permitted.\r\n• Please do not try to cleverly circumvent the rules listed here. These rules are general guidelines and are very flexible, and will be enforced.\r\n• Join a server in your area. You will not get banned for lagging, although you may get kicked from the current game.\r\n• Above all, use common sense.\r\n• Have fun:-)\r\n";
    public static readonly List<int> FirstLoadoutItemIds = new List<int>()
    {
      1000,
      1002,
      1084,
      1085,
      1086,
      1087,
      1088,
      1089,
      1094
    };
    public static readonly List<int> FirstLoadoutWeaponItemIds = new List<int>()
    {
      1000,
      1002
    };
    public static readonly LoadoutView DefaultLoadout = new LoadoutView(0, 0, 1089, 0, 1085, 0, 0, 0, 1086, 1084, 1088, 1000, 0, 0, 0, AvatarType.LutzRavinoff, 1087, 1002, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "c69c6d");
    public static readonly Dictionary<int, string> LiveFeedPriorityNames = new Dictionary<int, string>()
    {
      {
        0,
        "Critical"
      },
      {
        1,
        "Important"
      },
      {
        2,
        "Normal"
      }
    };
  }
}
