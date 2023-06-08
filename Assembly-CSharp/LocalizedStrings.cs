using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using UberStrike.Core.Types;
using UnityEngine;

public static class LocalizedStrings
{
	public static string Login = "Login";

	public static string Logout = "Logout";

	public static string LogoutCaps = "LOGOUT";

	public static string EmailAddress = "Email Address";

	public static string Password = "Password";

	public static string RememberMe = "Remember Me";

	public static string ForgotPassword = "Forgot Password?";

	public static string Quit = "Quit";

	public static string OkCaps = "OK";

	public static string ChangeTeam = "Change Team";

	public static string Continue = "Continue";

	public static string DeathMatch = "Deathmatch";

	public static string Fullscreen = "Fullscreen";

	public static string Headshot = "Headshot!";

	public static string KillsRemain = "Kills Remain";

	public static string Nutshot = "Nutshot!";

	public static string Respawn = "Respawn";

	public static string ShowMenu = "Show Menu";

	public static string SignUp = "Create new account";

	public static string TeamDeathMatch = "Team Deathmatch";

	public static string TeamElimination = "Team Elimination";

	public static string YouDied = "You Died!";

	public static string Absorption = "Absorption";

	public static string Accept = "Accept";

	public static string AcceptingClanInvitation = "Accepting clan invitation...";

	public static string AccountExistsMsg = "This account already has a UberStrike account associated with it.";

	public static string AccountIsInvalid = "Account is invalid.";

	public static string Accuracy = "Accuracy";

	public static string AddFriends = "Add Friends";

	public static string AdvancedCaps = "ADVANCED";

	public static string AdvancedWater = "Advanced Water";

	public static string ALeaderCannotLeaveHisOwnClanMsg = "A leader cannot leave his own clan.\nYou can only disband it.";

	public static string All = "All";

	public static string AreYouSureQuitMsg = "Are you sure you want to quit Uberstrike?";

	public static string AreYouSureLogoutMsg = "Are you sure you want to logout of Uberstrike?";

	public static string AuthenticatingAccount = "Authenticating Account";

	public static string BackCaps = "BACK";

	public static string BangYouJustReachedLevelN = "Bang! You just reached";

	public static string BestWeaponByAccuracy = "Best Weapon by Accuracy";

	public static string BestWeaponByDamageDealt = "Best Weapon by Damage Dealt";

	public static string BestWeaponByHits = "Best Weapon by Hits";

	public static string BestWeaponByKills = "Best Weapon by Kills";

	public static string BetweenYouAndN = "Between You and {0}";

	public static string BloomEffect = "Bloom Effect";

	public static string Blue = "Blue";

	public static string Boots = "Boots";

	public static string BothTeamsHadAndEqualNumberOfKills = "Both teams had an equal number of kills at match end.";

	public static string BothTeamsHadAndEqualNumberOfWins = "Both teams had an equal number of wins at match end.";

	public static string Bounty = "Bounty";

	public static string Busy = "Busy";

	public static string Buy = "Buy";

	public static string BuyAClanLicense = "- Buy a clan license in the Shop";

	public static string BuyCaps = "BUY";

	public static string BuyItem = "Buy Item";

	public static string Cancel = "Cancel";

	public static string CancelCaps = "CANCEL";

	public static string CancelingInvitation = "Canceling invitation...";

	public static string CancelingInvitationErrorMsg = "There has been an error canceling the invitation to {0}.";

	public static string CancelInvite = "Cancel Invite";

	public static string CancelInviteConfirmation = "Cancel Invite Confirmation";

	public static string CancelInviteToN = "Cancel invite to {0}?";

	public static string CancelInviteWarningMsg = "This will cancel the clan invitation to {0}.";

	public static string Cannons = "Cannons";

	public static string CannotFindPlayerData = "Cannot find any data for that player.";

	public static string Capacity = "Capacity";

	public static string CapacityDesc = "This refers to the amount of players that are currently playing on this game server.";

	public static string ChangePosition = "Change Position";

	public static string ChangeServer = "Change Server";

	public static string ChangingToTeamN = "Changing to team {0}";

	public static string ChatBtnTooltip = "Chat with friends and join their games.";

	public static string ChatCaps = "CHAT";

	public static string CheatingWillResultInAPermanentBan = "Cheating will result in a permanent ban!";

	public static string CheckAvailability = "Check Availability";

	public static string CheckingAvailableGameServers = "Checking available game servers...";

	public static string CheckingClanStatus = "Checking clan status...";

	public static string CheckMail = "Check Mail";

	public static string ChooseAGameCaps = "CHOOSE A GAME";

	public static string ChooseAMap = "Choose a Map";

	public static string ChooseCharacterName = "CHOOSE YOUR NAME";

	public static string ChooseYourRegionCaps = "CHOOSE YOUR REGION";

	public static string ClanAlreadyHasPendingRequestMsg = "This clan already has a pending request from you.\nYou cannot send another one.";

	public static string ClanBtnTooltip = "Create or manage a clan.";

	public static string ClanCreateSuccessMsg = "Congratulations! You have successfully created the {0} clan.\nInvite some members to join and sign up for clan wars on the forums!";

	public static string ClanDataNotFoundMsg = "Cannot find any data for that clan.\nEither an error has occurred or it has just disbanded.";

	public static string ClanDescriptionInvalidCharsMsg = "The description you have chosen contains\ninvalid characters(probably the weird ones).\nPlease specify a different one.";

	public static string ClanDescriptionRestrictedWordsMsg = "The description you have chosen contains restricted words.\nPlease specify a different one.";

	public static string ClanFullMsg = "This clan is full. You cannot request membership\nto a clan that is full.";

	public static string ClanHQCaps = "CLAN HQ";

	public static string ClanInvitationDeletedMsg = "This invitation no longer exists.\nIt may have expired or been withdrawn.";

	public static string ClanMottoInvalidMsg = "The motto you have chosen contains\ninvalid characters (probably the weird ones).\nPlease specify a different one.";

	public static string ClanMottoRestrictedWordsMsg = "The motto you have chosen contains restricted words.\nPlease specify a different one.";

	public static string ClanNameInUseMsg = "This clan name is already in use.\nPlease choose a different one.";

	public static string ClanNameInvlidMsg = "The name you have chosen contains non-alphanumeric characters.\nPlease specify a different one.";

	public static string ClanNameRestrictedWordsMsg = "The name you have chosen contains restricted words.\nPlease specify a different one.";

	public static string ClanNCreatedCaps = "CLAN {0} CREATED";

	public static string ClanRequestsYouHAveNPendingRequests = "Clan Requests - You have {0} pending clan request{1}";

	public static string Clans = "Clans";

	public static string ClansCaps = "CLANS";

	public static string ClanStatusErrorMsg = "Error checking player clan status.\nPlease try again in a moment.";

	public static string ClanTagInUseMsg = "This clan tag is already in use.\nPlease choose a different one.";

	public static string ClanTagInvalidMsg = "This clan tag contains non-alphanumeric characters.\nPlease specify a different one.";

	public static string ClanTagRestrictedWordsMsg = "The tag you have chosen contains restricted words.\nPlease specify a different one.";

	public static string ClickHereBuyCreditsMsg = "Click Here to buy UberStrike Credits!";

	public static string ColorCorrection = "Color Correction";

	public static string ComeBackTomorrowForEvenMorePoints = "Come back tomorrow for even more points!";

	public static string CongratulationsGrantedLicenseMsg = "Congratulations, you have just been granted the privateers license!\n\nLet's have some fun!";

	public static string ConnectingToLobby = "Connecting to Lobby...";

