// Decompiled with JetBrains decompiler
// Type: InboxPageGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.WebService.Unity;
using UnityEngine;

public class InboxPageGUI : MonoBehaviour
{
  private const int PanelHeight = 50;
  private const int TAB_MESSAGE = 0;
  private const int TAB_REQUEST = 1;
  [SerializeField]
  private Texture2D _sideTexture;
  [SerializeField]
  private Texture2D _newMessage;
  private int _threadWidth;
  private int _selectedTab;
  private GUIContent[] _tabContents;
  private int _threadViewWidth;
  private int _threadViewHeight;
  private Vector2 _threadScroll;
  private Vector2 _friendScroll;
  private Vector2 _requestScroll;
  private int _messageViewWidth;
  private int _messageViewHeight;
  private string _replyMessage = string.Empty;
  private string _searchMessage = string.Empty;
  private string _searchFriend = string.Empty;
  private int _friendWidth;
  private int _friendHeight;
  private int _requestWidth;
  private int _requestHeight;

  private void Start() => this._tabContents = new GUIContent[2]
  {
    new GUIContent(LocalizedStrings.MessagesCaps),
    new GUIContent(LocalizedStrings.RequestsCaps)
  };

  private void OnGUI()
  {
    GUI.depth = 11;
    GUI.skin = BlueStonez.Skin;
    Rect position = new Rect(0.0f, (float) GlobalUIRibbon.Instance.Height(), (float) Screen.width, (float) (Screen.height - GlobalUIRibbon.Instance.Height()));
    this._threadWidth = (int) position.width / 4;
    GUI.BeginGroup(position, BlueStonez.window_standard_grey38);
    GUI.enabled = PlayerDataManager.IsPlayerLoggedIn && this.IsNoPanelOpen();
    this.DrawInbox(new Rect(0.0f, 0.0f, position.width, position.height));
    GUI.enabled = true;
    GUI.EndGroup();
  }

  private void DrawInbox(Rect rect)
  {
    this.DoTitle(new Rect(1f, 0.0f, rect.width - 2f, 72f));
    switch (this._selectedTab)
    {
      case 0:
        this.DoToolbarMessage(new Rect(1f, 72f, rect.width - 2f, 40f));
        this.DoThreads(new Rect(1f, 112f, (float) this._threadWidth, rect.height - 112f));
        this.DoMessages(new Rect((float) this._threadWidth, 110f, rect.width - (float) this._threadWidth, rect.height - 112f));
        break;
      case 1:
        float num = Mathf.Max(Singleton<InboxManager>.Instance.NextRequestRefresh - Time.time, 0.0f);
        GUITools.PushGUIState();
        GUI.enabled &= (double) num == 0.0;
        if (GUITools.Button(new Rect(rect.width - 131f, 80f, 123f, 24f), new GUIContent(string.Format(LocalizedStrings.Refresh + " {0}", (double) num <= 0.0 ? (object) string.Empty : (object) ("(" + num.ToString("N0") + ")"))), BlueStonez.buttondark_medium))
          Singleton<InboxManager>.Instance.RefreshAllRequests();
        GUITools.PopGUIState();
        this.DoRequests(new Rect(0.0f, 112f, rect.width, rect.height - 112f));
        break;
    }
  }

  private void DoTitle(Rect rect)
  {
    GUI.BeginGroup(rect, BlueStonez.tab_strip_large);
    int num = UnityGUI.Toolbar(new Rect(1f, 32f, 508f, 40f), this._selectedTab, this._tabContents, this._tabContents.Length, BlueStonez.tab_large);
    if (GUI.changed)
    {
      GUI.changed = false;
      SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
    }
    if (num != this._selectedTab)
    {
      GUIUtility.keyboardControl = 0;
      this._selectedTab = num;
    }
    if (Singleton<InboxManager>.Instance.HasUnreadMessages)
      GUI.DrawTexture(new Rect(133f, 32f, 20f, 20f), (Texture) this._newMessage);
    if (Singleton<InboxManager>.Instance.HasUnreadRequests)
      GUI.DrawTexture(new Rect(311f, 32f, 20f, 20f), (Texture) this._newMessage);
    GUI.EndGroup();
  }

