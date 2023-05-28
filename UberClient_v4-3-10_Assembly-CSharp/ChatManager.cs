// Decompiled with JetBrains decompiler
// Type: ChatManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ChatManager : Singleton<ChatManager>
{
  private List<CommUser> _otherUsers;
  private List<CommUser> _friendUsers;
  private List<CommUser> _lobbyUsers;
  public Dictionary<int, CommUser> _modUsers;
  private Dictionary<int, CommUser> _clanUsers;
  private List<CommUser> _ingameUsers;
  private List<CommUser> _lastgameUsers;
  private Dictionary<int, CommUser> _allTimePlayers;
  private HashSet<TabArea> _tabAreas;
  private float _nextRefreshTime;
  public int _selectedCmid;
  public ChatDialog _selectedDialog;
  public ChatGroupPanel[] _commPanes;
  public Dictionary<int, ChatDialog> _dialogsByCmid;

  private ChatManager()
  {
    this._otherUsers = new List<CommUser>();
    this._friendUsers = new List<CommUser>();
    this._lobbyUsers = new List<CommUser>();
    this._clanUsers = new Dictionary<int, CommUser>();
    this._modUsers = new Dictionary<int, CommUser>();
    this._ingameUsers = new List<CommUser>();
    this._lastgameUsers = new List<CommUser>();
    this._allTimePlayers = new Dictionary<int, CommUser>();
    this._dialogsByCmid = new Dictionary<int, ChatDialog>();
    this.ClanDialog = new ChatDialog(string.Empty);
    this.LobbyDialog = new ChatDialog(string.Empty);
    this.ModerationDialog = new ChatDialog(string.Empty);
    this.InGameDialog = new ChatDialog(string.Empty);
    this.InGameDialog.CanShow = new ChatDialog.CanShowMessage(ChatManager.CanShowMessage);
    this._commPanes = new ChatGroupPanel[5];
    this._commPanes[0] = new ChatGroupPanel();
    this._commPanes[1] = new ChatGroupPanel();
    this._commPanes[2] = new ChatGroupPanel();
    this._commPanes[3] = new ChatGroupPanel();
    this._commPanes[4] = new ChatGroupPanel();
    this._tabAreas = new HashSet<TabArea>()
    {
      TabArea.Lobby,
      TabArea.Private
    };
    this.ClanDialog.Title = LocalizedStrings.ChatInClan;
    this.LobbyDialog.Title = LocalizedStrings.ChatInLobby;
    this.ModerationDialog.Title = LocalizedStrings.Moderate;
    this._commPanes[0].AddGroup(UserGroups.None, LocalizedStrings.Lobby, this.LobbyUsers);
    this._commPanes[1].AddGroup(UserGroups.Friend, LocalizedStrings.Friends, this.FriendUsers);
    this._commPanes[1].AddGroup(UserGroups.Other, LocalizedStrings.Others, this.OtherUsers);
    this._commPanes[2].AddGroup(UserGroups.None, LocalizedStrings.Clan, this.ClanUsers);
    this._commPanes[3].AddGroup(UserGroups.None, LocalizedStrings.Game, this.GameUsers);
    this._commPanes[3].AddGroup(UserGroups.Other, "History", this.GameHistoryUsers);
    this._commPanes[4].AddGroup(UserGroups.None, "Naughty List", this.NaughtyUsers);
    CmuneEventHandler.AddListener<LoginEvent>(new Action<LoginEvent>(this.OnLoginEvent));
  }

  public bool HasUnreadPrivateMessage { get; set; }

  public bool HasUnreadClanMessage { get; set; }

  public ChatDialog ClanDialog { get; private set; }

  public ChatDialog LobbyDialog { get; private set; }

  public ChatDialog InGameDialog { get; private set; }

  public ChatDialog ModerationDialog { get; private set; }

  public ICollection<CommUser> OtherUsers => (ICollection<CommUser>) this._otherUsers;

  public ICollection<CommUser> FriendUsers => (ICollection<CommUser>) this._friendUsers;

  public ICollection<CommUser> LobbyUsers => (ICollection<CommUser>) this._lobbyUsers;

  public ICollection<CommUser> ClanUsers => (ICollection<CommUser>) this._clanUsers.Values;

  public ICollection<CommUser> NaughtyUsers => (ICollection<CommUser>) this._modUsers.Values;

  public ICollection<CommUser> GameUsers => (ICollection<CommUser>) this._ingameUsers;

  public ICollection<CommUser> GameHistoryUsers => (ICollection<CommUser>) this._lastgameUsers;

  protected override void OnDispose() => CmuneEventHandler.RemoveListener<LoginEvent>(new Action<LoginEvent>(this.OnLoginEvent));

  private void OnLoginEvent(LoginEvent ev)
  {
    if (ev.AccessLevel <= MemberAccessLevel.Default)
      return;
    this._tabAreas.Add(TabArea.Moderation);
  }

  public int TabCounter => this._tabAreas.Count + (!this.ShowTab(TabArea.InGame) ? 0 : 1) + (!this.ShowTab(TabArea.Clan) ? 0 : 1) + (!this.ShowTab(TabArea.Moderation) ? 0 : 1);

  public bool ShowTab(TabArea tab)
  {
    switch (tab)
    {
      case TabArea.Clan:
        return PlayerDataManager.IsPlayerInClan;
      case TabArea.InGame:
        return GameState.HasCurrentGame || Singleton<ChatManager>.Instance.GameHistoryUsers.Count > 0;
      case TabArea.Moderation:
        return PlayerDataManager.AccessLevel > MemberAccessLevel.Default;
      default:
        return this._tabAreas.Contains(tab);
    }
  }

  public static ChatContext CurrentChatContext => Singleton<PlayerSpectatorControl>.Instance.IsEnabled ? ChatContext.Spectator : ChatContext.Player;

  public static bool CanShowMessage(ChatContext ctx)
  {
    if (!GameState.HasCurrentGame || GameState.CurrentGameMode != GameMode.TeamElimination || !GameState.CurrentGame.IsMatchRunning)
      return true;
    ChatContext chatContext = !Singleton<PlayerSpectatorControl>.Instance.IsEnabled ? ChatContext.Player : ChatContext.Spectator;
    return ctx == chatContext;
  }

  public bool HasDialogWith(int cmid) => this._dialogsByCmid.ContainsKey(cmid);

  public void UpdateClanSection()
  {
    Singleton<ChatManager>.Instance._clanUsers.Clear();
    foreach (ClanMemberView clanMember in Singleton<PlayerDataManager>.Instance.ClanMembers)
      Singleton<ChatManager>.Instance._clanUsers[clanMember.Cmid] = new CommUser(clanMember);
    this.RefreshAll(true);
  }

  public void RefreshAll(bool forceRefresh = false)
  {
    if (!forceRefresh && (double) this._nextRefreshTime >= (double) Time.time)
      return;
    this._nextRefreshTime = Time.time + 5f;
    this._lobbyUsers.Clear();
    foreach (CommActorInfo player in CommConnectionManager.CommCenter.Players)
    {
      if (player.ActorId > 0)
        this._lobbyUsers.Add(new CommUser(player)
        {
          IsClanMember = PlayerDataManager.IsClanMember(player.Cmid),
          IsFriend = PlayerDataManager.IsFriend(player.Cmid)
        });
    }
    this._lobbyUsers.Sort((IComparer<CommUser>) new CommUserNameComparer());
    this._lobbyUsers.Sort((IComparer<CommUser>) new CommUserFriendsComparer());
    CommActorInfo info;
    foreach (CommUser lastgameUser in Singleton<ChatManager>.Instance._lastgameUsers)
    {
      lastgameUser.IsClanMember = PlayerDataManager.IsClanMember(lastgameUser.Cmid);
      lastgameUser.IsFriend = PlayerDataManager.IsFriend(lastgameUser.Cmid);
      if (CommConnectionManager.CommCenter.TryGetActorWithCmid(lastgameUser.Cmid, out info))
        lastgameUser.SetActor(info);
      else
        lastgameUser.SetActor((CommActorInfo) null);
    }
    Singleton<ChatManager>.Instance._lastgameUsers.Sort((IComparer<CommUser>) new CommUserPresenceComparer());
    foreach (CommUser friendUser in Singleton<ChatManager>.Instance._friendUsers)
    {
      if (CommConnectionManager.CommCenter.TryGetActorWithCmid(friendUser.Cmid, out info))
        friendUser.SetActor(info);
      else
        friendUser.SetActor((CommActorInfo) null);
    }
    Singleton<ChatManager>.Instance._friendUsers.Sort((IComparer<CommUser>) new CommUserPresenceComparer());
    foreach (CommUser commUser in Singleton<ChatManager>.Instance._clanUsers.Values)
    {
      if (CommConnectionManager.CommCenter.TryGetActorWithCmid(commUser.Cmid, out info))
        commUser.SetActor(info);
      else
        commUser.SetActor((CommActorInfo) null);
    }
    foreach (CommUser otherUser in Singleton<ChatManager>.Instance._otherUsers)
    {
      if (CommConnectionManager.CommCenter.TryGetActorWithCmid(otherUser.Cmid, out info))
        otherUser.SetActor(info);
      else
        otherUser.SetActor((CommActorInfo) null);
    }
    Singleton<ChatManager>.Instance._otherUsers.Sort((IComparer<CommUser>) new CommUserNameComparer());
    foreach (KeyValuePair<int, CommUser> modUser in Singleton<ChatManager>.Instance._modUsers)
    {
      if (CommConnectionManager.CommCenter.TryGetActorWithCmid(modUser.Key, out info))
        modUser.Value.SetActor(info);
      else
        modUser.Value.SetActor((CommActorInfo) null);
    }
  }

  public void UpdateFriendSection()
  {
    List<CommUser> commUserList = new List<CommUser>((IEnumerable<CommUser>) Singleton<ChatManager>.Instance._friendUsers);
    Singleton<ChatManager>.Instance._friendUsers.Clear();
    foreach (PublicProfileView friend in Singleton<PlayerDataManager>.Instance.FriendList)
      Singleton<ChatManager>.Instance._friendUsers.Add(new CommUser(friend));
    foreach (CommUser friendUser in Singleton<ChatManager>.Instance._friendUsers)
    {
      CommUser f = friendUser;
      ChatDialog chatDialog;
      if (Singleton<ChatManager>.Instance._otherUsers.RemoveAll((Predicate<CommUser>) (u => u.Cmid == f.Cmid)) > 0 && Singleton<ChatManager>.Instance._dialogsByCmid.TryGetValue(f.Cmid, out chatDialog))
        chatDialog.Group = UserGroups.Friend;
    }
    foreach (CommUser commUser in commUserList)
    {
      CommUser f = commUser;
      ChatDialog chatDialog;
      if (Singleton<ChatManager>.Instance._dialogsByCmid.TryGetValue(f.Cmid, out chatDialog) && !Singleton<ChatManager>.Instance._friendUsers.Exists((Predicate<CommUser>) (u => u.Cmid == f.Cmid)) && !Singleton<ChatManager>.Instance._otherUsers.Exists((Predicate<CommUser>) (u => u.Cmid == f.Cmid)))
      {
        Singleton<ChatManager>.Instance._otherUsers.Add(f);
        chatDialog.Group = UserGroups.Other;
      }
    }
    Singleton<ChatManager>.Instance.RefreshAll();
  }

  public static Texture GetPresenceIcon(CommActorInfo user) => user != null ? ChatManager.GetPresenceIcon(!user.IsInGame ? PresenceType.Online : PresenceType.InGame) : ChatManager.GetPresenceIcon(PresenceType.Offline);

  public static Texture GetPresenceIcon(PresenceType index)
  {
    switch (index)
    {
      case PresenceType.Offline:
        return (Texture) CommunicatorIcons.PresenceOffline;
      case PresenceType.Online:
        return (Texture) CommunicatorIcons.PresenceOnline;
      case PresenceType.InGame:
        return (Texture) CommunicatorIcons.PresencePlaying;
      default:
        return (Texture) CommunicatorIcons.PresenceOffline;
    }
  }

  public void SetGameSection(CmuneRoomID roomId, IEnumerable<UberStrike.Realtime.UnitySdk.CharacterInfo> actors)
  {
    this._ingameUsers.Clear();
    this._lastgameUsers.Clear();
    this._lastgameUsers.AddRange((IEnumerable<CommUser>) this._allTimePlayers.Values);
    foreach (UberStrike.Realtime.UnitySdk.CharacterInfo actor in actors)
    {
      UberStrike.Realtime.UnitySdk.CharacterInfo v = actor;
      CommUser commUser = new CommUser(v)
      {
        CurrentGame = roomId
      };
      commUser.IsClanMember = PlayerDataManager.IsClanMember(commUser.Cmid);
      commUser.IsFriend = PlayerDataManager.IsFriend(commUser.Cmid);
      this._ingameUsers.Add(commUser);
      this._lastgameUsers.RemoveAll((Predicate<CommUser>) (p => p.Cmid == v.Cmid));
      if (v.Cmid != PlayerDataManager.Cmid && !this._allTimePlayers.ContainsKey(v.Cmid))
        this._allTimePlayers[v.Cmid] = new CommUser(v)
        {
          CurrentGame = roomId
        };
    }
    this._ingameUsers.Sort((IComparer<CommUser>) new CommUserNameComparer());
  }

  public List<CommUser> GetCommUsersToReport()
  {
    Dictionary<int, CommUser> dictionary = new Dictionary<int, CommUser>(this._ingameUsers.Count + this._lobbyUsers.Count + this._otherUsers.Count);
    foreach (CommUser ingameUser in this._ingameUsers)
      dictionary[ingameUser.Cmid] = ingameUser;
    foreach (CommUser otherUser in this._otherUsers)
      dictionary[otherUser.Cmid] = otherUser;
    foreach (CommUser lobbyUser in this._lobbyUsers)
      dictionary[lobbyUser.Cmid] = lobbyUser;
    return new List<CommUser>((IEnumerable<CommUser>) dictionary.Values);
  }

  public bool TryGetClanUsers(int cmid, out CommUser user) => this._clanUsers.TryGetValue(cmid, out user) && user != null;

  public bool TryGetLobbyCommUser(int cmid, out CommUser user)
  {
    user = (CommUser) null;
    foreach (CommUser lobbyUser in this._lobbyUsers)
    {
      if (lobbyUser.Cmid == cmid)
      {
        user = lobbyUser;
        return true;
      }
    }
    return false;
  }

  public bool TryGetFriend(int cmid, out CommUser user)
  {
    foreach (CommUser friendUser in this._friendUsers)
    {
      if (friendUser.Cmid == cmid)
      {
        user = friendUser;
        return true;
      }
    }
    user = (CommUser) null;
    return false;
  }

  public void CreatePrivateChat(int cmid)
  {
    ChatDialog chatDialog1 = (ChatDialog) null;
    ChatDialog chatDialog2;
    if (this._dialogsByCmid.TryGetValue(cmid, out chatDialog2) && chatDialog2 != null)
    {
      chatDialog1 = chatDialog2;
    }
    else
    {
      CommActorInfo info = (CommActorInfo) null;
      if (PlayerDataManager.IsFriend(cmid))
      {
        CommUser user = this._friendUsers.Find((Predicate<CommUser>) (u => u.Cmid == cmid));
        if (user != null && user.PresenceIndex != PresenceType.Offline)
          chatDialog1 = new ChatDialog(user, UserGroups.Friend);
      }
      else if (CommConnectionManager.CommCenter.TryGetActorWithCmid(cmid, out info))
      {
        ClanMemberView view;
        CommUser user;
        if (PlayerDataManager.TryGetClanMember(cmid, out view))
        {
          user = new CommUser(view);
          user.SetActor(info);
        }
        else
          user = new CommUser(info);
        this._otherUsers.Add(user);
        chatDialog1 = new ChatDialog(user, UserGroups.Other);
      }
      if (chatDialog1 != null)
        this._dialogsByCmid.Add(cmid, chatDialog1);
    }
    if (chatDialog1 != null)
    {
      ChatPageGUI.SelectedTab = TabArea.Private;
      this._selectedDialog = chatDialog1;
      this._selectedCmid = cmid;
    }
    else
      Debug.LogError((object) string.Format("Player with cmuneID {0} not found in communicator!", (object) cmid));
  }

  public string GetAllChatMessagesForPlayerReport()
  {
    StringBuilder stringBuilder = new StringBuilder();
    ICollection<InstantMessage> allMessages1 = Singleton<ChatManager>.Instance.InGameDialog.AllMessages;
    if (allMessages1.Count > 0)
    {
      stringBuilder.AppendLine("In Game Chat:");
      foreach (InstantMessage instantMessage in (IEnumerable<InstantMessage>) allMessages1)
        stringBuilder.AppendLine(instantMessage.PlayerName + " : " + instantMessage.MessageText);
      stringBuilder.AppendLine();
    }
    foreach (ChatDialog chatDialog in Singleton<ChatManager>.Instance._dialogsByCmid.Values)
    {
      ICollection<InstantMessage> allMessages2 = chatDialog.AllMessages;
      if (allMessages2.Count > 0)
      {
        stringBuilder.AppendLine("Private Chat:");
        foreach (InstantMessage instantMessage in (IEnumerable<InstantMessage>) allMessages2)
          stringBuilder.AppendLine(instantMessage.PlayerName + " : " + instantMessage.MessageText);
        stringBuilder.AppendLine();
      }
    }
    ICollection<InstantMessage> allMessages3 = Singleton<ChatManager>.Instance.ClanDialog.AllMessages;
    if (allMessages3.Count > 0)
    {
      stringBuilder.AppendLine("Clan Chat:");
      foreach (InstantMessage instantMessage in (IEnumerable<InstantMessage>) allMessages3)
        stringBuilder.AppendLine(instantMessage.PlayerName + " : " + instantMessage.MessageText);
      stringBuilder.AppendLine();
    }
    ICollection<InstantMessage> allMessages4 = Singleton<ChatManager>.Instance.LobbyDialog.AllMessages;
    if (allMessages4.Count > 0)
    {
      stringBuilder.AppendLine("Lobby Chat:");
      foreach (InstantMessage instantMessage in (IEnumerable<InstantMessage>) allMessages4)
        stringBuilder.AppendLine(instantMessage.PlayerName + " : " + instantMessage.MessageText);
      stringBuilder.AppendLine();
    }
    return stringBuilder.ToString();
  }

  public void UpdateLastgamePlayers()
  {
    Singleton<ChatManager>.Instance._lastgameUsers.Clear();
    foreach (CommUser commUser in Singleton<ChatManager>.Instance._allTimePlayers.Values)
    {
      commUser.IsInGame = false;
      commUser.IsClanMember = PlayerDataManager.IsClanMember(commUser.Cmid);
      commUser.IsFriend = PlayerDataManager.IsFriend(commUser.Cmid);
      CommActorInfo info;
      if (CommConnectionManager.CommCenter.TryGetActorWithCmid(commUser.Cmid, out info))
        commUser.SetActor(info);
      else
        commUser.SetActor((CommActorInfo) null);
      Singleton<ChatManager>.Instance._lastgameUsers.Add(commUser);
    }
    Singleton<ChatManager>.Instance._lastgameUsers.Sort((IComparer<CommUser>) new CommUserPresenceComparer());
  }

  public void SetNaughtyList(List<CommActorInfo> hackers)
  {
    foreach (CommActorInfo hacker in hackers)
      this._modUsers[hacker.Cmid] = new CommUser(hacker);
  }

  public void AddClanMessage(int cmid, InstantMessage msg)
  {
    this.ClanDialog.AddMessage(msg);
    if (cmid == PlayerDataManager.Cmid || ChatPageGUI.SelectedTab == TabArea.Clan)
      return;
    this.HasUnreadClanMessage = true;
    SfxManager.Play2dAudioClip(GameAudio.NewMessage);
  }

  public void AddNewPrivateMessage(int cmid, InstantMessage msg)
  {
    try
    {
      ChatDialog chatDialog;
      if (!this._dialogsByCmid.TryGetValue(cmid, out chatDialog))
      {
        CommActorInfo info;
        if (CommConnectionManager.CommCenter.TryGetActorWithCmid(cmid, out info))
        {
          CommUser user = new CommUser(info);
          chatDialog = this.AddNewDialog(user);
          if (!this._friendUsers.Exists((Predicate<CommUser>) (p => p.Cmid == cmid)))
            this._otherUsers.Add(user);
        }
        else
        {
          CommActorInfo user1 = new CommActorInfo(msg.PlayerName, 0, ChannelType.WebPortal);
          user1.Cmid = cmid;
          user1.AccessLevel = (int) msg.AccessLevel;
          CommUser user2 = new CommUser(user1);
          chatDialog = this.AddNewDialog(user2);
          if (!this._friendUsers.Exists((Predicate<CommUser>) (p => p.Cmid == cmid)))
          {
            this._otherUsers.Add(user2);
            CommConnectionManager.CommCenter.SendContactList();
          }
        }
      }
      if (chatDialog != null)
      {
        chatDialog.AddMessage(msg);
        if (ChatPageGUI.SelectedTab != TabArea.Private || chatDialog != this._selectedDialog)
          chatDialog.HasUnreadMessage = true;
      }
      if (msg.Cmid == PlayerDataManager.Cmid || ChatPageGUI.SelectedTab == TabArea.Private)
        return;
      this.HasUnreadPrivateMessage = true;
      SfxManager.Play2dAudioClip(GameAudio.NewMessage);
    }
    catch
    {
      Debug.LogError((object) string.Format("AddNewPrivateMessage from cmid={0}", (object) cmid));
      throw;
    }
  }

  public ChatDialog AddNewDialog(CommUser user)
  {
    ChatDialog chatDialog = (ChatDialog) null;
    if (user != null && !this._dialogsByCmid.TryGetValue(user.Cmid, out chatDialog))
    {
      chatDialog = !PlayerDataManager.IsFriend(user.Cmid) ? new ChatDialog(user, UserGroups.Other) : new ChatDialog(user, UserGroups.Friend);
      this._dialogsByCmid.Add(user.Cmid, chatDialog);
    }
    return chatDialog;
  }

  internal void RemoveDialog(ChatDialog d)
  {
    this._dialogsByCmid.Remove(d.UserCmid);
    this._otherUsers.RemoveAll((Predicate<CommUser>) (u => u.Cmid == d.UserCmid));
    this._selectedDialog = (ChatDialog) null;
  }
}
