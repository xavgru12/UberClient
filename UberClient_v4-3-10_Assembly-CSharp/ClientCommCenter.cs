// Decompiled with JetBrains decompiler
// Type: ClientCommCenter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

[NetworkClass(4)]
public class ClientCommCenter : ClientNetworkClass
{
  private int _totalContactsCount;
  private LimitedQueue<ClientCommCenter.Message> _lastMessages = new LimitedQueue<ClientCommCenter.Message>(5);
  private CommActorInfo _myInfo;
  private ClientCommCenter.ActorList _actors;
  private static bool _isPlayerMuted;

  public ClientCommCenter(RemoteMethodInterface rmi)
    : base(rmi)
  {
    this._actors = new ClientCommCenter.ActorList();
    this._myInfo = new CommActorInfo(string.Empty, 0, ChannelType.WebPortal);
  }

  protected override void OnInitialized()
  {
    base.OnInitialized();
    this.JoinCommServer();
  }

  protected override void OnUninitialized()
  {
    Debug.Log((object) "OnUninitialized ClientCommMode");
    this.MyInfo.ActorId = -1;
    this.MyInfo.CurrentRoom = CmuneRoomID.Empty;
    this._actors.Clear();
    Singleton<ChatManager>.Instance.RefreshAll();
    base.OnUninitialized();
  }

  public void UpdatePlayerRoom(CmuneRoomID room) => this.SendMethodToServer((byte) 21, (object) this._rmi.Messenger.PeerListener.ActorIdSecure, (object) room);

  public void ResetPlayerRoom() => this.SendMethodToServer((byte) 22, (object) this._rmi.Messenger.PeerListener.ActorIdSecure);

  public void Login() => this.JoinCommServer();

  private void UpdateMyInfo()
  {
    this.MyInfo.PlayerName = !string.IsNullOrEmpty(PlayerDataManager.ClanTag) ? string.Format("[{0}] {1}", (object) PlayerDataManager.ClanTag, (object) PlayerDataManager.NameSecure) : PlayerDataManager.NameSecure;
    this.MyInfo.ClanTag = !string.IsNullOrEmpty(PlayerDataManager.ClanTag) ? string.Empty : PlayerDataManager.ClanTag;
    this.MyInfo.Cmid = PlayerDataManager.CmidSecure;
    this.MyInfo.AccessLevel = (int) PlayerDataManager.AccessLevelSecure;
  }

  private void JoinCommServer()
  {
    if (!this.IsInitialized || !PlayerDataManager.IsPlayerLoggedIn)
      return;
    this.UpdateMyInfo();
    this.MyInfo.ActorId = this._rmi.Messenger.PeerListener.ActorIdSecure;
    this.MyInfo.CurrentRoom = this._rmi.Messenger.PeerListener.CurrentRoom;
    this.MyInfo.Channel = ApplicationDataManager.Channel;
    this.SendMethodToServer((byte) 1, (object) this.MyInfo);
    this.SendContactList();
    if (!GameConnectionManager.Client.PeerListener.HasJoinedRoom)
      return;
    this.UpdatePlayerRoom(GameConnectionManager.Client.PeerListener.CurrentRoom);
  }

  public void SendUpdatedActorInfo()
  {
    this.UpdateMyInfo();
    this.SendMethodToServer((byte) 3, (object) SyncObjectBuilder.GetSyncData((CmuneDeltaSync) this.MyInfo, false));
  }

  [NetworkMethod(2)]
  protected void OnPlayerLeft(int cmid)
  {
    CommActorInfo info;
    if (!this.TryGetActorWithCmid(cmid, out info))
      return;
    this.OnPlayerLeft(info, true);
  }

  [NetworkMethod(51)]
  protected void OnPlayerHide(int cmid)
  {
    CommActorInfo info;
    if (!this.TryGetActorWithCmid(cmid, out info) || PlayerDataManager.IsClanMember(cmid) || PlayerDataManager.IsFriend(cmid) || Singleton<ChatManager>.Instance.HasDialogWith(cmid))
      return;
    this.OnPlayerLeft(info, true);
  }

