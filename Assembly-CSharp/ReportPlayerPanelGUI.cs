using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ReportPlayerPanelGUI : PanelGuiBase
{
	private Rect _rect;

	private bool _isDropdownActive;

	private Vector2 _listScroll;

	private string[] _reportTypeTexts;

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

	private void Awake()
	{
	}

	private void Start()
	{
		_isDropdownActive = false;
		_abuse = LocalizedStrings.SelectType;
		Array values = Enum.GetValues(typeof(MemberReportType));
		_reportTypeTexts = new string[values.Length];
		int num = 0;
		foreach (int item in values)
		{
			_reportTypeTexts[num++] = Enum.GetName(typeof(MemberReportType), (MemberReportType)item);
		}
	}

	private void OnEnable()
	{
		GUI.FocusControl("ReportDetail");
	}

	private void OnGUI()
	{
		_rect = new Rect((float)(Screen.width - 570) * 0.5f, (float)(Screen.height - 345) * 0.5f, 570f, 345f);
		GUI.BeginGroup(_rect, GUIContent.none, BlueStonez.window_standard_grey38);
		DrawReportPanel();
		GUI.EndGroup();
	}

	public override void Show()
	{
		base.Show();
		_commUsersCount = 0;
		_commUsers = Singleton<ChatManager>.Instance.GetCommUsersToReport();
	}

	public override void Hide()
	{
		base.Hide();
		_commUsers = null;
		_selectedAbusion = -1;
		_abuse = LocalizedStrings.SelectType;
	}

	public static void ReportInboxPlayer(PrivateMessageView msg, string messageSender)
	{
		int reportedCmid = msg.FromCmid;
		string reason = msg.ContentText;
		if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.IsConnected)
		{
			PopupSystem.ShowMessage(LocalizedStrings.ReportPlayerCaps, string.Format(LocalizedStrings.ReportPlayerWarningMsg, messageSender), PopupSystem.AlertType.OKCancel, delegate
			{
				AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendPlayersReported(new List<int>
				{
					reportedCmid
				}, 0, reason, Singleton<ChatManager>.Instance.GetAllChatMessagesForPlayerReport());
			}, LocalizedStrings.Report, null, LocalizedStrings.Cancel, PopupSystem.ActionType.Negative);
		}
		else
		{
			PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.ReportPlayerErrorMsg, PopupSystem.AlertType.OK, null);
		}
	}

	private void DrawReportPanel()
	{
		GUI.depth = 3;
		GUI.skin = BlueStonez.Skin;
		GUI.Label(new Rect(0f, 0f, _rect.width, 56f), LocalizedStrings.ReportPlayerCaps, BlueStonez.tab_strip);
		GUI.color = Color.red;
		GUI.Label(new Rect(16f, _rect.height - 40f, 300f, 30f), LocalizedStrings.ReportPlayerInfoMsg, BlueStonez.label_interparkbold_11pt_left_wrap);
		GUI.color = Color.white;
		Rect position = new Rect(17f, 55f, _rect.width - 34f, _rect.height - 100f);
		GUI.BeginGroup(position, string.Empty, BlueStonez.window_standard_grey38);
		GUI.Label(new Rect(16f, 20f, 100f, 18f), LocalizedStrings.ReportType, BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(16f, 50f, 100f, 18f), LocalizedStrings.PlayerNames, BlueStonez.label_interparkbold_18pt_left);
		GUI.Label(new Rect(16f, 80f, 100f, 18f), LocalizedStrings.Details, BlueStonez.label_interparkbold_18pt_left);
		GUI.enabled = !_isDropdownActive;
		GUI.SetNextControlName("ReportDetail");
		_reason = GUI.TextArea(new Rect(16f, 110f, 290f, 120f), _reason);
		GUI.Label(new Rect(125f, 50f, 180f, 22f), _selectedPlayers, BlueStonez.textField);
		if (string.IsNullOrEmpty(_selectedPlayers))
		{
			GUI.color = Color.gray;
			GUI.Label(new Rect(130f, 52f, 180f, 22f), "(" + LocalizedStrings.NoPlayerSelected + ")");
			GUI.color = Color.white;
		}
		GUI.enabled = true;
		int num = DoDropDownList(new Rect(125f, 20f, 183f, 22f), new Rect(135f, 50f, 194f, 84f), _reportTypeTexts, ref _abuse, canEdit: false);
		if (num != -1)
		{
			_selectedAbusion = num;
			_abuse = _reportTypeTexts[num];
		}
		GUI.SetNextControlName("SearchUser");
		_searchPattern = GUI.TextField(new Rect(325f, 20f, 196f, 22f), _searchPattern);
		if (string.IsNullOrEmpty(_searchPattern) && GUI.GetNameOfFocusedControl() != "SearchUser")
		{
			GUI.color = Color.gray;
			GUI.Label(new Rect(333f, 22f, 196f, 22f), LocalizedStrings.SelectAPlayer);
			GUI.color = Color.white;
		}
		int num2 = 0;
		GUI.Label(new Rect(325f, 50f, 175f, 178f), GUIContent.none, BlueStonez.box_grey50);
		_scrollUsers = GUITools.BeginScrollView(new Rect(325f, 50f, 195f, 178f), _scrollUsers, new Rect(0f, 0f, 150f, Mathf.Max(_commUsersCount * 20, 178)), useHorizontal: false, useVertical: true);
		if (_commUsers != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = _searchPattern.ToLowerInvariant();
			foreach (CommUser commUser in _commUsers)
			{
				bool flag = _reportedCmids.Contains(commUser.Cmid);
				if (flag)
				{
					stringBuilder.Append(commUser.Name).Append(", ");
				}
				if (commUser.Name.ToLowerInvariant().Contains(value))
				{
					bool flag2 = GUI.Toggle(new Rect(2f, 2 + num2 * 20, 171f, 20f), flag, commUser.Name, BlueStonez.dropdown_listItem);
					if (flag2 != flag)
					{
						_reportedCmids.Clear();
						if (!flag)
						{
							_reportedCmids.Add(commUser.Cmid);
						}
					}
					num2++;
				}
			}
			_commUsersCount = num2;
			_selectedPlayers = stringBuilder.ToString();
		}
		GUITools.EndScrollView();
		if (_commUsersCount == 0)
		{
			GUI.Label(new Rect(325f, 50f, 175f, 178f), LocalizedStrings.NoPlayersToReport, BlueStonez.label_interparkmed_11pt);
		}
		else if (num2 == 0)
		{
			GUI.Label(new Rect(325f, 50f, 175f, 178f), LocalizedStrings.NoMatchFound, BlueStonez.label_interparkmed_11pt);
		}
		GUI.EndGroup();
		if (GUITools.Button(new Rect(_rect.width - 125f, _rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button))
		{
			PanelManager.Instance.ClosePanel(PanelType.ReportPlayer);
			_commUsers = null;
			_reportedCmids.Clear();
			_selectedPlayers = string.Empty;
			_reason = string.Empty;
			_selectedAbusion = -1;
		}
		GUI.enabled = (_selectedAbusion >= 0 && !string.IsNullOrEmpty(_selectedPlayers) && !string.IsNullOrEmpty(_reason));
		if (GUITools.Button(new Rect(_rect.width - 125f - 125f, _rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.SendCaps), BlueStonez.button_red))
		{
			if (AutoMonoBehaviour<CommConnectionManager>.Instance.Client.IsConnected)
			{
				PopupSystem.ShowMessage(LocalizedStrings.ReportPlayerCaps, string.Format(LocalizedStrings.ReportPlayerWarningMsg, _selectedPlayers), PopupSystem.AlertType.OKCancel, ConfirmAbuseReport, LocalizedStrings.Report, null, LocalizedStrings.Cancel, PopupSystem.ActionType.Negative);
			}
			else
			{
				PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.ReportPlayerErrorMsg, PopupSystem.AlertType.OK, null);
			}
		}
	}

	private void ConfirmAbuseReport()
	{
		AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendPlayersReported(_reportedCmids, 0, _reason, Singleton<ChatManager>.Instance.GetAllChatMessagesForPlayerReport());
		PanelManager.Instance.ClosePanel(PanelType.ReportPlayer);
		PopupSystem.ShowMessage(LocalizedStrings.ReportPlayerCaps, LocalizedStrings.ReportPlayerSuccessMsg, PopupSystem.AlertType.OK, null);
		_reportedCmids.Clear();
		_selectedPlayers = string.Empty;
		_reason = string.Empty;
		_selectedAbusion = -1;
	}

	private void DrawGroupControl(Rect rect, string title, GUIStyle style)
	{
		GUI.BeginGroup(rect, string.Empty, BlueStonez.group_grey81);
		GUI.EndGroup();
		float left = rect.x + 18f;
		float top = rect.y - 8f;
		Vector2 vector = style.CalcSize(new GUIContent(title));
		GUI.Label(new Rect(left, top, vector.x + 10f, 16f), title, style);
	}

	private int DoDropDownList(Rect position, Rect size, string[] items, ref string defaultText, bool canEdit)
	{
		int result = -1;
		Rect position2 = new Rect(position.x, position.y, position.width - position.height, position.height);
		Rect position3 = new Rect(position.x + position.width - position.height - 2f, position.y - 1f, position.height, position.height);
		bool enabled = GUI.enabled;
		GUI.enabled = (!_isDropdownActive || _currentActiveItems == items);
		if (canEdit)
		{
			defaultText = GUI.TextField(new Rect(position.x, position.y, position.width - position.height, position.height - 1f), defaultText, BlueStonez.textArea);
		}
		else
		{
			GUI.Label(position2, defaultText, BlueStonez.label_dropdown);
		}
		if (GUI.Button(position3, GUIContent.none, BlueStonez.dropdown_button))
		{
			_isDropdownActive = !_isDropdownActive;
			_currentActiveItems = items;
		}
		if (_isDropdownActive && _currentActiveItems == items)
		{
			Rect position4 = new Rect(position.x, position.y + position.height - 1f, size.width - 16f, size.height);
			GUI.Box(position4, string.Empty, BlueStonez.window_standard_grey38);
			_listScroll = GUITools.BeginScrollView(position4, _listScroll, new Rect(0f, 0f, position4.width - 20f, items.Length * 20));
			for (int i = 0; i < items.Length; i++)
			{
				if (GUI.Button(new Rect(2f, i * 20 + 2, position4.width - 4f, 20f), items[i], BlueStonez.dropdown_listItem))
				{
					_isDropdownActive = false;
					result = i;
				}
			}
			GUITools.EndScrollView();
		}
		GUI.enabled = enabled;
		return result;
	}
}
