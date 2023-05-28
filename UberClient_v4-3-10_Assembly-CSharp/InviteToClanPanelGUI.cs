// Decompiled with JetBrains decompiler
// Type: InviteToClanPanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.WebService.Unity;
using UnityEngine;

public class InviteToClanPanelGUI : PanelGuiBase
{
  private bool _showReceiverDropdownList;
  private Vector2 _friendListScroll;
  private float _receiverDropdownHeight;
  private int _cmid;
  private string _message = string.Empty;
  private string _name = string.Empty;
  private bool _fixReceiver;

  private void OnGUI() => this.DrawInvitePlayerMessage(new Rect(0.0f, (float) GlobalUIRibbon.Instance.Height(), (float) Screen.width, (float) (Screen.height - GlobalUIRibbon.Instance.Height())));

  private void DrawInvitePlayerMessage(Rect rect)
  {
    GUI.depth = 3;
    GUI.enabled = true;
    Rect position = new Rect(rect.x + (float) (((double) rect.width - 480.0) / 2.0), rect.y + (float) (((double) rect.height - 320.0) / 2.0), 480f, 320f);
    GUI.BeginGroup(position, BlueStonez.window);
    int left1 = 25;
    int left2 = 120;
    int width = 320;
    int top1 = 70;
    int top2 = 100;
    int top3 = 132;
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 0.0f), LocalizedStrings.InvitePlayer, BlueStonez.tab_strip);
    GUI.Label(new Rect(12f, 55f, position.width - 24f, 208f), GUIContent.none, BlueStonez.window_standard_grey38);
    GUI.Label(new Rect((float) left1, (float) top1, 400f, 20f), LocalizedStrings.UseThisFormToSendClanInvitations, BlueStonez.label_interparkbold_11pt);
    GUI.Label(new Rect((float) left1, (float) top2, 90f, 20f), LocalizedStrings.PlayerCaps, BlueStonez.label_interparkbold_18pt_right);
    GUI.Label(new Rect((float) left1, (float) top3, 90f, 20f), LocalizedStrings.MessageCaps, BlueStonez.label_interparkbold_18pt_right);
    GUI.SetNextControlName("Message Receiver");
    GUI.enabled = !this._fixReceiver;
    this._name = GUI.TextField(new Rect((float) left2, (float) top2, (float) width, 24f), this._name, BlueStonez.textField);
    if (string.IsNullOrEmpty(this._name) && !GUI.GetNameOfFocusedControl().Equals("Message Receiver"))
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(new Rect((float) left2, (float) top2, (float) width, 24f), " " + LocalizedStrings.StartTypingTheNameOfAFriend, BlueStonez.label_interparkbold_11pt_left);
      GUI.color = Color.white;
    }
    GUI.enabled = !this._showReceiverDropdownList;
    GUI.SetNextControlName("Description");
    this._message = GUI.TextArea(new Rect((float) left2, (float) top3, (float) width, 108f), this._message, BlueStonez.textArea);
    this._message = this._message.Trim('\n', '\t');
    GUI.enabled = this._cmid != 0;
    if (GUITools.Button(new Rect((float) ((double) position.width - 155.0 - 155.0), position.height - 44f, 150f, 32f), new GUIContent(LocalizedStrings.SendCaps), BlueStonez.button_green))
    {
      ClanWebServiceClient.InviteMemberToJoinAGroup(PlayerDataManager.ClanIDSecure, PlayerDataManager.CmidSecure, this._cmid, this._message, (Action<int>) (ev => { }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
      PanelManager.Instance.ClosePanel(PanelType.ClanRequest);
    }
    GUI.enabled = true;
    if (GUITools.Button(new Rect(position.width - 155f, position.height - 44f, 150f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button))
    {
      this._message = string.Empty;
      PanelManager.Instance.ClosePanel(PanelType.ClanRequest);
    }
    if (!this._fixReceiver)
    {
      if (!this._showReceiverDropdownList && GUI.GetNameOfFocusedControl().Equals("Message Receiver"))
      {
        this._cmid = 0;
        this._showReceiverDropdownList = true;
      }
      if (this._showReceiverDropdownList)
        this.DoReceiverDropdownList(new Rect((float) left2, (float) (top2 + 24), (float) width, this._receiverDropdownHeight));
    }
    GUI.EndGroup();
  }

  private void Update()
  {
    this._receiverDropdownHeight = Mathf.Lerp(this._receiverDropdownHeight, !this._showReceiverDropdownList ? 0.0f : 146f, Time.deltaTime * 9f);
    if (this._showReceiverDropdownList || !Mathf.Approximately(this._receiverDropdownHeight, 0.0f))
      return;
    this._receiverDropdownHeight = 0.0f;
  }

  private void DoReceiverDropdownList(Rect rect)
  {
    GUI.BeginGroup(rect, BlueStonez.window);
    int num1 = -1;
    if (Singleton<PlayerDataManager>.Instance.FriendsCount > 0)
    {
      int num2 = 0;
      int num3 = 0;
      this._friendListScroll = GUITools.BeginScrollView(new Rect(0.0f, 0.0f, rect.width, rect.height), this._friendListScroll, new Rect(0.0f, 0.0f, rect.width - 20f, (float) (Singleton<PlayerDataManager>.Instance.FriendsCount * 24)));
      foreach (PublicProfileView friend in Singleton<PlayerDataManager>.Instance.FriendList)
      {
        if (this._name.Length <= 0 || friend.Name.ToLower().Contains(this._name.ToLower()))
        {
          if (num1 == -1)
            num1 = num3;
          bool flag = !string.IsNullOrEmpty(PlayerDataManager.ClanTag) && friend.GroupTag == PlayerDataManager.ClanTag;
          Rect position = new Rect(0.0f, (float) (num2 * 24), rect.width, 24f);
          if (GUI.enabled && position.Contains(Event.current.mousePosition) && GUI.Button(position, GUIContent.none, BlueStonez.box_grey50) && !flag)
          {
            this._cmid = friend.Cmid;
            this._name = friend.Name;
            this._showReceiverDropdownList = false;
            GUI.FocusControl("Description");
          }
          string text = !string.IsNullOrEmpty(friend.GroupTag) ? string.Format("[{0}] {1}", (object) friend.GroupTag, (object) friend.Name) : friend.Name;
          GUI.Label(new Rect(8f, (float) (num2 * 24 + 4), rect.width, rect.height), text, BlueStonez.label_interparkmed_11pt_left);
          if (flag)
          {
            GUI.contentColor = Color.gray;
            GUI.Label(new Rect(rect.width - 100f, (float) (num2 * 24 + 4), 100f, rect.height), LocalizedStrings.InMyClan, BlueStonez.label_interparkmed_11pt_left);
            GUI.contentColor = Color.white;
          }
          ++num2;
        }
      }
      GUITools.EndScrollView();
    }
    else
      GUI.Label(new Rect(0.0f, 0.0f, rect.width, rect.height), LocalizedStrings.YouHaveNoFriends, BlueStonez.label_interparkmed_11pt);
    GUI.EndGroup();
    if (Event.current.type != UnityEngine.EventType.MouseDown || rect.Contains(Event.current.mousePosition))
      return;
    GUI.FocusControl("Description");
    this._showReceiverDropdownList = false;
    PublicProfileView view;
    if (PlayerDataManager.TryGetFriend(this._cmid, out view))
    {
      this._name = view.Name;
    }
    else
    {
      this._name = string.Empty;
      this._cmid = 0;
    }
  }

  public override void Show()
  {
    base.Show();
    this._message = string.Format(LocalizedStrings.HiYoureInvitedToJoinMyClanN, (object) PlayerDataManager.ClanName);
  }

  public override void Hide()
  {
    base.Hide();
    this._name = string.Empty;
    this._fixReceiver = false;
    this._cmid = 0;
  }

  public void SelectReceiver(int cmid, string name)
  {
    this._cmid = cmid;
    this._name = name;
    this._fixReceiver = this._cmid != 0;
  }
}