	public static string ConnectingToServer = "Connecting to server.";

	public static string ConnectionSlowMsg = "Your connection to this server is slow. Your game performance may be sub-optimal.";

	public static string CreateAClan = "Create a Clan";

	public static string CreateAClanAndInviteYourFriends = "Create a clan and invite your friends to join!";

	public static string CreateAClanCaps = "CREATE A CLAN";

	public static string CreateCaps = "CREATE";

	public static string CreatedN = "Created: {0}";

	public static string CreateGameCaps = "CREATE GAME";

	public static string CreateYourCharacterCaps = "CREATE YOUR CHARACTER";

	public static string CreatingAccount = "Creating Account...";

	public static string CreatingAccountMsg = "Creating your account.\nPlease wait a moment.";

	public static string Credits = "Credits";

	public static string CurrentXP = "Current XP:";

	public static string DailyRewardCaps = "DAILY REWARD";

	public static string Damage = "Damage";

	public static string DamageXP = "DAMAGE XP";

	public static string DataError = "Data Error";

	public static string DataInvalidMsg = "That data is extremely invalid. It's so invalid that it has caused this error.";

	public static string Day = "Day";

	public static string DaysAgo = "Days Ago";

	public static string DeathsCaps = "DEATHS";

	public static string Deaths = "Deaths";

	public static string DeleteThreadCaps = "DELETE THREAD";

	public static string DemoteMember = "Demote Member";

	public static string DisbandClan = "Disband Clan";

	public static string DisbandClanCaps = "DISBAND CLAN";

	public static string DisbandClanErrorMsg = "There has been an error processing your request to disband clan {0}.\nPlease try again in a moment.";

	public static string DisbandClanN = "Disband clan {0}?";

	public static string DisbandClanSuccessMsg = "Clan {0} has been disbanded.";

	public static string DisbandClanWarningMsg = "Disbanding your clan will completely remove all clan information from the\nUberStrike database. After disbanding, the clan will no longer exist.";

	public static string DiscardCaps = "DISCARD";

	public static string Done = "Done";

	public static string DownloadClanRequestErrorMsg = "Error downloading clan request data. Please try again in a moment.";

	public static string DownloadingClanData = "Downloading clan data...";

	public static string DownloadingClanDataErrorMsg = "Error downloading clan data.\nPlease try again in a moment.";

	public static string DownloadingRequestData = "Downloading request data...";

	public static string DoYouReallyWantToRemoveNFromYourFriendsList = "Do you really want to remove {0} from your friends list?";

	public static string DoYouWantToDeleteTheConversation = "Do you want to delete the conversation?";

	public static string DragToEquipTheN = "Drag to equip the {0}";

	public static string Draw = "Draw!";

	public static string Duration = "Duration";

	public static string EffectsVolume = "Effects Volume:";

	public static string Email = "Email";

	public static string EmailAddressAndNameInUseMsg = "By some miracle coincidence, both the email address AND the name you've chosen are in use. You'll have to choose different ones.";

	public static string EmailAddressInUseMsg = "This email address is already in use. Please use another email address.";

	public static string EmailAddressIsInvalid = "Email address is invalid.";

	public static string EmailInvalidMsg = "That email address is invalid. Either you typed it wrong or you're an evil hacker trying to fool our system!";

	public static string Empty = "Empty";

	public static string EnableCustomizers = "Enable Customizers";

	public static string EnableGamepad = "Enable Gamepad";

	public static string EnterGameName = "Enter game name";

	public static string EnterPassword = "Enter Password";

	public static string EnterYourEmailAddress = "Enter Your Email Address";

	public static string EnterYourName = "Enter Your Name";

	public static string EnterYourPassword = "Enter Your Password";

	public static string Equip = "Equip";

	public static string Unequip = "Unequip";

	public static string Error = "Error";

	public static string ErrorCreatingClan = "Error creating clan.\nPlease try again later.";

	public static string ErrorInvitingNToJoinClanN = "Error inviting {0} to join clan {1}.\nPlease try again in a moment.";

	public static string ErrorInvitingToJoinClanN = "There was an error inviting the player to clan {0}.";

	public static string ErrorSendingMessage = "Error sending message, Please try again later...";

	public static string ExitFullscreen = "Exit fullscreen.";

	public static string Expired = "Expired";

	public static string Face = "Face";

	public static string FailedToGetContacts = "Failed to get contacts";

	public static string FailedToRegister = "Failed to register";

	public static string FailedToRemoveNFromYourFriendList = "Failed to remove {0} from your friend list!";

	public static string FailedToSendYourMessage = "Failed to send your message!";

	public static string FastCaps = "FAST";

	public static string FiltersCaps = "FILTERS";

	public static string FrameRate = "Frame Rate";

	public static string LowGravity = "Low gravity";

	public static string Instakill = "Instakill";

	public static string NinjaArena = "Ninja Arena";

	public static string SniperArena = "Sniper Arena";

	public static string CannonArena = "Cannon Arena";

	public static string FriendRequest = "Friend Request";

	public static string FriendRequestCaps = "FRIEND REQUEST";

	public static string FriendRequestsYouHaveNPendingRequests = "Friend Requests - You have {0} pending friend request{1}";

	public static string FriendsCaps = "FRIENDS";

	public static string Full = "Full";

	public static string FunctionalItem = "Functional Item";

	public static string FunctionalItems = "Functional Items";

	public static string Game = "Game";

	public static string GameName = "Game Name";

	public static string GameNotFull = "Game not full";

	public static string Gamepad = "Gamepad";

	public static string Games = "Games";

	public static string GamesNotFound = "Games not found.";

	public static string GameType = "Game Type";

	public static string Gear = "Gear";

	public static string GeneralCaps = "GENERAL";

	public static string BuyCreditsCaps = "GET CREDITS!";

	public static string GettingClan = "Getting player clan data...";

	public static string GettingInventory = "Getting player inventory...";

	public static string GettingLabsData = "Getting labs data...";

	public static string GettingLoadout = "Getting player loadout...";

	public static string GettingStats = "Getting player statistics...";

	public static string Gloves = "Gloves";

	public static string GoCaps = "GO!";

	public static string GoFullscreen = "Go fullscreen.";

	public static string HardcoreMode = "Hardcore Mode";

	public static string HaveAtLeastOneFriend = "- Have at least 1 friend";

	public static string Head = "Head";

	public static string HeadshotXP = "HEADSHOT XP";

	public static string ShowPostProcessingEffects = "Enable Post Processing Effects";

	public static string HelpBtnTooltip = "Learn how to really play the game.";

	public static string HelpCaps = "HELP";

	public static string HereYouCanCreateYourOwnClan = "Here you can create your own clan.";

	public static string HiYoureInvitedToJoinMyClanN = "Hi, you're invited to join my clan, {0}.";

	public static string Holo = "Holo";

	public static string HomeBtnTooltip = "Back to the blimp.";

	public static string HomeCaps = "HOME";

	public static string IAgreeToThe = "By clicking OK you agree to the";

	public static string IfTheErrorPersistsBugReportTaskbar = "If the error persists, please use\nthe bug reporting feature on the taskbar.";

	public static string Ignore = "Ignore";

	public static string IgnoringClanInvitation = "Ignoring clan invitation...";

	public static string Impact = "Impact";

	public static string InboxBtnTooltip = "Receive messages from your friends.";

	public static string InboxCaps = "INBOX";

	public static string InGame = "In Game";

	public static string InivitingNToJoinClanN = "Inviting {0} to join clan {1}...";

	public static string InMyClan = "In my clan";

	public static string InvalidData = "Invalid data has been supplied.";

