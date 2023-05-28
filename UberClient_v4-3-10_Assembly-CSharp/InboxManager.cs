// Decompiled with JetBrains decompiler
// Type: InboxManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class InboxManager : Singleton<InboxManager>
{
  public int _unreadMessageCount;
  private Dictionary<int, InboxThread> _allThreads = new Dictionary<int, InboxThread>();
  private List<InboxThread> _sortedAllThreads = new List<InboxThread>();
  private int _curThreadsPageIndex;
  public List<ContactRequestView> _friendRequests = new List<ContactRequestView>();
  public List<GroupInvitationView> _incomingClanRequests = new List<GroupInvitationView>();
  public List<GroupInvitationView> _outgoingClanRequests = new List<GroupInvitationView>();

  private InboxManager()
  {
  }

  public bool IsInitialized { get; private set; }

  public IList<InboxThread> AllThreads => (IList<InboxThread>) this._sortedAllThreads;

  public int ThreadCount => this._sortedAllThreads.Count;

  public bool IsLoadingThreads { get; private set; }

  public bool IsNoMoreThreads { get; private set; }

  public float NextInboxRefresh { get; private set; }

  public float NextRequestRefresh { get; private set; }

  public void Initialize()
  {
    if (this.IsInitialized)
      return;
    this.IsInitialized = true;
    this.LoadNextPageThreads();
    this.RefreshAllRequests();
  }

  public void SendPrivateMessage(int cmidId, string name, string rawMessage)
  {
    string str = TextUtilities.ShortenText(TextUtilities.Trim(rawMessage), 140, false);
    if (string.IsNullOrEmpty(str))
      return;
    if (!this._allThreads.ContainsKey(cmidId))
    {
      InboxThread inboxThread = new InboxThread(new MessageThreadView()
      {
        HasNewMessages = false,
        ThreadName = name,
        LastMessagePreview = string.Empty,
        ThreadId = cmidId,
        LastUpdate = DateTime.Now,
        MessageCount = 0
      });
      this._allThreads.Add(inboxThread.ThreadId, inboxThread);
      this._sortedAllThreads.Add(inboxThread);
    }
    PrivateMessageWebServiceClient.SendMessage(PlayerDataManager.CmidSecure, cmidId, str, (Action<PrivateMessageView>) (pm => this.OnPrivateMessageSent(cmidId, pm)), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, LocalizedStrings.YourMessageHasNotBeenSent)));
  }

  public void UpdateNewMessageCount()
  {
    this._sortedAllThreads.Sort((Comparison<InboxThread>) ((t1, t2) => t2.LastMessageDateTime.CompareTo(t1.LastMessageDateTime)));
    this._unreadMessageCount = 0;
    foreach (InboxThread sortedAllThread in this._sortedAllThreads)
    {
      if (sortedAllThread.HasUnreadMessage)
        ++this._unreadMessageCount;
    }
  }

  public bool HasUnreadMessages => this._unreadMessageCount > 0;

  public bool HasUnreadRequests => this._incomingClanRequests.Count > 0 || this._friendRequests.Count > 0;

  public void RemoveFriend(int friendCmid)
  {
    Singleton<PlayerDataManager>.Instance.RemoveFriend(friendCmid);
    RelationshipWebServiceClient.DeleteContact(PlayerDataManager.CmidSecure, friendCmid, (Action<MemberOperationResult>) (ev =>
    {
      if (ev == MemberOperationResult.Ok)
      {
        CommConnectionManager.CommCenter.NotifyFriendUpdate(friendCmid);
        Singleton<CommsManager>.Instance.UpdateCommunicator();
      }
      else
        Debug.LogError((object) ("DeleteContact failed with: " + (object) ev));
    }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.")));
  }

  public void AcceptContactRequest(int requestId)
  {
    this._friendRequests.RemoveAll((Predicate<ContactRequestView>) (r => r.RequestId == requestId));
    RelationshipWebServiceClient.AcceptContactRequest(requestId, PlayerDataManager.CmidSecure, 1, (Action<ContactRequestAcceptView>) (view =>
    {
      if (view != null && view.ActionResult == 0 && view.Contact != null)
      {
        Singleton<PlayerDataManager>.Instance.AddFriend(view.Contact);
        CommConnectionManager.CommCenter.NotifyFriendUpdate(view.Contact.Cmid);
        Singleton<CommsManager>.Instance.UpdateCommunicator();
      }
      else
        PopupSystem.ShowMessage(LocalizedStrings.Clan, "Failed accepting friend request", PopupSystem.AlertType.OK);
    }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.")));
  }

  public void DeclineContactRequest(int requestId)
  {
    this._friendRequests.RemoveAll((Predicate<ContactRequestView>) (r => r.RequestId == requestId));
    RelationshipWebServiceClient.DeclineContactRequest(requestId, PlayerDataManager.CmidSecure, (Action<ContactRequestDeclineView>) (ev => { }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.")));
  }

  public void AcceptClanRequest(int requestId)
  {
    this._incomingClanRequests.RemoveAll((Predicate<GroupInvitationView>) (r => r.GroupInvitationId == requestId));
    ClanWebServiceClient.AcceptClanInvitation(requestId, PlayerDataManager.CmidSecure, (Action<ClanRequestAcceptView>) (ev =>
    {
      if (ev != null && ev.ActionResult == 0)
      {
        PopupSystem.ShowMessage(LocalizedStrings.Clan, LocalizedStrings.JoinClanSuccessMsg, PopupSystem.AlertType.OKCancel, (Action) (() => MenuPageManager.Instance.LoadPage(PageType.Clans)), "Go to Clans", (Action) null, "Not now", PopupSystem.ActionType.Positive);
        Singleton<ClanDataManager>.Instance.SetClanData(ev.ClanView);
        CommConnectionManager.CommCenter.SendUpdateClanMembers((IEnumerable<ClanMemberView>) Singleton<PlayerDataManager>.Instance.ClanMembers);
      }
      else
        PopupSystem.ShowMessage(LocalizedStrings.Clan, LocalizedStrings.JoinClanErrorMsg, PopupSystem.AlertType.OK);
    }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.")));
  }

  public void DeclineClanRequest(int requestId)
  {
    this._incomingClanRequests.RemoveAll((Predicate<GroupInvitationView>) (r => r.GroupInvitationId == requestId));
    ClanWebServiceClient.DeclineClanInvitation(requestId, PlayerDataManager.CmidSecure, (Action<ClanRequestDeclineView>) (ev => { }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.")));
  }

  internal void LoadNextPageThreads()
  {
    if (this.IsNoMoreThreads && (double) this.NextInboxRefresh - (double) Time.time >= 0.0)
      return;
    this.IsLoadingThreads = true;
    this.NextInboxRefresh = Time.time + 30f;
    PrivateMessageWebServiceClient.GetAllMessageThreadsForUser(PlayerDataManager.CmidSecure, this._curThreadsPageIndex, new Action<List<MessageThreadView>>(this.OnFinishLoadingNextPageThreads), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
  }

  private void OnFinishLoadingNextPageThreads(List<MessageThreadView> listView)
  {
    this.IsLoadingThreads = false;
    if (listView.Count > 0)
    {
      ++this._curThreadsPageIndex;
      this.OnGetThreads(listView);
      this.IsNoMoreThreads = false;
    }
    else
      this.IsNoMoreThreads = true;
  }

  internal void LoadMessagesForThread(InboxThread inboxThread, int pageIndex)
  {
    inboxThread.IsLoading = true;
    PrivateMessageWebServiceClient.GetThreadMessages(PlayerDataManager.CmidSecure, inboxThread.ThreadId, pageIndex, (Action<List<PrivateMessageView>>) (list =>
    {
      inboxThread.IsLoading = false;
      this.OnGetMessages(inboxThread.ThreadId, list);
    }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
  }

  private void OnGetThreads(List<MessageThreadView> threadView)
  {
    foreach (MessageThreadView messageThreadView in threadView)
    {
      InboxThread inboxThread;
      if (this._allThreads.TryGetValue(messageThreadView.ThreadId, out inboxThread))
      {
        inboxThread.UpdateThread(messageThreadView);
      }
      else
      {
        inboxThread = new InboxThread(messageThreadView);
        this._allThreads.Add(inboxThread.ThreadId, inboxThread);
        this._sortedAllThreads.Add(inboxThread);
      }
    }
    this.UpdateNewMessageCount();
  }

  private void OnGetMessages(int threadId, List<PrivateMessageView> messages)
  {
    InboxThread inboxThread;
    if (this._allThreads.TryGetValue(threadId, out inboxThread))
      inboxThread.AddMessages(messages);
    else
      Debug.LogError((object) ("Getting messages of non existing thread " + (object) threadId));
  }

  private void OnPrivateMessageSent(int threadId, PrivateMessageView privateMessage)
  {
    if (privateMessage != null)
    {
      CommConnectionManager.CommCenter.MessageSentWithId(privateMessage.PrivateMessageId, privateMessage.ToCmid);
      privateMessage.IsRead = true;
      this.AddMessageToThread(threadId, privateMessage);
    }
    else
    {
      Debug.LogError((object) "PrivateMessage sending failed");
      PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.YourMessageHasNotBeenSent);
    }
  }

  private void AddMessage(PrivateMessageView privateMessage)
  {
    if (privateMessage != null)
      this.AddMessageToThread(privateMessage.FromCmid, privateMessage);
    else
      Debug.LogError((object) "AddMessage called with NULL message");
  }

  private void AddMessageToThread(int threadId, PrivateMessageView privateMessage)
  {
    InboxThread inboxThread;
    if (!this._allThreads.TryGetValue(threadId, out inboxThread))
    {
      inboxThread = new InboxThread(new MessageThreadView()
      {
        ThreadName = privateMessage.FromName,
        ThreadId = threadId
      });
      this._allThreads.Add(inboxThread.ThreadId, inboxThread);
      this._sortedAllThreads.Add(inboxThread);
    }
    inboxThread.AddMessage(privateMessage);
    this.UpdateNewMessageCount();
  }

  internal void MarkThreadAsRead(int threadId)
  {
    PrivateMessageWebServiceClient.MarkThreadAsRead(PlayerDataManager.CmidSecure, threadId, (Action) (() => { }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
    this.UpdateNewMessageCount();
  }

  internal void DeleteThread(int threadId) => PrivateMessageWebServiceClient.DeleteThread(PlayerDataManager.CmidSecure, threadId, (Action) (() => this.OnDeleteThread(threadId)), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.")));

  private void OnDeleteThread(int threadId)
  {
    this._allThreads.Remove(threadId);
    this._sortedAllThreads.RemoveAll((Predicate<InboxThread>) (t => t.ThreadId == threadId));
    this.UpdateNewMessageCount();
  }

  internal void GetMessageWithId(int messageId) => PrivateMessageWebServiceClient.GetMessageWithId(messageId, PlayerDataManager.CmidSecure, new Action<PrivateMessageView>(this.AddMessage), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));

  internal void RefreshAllRequests()
  {
    this.NextRequestRefresh = Time.time + 30f;
    RelationshipWebServiceClient.GetContactRequests(PlayerDataManager.CmidSecure, new Action<List<ContactRequestView>>(this.OnGetContactRequests), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
    ClanWebServiceClient.GetAllGroupInvitations(PlayerDataManager.CmidSecure, 1, new Action<List<GroupInvitationView>>(this.OnGetAllGroupInvitations), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
    if (Singleton<PlayerDataManager>.Instance.RankInClan == GroupPosition.Member)
      return;
    ClanWebServiceClient.GetPendingGroupInvitations(PlayerDataManager.ClanIDSecure, new Action<List<GroupInvitationView>>(this.OnGetPendingGroupInvitations), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
  }

  private void OnGetContactRequests(List<ContactRequestView> requests)
  {
    this._friendRequests = requests;
    if (this._friendRequests.Count <= 0)
      return;
    SfxManager.Play2dAudioClip(GameAudio.NewRequest);
  }

  private void OnGetAllGroupInvitations(List<GroupInvitationView> requests)
  {
    this._incomingClanRequests = requests;
    if (this._incomingClanRequests.Count <= 0)
      return;
    SfxManager.Play2dAudioClip(GameAudio.NewRequest);
  }

  private void OnGetPendingGroupInvitations(List<GroupInvitationView> requests) => this._outgoingClanRequests = requests;
}
