// Decompiled with JetBrains decompiler
// Type: PlayerDataManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Core.Types;
using UberStrike.Core.ViewModel;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
  private PlayerStatisticsView _serverLocalPlayerPlayerStatisticsView;
  private MemberView _serverLocalPlayerMemberView;
  private LoadoutView _serverLocalPlayerLoadoutView;
  private Dictionary<int, PublicProfileView> _friends = new Dictionary<int, PublicProfileView>();
  private Dictionary<int, ClanMemberView> _clanMembers = new Dictionary<int, ClanMemberView>();
  private Color _localPlayerSkinColor = Color.white;
  private SecureMemory<int> _cmid;
  private SecureMemory<string> _name;
  private SecureMemory<string> _email;
  private SecureMemory<int> _accessLevel;
  private SecureMemory<int> _clanID;
  private SecureMemory<int> _experience;
  private SecureMemory<int> _level;
  private SecureMemory<int> _points;
  private SecureMemory<int> _credits;
  private ClanView _playerClanData;
  private GroupPosition _playerClanPosition = GroupPosition.Member;
  private bool _setLoadoutEventReturnDone;

  private PlayerDataManager()
  {
    bool useAOTCompatibleMode = ApplicationDataManager.Channel == ChannelType.IPad || ApplicationDataManager.Channel == ChannelType.IPhone;
    this._cmid = new SecureMemory<int>(0, useAOTCompatibleMode: useAOTCompatibleMode);
    this._name = new SecureMemory<string>(string.Empty, useAOTCompatibleMode: useAOTCompatibleMode);
    this._email = new SecureMemory<string>(string.Empty, useAOTCompatibleMode: useAOTCompatibleMode);
    this._accessLevel = new SecureMemory<int>(0, useAOTCompatibleMode: useAOTCompatibleMode);
    this._points = new SecureMemory<int>(0, useAOTCompatibleMode: useAOTCompatibleMode);
    this._credits = new SecureMemory<int>(0, useAOTCompatibleMode: useAOTCompatibleMode);
    this._level = new SecureMemory<int>(0, useAOTCompatibleMode: useAOTCompatibleMode);
    this._experience = new SecureMemory<int>(0, useAOTCompatibleMode: useAOTCompatibleMode);
    this._clanID = new SecureMemory<int>(0, useAOTCompatibleMode: useAOTCompatibleMode);
    this._serverLocalPlayerLoadoutView = new LoadoutView();
    this._serverLocalPlayerMemberView = new MemberView();
    this._serverLocalPlayerPlayerStatisticsView = new PlayerStatisticsView();
    this._playerClanData = new ClanView();
  }

  public float GearWeight { get; private set; }

  public int FriendsCount => this._friends.Count;

  public MemberView ServerLocalPlayerMemberView
  {
    get => this._serverLocalPlayerMemberView;
    set
    {
      this._serverLocalPlayerMemberView = value;
      this._cmid.WriteData(value.PublicProfile.Cmid);
      this._accessLevel.WriteData((int) value.PublicProfile.AccessLevel);
      this._name.WriteData(value.PublicProfile.Name);
      this._points.WriteData(value.MemberWallet.Points);
      this._credits.WriteData(value.MemberWallet.Credits);
    }
  }

  public void SetPlayerStatisticsView(PlayerStatisticsView value)
  {
    if (value == null)
      return;
    this._serverLocalPlayerPlayerStatisticsView = value;
    this.UpdateSecureLevelAndXp(PlayerXpUtil.GetLevelForXp(value.Xp), value.Xp);
  }

  public PlayerStatisticsView ServerLocalPlayerStatisticsView => this._serverLocalPlayerPlayerStatisticsView;

  public static Color SkinColor => Singleton<PlayerDataManager>.Instance._localPlayerSkinColor;

  public IEnumerable<PublicProfileView> FriendList
  {
    get => (IEnumerable<PublicProfileView>) this._friends.Values;
    set
    {
      this._friends.Clear();
      if (value == null)
        return;
      foreach (PublicProfileView publicProfileView in value)
        this._friends.Add(publicProfileView.Cmid, publicProfileView);
    }
  }

  public void AddFriend(PublicProfileView view) => this._friends.Add(view.Cmid, view);

  public void RemoveFriend(int friendCmid) => this._friends.Remove(friendCmid);

  public static bool IsPlayerLoggedIn => PlayerDataManager.Cmid > 0;

  public static MemberAccessLevel AccessLevel => (MemberAccessLevel) Singleton<PlayerDataManager>.Instance._accessLevel.ReadData(false);

  public static int Cmid => Singleton<PlayerDataManager>.Instance._cmid.ReadData(false);

  public static string GroupTag => Singleton<PlayerDataManager>.Instance.ServerLocalPlayerMemberView.PublicProfile.GroupTag;

  public static string Name => Singleton<PlayerDataManager>.Instance._name.ReadData(false);

  public static string Email => Singleton<PlayerDataManager>.Instance._email.ReadData(false);

  public static int Credits => Singleton<PlayerDataManager>.Instance._credits.ReadData(false);

  public static int Points => Singleton<PlayerDataManager>.Instance._points.ReadData(false);

  public static int PlayerExperience => Singleton<PlayerDataManager>.Instance._experience.ReadData(false);

  public static int PlayerLevel => Singleton<PlayerDataManager>.Instance._level.ReadData(false);

  public static ClanView ClanData
  {
    set
    {
      Singleton<PlayerDataManager>.Instance._playerClanData = value;
      Singleton<PlayerDataManager>.Instance._clanMembers.Clear();
      if (value != null)
      {
        Singleton<PlayerDataManager>.Instance._clanID.WriteData(value.GroupId);
        int cmidSecure = PlayerDataManager.CmidSecure;
        if (value.Members == null)
          return;
        foreach (ClanMemberView member in value.Members)
        {
          Singleton<PlayerDataManager>.Instance._clanMembers[member.Cmid] = member;
          if (member.Cmid == cmidSecure)
            Singleton<PlayerDataManager>.Instance._playerClanPosition = member.Position;
        }
      }
      else
      {
        Singleton<PlayerDataManager>.Instance._clanID.WriteData(0);
        Singleton<PlayerDataManager>.Instance._clanMembers.Clear();
        Singleton<PlayerDataManager>.Instance._playerClanPosition = GroupPosition.Member;
      }
    }
  }

  public static bool IsPlayerInClan => PlayerDataManager.ClanID > 0;

  public static int ClanID
  {
    get => Singleton<PlayerDataManager>.Instance._clanID.ReadData(false);
    set => Singleton<PlayerDataManager>.Instance._clanID.WriteData(value);
  }

  public GroupPosition RankInClan
  {
    get => this._playerClanPosition;
    set => this._playerClanPosition = value;
  }

  public static string ClanName => Singleton<PlayerDataManager>.Instance._playerClanData != null ? Singleton<PlayerDataManager>.Instance._playerClanData.Name : string.Empty;

  public static string ClanTag => Singleton<PlayerDataManager>.Instance._playerClanData != null ? Singleton<PlayerDataManager>.Instance._playerClanData.Tag : string.Empty;

  public static string ClanMotto => Singleton<PlayerDataManager>.Instance._playerClanData != null ? Singleton<PlayerDataManager>.Instance._playerClanData.Motto : string.Empty;

  public static DateTime ClanFoundingDate => Singleton<PlayerDataManager>.Instance._playerClanData != null ? Singleton<PlayerDataManager>.Instance._playerClanData.FoundingDate : DateTime.Now;

  public static string ClanOwnerName => Singleton<PlayerDataManager>.Instance._playerClanData != null ? Singleton<PlayerDataManager>.Instance._playerClanData.OwnerName : string.Empty;

  public static int ClanMembersLimit => Singleton<PlayerDataManager>.Instance._playerClanData != null ? Singleton<PlayerDataManager>.Instance._playerClanData.MembersLimit : 0;

  public int ClanMembersCount => this._playerClanData != null ? this._playerClanData.Members.Count : 0;

  public List<ClanMemberView> ClanMembers => this._playerClanData != null ? this._playerClanData.Members : new List<ClanMemberView>(0);

  public static bool CanInviteToClan => Singleton<PlayerDataManager>.Instance._playerClanPosition == GroupPosition.Leader || Singleton<PlayerDataManager>.Instance._playerClanPosition == GroupPosition.Officer;

  public static MemberAccessLevel AccessLevelSecure => (MemberAccessLevel) Singleton<PlayerDataManager>.Instance._accessLevel.ReadData(true);

  public static int CmidSecure => Singleton<PlayerDataManager>.Instance._cmid.ReadData(true);

  public static string NameSecure
  {
    get => Singleton<PlayerDataManager>.Instance._name.ReadData(true);
    set => Singleton<PlayerDataManager>.Instance._name.WriteData(value);
  }

  public static string EmailSecure
  {
    get => Singleton<PlayerDataManager>.Instance._email.ReadData(true);
    set => Singleton<PlayerDataManager>.Instance._email.WriteData(value);
  }

  public static int CreditsSecure => Singleton<PlayerDataManager>.Instance._credits.ReadData(true);

  public static int PointsSecure => Singleton<PlayerDataManager>.Instance._points.ReadData(true);

  public static void AddPointsSecure(int points) => Singleton<PlayerDataManager>.Instance._points.WriteData(PlayerDataManager.PointsSecure + points);

  public static int PlayerExperienceSecure => Singleton<PlayerDataManager>.Instance._experience.ReadData(true);

  public static int PlayerLevelSecure => Singleton<PlayerDataManager>.Instance._level.ReadData(true);

  public static int ClanIDSecure => Singleton<PlayerDataManager>.Instance._clanID.ReadData(true);

  private void HandleWebServiceError()
  {
  }

  public void SetSkinColor(Color skinColor) => this._localPlayerSkinColor = skinColor;

  private LoadoutView CreateLocalPlayerLoadoutView() => new LoadoutView(this._serverLocalPlayerLoadoutView.LoadoutId, this._serverLocalPlayerLoadoutView.Backpack, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearBoots), this._serverLocalPlayerLoadoutView.Cmid, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearFace), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem1), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem2), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem3), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearGloves), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearHead), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearLowerBody), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponMelee), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem1), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem2), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem3), AvatarType.LutzRavinoff, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearUpperBody), Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponPrimary), this._serverLocalPlayerLoadoutView.Weapon1Mod1, this._serverLocalPlayerLoadoutView.Weapon1Mod2, this._serverLocalPlayerLoadoutView.Weapon1Mod3, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponSecondary), this._serverLocalPlayerLoadoutView.Weapon2Mod1, this._serverLocalPlayerLoadoutView.Weapon2Mod2, this._serverLocalPlayerLoadoutView.Weapon2Mod3, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.WeaponTertiary), this._serverLocalPlayerLoadoutView.Weapon3Mod1, this._serverLocalPlayerLoadoutView.Weapon3Mod2, this._serverLocalPlayerLoadoutView.Weapon3Mod3, Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearHolo), ColorConverter.ColorToHex(this._localPlayerSkinColor));

  [DebuggerHidden]
  public IEnumerator StartGetMemberWallet() => (IEnumerator) new PlayerDataManager.\u003CStartGetMemberWallet\u003Ec__Iterator75()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  public IEnumerator StartSetLoadout() => (IEnumerator) new PlayerDataManager.\u003CStartSetLoadout\u003Ec__Iterator76()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  public IEnumerator StartGetLoadout() => (IEnumerator) new PlayerDataManager.\u003CStartGetLoadout\u003Ec__Iterator77()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  public IEnumerator StartGetMember() => (IEnumerator) new PlayerDataManager.\u003CStartGetMember\u003Ec__Iterator78()
  {
    \u003C\u003Ef__this = this
  };

  private void OnGetMemberWalletEventReturn(MemberWalletView ev)
  {
    this.NotifyPointsAndCreditsChanges(ev.Points, ev.Credits);
    this._serverLocalPlayerMemberView.MemberWallet = ev;
    this.UpdateSecurePointsAndCredits(ev.Points, ev.Credits);
  }

  private void OnGetMemberEventReturn(UberstrikeUserViewModel ev)
  {
    this.NotifyPointsAndCreditsChanges(ev.CmuneMemberView.MemberWallet.Points, ev.CmuneMemberView.MemberWallet.Credits);
    this.SetPlayerStatisticsView(ev.UberstrikeMemberView.PlayerStatisticsView);
    this.ServerLocalPlayerMemberView = ev.CmuneMemberView;
  }

  private void NotifyPointsAndCreditsChanges(int newPoints, int newCredits)
  {
    int num1 = this._points.ReadData(true);
    int num2 = this._credits.ReadData(true);
    if (newPoints != num1)
      GlobalUIRibbon.Instance.AddPointsEvent(newPoints - num1);
    if (newCredits == num2)
      return;
    GlobalUIRibbon.Instance.AddCreditsEvent(newCredits - num2);
  }

  public void AttributeXp(int xp)
  {
    int xp1 = PlayerDataManager.PlayerExperienceSecure + xp;
    int levelForXp = PlayerXpUtil.GetLevelForXp(xp1);
    this._serverLocalPlayerPlayerStatisticsView.Xp = xp1;
    this._serverLocalPlayerPlayerStatisticsView.Level = levelForXp;
    this.UpdateSecureLevelAndXp(levelForXp, xp1);
  }

  private void UpdateSecureLevelAndXp(int level, int xp)
  {
    GlobalUIRibbon.Instance.UpdateLevelBounds(level);
    this._experience.WriteData(xp);
    this._level.WriteData(level);
  }

  public void UpdateSecurePointsAndCredits(int points, int credits)
  {
    this._points.WriteData(points);
    this._credits.WriteData(credits);
  }

  public void CheckLoadoutForExpiredItems()
  {
    LoadoutView playerLoadoutView = this._serverLocalPlayerLoadoutView;
    this._serverLocalPlayerLoadoutView = new LoadoutView(playerLoadoutView.LoadoutId, !this.IsExpired(playerLoadoutView.Backpack, "Backpack") ? playerLoadoutView.Backpack : 0, !this.IsExpired(playerLoadoutView.Boots, "Boots") ? playerLoadoutView.Boots : 0, playerLoadoutView.Cmid, !this.IsExpired(playerLoadoutView.Face, "Face") ? playerLoadoutView.Face : 0, !this.IsExpired(playerLoadoutView.FunctionalItem1, "FunctionalItem1") ? playerLoadoutView.FunctionalItem1 : 0, !this.IsExpired(playerLoadoutView.FunctionalItem2, "FunctionalItem2") ? playerLoadoutView.FunctionalItem2 : 0, !this.IsExpired(playerLoadoutView.FunctionalItem3, "FunctionalItem3") ? playerLoadoutView.FunctionalItem3 : 0, !this.IsExpired(playerLoadoutView.Gloves, "Gloves") ? playerLoadoutView.Gloves : 0, !this.IsExpired(playerLoadoutView.Head, "Head") ? playerLoadoutView.Head : 0, !this.IsExpired(playerLoadoutView.LowerBody, "LowerBody") ? playerLoadoutView.LowerBody : 0, !this.IsExpired(playerLoadoutView.MeleeWeapon, "MeleeWeapon") ? playerLoadoutView.MeleeWeapon : 0, !this.IsExpired(playerLoadoutView.QuickItem1, "QuickItem1") ? playerLoadoutView.QuickItem1 : 0, !this.IsExpired(playerLoadoutView.QuickItem2, "QuickItem2") ? playerLoadoutView.QuickItem2 : 0, !this.IsExpired(playerLoadoutView.QuickItem3, "QuickItem3") ? playerLoadoutView.QuickItem3 : 0, playerLoadoutView.Type, !this.IsExpired(playerLoadoutView.UpperBody, "UpperBody") ? playerLoadoutView.UpperBody : 0, !this.IsExpired(playerLoadoutView.Weapon1, "Weapon1") ? playerLoadoutView.Weapon1 : 0, !this.IsExpired(playerLoadoutView.Weapon1Mod1, "Weapon1Mod1") ? playerLoadoutView.Weapon1Mod1 : 0, !this.IsExpired(playerLoadoutView.Weapon1Mod2, "Weapon1Mod2") ? playerLoadoutView.Weapon1Mod2 : 0, !this.IsExpired(playerLoadoutView.Weapon1Mod3, "Weapon1Mod3") ? playerLoadoutView.Weapon1Mod3 : 0, !this.IsExpired(playerLoadoutView.Weapon2, "Weapon2") ? playerLoadoutView.Weapon2 : 0, !this.IsExpired(playerLoadoutView.Weapon2Mod1, "Weapon2Mod1") ? playerLoadoutView.Weapon2Mod1 : 0, !this.IsExpired(playerLoadoutView.Weapon2Mod2, "Weapon2Mod2") ? playerLoadoutView.Weapon2Mod2 : 0, !this.IsExpired(playerLoadoutView.Weapon2Mod3, "Weapon2Mod3") ? playerLoadoutView.Weapon2Mod3 : 0, !this.IsExpired(playerLoadoutView.Weapon3, "Weapon3") ? playerLoadoutView.Weapon3 : 0, !this.IsExpired(playerLoadoutView.Weapon3Mod1, "Weapon3Mod1") ? playerLoadoutView.Weapon3Mod1 : 0, !this.IsExpired(playerLoadoutView.Weapon3Mod2, "Weapon3Mod2") ? playerLoadoutView.Weapon3Mod2 : 0, !this.IsExpired(playerLoadoutView.Weapon3Mod3, "Weapon3Mod3") ? playerLoadoutView.Weapon3Mod3 : 0, !this.IsExpired(playerLoadoutView.Webbing, "Webbing") ? playerLoadoutView.Webbing : 0, playerLoadoutView.SkinColor);
  }

  private bool IsExpired(int itemId, string debug) => !Singleton<InventoryManager>.Instance.IsItemInInventory(itemId);

  public bool ValidateMemberData() => this._serverLocalPlayerMemberView.PublicProfile.Cmid > 0 && this._serverLocalPlayerPlayerStatisticsView.Cmid > 0;

  public static LoadoutSlotType GetSlotTypeForItemClass(UberstrikeItemClass itemClass)
  {
    LoadoutSlotType typeForItemClass = LoadoutSlotType.None;
    switch (itemClass)
    {
      case UberstrikeItemClass.GearBoots:
        typeForItemClass = LoadoutSlotType.GearBoots;
        break;
      case UberstrikeItemClass.GearHead:
        typeForItemClass = LoadoutSlotType.GearHead;
        break;
      case UberstrikeItemClass.GearFace:
        typeForItemClass = LoadoutSlotType.GearFace;
        break;
      case UberstrikeItemClass.GearUpperBody:
        typeForItemClass = LoadoutSlotType.GearUpperBody;
        break;
      case UberstrikeItemClass.GearLowerBody:
        typeForItemClass = LoadoutSlotType.GearLowerBody;
        break;
      case UberstrikeItemClass.GearGloves:
        typeForItemClass = LoadoutSlotType.GearGloves;
        break;
      case UberstrikeItemClass.GearHolo:
        typeForItemClass = LoadoutSlotType.GearHolo;
        break;
    }
    return typeForItemClass;
  }

  public static bool IsClanMember(int cmid) => Singleton<PlayerDataManager>.Instance._clanMembers.ContainsKey(cmid);

  public static bool IsFriend(int cmid) => Singleton<PlayerDataManager>.Instance._friends.ContainsKey(cmid);

  public static bool TryGetFriend(int cmid, out PublicProfileView view) => Singleton<PlayerDataManager>.Instance._friends.TryGetValue(cmid, out view) && view != null;

  public static bool TryGetClanMember(int cmid, out ClanMemberView view) => Singleton<PlayerDataManager>.Instance._clanMembers.TryGetValue(cmid, out view) && view != null;
}