	public static string Inventory = "Inventory";

	public static string InventoryCaps = "INVENTORY";

	public static string InvertMouseButtons = "Invert Mouse Buttons";

	public static string InvitationCancelled = "Invitation Cancelled";

	public static string InvitationPending = "Invitation Pending";

	public static string InvitationToJoinClanNHasBeenSent = "Invitation to join clan {0}\nhas been sent.";

	public static string Invite = "Invite!";

	public static string InviteFriends = "Invite Friends";

	public static string InvitePlayer = "Invite Player";

	public static string InvitingToJoinClan = "Inviting to join clan...";

	public static string JoinBlueCaps = "JOIN BLUE";

	public static string JoinCaps = "JOIN";

	public static string JoinDate = "Join Date";

	public static string JoinRedCaps = "JOIN RED";

	public static string JoinClanSuccessMsg = "You have successfully joined the clan.";

	public static string JoinClanErrorMsg = "There was an error joining the clan.";

	public static string JustForFun = "Just for fun";

	public static string KDR = "KDR";

	public static string Keyboard = "Keyboard";

	public static string KeyButton = "Key/Button";

	public static string Kills = "Kills";

	public static string KillXP = "KILL XP";

	public static string LabsCaps = "LABS";

	public static string LastDay = "Last Day";

	public static string LastOnlineN = "Last Online: {0}";

	public static string Launchers = "Launchers";

	public static string Leader = "Leader";

	public static string LeaderN = "Leader: {0}";

	public static string LeaveClan = "Leave Clan";

	public static string LeaveClanCaps = "LEAVE CLAN";

	public static string LeaveClanErrorMsg = "There has been an error processing your request to leave clan {0}. Please try again in a moment.";

	public static string LeaveClanN = "Leave clan {0}?";

	public static string LeaveClanSuccessMsg = "You have successfully left the clan {0}.";

	public static string LeaveClanWarningMsg = "After leaving you will no longer receive clan notifications or have the clan tag displayed next to your name.";

	public static string LeavingClanN = "Leaving Clan {0}...";

	public static string Level = "Level";

	public static string LevelAndXP = "Level & XP";

	public static string LimitFramerate = "Limit FrameRate:";

	public static string LoadingGameList = "Loading game list...";

	public static string LoadoutCaps = "LOADOUT";

	public static string LoginFailed = "Login Failed";

	public static string Low = "Low";

	public static string LowerBody = "Lower Body";

	public static string LowerBodyArmor = "Lower Body Armor";

	public static string Machineguns = "Machineguns";

	public static string ManageYourClan = "Manage your clan";

	public static string Map = "Map";

	public static string MasterVolume = "Master Volume:";

	public static string MatchPerformance = "Match Performance";

	public static string MaxPlayers = "Max Players";

	public static string MedCaps = "MED";

	public static string Melee = "Melee";

	public static string MeleeWeapons = "Melee Weapons";

	public static string Member = "Member";

	public static string Message = "Message:";

	public static string MessageCaps = "MESSAGE:";

	public static string MessagesCaps = "MESSAGES";

	public static string Minutes = "Minutes";

	public static string Misc = "Misc";

	public static string Mode = "Mode";

	public static string Moderate = "Moderate";

	public static string MonthsAgo = "Months Ago";

	public static string MostArmorPickedUp = "Most Armor Picked Up";

	public static string MostCannonKills = "Most Cannon Kills";

	public static string MostConsecutiveSnipes = "Most Consecutive Snipes";

	public static string MostDamageDealt = "Most Damage Dealt";

	public static string MostDamageReceived = "Most Damage Received";

	public static string MostDeadlyWeapons = "Most Deadly Weapons";

	public static string MostHeadshots = "Most Headshots";

	public static string MostHealthPickedUp = "Most Health Picked Up";

	public static string MostKills = "Most Kills";

	public static string MostLauncherKills = "Most Launcher Kills";

	public static string MostMachinegunKills = "Most Machinegun Kills";

	public static string MostMeleeKills = "Most Melee Kills";

	public static string MostNutshots = "Most Nutshots";

	public static string MostShotgunKills = "Most Shotgun Kills";

	public static string MostSniperRifleKills = "Most Sniper Rifle Kills";

	public static string MostSplattergunKills = "Most Splattergun Kills";

	public static string MostValuablePlayers = "Most Valuable Players";

	public static string MostXPEarned = "Most XP Earned";

	public static string MotionBlur = "Motion Blur";

	public static string Motto = "Motto";

	public static string MottoN = "Motto: {0}";

	public static string Mouse = "Mouse";

	public static string MouseSensitivity = "Mouse Sensitivity:";

	public static string Movement = "Movement";

	public static string MusicVolume = "Background/Music:";

	public static string Mute = "Mute audio.";

	public static string Name = "Name";

	public static string NameInUseMsg = "This name has already been taken. Please choose another one.";

	public static string NameInvalidCharsMsg = "That name contains invalid characters.";

	public static string NameN = "Name: {0}";

	public static string NameTooManySpacesMsg = "That name has too many spaces.";

	public static string NameTooShortMsg = "That name is too short.";

	public static string NClanInformation = "[{0}] Clan Information";

	public static string NClanRoster = "[{0}] Clan Roster";

	public static string NetworkError = "Network Error";

	public static string NewAndSaleItems = "New And Sale Items";

	public static string NewHereSignUpAndPlayForFree = "New here? SIGN UP and play for free!";

	public static string NewMessage = "New Mail";

	public static string NextCaps = "NEXT";

	public static string NiceJobThatWasSeriousCarnage = "Nice job, that was some serious carnage!";

	public static string NinetyDays = "90 Days";

	public static string NIsNoLongerAMemberOfClanN = "{0} is no longer a member of clan {1}";

	public static string NIsNoLongerInvitedToJoinClanN = "{0} is no longer invited to jon clan {1}.";

	public static string NMembersNOnline = "{0}/{1} members ({2} online)";

	public static string NNeedsAClanLicenseToBeLeader = "{0} need a Clan License to be leader of a clan.\nThe Clan License is a special item\nthat {0} can buy in the Shop.";

	public static string NNeedsToBeLevelFourToBeAClanLeader = "{0} need to be at least level 4\nto be leader of a clan.";

	public static string NNeedsToHaveOneFriendToBeAClanLeader = "{0} need to have at least 1 friend\nto be leader of a clan.";

	public static string NoConversationSelected = "No Conversation Selected";

	public static string NoFriendlyFire = "No Friendly Fire";

	public static string None = "None";

	public static string NoSelectedItems = "No Selected Items";

	public static string NoThanks = "No Thanks";

	public static string NotPasswordProtected = "Not password protected";

	public static string NPercentOff = "{0:N0}% Off!";

	public static string NPlayers = "{0} Players";

	public static string NTeamWins = "{0} Team Wins!";

	public static string NutshotXP = "NUTSHOT XP";

	public static string OffensiveNameMsg = "The name contains offensive words.";

	public static string Officer = "Officer";

	public static string Offline = "Offline";

	public static string On = "On";

	public static string OneDay = "1 Day";

	public static string Online = "Online";

	public static string OnlyYourClanLeaderCanDoThis = "Only your clan leader can do this.";

	public static string OptionsBtnTooltip = "Configure controls, visual and gameplay options.";

	public static string OptionsCaps = "OPTIONS";

	public static string PasswordDoNotMatch = "Passwords do not match.";

	public static string PasswordIncorrect = "Password Incorrect";

	public static string PasswordInvalidCharsMsg = "Password should have at least 6 characters.";

	public static string PasswordIsInvalid = "Password is invalid.";