  protected void OnPlayerLeft(CommActorInfo user, bool refreshComm)
  {
    this._actors.Remove(user);
    user.CurrentRoom = CmuneRoomID.Empty;
    user.ActorId = -1;
    Singleton<ChatManager>.Instance.RefreshAll(refreshComm);
  }

  [NetworkMethod(3)]
  protected void OnPlayerUpdate(SyncObject data)
  {
    CommActorInfo actor;
    if (this._actors.TryGetByActorId(data.Id, out actor))
      actor.ReadSyncData(data);
    else if (!data.IsEmpty)
      this._actors.Add(new CommActorInfo(data));
    Singleton<ChatManager>.Instance.RefreshAll();
  }

  [NetworkMethod(44)]
  protected void OnUpdateContacts(List<SyncObject> updated, List<int> removed)
  {
    foreach (SyncObject data in updated)
      this.OnPlayerJoined(data);
    foreach (int cmid in removed)
    {
      CommActorInfo actor;
      if (this._actors.TryGetByCmid(cmid, out actor))
        this.OnPlayerLeft(actor, false);
    }
    Singleton<ChatManager>.Instance.RefreshAll(true);
  }

  [NetworkMethod(4)]
  protected void OnFullPlayerListUpdate(List<SyncObject> players)
  {
    this._actors.Clear();
    foreach (SyncObject player in players)
    {
      if (player.Id == this._rmi.Messenger.PeerListener.ActorId)
      {
        this.MyInfo.ReadSyncData(player);
        this._actors.Add(this.MyInfo);
      }
      else
        this._actors.Add(new CommActorInfo(player));
    }
    Singleton<ChatManager>.Instance.RefreshAll();
  }

  private void OnPlayerJoined(SyncObject data)
  {
    CommActorInfo actor1;
    if (this._actors.TryGetByActorId(data.Id, out actor1))
    {
      actor1.ReadSyncData(data);
    }
    else
    {
      CommActorInfo actor2;
      if (data.Id == this._rmi.Messenger.PeerListener.ActorIdSecure)
      {
        this.MyInfo.ReadSyncData(data);
        actor2 = this.MyInfo;
      }
      else
        actor2 = new CommActorInfo(data);
      this._actors.Add(actor2);
    }
  }

  [NetworkMethod(42)]
  public void OnClanChatMessage(int cmid, int actorId, string name, string message)
  {
    InstantMessage msg = new InstantMessage(cmid, actorId, name, message, MemberAccessLevel.Default);
    Singleton<ChatManager>.Instance.AddClanMessage(cmid, msg);
    PlayerChatLog.AddIncomingMessage(cmid, name, message, PlayerChatLog.ChatContextType.Clan);
  }

  [NetworkMethod(23)]
  protected void OnInGameChatMessage(
    int cmid,
    int actorID,
    string name,
    string message,
    byte accessLevel,
    byte context)
  {
    ChatContext chatContext = (ChatContext) context;
    if (ChatManager.CanShowMessage(chatContext))
      Singleton<InGameChatHud>.Instance.AddChatMessage(name, message, (MemberAccessLevel) accessLevel);
    Singleton<ChatManager>.Instance.InGameDialog.AddMessage(new InstantMessage(cmid, actorID, name, message, (MemberAccessLevel) accessLevel, chatContext));
    PlayerChatLog.AddIncomingMessage(cmid, name, message, PlayerChatLog.ChatContextType.Game);
  }

  [NetworkMethod(25)]
  protected void OnLobbyChatMessage(int cmid, int actorID, string name, string message)
  {
    MemberAccessLevel level = MemberAccessLevel.Default;
    CommActorInfo actor;
    if (this._actors.TryGetByCmid(cmid, out actor))
      level = (MemberAccessLevel) actor.AccessLevel;
    Singleton<ChatManager>.Instance.LobbyDialog.AddMessage(new InstantMessage(cmid, actorID, name, message, level));
    PlayerChatLog.AddIncomingMessage(cmid, name, message, PlayerChatLog.ChatContextType.Lobby);
  }

