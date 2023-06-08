using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.Text;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class ChatManager : Singleton<ChatManager>
{
	private List<CommUser> _lobbyUsers;

	private List<CommUser> _otherUsers;

	private List<CommUser> _friendUsers;

	public Dictionary<int, CommUser> _naughtyUsers;

	private Dictionary<int, CommUser> _clanUsers;

	private List<CommUser> _ingameUsers;

	private List<CommUser> _lastgameUsers;

	private Dictionary<int, CommUser> _allTimePlayers;

	private HashSet<TabArea> _tabAreas;

	private float _nextRefreshTime;

	public int SelectedCmid;

	public ChatDialog SelectedDialog;

	public Property<bool> HasUnreadPrivateMessage = new Property<bool>(defaultValue: false);

	public Property<bool> HasUnreadClanMessage = new Property<bool>(defaultValue: false);

	public ChatGroupPanel[] _commPanes;

	public Dictionary<int, ChatDialog> _dialogsByCmid;

	public HashSet<int> _mutedCmids;

	public int TotalContacts => _friendUsers.Count + _otherUsers.Count + _clanUsers.Count;

	public ChatDialog ClanDialog
	{
		get;
		private set;
	}

	public ChatDialog LobbyDialog
	{
		get;
		private set;
	}

	public ChatDialog InGameDialog
	{
		get;
		private set;
	}

	public ChatDialog ModerationDialog
	{
		get;
		private set;
	}

	public ICollection<CommUser> OtherUsers => _otherUsers;

	public ICollection<CommUser> FriendUsers => _friendUsers;

	public ICollection<CommUser> LobbyUsers => _lobbyUsers;

	public ICollection<CommUser> ClanUsers => _clanUsers.Values;

	public ICollection<CommUser> NaughtyUsers => _naughtyUsers.Values;

	public ICollection<CommUser> GameUsers => _ingameUsers;

	public ICollection<CommUser> GameHistoryUsers => _lastgameUsers;

	public int TabCounter => _tabAreas.Count + (ShowTab(TabArea.InGame) ? 1 : 0) + (ShowTab(TabArea.Clan) ? 1 : 0) + (ShowTab(TabArea.Moderation) ? 1 : 0);

	public static ChatContext CurrentChatContext
	{
		get
		{
			if (!GameState.Current.PlayerData.IsSpectator)
			{
				return ChatContext.Player;
			}
			return ChatContext.Spectator;
		}
	}

	private ChatManager()
	{
		_otherUsers = new List<CommUser>();
		_friendUsers = new List<CommUser>();
		_lobbyUsers = new List<CommUser>();
		_clanUsers = new Dictionary<int, CommUser>();
		_naughtyUsers = new Dictionary<int, CommUser>();
		_ingameUsers = new List<CommUser>();
		_lastgameUsers = new List<CommUser>();
		_allTimePlayers = new Dictionary<int, CommUser>();
		_dialogsByCmid = new Dictionary<int, ChatDialog>();
		_mutedCmids = new HashSet<int>();
		ClanDialog = new ChatDialog(LocalizedStrings.ChatInClan);
		LobbyDialog = new ChatDialog(LocalizedStrings.ChatInLobby);
		ModerationDialog = new ChatDialog(LocalizedStrings.Moderate);
		InGameDialog = new ChatDialog(string.Empty);
		InGameDialog.CanShow = CanShowMessage;
		_commPanes = new ChatGroupPanel[5];
		_commPanes[0] = new ChatGroupPanel();
		_commPanes[1] = new ChatGroupPanel();
		_commPanes[2] = new ChatGroupPanel();
		_commPanes[3] = new ChatGroupPanel();
		_commPanes[4] = new ChatGroupPanel();
		_tabAreas = new HashSet<TabArea>
		{
			TabArea.Lobby,
			TabArea.Private
		};
		_commPanes[0].AddGroup(UserGroups.None, LocalizedStrings.Lobby, LobbyUsers);
		_commPanes[1].AddGroup(UserGroups.Friend, LocalizedStrings.Friends, FriendUsers);
		_commPanes[1].AddGroup(UserGroups.Other, LocalizedStrings.Others, OtherUsers);
		_commPanes[2].AddGroup(UserGroups.None, LocalizedStrings.Clan, ClanUsers);
		_commPanes[3].AddGroup(UserGroups.None, LocalizedStrings.Game, GameUsers);
		_commPanes[3].AddGroup(UserGroups.Other, "History", GameHistoryUsers);
		_commPanes[4].AddGroup(UserGroups.None, "Naughty List", NaughtyUsers);
		EventHandler.Global.AddListener<GlobalEvents.Login>(OnLoginEvent);
	}

	protected override void OnDispose()
	{
		EventHandler.Global.RemoveListener<GlobalEvents.Login>(OnLoginEvent);
	}

	private void OnLoginEvent(GlobalEvents.Login ev)
	{
		if (ev.AccessLevel >= MemberAccessLevel.Moderator)
		{
			_tabAreas.Add(TabArea.Moderation);
		}
	}

	public bool IsMuted(int cmid)
	{
		return _mutedCmids.Contains(cmid);
	}

	public void HideConversations(int cmid)
	{
		_mutedCmids.Add(cmid);
		LobbyDialog.RecalulateBounds();
		if (_dialogsByCmid.TryGetValue(cmid, out ChatDialog value))
		{
			value.RecalulateBounds();
		}
	}

	public void ShowConversations(int cmid)
	{
		_mutedCmids.Remove(cmid);
		LobbyDialog.RecalulateBounds();
		if (_dialogsByCmid.TryGetValue(cmid, out ChatDialog value))
		{
			value.RecalulateBounds();
		}
	}

	public bool ShowTab(TabArea tab)
	{
		switch (tab)
		{
		case TabArea.InGame:
			if (!GameState.Current.HasJoinedGame)
			{
				return Singleton<ChatManager>.Instance.GameHistoryUsers.Count > 0;
			}
			return true;
		case TabArea.Clan:
			return PlayerDataManager.IsPlayerInClan;
		case TabArea.Moderation:
			return PlayerDataManager.AccessLevel >= MemberAccessLevel.Moderator;
		default:
			return _tabAreas.Contains(tab);
		}
	}

	public static bool CanShowMessage(ChatContext ctx)
	{
		if (GameState.Current.HasJoinedGame && GameState.Current.GameMode == GameModeType.EliminationMode && GameState.Current.IsInGame)
		{
			ChatContext chatContext = GameState.Current.PlayerData.IsSpectator ? ChatContext.Spectator : ChatContext.Player;
			return ctx == chatContext;
		}
		return true;
	}

	public bool HasDialogWith(int cmid)
	{
		return _dialogsByCmid.ContainsKey(cmid);
	}

	public void UpdateClanSection()
	{
		Singleton<ChatManager>.Instance._clanUsers.Clear();
		foreach (ClanMemberView clanMember in Singleton<PlayerDataManager>.Instance.ClanMembers)
		{
			Singleton<ChatManager>.Instance._clanUsers[clanMember.Cmid] = new CommUser(clanMember);
		}
		RefreshAll(forceRefresh: true);
	}

	public void RefreshAll(bool forceRefresh = false)
	{
		if (forceRefresh || _nextRefreshTime < Time.time)
		{
			_nextRefreshTime = Time.time + 5f;
			_lobbyUsers.Clear();
			foreach (CommActorInfo player2 in AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Players)
			{
				if (player2.Cmid > 0)
				{
					CommUser commUser = new CommUser(player2);
					commUser.IsClanMember = PlayerDataManager.IsClanMember(player2.Cmid);
					commUser.IsFriend = PlayerDataManager.IsFriend(player2.Cmid);
					commUser.IsFacebookFriend = PlayerDataManager.IsFacebookFriend(player2.Cmid);
					commUser.IsOnline = true;
					CommUser item = commUser;
					_lobbyUsers.Add(item);
				}
			}
			_lobbyUsers.Sort(new CommUserNameComparer());
			_lobbyUsers.Sort(new CommUserFriendsComparer());
			CommActorInfo player;
			foreach (CommUser lastgameUser in Singleton<ChatManager>.Instance._lastgameUsers)
			{
				lastgameUser.IsClanMember = PlayerDataManager.IsClanMember(lastgameUser.Cmid);
				lastgameUser.IsFriend = PlayerDataManager.IsFriend(lastgameUser.Cmid);
				lastgameUser.IsFacebookFriend = PlayerDataManager.IsFacebookFriend(lastgameUser.Cmid);
				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(lastgameUser.Cmid, out player))
				{
					lastgameUser.SetActor(player);
				}
				else
				{
					lastgameUser.SetActor(null);
				}
			}
			Singleton<ChatManager>.Instance._lastgameUsers.Sort(new CommUserPresenceComparer());
			foreach (CommUser friendUser in Singleton<ChatManager>.Instance._friendUsers)
			{
				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(friendUser.Cmid, out player))
				{
					friendUser.SetActor(player);
				}
				else
				{
					friendUser.SetActor(null);
				}
			}
			Singleton<ChatManager>.Instance._friendUsers.Sort(new CommUserPresenceComparer());
			foreach (CommUser value in Singleton<ChatManager>.Instance._clanUsers.Values)
			{
				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(value.Cmid, out player))
				{
					value.SetActor(player);
				}
				else
				{
					value.SetActor(null);
				}
			}
			foreach (CommUser otherUser in Singleton<ChatManager>.Instance._otherUsers)
			{
				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(otherUser.Cmid, out player))
				{
					otherUser.SetActor(player);
				}
				else
				{
					otherUser.SetActor(null);
				}
			}
			Singleton<ChatManager>.Instance._otherUsers.Sort(new CommUserNameComparer());
			foreach (KeyValuePair<int, CommUser> naughtyUser in Singleton<ChatManager>.Instance._naughtyUsers)
			{
				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(naughtyUser.Key, out player))
				{
					naughtyUser.Value.SetActor(player);
				}
				else
				{
					naughtyUser.Value.SetActor(null);
				}
			}
		}
	}

	public void UpdateFriendSection()
	{
		List<CommUser> list = new List<CommUser>(Singleton<ChatManager>.Instance._friendUsers);
		Singleton<ChatManager>.Instance._friendUsers.Clear();
		foreach (PublicProfileView friend in Singleton<PlayerDataManager>.Instance.FriendList)
		{
			Singleton<ChatManager>.Instance._friendUsers.Add(new CommUser(friend));
		}
		foreach (PublicProfileView facebookFriend in Singleton<PlayerDataManager>.Instance.FacebookFriends)
		{
			Singleton<ChatManager>.Instance._friendUsers.Add(new CommUser(facebookFriend));
		}
		using (List<CommUser>.Enumerator enumerator3 = Singleton<ChatManager>.Instance._friendUsers.GetEnumerator())
		{
			CommUser f3;
			while (enumerator3.MoveNext())
			{
				f3 = enumerator3.Current;
				if (Singleton<ChatManager>.Instance._otherUsers.RemoveAll((CommUser u) => u.Cmid == f3.Cmid) > 0 && Singleton<ChatManager>.Instance._dialogsByCmid.TryGetValue(f3.Cmid, out ChatDialog value))
				{
					value.Group = UserGroups.Friend;
				}
			}
		}
		using (List<CommUser>.Enumerator enumerator4 = list.GetEnumerator())
		{
			CommUser f2;
			while (enumerator4.MoveNext())
			{
				f2 = enumerator4.Current;
				if (Singleton<ChatManager>.Instance._dialogsByCmid.TryGetValue(f2.Cmid, out ChatDialog value2) && !Singleton<ChatManager>.Instance._friendUsers.Exists((CommUser u) => u.Cmid == f2.Cmid) && !Singleton<ChatManager>.Instance._otherUsers.Exists((CommUser u) => u.Cmid == f2.Cmid))
				{
					Singleton<ChatManager>.Instance._otherUsers.Add(f2);
					value2.Group = UserGroups.Other;
				}
			}
		}
		Singleton<ChatManager>.Instance.RefreshAll();
	}

	public static Texture GetPresenceIcon(CommActorInfo user)
	{
		if (user != null)
		{
			return GetPresenceIcon((user.CurrentRoom == null) ? PresenceType.Online : PresenceType.InGame);
		}
		return GetPresenceIcon(PresenceType.Offline);
	}

	public static Texture GetPresenceIcon(PresenceType index)
	{
		switch (index)
		{
		case PresenceType.InGame:
			return CommunicatorIcons.PresencePlaying;
		case PresenceType.Online:
			return CommunicatorIcons.PresenceOnline;
		case PresenceType.Offline:
			return CommunicatorIcons.PresenceOffline;
		default:
			return CommunicatorIcons.PresenceOffline;
		}
	}

	public void SetGameSection(string server, int roomId, int mapId, IEnumerable<GameActorInfo> actors)
	{
		_ingameUsers.Clear();
		_lastgameUsers.Clear();
		_lastgameUsers.AddRange(_allTimePlayers.Values);
		using (IEnumerator<GameActorInfo> enumerator = actors.GetEnumerator())
		{
			GameActorInfo v;
			while (enumerator.MoveNext())
			{
				v = enumerator.Current;
				CommUser commUser = new CommUser(v);
				commUser.CurrentGame = new GameRoom
				{
					Server = new ConnectionAddress(server),
					Number = roomId,
					MapId = mapId
				};
				commUser.IsClanMember = PlayerDataManager.IsClanMember(commUser.Cmid);
				commUser.IsFriend = PlayerDataManager.IsFriend(commUser.Cmid);
				commUser.IsFacebookFriend = PlayerDataManager.IsFacebookFriend(commUser.Cmid);
				_ingameUsers.Add(commUser);
				_lastgameUsers.RemoveAll((CommUser p) => p.Cmid == v.Cmid);
				if (v.Cmid != PlayerDataManager.Cmid && !_allTimePlayers.ContainsKey(v.Cmid))
				{
					CommUser commUser2 = new CommUser(v);
					commUser2.CurrentGame = new GameRoom
					{
						Server = new ConnectionAddress(server),
						Number = roomId,
						MapId = mapId
					};
					_allTimePlayers[v.Cmid] = commUser2;
				}
			}
		}
		_ingameUsers.Sort(new CommUserNameComparer());
	}

	public List<CommUser> GetCommUsersToReport()
	{
		int capacity = _ingameUsers.Count + _lobbyUsers.Count + _otherUsers.Count;
		Dictionary<int, CommUser> dictionary = new Dictionary<int, CommUser>(capacity);
		foreach (CommUser ingameUser in _ingameUsers)
		{
			dictionary[ingameUser.Cmid] = ingameUser;
		}
		foreach (CommUser otherUser in _otherUsers)
		{
			dictionary[otherUser.Cmid] = otherUser;
		}
		foreach (CommUser lobbyUser in _lobbyUsers)
		{
			dictionary[lobbyUser.Cmid] = lobbyUser;
		}
		return new List<CommUser>(dictionary.Values);
	}

	public bool TryGetClanUsers(int cmid, out CommUser user)
	{
		if (_clanUsers.TryGetValue(cmid, out user))
		{
			return user != null;
		}
		return false;
	}

	public bool TryGetGameUser(int cmid, out CommUser user)
	{
		user = null;
		foreach (CommUser ingameUser in _ingameUsers)
		{
			if (ingameUser.Cmid == cmid)
			{
				user = ingameUser;
				return true;
			}
		}
		return false;
	}

	public bool TryGetLobbyCommUser(int cmid, out CommUser user)
	{
		user = _lobbyUsers.Find((CommUser u) => u.Cmid == cmid);
		return user != null;
	}

	public bool TryGetFriend(int cmid, out CommUser user)
	{
		foreach (CommUser friendUser in _friendUsers)
		{
			if (friendUser.Cmid == cmid)
			{
				user = friendUser;
				return true;
			}
		}
		user = null;
		return false;
	}

	public void CreatePrivateChat(int cmid)
	{
		ChatDialog chatDialog = null;
		if (_dialogsByCmid.TryGetValue(cmid, out ChatDialog value) && value != null)
		{
			chatDialog = value;
		}
		else
		{
			CommUser commUser = null;
			CommActorInfo player = null;
			if (PlayerDataManager.IsFriend(cmid) || PlayerDataManager.IsFacebookFriend(cmid))
			{
				commUser = _friendUsers.Find((CommUser u) => u.Cmid == cmid);
				if (commUser != null)
				{
					chatDialog = new ChatDialog(commUser, UserGroups.Friend);
				}
			}
			else if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(cmid, out player))
			{
				if (PlayerDataManager.TryGetClanMember(cmid, out ClanMemberView view))
				{
					commUser = new CommUser(view);
					commUser.SetActor(player);
				}
				else
				{
					commUser = new CommUser(player);
				}
				_otherUsers.Add(commUser);
				chatDialog = new ChatDialog(commUser, UserGroups.Other);
			}
			if (chatDialog != null)
			{
				_dialogsByCmid.Add(cmid, chatDialog);
			}
		}
		if (chatDialog != null)
		{
			ChatPageGUI.SelectedTab = TabArea.Private;
			SelectedDialog = chatDialog;
			SelectedCmid = cmid;
		}
		else
		{
			Debug.LogError($"Player with cmuneID {cmid} not found in communicator!");
		}
	}

	public string GetAllChatMessagesForPlayerReport()
	{
		StringBuilder stringBuilder = new StringBuilder();
		ICollection<InstantMessage> allMessages = Singleton<ChatManager>.Instance.InGameDialog.AllMessages;
		if (allMessages.Count > 0)
		{
			stringBuilder.AppendLine("In Game Chat:");
			foreach (InstantMessage item in allMessages)
			{
				stringBuilder.AppendLine(item.PlayerName + " : " + item.Text);
			}
			stringBuilder.AppendLine();
		}
		foreach (ChatDialog value in Singleton<ChatManager>.Instance._dialogsByCmid.Values)
		{
			allMessages = value.AllMessages;
			if (allMessages.Count > 0)
			{
				stringBuilder.AppendLine("Private Chat:");
				foreach (InstantMessage item2 in allMessages)
				{
					stringBuilder.AppendLine(item2.PlayerName + " : " + item2.Text);
				}
				stringBuilder.AppendLine();
			}
		}
		allMessages = Singleton<ChatManager>.Instance.ClanDialog.AllMessages;
		if (allMessages.Count > 0)
		{
			stringBuilder.AppendLine("Clan Chat:");
			foreach (InstantMessage item3 in allMessages)
			{
				stringBuilder.AppendLine(item3.PlayerName + " : " + item3.Text);
			}
			stringBuilder.AppendLine();
		}
		allMessages = Singleton<ChatManager>.Instance.LobbyDialog.AllMessages;
		if (allMessages.Count > 0)
		{
			stringBuilder.AppendLine("Lobby Chat:");
			foreach (InstantMessage item4 in allMessages)
			{
				stringBuilder.AppendLine(item4.PlayerName + " : " + item4.Text);
			}
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	public void UpdateLastgamePlayers()
	{
		Singleton<ChatManager>.Instance._lastgameUsers.Clear();
		foreach (CommUser value in Singleton<ChatManager>.Instance._allTimePlayers.Values)
		{
			value.IsClanMember = PlayerDataManager.IsClanMember(value.Cmid);
			value.IsFriend = PlayerDataManager.IsFriend(value.Cmid);
			value.IsFacebookFriend = PlayerDataManager.IsFacebookFriend(value.Cmid);
			if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(value.Cmid, out CommActorInfo player))
			{
				value.SetActor(player);
			}
			else
			{
				value.SetActor(null);
			}
			Singleton<ChatManager>.Instance._lastgameUsers.Add(value);
		}
		Singleton<ChatManager>.Instance._lastgameUsers.Sort(new CommUserPresenceComparer());
	}

	public void SetNaughtyList(List<CommActorInfo> hackers)
	{
		foreach (CommActorInfo hacker in hackers)
		{
			_naughtyUsers[hacker.Cmid] = new CommUser(hacker);
		}
	}

	public void AddClanMessage(int cmid, InstantMessage msg)
	{
		ClanDialog.AddMessage(msg);
		if (cmid != PlayerDataManager.Cmid && ChatPageGUI.SelectedTab != TabArea.Clan)
		{
			HasUnreadClanMessage.Value = true;
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.NewMessage, 0uL);
		}
	}

	public void AddNewPrivateMessage(int cmid, InstantMessage msg)
	{
		try
		{
			if (!_dialogsByCmid.TryGetValue(cmid, out ChatDialog value) && !msg.IsNotification)
			{
				if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.TryGetPlayer(cmid, out CommActorInfo player))
				{
					CommUser commUser = new CommUser(player);
					value = AddNewDialog(commUser);
					if (!_friendUsers.Exists((CommUser p) => p.Cmid == cmid))
					{
						_otherUsers.Add(commUser);
					}
				}
				else
				{
					CommActorInfo commActorInfo = new CommActorInfo();
					commActorInfo.Cmid = cmid;
					commActorInfo.PlayerName = msg.PlayerName;
					commActorInfo.Channel = ChannelType.Steam;
					commActorInfo.AccessLevel = msg.AccessLevel;
					CommUser commUser2 = new CommUser(commActorInfo);
					value = AddNewDialog(commUser2);
					if (!_friendUsers.Exists((CommUser p) => p.Cmid == cmid))
					{
						_otherUsers.Add(commUser2);
					}
				}
			}
			if (value != null)
			{
				value.AddMessage(msg);
				if (value != SelectedDialog)
				{
					value.HasUnreadMessage = true;
				}
				if (ChatPageGUI.SelectedTab != TabArea.Private)
				{
					HasUnreadPrivateMessage.Value = true;
					AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.NewMessage, 0uL);
				}
			}
		}
		catch
		{
			Debug.LogError($"AddNewPrivateMessage from cmid={cmid}");
			throw;
		}
	}

	public ChatDialog AddNewDialog(CommUser user)
	{
		ChatDialog value = null;
		if (user != null && !_dialogsByCmid.TryGetValue(user.Cmid, out value))
		{
			value = ((!PlayerDataManager.IsFriend(user.Cmid) && !PlayerDataManager.IsFacebookFriend(user.Cmid)) ? new ChatDialog(user, UserGroups.Other) : new ChatDialog(user, UserGroups.Friend));
			_dialogsByCmid.Add(user.Cmid, value);
		}
		return value;
	}

	internal void RemoveDialog(ChatDialog d)
	{
		_dialogsByCmid.Remove(d.UserCmid);
		_otherUsers.RemoveAll((CommUser u) => u.Cmid == d.UserCmid);
		SelectedDialog = null;
	}
}