	public static string PasswordProtected = "Password Protected";

	public static string PasswordsDoNotMatch = "Passwords do not match.";

	public static string PendingRequestsCaps = "PENDING REQUESTS";

	public static string PerformanceReportCaps = "PERFORMANCE REPORT";

	public static string Permanent = "Permanent";

	public static string Personal = "Personal";

	public static string PersonalRecordsPerLife = "Personal Records (per Life)";

	public static string Ping = "Ping";

	public static string PlayBtnTooltip = "Join other players games.";

	public static string PlayCaps = "PLAY";

	public static string Player = "Player";

	public static string PlayerAlreadyHasPendingClanInviteMsg = "This player already has a pending invitation.\nYou cannot send another one.";

	public static string PlayerAlreadyInClanMsg = "This player is already in a clan. You cannot invite players\nto join your clan if they are already in a clan.";

	public static string PlayerCaps = "PLAYER:";

	public static string PlayerLevelNMinusRestriction = "[Player Level <{0} Restriction]";

	public static string PlayerLevelNPlusRestriction = "[Player Level {0}+ Restriction]";

	public static string PlayerLevelNRestriction = "[Player Level {0} Restriction]";

	public static string PlayerLevelNToNRestriction = "[Player Level {0}-{1} Restriction]";

	public static string Players = "Players";

	public static string PlayersOnline = "Players Online";

	public static string PlayTime = "Playtime";

	public static string PleaseAgreeTOSMsg = "Please agree to the Terms of Service.";

	public static string PleaseProvideValidEmailPasswordMsg = "Please provide a valid email address and password.\nThis email will be used as your login in future.";

	public static string PleaseWait = "Please wait";

	public static string Points = "Points";

	public static string PointsEarnedN = "Points Earned {0}";

	public static string Position = "Position";

	public static string PressRefreshToSeeCurrentGames = "Press Refresh to see current games";

	public static string Price = "Price";

	public static string PrimaryWeapon = "Primary";

	public static string PrivateChat = "Private Chat";

	public static string PrivateersLicense = "Privateers\nLicense";

	public static string PrivateersLicenseGranted = "Privateers License Granted!";

	public static string PrivatePosition = "Private Chat";

	public static string ProblemBuyingItem = "Problem Buying Item";

	public static string ProcessingLogin = "Processing Login...";

	public static string ProcessingTransactionPleaseWait = "Processing transaction, please wait...";

	public static string PromoteMember = "Promote Member";

	public static string QualitySettings = "Quality Settings";

	public static string QuickItem = "Quick Item";

	public static string QuickItems = "Quick Items";

	public static string QuickSearch = "Quick Search";

	public static string QuitCaps = "QUIT";

	public static string RateOfFire = "Rate of Fire";

	public static string ReachLevelFour = "- Reach Level 4";

	public static string ReadyCaps = "READY";

	public static string RecentPickupWeapons = "Recent Pick-up Weapons";

	public static string Recoil = "Recoil";

	public static string Red = "Red";

	public static string Refresh = "Refresh";

	public static string RefreshCaps = "REFRESH";

	public static string RefreshingServer = "Refreshing server";

	public static string RejectClanInvitation = "Reject Clan Invitation";

	public static string RejectTheClanInvitationFromN = "Reject the clan invitation from {0}?";

	public static string RemainingXP = "Remaining XP:";

	public static string Remove = "Remove";

	public static string RemoveFriendCaps = "REMOVE FRIEND";

	public static string RemoveMember = "Remove Member";

	public static string RemoveMemberCaps = "REMOVE MEMBER";

	public static string RemoveMemberErrorMsg = "Error removing {0} from clan {1}.\nPlease try again in a moment.";

	public static string RemoveMemberWarningMsg = "The clan tag will not show up next to their name and they will not receive clan notifications.";

	public static string RemoveNFromClanN = "Remove {0} from clan {1}?";

	public static string RemovingNFromClanN = "Removing {0} from clan {1}...";

	public static string Renew = "Renew";

	public static string Reply = "Reply";

	public static string ReportBug = "Report a bug.";

	public static string ReportPlayer = "Report a player.";

	public static string RequestNoLongerExistsMsg = "This request no longer exists.\nIt may have expired or been withdrawn.";

	public static string RequestsCaps = "REQUESTS";

	public static string ResetDefaults = "Reset Defaults";

	public static string ResetFiltersCaps = "RESET FILTERS";

	public static string Retry = "Retry";

	public static string RetypeYourPassword = "Retype Your Password";

	public static string Round = "Round";

	public static string RoundEndCaps = "ROUND END!";

	public static string StartsInCaps = "STARTS IN";

	public static string RoundTime = "Round Time";

	public static string Search = "Search";

	public static string SearchMessages = "Search Messages";

	public static string SecondaryWeapon = "Secondary";

	public static string SelectWeapon = "Select weapon:";

	public static string SelectYourLoadoutCaps = "SELECT YOUR LOADOUT";

	public static string SendCaps = "SEND";

	public static string SendMessage = "Send Mail";

	public static string SentInvites = "Sent Invites";

	public static string ServerError = "Server Error";

	public static string ServerFull = "Server full";

	public static string ServerFullMsg = "This server is full. Please select a different server.";

	public static string ServerIsNotReachable = "Server is not reachable";

	public static string ServerName = "Server Name";

	public static string ServerNameDesc = "The server is named after the region that the server is located. Choose the server that is located closest to you for the best network performance.";

	public static string SettingUp = "Setting Up...";

	public static string SevenDays = "7 Days";

	public static string Share = "Share!";

	public static string ShareTheFunWithYourFaceBookFriends = "Share the fun with your Facebook friends!";

	public static string ShopBtnTooltip = "Equip your character with kick ass weapons and gear!";

	public static string ShopCaps = "SHOP";

	public static string Shotguns = "Shotguns";

	public static string ShowFPS = "Show FPS in-game";

	public static string SingleWeaponLimit = "Single Weapon Limit";

	public static string SkinTone = "Skin Tone";

	public static string SlowCaps = "SLOW";

	public static string Smackdown = "Smackdowns";

	public static string SniperRifles = "Sniper Rifles";

	public static string SocNetInvalidMsg = "The social network you have arrived from is incorrect.";

	public static string Spectate = "Spectate";

	public static string Speed = "Speed";

	public static string SpeedDesc = "This refers to the network performance between you and the game server. The number value (known as 'ping'), in milliseconds, refers to the amount of time it takes for information to travel from your computer to the game server and back. Lower ping equates to better network performance, so always look to join the fastest available server.";

	public static string KillLimit = "Kill Limit";

	public static string Splatterguns = "Splatterguns";

	public static string StartTypingTheNameOfAFriend = "Start typing the name of a friend";

	public static string StatsBtnTooltip = "Check your characters statistics.";

	public static string ProfileCaps = "PROFILE";

	public static string Status = "Status";

	public static string Subject = "Subject:";

	public static string SuicideXP = "SUICIDE XP";

	public static string Tag = "Tag";

	public static string TermsOfService = "Terms of Service";

	public static string TertiaryWeapon = "Tertiary";

	public static string TheAmountYouTriedToPurchaseIsInvalid = "Sorry, the maximum amount for this consumable is {0}!";

	public static string TheNTeamHadTheMostKillsAtMatchEnd = "The {0} team had the most kills at match end.";

	public static string ThereAreNoPendingRequestsForN = "There are no pending requests for {0}.";

	public static string ThirtyDays = "30 Days";

	public static string ThisActionCannotBeUndoneCaps = "THIS ACTION CANNOT BE UNDONE.";