  [NetworkMethod(24)]
  protected void OnPrivateChatMessage(int cmid, int actorID, string name, string message)
  {
    MemberAccessLevel level = MemberAccessLevel.Default;
    CommActorInfo actor;
    if (this._actors.TryGetByCmid(cmid, out actor))
      level = (MemberAccessLevel) actor.AccessLevel;
    InstantMessage msg = new InstantMessage(cmid, actorID, name, message, level);
    Singleton<ChatManager>.Instance.AddNewPrivateMessage(cmid, msg);
    PlayerChatLog.AddIncomingMessage(cmid, name, message, PlayerChatLog.ChatContextType.Private);
  }

  [NetworkMethod(35)]
  protected void OnGameInviteMessage(int actorId, string message, CmuneRoomID roomId)
  {
  }

  [NetworkMethod(36)]
  public void OnDisconnectAndDisablePhoton(string message)
  {
    if (!PhotonClient.IsPhotonEnabled)
      return;
    if (GameState.HasCurrentGame)
      Singleton<GameStateController>.Instance.LeaveGame();
    PhotonClient.IsPhotonEnabled = false;
    LobbyConnectionManager.Stop();
    CommConnectionManager.Stop();
    ApplicationDataManager.LockApplication(message);
  }

  [NetworkMethod(26)]
  protected virtual void OnUpdateIngameGroup(List<int> actorIDs)
  {
  }

  [NetworkMethod(37)]
  protected virtual void OnUpdateInboxRequests() => Singleton<InboxManager>.Instance.RefreshAllRequests();

  [NetworkMethod(38)]
  protected virtual void OnUpdateFriendsList() => MonoRoutine.Start(Singleton<CommsManager>.Instance.GetContactsByGroups());

  [NetworkMethod(39)]
  protected void OnUpdateInboxMessages(int messageId) => Singleton<InboxManager>.Instance.GetMessageWithId(messageId);

  [NetworkMethod(40)]
  protected void OnUpdateClanMembers() => Singleton<ClanDataManager>.Instance.RefreshClanData(true);

  [NetworkMethod(53)]
  protected void OnUpdateClanData() => Singleton<ClanDataManager>.Instance.CheckCompleteClanData();

  [NetworkMethod(46)]
  public void OnUpdateActorsForModeration(List<SyncObject> allHackers)
  {
    List<CommActorInfo> hackers = new List<CommActorInfo>(allHackers.Count);
    foreach (SyncObject allHacker in allHackers)
    {
      CommActorInfo actor;
      if (this._actors.TryGetByActorId(allHacker.Id, out actor) && actor != null)
        actor.ReadSyncData(allHacker);
      else
        actor = new CommActorInfo(allHacker);
      hackers.Add(actor);
    }
    Singleton<ChatManager>.Instance.SetNaughtyList(hackers);
    this.SendContactList();
  }

  [NetworkMethod(31)]
  public void OnModerationCustomMessage(string message)
  {
    if (GameState.HasCurrentGame && !GameState.LocalPlayer.IsGamePaused)
      GameState.LocalPlayer.Pause();
    PopupSystem.ShowMessage("Administrator Message", message, PopupSystem.AlertType.OK, (Action) (() => { }));
  }

  [NetworkMethod(30)]
  public void OnModerationMutePlayer(bool isPlayerMuted)
  {
    ClientCommCenter.IsPlayerMuted = isPlayerMuted;
    if (!isPlayerMuted)
      return;
    PopupSystem.ShowMessage("ADMIN MESSAGE", "You have been muted!", PopupSystem.AlertType.OK, (Action) (() => { }));
  }

  [NetworkMethod(32)]
  public void OnModerationKickGame()
  {
    Singleton<GameStateController>.Instance.LeaveGame();
    PopupSystem.ShowMessage("ADMIN MESSAGE", "You were kicked out of the game!", PopupSystem.AlertType.OK, (Action) (() => { }));
  }

  public void SendModerationBanPlayer(int cmid) => CommConnectionManager.CommCenter.SendMethodToServer((byte) 33, (object) PlayerDataManager.CmidSecure, (object) cmid);

