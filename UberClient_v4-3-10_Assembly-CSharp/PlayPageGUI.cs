// Decompiled with JetBrains decompiler
// Type: PlayPageGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PlayPageGUI : MonoBehaviour
{
  private const float _doubleClickFrame = 0.5f;
  private const int _dropDownListMap = 1;
  private const int _dropDownListGameMode = 2;
  private const int _dropDownListSingleWeapon = 4;
  private const int _privateGameWidth = 25;
  private const int _moderatorInGameWidth = 25;
  private const int _gameTimeWidth = 80;
  private const int _playerCountWidth = 80;
  private const float _serverSpeedColumnWidth = 110f;
  private const float _serverCapacityColumnWidth = 130f;
  private const int LobbyConnectionMaxTime = 30;
  private const int ServerCheckDelay = 5;
  private string[] _mapsFilter;
  private string[] _modesFilter;
  [SerializeField]
  private Texture2D _level1GameIcon;
  [SerializeField]
  private Texture2D _level2GameIcon;
  [SerializeField]
  private Texture2D _level3GameIcon;
  [SerializeField]
  private Texture2D _level5GameIcon;
  [SerializeField]
  private Texture2D _level10GameIcon;
  [SerializeField]
  private Texture2D _level20GameIcon;
  [SerializeField]
  private Texture2D _privateGameIcon;
  [SerializeField]
  private Texture2D _sortUpArrow;
  [SerializeField]
  private Texture2D _sortDownArrow;
  private float _gameJoinDoubleClick;
  private float _serverJoinDoubleClick;
  private bool _isConnectedToGameServer;
  private bool _refreshGameListOnFilterChange;
  private bool _refreshGameListOnSortChange;
  private Vector2 _serverScroll;
  private Vector2 _filterScroll;
  private bool _gameNotFull;
  private bool _noPrivateGames;
  private bool _instasplat;
  private bool _lowGravity;
  private bool _justForFun;
  private bool _singleWeapon;
  private bool _noFriendFire;
  private bool _showFilters;
  private string[] _weaponClassTexts;
  private int _dropDownList;
  private Rect _dropDownRect;
  private int _currentMap;
  private int _currentMode;
  private int _currentWeapon;
  private GameMetaData _selectedGame;
  private int _mapNameWidth = 135;
  private int _gameModeWidth = 80;
  private int _gameNameWidth = 200;
  private float _serverNameColumnWidth;
  private string _inputedPassword = string.Empty;
  private bool _unFocus;
  private bool _checkForPassword;
  private bool _isPasswordOk = true;
  private float _timeDelayOnCheckPassword = 2f;
  private float _nextPasswordCheck;
  private bool _sortServerAscending;
  private bool _sortGamesAscending;
  private int _filteredActiveRoomCount;
  private PlayPageGUI.GameListColumns _lastSortedColumn;
  private PlayPageGUI.FilterSavedData _filterSavedData;
  private List<GameMetaData> _cachedGameList;
  private IComparer<GameMetaData> _gameSortingMethod;
  private Vector2 _serverSelectionHelpScrollBar;
  private Vector2 _serverSelectionScrollBar;
  private PlayPageGUI.ServerListColumns _lastSortedServerColumn;
  private float _nextServerCheckTime;
  private float _timeToLobbyDisconnect;
  private SearchBarGUI _searchBar;

  public static PlayPageGUI Instance { get; private set; }

  public static bool Exists => (UnityEngine.Object) PlayPageGUI.Instance != (UnityEngine.Object) null;

  private void Awake()
  {
    PlayPageGUI.Instance = this;
    this._filterSavedData = new PlayPageGUI.FilterSavedData();
    this._cachedGameList = new List<GameMetaData>();
    this._sortGamesAscending = false;
    this._gameSortingMethod = (IComparer<GameMetaData>) new GameDataPlayerComparer();
    this._lastSortedColumn = PlayPageGUI.GameListColumns.PlayerCount;
    this._searchBar = new SearchBarGUI("SearchGame");
    if ((UnityEngine.Object) this._privateGameIcon == (UnityEngine.Object) null)
      throw new Exception("_privateGameIcon not assigned");
  }

  private void Start()
  {
    this._weaponClassTexts = new string[5]
    {
      LocalizedStrings.Handguns,
      LocalizedStrings.Machineguns,
      LocalizedStrings.SniperRifles,
      LocalizedStrings.Shotguns,
      LocalizedStrings.Launchers
    };
    this._modesFilter = new string[4]
    {
      LocalizedStrings.All + " Modes",
      LocalizedStrings.DeathMatch,
      LocalizedStrings.TeamDeathMatch,
      LocalizedStrings.TeamElimination
    };
    List<string> stringList = new List<string>();
    stringList.Add(LocalizedStrings.All + " Maps");
    foreach (UberstrikeMap allMap in Singleton<MapManager>.Instance.AllMaps)
    {
      if (allMap.Id != 0)
        stringList.Add(allMap.Name);
    }
    stringList.RemoveAll((Predicate<string>) (s => string.IsNullOrEmpty(s)));
    this._mapsFilter = stringList.ToArray();
  }

  private void OnEnable()
  {
    this._showFilters = false;
    this.ResetFilters();
    this._unFocus = true;
  }

  private void OnDisable()
  {
  }

  private void OnGUI()
  {
    GUI.depth = 11;
    GUI.skin = BlueStonez.Skin;
    if (this._unFocus)
    {
      if (GUIUtility.keyboardControl != 0)
        GUIUtility.keyboardControl = 0;
      this._unFocus = false;
    }
    Rect rect = new Rect(0.0f, (float) GlobalUIRibbon.Instance.Height(), (float) Screen.width, (float) (Screen.height - GlobalUIRibbon.Instance.Height()));
    GUI.Box(rect, string.Empty, BlueStonez.box_grey31);
    if (!this._isConnectedToGameServer || Singleton<GameServerController>.Instance.SelectedServer == null)
      this.DoServerPage(rect);
    else if (this._isConnectedToGameServer && Singleton<GameServerController>.Instance.SelectedServer != null)
      this.DoGamePage(rect);
    if (this._checkForPassword)
      this.PasswordCheck(new Rect((float) ((Screen.width - 280) / 2), (float) ((Screen.height - 200) / 2), 280f, 200f));
    GuiManager.DrawTooltip();
  }

  private void ResetFilters()
  {
    this._currentMap = 0;
    this._currentMode = 0;
    this._currentWeapon = 0;
    this._noFriendFire = false;
    this._gameNotFull = false;
    this._noPrivateGames = false;
    this._instasplat = false;
    this._lowGravity = false;
    this._justForFun = false;
    this._singleWeapon = false;
    this._searchBar.ClearFilter();
  }

  private void DoServerPage(Rect rect)
  {
    float helpPartWidth = 200f;
    GUI.BeginGroup(rect);
    GUI.Label(new Rect(0.0f, 0.0f, rect.width, 56f), LocalizedStrings.ChooseYourRegionCaps, BlueStonez.tab_strip);
    GUI.Box(new Rect(0.0f, 55f, rect.width, rect.height - 57f), string.Empty, BlueStonez.window_standard_grey38);
    GUI.color = new Color(1f, 1f, 1f, 0.5f);
    GUI.Label(new Rect(0.0f, 28f, rect.width - 5f, 28f), string.Format("{0} {1}, {2} {3} ", (object) Singleton<GameServerManager>.Instance.AllPlayersCount, (object) LocalizedStrings.PlayersOnline, (object) Singleton<GameServerManager>.Instance.AllGamesCount, (object) LocalizedStrings.Games), BlueStonez.label_interparkbold_18pt_right);
    GUI.color = Color.white;
    this.DoServerList(new Rect(10f, 55f, (float) ((double) rect.width - (double) helpPartWidth - 10.0), rect.height - 49f));
    this.DoServerHelpText(new Rect(rect.width - helpPartWidth, 55f, helpPartWidth - 10f, rect.height - 49f));
    this.DrawServerListButtons(rect, helpPartWidth);
    GUI.EndGroup();
  }

  private void DoServerHelpText(Rect position)
  {
    GUI.BeginGroup(position);
    GUI.Box(new Rect(0.0f, 0.0f, position.width, 32f), LocalizedStrings.HelpCaps, BlueStonez.box_grey50);
    GUI.Box(new Rect(0.0f, 31f, position.width, (float) ((double) position.height - 31.0 - 55.0)), string.Empty, BlueStonez.box_grey50);
    this._serverSelectionHelpScrollBar = GUITools.BeginScrollView(new Rect(0.0f, 33f, position.width, (float) ((double) position.height - 31.0 - 60.0)), this._serverSelectionHelpScrollBar, new Rect(0.0f, 0.0f, position.width - 20f, 400f));
    this.DrawGroupLabel(new Rect(5f, 5f, position.width - 25f, 100f), "1. " + LocalizedStrings.ServerName, LocalizedStrings.ServerNameDesc);
    this.DrawGroupLabel(new Rect(5f, 105f, position.width - 25f, 70f), "2. " + LocalizedStrings.Capacity, LocalizedStrings.CapacityDesc);
    this.DrawGroupLabel(new Rect(5f, 180f, position.width - 25f, 180f), "3. " + LocalizedStrings.Speed, LocalizedStrings.SpeedDesc);
    GUITools.EndScrollView();
    GUI.EndGroup();
  }

  private void DoServerList(Rect position)
  {
    this._serverNameColumnWidth = (float) ((double) position.width - 130.0 - 110.0 + 1.0);
    GUI.BeginGroup(position);
    GUI.Box(new Rect(0.0f, 0.0f, this._serverNameColumnWidth + 1f, 32f), string.Empty, BlueStonez.box_grey50);
    GUI.Box(new Rect(this._serverNameColumnWidth, 0.0f, 131f, 32f), string.Empty, BlueStonez.box_grey50);
    GUI.Box(new Rect(this._serverNameColumnWidth + 130f, 0.0f, 110f, 32f), string.Empty, BlueStonez.box_grey50);
    GUI.Box(new Rect(0.0f, 31f, position.width + 1f, (float) ((double) position.height - 31.0 - 55.0)), string.Empty, BlueStonez.box_grey50);
    if (this._lastSortedServerColumn == PlayPageGUI.ServerListColumns.ServerName)
    {
      if (this._sortServerAscending)
        GUI.Label(new Rect(5f, 0.0f, (float) ((double) this._serverNameColumnWidth + 1.0 - 5.0), 32f), new GUIContent(LocalizedStrings.ServerName, (Texture) this._sortUpArrow), BlueStonez.label_interparkbold_18pt_left);
      else
        GUI.Label(new Rect(5f, 0.0f, (float) ((double) this._serverNameColumnWidth + 1.0 - 5.0), 32f), new GUIContent(LocalizedStrings.ServerName, (Texture) this._sortDownArrow), BlueStonez.label_interparkbold_18pt_left);
    }
    else
      GUI.Label(new Rect(12f, 0.0f, (float) ((double) this._serverNameColumnWidth + 1.0 - 5.0), 32f), LocalizedStrings.ServerName, BlueStonez.label_interparkbold_18pt_left);
    if (GUI.Button(new Rect(0.0f, 0.0f, (float) ((double) this._serverNameColumnWidth + 1.0 - 5.0), 32f), GUIContent.none, BlueStonez.label_interparkbold_11pt_left))
      this.SortServerList(PlayPageGUI.ServerListColumns.ServerName);
    if (this._lastSortedServerColumn == PlayPageGUI.ServerListColumns.ServerCapacity)
    {
      if (this._sortServerAscending)
        GUI.Label(new Rect(5f + this._serverNameColumnWidth, 0.0f, 126f, 32f), new GUIContent(LocalizedStrings.Capacity, (Texture) this._sortUpArrow), BlueStonez.label_interparkbold_18pt_left);
      else
        GUI.Label(new Rect(5f + this._serverNameColumnWidth, 0.0f, 126f, 32f), new GUIContent(LocalizedStrings.Capacity, (Texture) this._sortDownArrow), BlueStonez.label_interparkbold_18pt_left);
    }
    else
      GUI.Label(new Rect(this._serverNameColumnWidth + 12f, 0.0f, 126f, 32f), LocalizedStrings.Capacity, BlueStonez.label_interparkbold_18pt_left);
    if (GUI.Button(new Rect(this._serverNameColumnWidth, 0.0f, 126f, 32f), GUIContent.none, BlueStonez.label_interparkbold_11pt_left))
      this.SortServerList(PlayPageGUI.ServerListColumns.ServerCapacity);
    if (this._lastSortedServerColumn == PlayPageGUI.ServerListColumns.ServerSpeed)
    {
      if (this._sortServerAscending)
        GUI.Label(new Rect((float) (5.0 + (double) this._serverNameColumnWidth + 130.0), 0.0f, 105f, 32f), new GUIContent(LocalizedStrings.Speed, (Texture) this._sortUpArrow), BlueStonez.label_interparkbold_18pt_left);
      else
        GUI.Label(new Rect((float) (5.0 + (double) this._serverNameColumnWidth + 130.0), 0.0f, 105f, 32f), new GUIContent(LocalizedStrings.Speed, (Texture) this._sortDownArrow), BlueStonez.label_interparkbold_18pt_left);
    }
    else
      GUI.Label(new Rect((float) ((double) this._serverNameColumnWidth + 130.0 + 12.0), 0.0f, 105f, 32f), LocalizedStrings.Speed, BlueStonez.label_interparkbold_18pt_left);
    if (GUI.Button(new Rect(this._serverNameColumnWidth + 130f, 0.0f, 105f, 32f), GUIContent.none, BlueStonez.label_interparkbold_11pt_left))
      this.SortServerList(PlayPageGUI.ServerListColumns.ServerSpeed);
    this.DrawAllServers(position);
    GUI.EndGroup();
  }

  private void DrawProgressBarLarge(Rect position, float amount)
  {
    amount = Mathf.Clamp01(amount);
    GUI.Box(new Rect(position.x, position.y, position.width, 23f), GUIContent.none, BlueStonez.progressbar_large_background);
    GUI.color = ColorScheme.ProgressBar;
    GUI.Box(new Rect(position.x + 2f, position.y + 2f, (float) Mathf.RoundToInt((position.width - 4f) * amount), 19f), GUIContent.none, BlueStonez.progressbar_large_thumb);
    GUI.color = Color.white;
  }

  private void DrawAllServers(Rect pos)
  {
    int height = Singleton<GameServerManager>.Instance.PhotonServerCount * 48;
    GUI.color = Color.white;
    this._serverSelectionScrollBar = GUITools.BeginScrollView(new Rect(0.0f, 31f, pos.width + 1f, (float) ((double) pos.height - 31.0 - 55.0)), this._serverSelectionScrollBar, new Rect(0.0f, 0.0f, pos.width - 20f, (float) height));
    int num1 = 0;
    string empty = string.Empty;
    foreach (GameServerView photonServer in Singleton<GameServerManager>.Instance.PhotonServerList)
    {
      GUI.BeginGroup(new Rect(0.0f, (float) (num1 * 48), pos.width + 2f, 49f), BlueStonez.box_grey50);
      if (Singleton<GameServerController>.Instance.SelectedServer != null && Singleton<GameServerController>.Instance.SelectedServer == photonServer)
      {
        GUI.color = new Color(1f, 1f, 1f, 0.03f);
        GUI.DrawTexture(new Rect(1f, 0.0f, pos.width + 1f, 49f), (Texture) UberstrikeIconsHelper.White);
        GUI.color = Color.white;
      }
      photonServer.Flag.Draw(new Rect(5f, 8f, 32f, 32f));
      GUI.Label(new Rect(42f, 1f, (float) ((double) this._serverNameColumnWidth + 1.0 - 42.0), 48f), photonServer.Name, BlueStonez.label_interparkbold_16pt_left);
      if (photonServer.Data.State == ServerLoadData.Status.Alive)
      {
        GUI.BeginGroup(new Rect(5f + this._serverNameColumnWidth, 0.0f, 126f, 48f));
        int num2 = PlayerDataManager.AccessLevel != MemberAccessLevel.Admin ? Mathf.Clamp(photonServer.Data.PlayersConnected, 0, (int) photonServer.Data.MaxPlayerCount) : photonServer.Data.PlayersConnected;
        this.DrawProgressBarLarge(new Rect(2f, 12f, 58f, 20f), (double) num2 < (double) photonServer.Data.MaxPlayerCount ? (float) num2 / photonServer.Data.MaxPlayerCount : 1f);
        GUI.Label(new Rect(64f, 14f, 60f, 20f), string.Format("{0}/{1}", (object) num2, (object) photonServer.Data.MaxPlayerCount), BlueStonez.label_interparkmed_10pt_left);
        GUI.EndGroup();
        GUI.BeginGroup(new Rect((float) (5.0 + (double) this._serverNameColumnWidth + 130.0), 0.0f, (float) (105.0 - ((double) height <= (double) pos.height - 31.0 ? 0.0 : 21.0)), 48f));
        int latency = photonServer.Latency;
        empty = string.Empty;
        string text;
        if (latency < 100)
        {
          GUI.color = ColorConverter.RgbToColor(80f, 99f, 42f);
          text = LocalizedStrings.FastCaps;
        }
        else if (latency < 300)
        {
          GUI.color = ColorConverter.RgbToColor(234f, 112f, 13f);
          text = LocalizedStrings.MedCaps;
        }
        else
        {
          GUI.color = ColorConverter.RgbToColor(192f, 80f, 70f);
          text = LocalizedStrings.SlowCaps;
        }
        GUI.DrawTexture(new Rect(0.0f, 14f, 45f, 20f), (Texture) UberstrikeIconsHelper.White);
        GUI.color = Color.white;
        GUI.Label(new Rect(2f, 14f, 40f, 20f), text, BlueStonez.label_interparkbold_16pt);
        GUI.Label(new Rect(48f, 4f, 40f, 40f), string.Format("{0}ms", (object) latency), BlueStonez.label_interparkmed_10pt_left);
        GUI.EndGroup();
      }
      else if (photonServer.Data.State == ServerLoadData.Status.None)
      {
        Rect position = new Rect(5f + this._serverNameColumnWidth, 0.0f, (float) (236.0 - ((double) height <= (double) pos.height - 31.0 ? 0.0 : 21.0)), 48f);
        GUI.BeginGroup(position);
        GUI.Label(new Rect(0.0f, 0.0f, position.width, 48f), LocalizedStrings.RefreshingServer, BlueStonez.label_interparkbold_16pt);
        GUI.EndGroup();
      }
      else if (photonServer.Data.State == ServerLoadData.Status.NotReachable)
      {
        Rect position = new Rect(5f + this._serverNameColumnWidth, 0.0f, (float) (236.0 - ((double) height <= (double) pos.height - 31.0 ? 0.0 : 21.0)), 48f);
        GUI.BeginGroup(position);
        GUI.Label(new Rect(0.0f, 0.0f, position.width, 48f), LocalizedStrings.ServerIsNotReachable, BlueStonez.label_interparkbold_16pt);
        GUI.EndGroup();
      }
      if (GUI.Button(new Rect(0.0f, 0.0f, pos.width + 1f, 49f), GUIContent.none, GUIStyle.none) && photonServer.Data.State != ServerLoadData.Status.NotReachable)
      {
        if (Singleton<GameServerController>.Instance.SelectedServer == photonServer && (double) this._serverJoinDoubleClick > (double) Time.time)
        {
          this._serverJoinDoubleClick = 0.0f;
          this.SelectedServerUpdated();
          SfxManager.Play2dAudioClip(GameAudio.JoinServer);
        }
        else
          this._serverJoinDoubleClick = Time.time + 0.5f;
        Singleton<GameServerController>.Instance.SelectedServer = photonServer;
      }
      GUI.EndGroup();
      ++num1;
    }
    GUITools.EndScrollView();
  }

  private void DrawServerListButtons(Rect rect, float helpPartWidth)
  {
    bool enabled = GUI.enabled;
    GUI.enabled = enabled && Singleton<GameServerController>.Instance.SelectedServer != null;
    if (GUITools.Button(new Rect(rect.width - 140f, rect.height - 42f, 120f, 32f), new GUIContent(LocalizedStrings.JoinCaps), BlueStonez.button_green, GameAudio.JoinServer))
      this.SelectedServerUpdated();
    GUI.enabled = enabled && (double) Time.time > (double) this._nextServerCheckTime;
    if (GUITools.Button(new Rect(rect.width - 263f, rect.height - 42f, 120f, 32f), new GUIContent(LocalizedStrings.RefreshCaps), BlueStonez.button))
      this.RefreshServerLoad();
    GUI.enabled = enabled;
  }

  public void SelectedServerUpdated()
  {
    if (Singleton<GameServerController>.Instance.SelectedServer != null && Singleton<GameServerController>.Instance.SelectedServer.Data.State == ServerLoadData.Status.Alive)
    {
      if (PlayerDataManager.AccessLevelSecure >= MemberAccessLevel.SeniorModerator)
        this.ShowGameSelection();
      else if ((double) Singleton<GameServerController>.Instance.SelectedServer.Data.PlayersConnected >= (double) Singleton<GameServerController>.Instance.SelectedServer.Data.MaxPlayerCount)
        PopupSystem.ShowMessage(LocalizedStrings.ServerFull, LocalizedStrings.ServerFullMsg);
      else if (!Singleton<GameServerController>.Instance.SelectedServer.CheckLatency())
        PopupSystem.ShowMessage(LocalizedStrings.Warning, "Your connection to this server is too slow.", PopupSystem.AlertType.OK, (Action) null);
      else if (Singleton<GameServerController>.Instance.SelectedServer.Latency >= 300)
        PopupSystem.ShowMessage(LocalizedStrings.Warning, LocalizedStrings.ConnectionSlowMsg, PopupSystem.AlertType.OKCancel, new Action(this.ShowGameSelection), LocalizedStrings.OkCaps, (Action) null, LocalizedStrings.CancelCaps);
      else
        this.ShowGameSelection();
    }
    else
      UnityEngine.Debug.LogError((object) "Couldn't connect to server!");
  }

  private void DrawGroupLabel(Rect position, string header, string text)
  {
    GUI.color = Color.white;
    GUI.Label(new Rect(position.x, position.y, position.width, 16f), header, BlueStonez.label_interparkbold_13pt);
    GUI.color = new Color(1f, 1f, 1f, 0.8f);
    GUI.Label(new Rect(position.x, position.y + 16f, position.width, position.height - 16f), text, BlueStonez.label_interparkbold_11pt_left_wrap);
    GUI.color = Color.white;
  }

  private void DoGamePage(Rect rect)
  {
    GUI.BeginGroup(rect);
    GUI.Label(new Rect(0.0f, 0.0f, rect.width, 56f), LocalizedStrings.ChooseAGameCaps, BlueStonez.tab_strip);
    GUI.color = new Color(1f, 1f, 1f, 0.5f);
    GUI.Label(new Rect(10f, 28f, rect.width - 37f, 28f), string.Format("{0} ({1}ms)", (object) Singleton<GameServerController>.Instance.SelectedServer.Name, (object) Singleton<GameServerController>.Instance.SelectedServer.Latency.ToString()), BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(0.0f, 28f, rect.width - 5f, 28f), string.Format("{0} {1}, {2} {3} ", (object) Singleton<GameListManager>.Instance.PlayersCount, (object) LocalizedStrings.PlayersOnline, (object) Singleton<GameListManager>.Instance.GamesCount, (object) LocalizedStrings.Games), BlueStonez.label_interparkbold_18pt_right);
    GUI.color = Color.white;
    GUI.Box(new Rect(0.0f, 55f, rect.width, rect.height - 57f), string.Empty, BlueStonez.window_standard_grey38);
    this.DrawQuickSearch(rect);
    if (GUITools.Button(new Rect(rect.width - 300f, 6f, 140f, 23f), new GUIContent(LocalizedStrings.Refresh), BlueStonez.buttondark_medium))
    {
      if (!LobbyConnectionManager.IsConnected)
      {
        this.ResetLobbyDisconnectTimeout();
        LobbyConnectionManager.StartConnection();
      }
      this.RefreshGameList();
    }
    bool enabled = GUI.enabled;
    GUI.enabled = ((GUI.enabled ? 1 : 0) & (this._dropDownList != 0 ? 0 : (!this._checkForPassword ? 1 : 0))) != 0;
    if (this._showFilters)
      this.DoGameList(new Rect(25f, 73f, (float) Screen.width, (float) (Screen.height - 105)));
    else
      this.DoGameList(rect);
    this.DoBottomArea(rect);
    GUI.enabled = enabled;
    if (this._showFilters)
      this.DoFilterArea(rect);
    GUI.EndGroup();
  }

  private void DoGameList(Rect rect)
  {
    this.UpdateColumnWidth();
    int height1 = Mathf.RoundToInt(rect.height) - 73 - 104;
    if (!this._showFilters)
      height1 += 73;
    Rect rect1 = new Rect(10f, 55f, rect.width - 20f, (float) height1);
    GUI.Box(rect1, string.Empty, BlueStonez.box_grey50);
    GUI.BeginGroup(rect1);
    if (!LobbyConnectionManager.IsConnected)
    {
      GUI.Label(new Rect(0.0f, rect1.height * 0.5f, rect1.width, 23f), LocalizedStrings.PressRefreshToSeeCurrentGames, BlueStonez.label_interparkmed_11pt);
      if (GUITools.Button(new Rect((float) ((double) rect1.width * 0.5 - 70.0), (float) ((double) rect1.height * 0.5 - 30.0), 140f, 23f), new GUIContent(LocalizedStrings.Refresh), BlueStonez.buttondark_medium))
      {
        if (!LobbyConnectionManager.IsConnected)
        {
          this.ResetLobbyDisconnectTimeout();
          LobbyConnectionManager.StartConnection();
        }
        this.RefreshGameList();
      }
    }
    else if (LobbyConnectionManager.IsConnecting)
      GUI.Label(new Rect(0.0f, 0.0f, rect1.width, rect1.height - 1f), LocalizedStrings.ConnectingToLobby, BlueStonez.label_interparkmed_11pt);
    else if (LobbyConnectionManager.IsInLobby && this._cachedGameList.Count == 0)
      GUI.Label(new Rect(0.0f, 0.0f, rect1.width, rect1.height - 1f), LocalizedStrings.LoadingGameList, BlueStonez.label_interparkmed_11pt);
    int height2 = Singleton<GameServerController>.Instance.SelectedServer == null ? 0 : 48 * (this._filteredActiveRoomCount < 0 || this._filteredActiveRoomCount > this._cachedGameList.Count ? this._cachedGameList.Count : this._filteredActiveRoomCount) + 5;
    int num1 = 0;
    int left1 = 5;
    Texture2D image1 = this._lastSortedColumn != PlayPageGUI.GameListColumns.Lock ? (Texture2D) null : (!this._sortGamesAscending ? this._sortDownArrow : this._sortUpArrow);
    num1 = this._lastSortedColumn != PlayPageGUI.GameListColumns.Lock ? 12 : 5;
    GUI.Box(new Rect((float) left1, 0.0f, 25f, 25f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect((float) left1, 0.0f, 25f, 25f), new GUIContent(string.Empty, (Texture) image1), BlueStonez.label_interparkbold_16pt_left);
    if (GUI.Button(new Rect((float) left1, 0.0f, 25f, 25f), GUIContent.none, BlueStonez.label_interparkbold_11pt_left))
      this.SortGameList(PlayPageGUI.GameListColumns.Lock);
    int left2 = 24;
    Texture2D image2 = this._lastSortedColumn != PlayPageGUI.GameListColumns.Star ? (Texture2D) null : (!this._sortGamesAscending ? this._sortDownArrow : this._sortUpArrow);
    int num2 = this._lastSortedColumn != PlayPageGUI.GameListColumns.Star ? 12 : 5;
    GUI.Box(new Rect((float) left2, 0.0f, 25f, 25f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect((float) (left2 + num2), 0.0f, 25f, 25f), new GUIContent(string.Empty, (Texture) image2), BlueStonez.label_interparkbold_16pt_left);
    if (GUI.Button(new Rect((float) left2, 0.0f, 25f, 25f), GUIContent.none, BlueStonez.label_interparkbold_11pt_left))
      this.SortGameList(PlayPageGUI.GameListColumns.Star);
    int left3 = 48;
    Texture2D image3 = this._lastSortedColumn != PlayPageGUI.GameListColumns.GameName ? (Texture2D) null : (!this._sortGamesAscending ? this._sortDownArrow : this._sortUpArrow);
    int num3 = this._lastSortedColumn != PlayPageGUI.GameListColumns.GameName ? 12 : 5;
    GUI.Box(new Rect((float) left3, 0.0f, (float) this._gameNameWidth, 25f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect((float) (left3 + num3), 0.0f, (float) this._gameNameWidth, 25f), new GUIContent(LocalizedStrings.Name, (Texture) image3), BlueStonez.label_interparkbold_16pt_left);
    if (GUI.Button(new Rect((float) left3, 0.0f, (float) this._gameNameWidth, 25f), GUIContent.none, BlueStonez.label_interparkbold_11pt_left))
      this.SortGameList(PlayPageGUI.GameListColumns.GameName);
    int left4 = 50 + this._gameNameWidth - 3;
    Texture2D image4 = this._lastSortedColumn != PlayPageGUI.GameListColumns.GameMap ? (Texture2D) null : (!this._sortGamesAscending ? this._sortDownArrow : this._sortUpArrow);
    int num4 = this._lastSortedColumn != PlayPageGUI.GameListColumns.GameMap ? 12 : 5;
    GUI.Box(new Rect((float) left4, 0.0f, (float) this._mapNameWidth, 25f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect((float) (left4 + num4), 0.0f, (float) this._mapNameWidth, 25f), new GUIContent(LocalizedStrings.Map, (Texture) image4), BlueStonez.label_interparkbold_16pt_left);
    if (GUI.Button(new Rect((float) left4, 0.0f, (float) this._mapNameWidth, 25f), string.Empty, BlueStonez.label_interparkbold_11pt_left))
      this.SortGameList(PlayPageGUI.GameListColumns.GameMap);
    int left5 = 50 + this._gameNameWidth + this._mapNameWidth - 4;
    Texture2D image5 = this._lastSortedColumn != PlayPageGUI.GameListColumns.GameMode ? (Texture2D) null : (!this._sortGamesAscending ? this._sortDownArrow : this._sortUpArrow);
    int num5 = this._lastSortedColumn != PlayPageGUI.GameListColumns.GameMode ? 12 : 5;
    GUI.Box(new Rect((float) left5, 0.0f, (float) this._gameModeWidth, 25f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect((float) (left5 + num5), 0.0f, (float) this._gameModeWidth, 25f), new GUIContent(LocalizedStrings.Mode, (Texture) image5), BlueStonez.label_interparkbold_16pt_left);
    if (GUI.Button(new Rect((float) left5, 0.0f, (float) this._gameModeWidth, 25f), string.Empty, BlueStonez.label_interparkbold_11pt_left))
      this.SortGameList(PlayPageGUI.GameListColumns.GameMode);
    int left6 = 50 + this._gameNameWidth + this._mapNameWidth + this._gameModeWidth - 10;
    Texture2D image6 = this._lastSortedColumn != PlayPageGUI.GameListColumns.GameTime ? (Texture2D) null : (!this._sortGamesAscending ? this._sortDownArrow : this._sortUpArrow);
    int num6 = this._lastSortedColumn != PlayPageGUI.GameListColumns.GameTime ? 12 : 5;
    GUI.Box(new Rect((float) left6, 0.0f, 85f, 25f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect((float) (left6 + num6), 0.0f, 80f, 25f), new GUIContent(LocalizedStrings.Minutes, (Texture) image6), BlueStonez.label_interparkbold_16pt_left);
    if (GUI.Button(new Rect((float) left6, 0.0f, 80f, 25f), string.Empty, BlueStonez.label_interparkbold_11pt_left))
      this.SortGameList(PlayPageGUI.GameListColumns.GameTime);
    int left7 = 50 + this._gameNameWidth + this._mapNameWidth + this._gameModeWidth + 80 - 6;
    Texture2D image7 = this._lastSortedColumn != PlayPageGUI.GameListColumns.PlayerCount ? (Texture2D) null : (!this._sortGamesAscending ? this._sortDownArrow : this._sortUpArrow);
    int num7 = this._lastSortedColumn != PlayPageGUI.GameListColumns.PlayerCount ? 12 : 5;
    GUI.Box(new Rect((float) left7, 0.0f, 80f, 25f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect((float) (left7 + num7), 0.0f, 80f, 25f), new GUIContent(LocalizedStrings.Players, (Texture) image7), BlueStonez.label_interparkbold_16pt_left);
    if (GUI.Button(new Rect((float) left7, 0.0f, 80f, 25f), string.Empty, BlueStonez.label_interparkbold_11pt_left))
      this.SortGameList(PlayPageGUI.GameListColumns.PlayerCount);
    if (LobbyConnectionManager.IsConnected)
    {
      Vector2 serverScroll = this._serverScroll;
      this._serverScroll = GUITools.BeginScrollView(new Rect(0.0f, 25f, rect1.width, (float) ((double) rect1.height - 1.0 - 25.0)), this._serverScroll, new Rect(0.0f, 0.0f, rect1.width - 60f, (float) height2), BlueStonez.horizontalScrollbar, BlueStonez.verticalScrollbar);
      this._filteredActiveRoomCount = this.DrawAllGames(rect1, (double) rect1.height <= (double) height2);
      GUITools.EndScrollView();
      if (serverScroll != this._serverScroll)
        this.ResetLobbyDisconnectTimeout();
    }
    GUI.EndGroup();
  }

  private void SortServerList(PlayPageGUI.ServerListColumns sortedColumn) => this.SortServerList(sortedColumn, true);

  private void SortServerList(PlayPageGUI.ServerListColumns sortedColumn, bool changeDirection)
  {
    if (changeDirection && sortedColumn == this._lastSortedServerColumn)
      this._sortServerAscending = !this._sortServerAscending;
    this._lastSortedServerColumn = sortedColumn;
    switch (sortedColumn)
    {
      case PlayPageGUI.ServerListColumns.ServerName:
        Singleton<GameServerManager>.Instance.SortServers((IComparer<GameServerView>) new GameServerNameComparer(), this._sortServerAscending);
        break;
      case PlayPageGUI.ServerListColumns.ServerCapacity:
        Singleton<GameServerManager>.Instance.SortServers((IComparer<GameServerView>) new GameServerPlayerCountComparer(), this._sortServerAscending);
        break;
      case PlayPageGUI.ServerListColumns.ServerSpeed:
        Singleton<GameServerManager>.Instance.SortServers((IComparer<GameServerView>) new GameServerLatencyComparer(), this._sortServerAscending);
        break;
      default:
        Singleton<GameServerManager>.Instance.SortServers((IComparer<GameServerView>) new GameServerLatencyComparer(), this._sortServerAscending);
        break;
    }
  }

  private void SortGameList(PlayPageGUI.GameListColumns sortedColumn)
  {
    if (sortedColumn == this._lastSortedColumn)
      this._sortGamesAscending = !this._sortGamesAscending;
    this._lastSortedColumn = sortedColumn;
    switch (sortedColumn)
    {
      case PlayPageGUI.GameListColumns.Lock:
        this.SortGameList((IComparer<GameMetaData>) new GameDataAccessComparer());
        break;
      case PlayPageGUI.GameListColumns.GameName:
        this.SortGameList((IComparer<GameMetaData>) new GameDataNameComparer());
        break;
      case PlayPageGUI.GameListColumns.GameMap:
        this.SortGameList((IComparer<GameMetaData>) new GameDataMapComparer());
        break;
      case PlayPageGUI.GameListColumns.GameMode:
        this.SortGameList((IComparer<GameMetaData>) new GameDataRuleComparer());
        break;
      case PlayPageGUI.GameListColumns.PlayerCount:
        this.SortGameList((IComparer<GameMetaData>) new GameDataPlayerComparer());
        break;
      case PlayPageGUI.GameListColumns.GameTime:
        this.SortGameList((IComparer<GameMetaData>) new GameDataTimeComparer());
        break;
      default:
        this.SortGameList((IComparer<GameMetaData>) new GameDataLatencyComparer());
        break;
    }
  }

  private void SortGameList(IComparer<GameMetaData> sortedColumn)
  {
    this._gameSortingMethod = sortedColumn;
    this.RefreshGameList();
  }

  private void DoFilterArea(Rect rect)
  {
    bool enabled = GUI.enabled;
    Rect position = new Rect(10f, (float) ((double) rect.height - 73.0 - 50.0), rect.width - 20f, 74f);
    GUI.Box(position, string.Empty, BlueStonez.box_grey50);
    GUI.BeginGroup(new Rect(position.x, position.y, position.width, position.width + 60f));
    GUI.enabled = enabled && (this._dropDownList == 0 || this._dropDownList == 1);
    GUI.Label(new Rect(10f, 10f, 115f, 21f), this._mapsFilter[this._currentMap], BlueStonez.label_dropdown);
    if (GUI.Button(new Rect(123f, 9f, 21f, 21f), GUIContent.none, BlueStonez.dropdown_button))
    {
      this._dropDownList = this._dropDownList != 0 ? 0 : 1;
      this._dropDownRect = new Rect(10f, 31f, 133f, 80f);
    }
    GUI.enabled = enabled && (this._dropDownList == 0 || this._dropDownList == 2);
    GUI.Label(new Rect(10f, 42f, 115f, 21f), this._modesFilter[this._currentMode], BlueStonez.label_dropdown);
    if (GUI.Button(new Rect(123f, 41f, 21f, 21f), GUIContent.none, BlueStonez.dropdown_button))
    {
      this._dropDownList = this._dropDownList != 0 ? 0 : 2;
      this._dropDownRect = new Rect(10f, 63f, 133f, 60f);
    }
    GUI.enabled = enabled && (this._dropDownList == 0 || this._dropDownList == 4) && this._singleWeapon;
    GUI.enabled = enabled && this._dropDownList == 0;
    this._gameNotFull = GUI.Toggle(new Rect(165f, 7f, 170f, 16f), this._gameNotFull, LocalizedStrings.GameNotFull, BlueStonez.toggle);
    this._noPrivateGames = GUI.Toggle(new Rect(165f, 28f, 170f, 16f), this._noPrivateGames, LocalizedStrings.NotPasswordProtected, BlueStonez.toggle);
    GUI.enabled = false;
    GUI.enabled = enabled;
    if (this._dropDownList != 0)
      this.DoDropDownList();
    GUI.EndGroup();
  }

  private bool CheckChangesInFilter()
  {
    bool flag = false;
    if (this._filterSavedData.UseFilter != this._showFilters)
    {
      this._filterSavedData.UseFilter = this._showFilters;
      flag = true;
    }
    if (this._filterSavedData.MapName != this._mapsFilter[this._currentMap])
    {
      this._filterSavedData.MapName = this._mapsFilter[this._currentMap];
      flag = true;
    }
    if (this._filterSavedData.GameMode != this._modesFilter[this._currentMode])
    {
      this._filterSavedData.GameMode = this._modesFilter[this._currentMode];
      flag = true;
    }
    if (this._filterSavedData.NoFriendlyFire != this._noFriendFire)
    {
      this._filterSavedData.NoFriendlyFire = this._noFriendFire;
      flag = true;
    }
    if (this._filterSavedData.ISGameNotFull != this._gameNotFull)
    {
      this._filterSavedData.ISGameNotFull = this._gameNotFull;
      flag = true;
    }
    if (this._filterSavedData.NoPasswordProtection != this._noPrivateGames)
    {
      this._filterSavedData.NoPasswordProtection = this._noPrivateGames;
      flag = true;
    }
    return flag;
  }

  private void DoBottomArea(Rect rect)
  {
    GUITools.PushGUIState();
    GUI.enabled = this._dropDownList == 0;
    if (GUITools.Button(new Rect(20f, rect.height - 42f, 120f, 32f), new GUIContent("< BACK", LocalizedStrings.ChangeServer), BlueStonez.button, GameAudio.LeaveServer))
      this.ShowServerSelection();
    bool showFilters = this._showFilters;
    this._showFilters = GUI.Toggle(new Rect(147f, rect.height - 42f, 120f, 32f), this._showFilters, LocalizedStrings.FiltersCaps, BlueStonez.button);
    if (showFilters != this._showFilters)
      this.ResetLobbyDisconnectTimeout();
    if (this._showFilters && this.AreFiltersActive && GUITools.Button(new Rect(273f, rect.height - 42f, 145f, 32f), new GUIContent(LocalizedStrings.ResetFiltersCaps), BlueStonez.button))
    {
      this.ResetLobbyDisconnectTimeout();
      this.ResetFilters();
    }
    if (!this._showFilters && this._filterSavedData.UseFilter)
      this._filterSavedData.UseFilter = false;
    if (!this._refreshGameListOnFilterChange && this.AreFiltersActive)
    {
      this.RefreshGameList();
      this._refreshGameListOnFilterChange = true;
    }
    if (this._refreshGameListOnFilterChange && !this.AreFiltersActive)
    {
      this.RefreshGameList();
      this._refreshGameListOnFilterChange = false;
    }
    GUI.enabled = true;
    if (GUITools.Button(new Rect(rect.width - 306f, rect.height - 42f, 140f, 32f), new GUIContent(LocalizedStrings.CreateGameCaps), BlueStonez.button))
    {
      this.ResetLobbyDisconnectTimeout();
      PanelManager.Instance.OpenPanel(PanelType.CreateGame);
    }
    GUI.enabled = LobbyConnectionManager.IsConnected && this._selectedGame != null && Singleton<GameServerController>.Instance.SelectedServer != null && Singleton<GameServerController>.Instance.SelectedServer.Data.RoomsCreated != 0 && !PanelManager.Instance.IsPanelOpen(PanelType.CreateGame);
    GUI.Label(new Rect(0.0f, (float) (Screen.height - 20), (float) Screen.width, 20f), "lobby: " + (object) LobbyConnectionManager.IsConnected + " sel game: " + (object) (this._selectedGame != null) + " sel srv: " + (object) (Singleton<GameServerController>.Instance.SelectedServer != null) + " room: " + (object) Singleton<GameServerController>.Instance.SelectedServer.Data.RoomsCreated);
    if (GUITools.Button(new Rect(rect.width - 160f, rect.height - 42f, 140f, 32f), new GUIContent(LocalizedStrings.JoinCaps), BlueStonez.button_green, GameAudio.JoinGame))
    {
      if (this._selectedGame != null && this._selectedGame.IsPublic || PlayerDataManager.AccessLevelSecure >= MemberAccessLevel.SeniorModerator)
      {
        this.JoinGame();
      }
      else
      {
        this._checkForPassword = true;
        this._nextPasswordCheck = Time.time;
      }
    }
    GUITools.PopGUIState();
  }

  private void DrawQuickSearch(Rect position)
  {
    this._searchBar.Draw(new Rect(position.width - 154f, 8f, 142f, 20f));
    if (!this._refreshGameListOnSortChange && this._searchBar.FilterText.Length > 0)
    {
      this.RefreshGameList();
      this._refreshGameListOnSortChange = true;
    }
    if (!this._refreshGameListOnSortChange || this._searchBar.FilterText.Length != 0)
      return;
    this.RefreshGameList();
    this._refreshGameListOnSortChange = false;
  }

  private void DoDropDownList()
  {
    string[] strArray;
    switch (this._dropDownList)
    {
      case 1:
        strArray = this._mapsFilter;
        break;
      case 2:
        strArray = this._modesFilter;
        break;
      case 4:
        strArray = this._weaponClassTexts;
        break;
      default:
        UnityEngine.Debug.LogError((object) ("Nondefined drop down list: " + (object) this._dropDownList));
        return;
    }
    GUI.Box(this._dropDownRect, string.Empty, BlueStonez.window);
    this._filterScroll = GUITools.BeginScrollView(this._dropDownRect, this._filterScroll, new Rect(0.0f, 0.0f, this._dropDownRect.width - 20f, (float) (20 * strArray.Length)));
    for (int index = 0; index < strArray.Length; ++index)
    {
      GUI.Label(new Rect(2f, (float) (20 * index), this._dropDownRect.width, 20f), strArray[index], BlueStonez.dropdown_list);
      if (GUI.Button(new Rect(2f, (float) (20 * index), this._dropDownRect.width, 20f), string.Empty, BlueStonez.dropdown_list))
      {
        switch (this._dropDownList)
        {
          case 1:
            this._currentMap = index;
            break;
          case 2:
            this._currentMode = index;
            break;
          case 4:
            this._currentWeapon = index;
            break;
        }
        this._dropDownList = 0;
        this._filterScroll.y = 0.0f;
      }
    }
    GUITools.EndScrollView();
  }

  private void JoinGame()
  {
    if (this._selectedGame.ConnectedPlayers < this._selectedGame.MaxPlayers && (this._selectedGame.ConnectedPlayers > 0 || this._selectedGame.HasLevelRestriction) || PlayerDataManager.AccessLevelSecure >= MemberAccessLevel.SeniorModerator)
    {
      if (ApplicationDataManager.IsMobile && this._selectedGame.MaxPlayers > 8)
        PopupSystem.ShowMessage(LocalizedStrings.Warning, LocalizedStrings.MobileGameMoreThan8Players, PopupSystem.AlertType.OKCancel, (Action) (() => Singleton<GameStateController>.Instance.JoinGame(this._selectedGame)), LocalizedStrings.OkCaps, (Action) null, LocalizedStrings.CancelCaps);
      else
        Singleton<GameStateController>.Instance.JoinGame(this._selectedGame);
    }
    else if (this._selectedGame.ConnectedPlayers == 0)
    {
      PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.ThisGameNoLongerExists);
    }
    else
    {
      if (this._selectedGame.ConnectedPlayers != this._selectedGame.MaxPlayers)
        return;
      PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.ThisGameIsFull);
    }
  }

  private void PasswordCheck(Rect position)
  {
    GUITools.PushGUIState();
    GUI.BeginGroup(position, GUIContent.none, BlueStonez.window);
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 56f), LocalizedStrings.EnterPassword, BlueStonez.tab_strip);
    GUI.Box(new Rect(16f, 55f, position.width - 32f, (float) ((double) position.height - 56.0 - 64.0)), GUIContent.none, BlueStonez.window_standard_grey38);
    GUI.SetNextControlName("EnterPassword");
    Rect position1 = new Rect((float) (((double) position.width - 188.0) / 2.0), 80f, 188f, 24f);
    this._inputedPassword = GUI.PasswordField(position1, this._inputedPassword, '*', 18, BlueStonez.textField);
    this._inputedPassword = this._inputedPassword.Trim('\n');
    if (string.IsNullOrEmpty(this._inputedPassword) && GUI.GetNameOfFocusedControl() != "EnterPassword")
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(position1, LocalizedStrings.TypePasswordHere, BlueStonez.label_interparkmed_11pt);
      GUI.color = Color.white;
    }
    GUI.enabled = (double) Time.time > (double) this._nextPasswordCheck;
    if (GUITools.Button(new Rect((float) ((double) position.width - 100.0 - 10.0), 152f, 100f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button) || Event.current.keyCode == KeyCode.Return && Event.current.type == UnityEngine.EventType.Layout && (double) Time.time > (double) this._nextPasswordCheck)
    {
      if (this._selectedGame != null && this._inputedPassword == this._selectedGame.Password)
      {
        this._checkForPassword = false;
        this._inputedPassword = string.Empty;
        this._isPasswordOk = true;
        this.JoinGame();
      }
      else
      {
        this._inputedPassword = string.Empty;
        this._isPasswordOk = false;
        this._nextPasswordCheck = Time.time + this._timeDelayOnCheckPassword;
      }
      Input.ResetInputAxes();
    }
    GUI.enabled = true;
    if (GUITools.Button(new Rect(10f, 152f, 100f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button))
    {
      this._isPasswordOk = true;
      this._checkForPassword = false;
      this._inputedPassword = string.Empty;
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

  private bool CanPassFilter(GameMetaData game)
  {
    if (game == null)
      return false;
    new GameFlags().SetFlags(game.GameModifierFlags);
    bool flag1 = this._searchBar.CheckIfPassFilter(game.RoomName);
    bool flag2 = true;
    bool flag3 = true;
    bool flag4 = this._mapsFilter[this._currentMap] == LocalizedStrings.All + " Maps" || Singleton<MapManager>.Instance.GetMapName(game.MapID) == this._mapsFilter[this._currentMap];
    bool flag5 = this._modesFilter[this._currentMode] == LocalizedStrings.All + " Modes" || GameModes.GetModeName((GameMode) game.GameMode) == this._modesFilter[this._currentMode];
    if (this._gameNotFull)
      flag2 = !game.IsFull;
    if (this._noPrivateGames)
      flag3 = game.IsPublic;
    if (!this._showFilters)
      return flag1;
    return flag1 && flag4 && flag5 && flag2 && flag3 && this._showFilters;
  }

  private int DrawAllGames(Rect rect, bool hasVScroll)
  {
    int playerLevelSecure = PlayerDataManager.PlayerLevelSecure;
    int num = 0;
    foreach (GameMetaData cachedGame in this._cachedGameList)
    {
      if (this.CanPassFilter(cachedGame))
      {
        bool enabled = GUI.enabled;
        bool flag = (!cachedGame.IsFull && cachedGame.IsLevelAllowed(playerLevelSecure) && (cachedGame.ConnectedPlayers > 0 || cachedGame.HasLevelRestriction) || PlayerDataManager.AccessLevel >= MemberAccessLevel.SeniorModerator) & Singleton<MapManager>.Instance.HasMapWithId(cachedGame.MapID);
        GUI.enabled = enabled && flag && this._dropDownList == 0 && !this._checkForPassword;
        int top = 48 * num - 1;
        if (!ApplicationDataManager.IsMobile)
        {
          string tooltip = LocalizedStrings.PlayCaps;
          if (!cachedGame.IsLevelAllowed(playerLevelSecure) && cachedGame.LevelMin > playerLevelSecure)
            tooltip = string.Format(LocalizedStrings.YouHaveToReachLevelNToJoinThisGame, (object) cachedGame.LevelMin);
          else if (!cachedGame.IsLevelAllowed(playerLevelSecure) && cachedGame.LevelMax < playerLevelSecure)
            tooltip = string.Format(LocalizedStrings.YouAlreadyMasteredThisLevel);
          else if (cachedGame.IsFull)
            tooltip = string.Format(LocalizedStrings.ThisGameIsFull);
          else if (!cachedGame.HasLevelRestriction && cachedGame.ConnectedPlayers == 0)
            tooltip = string.Format(LocalizedStrings.ThisGameNoLongerExists);
          GUI.Box(new Rect(0.0f, (float) top, rect.width, 49f), new GUIContent(string.Empty, tooltip), BlueStonez.box_grey50);
        }
        if (this._selectedGame != null && this._selectedGame.RoomID == cachedGame.RoomID)
        {
          GUI.color = new Color(1f, 1f, 1f, 0.03f);
          GUI.DrawTexture(new Rect(1f, (float) top, rect.width + 1f, 48f), (Texture) UberstrikeIconsHelper.White);
          GUI.color = Color.white;
        }
        int height = 48;
        GUIStyle style = !flag ? BlueStonez.label_interparkmed_10pt_left : BlueStonez.label_interparkbold_11pt_left;
        int left1 = 0;
        if (!cachedGame.IsPublic)
          GUI.DrawTexture(new Rect((float) left1, (float) (top + 12), 25f, 25f), (Texture) this._privateGameIcon);
        else if (cachedGame.HasLevelRestriction)
        {
          if (cachedGame.LevelMax <= 5)
            GUI.DrawTexture(new Rect((float) (left1 + 5), (float) (top + 5), 40f, 40f), (Texture) this._level1GameIcon);
          else if (cachedGame.LevelMax <= 10)
            GUI.DrawTexture(new Rect((float) (left1 + 5), (float) (top + 5), 40f, 40f), (Texture) this._level2GameIcon);
          else if (cachedGame.LevelMax <= 20)
            GUI.DrawTexture(new Rect((float) (left1 + 5), (float) (top + 5), 40f, 40f), (Texture) this._level3GameIcon);
          else if (cachedGame.LevelMin >= 40)
            GUI.DrawTexture(new Rect((float) (left1 + 5), (float) (top + 5), 40f, 40f), (Texture) this._level20GameIcon);
          else if (cachedGame.LevelMin >= 30)
            GUI.DrawTexture(new Rect((float) (left1 + 5), (float) (top + 5), 40f, 40f), (Texture) this._level10GameIcon);
          else if (cachedGame.LevelMin >= 20)
            GUI.DrawTexture(new Rect((float) (left1 + 5), (float) (top + 5), 40f, 40f), (Texture) this._level5GameIcon);
          if (playerLevelSecure > cachedGame.LevelMax)
            GUI.DrawTexture(new Rect((float) left1, (float) top, 50f, 50f), (Texture) UberstrikeIcons.LevelMastered);
        }
        else if (Singleton<MapManager>.Instance.IsBlueBox(cachedGame.MapID))
          GUI.Label(new Rect(10f, (float) (top + 6), 64f, 64f), new GUIContent((Texture) UberstrikeIcons.BlueLevel32, LocalizedStrings.BlueLevelTooltip));
        GUI.color = !cachedGame.HasLevelRestriction ? Color.white : new Color(1f, 0.7f, 0.0f);
        int left2 = 60;
        GUI.Label(new Rect((float) left2, (float) top, (float) this._gameNameWidth, 48f), cachedGame.RoomName, style);
        GUI.Label(new Rect((float) (left2 + 5), (float) (top + 15), (float) (this._gameNameWidth - 20), 48f), this.LevelRestrictionText(cachedGame), BlueStonez.label_interparkmed_10pt_left);
        GUI.Label(new Rect((float) (62 + this._gameNameWidth - 3), (float) top, (float) this._mapNameWidth, 48f), Singleton<MapManager>.Instance.GetMapName(cachedGame.MapID), style);
        int left3 = 62 + this._gameNameWidth + this._mapNameWidth - 4;
        GUI.Label(new Rect((float) left3, (float) top, (float) this._gameModeWidth, 48f), GameModes.GetModeName((int) cachedGame.GameMode), style);
        GUI.Label(new Rect((float) left3, (float) (top + 12), (float) this._gameModeWidth, 48f), FpsGameMode.GetGameFlagText(cachedGame), style);
        GUI.Label(new Rect((float) (62 + this._gameNameWidth + this._mapNameWidth + this._gameModeWidth - 5), (float) top, 80f, 48f), CmunePrint.Time(cachedGame.RoundTime), style);
        GUI.Label(new Rect((float) (62 + this._gameNameWidth + this._mapNameWidth + this._gameModeWidth + 80 - 6), (float) top, 80f, 48f), cachedGame.ConnectedPlayersString, style);
        if (GUI.Button(new Rect(0.0f, (float) top, rect.width, (float) height), string.Empty, BlueStonez.label_interparkbold_11pt_left))
        {
          this.ResetLobbyDisconnectTimeout();
          if (this._selectedGame != null && this._selectedGame.RoomID == cachedGame.RoomID && (double) this._gameJoinDoubleClick > (double) Time.time)
          {
            this._gameJoinDoubleClick = 0.0f;
            if (this._selectedGame.IsPublic || PlayerDataManager.AccessLevelSecure >= MemberAccessLevel.SeniorModerator)
            {
              this.JoinGame();
              SfxManager.Play2dAudioClip(GameAudio.JoinGame);
            }
            else
            {
              this._checkForPassword = true;
              this._nextPasswordCheck = Time.time;
            }
          }
          else
            this._gameJoinDoubleClick = Time.time + 0.5f;
          this._selectedGame = cachedGame;
        }
        ++num;
        GUI.color = Color.white;
        GUI.enabled = enabled;
      }
    }
    if (num == 0 && Singleton<GameServerController>.Instance.SelectedServer != null && Singleton<GameServerController>.Instance.SelectedServer.Data.RoomsCreated > 0 && this._cachedGameList.Count > 0)
      GUI.Label(new Rect(0.0f, 0.0f, rect.width, rect.height), "There are no games matching your filter", BlueStonez.label_interparkmed_11pt);
    return num;
  }

  private string LevelRestrictionText(GameMetaData m)
  {
    if (!m.HasLevelRestriction)
      return string.Empty;
    if (m.LevelMax == m.LevelMin)
      return string.Format(LocalizedStrings.PlayerLevelNRestriction, (object) m.LevelMin);
    if (m.LevelMax == (int) byte.MaxValue)
      return string.Format(LocalizedStrings.PlayerLevelNPlusRestriction, (object) m.LevelMin);
    return m.LevelMin == 0 ? string.Format(LocalizedStrings.PlayerLevelNMinusRestriction, (object) (m.LevelMax + 1)) : string.Format(LocalizedStrings.PlayerLevelNToNRestriction, (object) m.LevelMin, (object) m.LevelMax);
  }

  private void UpdateColumnWidth()
  {
    int num = Screen.width - 20 - 25 - 25 - 80 - 80;
    this._gameModeWidth = Mathf.Clamp(Mathf.CeilToInt((float) ((double) num * 25.0 / 100.0)), 100, 200);
    this._mapNameWidth = Mathf.Clamp(Mathf.CeilToInt((float) ((double) num * 25.0 / 100.0)), 100, 250);
    this._gameNameWidth = num - this._gameModeWidth - this._mapNameWidth + 6;
  }

  public void Show()
  {
    if (this._isConnectedToGameServer)
      this.ShowGameSelection();
    else
      this.ShowServerSelection();
  }

  private void ShowGameSelection()
  {
    if (Singleton<GameServerController>.Instance.SelectedServer == null)
      return;
    this._isConnectedToGameServer = true;
    this._cachedGameList.Clear();
    CmuneNetworkManager.CurrentLobbyServer = Singleton<GameServerController>.Instance.SelectedServer;
    LobbyConnectionManager.StartConnection();
    CoroutineManager.StartCoroutine(new CoroutineManager.CoroutineFunction(this.StartUpdatingGameListFromServer));
    CoroutineManager.StartCoroutine(new CoroutineManager.CoroutineFunction(this.StartDisconnectFormServerAfterTimeout));
  }

  private void ShowServerSelection()
  {
    this._isConnectedToGameServer = false;
    if (this._lastSortedServerColumn == PlayPageGUI.ServerListColumns.None)
    {
      this._lastSortedServerColumn = PlayPageGUI.ServerListColumns.ServerSpeed;
      this.SortServerList(this._lastSortedServerColumn, false);
    }
    this.StopGameSelection();
    this.RefreshServerLoad();
  }

  public void Hide() => this.StopGameSelection();

  private void StopGameSelection()
  {
    LobbyConnectionManager.Stop();
    CoroutineManager.StopCoroutine(new CoroutineManager.CoroutineFunction(this.StartUpdatingGameListFromServer));
    CoroutineManager.StopCoroutine(new CoroutineManager.CoroutineFunction(this.StartDisconnectFormServerAfterTimeout));
    this.CheckForPassword = false;
  }

  public void RefreshGameList()
  {
    bool flag = false;
    this._cachedGameList.Clear();
    if (Singleton<GameListManager>.Instance.GamesCount > 0)
    {
      foreach (GameMetaData game in Singleton<GameListManager>.Instance.GameList)
      {
        this._cachedGameList.Add(game);
        if (this._selectedGame != null && game.RoomID == this._selectedGame.RoomID)
          flag = true;
      }
      this.SortGames(this._gameSortingMethod);
      this.ResetLobbyDisconnectTimeout();
    }
    else
      UnityEngine.Debug.LogWarning((object) "Failed to sort game list because games count is zero!");
    if (flag)
      return;
    this._selectedGame = (GameMetaData) null;
  }

  private void SortGames(IComparer<GameMetaData> method)
  {
    GameDataComparer.SortAscending = this._sortGamesAscending;
    this._cachedGameList.Sort(method);
  }

  [DebuggerHidden]
  private IEnumerator StartUpdatingGameListFromServer() => (IEnumerator) new PlayPageGUI.\u003CStartUpdatingGameListFromServer\u003Ec__Iterator16()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartDisconnectFormServerAfterTimeout() => (IEnumerator) new PlayPageGUI.\u003CStartDisconnectFormServerAfterTimeout\u003Ec__Iterator17()
  {
    \u003C\u003Ef__this = this
  };

  private void ResetLobbyDisconnectTimeout() => this._timeToLobbyDisconnect = Time.time + 30f;

  private void RefreshServerLoad()
  {
    if ((double) this._nextServerCheckTime >= (double) Time.time)
      return;
    this._nextServerCheckTime = Time.time + 5f;
    this.StartCoroutine(Singleton<GameServerManager>.Instance.StartUpdatingServerLoads());
  }

  public bool CheckForPassword
  {
    set => this._checkForPassword = value;
  }

  public bool IsConnectedToGameServer => this._isConnectedToGameServer;

  private bool AreFiltersActive => this._currentMap != 0 || this._currentMode != 0 || this._currentWeapon != 0 || this._noFriendFire || this._gameNotFull || this._noPrivateGames || this._instasplat || this._lowGravity || this._justForFun || this._singleWeapon;

  private enum ServerLatency
  {
    Fast = 100, // 0x00000064
    Med = 300, // 0x0000012C
  }

  private enum GameListColumns
  {
    None,
    Lock,
    Star,
    GameName,
    GameMap,
    GameMode,
    PlayerCount,
    GameServerPing,
    GameTime,
  }

  private enum ServerListColumns
  {
    None,
    ServerName,
    ServerCapacity,
    ServerSpeed,
  }

  private class FilterSavedData
  {
    public bool UseFilter;
    public string MapName = string.Empty;
    public string GameMode = string.Empty;
    public bool NoFriendlyFire;
    public bool ISGameNotFull;
    public bool NoPasswordProtection;
  }
}