	public static string ThisGameIsFull = "This game is full.";

	public static string ThisGameNoLongerExists = "This game no longer exists.";

	public static string ThisIsAListOfPeopleYouHaveAskedToJoinClanNotAccepted = "This is a list of people you have asked to join the clan who have not yet accepted.";

	public static string ThisIsTheOfficialNameOfYourClan = "This is the official name of your clan.";

	public static string ThisIsYourOfficialClanMotto = "This is your official clan motto.";

	public static string ThisItemCannotBePurchasedForDuration = "This item cannot be purchased for that particular duration.";

	public static string ThisItemCannotBePurchasedFromTheShop = "This item cannot be purchased from the Shop.";

	public static string ThisItemCannotBePurchasedFromTheUnderground = "This item cannot be purchased from the Underground.";

	public static string ThisItemCannotBePurchasedPermanently = "This item cannot be purchased permanently.";

	public static string ThisItemCannotBeRented = "This item cannot be rented.";

	public static string ThisItemIsNotForSale = "This item is not for sale!";

	public static string ThisItemIsOutOfStock = "This item is now out of stock.";

	public static string ThisPackIsDisabled = "This pack is disabled.";

	public static string ThisTagGetsDisplayedNextToYourName = "This tag gets displayed next to clan member names.";

	public static string ThisWillChangeNPositionToN = "This will change {0} position to {1}.";

	public static string TiedForWinner = "Tied for winner!";

	public static string TimeLimit = "Time Limit";

	public static string To = "To:";

	public static string ToCreateAClanYouStillNeedTo = "To create a clan, you still need to:";

	public static string Today = "Today";

	public static string TopBlues = "Top Blue";

	public static string TopReds = "Top Reds";

	public static string TrainingCaps = "TRAINING";

	public static string TrainingModeDesc = "Explore mode is for you to practice without the stress of having other players around shooting you in the face.";

	public static string TranserClanSuccessMsg = "Leadership of clan {0} has successfully\nbe transferred to {1}";

	public static string TransferClanErrorMsg = "Error transferring leadership of clan {0} to {1}.\nPlease try again in a moment..";

	public static string TransferClanLeaderhsipToN = "Transfer clan leadership to {0}?";

	public static string TransferClanWarningMsg = "You will not be the leader of this clan anymore.";

	public static string TransferLeadership = "Transfer Leadership";

	public static string TransferLeadershipCaps = "TRANSFER LEADERSHIP";

	public static string TransferringLeadershipOfClanNtoN = "Transferring leadership of clan {0} to {1}...";

	public static string Try = "Try";

	public static string TryThe = "Try the";

	public static string TryYourWeapons = "Try your Weapons";

	public static string TypePasswordHere = "Type Password Here";

	public static string UndergroundCaps = "UNDERGROUND";

	public static string UnequippingDuplicateFunctionalItemClass = "Unequipping duplicate\nfunctional item class.";

	public static string UnequippingDuplicateQuickItemClass = "Unequipping duplicate\nquick item class.";

	public static string UnequippingDuplicateWeaponClass = "Unequipping duplicate\nweapon class.";

	public static string UnexpectedReturn = "Unexpected Return";

	public static string Unknown = "Unknown";

	public static string Unmute = "Unmute audio.";

	public static string UnreadyCaps = "UNREADY";

	public static string UnspecifiedErrorMsg = "An error of unspecified nature has\nsurreptitiously caused this popup to appear.\nIt would be great if you could report this.";

	public static string UpdateMemberErrorMsg = "Error updating member status of {0}.\nPlease try again in a moment.";

	public static string UpdateMemberStatus = "Update Member Status";

	public static string UpdateMemberSuccessMsg = "Member status of {0} has been successfully updated.";

	public static string UpdateMemberWarningMsg = "This will change {0} position\n to {1}.";

	public static string UpdateNPosition = "Update {0} position?";

	public static string UpdatingInventory = "Updating Inventory";

	public static string UpdatingItemMall = "Updating Item Mall";

	public static string UpdatingMemberStatusOfN = "Updating member status of {0}...";

	public static string UpperBody = "Upper Body";

	public static string UpperBodyArmor = "Upper Body Armor";

	public static string UseThisFormToSendClanInvitations = "Use this form to send clan invitations to your friends.";

	public static string Velocity = "Velocity";

	public static string VerifyPassword = "Verify Password";

	public static string ViewOutstandingInvitesAndRequests = "View outstanding invites and requests";

	public static string Volume = "Volume";

	public static string Warning = "Warning";

	public static string WaterQuality = "Water Quality";

	public static string WeaponClassRecords = "Weapon Class Records";

	public static string WeaponLoadout = "Weapon Loadout";

	public static string WeaponPerformaceTotal = "Weapon Performance (in Total)";

	public static string Weapons = "Weapons";

	public static string WeaponsCaps = "WEAPONS";

	public static string Welcome = "Welcome";

	public static string WelcomeToUS = "Welcome To UberStrike";

	public static string WereUpdatingTheItemMallPleaseWait = "We're updating the item mall, please wait...";

	public static string WereUpdatingYourInventoryPleaseWait = "We're updating your inventory, please wait...";

	public static string WonWithAScoreOf = "won with a score of";

	public static string XPEarnedN = "XP Earned {0}";

	public static string Yesterday = "Yesterday";

	public static string YouAlreadyMasteredThisLevel = "You already mastered this Level!";

	public static string YouAlreadyOwnThisItem = "You already own this item.";

	public static string YouCannotPurchaseThisItemForMoreThanNDays = "You cannot purchase this item for more than {0} days.";

	public static string YouCantChangeYourClanInfoOnceCreated = "You can’t change any of your clan info once your clan is created, so make it count!";

	public static string YouDontHaveEnoughPointsOrCreditsToPurchaseThisItem = "You don't have enough points or credits to buy this item.";

	public static string YouDontNeedToEquipLicenses = "You don't need to\nequip licenses.";

	public static string YouDontPermissionToPerformThisAction = "You don't have the right permissions to perform this action.";

	public static string YouHaveNFriends = "You have {0} friends";

	public static string YouHaveNNewMessages = "You have {0} new message(s)";

	public static string YouHaveNoFriends = "You have no friends.";

	public static string YouHaveOnlyOneFriend = "You have only one friend";

	public static string YouHaveToReachLevelNToJoinThisGame = "You have to reach Level {0} to join this game!";

	public static string YouNeedAClanLicenseToCreateClanMsg = "You need a Clan License to create a clan.\nThe Clan License is a special item that you can buy in the Shop.";

	public static string YouNeedAtLeastOneFriendToCreateAClan = "You need to have at least 1 friend to create a clan.";

	public static string YouNeedMoreCreditsToBuyThisItem = "You need more credits to buy this item.";

	public static string YouNeedToBeAtLeastLevelFourToCreateAClan = "You need to be at least level 4 to create a clan.";

	public static string YouNeedToBeLevelNToBuyThisItem = "You need to be Level {0} to buy this item.";

	public static string YouNeedToEarnMorePointsToBuyThisItem = "You need to earn more points to buy this item.";

	public static string YourAccountHasBeenBanned = "Your account\nhas been banned.";

	public static string YourAreNoLongerAMemberOfClanN = "You are no longer a member of clan {0}.";

	public static string YourClanIsBeingCreated = "Your clan is being created...";

	public static string YourFriendRequestIsAlreadySent = "Your friend request is sent already!";

	public static string YourGameNameIsInvalid = "Your Game Name is not valid.";

	public static string YourLevelIsTooLowToBuyThisItem = "Your level is too low to purchase this item.";

	public static string YourMessageHasBeenSent = "Your message has been sent!";