  public void SendModerationBanFromCmune(int cmid) => CommConnectionManager.CommCenter.SendMethodToServer((byte) 49, (object) PlayerDataManager.CmidSecure, (object) cmid);

  public void SendModerationUnbanPlayer(int cmid) => CommConnectionManager.CommCenter.SendMethodToServer((byte) 34, (object) cmid);

  public void SendKickFromGame(int actorId)
  {
    CommConnectionManager.CommCenter.SendMethodToPlayer(actorId, (byte) 32);
    CommConnectionManager.CommCenter.SendMethodToServer((byte) 32, (object) actorId, (object) PlayerDataManager.CmidSecure);
  }

  public void SendCustomMessage(int actorId, string message) => CommConnectionManager.CommCenter.SendMethodToPlayer(actorId, (byte) 31, (object) message);

  public void SendMutePlayer(int cmid, int actorId, int minutes) => CommConnectionManager.CommCenter.SendMethodToServer((byte) 30, (object) cmid, (object) minutes, (object) actorId, (object) true);

  public void SendGhostPlayer(int cmid, int actorId, int minutes) => CommConnectionManager.CommCenter.SendMethodToServer((byte) 30, (object) cmid, (object) minutes, (object) actorId, (object) false);

  public void SendUnmutePlayer(int actorId) => CommConnectionManager.CommCenter.SendMethodToPlayer(actorId, (byte) 30, (object) false);

  public void SendUpdateClanMembers(IEnumerable<ClanMemberView> list)
  {
    List<int> intList = new List<int>();
    foreach (ClanMemberView clanMemberView in list)
      intList.Add(clanMemberView.Cmid);
    intList.RemoveAll((Predicate<int>) (id => id == PlayerDataManager.CmidSecure));
    CommConnectionManager.CommCenter.SendMethodToServer((byte) 40, (object) intList);
  }

  public void SendRefreshClanData(int cmid) => CommConnectionManager.CommCenter.SendMethodToServer((byte) 53, (object) cmid);

  public void SendContactList()
  {
    HashSet<int> intSet = new HashSet<int>();
    if (Singleton<PlayerDataManager>.Instance.FriendList != null)
    {
      foreach (PublicProfileView friend in Singleton<PlayerDataManager>.Instance.FriendList)
        intSet.Add(friend.Cmid);
    }
    if (Singleton<PlayerDataManager>.Instance.ClanMembers != null)
    {
      foreach (ClanMemberView clanMember in Singleton<PlayerDataManager>.Instance.ClanMembers)
        intSet.Add(clanMember.Cmid);
    }
    foreach (CommUser commUser in Singleton<ChatManager>.Instance._modUsers.Values)
      intSet.Add(commUser.Cmid);
    foreach (CommUser otherUser in (IEnumerable<CommUser>) Singleton<ChatManager>.Instance.OtherUsers)
      intSet.Add(otherUser.Cmid);
    this._totalContactsCount = intSet.Count;
    if (this._totalContactsCount <= 0)
      return;
    this.SendMethodToServer((byte) 43, (object) PlayerDataManager.CmidSecure, (object) intSet);
  }

  public void UpdateContacts()
  {
    if (this._totalContactsCount <= 0)
      return;
    this.SendMethodToServer((byte) 44, (object) PlayerDataManager.CmidSecure);
  }

  public void SendUpdateResetLobby()
  {
    this._actors.Clear();
    this._actors.Add(this.MyInfo);
    Singleton<ChatManager>.Instance.RefreshAll();
    this.SendMethodToServer((byte) 4, (object) this._rmi.Messenger.PeerListener.ActorIdSecure);
  }

  public void SendUpdateAllPlayers() => this.SendMethodToServer((byte) 47, (object) this._rmi.Messenger.PeerListener.ActorIdSecure);

  public void SendSpeedhackDetection(List<float> timeDifference) => this.SendMethodToServer((byte) 50, (object) PlayerDataManager.CmidSecure, (object) timeDifference);

  public void UpdateActorsForModeration() => this.SendMethodToServer((byte) 46, (object) this._rmi.Messenger.PeerListener.ActorIdSecure);

