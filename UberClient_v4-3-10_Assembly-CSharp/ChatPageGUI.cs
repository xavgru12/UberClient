// Decompiled with JetBrains decompiler
// Type: ChatPageGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ChatPageGUI : PageGUI
{
  private const float TitleHeight = 24f;
  private const int SEARCH_HEIGHT = 36;
  private const int TAB_WIDTH = 300;
  private const int CHAT_USER_HEIGHT = 24;
  private const int CHAT_TEXTFIELD_HEIGHT = 36;
  private const float CHECK_PASSWORD_DELAY = 2f;
  private Rect _mainRect;
  private Vector2 _dialogScroll;
  private int _lastMessageCount;
  public static bool IsCompleteLobbyLoaded;
  [SerializeField]
  private Texture2D _speedhackerIcon;
  private float _nextFullLobbyUpdate;
  private float _spammingNotificationTime;
  private float _nextNaughtyListUpdate;
  private bool _checkForPassword;
  private bool _isPasswordOk = true;
  private float _yPosition;
  private float _lastMessageSentTimer = 0.3f;
  private float _nextPasswordCheck;
  private string _inputedPassword = string.Empty;
  private string _currentChatMessage = string.Empty;
  private GameMetaData _game;
  private PopupMenu _playerMenu;
  private float _keyboardOffset;

  private void Awake()
  {
    this._playerMenu = new PopupMenu();
    this.IsOnGUIEnabled = true;
  }

  private void Start()
  {
    this._playerMenu.AddMenuItem("Add Friend", new Action<CommUser>(this.MenuCmdAddFriend), new PopupMenu.IsEnabledForUser(this.MenuChkAddFriend));
    this._playerMenu.AddMenuItem("Unfriend", new Action<CommUser>(this.MenuCmdRemoveFriend), new PopupMenu.IsEnabledForUser(this.MenuChkRemoveFriend));
    this._playerMenu.AddMenuItem(LocalizedStrings.PrivateChat, new Action<CommUser>(this.MenuCmdChat), new PopupMenu.IsEnabledForUser(this.MenuChkChat));
    this._playerMenu.AddMenuItem(LocalizedStrings.SendMessage, new Action<CommUser>(this.MenuCmdSendMessage), new PopupMenu.IsEnabledForUser(this.MenuChkSendMessage));
    this._playerMenu.AddMenuItem(LocalizedStrings.JoinGame, new Action<CommUser>(this.MenuCmdJoinGame), new PopupMenu.IsEnabledForUser(this.MenuChkJoinGame));
    this._playerMenu.AddMenuItem(LocalizedStrings.InviteToClan, new Action<CommUser>(this.MenuCmdInviteClan), new PopupMenu.IsEnabledForUser(this.MenuChkInviteClan));
    if (PlayerDataManager.AccessLevelSecure < MemberAccessLevel.SeniorModerator)
      return;
    this._playerMenu.AddMenuItem("MODERATE", new Action<CommUser>(this.MenuCmdModeratePlayer), (PopupMenu.IsEnabledForUser) (_param0 => true));
  }

  private void Update()
  {
    if ((double) this._lastMessageSentTimer < 0.30000001192092896)
      this._lastMessageSentTimer += Time.deltaTime;
    if ((double) this._yPosition < 0.0)
      this._yPosition = Mathf.Lerp(this._yPosition, 0.1f, Time.deltaTime * 8f);
    else
      this._yPosition = 0.0f;
  }

  private void OnGUI()
  {
    if (!this.IsOnGUIEnabled)
      return;
    GUI.skin = BlueStonez.Skin;
    GUI.depth = 9;
    this._mainRect = new Rect(0.0f, (float) GlobalUIRibbon.Instance.Height(), (float) Screen.width, (float) (Screen.height - GlobalUIRibbon.Instance.Height()));
    this.DrawGUI(this._mainRect);
    if (PopupMenu.Current == null)
      return;
    PopupMenu.Current.Draw();
  }

  public override void DrawGUI(Rect rect)
  {
    GUI.BeginGroup(rect, BlueStonez.window);
    if (CommConnectionManager.Client.ConnectionState == PhotonClient.ConnectionStatus.RUNNING)
    {
      this.DoTabs(new Rect(10f, 0.0f, 300f, 30f));
      if (Event.current.type == UnityEngine.EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        GUIUtility.keyboardControl = 0;
      Rect rect1 = new Rect(0.0f, 21f, 300f, rect.height - 21f);
      Rect rect2 = new Rect(299f, 0.0f, rect.width - 300f, 22f);
      Rect rect3 = new Rect(300f, 22f, rect.width - 300f, (float) ((double) rect.height - 22.0 - 36.0) - this._keyboardOffset);
      Rect rect4 = new Rect(299f, rect.height - 37f, (float) ((double) rect.width - 300.0 + 1.0), 37f);
      ChatGroupPanel commPane = Singleton<ChatManager>.Instance._commPanes[(int) ChatPageGUI.SelectedTab];
      switch (ChatPageGUI.SelectedTab)
      {
        case TabArea.Lobby:
          this.DoDialogFooter(rect4, commPane, Singleton<ChatManager>.Instance.LobbyDialog);
          this.DoLobbyCommPane(rect1, commPane);
          this.DoDialogHeader(rect2, Singleton<ChatManager>.Instance.LobbyDialog);
          this.DoDialog(rect3, commPane, Singleton<ChatManager>.Instance.LobbyDialog);
          break;
        case TabArea.Private:
          this.DoDialogFooter(rect4, commPane, Singleton<ChatManager>.Instance._selectedDialog);
          this.DrawCommPane(rect1, commPane);
          this.DoPrivateDialogHeader(rect2, Singleton<ChatManager>.Instance._selectedDialog);
          this.DoDialog(rect3, commPane, Singleton<ChatManager>.Instance._selectedDialog);
          break;
        case TabArea.Clan:
          this.DoDialogFooter(rect4, commPane, Singleton<ChatManager>.Instance.ClanDialog);
          this.DrawCommPane(rect1, commPane);
          this.DoDialogHeader(rect2, Singleton<ChatManager>.Instance.ClanDialog);
          this.DoDialog(rect3, commPane, Singleton<ChatManager>.Instance.ClanDialog);
          break;
        case TabArea.InGame:
          this.DoDialogFooter(rect4, commPane, Singleton<ChatManager>.Instance.InGameDialog);
          this.DrawCommPane(rect1, commPane);
          this.DoDialogHeader(rect2, Singleton<ChatManager>.Instance.InGameDialog);
          this.DoDialog(rect3, commPane, Singleton<ChatManager>.Instance.InGameDialog);
          break;
        case TabArea.Moderation:
          this.DoModeratorPaneFooter(rect4, commPane);
          this.DrawModeratorCommPane(rect1, commPane);
          this.DoDialogHeader(rect2, Singleton<ChatManager>.Instance.ModerationDialog);
          this.DoModeratorDialog(rect3, commPane);
          break;
      }
    }
    else
    {
      GUI.color = Color.gray;
      if (CommConnectionManager.Client.ConnectionState == PhotonClient.ConnectionStatus.STOPPED)
        GUI.Label(new Rect(0.0f, rect.height / 2f, rect.width, 20f), LocalizedStrings.ServerIsNotReachable, BlueStonez.label_interparkbold_11pt);
      else
        GUI.Label(new Rect(0.0f, rect.height / 2f, rect.width, 20f), LocalizedStrings.ConnectingToServer, BlueStonez.label_interparkbold_11pt);
      GUI.color = Color.white;
    }
    GUI.EndGroup();
    if (this._checkForPassword)
      this.PasswordCheck(new Rect((float) (Screen.width - 280) * 0.5f, (float) (Screen.height - 200) * 0.5f, 280f, 200f));
    GuiManager.DrawTooltip();
  }

  private void PasswordCheck(Rect position)
  {
    if (this._game == null)
      return;
    GUITools.PushGUIState();
    GUI.BeginGroup(position, GUIContent.none, BlueStonez.window);
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 56f), LocalizedStrings.EnterPassword, BlueStonez.tab_strip);
    GUI.Box(new Rect(16f, 55f, position.width - 32f, (float) ((double) position.height - 56.0 - 64.0)), GUIContent.none, BlueStonez.window_standard_grey38);
    GUI.SetNextControlName("@EnterPassword");
    Rect position1 = new Rect((float) (((double) position.width - 188.0) / 2.0), 80f, 188f, 24f);
    this._inputedPassword = GUI.PasswordField(position1, this._inputedPassword, '*', 18, BlueStonez.textField);
    this._inputedPassword = this._inputedPassword.Trim('\n');
    if (string.IsNullOrEmpty(this._inputedPassword) && GUI.GetNameOfFocusedControl() != "@EnterPassword")
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(position1, LocalizedStrings.TypePasswordHere, BlueStonez.label_interparkmed_11pt);
      GUI.color = Color.white;
    }
    GUI.enabled = (double) Time.time > (double) this._nextPasswordCheck;
    if (GUITools.Button(new Rect((float) ((double) position.width - 100.0 - 10.0), 152f, 100f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button) || Event.current.keyCode == KeyCode.Return && Event.current.type == UnityEngine.EventType.Layout && (double) Time.time > (double) this._nextPasswordCheck)
    {
      if (this._inputedPassword == this._game.Password)
      {
        this._checkForPassword = false;
        this._inputedPassword = string.Empty;
        this._isPasswordOk = true;
        Singleton<GameStateController>.Instance.JoinGame(this._game);
        this._game = (GameMetaData) null;
      }
      else
      {
        this._inputedPassword = string.Empty;
        this._isPasswordOk = false;
        this._nextPasswordCheck = Time.time + 2f;
      }
    }
    GUI.enabled = true;
    if (GUITools.Button(new Rect(10f, 152f, 100f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button))
    {
      this._isPasswordOk = true;
      this._checkForPassword = false;
      this._inputedPassword = string.Empty;
      this._game = (GameMetaData) null;
    }
    if (!this._isPasswordOk && string.IsNullOrEmpty(this._inputedPassword))
    {
      GUI.color = Color.red;
      GUI.Label(new Rect((float) (((double) position.width - 188.0) / 2.0), 110f, 188f, 24f), LocalizedStrings.PasswordIncorrect, BlueStonez.label_interparkbold_11pt);
      GUI.color = Color.white;
    }
    GUI.EndGroup();
    GUITools.PopGUIState();
  }

  public void JoinRoom(CmuneRoomID roomId) => GameConnectionManager.RequestRoomMetaData(roomId, new Action<int, GameMetaData>(this.OnRequestRoomMetaData));

  private void OnRequestRoomMetaData(int returncode, GameMetaData data)
  {
    MemberAccessLevel accessLevelSecure = PlayerDataManager.AccessLevelSecure;
    switch (returncode)
    {
      case 0:
        if (PlayerDataManager.AccessLevelSecure >= MemberAccessLevel.SeniorModerator)
        {
          this._game = (GameMetaData) null;
          this._checkForPassword = false;
          Singleton<GameStateController>.Instance.JoinGame(data);
          break;
        }
        if (JoinGameUtil.IsMobileChannel(this._playerMenu.SelectedUser.Channel) && !JoinGameUtil.IsMobileChannel(ApplicationDataManager.Channel))
        {
          PopupSystem.ShowMessage("Error Joining Game Server", "Sorry, only mobile players can join mobile game servers.");
          break;
        }
        if (data.IsFull)
        {
          PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.ThisGameIsFull, PopupSystem.AlertType.OK, (Action) null);
          break;
        }
        if (data.IsLevelAllowed(PlayerDataManager.PlayerLevelSecure))
        {
          if (data.IsPublic)
          {
            this._game = (GameMetaData) null;
            this._checkForPassword = false;
            Singleton<GameStateController>.Instance.JoinGame(data);
            break;
          }
          this._game = data;
          this._checkForPassword = true;
          break;
        }
        PopupSystem.ShowMessage(LocalizedStrings.Error, string.Format(LocalizedStrings.YouHaveToReachLevelNToJoinThisGame, (object) data.LevelMin));
        break;
      case 1:
        PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.ThisGameNoLongerExists, PopupSystem.AlertType.OK, (Action) null);
        break;
      case 2:
        PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.ServerIsNotReachable, PopupSystem.AlertType.OK, (Action) null);
        break;
    }
  }

  private int DoModeratorControlPanel(Rect rect, ChatGroupPanel pane)
  {
    if (PlayerDataManager.AccessLevel < MemberAccessLevel.JuniorModerator)
      return 0;
    int num1 = 0;
    bool flag1 = PlayerDataManager.AccessLevel >= MemberAccessLevel.JuniorModerator;
    bool flag2 = flag1 && ChatPageGUI.IsCompleteLobbyLoaded;
    rect = new Rect(rect.x, (float) ((double) rect.yMax - 36.0 - (!flag2 ? (!flag1 ? 0.0 : 30.0) : 60.0) - 1.0), rect.width, (float) (37 + (!flag2 ? (!flag1 ? 0 : 30) : 60)));
    GUI.BeginGroup(rect, GUIContent.none, BlueStonez.window_standard_grey38);
    if (flag1)
    {
      GUI.enabled = (double) this._nextNaughtyListUpdate < (double) Time.time;
      if (GUITools.Button(new Rect(6f, rect.height - 61f, (float) (((double) rect.width - 12.0) * 0.5), 26f), new GUIContent((double) this._nextNaughtyListUpdate >= (double) Time.time ? string.Format("Next Update ({0:N0})", (object) (float) ((double) this._nextNaughtyListUpdate - (double) Time.time)) : "Update Naughty List"), BlueStonez.buttondark_medium))
      {
        this._nextNaughtyListUpdate = Time.time + 10f;
        CommConnectionManager.CommCenter.UpdateActorsForModeration();
      }
      GUI.enabled = true;
      GUI.enabled = (double) this._nextNaughtyListUpdate < (double) Time.time;
      if (GUITools.Button(new Rect((float) (6.0 + ((double) rect.width - 12.0) * 0.5), rect.height - 61f, (float) (((double) rect.width - 12.0) * 0.5), 26f), new GUIContent((double) this._nextNaughtyListUpdate >= (double) Time.time ? string.Format("Next Update ({0:N0})", (object) (float) ((double) this._nextNaughtyListUpdate - (double) Time.time)) : "Unban Next 50"), BlueStonez.buttondark_medium))
      {
        List<CommUser> commUserList = new List<CommUser>((IEnumerable<CommUser>) Singleton<ChatManager>.Instance.NaughtyUsers);
        int num2 = 0;
        foreach (CommUser commUser in commUserList)
        {
          if (commUser.Name.StartsWith("Banned:"))
          {
            CommConnectionManager.CommCenter.SendClearAllFlags(commUser.Cmid);
            Singleton<ChatManager>.Instance._selectedCmid = 0;
            Singleton<ChatManager>.Instance._modUsers.Remove(commUser.Cmid);
            if (++num2 > 50)
              break;
          }
        }
      }
      GUI.enabled = true;
      num1 += !ChatPageGUI.IsCompleteLobbyLoaded ? 30 : 60;
    }
    bool flag3 = !string.IsNullOrEmpty(pane.SearchText);
    GUI.SetNextControlName("@ModSearch");
    GUI.changed = false;
    pane.SearchText = GUI.TextField(new Rect(6f, rect.height - 30f, rect.width - (!flag3 ? 12f : 37f), 24f), pane.SearchText, 20, BlueStonez.textField);
    if (!flag3 && GUI.GetNameOfFocusedControl() != "@ModSearch")
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(new Rect(12f, rect.height - 30f, rect.width - 20f, 24f), LocalizedStrings.Search, BlueStonez.label_interparkmed_10pt_left);
      GUI.color = Color.white;
    }
    if (flag3 && GUITools.Button(new Rect(rect.width - 28f, rect.height - 30f, 22f, 22f), new GUIContent("x"), BlueStonez.panelquad_button))
    {
      pane.SearchText = string.Empty;
      GUIUtility.keyboardControl = 0;
    }
    pane.SearchText = TextUtilities.Trim(pane.SearchText);
    GUI.EndGroup();
    return num1 + 36;
  }

  public void DrawCommPane(Rect rect, ChatGroupPanel pane)
  {
    GUI.BeginGroup(rect);
    bool enabled = GUI.enabled;
    GUI.enabled = ((GUI.enabled ? 1 : 0) & (PopupMenu.IsEnabled ? 0 : (!this._checkForPassword ? 1 : 0))) != 0;
    float height1 = rect.height;
    float height2 = Mathf.Max(height1, pane.ContentHeight);
    float vOffset = 0.0f;
    pane.Scroll = GUITools.BeginScrollView(new Rect(0.0f, 0.0f, rect.width, height1), pane.Scroll, new Rect(0.0f, 0.0f, rect.width - 17f, height2), useVertical: true);
    GUI.BeginGroup(new Rect(0.0f, 0.0f, rect.width, height1 + pane.Scroll.y));
    foreach (ChatGroup group in pane.Groups)
      vOffset += this.DrawPlayerGroup(group, vOffset, rect.width - 17f, pane.SearchText.ToLower());
    GUI.EndGroup();
    GUITools.EndScrollView();
    pane.ContentHeight = vOffset;
    GUI.enabled = enabled;
    GUI.EndGroup();
  }

  private void DoLobbyCommPane(Rect rect, ChatGroupPanel pane)
  {
    GUI.BeginGroup(rect);
    bool enabled = GUI.enabled;
    GUI.enabled = ((GUI.enabled ? 1 : 0) & (PopupMenu.IsEnabled ? 0 : (!this._checkForPassword ? 1 : 0))) != 0;
    int num1 = this.DoLobbyControlPanel(new Rect(0.0f, 0.0f, rect.width, rect.height), pane);
    float num2 = rect.height - (float) num1;
    float height = Mathf.Max(num2, pane.ContentHeight);
    float vOffset = 0.0f;
    pane.Scroll = GUITools.BeginScrollView(new Rect(0.0f, 0.0f, rect.width, num2), pane.Scroll, new Rect(0.0f, 0.0f, rect.width - 17f, height), useVertical: true);
    GUI.BeginGroup(new Rect(0.0f, 0.0f, rect.width, num2 + pane.Scroll.y));
    foreach (ChatGroup group in pane.Groups)
      vOffset += this.DrawPlayerGroup(group, vOffset, rect.width - 17f, pane.SearchText.ToLower());
    GUI.EndGroup();
    GUITools.EndScrollView();
    pane.ContentHeight = vOffset;
    GUI.enabled = enabled;
    GUI.EndGroup();
  }

  private void DrawModeratorCommPane(Rect rect, ChatGroupPanel pane)
  {
    GUI.BeginGroup(rect);
    bool enabled = GUI.enabled;
    GUI.enabled = ((GUI.enabled ? 1 : 0) & (PopupMenu.Current != null ? 0 : (!this._checkForPassword ? 1 : 0))) != 0;
    int num1 = this.DoModeratorControlPanel(new Rect(0.0f, 0.0f, rect.width, rect.height), pane);
    float num2 = rect.height - (float) num1;
    float height = Mathf.Max(num2, pane.ContentHeight);
    float vOffset = 0.0f;
    pane.Scroll = GUITools.BeginScrollView(new Rect(0.0f, 0.0f, rect.width, num2), pane.Scroll, new Rect(0.0f, 0.0f, rect.width - 17f, height), useVertical: true);
    GUI.BeginGroup(new Rect(0.0f, 0.0f, rect.width, num2 + pane.Scroll.y));
    foreach (ChatGroup group in pane.Groups)
    {
      vOffset += this.DrawPlayerGroup(group, vOffset, rect.width - 17f, pane.SearchText.ToLower(), true);
      if ((double) vOffset > (double) pane.Scroll.y + (double) num2)
        break;
    }
    GUI.EndGroup();
    GUITools.EndScrollView();
    pane.ContentHeight = vOffset;
    GUI.enabled = enabled;
    GUI.EndGroup();
  }

  private int DoLobbyControlPanel(Rect rect, ChatGroupPanel pane)
  {
    int num = 0;
    bool flag1 = PlayerDataManager.AccessLevel >= MemberAccessLevel.JuniorModerator;
    bool flag2 = flag1 && ChatPageGUI.IsCompleteLobbyLoaded;
    rect = new Rect(rect.x, (float) ((double) rect.yMax - 36.0 - (!flag2 ? (!flag1 ? 0.0 : 30.0) : 60.0) - 1.0), rect.width, (float) (37 + (!flag2 ? (!flag1 ? 0 : 30) : 60)));
    GUI.BeginGroup(rect, GUIContent.none, BlueStonez.window_standard_grey38);
    if (flag1)
    {
      GUI.enabled = (double) this._nextFullLobbyUpdate < (double) Time.time;
      if (flag2 && GUITools.Button(new Rect(6f, 5f, rect.width - 12f, 26f), new GUIContent("Reset Lobby"), BlueStonez.buttondark_medium))
      {
        ChatPageGUI.IsCompleteLobbyLoaded = false;
        this._nextFullLobbyUpdate = Time.time + 10f;
        CommConnectionManager.CommCenter.SendUpdateResetLobby();
      }
      if (GUITools.Button(new Rect(6f, rect.height - 61f, rect.width - 12f, 26f), new GUIContent((double) this._nextFullLobbyUpdate >= (double) Time.time ? string.Format("Next Update ({0:N0})", (object) (float) ((double) this._nextFullLobbyUpdate - (double) Time.time)) : "Get All Players "), BlueStonez.buttondark_medium))
      {
        ChatPageGUI.IsCompleteLobbyLoaded = true;
        this._nextFullLobbyUpdate = Time.time + 10f;
        CommConnectionManager.CommCenter.SendUpdateAllPlayers();
      }
      GUI.enabled = true;
      num += !ChatPageGUI.IsCompleteLobbyLoaded ? 30 : 60;
    }
    bool flag3 = !string.IsNullOrEmpty(pane.SearchText);
    GUI.SetNextControlName("@LobbySearch");
    GUI.changed = false;
    pane.SearchText = GUI.TextField(new Rect(6f, rect.height - 30f, rect.width - (!flag3 ? 12f : 37f), 24f), pane.SearchText, 20, BlueStonez.textField);
    if (!flag3 && GUI.GetNameOfFocusedControl() != "@LobbySearch")
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(new Rect(12f, rect.height - 30f, rect.width - 20f, 24f), LocalizedStrings.Search, BlueStonez.label_interparkmed_10pt_left);
      GUI.color = Color.white;
    }
    if (flag3 && GUITools.Button(new Rect(rect.width - 28f, rect.height - 30f, 22f, 22f), new GUIContent("x"), BlueStonez.panelquad_button))
    {
      pane.SearchText = string.Empty;
      GUIUtility.keyboardControl = 0;
    }
    pane.SearchText = TextUtilities.Trim(pane.SearchText);
    GUI.EndGroup();
    return num + 36;
  }

  public float DrawPlayerGroup(
    ChatGroup group,
    float vOffset,
    float width,
    string search,
    bool allowSelfSelection = false)
  {
    Rect position = new Rect(0.0f, vOffset, width, 24f);
    GUITools.PushGUIState();
    GUI.enabled &= !PopupMenu.IsEnabled;
    GUI.Label(position, GUIContent.none, BlueStonez.window_standard_grey38);
    if (group.Players != null)
      GUI.Label(position, group.Title + " (" + (object) group.Players.Count + ")", BlueStonez.label_interparkbold_11pt);
    GUITools.PopGUIState();
    vOffset += 24f;
    int num = 0;
    if (group.Players != null)
    {
      GUITools.PushGUIState();
      GUI.enabled &= !PopupMenu.IsEnabled;
      GUI.BeginGroup(new Rect(0.0f, vOffset, width, (float) (group.Players.Count * 24)));
      foreach (CommUser player in (IEnumerable<CommUser>) group.Players)
      {
        if (string.IsNullOrEmpty(search) || player.Name.ToLower().Contains(search))
          this.GroupDrawUser((float) (num++ * 24), width, player, allowSelfSelection);
      }
      GUI.EndGroup();
      GUITools.PopGUIState();
    }
    return 24f + (float) (group.Players.Count * 24);
  }

  private void DoTabs(Rect rect)
  {
    float width = Mathf.Floor(rect.width / (float) Singleton<ChatManager>.Instance.TabCounter);
    bool flag = false;
    int num1 = 0;
    if (GUI.Toggle(new Rect(rect.x + (float) num1 * width, rect.y, width, rect.height), ChatPageGUI.SelectedTab == TabArea.Lobby, LocalizedStrings.Lobby, BlueStonez.tab_medium) && ChatPageGUI.SelectedTab != TabArea.Lobby)
    {
      ChatPageGUI.SelectedTab = TabArea.Lobby;
      flag = true;
    }
    int num2 = num1 + 1;
    if (GUI.Toggle(new Rect(rect.x + (float) num2 * width, rect.y, width, rect.height), ChatPageGUI.SelectedTab == TabArea.Private, LocalizedStrings.Private, BlueStonez.tab_medium) && ChatPageGUI.SelectedTab != TabArea.Private)
    {
      ChatPageGUI.SelectedTab = TabArea.Private;
      flag = true;
      Singleton<ChatManager>.Instance.HasUnreadPrivateMessage = false;
    }
    if (Singleton<ChatManager>.Instance.HasUnreadPrivateMessage)
      GUI.DrawTexture(new Rect(rect.x + (float) num2 * width, rect.y + 1f, 18f, 18f), (Texture) CommunicatorIcons.NewInboxMessage);
    int num3 = num2 + 1;
    if (Singleton<ChatManager>.Instance.ShowTab(TabArea.Clan))
    {
      if (GUI.Toggle(new Rect(rect.x + (float) num3 * width, rect.y, width, rect.height), ChatPageGUI.SelectedTab == TabArea.Clan, LocalizedStrings.Clan, BlueStonez.tab_medium) && ChatPageGUI.SelectedTab != TabArea.Clan)
      {
        ChatPageGUI.SelectedTab = TabArea.Clan;
        flag = true;
        Singleton<ChatManager>.Instance.HasUnreadClanMessage = false;
      }
      if (PlayerDataManager.IsPlayerInClan && Singleton<ChatManager>.Instance.HasUnreadClanMessage)
        GUI.DrawTexture(new Rect(rect.x + (float) num3 * width, rect.y + 1f, 18f, 18f), (Texture) CommunicatorIcons.NewInboxMessage);
      ++num3;
    }
    if (Singleton<ChatManager>.Instance.ShowTab(TabArea.InGame))
    {
      if (GUI.Toggle(new Rect(rect.x + (float) num3 * width, rect.y, width, rect.height), ChatPageGUI.SelectedTab == TabArea.InGame, LocalizedStrings.Game, BlueStonez.tab_medium) && ChatPageGUI.SelectedTab != TabArea.InGame)
      {
        ChatPageGUI.SelectedTab = TabArea.InGame;
        this._currentChatMessage = string.Empty;
        flag = true;
      }
      ++num3;
    }
    if (Singleton<ChatManager>.Instance.ShowTab(TabArea.Moderation))
    {
      if (GUI.Toggle(new Rect(rect.x + (float) num3 * width, rect.y, width, rect.height), ChatPageGUI.SelectedTab == TabArea.Moderation, LocalizedStrings.Admin, BlueStonez.tab_medium) && ChatPageGUI.SelectedTab != TabArea.Moderation && PlayerDataManager.AccessLevelSecure > MemberAccessLevel.Default && PlayerDataManager.AccessLevelSecure <= MemberAccessLevel.Admin)
      {
        ChatPageGUI.SelectedTab = TabArea.Moderation;
        this._currentChatMessage = string.Empty;
        flag = true;
      }
      int num4 = num3 + 1;
    }
    if (!flag)
      return;
    this._currentChatMessage = string.Empty;
    PopupMenu.Hide();
    GUIUtility.keyboardControl = 0;
  }

  private void DoDialog(Rect rect, ChatGroupPanel pane, ChatDialog dialog)
  {
    if (dialog == null)
      return;
    dialog.CheckSize(rect);
    GUI.BeginGroup(new Rect(rect.x, rect.y + Mathf.Clamp(rect.height - dialog._heightCache, 0.0f, rect.height), rect.width, rect.height));
    int index = 0;
    float top = 0.0f;
    if (this._lastMessageCount != dialog._msgQueue.Count)
    {
      if (!Input.GetMouseButton(0))
        this._dialogScroll.y = float.MaxValue;
      this._lastMessageCount = dialog._msgQueue.Count;
    }
    this._dialogScroll = GUITools.BeginScrollView(new Rect(0.0f, 0.0f, dialog._frameSize.x, dialog._frameSize.y), this._dialogScroll, new Rect(0.0f, 0.0f, dialog._contentSize.x, dialog._contentSize.y));
    foreach (InstantMessage msg in dialog._msgQueue)
    {
      if (dialog.CanShow == null || dialog.CanShow(msg.Context))
      {
        if (index % 2 == 0)
          GUI.Label(new Rect(0.0f, top, dialog._contentSize.x - 1f, dialog._msgHeight[index]), GUIContent.none, BlueStonez.box_grey38);
        GUI.color = this.GetNameColor(msg);
        GUI.Label(new Rect(4f, top, dialog._contentSize.x - 8f, 20f), msg.PlayerName + ":", BlueStonez.label_interparkbold_11pt_left);
        GUI.color = new Color(0.9f, 0.9f, 0.9f);
        GUI.Label(new Rect(4f, top + 20f, dialog._contentSize.x - 8f, dialog._msgHeight[index] - 20f), msg.MessageText, BlueStonez.label_interparkmed_11pt_left);
        GUI.color = new Color(1f, 1f, 1f, 0.5f);
        GUI.Label(new Rect(4f, top, dialog._contentSize.x - 8f, 20f), msg.MessageDateTime, BlueStonez.label_interparkmed_10pt_right);
        GUI.color = Color.white;
        top += dialog._msgHeight[index];
        ++index;
      }
    }
    GUITools.EndScrollView();
    dialog._heightCache = top;
    GUI.EndGroup();
  }

  private void DoModeratorDialog(Rect rect, ChatGroupPanel pane)
  {
    if (PlayerDataManager.AccessLevel < MemberAccessLevel.SeniorModerator)
      return;
    GUI.BeginGroup(rect, GUIContent.none);
    CommUser commUser;
    if (Singleton<ChatManager>.Instance._modUsers.TryGetValue(Singleton<ChatManager>.Instance._selectedCmid, out commUser) && commUser != null)
    {
      GUI.TextField(new Rect(10f, 15f, rect.width, 20f), "Name: " + commUser.Name, BlueStonez.label_interparkbold_11pt_left);
      GUI.TextField(new Rect(10f, 37f, rect.width, 20f), "Cmid: " + (object) commUser.Cmid, BlueStonez.label_interparkmed_11pt_left);
      if (PlayerDataManager.AccessLevel == MemberAccessLevel.Admin)
      {
        if (Application.isWebPlayer)
          GUI.TextField(new Rect(10f, 52f, rect.width, 20f), "http://instrumentation.cmune.com/Members/SeeMember.aspx?cmid=" + (object) commUser.Cmid, BlueStonez.label_interparkbold_11pt_left);
        else if (GUITools.Button(new Rect(10f, 52f, 70f, 20f), new GUIContent("Open Profile"), BlueStonez.label_interparkmed_11pt_url))
          Application.OpenURL("http://instrumentation.cmune.com/Members/SeeMember.aspx?cmid=" + (object) commUser.Cmid);
      }
      float width = rect.width - 20f;
      GUI.BeginGroup(new Rect(10f, 80f, width, rect.height - 70f), GUIContent.none, BlueStonez.box_grey50);
      if (GUITools.Button(new Rect(5f, 5f, width - 10f, 20f), new GUIContent("Clear and Unban"), BlueStonez.buttondark_medium))
      {
        CommConnectionManager.CommCenter.SendClearAllFlags(commUser.Cmid);
        Singleton<ChatManager>.Instance._selectedCmid = 0;
        Singleton<ChatManager>.Instance._modUsers.Remove(commUser.Cmid);
      }
      int top = 40;
      if ((commUser.ModerationFlag & 4) != 0)
      {
        GUI.Label(new Rect(8f, (float) top, width - 10f, 20f), "- BANNED", BlueStonez.label_interparkbold_11pt_left);
        top += 20;
      }
      if ((commUser.ModerationFlag & 2) != 0)
      {
        GUI.Label(new Rect(8f, (float) top, width - 10f, 20f), "- Ghosted", BlueStonez.label_interparkmed_11pt_left);
        top += 20;
      }
      if ((commUser.ModerationFlag & 1) != 0)
      {
        GUI.Label(new Rect(8f, (float) top, width - 10f, 20f), "- Muted", BlueStonez.label_interparkmed_11pt_left);
        top += 20;
      }
      if ((commUser.ModerationFlag & 8) != 0)
      {
        GUI.Label(new Rect(8f, (float) top, width - 10f, 20f), "- Speed " + commUser.ModerationInfo, BlueStonez.label_interparkmed_11pt_left);
        top += 20;
      }
      if ((commUser.ModerationFlag & 16) != 0)
      {
        GUI.Label(new Rect(8f, (float) top, width - 10f, 20f), "- Spamming", BlueStonez.label_interparkmed_11pt_left);
        top += 20;
      }
      if ((commUser.ModerationFlag & 32) != 0)
      {
        GUI.Label(new Rect(8f, (float) top, width - 10f, 20f), "- CrudeLanguage", BlueStonez.label_interparkmed_11pt_left);
        top += 20;
      }
      GUI.Label(new Rect(8f, (float) (top + 20), width - 10f, 100f), commUser.ModerationInfo, BlueStonez.label_interparkmed_11pt_left);
      GUI.EndGroup();
    }
    else
      GUI.Label(new Rect(0.0f, rect.height / 2f, rect.width, 20f), "No user selected", BlueStonez.label_interparkmed_11pt);
    GUI.EndGroup();
  }

  private void DoDialogHeader(Rect rect, ChatDialog d)
  {
    GUI.Label(rect, GUIContent.none, BlueStonez.window_standard_grey38);
    GUI.Label(rect, d.Title, BlueStonez.label_interparkbold_11pt);
  }

  private void DoPrivateDialogHeader(Rect rect, ChatDialog d)
  {
    GUI.Label(rect, GUIContent.none, BlueStonez.window_standard_grey38);
    if (d != null && d.UserCmid > 0)
    {
      GUI.Label(rect, d.Title, BlueStonez.label_interparkbold_11pt);
      if (!GUITools.Button(new Rect((float) ((double) rect.x + (double) rect.width - 20.0), rect.y + 3f, 16f, 16f), new GUIContent("x"), BlueStonez.panelquad_button))
        return;
      Singleton<ChatManager>.Instance.RemoveDialog(d);
    }
    else
      GUI.Label(rect, LocalizedStrings.PrivateChat, BlueStonez.label_interparkbold_11pt);
  }

  private void DoModeratorPaneFooter(Rect rect, ChatGroupPanel pane)
  {
    GUI.BeginGroup(rect, BlueStonez.window_standard_grey38);
    CommUser user;
    if (Singleton<ChatManager>.Instance._selectedCmid > 0 && Singleton<ChatManager>.Instance.TryGetLobbyCommUser(Singleton<ChatManager>.Instance._selectedCmid, out user) && user != null)
    {
      if (GUITools.Button(new Rect(5f, 6f, rect.width - 10f, rect.height - 12f), new GUIContent("Moderate User"), BlueStonez.buttondark_medium))
      {
        ModerationPanelGUI moderationPanelGui = PanelManager.Instance.OpenPanel(PanelType.Moderation) as ModerationPanelGUI;
        if ((bool) (UnityEngine.Object) moderationPanelGui)
          moderationPanelGui.SetSelectedUser(user);
      }
    }
    else if (GUITools.Button(new Rect(5f, 6f, rect.width - 10f, rect.height - 12f), new GUIContent("Open Moderator"), BlueStonez.buttondark_medium))
      PanelManager.Instance.OpenPanel(PanelType.Moderation);
    GUI.EndGroup();
  }

  private void DoDialogFooter(Rect rect, ChatGroupPanel pane, ChatDialog dialog)
  {
    GUI.BeginGroup(rect, BlueStonez.window_standard_grey38);
    bool enabled = GUI.enabled;
    GUI.enabled = ((GUI.enabled ? 1 : 0) & (ClientCommCenter.IsPlayerMuted || dialog == null ? 0 : (dialog.CanChat ? 1 : 0))) != 0;
    if (ChatPageGUI.SelectedTab == TabArea.InGame)
      GUI.enabled = ((GUI.enabled ? 1 : 0) & (!GameState.HasCurrentGame ? 0 : (GameState.CurrentGame.IsGameStarted ? 1 : 0))) != 0;
    GUI.SetNextControlName("@CurrentChatMessage");
    this._currentChatMessage = GUI.TextField(new Rect(6f, 6f, rect.width - 60f, rect.height - 12f), this._currentChatMessage, 140, BlueStonez.textField);
    this._currentChatMessage = this._currentChatMessage.Trim('\n');
    if ((double) this._spammingNotificationTime > (double) Time.time)
    {
      GUI.color = Color.red;
      GUI.Label(new Rect(15f, 6f, rect.width - 66f, rect.height - 12f), LocalizedStrings.DontSpamTheLobbyChat, BlueStonez.label_interparkmed_10pt_left);
      GUI.color = Color.white;
    }
    else
    {
      string empty = string.Empty;
      string text = dialog == null || dialog.UserCmid <= 0 ? LocalizedStrings.EnterAMessageHere : (!dialog.CanChat ? dialog.UserName + LocalizedStrings.Offline : LocalizedStrings.EnterAMessageHere);
      if (string.IsNullOrEmpty(this._currentChatMessage) && GUI.GetNameOfFocusedControl() != "@CurrentChatMessage")
      {
        GUI.color = new Color(1f, 1f, 1f, 0.3f);
        GUI.Label(new Rect(10f, 6f, rect.width - 66f, rect.height - 12f), text, BlueStonez.label_interparkmed_10pt_left);
        GUI.color = Color.white;
      }
    }
    if ((GUITools.Button(new Rect(rect.width - 51f, 6f, 45f, rect.height - 12f), new GUIContent(LocalizedStrings.Send), BlueStonez.buttondark_small) || Event.current.keyCode == KeyCode.Return) && !ClientCommCenter.IsPlayerMuted && (double) this._lastMessageSentTimer > 0.28999999165534973)
    {
      this.SendChatMessage();
      GUI.FocusControl("@CurrentChatMessage");
    }
    GUI.enabled = enabled;
    GUI.EndGroup();
  }

  public static bool IsChatActive => GUI.GetNameOfFocusedControl() == "@CurrentChatMessage";

  private void GroupDrawUser(float vOffset, float width, CommUser user, bool allowSelfSelection = false)
  {
    int cmid = PlayerDataManager.Cmid;
    Rect rect = new Rect(3f, vOffset, width - 3f, 24f);
    if (Singleton<ChatManager>.Instance._selectedCmid == user.Cmid)
    {
      GUI.color = new Color(ColorScheme.UberStrikeBlue.r, ColorScheme.UberStrikeBlue.g, ColorScheme.UberStrikeBlue.b, 0.5f);
      GUI.Label(rect, GUIContent.none, BlueStonez.box_white);
      GUI.color = Color.white;
    }
    bool enabled = GUI.enabled;
    if (user.Cmid != cmid && this.CheckMouseClickIn(rect, 1))
    {
      this.SelectCommUser(user);
      this._playerMenu.Show(GUIUtility.GUIToScreenPoint(Event.current.mousePosition), user);
    }
    if (Singleton<MouseInput>.Instance.DoubleClick(rect) && user.Cmid != cmid && user.PresenceIndex != PresenceType.Offline && (ChatPageGUI.SelectedTab != TabArea.Private || Singleton<ChatManager>.Instance._selectedCmid != user.Cmid))
    {
      this.SelectCommUser(user);
      Singleton<ChatManager>.Instance.CreatePrivateChat(user.Cmid);
      Event.current.Use();
    }
    else
    {
      if ((allowSelfSelection || user.Cmid != cmid) && this.CheckMouseClickIn(new Rect(rect.x, rect.y, rect.width - 20f, rect.height)))
        this.SelectCommUser(user);
      GUI.Label(new Rect(10f, vOffset + 3f, 11.2f, 16f), ChatManager.GetPresenceIcon(user.PresenceIndex), GUIStyle.none);
      GUI.Label(new Rect(23f, vOffset + 3f, 16f, 16f), (Texture) UberstrikeIconsHelper.GetIconForChannel(user.Channel), GUIStyle.none);
      GUI.color = ColorScheme.ChatNameOtherUser;
      if (user.Cmid == PlayerDataManager.Cmid)
        GUI.color = ColorScheme.ChatNameCurrentUser;
      else if (user.IsFriend || user.IsClanMember)
        GUI.color = ColorScheme.ChatNameFriendsUser;
      else if (user.AccessLevel == MemberAccessLevel.Admin)
        GUI.color = ColorScheme.ChatNameAdminUser;
      else if (user.AccessLevel > MemberAccessLevel.Default)
        GUI.color = ColorScheme.ChatNameModeratorUser;
      GUI.Label(new Rect(44f, vOffset, width - 66f, 24f), user.Name, BlueStonez.label_interparkmed_10pt_left);
      GUI.color = Color.white;
      if (user.Cmid != cmid && GUI.Button(new Rect(rect.width - 17f, vOffset + 1f, 18f, 18f), GUIContent.none, BlueStonez.button_context))
      {
        this.SelectCommUser(user);
        this._playerMenu.Show(GUIUtility.GUIToScreenPoint(Event.current.mousePosition), user);
      }
      GUI.Box(rect.Expand(0, -1), GUIContent.none, BlueStonez.dropdown_list);
      ChatDialog chatDialog;
      if (ChatPageGUI.SelectedTab == TabArea.Private && Singleton<ChatManager>.Instance._dialogsByCmid.TryGetValue(user.Cmid, out chatDialog) && chatDialog != null && chatDialog.HasUnreadMessage && chatDialog != Singleton<ChatManager>.Instance._selectedDialog)
        GUI.Label(new Rect(rect.width - 50f, vOffset, 25f, 25f), (Texture) CommunicatorIcons.NewInboxMessage);
      if (PlayerDataManager.AccessLevel > MemberAccessLevel.Default && (user.ModerationFlag & 12) == 8)
        GUI.Label(new Rect(width - 50f, vOffset + 3f, 20f, 20f), (Texture) this._speedhackerIcon);
      GUI.enabled = enabled;
    }
  }

  private void SelectCommUser(CommUser user)
  {
    Singleton<ChatManager>.Instance._selectedCmid = user.Cmid;
    if (ChatPageGUI.SelectedTab != TabArea.Private)
      return;
    ChatDialog chatDialog;
    if (Singleton<ChatManager>.Instance._dialogsByCmid.TryGetValue(user.Cmid, out chatDialog))
      chatDialog.HasUnreadMessage = false;
    else
      chatDialog = Singleton<ChatManager>.Instance.AddNewDialog(user);
    Singleton<ChatManager>.Instance._selectedDialog = chatDialog;
    this._currentChatMessage = string.Empty;
  }

  private void SendChatMessage()
  {
    if (string.IsNullOrEmpty(this._currentChatMessage))
      return;
    this._dialogScroll.y = float.MaxValue;
    this._currentChatMessage = TextUtilities.ShortenText(TextUtilities.Trim(this._currentChatMessage), 140, false);
    switch (ChatPageGUI.SelectedTab)
    {
      case TabArea.Lobby:
        if (!CommConnectionManager.CommCenter.SendLobbyChatMessage(this._currentChatMessage))
        {
          this._spammingNotificationTime = Time.time + 5f;
          break;
        }
        break;
      case TabArea.Private:
        CommActorInfo actor;
        if (Singleton<ChatManager>.Instance._selectedDialog != null && CommConnectionManager.TryGetActor(Singleton<ChatManager>.Instance._selectedDialog.UserCmid, out actor))
        {
          CommConnectionManager.CommCenter.SendPrivateChatMessage(actor, this._currentChatMessage);
          break;
        }
        break;
      case TabArea.Clan:
        string nameSecure = PlayerDataManager.NameSecure;
        int cmidSecure = PlayerDataManager.CmidSecure;
        using (IEnumerator<CommUser> enumerator = Singleton<ChatManager>.Instance.ClanUsers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            CommUser current = enumerator.Current;
            if (current.IsOnline)
              CommConnectionManager.CommCenter.SendClanChatMessage(cmidSecure, current.ActorId, nameSecure, this._currentChatMessage);
          }
          break;
        }
      case TabArea.InGame:
        CommConnectionManager.CommCenter.SendInGameChatMessage(this._currentChatMessage, ChatContext.Player);
        break;
    }
    this._lastMessageSentTimer = 0.0f;
    this._currentChatMessage = string.Empty;
  }

  private Color GetNameColor(InstantMessage msg)
  {
    Color nameColor = ColorScheme.ChatNameOtherUser;
    if (msg.Cmid == PlayerDataManager.Cmid)
      nameColor = ColorScheme.ChatNameCurrentUser;
    else if (msg.IsFriend || msg.IsClan)
      nameColor = ColorScheme.ChatNameFriendsUser;
    else if (msg.AccessLevel == MemberAccessLevel.Admin)
      nameColor = ColorScheme.ChatNameAdminUser;
    else if (msg.AccessLevel > MemberAccessLevel.Default)
      nameColor = ColorScheme.ChatNameModeratorUser;
    return nameColor;
  }

  private bool CheckMouseClickIn(Rect rect, int mouse = 0) => InputManager.GetMouseButtonDown(mouse) && rect.Contains(Event.current.mousePosition);

  private void MenuCmdRemoveFriend(CommUser user)
  {
    if (user == null)
      return;
    int friendCmid = user.Cmid;
    PopupSystem.ShowMessage(LocalizedStrings.RemoveFriendCaps, string.Format(LocalizedStrings.DoYouReallyWantToRemoveNFromYourFriendsList, (object) user.Name), PopupSystem.AlertType.OKCancel, (Action) (() => Singleton<InboxManager>.Instance.RemoveFriend(friendCmid)), LocalizedStrings.Remove, (Action) null, LocalizedStrings.Cancel, PopupSystem.ActionType.Negative);
  }

  private void MenuCmdAddFriend(CommUser user)
  {
    if (user == null)
      return;
    FriendRequestPanelGUI friendRequestPanelGui = PanelManager.Instance.OpenPanel(PanelType.FriendRequest) as FriendRequestPanelGUI;
    if (!(bool) (UnityEngine.Object) friendRequestPanelGui)
      return;
    friendRequestPanelGui.SelectReceiver(user.Cmid, user.Name);
  }

  private void MenuCmdChat(CommUser user)
  {
    if (user == null)
      return;
    Singleton<ChatManager>.Instance.CreatePrivateChat(user.Cmid);
  }

  private void MenuCmdSendMessage(CommUser user)
  {
    if (user == null)
      return;
    SendMessagePanelGUI sendMessagePanelGui = PanelManager.Instance.OpenPanel(PanelType.SendMessage) as SendMessagePanelGUI;
    if (!(bool) (UnityEngine.Object) sendMessagePanelGui)
      return;
    sendMessagePanelGui.SelectReceiver(user.Cmid, user.Name);
  }

  private void MenuCmdJoinGame(CommUser user)
  {
    if (user == null || user.CurrentGame.IsEmpty)
      return;
    this.JoinRoom(user.CurrentGame);
  }

  private void MenuCmdInviteClan(CommUser user)
  {
    if (user == null)
      return;
    InviteToClanPanelGUI inviteToClanPanelGui = PanelManager.Instance.OpenPanel(PanelType.ClanRequest) as InviteToClanPanelGUI;
    if (!(bool) (UnityEngine.Object) inviteToClanPanelGui)
      return;
    inviteToClanPanelGui.SelectReceiver(user.Cmid, user.ShortName);
  }

  private void MenuCmdModeratePlayer(CommUser user)
  {
    CommActorInfo info;
    if (user == null || !CommConnectionManager.CommCenter.TryGetActorWithCmid(Singleton<ChatManager>.Instance._selectedCmid, out info) || info == null)
      return;
    ModerationPanelGUI moderationPanelGui = PanelManager.Instance.OpenPanel(PanelType.Moderation) as ModerationPanelGUI;
    if (!(bool) (UnityEngine.Object) moderationPanelGui)
      return;
    moderationPanelGui.SetSelectedUser(user);
  }

  private bool MenuChkAddFriend(CommUser user) => user != null && user.Cmid != PlayerDataManager.Cmid && user.AccessLevel <= PlayerDataManager.AccessLevel && !PlayerDataManager.IsFriend(user.Cmid);

  private bool MenuChkRemoveFriend(CommUser user) => user != null && user.Cmid != PlayerDataManager.Cmid && PlayerDataManager.IsFriend(user.Cmid);

  private bool MenuChkChat(CommUser user) => user != null && user.Cmid != PlayerDataManager.Cmid && user.IsOnline;

  private bool MenuChkSendMessage(CommUser user) => user != null && user.Cmid != PlayerDataManager.Cmid && !GameState.HasCurrentGame;

  private bool MenuChkJoinGame(CommUser user) => user != null && user.Cmid != PlayerDataManager.Cmid && user.IsOnline && !user.CurrentGame.IsEmpty && user.CurrentGame != CommConnectionManager.CurrentRoomID;

  private bool MenuChkInviteClan(CommUser user) => user != null && user.Cmid != PlayerDataManager.Cmid && user.AccessLevel <= PlayerDataManager.AccessLevel && PlayerDataManager.IsPlayerInClan && PlayerDataManager.CanInviteToClan && !PlayerDataManager.IsClanMember(user.Cmid);

  public static TabArea SelectedTab { get; set; }
}
