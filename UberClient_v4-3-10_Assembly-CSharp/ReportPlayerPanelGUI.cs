// Decompiled with JetBrains decompiler
// Type: ReportPlayerPanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ReportPlayerPanelGUI : PanelGuiBase
{
  private Rect _rect;
  private bool _isDropdownActive;
  private Vector2 _listScroll;
  private string[] _reportTypeTexts;
  private string[] _playerNameTexts;
  private int _selectedAbusion = -1;
  private string _reason = string.Empty;
  private string _abuse = string.Empty;
  private string _selectedPlayers = string.Empty;
  private string _searchPattern = string.Empty;
  private Vector2 _scrollUsers;
  private List<int> _reportedCmids = new List<int>();
  private string[] _currentActiveItems;
  private int _commUsersCount;
  private List<CommUser> _commUsers;

  private void Awake() => CmuneEventHandler.AddListener<MemberListUpdatedEvent>(new Action<MemberListUpdatedEvent>(this.OnMemberListUpdatedEvent));

  private void Start()
  {
    this._isDropdownActive = false;
    this._abuse = LocalizedStrings.SelectType;
    Array values = Enum.GetValues(typeof (MemberReportType));
    this._reportTypeTexts = new string[values.Length];
    int num1 = 0;
    foreach (int num2 in values)
    {
      MemberReportType memberReportType = (MemberReportType) num2;
      this._reportTypeTexts[num1++] = Enum.GetName(typeof (MemberReportType), (object) memberReportType);
    }
    this._playerNameTexts = new string[0];
  }

  private void OnEnable() => GUI.FocusControl("ReportDetail");

  private void OnGUI()
  {
    this._rect = new Rect((float) (Screen.width - 570) * 0.5f, (float) (Screen.height - 345) * 0.5f, 570f, 345f);
    GUI.BeginGroup(this._rect, GUIContent.none, BlueStonez.window_standard_grey38);
    this.DrawReportPanel();
    GUI.EndGroup();
  }

  public override void Show()
  {
    base.Show();
    this._commUsersCount = 0;
    this._commUsers = Singleton<ChatManager>.Instance.GetCommUsersToReport();
  }

  public override void Hide()
  {
    base.Hide();
    this._commUsers = (List<CommUser>) null;
    this._selectedAbusion = -1;
    this._abuse = LocalizedStrings.SelectType;
  }

  public static void ReportInboxPlayer(PrivateMessageView msg, string messageSender)
  {
    int reportedCmid = msg.FromCmid;
    string reason = msg.ContentText;
    if (CommConnectionManager.IsConnected)
      PopupSystem.ShowMessage(LocalizedStrings.ReportPlayerCaps, string.Format(LocalizedStrings.ReportPlayerWarningMsg, (object) messageSender), PopupSystem.AlertType.OKCancel, (Action) (() => CommConnectionManager.CommCenter.SendPlayerReport(new int[1]
      {
        reportedCmid
      }, MemberReportType.OffensiveChat, reason)), LocalizedStrings.Report, (Action) null, LocalizedStrings.Cancel, PopupSystem.ActionType.Negative);
    else
      PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.ReportPlayerErrorMsg, PopupSystem.AlertType.OK, (Action) null);
  }

  private void DrawReportPanel()
  {
    GUI.depth = 3;
    GUI.skin = BlueStonez.Skin;
    GUI.Label(new Rect(0.0f, 0.0f, this._rect.width, 56f), LocalizedStrings.ReportPlayerCaps, BlueStonez.tab_strip);
    GUI.color = Color.red;
    GUI.Label(new Rect(16f, this._rect.height - 40f, 300f, 30f), LocalizedStrings.ReportPlayerInfoMsg, BlueStonez.label_interparkbold_11pt_left_wrap);
    GUI.color = Color.white;
    GUI.BeginGroup(new Rect(17f, 55f, this._rect.width - 34f, this._rect.height - 100f), string.Empty, BlueStonez.window_standard_grey38);
    GUI.Label(new Rect(16f, 20f, 100f, 18f), LocalizedStrings.ReportType, BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(16f, 50f, 100f, 18f), LocalizedStrings.PlayerNames, BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(16f, 80f, 100f, 18f), LocalizedStrings.Details, BlueStonez.label_interparkbold_18pt_left);
    GUI.enabled = !this._isDropdownActive;
    GUI.SetNextControlName("ReportDetail");
    this._reason = GUI.TextArea(new Rect(16f, 110f, 290f, 120f), this._reason);
    GUI.Label(new Rect(125f, 50f, 180f, 22f), this._selectedPlayers, BlueStonez.textField);
    if (string.IsNullOrEmpty(this._selectedPlayers))
    {
      GUI.color = Color.gray;
      GUI.Label(new Rect(130f, 52f, 180f, 22f), "(" + LocalizedStrings.NoPlayerSelected + ")");
      GUI.color = Color.white;
    }
    GUI.enabled = true;
    int index = this.DoDropDownList(new Rect(125f, 20f, 183f, 22f), new Rect(135f, 50f, 194f, 84f), this._reportTypeTexts, ref this._abuse, false);
    if (index != -1)
    {
      this._selectedAbusion = index;
      this._abuse = this._reportTypeTexts[index];
    }
    GUI.SetNextControlName("SearchUser");
    this._searchPattern = GUI.TextField(new Rect(325f, 20f, 196f, 22f), this._searchPattern);
    if (string.IsNullOrEmpty(this._searchPattern) && GUI.GetNameOfFocusedControl() != "SearchUser")
    {
      GUI.color = Color.gray;
      GUI.Label(new Rect(333f, 22f, 196f, 22f), LocalizedStrings.SelectAPlayer);
      GUI.color = Color.white;
    }
    int num = 0;
    GUI.Label(new Rect(325f, 50f, 175f, 178f), GUIContent.none, BlueStonez.box_grey50);
    this._scrollUsers = GUITools.BeginScrollView(new Rect(325f, 50f, 195f, 178f), this._scrollUsers, new Rect(0.0f, 0.0f, 150f, (float) Mathf.Max(this._commUsersCount * 20, 178)), useVertical: true);
    if (this._commUsers != null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string lowerInvariant = this._searchPattern.ToLowerInvariant();
      foreach (CommUser commUser in this._commUsers)
      {
        bool flag = this._reportedCmids.Contains(commUser.Cmid);
        if (flag)
          stringBuilder.Append(commUser.Name).Append(", ");
        if (commUser.Name.ToLowerInvariant().Contains(lowerInvariant))
        {
          if (GUI.Toggle(new Rect(2f, (float) (2 + num * 20), 171f, 20f), flag, commUser.Name, BlueStonez.dropdown_listItem) != flag)
          {
            this._reportedCmids.Clear();
            if (!flag)
              this._reportedCmids.Add(commUser.Cmid);
          }
          ++num;
        }
      }
      this._commUsersCount = num;
      this._selectedPlayers = stringBuilder.ToString();
    }
    GUITools.EndScrollView();
    if (this._commUsersCount == 0)
      GUI.Label(new Rect(325f, 50f, 175f, 178f), LocalizedStrings.NoPlayersToReport, BlueStonez.label_interparkmed_11pt);
    else if (num == 0)
      GUI.Label(new Rect(325f, 50f, 175f, 178f), LocalizedStrings.NoMatchFound, BlueStonez.label_interparkmed_11pt);
    GUI.EndGroup();
    if (GUITools.Button(new Rect(this._rect.width - 125f, this._rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button))
    {
      PanelManager.Instance.ClosePanel(PanelType.ReportPlayer);
      this._commUsers = (List<CommUser>) null;
      this._reportedCmids.Clear();
      this._selectedPlayers = string.Empty;
      this._reason = string.Empty;
      this._selectedAbusion = -1;
    }
    GUI.enabled = this._selectedAbusion >= 0 && !string.IsNullOrEmpty(this._selectedPlayers) && !string.IsNullOrEmpty(this._reason);
    if (!GUITools.Button(new Rect((float) ((double) this._rect.width - 125.0 - 125.0), this._rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.SendCaps), BlueStonez.button_red))
      return;
    if (CommConnectionManager.IsConnected)
      PopupSystem.ShowMessage(LocalizedStrings.ReportPlayerCaps, string.Format(LocalizedStrings.ReportPlayerWarningMsg, (object) this._selectedPlayers), PopupSystem.AlertType.OKCancel, new Action(this.ConfirmAbuseReport), LocalizedStrings.Report, (Action) null, LocalizedStrings.Cancel, PopupSystem.ActionType.Negative);
    else
      PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.ReportPlayerErrorMsg, PopupSystem.AlertType.OK, (Action) null);
  }

  private void ConfirmAbuseReport()
  {
    CommConnectionManager.CommCenter.SendPlayerReport(this._reportedCmids.ToArray(), MemberReportType.OffensiveChat, this._reason);
    PanelManager.Instance.ClosePanel(PanelType.ReportPlayer);
    PopupSystem.ShowMessage(LocalizedStrings.ReportPlayerCaps, LocalizedStrings.ReportPlayerSuccessMsg, PopupSystem.AlertType.OK, (Action) null);
    this._reportedCmids.Clear();
    this._selectedPlayers = string.Empty;
    this._reason = string.Empty;
    this._selectedAbusion = -1;
  }

  private void DrawGroupControl(Rect rect, string title, GUIStyle style)
  {
    GUI.BeginGroup(rect, string.Empty, BlueStonez.group_grey81);
    GUI.EndGroup();
    GUI.Label(new Rect(rect.x + 18f, rect.y - 8f, style.CalcSize(new GUIContent(title)).x + 10f, 16f), title, style);
  }

  private int DoDropDownList(
    Rect position,
    Rect size,
    string[] items,
    ref string defaultText,
    bool canEdit)
  {
    int num = -1;
    Rect position1 = new Rect(position.x, position.y, position.width - position.height, position.height);
    Rect position2 = new Rect((float) ((double) position.x + (double) position.width - (double) position.height - 2.0), position.y - 1f, position.height, position.height);
    bool enabled = GUI.enabled;
    GUI.enabled = (!this._isDropdownActive ? 0 : (this._currentActiveItems != items ? 1 : 0)) == 0;
    if (canEdit)
      defaultText = GUI.TextField(new Rect(position.x, position.y, position.width - position.height, position.height - 1f), defaultText, BlueStonez.textArea);
    else
      GUI.Label(position1, defaultText, BlueStonez.label_dropdown);
    if (GUI.Button(position2, GUIContent.none, BlueStonez.dropdown_button))
    {
      this._isDropdownActive = !this._isDropdownActive;
      this._currentActiveItems = items;
    }
    if (this._isDropdownActive && this._currentActiveItems == items)
    {
      Rect position3 = new Rect(position.x, (float) ((double) position.y + (double) position.height - 1.0), size.width - 16f, size.height);
      GUI.Box(position3, string.Empty, BlueStonez.window_standard_grey38);
      this._listScroll = GUITools.BeginScrollView(position3, this._listScroll, new Rect(0.0f, 0.0f, position3.width - 20f, (float) (items.Length * 20)));
      for (int index = 0; index < items.Length; ++index)
      {
        if (GUI.Button(new Rect(2f, (float) (index * 20 + 2), position3.width - 4f, 20f), items[index], BlueStonez.dropdown_listItem))
        {
          this._isDropdownActive = false;
          num = index;
        }
      }
      GUITools.EndScrollView();
    }
    GUI.enabled = enabled;
    return num;
  }

  private void OnMemberListUpdatedEvent(MemberListUpdatedEvent ev)
  {
    this._playerNameTexts = new string[ev.Players.Length];
    int num = 0;
    foreach (ActorInfo player in ev.Players)
      this._playerNameTexts[num++] = player.PlayerName;
  }
}