  public void SendClanChatMessage(int cmid, int playerId, string name, string message)
  {
    message = this.CleanupChatMessage(message);
    if (string.IsNullOrEmpty(message))
      return;
    this.SendMethodToPlayer(playerId, (byte) 42, (object) cmid, (object) playerId, (object) name, (object) message);
  }

  public bool SendLobbyChatMessage(string message)
  {
    bool flag = this.CheckSpamFilter(message);
    message = this.CleanupChatMessage(message);
    if (!flag || string.IsNullOrEmpty(message))
      return false;
    this.OnLobbyChatMessage(PlayerDataManager.CmidSecure, this._rmi.Messenger.PeerListener.ActorIdSecure, PlayerDataManager.NameSecure, message);
    this.SendMethodToServer((byte) 25, (object) this._rmi.Messenger.PeerListener.ActorIdSecure, (object) message);
    return true;
  }

  public bool SendInGameChatMessage(string message, ChatContext context)
  {
    bool flag = this.CheckSpamFilter(message);
    message = this.CleanupChatMessage(message);
    if (flag && !string.IsNullOrEmpty(message))
    {
      this.OnInGameChatMessage(PlayerDataManager.CmidSecure, this._rmi.Messenger.PeerListener.ActorIdSecure, PlayerDataManager.NameSecure, message, (byte) PlayerDataManager.AccessLevelSecure, (byte) ChatManager.CurrentChatContext);
      this.SendMethodToServer((byte) 23, (object) this._rmi.Messenger.PeerListener.ActorIdSecure, (object) message, (object) (byte) ChatManager.CurrentChatContext);
    }
    return flag;
  }

  public void SendPrivateChatMessage(CommActorInfo info, string message)
  {
    message = this.CleanupChatMessage(message);
    if (string.IsNullOrEmpty(message))
      return;
    int cmidSecure = PlayerDataManager.CmidSecure;
    InstantMessage msg = new InstantMessage(cmidSecure, this._rmi.Messenger.PeerListener.ActorIdSecure, PlayerDataManager.NameSecure, message, PlayerDataManager.AccessLevelSecure);
    Singleton<ChatManager>.Instance.AddNewPrivateMessage(info.Cmid, msg);
    this.SendMethodToServer((byte) 24, (object) this._rmi.Messenger.PeerListener.ActorIdSecure, (object) info.ActorId, (object) message);
    PlayerChatLog.AddOutgoingPrivateMessage(cmidSecure, info.PlayerName, message, PlayerChatLog.ChatContextType.Private);
  }

  private bool CheckSpamFilter(string message)
  {
    bool flag1 = false;
    bool flag2 = false;
    float num1 = 0.0f;
    float num2 = 0.0f;
    int num3 = 0;
    string str = string.Empty;
    foreach (ClientCommCenter.Message lastMessage in this._lastMessages)
    {
      if ((double) lastMessage.Time + 5.0 > (double) Time.time)
      {
        if (message.StartsWith(lastMessage.Text, StringComparison.InvariantCultureIgnoreCase))
        {
          lastMessage.Time = Time.time;
          ++lastMessage.Count;
          flag1 = lastMessage.Count > 1;
          flag2 = true;
        }
        if ((double) num2 != 0.0)
        {
          num1 += Mathf.Clamp((float) (1.0 - ((double) lastMessage.Time - (double) num2)), 0.0f, 1f);
          ++num3;
        }
      }
      num2 = lastMessage.Time;
      str = lastMessage.Text;
    }
    if (!flag2)
      this._lastMessages.Enqueue(new ClientCommCenter.Message(message));
    if (message.Equals(str, StringComparison.InvariantCultureIgnoreCase) && (double) num2 + 10.0 > (double) Time.time)
      flag1 = true;
    if (num3 > 0)
      num1 /= (float) num3;
    return !(flag1 | (double) num1 > 0.30000001192092896);
  }

  private string CleanupChatMessage(string msg) => TextUtilities.ShortenText(TextUtilities.Trim(msg), 140, false);

  public void SendPrivateGameInvitation(CommActorInfo info, string message, CmuneRoomID roomId) => this.SendMethodToPlayer(info.ActorId, (byte) 35, (object) this._rmi.Messenger.PeerListener.ActorIdSecure, (object) message, (object) roomId);