	public static string YourMessageHasNotBeenSent = "Your message has not been sent!";

	public static string YourRequestHasBeenSent = "Your request has been sent!";

	public static string YourProfileCaps = "YOUR PROFILE";

	public static string YouveAlreadyInvitedThatPlayer = "You've already invited the player!";

	public static string AddAsFriend = "Add as Friend";

	public static string Admin = "Admin";

	public static string Always = "Always";

	public static string AreYouSureYouWantToLeaveTheGame = "Are you sure you want to leave the game";

	public static string AudioCaps = "AUDIO";

	public static string BugType = "Bug Type";

	public static string ChangingToBlueTeam = "Changing to the Blue Team.";

	public static string ChangingToRedTeam = "Changing to the Red Team.";

	public static string Chat = "Chat";

	public static string ChatInClan = "CHAT IN CLAN";

	public static string ChatInLobby = "CHAT IN LOBBY";

	public static string ChatWith = "Chat with";

	public static string Cheaters = "Cheaters";

	public static string Clan = "Clan";

	public static string ClickToRespawn = "Click to Respawn!";

	public static string CongratulationsYouKilledYourself = "Congratulations. You killed yourself.";

	public static string Contacts = "Contacts";

	public static string Controls = "Controls";

	public static string ControlsCaps = "CONTROLS";

	public static string Crash = "Crash";

	public static string Details = "Details";

	public static string DisconnectionIn = "Disconnection in";

	public static string DontSpamTheLobbyChat = "Don't spam the Lobby chat!";

	public static string EnableRagdoll = "Enable Ragdoll";

	public static string EnterAMessageHere = "Enter a message here";

	public static string EnterAMessageInGameHere = "Enter a message in game here";

	public static string EnterAMessageToClanHere = "Enter a message to clan here";

	public static string EnterAMessageToLobbyHere = "Enter a message to lobby here";

	public static string EnteredTrainingMode = "entered Training Mode!";

	public static string FirstTime = "First Time";

	public static string Frequency = "Frequency";

	public static string Friends = "Friends";

	public static string Gameplay = "Gameplay";

	public static string Graphics = "Graphics";

	public static string HeadshotFromN = "Headshot from {0}.";

	public static string HowToReproduceIt = "How to reproduce it?";

	public static string InviteToClan = "Invite to Clan";

	public static string IsNotOnline = "is not online";

	public static string JoinedTheGame = "joined the game.";

	public static string JoinGame = "Join Game";

	public static string KilledByN = "killed by {0}.";

	public static string LeaveCaps = "LEAVE";

	public static string LeaveGameWarningMsg = "If you leave before the round ends you will lose all the XP and Points that you earned in this round!\nAre you sure you want to leave?";

	public static string LeavingGame = "Leaving Game";

	public static string LeftTheGame = "left the game";

	public static string Lobby = "Lobby";

	public static string MoreInfo = "More info...";

	public static string NKilledThemself = "suicided.";

	public static string NKillsLeft = "{0} Kills Left!";

	public static string NoMatchFound = "No Match Found";

	public static string NoPlayerSelected = "No Player Selected";

	public static string NoPlayersToReport = "No Players to Report";

	public static string NutshotFromN = "Nutshot from {0}.";

	public static string OneKillLeft = "One Kill Left!";

	public static string OnlyOneTeamChangePerLife = "Only one team change per life.";

	public static string OpenThisLinkInANewBrowserWindow = "Open this link in a new browser window";

	public static string Others = "Others";

	public static string PlayerNames = "Player Names";

	public static string Private = "Private";

	public static string Report = "Report";

	public static string ReportBugCaps = "REPORT BUG";

	public static string ReportBugSuccessMsg = "Your  bug has been successfully reported!";

	public static string ReportPlayerCaps = "REPORT PLAYER";

	public static string ReportPlayerErrorMsg = "Your report was not submitted because you are not connected to our servers";

	public static string ReportPlayerInfoMsg = "In this form you can report illegal activities detected while playing UberStrike.";

	public static string ReportPlayerSuccessMsg = "Your report was sent and will be reviewed!\n\nThank you for keeping our community clean.";

	public static string ReportPlayerWarningMsg = "Are you sure you want to report the player '{0}'?\n\nFalsifying reports is a punishable offense!";

	public static string ReportType = "Report Type";

	public static string SelectAPlayer = "Select a Player";

	public static string SelectType = "Select Type";

	public static string Send = "Send";

	public static string ServerDown = "Server Down";

	public static string ShopTutorialMsg01 = "Welcome to the Testing Area!";

	public static string ShopTutorialMsg02 = "Press 'ESC' or 'Backspace' at any time to exit.";

	public static string SmackdownFromN = "Smackdown from {0}.";

	public static string Sometimes = "Sometimes";

	public static string SysInfoCaps = "SYS INFO";

	public static string TheCommunicator = "The Communicator";

	public static string TrainingBtnTooltip = "Practice Uberstrike in single player mode.";

	public static string TrainingTutorialMsg01 = "Welcome to Basic Training!";

	public static string TrainingTutorialMsg02 = "Press {0} at any time to dismiss these messages.";

	public static string TrainingTutorialMsg03 = "Ok, let's get started...";

	public static string TrainingTutorialMsg04 = "You can use the mouse to look around.";

	public static string TrainingTutorialMsg05 = "Now, use the {0}{1}{2}{3} keys to move your character.";

	public static string TrainingTutorialMsg06 = "To shoot, use the {0}.";

	public static string TrainingTutorialMsg07 = "To select next weapon, use {0}.\nTo select previous weapon, use {1}.";

	public static string TrainingTutorialMsg08 = "You can also use\n the {0} {1} {2} {3} keys \n to directly select a weapon.";

	public static string TrainingTutorialMsg09 = "To crouch, hold down the {0} key.";

	public static string TrainingTutorialMsg10 = "Use {0} to enter full screen mode.";

	public static string TrainingTutorialMsg11 = "That's all you need to get started. Good luck!";

	public static string TrainingMobileMsg1 = "Swipe your finger to look around.";

	public static string TrainingMobileMsg2 = "You can change between multi-finger mode\n and two-thumb mode at any time.";

	public static string TrainingMobileMsg3 = "Tap the buttons on screen to control your character.";

	public static string TrainingMobileMsg4 = "In multi-finger mode, double tap to zoom your weapon.";

	public static string UnassignedKeyMappingsWarningMsg = "There are unassigned key mappings!\nPlease check your Control Settings.";

	public static string UserInterface = "User Interface";

	public static string VideoCaps = "VIDEO";

	public static string WaitingForOtherPlayers = "Waiting for other players...";

	public static string WhatHappened = "What happened?";

	public static string YouAreOnTheBlueTeam = "You are on the Blue Team.";

	public static string YouAreOnTheRedTeam = "You are on the Red Team.";

	public static string YouCannotChangeToATeamWithEqual = "You cannot change to a team\nwith equal or more players.";

	public static string YouKilledN = "you killed {0}.";

	public static string YouShotNInTheHead = "You shot {0} in the head.";

	public static string YouShotNInTheNuts = "You shot {0} in the nuts.";

	public static string YouSmackedDownN = "You smacked down {0}.";

	public static string General = "General";

	public static string Home = "Home";

	public static string HomeHelpDesc = "The Home Screen is where you are when you first launch UberStrike. From here you can join games, create games or chat with friends.";

	public static string Introduction = "Introduction";

