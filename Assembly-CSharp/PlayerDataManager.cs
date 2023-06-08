using Cmune.DataCenter.Common.Entities;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Core.ViewModel;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.WebService.Unity;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
	private PlayerStatisticsView _serverLocalPlayerPlayerStatisticsView;

	private Dictionary<int, PublicProfileView> _friends = new Dictionary<int, PublicProfileView>();

	private Dictionary<int, PublicProfileView> _facebookFriends = new Dictionary<int, PublicProfileView>();

	private Dictionary<int, ClanMemberView> _clanMembers = new Dictionary<int, ClanMemberView>();

	private Color _localPlayerSkinColor = Color.white;

	private ClanView _playerClanData;

	private float _updateLoadoutTime;

	public static string MagicHash
	{
		get;
		set;
	}

	public static bool IsLoadoutUpdating = false;
	public static MemberOperationResult result;

	public static int LoginStreak
	{
		get;
		set;
	}

	public static bool IsTestBuild => false;

	public float GearWeight => Mathf.Clamp01((float)((int)GameState.Current.PlayerData.ArmorCarried / 2 + 40) / 100f);

	public int FriendsCount => _friends.Count + _facebookFriends.Count;

	public static string SteamId => SteamUser.GetSteamID().ToString();

	public PlayerStatisticsView ServerLocalPlayerStatisticsView => _serverLocalPlayerPlayerStatisticsView;

	public static Color SkinColor => Singleton<PlayerDataManager>.Instance._localPlayerSkinColor;

	public IEnumerable<PublicProfileView> FriendList
	{
		get
		{
			return _friends.Values;
		}
		set
		{
			_friends.Clear();
			if (value != null)
			{
				foreach (PublicProfileView item in value)
				{
					_friends[item.Cmid] = item;
				}
			}
		}
	}

	public IEnumerable<PublicProfileView> FacebookFriends
	{
		get
		{
			return _facebookFriends.Values;
		}
		set
		{
			_facebookFriends.Clear();
			if (value != null)
			{
				foreach (PublicProfileView item in value)
				{
					if (!_friends.ContainsKey(item.Cmid))
					{
						_facebookFriends[item.Cmid] = item;
					}
				}
			}
		}
	}

	public List<PublicProfileView> MergedFriends
	{
		get
		{
			List<PublicProfileView> list = new List<PublicProfileView>(FriendList);
			list.AddRange(FacebookFriends);
			return list;
		}
	}

	public static bool IsPlayerLoggedIn => Cmid > 0;

	public static MemberAccessLevel AccessLevel
	{
		get;
		private set;
	}

	public static int Cmid
	{
		get;
		private set;
	}

	public static string Name
	{
		get;
		set;
	}

	public static string Email
	{
		get;
		private set;
	}

	public static int Credits
	{
		get;
		private set;
	}

	public static int Points
	{
		get;
		set;
	}

	public static int PlayerExperience
	{
		get;
		private set;
	}

	public static int PlayerLevel
	{
		get;
		private set;
	}

	public static string AuthToken
	{
		get;
		set;
	}

	public static ClanView ClanData
	{
		set
		{
			Singleton<PlayerDataManager>.Instance._playerClanData = value;
			Singleton<PlayerDataManager>.Instance._clanMembers.Clear();
			if (value != null)
			{
				ClanID = value.GroupId;
				if (value.Members != null)
				{
					foreach (ClanMemberView member in value.Members)
					{
						Singleton<PlayerDataManager>.Instance._clanMembers[member.Cmid] = member;
						if (member.Cmid == Cmid)
						{
							Singleton<PlayerDataManager>.Instance.RankInClan = member.Position;
						}
					}
				}
			}
			else
			{
				ClanID = 0;
				Singleton<PlayerDataManager>.Instance._clanMembers.Clear();
				Singleton<PlayerDataManager>.Instance.RankInClan = GroupPosition.Member;
			}
		}
	}

	public static bool IsPlayerInClan => ClanID > 0;

	public static int ClanID
	{
		get;
		set;
	}

	public GroupPosition RankInClan
	{
		get;
		set;
	}

	public static string ClanName
	{
		get
		{
			if (Singleton<PlayerDataManager>.Instance._playerClanData != null)
			{
				return Singleton<PlayerDataManager>.Instance._playerClanData.Name;
			}
			return string.Empty;
		}
	}

	public static string ClanTag
	{
		get
		{
			if (Singleton<PlayerDataManager>.Instance._playerClanData != null)
			{
				return Singleton<PlayerDataManager>.Instance._playerClanData.Tag;
			}
			return string.Empty;
		}
	}

	public static string ClanMotto
	{
		get
		{
			if (Singleton<PlayerDataManager>.Instance._playerClanData != null)
			{
				return Singleton<PlayerDataManager>.Instance._playerClanData.Motto;
			}
			return string.Empty;
		}
	}

	public static DateTime ClanFoundingDate
	{
		get
		{
			if (Singleton<PlayerDataManager>.Instance._playerClanData != null)
			{
				return Singleton<PlayerDataManager>.Instance._playerClanData.FoundingDate;
			}
			return DateTime.Now;
		}
	}

	public static string ClanOwnerName
	{
		get
		{
			if (Singleton<PlayerDataManager>.Instance._playerClanData != null)
			{
				return Singleton<PlayerDataManager>.Instance._playerClanData.OwnerName;
			}
			return string.Empty;
		}
	}

	public static int ClanMembersLimit
	{
		get
		{
			if (Singleton<PlayerDataManager>.Instance._playerClanData == null)
			{
				return 0;
			}
			return Singleton<PlayerDataManager>.Instance._playerClanData.MembersLimit;
		}
	}

	public int ClanMembersCount
	{
		get
		{
			if (_playerClanData == null)
			{
				return 0;
			}
			return _playerClanData.Members.Count;
		}
	}

	public List<ClanMemberView> ClanMembers
	{
		get
		{
			if (_playerClanData != null)
			{
				return _playerClanData.Members;
			}
			return new List<ClanMemberView>(0);
		}
	}

	public static bool CanInviteToClan
	{
		get
		{
			if (Singleton<PlayerDataManager>.Instance.RankInClan != 0)
			{
				return Singleton<PlayerDataManager>.Instance.RankInClan == GroupPosition.Officer;
			}
			return true;
		}
	}

	public static string NameAndTag
	{
		get
		{
			if (IsPlayerInClan)
			{
				return "[" + ClanTag + "] " + Name;
			}
			return Name;
		}
	}

	private PlayerDataManager()
	{
		_serverLocalPlayerPlayerStatisticsView = new PlayerStatisticsView();
		_playerClanData = new ClanView();
	}

	public void AddFriend(PublicProfileView view)
	{
		_friends[view.Cmid] = view;
	}

	public void RemoveFriend(int friendCmid)
	{
		_friends.Remove(friendCmid);
	}

	public void SetLocalPlayerMemberView(MemberView memberView)
	{
		Cmid = memberView.PublicProfile.Cmid;
		AccessLevel = memberView.PublicProfile.AccessLevel;
		Name = memberView.PublicProfile.Name;
		Points = memberView.MemberWallet.Points;
		Credits = memberView.MemberWallet.Credits;
	}

	public void SetPlayerStatisticsView(PlayerStatisticsView value)
	{
		if (value != null)
		{
			_serverLocalPlayerPlayerStatisticsView = value;
			PlayerExperience = value.Xp;
			PlayerLevel = XpPointsUtil.GetLevelForXp(value.Xp);
		}
	}

	public void UpdatePlayerStats(StatsCollection stats, StatsCollection best)
	{
		PlayerStatisticsView serverLocalPlayerStatisticsView = ServerLocalPlayerStatisticsView;
		int xp = serverLocalPlayerStatisticsView.Xp + GameState.Current.Statistics.GainedXp;
		int levelForXp = XpPointsUtil.GetLevelForXp(xp);
		SetPlayerStatisticsView(new PlayerStatisticsView(serverLocalPlayerStatisticsView.Cmid, serverLocalPlayerStatisticsView.Splats + stats.GetKills(), serverLocalPlayerStatisticsView.Splatted + stats.Deaths, serverLocalPlayerStatisticsView.Shots + stats.GetShots(), serverLocalPlayerStatisticsView.Hits + stats.GetHits(), serverLocalPlayerStatisticsView.Headshots + stats.Headshots, serverLocalPlayerStatisticsView.Nutshots + stats.Nutshots, xp, levelForXp, new PlayerPersonalRecordStatisticsView((serverLocalPlayerStatisticsView.PersonalRecord.MostHeadshots <= best.Headshots) ? best.Headshots : serverLocalPlayerStatisticsView.PersonalRecord.MostHeadshots, (serverLocalPlayerStatisticsView.PersonalRecord.MostNutshots <= best.Nutshots) ? best.Nutshots : serverLocalPlayerStatisticsView.PersonalRecord.MostNutshots, (serverLocalPlayerStatisticsView.PersonalRecord.MostConsecutiveSnipes <= best.ConsecutiveSnipes) ? best.ConsecutiveSnipes : serverLocalPlayerStatisticsView.PersonalRecord.MostConsecutiveSnipes, 0, (serverLocalPlayerStatisticsView.PersonalRecord.MostSplats <= best.GetKills()) ? best.GetKills() : serverLocalPlayerStatisticsView.PersonalRecord.MostSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostDamageDealt <= best.GetDamageDealt()) ? best.GetDamageDealt() : serverLocalPlayerStatisticsView.PersonalRecord.MostDamageDealt, (serverLocalPlayerStatisticsView.PersonalRecord.MostDamageReceived <= best.DamageReceived) ? best.DamageReceived : serverLocalPlayerStatisticsView.PersonalRecord.MostDamageReceived, (serverLocalPlayerStatisticsView.PersonalRecord.MostArmorPickedUp <= best.ArmorPickedUp) ? best.ArmorPickedUp : serverLocalPlayerStatisticsView.PersonalRecord.MostArmorPickedUp, (serverLocalPlayerStatisticsView.PersonalRecord.MostHealthPickedUp <= best.HealthPickedUp) ? best.HealthPickedUp : serverLocalPlayerStatisticsView.PersonalRecord.MostHealthPickedUp, (serverLocalPlayerStatisticsView.PersonalRecord.MostMeleeSplats <= best.MeleeKills) ? best.MeleeKills : serverLocalPlayerStatisticsView.PersonalRecord.MostMeleeSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostMachinegunSplats <= best.MachineGunKills) ? best.MachineGunKills : serverLocalPlayerStatisticsView.PersonalRecord.MostMachinegunSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostShotgunSplats <= best.ShotgunSplats) ? best.ShotgunSplats : serverLocalPlayerStatisticsView.PersonalRecord.MostShotgunSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostSniperSplats <= best.SniperKills) ? best.SniperKills : serverLocalPlayerStatisticsView.PersonalRecord.MostSniperSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostSplattergunSplats <= best.SplattergunKills) ? best.SplattergunKills : serverLocalPlayerStatisticsView.PersonalRecord.MostSplattergunSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostCannonSplats <= best.CannonKills) ? best.CannonKills : serverLocalPlayerStatisticsView.PersonalRecord.MostCannonSplats, (serverLocalPlayerStatisticsView.PersonalRecord.MostLauncherSplats <= best.LauncherKills) ? best.LauncherKills : serverLocalPlayerStatisticsView.PersonalRecord.MostLauncherSplats), new PlayerWeaponStatisticsView(serverLocalPlayerStatisticsView.WeaponStatistics.MeleeTotalSplats + stats.MeleeKills, serverLocalPlayerStatisticsView.WeaponStatistics.MachineGunTotalSplats + stats.MachineGunKills, serverLocalPlayerStatisticsView.WeaponStatistics.ShotgunTotalSplats + stats.ShotgunSplats, serverLocalPlayerStatisticsView.WeaponStatistics.SniperTotalSplats + stats.SniperKills, serverLocalPlayerStatisticsView.WeaponStatistics.SplattergunTotalSplats + stats.SplattergunKills, serverLocalPlayerStatisticsView.WeaponStatistics.CannonTotalSplats + stats.CannonKills, serverLocalPlayerStatisticsView.WeaponStatistics.LauncherTotalSplats + stats.LauncherKills, serverLocalPlayerStatisticsView.WeaponStatistics.MeleeTotalShotsFired + stats.MeleeShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.MeleeTotalShotsHit + stats.MeleeShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.MeleeTotalDamageDone + stats.MeleeDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.MachineGunTotalShotsFired + stats.MachineGunShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.MachineGunTotalShotsHit + stats.MachineGunShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.MachineGunTotalDamageDone + stats.MachineGunDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.ShotgunTotalShotsFired + stats.ShotgunShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.ShotgunTotalShotsHit + stats.ShotgunShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.ShotgunTotalDamageDone + stats.ShotgunDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.SniperTotalShotsFired + stats.SniperShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.SniperTotalShotsHit + stats.SniperShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.SniperTotalDamageDone + stats.SniperDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.SplattergunTotalShotsFired + stats.SplattergunShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.SplattergunTotalShotsHit + stats.SplattergunShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.SplattergunTotalDamageDone + stats.SplattergunDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.CannonTotalShotsFired + stats.CannonShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.CannonTotalShotsHit + stats.CannonShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.CannonTotalDamageDone + stats.CannonDamageDone, serverLocalPlayerStatisticsView.WeaponStatistics.LauncherTotalShotsFired + stats.LauncherShotsFired, serverLocalPlayerStatisticsView.WeaponStatistics.LauncherTotalShotsHit + stats.LauncherShotsHit, serverLocalPlayerStatisticsView.WeaponStatistics.LauncherTotalDamageDone + stats.LauncherDamageDone)));
	}

	private void HandleWebServiceError()
	{
	}

	public void SetSkinColor(Color skinColor)
	{
		_localPlayerSkinColor = skinColor;
	}

	private LoadoutView CreateLocalPlayerLoadoutView()
	{
		return new LoadoutView(0, 0, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearBoots), Cmid, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearFace), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem1), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem2), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem3), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearGloves), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearHead), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearLowerBody), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponMelee), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem1), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem2), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem3), AvatarType.LutzRavinoff, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearUpperBody), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponPrimary), 0, 0, 0, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponSecondary), 0, 0, 0, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponTertiary), 0, 0, 0, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearHolo), ColorConverter.ColorToHex(_localPlayerSkinColor));
	}

	public IEnumerator StartGetMemberWallet()
	{
		if (Cmid < 1)
		{
			Debug.LogError("Player CMID is invalid! Have you called AuthenticationManager.StartAuthenticateMember?");
			ApplicationDataManager.LockApplication("The authentication process failed. Please try again later or contact us at https://discord.gg/hhxZCBamRT");
			yield break;
		}
		IPopupDialog popupDialog = PopupSystem.ShowMessage("Updating", "Updating your points and credits balance...", PopupSystem.AlertType.None);
		yield return UserWebServiceClient.GetMemberWallet(AuthToken, OnGetMemberWalletEventReturn, delegate
		{
		});
		yield return new WaitForSeconds(0.5f);
		PopupSystem.HideMessage(popupDialog);
	}

	public IEnumerator StartSetLoadout()
	{
		if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.IsConnected)
		{
			var popup = new ProgressPopupDialog("Authentication", "Connecting to Server & Syncing Loadout");
			PopupSystem.Show(popup);
			IsLoadoutUpdating = true;
			AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Operations.SendUpdateLoadout(AuthToken, CreateLocalPlayerLoadoutView());
			while (IsLoadoutUpdating)
			{
				yield return new WaitForEndOfFrame();
			}
			yield return StartGetLoadout();
			if (result != MemberOperationResult.Ok)
			{
				PopupSystem.HideMessage(popup);
				PopupSystem.ShowMessage(null, $"Error updating your loadout {result}");
			}
			else
			{
				PopupSystem.HideMessage(popup);
			}
			Singleton<GameStateController>.Instance.Client.Operations.SendUpdateLoadout();
		}
		else { yield return StartGetLoadout(); ApplicationDataManager.LockApplication("Client and server data out of sync, please restart"); }
	}

	public IEnumerator StartGetLoadout()
	{
		if (!Singleton<ItemManager>.Instance.ValidateItemMall())
		{
			PopupSystem.ShowMessage("Error Getting Shop Data", "The shop is empty, perhaps there\nwas an error getting the Shop data?", PopupSystem.AlertType.OK, HandleWebServiceError);
		}
		else
		{
			yield return UserWebServiceClient.GetLoadout(AuthToken, delegate(LoadoutView ev)
			{
				if (ev != null)
				{
					CheckLoadoutForExpiredItems(ev);
					Singleton<LoadoutManager>.Instance.UpdateLoadout(ev);
					GameState.Current.Avatar.SetLoadout(new Loadout(Singleton<LoadoutManager>.Instance.Loadout));
					_localPlayerSkinColor = ColorConverter.HexToColor(ev.SkinColor);
				}
				else
				{
					ApplicationDataManager.LockApplication("It seems that you account is corrupted. Please try again later or contact us at https://discord.gg/hhxZCBamRT");
				}
			}, delegate
			{
				ApplicationDataManager.LockApplication("There was an error getting your loadout.");
			});
		}
	}

	public IEnumerator StartGetMember()
	{
		yield return UserWebServiceClient.GetMember(AuthToken, OnGetMemberEventReturn, delegate
		{
			ApplicationDataManager.LockApplication("There was an error getting your player data.");
		});
	}

	private void OnGetMemberWalletEventReturn(MemberWalletView ev)
	{
		NotifyPointsAndCreditsChanges(ev.Points, ev.Credits);
		UpdateSecurePointsAndCredits(ev.Points, ev.Credits);
	}

	private void OnGetMemberEventReturn(UberstrikeUserViewModel ev)
	{
		NotifyPointsAndCreditsChanges(ev.CmuneMemberView.MemberWallet.Points, ev.CmuneMemberView.MemberWallet.Credits);
		SetPlayerStatisticsView(ev.UberstrikeMemberView.PlayerStatisticsView);
		SetLocalPlayerMemberView(ev.CmuneMemberView);
	}

	private void NotifyPointsAndCreditsChanges(int newPoints, int newCredits)
	{
		if (newPoints != Points)
		{
			GlobalUIRibbon.Instance.AddPointsEvent(newPoints - Points);
		}
		if (newCredits != Credits)
		{
			GlobalUIRibbon.Instance.AddCreditsEvent(newCredits - Credits);
		}
	}

	public bool ValidateMemberData()
	{
		if (Cmid > 0)
		{
			return _serverLocalPlayerPlayerStatisticsView.Cmid > 0;
		}
		return false;
	}

	public void AttributeXp(int xp)
	{
		PlayerExperience += xp;
		PlayerLevel = XpPointsUtil.GetLevelForXp(PlayerExperience);
		_serverLocalPlayerPlayerStatisticsView.Xp = PlayerExperience;
		_serverLocalPlayerPlayerStatisticsView.Level = PlayerLevel;
	}

	public void UpdateSecurePointsAndCredits(int points, int credits)
	{
		Points = points;
		Credits = credits;
	}

	public void CheckLoadoutForExpiredItems(LoadoutView view)
	{
		view = new LoadoutView(view.LoadoutId, (!IsExpired(view.Backpack, "Backpack")) ? view.Backpack : 0, (!IsExpired(view.Boots, "Boots")) ? view.Boots : 0, view.Cmid, (!IsExpired(view.Face, "Face")) ? view.Face : 0, (!IsExpired(view.FunctionalItem1, "FunctionalItem1")) ? view.FunctionalItem1 : 0, (!IsExpired(view.FunctionalItem2, "FunctionalItem2")) ? view.FunctionalItem2 : 0, (!IsExpired(view.FunctionalItem3, "FunctionalItem3")) ? view.FunctionalItem3 : 0, (!IsExpired(view.Gloves, "Gloves")) ? view.Gloves : 0, (!IsExpired(view.Head, "Head")) ? view.Head : 0, (!IsExpired(view.LowerBody, "LowerBody")) ? view.LowerBody : 0, (!IsExpired(view.MeleeWeapon, "MeleeWeapon")) ? view.MeleeWeapon : 0, (!IsExpired(view.QuickItem1, "QuickItem1")) ? view.QuickItem1 : 0, (!IsExpired(view.QuickItem2, "QuickItem2")) ? view.QuickItem2 : 0, (!IsExpired(view.QuickItem3, "QuickItem3")) ? view.QuickItem3 : 0, view.Type, (!IsExpired(view.UpperBody, "UpperBody")) ? view.UpperBody : 0, (!IsExpired(view.Weapon1, "Weapon1")) ? view.Weapon1 : 0, (!IsExpired(view.Weapon1Mod1, "Weapon1Mod1")) ? view.Weapon1Mod1 : 0, (!IsExpired(view.Weapon1Mod2, "Weapon1Mod2")) ? view.Weapon1Mod2 : 0, (!IsExpired(view.Weapon1Mod3, "Weapon1Mod3")) ? view.Weapon1Mod3 : 0, (!IsExpired(view.Weapon2, "Weapon2")) ? view.Weapon2 : 0, (!IsExpired(view.Weapon2Mod1, "Weapon2Mod1")) ? view.Weapon2Mod1 : 0, (!IsExpired(view.Weapon2Mod2, "Weapon2Mod2")) ? view.Weapon2Mod2 : 0, (!IsExpired(view.Weapon2Mod3, "Weapon2Mod3")) ? view.Weapon2Mod3 : 0, (!IsExpired(view.Weapon3, "Weapon3")) ? view.Weapon3 : 0, (!IsExpired(view.Weapon3Mod1, "Weapon3Mod1")) ? view.Weapon3Mod1 : 0, (!IsExpired(view.Weapon3Mod2, "Weapon3Mod2")) ? view.Weapon3Mod2 : 0, (!IsExpired(view.Weapon3Mod3, "Weapon3Mod3")) ? view.Weapon3Mod3 : 0, (!IsExpired(view.Webbing, "Webbing")) ? view.Webbing : 0, view.SkinColor);
	}

	private bool IsExpired(int itemId, string debug)
	{
		return !Singleton<InventoryManager>.Instance.Contains(itemId);
	}

	public static bool IsClanMember(int cmid)
	{
		return Singleton<PlayerDataManager>.Instance._clanMembers.ContainsKey(cmid);
	}

	public static bool IsFriend(int cmid)
	{
		return Singleton<PlayerDataManager>.Instance._friends.ContainsKey(cmid);
	}

	public static bool IsFacebookFriend(int cmid)
	{
		return Singleton<PlayerDataManager>.Instance._facebookFriends.ContainsKey(cmid);
	}

	public static bool TryGetFriend(int cmid, out PublicProfileView view)
	{
		if (Singleton<PlayerDataManager>.Instance._friends.TryGetValue(cmid, out view))
		{
			return view != null;
		}
		return false;
	}

	public static bool TryGetClanMember(int cmid, out ClanMemberView view)
	{
		if (Singleton<PlayerDataManager>.Instance._clanMembers.TryGetValue(cmid, out view))
		{
			return view != null;
		}
		return false;
	}
}