  public void SendPlayerReport(int[] players, MemberReportType type, string details)
  {
    string messagesForPlayerReport = Singleton<ChatManager>.Instance.GetAllChatMessagesForPlayerReport();
    this.SendMethodToServer((byte) 28, (object) PlayerDataManager.CmidSecure, (object) players, (object) (int) type, (object) details, (object) messagesForPlayerReport);
  }

  public void SendClearAllFlags(int cmid) => this.SendMethodToServer((byte) 48, (object) cmid);

  public bool TryGetActorWithCmid(int cmid, out CommActorInfo info) => this._actors.TryGetByCmid(cmid, out info);

  public bool HasActorWithCmid(int cmid)
  {
    CommActorInfo actor;
    return this._actors.TryGetByCmid(cmid, out actor) && actor != null;
  }

  public void UpdateInboxRequest(int actorId) => this.SendMethodToPlayer(actorId, (byte) 37);

  public void NotifyFriendUpdate(int cmid) => this.SendMethodToServer((byte) 38, (object) cmid);

  public void MessageSentWithId(int messageId, int cmid) => this.SendMethodToServer((byte) 39, (object) cmid, (object) messageId);

  public void SendGetPlayersWithMatchingName(string str)
  {
    this._actors.Clear();
    Singleton<ChatManager>.Instance.RefreshAll();
    this.SendMethodToServer((byte) 52, (object) PlayerDataManager.CmidSecure, (object) str);
  }

  public IEnumerable<CommActorInfo> Players => this._actors.Actors;

  public int PlayerCount => this._actors.ActorCount;

  public CommActorInfo MyInfo => this._myInfo;

  public static bool IsPlayerMuted
  {
    get => ClientCommCenter._isPlayerMuted;
    private set => ClientCommCenter._isPlayerMuted = value;
  }

  private class ActorList
  {
    private Dictionary<int, CommActorInfo> _actorsByCmid = new Dictionary<int, CommActorInfo>();
    private Dictionary<int, int> _cmidByActorId = new Dictionary<int, int>();

    public void Add(CommActorInfo actor)
    {
      if (actor == null || actor.Cmid <= 0 || actor.ActorId <= 0)
        return;
      if (this._cmidByActorId.ContainsKey(actor.ActorId))
      {
        this._actorsByCmid.Remove(this._cmidByActorId[actor.ActorId]);
        this._cmidByActorId.Remove(actor.ActorId);
      }
      if (this._actorsByCmid.ContainsKey(actor.Cmid))
      {
        this._cmidByActorId.Remove(this._actorsByCmid[actor.Cmid].ActorId);
        this._actorsByCmid.Remove(actor.Cmid);
      }
      this._actorsByCmid.Add(actor.Cmid, actor);
      this._cmidByActorId.Add(actor.ActorId, actor.Cmid);
    }

    public void Remove(CommActorInfo actor)
    {
      this._actorsByCmid.Remove(actor.Cmid);
      this._cmidByActorId.Remove(actor.ActorId);
    }

    public bool TryGetByActorId(int actorId, out CommActorInfo actor)
    {
      actor = (CommActorInfo) null;
      int cmid;
      return this._cmidByActorId.TryGetValue(actorId, out cmid) && this.TryGetByCmid(cmid, out actor);
    }

    public bool TryGetByCmid(int cmid, out CommActorInfo actor)
    {
      actor = (CommActorInfo) null;
      return cmid > 0 && this._actorsByCmid.TryGetValue(cmid, out actor) && actor != null;
    }

    public void Clear()
    {
      this._actorsByCmid.Clear();
      this._cmidByActorId.Clear();
    }

    public IEnumerable<CommActorInfo> Actors => (IEnumerable<CommActorInfo>) this._actorsByCmid.Values;

    public int ActorCount => this._actorsByCmid.Count;
  }

  private class Message
  {
    public float Time;
    public string Text;
    public int Count;

    public Message(string text)
    {
      this.Text = text;
      this.Time = Time.time;
    }
  }
}
