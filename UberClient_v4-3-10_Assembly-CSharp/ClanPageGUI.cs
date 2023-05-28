// Decompiled with JetBrains decompiler
// Type: ClanPageGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ClanPageGUI : MonoBehaviour
{
  private const int _indicatorWidth = 25;
  private const int _positionWidth = 70;
  private const int _joinDateWidth = 80;
  [SerializeField]
  private Texture2D _level4Icon;
  [SerializeField]
  private Texture2D _licenseIcon;
  [SerializeField]
  private Texture2D _friendsIcon;
  private bool createAClan;
  private int _onlineMemberCount;
  private int _nameWidth;
  private int _statusWidth;
  private Vector2 _clanMembersScrollView;
  private string _newClanName = string.Empty;
  private string _newClanTag = string.Empty;
  private string _newClanMotto = string.Empty;

  private void Awake() => CmuneEventHandler.AddListener<ClanPageGUI.ClanCreationEvent>(new Action<ClanPageGUI.ClanCreationEvent>(this.OnClanCreated));

  private void OnClanCreated(ClanPageGUI.ClanCreationEvent ev)
  {
    this.createAClan = false;
    this._newClanMotto = string.Empty;
    this._newClanName = string.Empty;
    this._newClanTag = string.Empty;
  }

  private void OnGUI()
  {
    GUI.depth = 11;
    GUI.skin = BlueStonez.Skin;
    Rect rect = new Rect(0.0f, (float) GlobalUIRibbon.Instance.Height(), (float) Screen.width, (float) (Screen.height - GlobalUIRibbon.Instance.Height()));
    GUI.BeginGroup(rect, BlueStonez.box_grey31);
    GUI.enabled = PlayerDataManager.IsPlayerLoggedIn && this.IsNoPopupOpen() && !Singleton<ClanDataManager>.Instance.IsProcessingWebservice;
    if (PlayerDataManager.IsPlayerInClan)
    {
      float num = 73f;
      float height1 = 40f;
      float height2 = rect.height - num - height1;
      this.DrawClanRosterHeader(new Rect(0.0f, 0.0f, rect.width, num));
      this.DrawMembersView(new Rect(0.0f, num, rect.width, height2));
      this.DrawClanRosterFooter(new Rect(0.0f, num + height2, rect.width, height1));
    }
    else
    {
      GUI.Box(rect, GUIContent.none, BlueStonez.box_grey38);
      if (this.createAClan)
        this.DrawCreateClanMessage(rect);
      else
        this.DrawNoClanMessage(rect);
    }
    GuiManager.DrawTooltip();
    GUI.enabled = true;
    GUI.EndGroup();
  }

  private void DrawClanRosterHeader(Rect rect)
  {
    int width = (int) rect.width;
    GUI.BeginGroup(rect, BlueStonez.box_grey31);
    GUI.Label(new Rect(10f, 5f, rect.width - 20f, 18f), string.Format("{0}: {1}", (object) LocalizedStrings.YourClan, (object) PlayerDataManager.ClanName), BlueStonez.label_interparkbold_16pt_left);
    float num = Mathf.Max(Singleton<ClanDataManager>.Instance.NextClanRefresh - Time.time, 0.0f);
    GUITools.PushGUIState();
    GUI.enabled &= (double) num == 0.0;
    if (GUITools.Button(new Rect(rect.width - 130f, 5f, 120f, 19f), new GUIContent(string.Format(LocalizedStrings.Refresh + " {0}", (double) num <= 0.0 ? (object) string.Empty : (object) ("(" + num.ToString("N0") + ")"))), BlueStonez.buttondark_medium))
      Singleton<ClanDataManager>.Instance.RefreshClanData();
    GUITools.PopGUIState();
    GUI.Label(new Rect(rect.width - 340f, 5f, 200f, 18f), string.Format(LocalizedStrings.NMembersNOnline, (object) Singleton<PlayerDataManager>.Instance.ClanMembersCount, (object) PlayerDataManager.ClanMembersLimit, (object) this._onlineMemberCount), BlueStonez.label_interparkmed_11pt_right);
    GUI.BeginGroup(new Rect(0.0f, 25f, rect.width, 50f), BlueStonez.box_grey50);
    GUI.Label(new Rect(10f, 7f, rect.width / 2f, 16f), string.Format("Tag: {0}", (object) PlayerDataManager.ClanTag), BlueStonez.label_interparkmed_11pt_left);
    GUI.Label(new Rect(10f, 28f, rect.width / 2f, 16f), string.Format(LocalizedStrings.MottoN, (object) PlayerDataManager.ClanMotto), BlueStonez.label_interparkmed_11pt_left);
    GUI.Label(new Rect(rect.width / 2f, 7f, rect.width / 2f, 16f), string.Format(LocalizedStrings.CreatedN, (object) PlayerDataManager.ClanFoundingDate.ToShortDateString()), BlueStonez.label_interparkmed_11pt_left);
    GUI.Label(new Rect(rect.width / 2f, 28f, rect.width / 2f, 16f), string.Format(LocalizedStrings.LeaderN, (object) PlayerDataManager.ClanOwnerName), BlueStonez.label_interparkmed_11pt_left);
    GUI.EndGroup();
    if (Singleton<PlayerDataManager>.Instance.RankInClan != GroupPosition.Member && GUITools.Button(new Rect((float) (int) ((double) rect.width - 10.0 - 120.0), 40f, 120f, 20f), new GUIContent(LocalizedStrings.InvitePlayer), BlueStonez.buttondark_medium))
      PanelManager.Instance.OpenPanel(PanelType.ClanRequest);
    GUI.EndGroup();
  }

  private void DrawMembersView(Rect rect)
  {
    GUI.BeginGroup(rect, BlueStonez.box_grey38);
    this.UpdateColumnWidth();
    GUI.Box(new Rect(0.0f, 0.0f, 25f, 25f), string.Empty, BlueStonez.box_grey50);
    int left1 = 24;
    GUI.Box(new Rect((float) left1, 0.0f, (float) this._nameWidth, 25f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect((float) (left1 + 5), 5f, (float) this._nameWidth, 25f), LocalizedStrings.Player, BlueStonez.label_interparkmed_11pt_left);
    int left2 = 25 + this._nameWidth - 2;
    GUI.Box(new Rect((float) left2, 0.0f, 70f, 25f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect((float) (left2 + 5), 5f, 70f, 25f), LocalizedStrings.Position, BlueStonez.label_interparkmed_11pt_left);
    int left3 = 25 + this._nameWidth + 70 - 3;
    GUI.Box(new Rect((float) left3, 0.0f, 80f, 25f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect((float) (left3 + 5), 5f, 80f, 25f), LocalizedStrings.JoinDate, BlueStonez.label_interparkmed_11pt_left);
    GUI.Box(new Rect((float) (25 + this._nameWidth + 70 + 80 - 4), 0.0f, (float) this._statusWidth, 25f), string.Empty, BlueStonez.box_grey50);
    int num = 0;
    int height = Singleton<PlayerDataManager>.Instance.ClanMembersCount * 50;
    this._clanMembersScrollView = GUITools.BeginScrollView(new Rect(0.0f, 25f, rect.width, rect.height - 25f), this._clanMembersScrollView, new Rect(0.0f, 0.0f, rect.width - 20f, (float) height));
    this._onlineMemberCount = 0;
    foreach (ClanMemberView clanMember in Singleton<PlayerDataManager>.Instance.ClanMembers)
      this.DrawClanMembers(new Rect(0.0f, (float) (50 * num++), rect.width - 20f, 50f), clanMember);
    GUITools.EndScrollView();
    GUI.EndGroup();
  }

  private void DrawClanMembers(Rect rect, ClanMemberView member)
  {
    GUIStyle style = !rect.Contains(Event.current.mousePosition) ? BlueStonez.box_grey38 : BlueStonez.box_grey50;
    GUI.BeginGroup(rect, style);
    CommUser user;
    if (Singleton<ChatManager>.Instance.TryGetClanUsers(member.Cmid, out user))
      GUI.DrawTexture(new Rect(5f, 12f, 14f, 20f), ChatManager.GetPresenceIcon(user.PresenceIndex));
    GUI.Label(new Rect(28f, 12f, (float) this._nameWidth, 25f), member.Name, BlueStonez.label_interparkbold_13pt_left);
    GUI.Label(new Rect((float) (25 + this._nameWidth + 3), 20f, 70f, 25f), this.ConvertClanPosition(member.Position), BlueStonez.label_interparkmed_11pt_left);
    GUI.Label(new Rect((float) (25 + this._nameWidth + 70 + 3), 20f, 80f, 25f), member.JoiningDate.ToString("d"), BlueStonez.label_interparkmed_11pt_left);
    float num1 = rect.width - 20f;
    if (member.Cmid != PlayerDataManager.Cmid)
    {
      if (user != null && user.IsOnline)
      {
        ++this._onlineMemberCount;
        if (GUITools.Button(new Rect(num1 - 120f, 15f, 100f, 20f), new GUIContent(LocalizedStrings.PrivateChat), BlueStonez.buttondark_medium))
        {
          Singleton<ChatManager>.Instance.CreatePrivateChat(member.Cmid);
          MenuPageManager.Instance.LoadPage(PageType.Chat);
          ChatPageGUI.SelectedTab = TabArea.Private;
        }
      }
      else
      {
        int days = DateTime.Now.Subtract(member.Lastlogin).Days;
        string text = string.Format(LocalizedStrings.LastOnlineN, days <= 1 ? (days != 0 ? (object) LocalizedStrings.Yesterday : (object) LocalizedStrings.Today) : (object) (days.ToString() + " " + LocalizedStrings.DaysAgo));
        GUI.Label(new Rect(num1 - 120f, 20f, 100f, 25f), text, BlueStonez.label_interparkmed_11pt_left);
      }
      if (GUITools.Button(new Rect(num1 - 230f, 15f, 100f, 20f), new GUIContent(LocalizedStrings.SendMessage), BlueStonez.buttondark_medium))
      {
        SendMessagePanelGUI sendMessagePanelGui = PanelManager.Instance.OpenPanel(PanelType.SendMessage) as SendMessagePanelGUI;
        if ((bool) (UnityEngine.Object) sendMessagePanelGui)
          sendMessagePanelGui.SelectReceiver(member.Cmid, member.Name);
      }
    }
    if (this.HasHigherPermissionThan(member.Position))
    {
      if (GUITools.Button(new Rect(num1 - 10f, 14f, 20f, 20f), new GUIContent("x"), BlueStonez.buttondark_medium))
      {
        int removeFromClanCmid = member.Cmid;
        string text = string.Format(LocalizedStrings.RemoveNFromClanN, (object) member.Name, (object) PlayerDataManager.ClanName) + "\n\n" + LocalizedStrings.RemoveMemberWarningMsg;
        PopupSystem.ShowMessage(LocalizedStrings.RemoveMember, text, PopupSystem.AlertType.OKCancel, (Action) (() => Singleton<ClanDataManager>.Instance.RemoveMemberFromClan(removeFromClanCmid)), "OK", (Action) null, LocalizedStrings.Cancel, PopupSystem.ActionType.Negative);
      }
      num1 -= 160f;
    }
    int num2 = 25 + this._nameWidth;
    if (Singleton<PlayerDataManager>.Instance.RankInClan == GroupPosition.Leader && this.HasHigherPermissionThan(member.Position))
    {
      if (GUITools.Button(new Rect((float) (num2 - 140), 4f, 130f, 20f), new GUIContent(LocalizedStrings.TransferLeadership), BlueStonez.buttondark_medium))
      {
        int newLeader = member.Cmid;
        string text = string.Format(LocalizedStrings.TransferClanLeaderhsipToN, (object) member.Name) + "\n\n" + LocalizedStrings.TransferClanWarningMsg;
        PopupSystem.ShowMessage(LocalizedStrings.TransferLeadership, text, PopupSystem.AlertType.OKCancel, (Action) (() => Singleton<ClanDataManager>.Instance.TransferOwnershipTo(newLeader)), LocalizedStrings.TransferCaps, (Action) null, LocalizedStrings.Cancel, PopupSystem.ActionType.Negative);
      }
      num1 -= 160f;
    }
    if (Singleton<PlayerDataManager>.Instance.RankInClan == GroupPosition.Leader && this.HasHigherPermissionThan(member.Position))
    {
      if (member.Position == GroupPosition.Member && GUITools.Button(new Rect((float) (num2 - 140), 28f, 130f, 20f), new GUIContent(LocalizedStrings.PromoteMember), BlueStonez.buttondark_medium))
      {
        int memberCmid = member.Cmid;
        PopupSystem.ShowMessage(LocalizedStrings.PromoteMember, string.Format(LocalizedStrings.ThisWillChangeNPositionToN, (object) member.Name, (object) LocalizedStrings.Officer), PopupSystem.AlertType.OKCancel, (Action) (() => Singleton<ClanDataManager>.Instance.UpdateMemberTo(memberCmid, GroupPosition.Officer)), LocalizedStrings.PromoteCaps, (Action) null, LocalizedStrings.Cancel, PopupSystem.ActionType.Positive);
      }
      else if (member.Position == GroupPosition.Officer && GUITools.Button(new Rect((float) (num2 - 140), 28f, 130f, 20f), new GUIContent(LocalizedStrings.DemoteMember), BlueStonez.buttondark_medium))
      {
        int memberCmid = member.Cmid;
        PopupSystem.ShowMessage(LocalizedStrings.DemoteMember, string.Format(LocalizedStrings.ThisWillChangeNPositionToN, (object) member.Name, (object) LocalizedStrings.Member), PopupSystem.AlertType.OKCancel, (Action) (() => Singleton<ClanDataManager>.Instance.UpdateMemberTo(memberCmid, GroupPosition.Member)), LocalizedStrings.DemoteCaps, (Action) null, LocalizedStrings.Cancel, PopupSystem.ActionType.Negative);
      }
      float num3 = num1 - 160f;
    }
    GUI.Label(new Rect(0.0f, rect.height - 1f, rect.width, 1f), string.Empty, BlueStonez.horizontal_line_grey95);
    GUI.EndGroup();
  }

  private void DrawClanRosterFooter(Rect rect)
  {
    GUI.BeginGroup(rect, BlueStonez.box_grey31);
    if (Singleton<PlayerDataManager>.Instance.RankInClan == GroupPosition.Leader)
    {
      if (GUITools.Button(new Rect(rect.width - 110f, 10f, 100f, 20f), new GUIContent(LocalizedStrings.DisbandClan), BlueStonez.buttondark_medium))
      {
        string text = string.Format(LocalizedStrings.DisbandClanN, (object) PlayerDataManager.ClanName) + "\n\n" + LocalizedStrings.DisbandClanWarningMsg;
        PopupSystem.ShowMessage(LocalizedStrings.DisbandClan, text, PopupSystem.AlertType.OKCancel, (Action) (() => Singleton<ClanDataManager>.Instance.DisbanClan()), LocalizedStrings.DisbandCaps, (Action) null, LocalizedStrings.Cancel, PopupSystem.ActionType.Negative);
      }
    }
    else if (GUITools.Button(new Rect(rect.width - 110f, 10f, 100f, 20f), new GUIContent(LocalizedStrings.LeaveClan), BlueStonez.buttondark_medium))
    {
      string text = string.Format(LocalizedStrings.LeaveClanN, (object) PlayerDataManager.ClanName) + "\n\n" + LocalizedStrings.LeaveClanWarningMsg;
      PopupSystem.ShowMessage(LocalizedStrings.LeaveClan, text, PopupSystem.AlertType.OKCancel, (Action) (() => Singleton<ClanDataManager>.Instance.LeaveClan()), LocalizedStrings.LeaveCaps, (Action) null, LocalizedStrings.Cancel, PopupSystem.ActionType.Negative);
    }
    GUI.EndGroup();
  }

  private bool HasHigherPermissionThan(GroupPosition gp)
  {
    switch (Singleton<PlayerDataManager>.Instance.RankInClan)
    {
      case GroupPosition.Leader:
        return gp != GroupPosition.Leader;
      case GroupPosition.Officer:
        return gp == GroupPosition.Member;
      default:
        return false;
    }
  }

  private string ConvertClanPosition(GroupPosition gp)
  {
    string empty = string.Empty;
    GroupPosition groupPosition = gp;
    switch (groupPosition)
    {
      case GroupPosition.Leader:
        return LocalizedStrings.Leader;
      case GroupPosition.Member:
        return LocalizedStrings.Member;
      default:
        return groupPosition == GroupPosition.Officer ? LocalizedStrings.Officer : LocalizedStrings.Unknown;
    }
  }

  private void UpdateColumnWidth()
  {
    int num = Screen.width - 25 - 70 - 80;
    this._nameWidth = Mathf.Clamp(Mathf.RoundToInt((float) num * 0.5f), 200, 300);
    this._statusWidth = num - this._nameWidth + 4;
  }

  private void DrawNoClanMessage(Rect rect)
  {
    Rect position = new Rect((float) (((double) rect.width - 480.0) / 2.0), (float) (((double) rect.height - 240.0) / 2.0), 480f, 240f);
    GUI.BeginGroup(position, BlueStonez.window_standard_grey38);
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 56f), LocalizedStrings.ClansCaps, BlueStonez.tab_strip);
    GUI.Box(new Rect((float) ((double) position.width / 2.0 - 82.0), 60f, 48f, 48f), new GUIContent((Texture) this._level4Icon), BlueStonez.item_slot_large);
    if (Singleton<ClanDataManager>.Instance.HaveLevel)
      GUI.Box(new Rect((float) ((double) position.width / 2.0 - 82.0), 60f, 48f, 48f), new GUIContent((Texture) UberstrikeIcons.LevelMastered));
    GUI.Box(new Rect((float) ((double) position.width / 2.0 - 24.0), 60f, 48f, 48f), new GUIContent((Texture) this._licenseIcon), BlueStonez.item_slot_large);
    if (Singleton<ClanDataManager>.Instance.HaveLicense)
      GUI.Box(new Rect((float) ((double) position.width / 2.0 - 24.0), 60f, 48f, 48f), new GUIContent((Texture) UberstrikeIcons.LevelMastered));
    GUI.Box(new Rect((float) ((double) position.width / 2.0 + 34.0), 60f, 48f, 48f), new GUIContent((Texture) this._friendsIcon), BlueStonez.item_slot_large);
    if (Singleton<ClanDataManager>.Instance.HaveFriends)
      GUI.Box(new Rect((float) ((double) position.width / 2.0 + 34.0), 60f, 48f, 48f), new GUIContent((Texture) UberstrikeIcons.LevelMastered));
    if (!Singleton<ClanDataManager>.Instance.HaveLevel || !Singleton<ClanDataManager>.Instance.HaveLicense || !Singleton<ClanDataManager>.Instance.HaveFriends)
    {
      bool enabled = GUI.enabled;
      GUI.Label(new Rect((float) ((double) position.width / 2.0 - 90.0), 110f, 210f, 14f), LocalizedStrings.ToCreateAClanYouStillNeedTo, BlueStonez.label_interparkbold_11pt_left);
      GUI.enabled = enabled && !Singleton<ClanDataManager>.Instance.HaveLevel;
      GUI.Label(new Rect((float) ((double) position.width / 2.0 - 90.0), 124f, 200f, 14f), LocalizedStrings.ReachLevelFour + (!Singleton<ClanDataManager>.Instance.HaveLevel ? string.Empty : string.Format(" ({0})", (object) LocalizedStrings.Done)), BlueStonez.label_interparkbold_11pt_left);
      GUI.enabled = enabled && !Singleton<ClanDataManager>.Instance.HaveFriends;
      GUI.Label(new Rect((float) ((double) position.width / 2.0 - 90.0), 138f, 200f, 14f), LocalizedStrings.HaveAtLeastOneFriend + (!Singleton<ClanDataManager>.Instance.HaveFriends ? string.Empty : string.Format(" ({0})", (object) LocalizedStrings.Done)), BlueStonez.label_interparkbold_11pt_left);
      GUI.enabled = enabled && !Singleton<ClanDataManager>.Instance.HaveLicense;
      GUI.Label(new Rect((float) ((double) position.width / 2.0 - 90.0), 152f, 240f, 14f), LocalizedStrings.BuyAClanLicense + (!Singleton<ClanDataManager>.Instance.HaveLicense ? string.Empty : string.Format(" ({0})", (object) LocalizedStrings.Done)), BlueStonez.label_interparkbold_11pt_left);
      GUI.enabled = enabled;
      if (!Singleton<ClanDataManager>.Instance.HaveLicense && GUITools.Button(new Rect((float) (((double) position.width - 200.0) / 2.0), 170f, 200f, 22f), new GUIContent(LocalizedStrings.BuyAClanLicense), BlueStonez.buttondark_medium))
      {
        IUnityItem itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(1234);
        if (itemInShop != null && itemInShop.ItemView != null)
        {
          BuyPanelGUI buyPanelGui = PanelManager.Instance.OpenPanel(PanelType.BuyItem) as BuyPanelGUI;
          if ((bool) (UnityEngine.Object) buyPanelGui)
            buyPanelGui.SetItem(itemInShop, BuyingLocationType.Shop, BuyingRecommendationType.None);
        }
      }
    }
    else
      GUI.Label(new Rect(0.0f, 140f, position.width, 14f), LocalizedStrings.CreateAClanAndInviteYourFriends, BlueStonez.label_interparkbold_11pt);
    GUITools.PushGUIState();
    GUI.enabled = ((GUI.enabled ? 1 : 0) & (!Singleton<ClanDataManager>.Instance.HaveLevel || !Singleton<ClanDataManager>.Instance.HaveLicense ? 0 : (Singleton<ClanDataManager>.Instance.HaveFriends ? 1 : 0))) != 0;
    if (GUITools.Button(new Rect((float) (((double) position.width - 200.0) / 2.0), 200f, 200f, 30f), new GUIContent(LocalizedStrings.CreateAClanCaps), BlueStonez.button_green))
      this.createAClan = true;
    GUITools.PopGUIState();
    GUI.EndGroup();
  }

  private void DrawCreateClanMessage(Rect rect)
  {
    Rect position = new Rect((float) (((double) rect.width - 480.0) / 2.0), (float) (((double) rect.height - 360.0) / 2.0), 480f, 360f);
    GUI.BeginGroup(position, BlueStonez.window_standard_grey38);
    int left1 = 35;
    int left2 = 120;
    int width = 320;
    int top1 = 130;
    int top2 = 190;
    int top3 = 250;
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 56f), LocalizedStrings.CreateAClan, BlueStonez.tab_strip);
    GUI.Label(new Rect(0.0f, 60f, position.width, 20f), LocalizedStrings.HereYouCanCreateYourOwnClan, BlueStonez.label_interparkbold_18pt);
    GUI.Label(new Rect(0.0f, 80f, position.width, 40f), LocalizedStrings.YouCantChangeYourClanInfoOnceCreated, BlueStonez.label_interparkmed_11pt);
    GUI.Label(new Rect((float) left1, (float) top1, 100f, 20f), LocalizedStrings.Name, BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect((float) left1, (float) top2, 100f, 20f), LocalizedStrings.Tag, BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect((float) left1, (float) top3, 100f, 20f), LocalizedStrings.Motto, BlueStonez.label_interparkbold_18pt_left);
    this._newClanName = GUI.TextField(new Rect((float) left2, (float) top1, (float) width, 24f), this._newClanName, BlueStonez.textField);
    this._newClanTag = GUI.TextField(new Rect((float) left2, (float) top2, (float) width, 24f), this._newClanTag, BlueStonez.textField);
    this._newClanMotto = GUI.TextField(new Rect((float) left2, (float) top3, (float) width, 24f), this._newClanMotto, BlueStonez.textField);
    GUI.Label(new Rect((float) left2, (float) (top1 + 25), 300f, 20f), LocalizedStrings.ThisIsTheOfficialNameOfYourClan, BlueStonez.label_interparkmed_10pt_left);
    GUI.Label(new Rect((float) left2, (float) (top2 + 25), 300f, 20f), LocalizedStrings.ThisTagGetsDisplayedNextToYourName, BlueStonez.label_interparkmed_10pt_left);
    GUI.Label(new Rect((float) left2, (float) (top3 + 25), 300f, 20f), LocalizedStrings.ThisIsYourOfficialClanMotto, BlueStonez.label_interparkmed_10pt_left);
    if (this._newClanName.Length > 25)
      this._newClanName = this._newClanName.Remove(25);
    if (this._newClanTag.Length > 5)
      this._newClanTag = this._newClanTag.Remove(5);
    if (this._newClanMotto.Length > 25)
      this._newClanMotto = this._newClanMotto.Remove(25);
    GUITools.PushGUIState();
    GUI.enabled = ((GUI.enabled ? 1 : 0) & (this._newClanName.Length < 3 || this._newClanTag.Length < 2 ? 0 : (this._newClanMotto.Length >= 3 ? 1 : 0))) != 0;
    if (GUITools.Button(new Rect((float) ((double) position.width - 110.0 - 110.0), 310f, 100f, 30f), new GUIContent(LocalizedStrings.CreateCaps), BlueStonez.button_green))
      Singleton<ClanDataManager>.Instance.CreateNewClan(this._newClanName, this._newClanMotto, this._newClanTag);
    GUITools.PopGUIState();
    if (GUITools.Button(new Rect(position.width - 110f, 310f, 100f, 30f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button))
      this.createAClan = false;
    GUI.EndGroup();
  }

  private void SortClanMembers()
  {
    if (Singleton<PlayerDataManager>.Instance.ClanMembers == null)
      return;
    Singleton<PlayerDataManager>.Instance.ClanMembers.Sort((IComparer<ClanMemberView>) new ClanPageGUI.ClanSorter());
  }

  private bool IsNoPopupOpen() => !PanelManager.IsAnyPanelOpen && !PopupSystem.IsAnyPopupOpen;

  public class ClanCreationEvent
  {
  }

  private class ClanSorter : IComparer<ClanMemberView>
  {
    public int Compare(ClanMemberView a, ClanMemberView b) => ClanPageGUI.CompareClanFunctionList.CompareClanMembers(a, b);
  }

  private static class CompareClanFunctionList
  {
    public static int CompareClanMembers(ClanMemberView a, ClanMemberView b)
    {
      int num = ClanPageGUI.CompareClanFunctionList.ComparePosition(a, b);
      return num != 0 ? num : ClanPageGUI.CompareClanFunctionList.CompareName(a, b);
    }

    public static int ComparePosition(ClanMemberView a, ClanMemberView b)
    {
      int num1 = 0;
      int num2 = 0;
      if (a.Position == GroupPosition.Leader)
        num1 = 1;
      else if (a.Position == GroupPosition.Officer)
        num1 = 2;
      else if (a.Position == GroupPosition.Member)
        num1 = 3;
      if (b.Position == GroupPosition.Leader)
        num2 = 1;
      else if (b.Position == GroupPosition.Officer)
        num2 = 2;
      else if (b.Position == GroupPosition.Member)
        num2 = 3;
      if (num1 == num2)
        return 0;
      return num1 > num2 ? 1 : -1;
    }

    public static int CompareName(ClanMemberView a, ClanMemberView b) => string.Compare(a.Name, b.Name);
  }
}