	public static string IntroHelpDesc = "UberStrike is a 3D multiplayer first person shooter (FPS) game. This means that you are looking through the eyes of a character, and you are playing in a game world with other players.\n\nUberStrike is, first and foremost, an action game, where the objective is to eliminate as many players as possible using the weapons available to you.\n\nUberStrike is played in a fully 3D environment that you can enjoy with your friends. You can explore, fight, chat, and form clans with your friends in the game. As you play UberStrike, you earn points, which you can use to customize your character and buy new items.";

	public static string Items = "Items";

	public static string Play = "Play";

	public static string PlayHelpDesc = "The Play Screen is where you can join active games. It is home to the Game List, which is a detailed view of all UberStrike games that are currently being played. Here you can view current players in the game and see your ping time to the game (measured in ms), which is a determinant of network performance. Lower ping is better.";

	public static string Shop = "Shop";

	public static string ShopHelpDesc = "The shop is where you can buy items, including weapons and gear. The shop is divided into three parts: your Loadout, your Inventory, and the Shop.";

	public static string Profile = "Profile";

	public static string ProfileHelpDesc = "The Profile Screen is where you can view your personal performance in UberStrike and compare your performance to other players. Performance is divided into Personal Records, which has your best 'per-life' stats, and Weapon Stats, which shows information on how you’ve performed with each class of weapon.";

	public static string GunsNStuffCaps = "GUNS N STUFF";

	public static string MainMenuPlayTooltip = "Instantly connect to a game with other\nplayers of a similar level and kick ass!";

	public static string MainMenuQuitTooltip = "Quit Uberstrike.";

	public static string MainMenuShopTooltip = "Grab some weapons of mass destruction\nbefore facing off with your opponents.";

	public static string MainMenuTrainTooltip = "Learn how to play Uberstrike and avoid pwnage.\nTraining is single player.";

	public static string FindingAServerToJoin = "Finding a server to join...";

	public static string LostParadise2 = "Lost Paradise 2";

	public static string MonkeyIsland2 = "Monkey Island 2";

	public static string TemplOfTheRaven = "Temple of the Raven";

	public static string TheWarehouse = "The Warehouse";

	public static string Week = "Week";

	public static string Month = "Month";

	public static string ThreeMonths = "3 Months";

	public static string Off = "Off";

	public static string MIDescriptionMsg = "The last resting place of the feared pirate Captain Bradford Pegleg. This forgotten island is rumoured to be home to Bradford's Treasure and was once a place of worship for the tribe of the Monkey King.";

	public static string LPDescriptionMsg = "A group of islands located just off the coast of Costa Rica, the Paradise Islands are a great hunting ground for privateers with a keen eye for sniping. Two towering volcanic pillars provide the perfect lookout for those able to make it there in one piece.";

	public static string TWDescriptionMsg = "Used by RAID Corporation for the storage of weapons and other experimental technology, the Warehouse is a fast paced arena for short range fire fights. Only for privateers who can handle the adrenaline hit.";

	public static string TORDescriptionMsg = "The mighty ravens watch over the temple as privateers try their luck at unlocking the secrets to the inner sanctum. Great for mid range gameplay.";

	public static string GTDescriptionMsg = "Brave the heights of Gideons Tower, situated in the heart of Ubercity One. Beware, for a fall from its narrow ledges means certain death for any privateer.";

	public static string FWDescriptionMsg = "Created by: Team Cmune\nFort Winter is a small, intense map designed for furious team elimination gameplay. Best played with 6 players per team.";

	public static string SGDescriptionMsg = "Created by: Team Cmune\nSky Garden is a wide open map that forces players to tread carefully and take cannon shots with care lest they fall off the edge of the map.";

	public static string DMModeDescriptionMsg = "Shoot anything that moves. The player with the most kills at the end of the round wins.";

	public static string TDMModeDescriptionMsg = "Shoot anyone not on your team. The team with the most kills at the end of the round wins.";

	public static string ELMModeDescriptionMsg = "Eliminate all players on the enemy team. The team with players standing at the end of the round wins.";

	public static string Effects = "Effects";

	public static string ScreenResolution = "Screen Resolution";

	public static string MaxRounds = "Max Rounds";

	public static string MaxKills = "Max Kills";

	public static string NewItem = "New Item";

	public static string NotNow = "Not now";

	public static string Congratulations = "Congratulations";

	public static string YouHaveBeenGrantedNItems = "You have been granted {0} free items! Go to your inventory to equip them!";

	public static string NoItems = "There are no items";

	public static string NoDescriptionAvailable = "No description available.";

	public static string Default = "Default";

	public static string EliminateAllYourEnemies = "Eliminate all your enemies!";

	public static string FinalRoundCaps = "FINAL ROUND";

	public static string FinalRoundX = "Final Round {0}";

	public static string NRoundsLeft = "{0} Rounds Left!";

	public static string RedCaps = "RED";

	public static string BlueCaps = "BLUE";

	public static string Following = "Following";

	public static string Nobody = "Nobody";

	public static string SpectatorMode = "Spectator Mode";

	public static string YouAreNotLoggedIn = "You are not logged in!";

	public static string UpdateAvailable = "Update Available";

	public static string UberStrikeIsOutOfDatePleaseRefreshPage = "UberStrike v{0} is out of date.\nRefresh your browser window to update to v{1}!";

	public static string UberStrikeIsOutOfDatePleaseDownloadClient = "UberStrike v{0} out of date, latest is v{1}.\nDownload the new client from:\n{2}";

	public static string UberStrikeIsOutOfDateVisitWebsite = "UberStrike v{0} is out of date.\nVisit https://discord.gg/hhxZCBamRT to get v{1}!";

	public static string UberStrikeIsOutOfDateUpdateMacApp = "UberStrike v{0} is out of date, latest version is v{1}.\nVisit https://discord.gg/hhxZCBamRT to get the latest version.";

	public static string ConnectionProblem = "Connection Problem";

	public static string ErrorInternetConnection = "No active internet connection detected.\nAn internet connection is required to play UberStrike.\nPlease check your connection and try again.";

	public static string ErrorWebservices = "There seems to be a problem with our service. Please restart Uberstrike and try to sign in again. Please try again later or contact us at https://discord.gg/hhxZCBamRT";

	public static string ErrorReadingConfiguration = "Error Reading Configuration";

	public static string ThereWasProblemRetrievingWebplayerConfiguration = "There was a problem retrieving the webplayer configuration. Please try again later or contact us at https://discord.gg/hhxZCBamRT";

	public static string RefreshWallet = "Refresh Wallet";

	public static string IfYouPurchasedCreditsClickOKToRefreshYourWallet = "If you purchased credits, click OK to refresh your wallet and inventory.";

	public static string ClanInvite = "Clan Invite";

	public static string YouAlreadyInClanMsg = "You are already in a clan. You cannot join\nanother clan until you leave your current clan.";

	public static string Hits = "Hits";

	public static string GideonsTower = "Gideons Tower";

	public static string SkyGarden = "SkyGarden";

	public static string FortWinter = "FortWinter";

	public static string AqualabResearchHub = "Aqualab Research Hub";

	public static string CuberStrike = "CuberStrike";

	public static string TuberStrike = "TuberStrike";

	public static string AccountHistory = "Account History";

	public static string RecommendedLoadoutCaps = "RECOMMENDED LOADOUT";

	public static string MostEfficientWeaponCaps = "MOST EFFICIENT WEAPON";

	public static string RecommendedArmorCaps = "RECOMMENDED ARMOR";

	public static string StaffPickCaps = "STAFF PICK";

	public static string RecommendedWeaponCaps = "RECOMMENDED WEAPON";

	public static string UseMultiTouchInput = "Use Multi-touch input";

	public static string LookSensitivity = "Look Sensitivity";

