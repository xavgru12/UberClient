
using Cmune.Core.Types.Attributes;

namespace Cmune.Realtime.Common
{
  [ExtendableEnumBounds(21, 250)]
  public class CommRPC : RPC
  {
    public const byte UpdatePlayerRoom = 21;
    public const byte ResetPlayerRoom = 22;
    public const byte ChatMessageInGame = 23;
    public const byte ChatMessageToPlayer = 24;
    public const byte ChatMessageToAll = 25;
    public const byte UpdateIngameGroup = 26;
    public const byte RemoveIngameGroup = 27;
    public const byte ReportPlayers = 28;
    public const byte ModerationGhostPlayer = 29;
    public const byte ModerationMutePlayer = 30;
    public const byte ModerationCustomMessage = 31;
    public const byte ModerationKickGame = 32;
    public const byte ModerationBanPlayer = 33;
    public const byte ModerationUnbanPlayer = 34;
    public const byte GameInviteToPlayer = 35;
    public const byte DisconnectAndDisablePhoton = 36;
    public const byte UpdateInboxRequests = 37;
    public const byte UpdateFriendsList = 38;
    public const byte UpdateInboxMessages = 39;
    public const byte UpdateClanMembers = 40;
    public const byte UpdateClanMemberRanks = 41;
    public const byte ChatMessageInClan = 42;
    public const byte SetContactList = 43;
    public const byte UpdateContacts = 44;
    public const byte SpeedhackDetection = 45;
    public const byte UpdateActorsForModeration = 46;
    public const byte UpdateAllActors = 47;
    public const byte ClearModeratorFlags = 48;
    public const byte ModerationPermanentBan = 49;
    public const byte SpeedhackDetectionNew = 50;
    public const byte HidePlayer = 51;
    public const byte SendPlayerNameSearchString = 52;
    public const byte UpdateClanData = 53;
  }
}
