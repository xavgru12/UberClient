// Decompiled with JetBrains decompiler
// Type: ModerationPanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ModerationPanelGUI : PanelGuiBase
{
  private float _nextUpdate;
  private CommUser _selectedCommUser;
  private Vector2 _playerScroll = Vector2.zero;
  private Vector2 _moderationScroll = Vector2.zero;
  private Rect _rect;
  private List<ModerationPanelGUI.Moderation> _moderations;
  private string _message = string.Empty;
  private string _filterText = string.Empty;
  private int _banDurationIndex = 1;
  private ModerationPanelGUI.Actions _moderationSelection;
  private int _playerCount;

  private void Awake()
  {
    this._moderations = new List<ModerationPanelGUI.Moderation>();
    CmuneEventHandler.AddListener<LoginEvent>((Action<LoginEvent>) (ev => this.InitModerations(ev.AccessLevel)));
  }

  private void OnGUI()
  {
    this._rect = new Rect((float) (GUITools.ScreenHalfWidth - 320), (float) (GUITools.ScreenHalfHeight - 202), 640f, 404f);
    GUI.BeginGroup(this._rect, GUIContent.none, BlueStonez.window_standard_grey38);
    this.DrawModerationPanel();
    GUI.EndGroup();
  }

  public override void Show()
  {
    base.Show();
    this._moderationSelection = ModerationPanelGUI.Actions.NONE;
  }

  public override void Hide()
  {
    base.Hide();
    this._moderationSelection = ModerationPanelGUI.Actions.NONE;
    this._filterText = string.Empty;
  }

  public void SetSelectedUser(CommUser user)
  {
    if (user == null)
      return;
    this._selectedCommUser = user;
    this._filterText = user.Name;
  }

  private void InitModerations(MemberAccessLevel level)
  {
    if (level >= MemberAccessLevel.ChatModerator)
    {
      this._moderations.Add(new ModerationPanelGUI.Moderation(MemberAccessLevel.ChatModerator, ModerationPanelGUI.Actions.UNMUTE_PLAYER, "Unmute Player", "Player is un-muted and un-ghosted immediately", "Unmute player", new Action<ModerationPanelGUI.Moderation, Rect>(this.DrawModeration)));
      this._moderations.Add(new ModerationPanelGUI.Moderation(MemberAccessLevel.ChatModerator, ModerationPanelGUI.Actions.GHOST_PLAYER, "Ghost Player", "Chat messages from player only appear in their own chat window, but not the windows of other players.", "Ghost player", new Action<ModerationPanelGUI.Moderation, Rect>(this.DrawModeration), new GUIContent[4]
      {
        new GUIContent("1 min"),
        new GUIContent("5 min"),
        new GUIContent("30 min"),
        new GUIContent("6 hrs")
      }));
      this._moderations.Add(new ModerationPanelGUI.Moderation(MemberAccessLevel.ChatModerator, ModerationPanelGUI.Actions.MUTE_PLAYER, "Mute Player", "Chat messages from player do not appear in anyones chat window.", "Mute player", new Action<ModerationPanelGUI.Moderation, Rect>(this.DrawModeration), new GUIContent[4]
      {
        new GUIContent("1 min"),
        new GUIContent("5 min"),
        new GUIContent("30 min"),
        new GUIContent("6 hrs")
      }));
    }
    if (level >= MemberAccessLevel.JuniorModerator)
    {
      this._moderations.Add(new ModerationPanelGUI.Moderation(MemberAccessLevel.JuniorModerator, ModerationPanelGUI.Actions.SEND_MESSAGE, "Send Custom Message", "Popup a window on a player's screen with a message from an admin.", "Send custom message to player", new Action<ModerationPanelGUI.Moderation, Rect>(this.DrawSendMessage)));
      this._moderations.Add(new ModerationPanelGUI.Moderation(MemberAccessLevel.JuniorModerator, ModerationPanelGUI.Actions.KICK_FROM_GAME, "Kick from Game", "Player is removed from the game he is currently in and dumped on the home screen.", "Kick player from game", new Action<ModerationPanelGUI.Moderation, Rect>(this.DrawModeration)));
    }
    if (level >= MemberAccessLevel.SeniorModerator)
      this._moderations.Add(new ModerationPanelGUI.Moderation(MemberAccessLevel.SeniorModerator, ModerationPanelGUI.Actions.KICK_FROM_APP, "Kick from Application", "Player is disconnected from all realtime connections for the current session.", "Kick player from application", new Action<ModerationPanelGUI.Moderation, Rect>(this.DrawModeration)));
    if (level < MemberAccessLevel.Admin)
      return;
    this._moderations.Add(new ModerationPanelGUI.Moderation(MemberAccessLevel.Admin, ModerationPanelGUI.Actions.BAN_FROM_CMUNE, "PERMANENT BAN", "Player is disconnected from all realtime connections. Player is banned permanently from CMUNE and can't login again", "PERMANENT BAN", new Action<ModerationPanelGUI.Moderation, Rect>(this.DrawModeration)));
  }

  private void DrawModerationPanel()
  {
    GUI.skin = BlueStonez.Skin;
    GUI.depth = 3;
    GUI.Label(new Rect(0.0f, 0.0f, this._rect.width, 56f), "MODERATION DASHBOARD", BlueStonez.tab_strip);
    this.DoModerationDashboard(new Rect(10f, 55f, this._rect.width - 20f, (float) ((double) this._rect.height - 55.0 - 52.0)));
    if (PlayerDataManager.AccessLevel >= MemberAccessLevel.SeniorModerator)
    {
      GUI.enabled = (double) this._nextUpdate < (double) Time.time;
      if (GUITools.Button(new Rect(10f, (float) ((double) this._rect.height - 10.0 - 32.0), 150f, 32f), new GUIContent((double) this._nextUpdate >= (double) Time.time ? string.Format("Next Update ({0:N0})", (object) (float) ((double) this._nextUpdate - (double) Time.time)) : "GET ALL PLAYERS"), BlueStonez.buttondark_medium))
      {
        ChatPageGUI.IsCompleteLobbyLoaded = true;
        this._selectedCommUser = (CommUser) null;
        this._filterText = string.Empty;
        this._nextUpdate = Time.time + 10f;
        CommConnectionManager.CommCenter.SendUpdateAllPlayers();
      }
    }
    GUI.enabled = this._selectedCommUser != null && this._moderationSelection != ModerationPanelGUI.Actions.NONE;
    if (GUITools.Button(new Rect((float) ((double) this._rect.width - 120.0 - 140.0), (float) ((double) this._rect.height - 10.0 - 32.0), 140f, 32f), new GUIContent("APPLY ACTION!"), !GUI.enabled ? BlueStonez.button : BlueStonez.button_red))
      this.ApplyModeration();
    GUI.enabled = true;
    if (!GUITools.Button(new Rect((float) ((double) this._rect.width - 10.0 - 100.0), (float) ((double) this._rect.height - 10.0 - 32.0), 100f, 32f), new GUIContent("CLOSE"), BlueStonez.button))
      return;
    PanelManager.Instance.ClosePanel(PanelType.Moderation);
  }

  private void DoModerationDashboard(Rect position)
  {
    GUI.BeginGroup(position, GUIContent.none, BlueStonez.window_standard_grey38);
    float width = 200f;
    this.DoPlayerModeration(new Rect(20f + width, 10f, position.width - 30f - width, position.height - 20f));
    this.DoPlayerSelection(new Rect(10f, 10f, width, position.height - 20f));
    GUI.EndGroup();
  }

  private void DoPlayerSelection(Rect position)
  {
    GUI.BeginGroup(position);
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 18f), "SELECT PLAYER", BlueStonez.label_interparkbold_18pt_left);
    bool flag = !string.IsNullOrEmpty(this._filterText);
    GUI.SetNextControlName("Filter");
    this._filterText = GUI.TextField(new Rect(0.0f, 26f, !flag ? position.width : position.width - 26f, 24f), this._filterText, 20, BlueStonez.textField);
    if (!flag && GUI.GetNameOfFocusedControl() != "Filter")
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      if (GUI.Button(new Rect(7f, 32f, position.width, 24f), "Enter player name", BlueStonez.label_interparkmed_11pt_left))
        GUI.FocusControl("Filter");
      GUI.color = Color.white;
    }
    if (flag && GUI.Button(new Rect(position.width - 24f, 26f, 24f, 24f), "x", BlueStonez.panelquad_button))
    {
      this._filterText = string.Empty;
      GUIUtility.keyboardControl = 0;
    }
    string text = string.Format("PLAYERS ONLINE ({0})", (object) this._playerCount);
    GUI.Label(new Rect(0.0f, 52f, position.width, 25f), GUIContent.none, BlueStonez.box_grey50);
    GUI.Label(new Rect(10f, 52f, position.width, 25f), text, BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, 76f, position.width, position.height - 76f), GUIContent.none, BlueStonez.box_grey50);
    this._playerScroll = GUITools.BeginScrollView(new Rect(0.0f, 77f, position.width, position.height - 78f), this._playerScroll, new Rect(0.0f, 0.0f, position.width - 20f, (float) (this._playerCount * 20)));
    int num = 0;
    string lower = this._filterText.ToLower();
    foreach (CommUser commUser in !GameState.HasCurrentGame ? (IEnumerable<CommUser>) Singleton<ChatManager>.Instance.LobbyUsers : (IEnumerable<CommUser>) Singleton<ChatManager>.Instance.GameUsers)
    {
      if (string.IsNullOrEmpty(lower) || commUser.Name.ToLower().Contains(lower))
      {
        if ((num & 1) == 0)
          GUI.Label(new Rect(1f, (float) (num * 20), position.width - 2f, 20f), GUIContent.none, BlueStonez.box_grey38);
        if (this._selectedCommUser != null && this._selectedCommUser.Cmid == commUser.Cmid)
        {
          GUI.color = new Color(ColorScheme.UberStrikeBlue.r, ColorScheme.UberStrikeBlue.g, ColorScheme.UberStrikeBlue.b, 0.5f);
          GUI.Label(new Rect(1f, (float) (num * 20), position.width - 2f, 20f), GUIContent.none, BlueStonez.box_white);
          GUI.color = Color.white;
        }
        if (GUI.Button(new Rect(10f, (float) (num * 20), position.width, 20f), commUser.Name, BlueStonez.label_interparkmed_10pt_left))
          this._selectedCommUser = commUser;
        GUI.color = Color.white;
        ++num;
      }
    }
    this._playerCount = num;
    GUITools.EndScrollView();
    GUI.EndGroup();
  }

  private void DoPlayerModeration(Rect position)
  {
    int height = this._moderations.Count * 100;
    GUI.BeginGroup(position);
    GUI.Label(new Rect(0.0f, 0.0f, position.width, position.height), GUIContent.none, BlueStonez.box_grey50);
    this._moderationScroll = GUITools.BeginScrollView(new Rect(0.0f, 0.0f, position.width, position.height), this._moderationScroll, new Rect(0.0f, 1f, position.width - 20f, (float) height));
    int index = 0;
    int num = 0;
    for (; index < this._moderations.Count; ++index)
      this._moderations[index].Draw(this._moderations[index], new Rect(10f, (float) (num++ * 100), 360f, 100f));
    GUITools.EndScrollView();
    GUI.EndGroup();
  }

  private void DrawModeration(ModerationPanelGUI.Moderation moderation, Rect position)
  {
    GUI.BeginGroup(position);
    GUI.Label(new Rect(21f, 0.0f, position.width, 30f), moderation.Title, BlueStonez.label_interparkbold_13pt);
    GUI.Label(new Rect(0.0f, 30f, 356f, 40f), moderation.Content, BlueStonez.label_itemdescription);
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
    if (GUI.Toggle(new Rect(0.0f, 7f, position.width, 16f), moderation.Selected, GUIContent.none, BlueStonez.radiobutton) && !moderation.Selected)
    {
      moderation.Selected = true;
      this.SelectModeration(moderation.ID);
      switch (moderation.SubSelectionIndex)
      {
        case 0:
          this._banDurationIndex = 1;
          break;
        case 1:
          this._banDurationIndex = 5;
          break;
        case 2:
          this._banDurationIndex = 30;
          break;
        case 3:
          this._banDurationIndex = 360;
          break;
        default:
          this._banDurationIndex = 1;
          break;
      }
      GUIUtility.keyboardControl = 0;
    }
    if (moderation.SubSelection != null)
    {
      GUI.enabled = moderation.Selected;
      GUI.changed = false;
      if (moderation.Selected)
        moderation.SubSelectionIndex = UnityGUI.Toolbar(new Rect(0.0f, position.height - 25f, position.width, 20f), moderation.SubSelectionIndex, moderation.SubSelection, moderation.SubSelection.Length, BlueStonez.panelquad_toggle);
      else
        UnityGUI.Toolbar(new Rect(0.0f, position.height - 25f, position.width, 20f), -1, moderation.SubSelection, moderation.SubSelection.Length, BlueStonez.panelquad_toggle);
      if (GUI.changed)
      {
        switch (moderation.SubSelectionIndex)
        {
          case 0:
            this._banDurationIndex = 1;
            break;
          case 1:
            this._banDurationIndex = 5;
            break;
          case 2:
            this._banDurationIndex = 30;
            break;
          case 3:
            this._banDurationIndex = 360;
            break;
          default:
            this._banDurationIndex = 1;
            break;
        }
      }
      GUI.enabled = true;
    }
    GUI.EndGroup();
  }

  private void DrawSendMessage(ModerationPanelGUI.Moderation moderation, Rect position)
  {
    GUI.BeginGroup(position);
    GUI.Label(new Rect(21f, 0.0f, position.width, 30f), moderation.Title, BlueStonez.label_interparkbold_13pt);
    GUI.Label(new Rect(0.0f, 30f, 356f, 40f), moderation.Content, BlueStonez.label_itemdescription);
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
    GUI.enabled = this._moderationSelection == ModerationPanelGUI.Actions.SEND_MESSAGE;
    GUI.SetNextControlName("ModCustom");
    this._message = GUI.TextField(new Rect(0.0f, 70f, 250f, 24f), this._message, BlueStonez.textField);
    GUI.enabled = true;
    if (GUI.Toggle(new Rect(0.0f, 7f, position.width, 16f), moderation.Selected, GUIContent.none, BlueStonez.radiobutton) && !moderation.Selected)
    {
      moderation.Selected = true;
      this.SelectModeration(moderation.ID);
      GUI.FocusControl("ModCustom");
    }
    GUI.EndGroup();
  }

  private void SelectModeration(ModerationPanelGUI.Actions id)
  {
    this._moderationSelection = id;
    for (int index = 0; index < this._moderations.Count; ++index)
    {
      if (id != this._moderations[index].ID)
        this._moderations[index].Selected = false;
    }
  }

  private void ApplyModeration()
  {
    if (PlayerDataManager.AccessLevelSecure <= MemberAccessLevel.Default || !this._moderations.Exists((Predicate<ModerationPanelGUI.Moderation>) (m => m.ID == this._moderationSelection)))
      return;
    switch (this._moderationSelection)
    {
      case ModerationPanelGUI.Actions.UNMUTE_PLAYER:
        if (this._selectedCommUser.IsInGame)
        {
          ServerRequest.Run((MonoBehaviour) this, this._selectedCommUser.CurrentGame.Server, (byte) 25, (object) this._selectedCommUser.CurrentGame.Number, (object) this._selectedCommUser.ActorId, (object) false);
        }
        else
        {
          CommConnectionManager.CommCenter.SendGhostPlayer(this._selectedCommUser.Cmid, this._selectedCommUser.ActorId, 0);
          CommConnectionManager.CommCenter.SendUnmutePlayer(this._selectedCommUser.ActorId);
        }
        PopupSystem.ShowMessage("Action Executed", string.Format("The Player '{0}' was unmuted.", (object) this._selectedCommUser.Name));
        break;
      case ModerationPanelGUI.Actions.GHOST_PLAYER:
        if (!this._selectedCommUser.IsInGame)
          CommConnectionManager.CommCenter.SendGhostPlayer(this._selectedCommUser.Cmid, this._selectedCommUser.ActorId, this._banDurationIndex);
        PopupSystem.ShowMessage("Action Executed", string.Format("The Player '{0}' was ghosted for {1} minutes.", (object) this._selectedCommUser.Name, (object) this._banDurationIndex));
        break;
      case ModerationPanelGUI.Actions.MUTE_PLAYER:
        if (this._selectedCommUser.IsInGame)
          ServerRequest.Run((MonoBehaviour) this, this._selectedCommUser.CurrentGame.Server, (byte) 24, (object) this._selectedCommUser.CurrentGame.Number, (object) this._selectedCommUser.ActorId, (object) true);
        else
          CommConnectionManager.CommCenter.SendMutePlayer(this._selectedCommUser.Cmid, this._selectedCommUser.ActorId, this._banDurationIndex);
        PopupSystem.ShowMessage("Action Executed", string.Format("The Player '{0}' was muted for {1} minutes.", (object) this._selectedCommUser.Name, (object) this._banDurationIndex));
        break;
      case ModerationPanelGUI.Actions.SEND_MESSAGE:
        if (this._selectedCommUser.IsInGame)
          ServerRequest.Run((MonoBehaviour) this, this._selectedCommUser.CurrentGame.Server, (byte) 23, (object) this._selectedCommUser.CurrentGame.Number, (object) this._selectedCommUser.ActorId, (object) this._message);
        else
          CommConnectionManager.CommCenter.SendCustomMessage(this._selectedCommUser.ActorId, this._message);
        PopupSystem.ShowMessage("Action Executed", string.Format("The Message was sent to Player '{0}'", (object) this._selectedCommUser.Name));
        this._message = string.Empty;
        break;
      case ModerationPanelGUI.Actions.KICK_FROM_GAME:
        if (this._selectedCommUser.IsInGame)
        {
          ServerRequest.Run((MonoBehaviour) this, this._selectedCommUser.CurrentGame.Server, (byte) 22, (object) this._selectedCommUser.Cmid, (object) this._selectedCommUser.CurrentGame.Number, (object) 0);
          PopupSystem.ShowMessage("Action Executed", string.Format("The Player '{0}' was kicked out of his current game!", (object) this._selectedCommUser.Name));
          break;
        }
        PopupSystem.ShowMessage("Warning", string.Format("The Player '{0}' is currently not in a game!", (object) this._selectedCommUser.Name));
        break;
      case ModerationPanelGUI.Actions.KICK_FROM_APP:
        CommConnectionManager.CommCenter.SendModerationBanPlayer(this._selectedCommUser.Cmid);
        PopupSystem.ShowMessage("Action Executed", string.Format("The Player '{0}' was disconnected from all servers!", (object) this._selectedCommUser.Name));
        break;
      case ModerationPanelGUI.Actions.BAN_FROM_CMUNE:
        CommConnectionManager.CommCenter.SendModerationBanFromCmune(this._selectedCommUser.Cmid);
        PopupSystem.ShowMessage("Action Executed", string.Format("The Player '{0}' was banned from CMUNE!", (object) this._selectedCommUser.Name));
        break;
    }
    this._moderationSelection = ModerationPanelGUI.Actions.NONE;
    foreach (ModerationPanelGUI.Moderation moderation in this._moderations)
      moderation.Selected = false;
  }

  private enum Actions
  {
    NONE,
    UNMUTE_PLAYER,
    GHOST_PLAYER,
    MUTE_PLAYER,
    SEND_MESSAGE,
    KICK_FROM_GAME,
    KICK_FROM_APP,
    BAN_FROM_CMUNE,
  }

  private class Moderation
  {
    public Moderation(
      MemberAccessLevel level,
      ModerationPanelGUI.Actions id,
      string title,
      string context,
      string option,
      Action<ModerationPanelGUI.Moderation, Rect> draw)
      : this(level, id, title, context, option, draw, (GUIContent[]) null)
    {
    }

    public Moderation(
      MemberAccessLevel level,
      ModerationPanelGUI.Actions id,
      string title,
      string context,
      string option,
      Action<ModerationPanelGUI.Moderation, Rect> draw,
      GUIContent[] subselection)
    {
      this.Level = level;
      this.ID = id;
      this.Title = title;
      this.Content = context;
      this.Draw = draw;
      this.SubSelection = subselection;
    }

    public MemberAccessLevel Level { get; private set; }

    public ModerationPanelGUI.Actions ID { get; private set; }

    public string Title { get; private set; }

    public string Content { get; private set; }

    public string Option { get; private set; }

    public Action<ModerationPanelGUI.Moderation, Rect> Draw { get; private set; }

    public GUIContent[] SubSelection { get; private set; }

    public int SubSelectionIndex { get; set; }

    public bool Selected { get; set; }
  }
}
