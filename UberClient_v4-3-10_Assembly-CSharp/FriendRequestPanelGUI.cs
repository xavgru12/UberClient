// Decompiled with JetBrains decompiler
// Type: FriendRequestPanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public class FriendRequestPanelGUI : PanelGuiBase
{
  private const string FocusReceiver = "Message Receiver";
  private const string FocusContent = "Message Content";
  private bool _useFixedReceiver;
  private bool _showComposeMessage;
  private bool _showReceiverDropdownList;
  private string _msgReceiver;
  private string _msgContent;
  private string _lastMsgRcvName;
  private int _msgRcvCmid;
  private int _receiverCount;
  private float _rcvDropdownWidth;
  private float _rcvDropdownHeight;
  private Vector2 _friendDropdownScroll;
  private float _keyboardOffset;
  private float _targetKeyboardOffset;

  private void OnGUI()
  {
    this._keyboardOffset = (double) Mathf.Abs(this._keyboardOffset - this._targetKeyboardOffset) <= 2.0 ? this._targetKeyboardOffset : Mathf.Lerp(this._keyboardOffset, this._targetKeyboardOffset, Time.deltaTime * 4f);
    if (!this._showComposeMessage)
      return;
    GUI.depth = 3;
    GUI.skin = BlueStonez.Skin;
    Rect rect = new Rect((float) ((Screen.width - 480) / 2), (float) ((Screen.height - 320) / 2) - this._keyboardOffset, 480f, 300f);
    GUI.Box(rect, GUIContent.none, BlueStonez.window);
    this.DoCompose(rect);
    if (this._showReceiverDropdownList)
      this.DoReceiverDropdownList(rect);
    this._rcvDropdownHeight = Mathf.Lerp(this._rcvDropdownHeight, !this._showReceiverDropdownList ? 0.0f : 146f, Time.deltaTime * 9f);
    if (!this._showReceiverDropdownList && Mathf.Approximately(this._rcvDropdownHeight, 0.0f))
      this._rcvDropdownHeight = 0.0f;
    GUI.enabled = true;
  }

  private void HideKeyboard()
  {
  }

  private void DoCompose(Rect rect)
  {
    Rect position = new Rect(rect.x + (float) (((double) rect.width - 480.0) / 2.0), rect.y + (float) (((double) rect.height - 300.0) / 2.0), 480f, 290f);
    GUI.BeginGroup(position, BlueStonez.window);
    int left1 = 35;
    int left2 = 120;
    int width = 320;
    int top1 = 70;
    int top2 = 100;
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 0.0f), LocalizedStrings.FriendRequestCaps, BlueStonez.tab_strip);
    GUI.Box(new Rect(12f, 55f, position.width - 24f, position.height - 101f), GUIContent.none, BlueStonez.window_standard_grey38);
    GUI.Label(new Rect((float) left1, (float) top1, 75f, 20f), LocalizedStrings.To, BlueStonez.label_interparkbold_18pt_right);
    GUI.Label(new Rect((float) left1, (float) top2, 75f, 20f), LocalizedStrings.Message, BlueStonez.label_interparkbold_18pt_right);
    bool enabled = GUI.enabled;
    GUI.enabled = enabled && !this._useFixedReceiver;
    GUI.SetNextControlName("Message Receiver");
    this._msgReceiver = GUI.TextField(new Rect((float) left2, (float) top1, (float) width, 24f), this._msgReceiver, BlueStonez.textField);
    if (string.IsNullOrEmpty(this._msgReceiver) && !GUI.GetNameOfFocusedControl().Equals("Message Receiver"))
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(new Rect((float) left2, (float) top1, (float) width, 24f), " " + LocalizedStrings.StartTypingTheNameOfAFriend, BlueStonez.label_interparkbold_11pt_left);
      GUI.color = Color.white;
    }
    GUI.enabled = enabled && !this._showReceiverDropdownList;
    GUI.SetNextControlName("Message Content");
    this._msgContent = GUI.TextArea(new Rect((float) left2, (float) top2, (float) width, 108f), this._msgContent, 140, BlueStonez.textArea);
    GUI.color = new Color(1f, 1f, 1f, 0.5f);
    GUI.Label(new Rect((float) left2, (float) (top2 + 110), (float) width, 24f), (140 - this._msgContent.Length).ToString(), BlueStonez.label_interparkbold_11pt_right);
    GUI.color = Color.white;
    GUI.enabled = enabled && !this._showReceiverDropdownList && this._msgRcvCmid != 0 && !string.IsNullOrEmpty(this._msgContent);
    if (GUITools.Button(new Rect((float) ((double) position.width - 95.0 - 100.0), position.height - 44f, 90f, 32f), new GUIContent(LocalizedStrings.SendCaps), BlueStonez.button_green))
    {
      Singleton<CommsManager>.Instance.SendFriendRequest(this._msgRcvCmid, this._msgContent);
      this._msgContent = string.Empty;
      this._msgReceiver = string.Empty;
      this._msgRcvCmid = 0;
      this.HideKeyboard();
      this.Hide();
    }
    GUI.enabled = enabled;
    if (GUITools.Button(new Rect(position.width - 100f, position.height - 44f, 90f, 32f), new GUIContent(LocalizedStrings.DiscardCaps), BlueStonez.button))
    {
      this.HideKeyboard();
      this.Hide();
    }
    if (!this._showReceiverDropdownList && GUI.GetNameOfFocusedControl().Equals("Message Receiver"))
    {
      this._showReceiverDropdownList = true;
      this._lastMsgRcvName = this._msgReceiver;
      this._msgReceiver = string.Empty;
    }
    GUI.EndGroup();
    if (!this._showReceiverDropdownList)
      return;
    this.DoReceiverDropdownList(rect);
  }

  private void DoReceiverDropdownList(Rect rect)
  {
    Rect position1 = new Rect(rect.x + 120f, rect.y + 94f, 320f, this._rcvDropdownHeight);
    GUI.BeginGroup(position1, BlueStonez.window);
    if (Singleton<PlayerDataManager>.Instance.FriendsCount > 0)
    {
      int num = 0;
      this._friendDropdownScroll = GUITools.BeginScrollView(new Rect(0.0f, 0.0f, position1.width, position1.height), this._friendDropdownScroll, new Rect(0.0f, 0.0f, this._rcvDropdownWidth, (float) (this._receiverCount * 24)));
      foreach (PublicProfileView friend in Singleton<PlayerDataManager>.Instance.FriendList)
      {
        if (this._msgReceiver.Length <= 0 || friend.Name.ToLower().Contains(this._msgReceiver.ToLower()))
        {
          Rect position2 = new Rect(0.0f, (float) (num * 24), position1.width, 24f);
          if (GUI.enabled && position2.Contains(Event.current.mousePosition) && GUI.Button(position2, GUIContent.none, BlueStonez.box_grey50))
          {
            this._msgRcvCmid = friend.Cmid;
            this._msgReceiver = friend.Name;
            this._showReceiverDropdownList = false;
            GUI.FocusControl("Message Content");
          }
          GUI.Label(new Rect(8f, (float) (num * 24 + 4), position1.width, position1.height), friend.Name, BlueStonez.label_interparkmed_11pt_left);
          ++num;
        }
      }
      this._receiverCount = num;
      this._rcvDropdownWidth = (double) (this._receiverCount * 24) <= (double) position1.height ? position1.width - 8f : position1.width - 22f;
      GUITools.EndScrollView();
    }
    else
      GUI.Label(new Rect(0.0f, 0.0f, position1.width, position1.height), LocalizedStrings.YouHaveNoFriends, BlueStonez.label_interparkmed_11pt);
    GUI.EndGroup();
    if (Event.current.type != UnityEngine.EventType.MouseDown || position1.Contains(Event.current.mousePosition))
      return;
    this._showReceiverDropdownList = false;
    if (this._msgRcvCmid != 0)
      return;
    this._msgReceiver = this._lastMsgRcvName;
  }

  public override void Show()
  {
    base.Show();
    this._msgRcvCmid = 0;
    this._msgContent = string.Empty;
    this._msgReceiver = string.Empty;
    this._showComposeMessage = true;
    this._showReceiverDropdownList = false;
    this._useFixedReceiver = false;
    GUI.FocusControl("Message Receiver");
  }

  public override void Hide()
  {
    base.Hide();
    this._showComposeMessage = false;
    this._showReceiverDropdownList = false;
  }

  public void SelectReceiver(int cmid, string name)
  {
    this._useFixedReceiver = true;
    this._msgRcvCmid = cmid;
    this._msgReceiver = name;
    GUI.FocusControl("Message Content");
  }
}
