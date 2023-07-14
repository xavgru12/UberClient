// Decompiled with JetBrains decompiler
// Type: StatsPageGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class StatsPageGUI : MonoBehaviour
{
  private const int StatsWidth = 490;
  [SerializeField]
  private Texture2D _mostSplatsIcon;
  [SerializeField]
  private Texture2D _mostXPEarnedIcon;
  [SerializeField]
  private Texture2D _mostHealthPickedUpIcon;
  [SerializeField]
  private Texture2D _mostArmorPickedUpIcon;
  [SerializeField]
  private Texture2D _mostDamageDealtIcon;
  [SerializeField]
  private Texture2D _mostDamageReceivedIcon;
  [SerializeField]
  private Texture2D _mostHeadshotsIcon;
  [SerializeField]
  private Texture2D _mostNutshotsIcon;
  [SerializeField]
  private Texture2D _mostConsecutiveSnipesIcon;
  [SerializeField]
  private Texture2D _mostMeleeSplatsIcon;
  [SerializeField]
  private Texture2D _mostHandgunSplatsIcon;
  [SerializeField]
  private Texture2D _mostMachinegunSplatsIcon;
  [SerializeField]
  private Texture2D _mostCannonSplatsIcon;
  [SerializeField]
  private Texture2D _mostShotgunSplatsIcon;
  [SerializeField]
  private Texture2D _mostSniperSplatsIcon;
  [SerializeField]
  private Texture2D _mostSplattergunSplatsIcon;
  [SerializeField]
  private Texture2D _mostLauncherSplatsIcon;
  private Vector2 _scrollGeneral;
  private Vector2 _filterScroll;
  private Rect statsPage;
  private int _selectedStatsTab;
  private int _playerCurrentLevelXpReq;
  private int _playerNextLevelXpReq;
  private Dictionary<string, Texture2D> _weaponIcons;
  private CmunePairList<float, string> _weaponStatList;
  private bool _isFilterDropDownOpen;
  private int _selectedFilterIndex;
  private float _maxWeaponStat;
  private float _xpSliderPos;
  private GUIContent[] _statsTabs;
  private string[] _selectionsToShow;
  private StatsPageGUI.StatisticValueType _currentStatsType;
  private float statsPositionX;

  private void Awake()
  {
    this._weaponStatList = new CmunePairList<float, string>();
    CmuneEventHandler.AddListener<LogoutEvent>((Action<LogoutEvent>) (ev =>
    {
      this._selectedStatsTab = 0;
      this._selectedFilterIndex = 0;
    }));
  }

  [DebuggerHidden]
  private IEnumerator ScrollStatsFromRight(float time) => (IEnumerator) new StatsPageGUI.\u003CScrollStatsFromRight\u003Ec__Iterator1B()
  {
    time = time,
    \u003C\u0024\u003Etime = time,
    \u003C\u003Ef__this = this
  };

  private void Start()
  {
    this._statsTabs = new GUIContent[3]
    {
      new GUIContent(LocalizedStrings.Personal),
      new GUIContent(LocalizedStrings.Weapons),
      new GUIContent("Account")
    };
    this._selectionsToShow = new string[4]
    {
      LocalizedStrings.Damage,
      LocalizedStrings.Kills,
      LocalizedStrings.Accuracy,
      LocalizedStrings.Hits
    };
    this._weaponIcons = new Dictionary<string, Texture2D>()
    {
      {
        LocalizedStrings.MeleeWeapons,
        this._mostMeleeSplatsIcon
      },
      {
        LocalizedStrings.Handguns,
        this._mostHandgunSplatsIcon
      },
      {
        LocalizedStrings.Machineguns,
        this._mostMachinegunSplatsIcon
      },
      {
        LocalizedStrings.Cannons,
        this._mostCannonSplatsIcon
      },
      {
        LocalizedStrings.Shotguns,
        this._mostShotgunSplatsIcon
      },
      {
        LocalizedStrings.Splatterguns,
        this._mostSplattergunSplatsIcon
      },
      {
        LocalizedStrings.Launchers,
        this._mostLauncherSplatsIcon
      },
      {
        LocalizedStrings.SniperRifles,
        this._mostSniperSplatsIcon
      }
    };
  }

  private void OnGUI()
  {
    GUI.depth = 11;
    GUI.skin = BlueStonez.Skin;
    this.statsPage = new Rect((float) Screen.width - this.statsPositionX, (float) GlobalUIRibbon.Instance.Height(), this.statsPositionX, (float) (Screen.height - GlobalUIRibbon.Instance.Height()));
    GUI.BeginGroup(this.statsPage, GUIContent.none, BlueStonez.window_standard_grey38);
    GUI.Label(new Rect(0.0f, 0.0f, this.statsPage.width, 56f), LocalizedStrings.YourProfileCaps, BlueStonez.tab_strip);
    GUI.changed = false;
    this._selectedStatsTab = UnityGUI.Toolbar(new Rect(0.0f, 34f, 260f, 22f), this._selectedStatsTab, this._statsTabs, 3, BlueStonez.tab_medium);
    if (GUI.changed)
    {
      if (this._selectedStatsTab == 2)
        Singleton<TransactionHistory>.Instance.GetCurrentTransactions();
      SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
    }
    GUI.BeginGroup(new Rect(0.0f, 55f, this.statsPage.width, this.statsPage.height - 55f), string.Empty, BlueStonez.window_standard_grey38);
    switch (this._selectedStatsTab)
    {
      case 0:
        this.DrawPersonalStatsTab(new Rect(0.0f, 0.0f, this.statsPage.width, this.statsPage.height - 56f));
        break;
      case 1:
        this.DrawWeaponsStatsTab(new Rect(0.0f, 0.0f, this.statsPage.width, this.statsPage.height - 56f));
        break;
      case 2:
        this.DrawAccountStatsTab(new Rect(0.0f, 0.0f, this.statsPage.width, this.statsPage.height - 55f));
        break;
    }
    GUI.EndGroup();
    GUI.EndGroup();
  }

  private void OnEnable()
  {
    PlayerXpUtil.GetXpRangeForLevel(PlayerDataManager.PlayerLevelSecure, out this._playerCurrentLevelXpReq, out this._playerNextLevelXpReq);
    this.StartCoroutine(this.ScrollStatsFromRight(0.25f));
    this.UpdateWeaponStatList();
    if (!((UnityEngine.Object) MouseOrbit.Instance != (UnityEngine.Object) null))
      return;
    MouseOrbit.Instance.MaxX = Screen.width - 490;
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) MouseOrbit.Instance != (UnityEngine.Object) null))
      return;
    MouseOrbit.Instance.MaxX = Screen.width;
  }

  private void DrawWeaponsStatsTab(Rect rect)
  {
    bool enabled = GUI.enabled;
    GUI.enabled = !this._isFilterDropDownOpen;
    GUI.changed = false;
    int num1 = UnityGUI.Toolbar(new Rect(2f, 5f, rect.width - 4f, 22f), this._selectedFilterIndex, this._selectionsToShow, 4, BlueStonez.tab_medium);
    if (GUI.changed)
      SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
    if (num1 != this._selectedFilterIndex)
    {
      this._selectedFilterIndex = num1;
      this.UpdateWeaponStatList();
    }
    string title = LocalizedStrings.WeaponPerformaceTotal;
    switch (num1)
    {
      case 0:
        title = LocalizedStrings.BestWeaponByDamageDealt;
        break;
      case 1:
        title = LocalizedStrings.BestWeaponByKills;
        break;
      case 2:
        title = LocalizedStrings.BestWeaponByAccuracy;
        break;
      case 3:
        title = LocalizedStrings.BestWeaponByHits;
        break;
    }
    this._scrollGeneral = GUITools.BeginScrollView(new Rect(0.0f, 26f, rect.width - 2f, rect.height - 26f), this._scrollGeneral, new Rect(0.0f, 0.0f, 340f, 680f));
    this.DrawGroupControl(new Rect(14f, 16f, rect.width - 40f, 646f), title, BlueStonez.label_group_interparkbold_18pt);
    int width = Mathf.RoundToInt((float) (((double) this.statsPage.width - 80.0) * 0.5));
    int num2 = 0;
    foreach (KeyValuePair<float, string> weaponStat in (List<KeyValuePair<float, string>>) this._weaponStatList)
    {
      float barValue = num1 != 2 ? ((double) this._maxWeaponStat <= 0.0 ? 0.0f : weaponStat.Key / this._maxWeaponStat) : weaponStat.Key / 100f;
      this.DrawWeaponStat(new Rect(36f, (float) (32 + num2 * 76), (float) width, 60f), weaponStat.Value, weaponStat.Key, barValue, this._weaponIcons[weaponStat.Value]);
      ++num2;
    }
    GUITools.EndScrollView();
    GUI.enabled = enabled;
  }

  private void UpdateWeaponStatList()
  {
    ((List<KeyValuePair<float, string>>) this._weaponStatList).Clear();
    PlayerWeaponStatisticsView weaponStatistics = Singleton<PlayerDataManager>.Instance.ServerLocalPlayerStatisticsView.WeaponStatistics;
    switch (this._selectedFilterIndex)
    {
      case 0:
        this._weaponStatList.Add((float) weaponStatistics.MeleeTotalDamageDone, LocalizedStrings.MeleeWeapons);
        this._weaponStatList.Add((float) weaponStatistics.HandgunTotalDamageDone, LocalizedStrings.Handguns);
        this._weaponStatList.Add((float) weaponStatistics.MachineGunTotalDamageDone, LocalizedStrings.Machineguns);
        this._weaponStatList.Add((float) weaponStatistics.CannonTotalDamageDone, LocalizedStrings.Cannons);
        this._weaponStatList.Add((float) weaponStatistics.ShotgunTotalDamageDone, LocalizedStrings.Shotguns);
        this._weaponStatList.Add((float) weaponStatistics.SplattergunTotalDamageDone, LocalizedStrings.Splatterguns);
        this._weaponStatList.Add((float) weaponStatistics.LauncherTotalDamageDone, LocalizedStrings.Launchers);
        this._weaponStatList.Add((float) weaponStatistics.SniperTotalDamageDone, LocalizedStrings.SniperRifles);
        this._currentStatsType = StatsPageGUI.StatisticValueType.Counter;
        break;
      case 1:
        this._weaponStatList.Add((float) weaponStatistics.MeleeTotalSplats, LocalizedStrings.MeleeWeapons);
        this._weaponStatList.Add((float) weaponStatistics.HandgunTotalSplats, LocalizedStrings.Handguns);
        this._weaponStatList.Add((float) weaponStatistics.MachineGunTotalSplats, LocalizedStrings.Machineguns);
        this._weaponStatList.Add((float) weaponStatistics.CannonTotalSplats, LocalizedStrings.Cannons);
        this._weaponStatList.Add((float) weaponStatistics.ShotgunTotalSplats, LocalizedStrings.Shotguns);
        this._weaponStatList.Add((float) weaponStatistics.SplattergunTotalSplats, LocalizedStrings.Splatterguns);
        this._weaponStatList.Add((float) weaponStatistics.LauncherTotalSplats, LocalizedStrings.Launchers);
        this._weaponStatList.Add((float) weaponStatistics.SniperTotalSplats, LocalizedStrings.SniperRifles);
        this._currentStatsType = StatsPageGUI.StatisticValueType.Counter;
        break;
      case 2:
        this._weaponStatList.Add(weaponStatistics.MeleeTotalShotsHit != 0 ? Mathf.Clamp01((float) weaponStatistics.MeleeTotalShotsHit / (float) weaponStatistics.MeleeTotalShotsFired) * 100f : 0.0f, LocalizedStrings.MeleeWeapons);
        this._weaponStatList.Add(weaponStatistics.HandgunTotalShotsHit != 0 ? Mathf.Clamp01((float) weaponStatistics.HandgunTotalShotsHit / (float) weaponStatistics.HandgunTotalShotsFired) * 100f : 0.0f, LocalizedStrings.Handguns);
        this._weaponStatList.Add(weaponStatistics.MachineGunTotalShotsHit != 0 ? Mathf.Clamp01((float) weaponStatistics.MachineGunTotalShotsHit / (float) weaponStatistics.MachineGunTotalShotsFired) * 100f : 0.0f, LocalizedStrings.Machineguns);
        this._weaponStatList.Add(weaponStatistics.CannonTotalShotsHit != 0 ? Mathf.Clamp01((float) weaponStatistics.CannonTotalShotsHit / (float) weaponStatistics.CannonTotalShotsFired) * 100f : 0.0f, LocalizedStrings.Cannons);
        this._weaponStatList.Add(weaponStatistics.ShotgunTotalShotsHit != 0 ? Mathf.Clamp01((float) weaponStatistics.ShotgunTotalShotsHit / (float) weaponStatistics.ShotgunTotalShotsFired) * 100f : 0.0f, LocalizedStrings.Shotguns);
        this._weaponStatList.Add(weaponStatistics.SplattergunTotalShotsHit != 0 ? Mathf.Clamp01((float) weaponStatistics.SplattergunTotalShotsHit / (float) weaponStatistics.SplattergunTotalShotsFired) * 100f : 0.0f, LocalizedStrings.Splatterguns);
        this._weaponStatList.Add(weaponStatistics.LauncherTotalShotsHit != 0 ? Mathf.Clamp01((float) weaponStatistics.LauncherTotalShotsHit / (float) weaponStatistics.LauncherTotalShotsFired) * 100f : 0.0f, LocalizedStrings.Launchers);
        this._weaponStatList.Add(weaponStatistics.SniperTotalShotsHit != 0 ? Mathf.Clamp01((float) weaponStatistics.SniperTotalShotsHit / (float) weaponStatistics.SniperTotalShotsFired) * 100f : 0.0f, LocalizedStrings.SniperRifles);
        this._currentStatsType = StatsPageGUI.StatisticValueType.Percent;
        break;
      case 3:
        this._weaponStatList.Add((float) weaponStatistics.MeleeTotalShotsHit, LocalizedStrings.MeleeWeapons);
        this._weaponStatList.Add((float) weaponStatistics.HandgunTotalShotsHit, LocalizedStrings.Handguns);
        this._weaponStatList.Add((float) weaponStatistics.MachineGunTotalShotsHit, LocalizedStrings.Machineguns);
        this._weaponStatList.Add((float) weaponStatistics.CannonTotalShotsHit, LocalizedStrings.Cannons);
        this._weaponStatList.Add((float) weaponStatistics.ShotgunTotalShotsHit, LocalizedStrings.Shotguns);
        this._weaponStatList.Add((float) weaponStatistics.SplattergunTotalShotsHit, LocalizedStrings.Splatterguns);
        this._weaponStatList.Add((float) weaponStatistics.LauncherTotalShotsHit, LocalizedStrings.Launchers);
        this._weaponStatList.Add((float) weaponStatistics.SniperTotalShotsHit, LocalizedStrings.SniperRifles);
        this._currentStatsType = StatsPageGUI.StatisticValueType.Counter;
        break;
    }
    ((List<KeyValuePair<float, string>>) this._weaponStatList).Sort((Comparison<KeyValuePair<float, string>>) ((a, b) => -a.Key.CompareTo(b.Key)));
    this._maxWeaponStat = 0.0f;
    foreach (KeyValuePair<float, string> weaponStat in (List<KeyValuePair<float, string>>) this._weaponStatList)
    {
      if ((double) weaponStat.Key > (double) this._maxWeaponStat)
        this._maxWeaponStat = weaponStat.Key;
    }
  }

  private void DebugWeaponStatistics()
  {
    int num = 0;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<float, string> weaponStat in (List<KeyValuePair<float, string>>) this._weaponStatList)
      stringBuilder.AppendLine(num++.ToString() + ": " + (object) weaponStat.Key + " " + weaponStat.Value);
  }

  private void DrawPersonalStatsTab(Rect rect)
  {
    this._scrollGeneral = GUITools.BeginScrollView(rect, this._scrollGeneral, new Rect(0.0f, 0.0f, 340f, 915f));
    int width = Mathf.RoundToInt((float) (((double) rect.width - 80.0) * 0.5));
    PlayerPersonalRecordStatisticsView personalRecord = Singleton<PlayerDataManager>.Instance.ServerLocalPlayerStatisticsView.PersonalRecord;
    this.DrawGroupControl(new Rect(14f, 16f, rect.width - 40f, 100f), LocalizedStrings.LevelAndXP, BlueStonez.label_group_interparkbold_18pt);
    this.DrawXPMeter(new Rect(24f, 32f, rect.width - 60f, 64f));
    this.DrawGroupControl(new Rect(14f, 142f, rect.width - 40f, 405f), LocalizedStrings.PersonalRecordsPerLife, BlueStonez.label_group_interparkbold_18pt);
    this.DrawPersonalStat(36, 158, width, LocalizedStrings.MostKills, personalRecord.MostSplats.ToString(), this._mostSplatsIcon);
    this.DrawPersonalStat(36, 234, width, LocalizedStrings.MostDamageDealt, personalRecord.MostDamageDealt.ToString(), this._mostDamageDealtIcon);
    this.DrawPersonalStat(36, 310, width, LocalizedStrings.MostHealthPickedUp, personalRecord.MostHealthPickedUp.ToString(), this._mostHealthPickedUpIcon);
    this.DrawPersonalStat(36, 386, width, LocalizedStrings.MostHeadshots, personalRecord.MostHeadshots.ToString(), this._mostHeadshotsIcon);
    this.DrawPersonalStat(36, 462, width, LocalizedStrings.MostConsecutiveSnipes, personalRecord.MostConsecutiveSnipes.ToString(), this._mostConsecutiveSnipesIcon);
    this.DrawPersonalStat(36 + width, 158, width, LocalizedStrings.MostXPEarned, personalRecord.MostXPEarned.ToString(), this._mostXPEarnedIcon);
    this.DrawPersonalStat(36 + width, 234, width, LocalizedStrings.MostDamageReceived, personalRecord.MostDamageReceived.ToString(), this._mostDamageReceivedIcon);
    this.DrawPersonalStat(36 + width, 310, width, LocalizedStrings.MostArmorPickedUp, personalRecord.MostArmorPickedUp.ToString(), this._mostArmorPickedUpIcon);
    this.DrawPersonalStat(36 + width, 386, width, LocalizedStrings.MostNutshots, personalRecord.MostNutshots.ToString(), this._mostNutshotsIcon);
    this.DrawGroupControl(new Rect(14f, 575f, this.statsPage.width - 40f, 328f), "Weapon Records (per Life)", BlueStonez.label_group_interparkbold_18pt);
    this.DrawPersonalStat(36, 593, width, LocalizedStrings.MostMeleeKills, personalRecord.MostMeleeSplats.ToString(), this._mostMeleeSplatsIcon);
    this.DrawPersonalStat(36, 669, width, LocalizedStrings.MostMachinegunKills, personalRecord.MostMachinegunSplats.ToString(), this._mostMachinegunSplatsIcon);
    this.DrawPersonalStat(36, 745, width, LocalizedStrings.MostShotgunKills, personalRecord.MostShotgunSplats.ToString(), this._mostShotgunSplatsIcon);
    this.DrawPersonalStat(36, 821, width, LocalizedStrings.MostSplattergunKills, personalRecord.MostSplattergunSplats.ToString(), this._mostSplattergunSplatsIcon);
    this.DrawPersonalStat(36 + width, 593, width, LocalizedStrings.MostHandgunKills, personalRecord.MostHandgunSplats.ToString(), this._mostHandgunSplatsIcon);
    this.DrawPersonalStat(36 + width, 669, width, LocalizedStrings.MostCannonKills, personalRecord.MostCannonSplats.ToString(), this._mostCannonSplatsIcon);
    this.DrawPersonalStat(36 + width, 745, width, LocalizedStrings.MostSniperRifleKills, personalRecord.MostSniperSplats.ToString(), this._mostSniperSplatsIcon);
    this.DrawPersonalStat(36 + width, 821, width, LocalizedStrings.MostLauncherKills, personalRecord.MostLauncherSplats.ToString(), this._mostLauncherSplatsIcon);
    GUITools.EndScrollView();
  }

  private void DrawAccountStatsTab(Rect rect) => Singleton<TransactionHistory>.Instance.DrawPanel(rect);

  private void DoDropDownList(Rect position)
  {
    Rect rect = new Rect(position.x, position.y, position.width - position.height, position.height);
    if (GUI.Button(new Rect((float) ((double) position.x + (double) position.width - (double) position.height - 2.0), position.y - 1f, position.height, position.height), GUIContent.none, BlueStonez.dropdown_button))
      this._isFilterDropDownOpen = !this._isFilterDropDownOpen;
    if (this._isFilterDropDownOpen)
    {
      Rect position1 = new Rect(position.x, position.y + position.height, position.width - position.height, (float) (this._selectionsToShow.Length * 20 + 1));
      GUI.Box(position1, string.Empty, BlueStonez.window_standard_grey38);
      this._filterScroll = GUITools.BeginScrollView(position1, this._filterScroll, new Rect(0.0f, 0.0f, position1.width - 20f, (float) (this._selectionsToShow.Length * 20)));
      for (int index = 0; index < this._selectionsToShow.Length; ++index)
      {
        GUI.Label(new Rect(4f, (float) (index * 20), position1.width, 20f), this._selectionsToShow[index], BlueStonez.label_interparkbold_11pt_left);
        if (GUI.Button(new Rect(2f, (float) (index * 20), position1.width, 20f), string.Empty, BlueStonez.dropdown_list))
        {
          this._isFilterDropDownOpen = false;
          this._selectedFilterIndex = index;
          this.UpdateWeaponStatList();
        }
      }
      GUITools.EndScrollView();
    }
    else
    {
      if (!GUITools.Button(rect, new GUIContent(this._selectionsToShow[this._selectedFilterIndex]), BlueStonez.label_dropdown))
        return;
      this._isFilterDropDownOpen = !this._isFilterDropDownOpen;
    }
  }

  private void DrawPersonalStat(
    int x,
    int y,
    int width,
    string statName,
    string statValue,
    Texture2D icon)
  {
    GUI.Label(new Rect((float) x, (float) y, (float) width, 20f), statName, BlueStonez.label_interparkbold_13pt_left);
    GUI.DrawTexture(new Rect((float) x, (float) (y + 22), 48f, 48f), (Texture) icon);
    GUI.Label(new Rect((float) (x + 54), (float) (y + 36), (float) (width - 54), 20f), statValue, BlueStonez.label_interparkbold_18pt_left);
  }

  private void DrawWeaponStat(
    Rect position,
    string statName,
    float statValue,
    float barValue,
    Texture2D icon)
  {
    GUI.Label(new Rect(position.x, position.y, position.width, 20f), statName, BlueStonez.label_interparkmed_18pt_left);
    GUI.DrawTexture(new Rect(position.x, position.y + 22f, 48f, 48f), (Texture) icon);
    this.DrawLevelBar(new Rect(position.x + 54f, position.y + 32f, position.width - 54f, 12f), barValue, ColorScheme.ProgressBar);
    string text = this._currentStatsType != StatsPageGUI.StatisticValueType.Percent ? statValue.ToString("f0") : statValue.ToString("f1") + "%";
    GUI.Label(new Rect(position.x + 54f, position.y + 48f, position.width - 54f, 20f), text, BlueStonez.label_interparkbold_11pt_left);
  }

  private void DrawLevelBar(Rect position, float amount, Color barColor)
  {
    GUI.BeginGroup(position);
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 12f), GUIContent.none, BlueStonez.progressbar_background);
    GUI.color = barColor;
    GUI.Label(new Rect(2f, 2f, (position.width - 4f) * Mathf.Clamp01(amount), 8f), GUIContent.none, BlueStonez.progressbar_thumb);
    GUI.color = Color.white;
    GUI.EndGroup();
  }

  private void DrawXPMeter(Rect position)
  {
    float num = (float) (this._playerNextLevelXpReq - this._playerCurrentLevelXpReq);
    this._xpSliderPos = (double) num <= 0.0 ? 0.0f : Mathf.Clamp01((float) (PlayerDataManager.PlayerExperience - this._playerCurrentLevelXpReq) / num);
    GUI.BeginGroup(position);
    if (PlayerDataManager.PlayerLevel < PlayerXpUtil.MaxPlayerLevel)
    {
      GUI.Label(new Rect(0.0f, 0.0f, 200f, 16f), LocalizedStrings.CurrentXP + " " + (object) PlayerDataManager.PlayerExperience, BlueStonez.label_interparkbold_11pt_left);
      GUI.Label(new Rect(position.width - 200f, 0.0f, 200f, 16f), LocalizedStrings.RemainingXP + " " + (object) Mathf.Max(0, this._playerNextLevelXpReq - PlayerDataManager.PlayerExperience), BlueStonez.label_interparkbold_11pt_right);
      GUI.Box(new Rect(0.0f, 25f, position.width, 23f), GUIContent.none, BlueStonez.progressbar_large_background);
      GUI.color = ColorScheme.ProgressBar;
      GUI.Box(new Rect(2f, 27f, (float) Mathf.RoundToInt((position.width - 4f) * this._xpSliderPos), 19f), GUIContent.none, BlueStonez.progressbar_large_thumb);
      GUI.color = Color.white;
      GUI.Label(new Rect(0.0f, 50f, position.width, 16f), PlayerXpUtil.GetLevelDescription(PlayerDataManager.PlayerLevel), BlueStonez.label_interparkbold_11pt_left);
      GUI.Label(new Rect(0.0f, 50f, position.width, 16f), PlayerXpUtil.GetLevelDescription(PlayerDataManager.PlayerLevel + 1), BlueStonez.label_interparkbold_11pt_right);
    }
    else
    {
      GUI.Label(new Rect(0.0f, 0.0f, 200f, 16f), LocalizedStrings.CurrentXP + " " + (object) PlayerDataManager.PlayerExperience, BlueStonez.label_interparkbold_11pt_left);
      GUI.Label(new Rect(position.width - 200f, 0.0f, 200f, 16f), "(Lvl " + (object) PlayerDataManager.PlayerLevel + ")", BlueStonez.label_interparkbold_11pt_right);
      GUI.color = new Color(0.0f, 0.607f, 0.662f);
      GUI.Box(new Rect(0.0f, 30f, position.width, 23f), "YOU ARE FLOATING IN UBER SPACE", BlueStonez.label_interparkbold_18pt);
      GUI.color = Color.white;
    }
    GUI.EndGroup();
  }

  private void DrawStatLabel(Rect position, string label, string text) => GUI.Label(position, label + ": " + text, BlueStonez.label_interparkbold_16pt);

  private Rect CenteredAspectRect(
    float aspectRatio,
    int screenWidth,
    int screenHeight,
    int offsetTop,
    int minpWidth,
    int minHeight)
  {
    Rect rect;
    if ((double) ((float) screenWidth / (float) screenHeight) > (double) aspectRatio)
    {
      int width = Mathf.Clamp(Mathf.RoundToInt((float) screenHeight * aspectRatio), minpWidth, 2048);
      rect = new Rect((float) (screenWidth - width) * 0.5f, (float) offsetTop, (float) width, (float) Mathf.Clamp(screenHeight, minHeight, 2048));
    }
    else
      rect = new Rect(0.0f, (float) offsetTop, (float) Mathf.Clamp(screenWidth, minpWidth, 2048), (float) Mathf.Clamp(Mathf.RoundToInt((float) screenWidth / aspectRatio), minHeight, 2048));
    return rect;
  }

  private void DrawGroupControl(Rect rect, string title, GUIStyle style)
  {
    GUI.BeginGroup(rect, string.Empty, BlueStonez.group_grey81);
    GUI.EndGroup();
    GUI.Label(new Rect(rect.x + 18f, rect.y - 8f, style.CalcSize(new GUIContent(title)).x + 10f, 16f), title, style);
  }

  private enum StatisticValueType
  {
    Counter,
    Percent,
  }
}