  private void DoToolbarMessage(Rect rect)
  {
    GUI.BeginGroup(rect);
    GUI.Label(new Rect(8f, 8f, 206f, 24f), string.Format(LocalizedStrings.YouHaveNNewMessages, (object) Singleton<InboxManager>.Instance._unreadMessageCount), BlueStonez.label_interparkbold_16pt_left);
    if (this._selectedTab == 0)
    {
      Rect position = new Rect(rect.width - 368f, 8f, 140f, 24f);
      GUI.SetNextControlName("SearchMessage");
      this._searchMessage = GUI.TextField(position, this._searchMessage, BlueStonez.textField);
      if (string.IsNullOrEmpty(this._searchMessage) && GUI.GetNameOfFocusedControl() != "SearchMessage")
      {
        GUI.color = new Color(1f, 1f, 1f, 0.3f);
        GUI.Label(position, " " + LocalizedStrings.SearchMessages, BlueStonez.label_interparkbold_11pt_left);
        GUI.color = Color.white;
      }
    }
    if (GUITools.Button(new Rect(rect.width - 224f, 8f, 106f, 24f), new GUIContent(LocalizedStrings.NewMessage), BlueStonez.buttondark_medium))
      PanelManager.Instance.OpenPanel(PanelType.SendMessage);
    float num = Mathf.Max(Singleton<InboxManager>.Instance.NextInboxRefresh - Time.time, 0.0f);
    GUITools.PushGUIState();
    GUI.enabled &= (double) num == 0.0;
    if (GUITools.Button(new Rect(rect.width - 114f, 8f, 106f, 24f), new GUIContent(string.Format(LocalizedStrings.CheckMail + " {0}", (double) num <= 0.0 ? (object) string.Empty : (object) ("(" + num.ToString("N0") + ")"))), BlueStonez.buttondark_medium))
      Singleton<InboxManager>.Instance.LoadNextPageThreads();
    GUITools.PopGUIState();
    GUI.EndGroup();
  }

  private bool IsNoPanelOpen() => !PanelManager.IsAnyPanelOpen;