	public static string JoystickSensitivity = "Joystick Sensitivity";

	public static string TouchInput = "Touch Input";

	public static string ControlStyle = "Control Style";

	public static string PackOneAmount = "10 Uses";

	public static string PackTwoAmount = "100 Uses";

	public static string PackThreeAmount = "1000 Uses";

	public static string DiscountPercentOff = "{0}% Off!";

	public static string NonMobileServer = "Warning! The server you are joining has players from the desktop version of UberStrike. You may get owned.";

	public static string MobileGameMoreThan6Players = "Warning! UberStrike on mobile is optimized for games with 6 players or less. Your game may run slowly.";

	public static string MessageQuickItemsTry = "QuickItems will not be consumed in this training mode";

	public static string TapToRespawn = "Tap to Respawn!";

	public static string TooltipForgotPassword = "Did you forget your password?\nFear not, we can resend it via email!";

	public static string CreateNewAccount = "Create a brand new account.";

	public static string TooltipFacebookAccount = "If you already play UberStrike on Facebook,\nget your email and password set up.";

	public static string ErrorEmailIsEmpty = "The email address or password you are trying to use is empty.";

	public static string ErrorProblemLoadingUberStrike = "There was a problem loading UberStrike. Please check your internet connection and try again.";

	public static string LoadingFriendsList = "Loading Friends List";

	public static string LoadingCharacterData = "Loading Character Data";

	public static string ErrorLoadingData = "There was an error loading player level data.";

	public static string LoadingNewsFeed = "Loading News Feeds";

	public static string ErrorLoadingNewsFeed = "There was an error getting the latest news.";

	public static string LoadingMapData = "Loading Map Data";

	public static string ErrorLoadingMaps = "There was an error loading the maps.";

	public static string ErrorLoadingMapsSupport = "There was an error getting the Maps.\nPlease try again later or contact us at https://discord.gg/hhxZCBamRT";

	public static string LoadingWeaponAndGear = "Loading Weapons and Gear";

	public static string ErrorGettingShopData = "Error Getting Shop Data";

	public static string ErrorGettingShopDataSupport = "There was an error getting the Shop Data.\nPlease try again later or contact us at https://discord.gg/hhxZCBamRT";

	public static string LoadingPlayerInventory = "Loading Player Inventory";

	public static string ErrorLoadingPlayerInventory = "It looks like you're trying to login with an old account.\nPlease try again later or contact us at https://discord.gg/hhxZCBamRT";

	public static string GettingPlayerLoadout = "Getting Player Loadout";

	public static string ErrorGettingPlayerLoadout = "Error Getting Player Loadout";

	public static string ErrorGettingPlayerLoadoutSupport = "There was an error getting the Player Loadout.\nPlease try again later or contact us at https://discord.gg/hhxZCBamRT";

	public static string LoadingPlayerStatistics = "Loading Player Statistics";

	public static string ErrorGettingPlayerStatistics = "Error Getting Player Statistics";

	public static string ErrorPlayerStatisticsSupport = "There was an error getting the Player Statistics.\nPlease try again later or contact us at https://discord.gg/hhxZCBamRT";

	public static string LoadingClanData = "Loading Clan Data";

	public static string ClaimYourDailyLuck = "CLAIM YOUR DAILY LUCK";

	public static string LuckyDrawHelpText = "Try your luck at winning one of the prizes above!\nBe careful not to play for items you already own permanently!";

	public static string LuckyDrawWinningsInInventory = "You find your winnings in the inventory!";

	public static string PlayAgainCaps = "PLAY AGAIN";

	public static string DoneCaps = "DONE";

	public static string Ammo = "Ammo";

	public static string Radius = "Radius";

	public static string ArmorCarried = "Armor Carried";

	public static string NDaysLeft = " {0} Days left";

	public static string LevelRequired = "Level Required: ";

	public static string CriticalHitBonus = "Critical Hit Bonus: ";

	public static string Instant = "instant";

	public static string Unlimited = "unlimited";

	public static string HealthColon = "Health: ";

	public static string AmmoColon = "Ammo: ";

	public static string ArmorColon = "Armor: ";

	public static string DamageColon = "Damage: ";

	public static string RadiusColon = "Radius: ";

	public static string ForceColon = "Force: ";

	public static string LifetimeColon = "Lifetime: ";

	public static string WarmupColon = "Warm-up: ";

	public static string CooldownColon = "Cooldown: ";

	public static string UsesPerLifeColon = "Uses per Life: ";

	public static string UsesPerGameColon = "Uses per Game: ";

	public static string TimeColon = "Time: ";

	public static string Help = "Help";

	public static string Otions = "Options";

	public static string Audio = "Audio";

	public static string Windowed = "Windowed";

	public static string FullscreenOnly = "fullscreen only";

	public static string Custom = "Custom";

	public static string ChangingScreenResolution = "Changing Screen Resolution...";

	public static string ChooseNewResolution = "Do you want to choose new resolution: ";

	public static string Auto = "Auto";

	public static string TargetFramerate = "Target Framerate:";

	public static string MaxQueuedFrames = "Max Queued Frames:";

	public static string SettingsTakeEffectAfterReloading = "This setting will take effect after reloading";

	public static string TextureQuality = "Texture Quality:";

	public static string VSync = "VSync:";

	public static string AntiAliasing = "Anti Aliasing:";

	public static string Options = "Options";

	public static string MysteryBox = "Mystery Box";

	public static string Packs = "Packs";

	public static string TransferCaps = "TRANSFER";

	public static string PromoteCaps = "PROMOTE";

	public static string DemoteCaps = "DEMOTE";

	public static string DisbandCaps = "DISBAND";

	public static string YourClan = "Your Clan";

	public static string ExploreMaps = "Explore Maps";

	public static string MobileGameMoreThan8Players = "Warning! UberStrike on iOS is optimized for games with 8 players or less. Your game may run slowly.";

	public static string HereYouCanCreateYourOwnClanFacebook = "Here you can create your own clan. This will create a group for your clan in Facebook";

	public static string TOTAL = "TOTAL";

	public static string Boost = "Boost";

	public static string SkillBonus = "Skill Bonus";

	public static string SharePhotoFacebook = "Press {0} to share screenshots";

	public static string SharePhotoIPad = "Share your avatar to Facebook";

	public static string ScreenshotTaken = "Added to 'UberStrike Photos' album";

	public static void UpdateLocalization(LocaleType type)
	{
		TextAsset textAsset = Resources.Load("strings." + type.ToString(), typeof(TextAsset)) as TextAsset;
		if (!(textAsset != null))
		{
			return;
		}
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.IgnoreComments = true;
		xmlReaderSettings.IgnoreWhitespace = true;
		XmlReaderSettings settings = xmlReaderSettings;
		Dictionary<string, string> allStrings = GetAllStrings(XmlReader.Create(new StringReader(textAsset.text), settings));
		FieldInfo[] fields = typeof(LocalizedStrings).GetFields(BindingFlags.Static | BindingFlags.Public);
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			if (allStrings.TryGetValue(fieldInfo.Name, out string value))
			{
				fieldInfo.SetValue(null, value);
			}
		}
	}

	private static Dictionary<string, string> GetAllStrings(XmlReader reader)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (reader != null)
		{
			try
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("data"))
					{
						string attribute = reader.GetAttribute("name");
						string value = string.Empty;
						if (reader.Read() && reader.Read())
						{
							value = reader.ReadString();
						}
						dictionary[attribute] = value;
					}
				}
				return dictionary;
			}
			finally
			{
				reader.Close();
			}
		}
		return dictionary;
	}
}