  private void DoToolbarFriend(Rect rect)
  {
    GUI.BeginGroup(rect);
    if (GUITools.Button(new Rect(8f, 8f, 106f, 24f), new GUIContent(LocalizedStrings.AddFriends), BlueStonez.buttondark_medium))
      PanelManager.Instance.OpenPanel(PanelType.FriendRequest);
    GUI.SetNextControlName("SearchFriend");
    this._searchFriend = GUI.TextField(new Rect(rect.width - 258f, 8f, 123f, 24f), this._searchFriend, BlueStonez.textField);
    if (string.IsNullOrEmpty(this._searchFriend) && GUI.GetNameOfFocusedControl() != "SearchFriend")
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(new Rect(rect.width - 258f, 8f, 123f, 24f), " " + LocalizedStrings.Search, BlueStonez.label_interparkbold_11pt_left);
      GUI.color = Color.white;
    }
    float num = Mathf.Max(Singleton<CommsManager>.Instance.NextFriendsRefresh - Time.time, 0.0f);
    GUITools.PushGUIState();
    GUI.enabled &= (double) num == 0.0;
    if (GUITools.Button(new Rect(rect.width - 131f, 8f, 123f, 24f), new GUIContent(string.Format(LocalizedStrings.Refresh + " {0}", (double) num <= 0.0 ? (object) string.Empty : (object) ("(" + num.ToString("N0") + ")"))), BlueStonez.buttondark_medium))
      this.StartCoroutine(Singleton<CommsManager>.Instance.GetContactsByGroups());
    GUITools.PopGUIState();
    string text = string.Format(LocalizedStrings.YouHaveNFriends, (object) Singleton<PlayerDataManager>.Instance.FriendsCount);
    if (Singleton<PlayerDataManager>.Instance.FriendsCount == 0)
      text = LocalizedStrings.YouHaveNoFriends;
    else if (Singleton<PlayerDataManager>.Instance.FriendsCount == 1)
      text = LocalizedStrings.YouHaveOnlyOneFriend;
    GUI.Label(new Rect(0.0f, 0.0f, rect.width, rect.height), text, BlueStonez.label_interparkbold_16pt);
    GUI.EndGroup();
  }

  private void DoThreads(Rect rect)
  {
    rect = new Rect(rect.x + 8f, rect.y, rect.width - 8f, rect.height - 8f);
    GUI.Box(rect, GUIContent.none, BlueStonez.window);
    if (Singleton<InboxManager>.Instance.ThreadCount > 0)
    {
      Vector2 vector2 = GUITools.BeginScrollView(rect, this._threadScroll, new Rect(0.0f, 0.0f, (float) this._threadViewWidth, (float) this._threadViewHeight));
      bool flag = (double) vector2.y > (double) this._threadScroll.y;
      this._threadScroll = vector2;
      int num1 = 0;
      for (int index = 0; index < Singleton<InboxManager>.Instance.ThreadCount; ++index)
      {
        InboxThread allThread = Singleton<InboxManager>.Instance.AllThreads[index];
        if (string.IsNullOrEmpty(this._searchMessage) || allThread.Contains(this._searchMessage))
        {
          num1 = allThread.DrawThread(num1, this._threadViewWidth);
          GUI.Label(new Rect(4f, (float) num1, (float) this._threadViewWidth, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
        }
      }
      int num2;
      if (Singleton<InboxManager>.Instance.IsLoadingThreads)
      {
        GUI.Label(new Rect(0.0f, (float) num1, rect.width, 30f), "Loading threads...", BlueStonez.label_interparkmed_11pt);
        num2 = num1 + 30;
      }
      else
      {
        if (Singleton<InboxManager>.Instance.IsNoMoreThreads)
        {
          GUI.contentColor = Color.gray;
          GUI.Label(new Rect(0.0f, (float) num1, rect.width, 30f), "No more threads", BlueStonez.label_interparkmed_11pt);
          GUI.contentColor = Color.white;
        }
        num2 = num1 + 30;
        float num3 = Mathf.Max((float) num2 - rect.height, 0.0f);
        if (flag && (double) this._threadScroll.y >= (double) num3)
          Singleton<InboxManager>.Instance.LoadNextPageThreads();
      }
      this._threadViewHeight = num2;
      this._threadViewWidth = (double) this._threadViewHeight <= (double) rect.height ? (int) ((double) rect.width - 8.0) : (int) ((double) rect.width - 22.0);
      GUITools.EndScrollView();
    }
    else if (Singleton<InboxManager>.Instance.IsLoadingThreads)
      GUI.Label(rect, "Loading threads...", BlueStonez.label_interparkbold_13pt);
    else
      GUI.Label(rect, LocalizedStrings.Empty, BlueStonez.label_interparkmed_11pt);
  }

  private void DoMessages(Rect rect)
  {
    InboxThread current = InboxThread.Current;
    bool flag = current != null && current.IsAdmin;
    Rect position1 = new Rect(rect.x + 8f, rect.y + 2f, rect.width - 16f, rect.height - 8f);
    GUI.Box(position1, GUIContent.none, BlueStonez.box_grey50);
    string text = LocalizedStrings.NoConversationSelected;
    if (current != null)
    {
      text = string.Format(LocalizedStrings.BetweenYouAndN, (object) current.Name);
      if (GUI.Button(new Rect(position1.x + 10f, position1.y + 10f, 150f, 20f), "Delete Conversation", BlueStonez.buttondark_medium))
      {
        InboxThread.Current = (InboxThread) null;
        Singleton<InboxManager>.Instance.DeleteThread(current.ThreadId);
      }
    }
    GUI.contentColor = new Color(1f, 1f, 1f, 0.75f);
    GUI.Label(new Rect(position1.x + 10f, position1.y, position1.width - 20f, 40f), text, BlueStonez.label_interparkmed_11pt_right);
    GUI.contentColor = Color.white;
    GUI.Label(new Rect(position1.x + 4f, position1.y + 40f, position1.width - 8f, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
    int y = 8;
    Rect position2 = new Rect(position1.x + 8f, position1.y + 48f, position1.width - 8f, position1.height - (!flag ? 90f : 49f));
    if (InboxThread.Current != null)
    {
      current.Scroll = GUITools.BeginScrollView(position2, current.Scroll, new Rect(0.0f, 0.0f, (float) this._messageViewWidth, (float) this._messageViewHeight));
      int num = current.DrawMessageList(y, this._messageViewWidth, position2.height, current.Scroll.y);
      if ((double) num > (double) position2.height)
      {
        this._messageViewHeight = num;
        this._messageViewWidth = (int) ((double) position2.width - 22.0);
      }
      else
      {
        this._messageViewHeight = (int) position2.height;
        this._messageViewWidth = (int) position2.width - 8;
      }
      GUITools.EndScrollView();
    }
    else
      GUI.Label(position2, "Select a message thread", BlueStonez.label_interparkbold_13pt);
    if (flag)
      return;
    GUITools.PushGUIState();
    GUI.enabled &= InboxThread.Current != null;
    GUI.Box(new Rect(rect.x + 8f, (float) ((double) rect.y + (double) rect.height - 51.0), rect.width - 16f, 45f), GUIContent.none, BlueStonez.window_standard_grey38);
    this.DoReply(new Rect(rect.x, (float) ((double) rect.y + (double) rect.height - 51.0), rect.width, 45f));
    GUITools.PopGUIState();
  }

  private void DoReply(Rect rect)
  {
    Rect position = new Rect(rect.x + (float) (((double) rect.width - 420.0) / 2.0), rect.y + 12f, 420f, rect.height);
    GUI.BeginGroup(position);
    GUI.SetNextControlName("Reply Edit");
    this._replyMessage = GUI.TextField(new Rect(0.0f, 0.0f, position.width - 64f, 24f), this._replyMessage, 140, BlueStonez.textField);
    this._replyMessage = this._replyMessage.Trim('\n');
    if (GUI.GetNameOfFocusedControl().Equals("Reply Edit") && !string.IsNullOrEmpty(this._replyMessage) && Event.current.type == UnityEngine.EventType.KeyUp && Event.current.keyCode == KeyCode.Return)
      this.SendMessage();
    GUITools.PushGUIState();
    GUI.enabled &= !string.IsNullOrEmpty(this._replyMessage);
    if (GUITools.Button(new Rect(position.width - 64f, 0.0f, 64f, 24f), new GUIContent(LocalizedStrings.Reply), BlueStonez.buttondark_medium))
      this.SendMessage();
    GUITools.PopGUIState();
    GUI.EndGroup();
  }

  private void SendMessage()
  {
    if (InboxThread.Current == null)
      return;
    Singleton<InboxManager>.Instance.SendPrivateMessage(InboxThread.Current.ThreadId, InboxThread.Current.Name, this._replyMessage);
    this._replyMessage = string.Empty;
    GUIUtility.keyboardControl = 0;
  }

  private void DoRequests(Rect rect)
  {
    Rect position = new Rect(rect.x + 8f, rect.y, rect.width - 16f, rect.height - 8f);
    GUI.BeginGroup(position, BlueStonez.window);
    int top = 5;
    this._requestHeight = 180 + Singleton<InboxManager>.Instance._friendRequests.Count * 60 + Singleton<InboxManager>.Instance._incomingClanRequests.Count * 60 + Singleton<InboxManager>.Instance._outgoingClanRequests.Count * 60;
    this._requestWidth = (int) position.width - ((double) this._requestHeight <= (double) position.height ? 8 : 22);
    this._requestScroll = GUITools.BeginScrollView(new Rect(0.0f, (float) top, position.width, position.height), this._requestScroll, new Rect(0.0f, 0.0f, (float) this._requestWidth, (float) this._requestHeight));
    GUI.Box(new Rect(4f, 0.0f, (float) this._requestWidth, 50f), GUIContent.none, BlueStonez.box_grey38);
    GUI.Label(new Rect(14f, 0.0f, (float) (this._requestWidth - 10), 50f), string.Format(LocalizedStrings.FriendRequestsYouHaveNPendingRequests, (object) Singleton<InboxManager>.Instance._friendRequests.Count.ToString(), Singleton<InboxManager>.Instance._friendRequests.Count == 1 ? (object) string.Empty : (object) "s"), BlueStonez.label_interparkmed_18pt_left);
    int num1 = top + 50;
    for (int index = 0; index < Singleton<InboxManager>.Instance._friendRequests.Count; ++index)
    {
      this.DrawFriendRequestView(Singleton<InboxManager>.Instance._friendRequests[index], (float) num1, this._requestWidth);
      GUI.Label(new Rect(25f, (float) (num1 + Mathf.RoundToInt(9f)), 32f, 32f), (index + 1).ToString(), BlueStonez.label_interparkbold_32pt);
      num1 += 60;
    }
    GUI.Box(new Rect(4f, (float) num1, (float) this._requestWidth, 50f), GUIContent.none, BlueStonez.box_grey38);
    GUI.Label(new Rect(14f, (float) num1, (float) (this._requestWidth - 10), 50f), string.Format("Clan Requests - You have {0} incoming invite{1}", (object) Singleton<InboxManager>.Instance._incomingClanRequests.Count, Singleton<InboxManager>.Instance._incomingClanRequests.Count == 1 ? (object) string.Empty : (object) "s"), BlueStonez.label_interparkmed_18pt_left);
    int num2 = num1 + 55;
    for (int index = 0; index < Singleton<InboxManager>.Instance._incomingClanRequests.Count; ++index)
    {
      this.DrawIncomingClanInvitation(Singleton<InboxManager>.Instance._incomingClanRequests[index], num2, this._requestWidth);
      GUI.Label(new Rect(25f, (float) (num2 + Mathf.RoundToInt(9f)), 32f, 32f), (index + 1).ToString(), BlueStonez.label_interparkbold_32pt);
      num2 += 60;
    }
    GUI.Box(new Rect(4f, (float) num2, (float) this._requestWidth, 50f), GUIContent.none, BlueStonez.box_grey38);
    GUI.Label(new Rect(14f, (float) num2, (float) (this._requestWidth - 10), 50f), string.Format("Clan Requests - You have {0} outgoing invite{1}", (object) Singleton<InboxManager>.Instance._outgoingClanRequests.Count, Singleton<InboxManager>.Instance._outgoingClanRequests.Count == 1 ? (object) string.Empty : (object) "s"), BlueStonez.label_interparkmed_18pt_left);
    int y = num2 + 55;
    for (int index = 0; index < Singleton<InboxManager>.Instance._outgoingClanRequests.Count; ++index)
    {
      this.DrawOutgoingClanInvitation(Singleton<InboxManager>.Instance._outgoingClanRequests[index], y, this._requestWidth);
      GUI.Label(new Rect(25f, (float) (y + Mathf.RoundToInt(9f)), 32f, 32f), (index + 1).ToString(), BlueStonez.label_interparkbold_32pt);
      y += 60;
    }
    GUITools.EndScrollView();
    GUI.EndGroup();
  }

  public void DrawFriendRequestView(ContactRequestView request, float y, int width)
  {
    Rect position1 = new Rect(4f, y + 4f, (float) (width - 1), 50f);
    GUI.BeginGroup(position1);
    Rect position2 = new Rect(0.0f, 0.0f, position1.width, position1.height - 1f);
    if (GUI.enabled && position2.Contains(Event.current.mousePosition))
      GUI.Box(position2, GUIContent.none, BlueStonez.box_grey50);
    GUI.Label(new Rect(80f, 5f, position1.width - 250f, 20f), string.Format("{0}: {1}", (object) LocalizedStrings.FriendRequest, (object) request.InitiatorName), BlueStonez.label_interparkbold_13pt_left);
    GUI.Label(new Rect(80f, 30f, position1.width - 250f, 20f), "> " + request.InitiatorMessage, BlueStonez.label_interparkmed_11pt_left);
    if (GUITools.Button(new Rect((float) ((double) position1.width - 120.0 - 18.0), 5f, 60f, 20f), new GUIContent(LocalizedStrings.Accept), BlueStonez.buttondark_medium))
      Singleton<InboxManager>.Instance.AcceptContactRequest(request.RequestId);
    if (GUITools.Button(new Rect((float) ((double) position1.width - 50.0 - 18.0), 5f, 60f, 20f), new GUIContent(LocalizedStrings.Ignore), BlueStonez.buttondark_medium))
      Singleton<InboxManager>.Instance.DeclineContactRequest(request.RequestId);
    GUI.EndGroup();
    GUI.Label(new Rect(4f, (float) ((double) y + 50.0 + 8.0), (float) width, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
  }

  private void DrawIncomingClanInvitation(GroupInvitationView view, int y, int width)
  {
    Rect position1 = new Rect(4f, (float) (y + 4), (float) (width - 1), 50f);
    GUI.BeginGroup(position1);
    Rect position2 = new Rect(0.0f, 0.0f, position1.width, position1.height - 1f);
    if (GUI.enabled && position2.Contains(Event.current.mousePosition))
      GUI.Box(position2, GUIContent.none, BlueStonez.box_grey50);
    GUI.Label(new Rect(80f, 5f, position1.width - 250f, 20f), string.Format("{0}: {1}", (object) LocalizedStrings.ClanInvite, (object) view.GroupName), BlueStonez.label_interparkbold_13pt_left);
    GUI.Label(new Rect(80f, 30f, position1.width - 250f, 20f), "> " + view.Message, BlueStonez.label_interparkmed_11pt_left);
    if (GUITools.Button(new Rect((float) ((double) position1.width - 120.0 - 18.0), 5f, 60f, 20f), new GUIContent(LocalizedStrings.Accept), BlueStonez.buttondark_medium))
    {
      if (PlayerDataManager.IsPlayerInClan)
      {
        PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.YouAlreadyInClanMsg, PopupSystem.AlertType.OK);
      }
      else
      {
        int requestId = view.GroupInvitationId;
        PopupSystem.ShowMessage(LocalizedStrings.Accept, "Do you want to accept this clan invitation?", PopupSystem.AlertType.OKCancel, (Action) (() => Singleton<InboxManager>.Instance.AcceptClanRequest(requestId)), "Join", (Action) null, LocalizedStrings.Cancel, PopupSystem.ActionType.Positive);
      }
    }
    if (GUITools.Button(new Rect((float) ((double) position1.width - 50.0 - 18.0), 5f, 60f, 20f), new GUIContent(LocalizedStrings.Ignore), BlueStonez.buttondark_medium))
      Singleton<InboxManager>.Instance.DeclineClanRequest(view.GroupInvitationId);
    GUI.EndGroup();
    GUI.Label(new Rect(4f, (float) (y + 50 + 8), (float) width, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
  }

  private void DrawOutgoingClanInvitation(GroupInvitationView view, int y, int width)
  {
    Rect position1 = new Rect(4f, (float) (y + 4), (float) (width - 1), 50f);
    GUI.BeginGroup(position1);
    Rect position2 = new Rect(0.0f, 0.0f, position1.width, position1.height - 1f);
    if (GUI.enabled && position2.Contains(Event.current.mousePosition))
      GUI.Box(position2, GUIContent.none, BlueStonez.box_grey50);
    GUI.Label(new Rect(80f, 5f, position1.width - 250f, 20f), string.Format("You invited: {0}", (object) view.InviteeName), BlueStonez.label_interparkbold_13pt_left);
    GUI.Label(new Rect(80f, 30f, position1.width - 250f, 20f), "> " + view.Message, BlueStonez.label_interparkmed_11pt_left);
    if (GUITools.Button(new Rect(position1.width - 140f, 5f, 120f, 20f), new GUIContent(LocalizedStrings.CancelInvite), BlueStonez.buttondark_medium))
    {
      int groupInvitationId = view.GroupInvitationId;
      if (Singleton<InboxManager>.Instance._outgoingClanRequests.Remove(view))
        ClanWebServiceClient.CancelInvitation(groupInvitationId, PlayerDataManager.CmidSecure, (Action<int>) null, (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.")));
    }
    GUI.EndGroup();
    GUI.Label(new Rect(4f, (float) (y + 50 + 8), (float) width, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
  }
}
