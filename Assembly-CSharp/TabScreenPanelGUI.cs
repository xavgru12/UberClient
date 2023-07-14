// Decompiled with JetBrains decompiler
// Type: TabScreenPanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class TabScreenPanelGUI : MonoBehaviour
{
  [SerializeField]
  private Texture2D _iconSplatted;
  private Rect _rect;
  private Vector2 _panelSize;
  private string _gameName = string.Empty;
  private string _serverName = string.Empty;
  private Vector2 _scrollPos;
  private Vector2 _redScrollPos;
  private Vector2 _blueScrollPos;
  private int _redTeamScore;
  private int _blueTeamScore;
  private List<UberStrike.Realtime.UnitySdk.CharacterInfo> _redTeam;
  private List<UberStrike.Realtime.UnitySdk.CharacterInfo> _blueTeam;
  private List<UberStrike.Realtime.UnitySdk.CharacterInfo> _allPlayers;
  private static bool _isEnabled;

  public static TabScreenPanelGUI Instance { get; private set; }

  public static bool Exists => (UnityEngine.Object) TabScreenPanelGUI.Instance != (UnityEngine.Object) null;

  private void Awake()
  {
    TabScreenPanelGUI.Instance = this;
    this.ForceShow = false;
    this._rect = new Rect();
    this._panelSize.x = 700f;
    this._panelSize.y = 400f;
    this._allPlayers = new List<UberStrike.Realtime.UnitySdk.CharacterInfo>(0);
    this._redTeam = new List<UberStrike.Realtime.UnitySdk.CharacterInfo>(0);
    this._blueTeam = new List<UberStrike.Realtime.UnitySdk.CharacterInfo>(0);
  }

  private void Update()
  {
    this._panelSize.x = (float) (Screen.width * 7 / 8);
    this._panelSize.y = (float) (Screen.height * 8 / 9);
    this._rect.x = (float) (((double) Screen.width - (double) this._panelSize.x) * 0.5);
    this._rect.y = (float) (((double) Screen.height - (double) this._panelSize.y) * 0.5);
    this._rect.width = this._panelSize.x;
    this._rect.height = this._panelSize.y;
    if (!GlobalUIRibbon.IsVisible)
      return;
    this._rect.y += 45f;
  }

  private void OnGUI()
  {
    if (!GameState.HasCurrentGame || !GameState.HasCurrentPlayer)
      return;
    bool flag = ((double) AutoMonoBehaviour<InputManager>.Instance.RawValue(GameInputKey.Tabscreen) > 0.0 || this.ForceShow) && !PanelManager.IsAnyPanelOpen && GameState.CurrentGame.CanShowTabscreen;
    if (TabScreenPanelGUI.Enabled != flag)
    {
      TabScreenPanelGUI.Enabled = flag;
      if (flag && this.SortPlayersByRank != null)
        this.SortPlayersByRank((IEnumerable<UberStrike.Realtime.UnitySdk.CharacterInfo>) GameState.CurrentGame.Players.Values);
    }
    if (!TabScreenPanelGUI.Enabled)
      return;
    GUI.depth = 10;
    this.DrawTabScreen();
  }

  public Action<IEnumerable<UberStrike.Realtime.UnitySdk.CharacterInfo>> SortPlayersByRank { get; set; }

  public void SetGameName(string name) => this._gameName = name;

  public void SetServerName(string name) => this._serverName = name;

  public void SetTeamSplats(int blueSplats, int redSplats)
  {
    this._redTeamScore = redSplats;
    this._blueTeamScore = blueSplats;
  }

  private void DrawTabScreen()
  {
    GUI.skin = BlueStonez.Skin;
    GUI.BeginGroup(this._rect, GUIContent.none, BlueStonez.label_interparkmed_11pt);
    this.DoTitle(new Rect(0.0f, 0.0f, this._panelSize.x, 50f));
    int num = true ? 60 : 174;
    switch (GameState.CurrentGameMode)
    {
      case GameMode.TeamDeathMatch:
        this.DoTeamStats(new Rect(0.0f, 46f, this._panelSize.x, this._panelSize.y - (float) num));
        break;
      case GameMode.TeamElimination:
        this.DoTeamStats(new Rect(0.0f, 46f, this._panelSize.x, this._panelSize.y - (float) num));
        break;
      default:
        this._scrollPos = this.DoAllStats(new Rect(0.0f, 46f, this._panelSize.x, this._panelSize.y - (float) num), this._scrollPos, this._allPlayers);
        break;
    }
    GUI.EndGroup();
  }

  private void DoTitle(Rect position)
  {
    GUI.BeginGroup(position, GUIContent.none, BlueStonez.box_overlay);
    GUI.Label(new Rect(10f, 2f, position.width - 230f, 30f), LocalizedStrings.Game + ": ", BlueStonez.label_interparkbold_18pt_left);
    GUI.contentColor = ColorScheme.UberStrikeYellow;
    GUI.Label(new Rect(65f, 2f, position.width - 280f, 30f), this._gameName, BlueStonez.label_interparkbold_18pt_left);
    GUI.contentColor = Color.white;
    GUI.Label(new Rect(10f, position.height - 32f, position.width - 230f, 30f), "Server: " + this._serverName, BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(position.width - 230f, 2f, 230f, 30f), this.MapName, BlueStonez.label_interparkbold_18pt_left);
    GUI.Label(new Rect(position.width - 230f, position.height - 32f, 220f, 30f), this.ModeName, BlueStonez.label_interparkbold_18pt_left);
    GUI.EndGroup();
  }

  private string MapName => LocalizedStrings.Map + ": " + Singleton<MapManager>.Instance.GetMapName(GameState.CurrentSpace.SceneName);

  private string ModeName => LocalizedStrings.Mode + ": " + GameModes.GetModeName(GameState.CurrentGameMode);

  private void DoTeamStats(Rect position)
  {
    Rect position1 = new Rect(position.x, position.y, position.width * 0.5f, position.height);
    Rect position2 = new Rect(position.x + position1.width, position1.y, position1.width, position1.height);
    this._blueScrollPos = this.DoTeam(position1, TeamID.BLUE, this._blueScrollPos, this._blueTeam);
    this._redScrollPos = this.DoTeam(position2, TeamID.RED, this._redScrollPos, this._redTeam);
  }

  private Vector2 DoTeam(
    Rect position,
    TeamID teamID,
    Vector2 scroll,
    List<UberStrike.Realtime.UnitySdk.CharacterInfo> players)
  {
    GUI.BeginGroup(position);
    Color contentColor = GUI.contentColor;
    GUI.BeginGroup(new Rect(0.0f, 0.0f, position.width, 60f), GUIContent.none, BlueStonez.box_overlay);
    GUI.color = teamID != TeamID.BLUE ? ColorScheme.HudTeamRed : ColorScheme.HudTeamBlue;
    if (teamID == TeamID.RED)
    {
      GUI.Label(new Rect(10f, 6f, 200f, 32f), ((Enum) teamID).ToString(), BlueStonez.label_interparkbold_32pt_left);
      GUI.Label(new Rect(10f, 34f, 200f, 18f), string.Format(LocalizedStrings.NPlayers, (object) players.Count), BlueStonez.label_interparkbold_18pt_left);
      if (GameState.CurrentGameMode == GameMode.TeamElimination)
        GUI.Label(new Rect(position.width - 215f, 8f, 200f, 48f), string.Format("{0} / {1}", (object) this._redTeamScore, (object) GameState.CurrentGame.GameData.SplatLimit), BlueStonez.label_interparkbold_48pt_right);
      else
        GUI.Label(new Rect(position.width - 215f, 8f, 200f, 48f), this._redTeamScore.ToString(), BlueStonez.label_interparkbold_48pt_right);
    }
    else
    {
      if (GameState.CurrentGameMode == GameMode.TeamElimination)
        GUI.Label(new Rect(15f, 8f, 200f, 48f), string.Format("{0} / {1}", (object) this._blueTeamScore, (object) GameState.CurrentGame.GameData.SplatLimit), BlueStonez.label_interparkbold_48pt_left);
      else
        GUI.Label(new Rect(15f, 8f, 200f, 48f), this._blueTeamScore.ToString(), BlueStonez.label_interparkbold_48pt_left);
      GUI.Label(new Rect(position.width - 210f, 6f, 200f, 32f), ((Enum) teamID).ToString(), BlueStonez.label_interparkbold_32pt_right);
      GUI.Label(new Rect(position.width - 210f, 34f, 200f, 18f), string.Format(LocalizedStrings.NPlayers, (object) players.Count), BlueStonez.label_interparkbold_18pt_right);
    }
    GUI.EndGroup();
    GUI.color = contentColor;
    scroll = this.DoAllStats(new Rect(0.0f, 56f, position.width, position.height - 56f), scroll, players);
    GUI.EndGroup();
    return scroll;
  }

  public void SetPlayerListAll(List<UberStrike.Realtime.UnitySdk.CharacterInfo> players) => this._allPlayers = players;

  private Vector2 DoAllStats(Rect position, Vector2 scroll, List<UberStrike.Realtime.UnitySdk.CharacterInfo> players)
  {
    int num1 = 8;
    int num2 = 25;
    int width1 = 25;
    int width2 = 30;
    int num3 = 32;
    int width3 = (double) position.width <= 540.0 ? 0 : 150;
    int width4 = (double) position.width <= 420.0 ? 0 : 50;
    int width5 = (double) position.width <= 450.0 ? 0 : 30;
    int num4 = (double) position.width <= 490.0 ? 0 : 40;
    int width6 = 30;
    int width7 = 50;
    int width8 = Mathf.Clamp(Mathf.RoundToInt(position.width - 30f - (float) num1 - (float) num2 - (float) width1 - (float) width2 - (float) num3 - (float) width3 - (float) width6 - (float) width7 - (float) width4 - (float) width5 - (float) num4), 110, 300);
    GUI.BeginGroup(position, GUIContent.none, BlueStonez.box_overlay);
    int left1 = 10 + num1 + num2;
    GUI.Label(new Rect((float) left1, 10f, (float) width8, 18f), LocalizedStrings.Name, BlueStonez.label_interparkmed_18pt_left);
    int left2 = left1 + width8;
    GUI.Label(new Rect((float) left2, 15f, (float) width2, 18f), LocalizedStrings.Kills, BlueStonez.label_interparkmed_11pt_left);
    int left3 = left2 + width2;
    if (width4 > 0)
    {
      GUI.Label(new Rect((float) left3, 15f, (float) width4, 18f), LocalizedStrings.Deaths, BlueStonez.label_interparkmed_11pt_left);
      left3 += width4;
    }
    if (width5 > 0)
    {
      GUI.Label(new Rect((float) left3, 15f, (float) width5, 18f), LocalizedStrings.KDR, BlueStonez.label_interparkmed_11pt_left);
      left3 += width5;
    }
    GUI.Label(new Rect((float) left3, 10f, (float) width6, 18f), GUIContent.none, BlueStonez.label_interparkbold_16pt_left);
    int left4 = left3 + width6;
    GUI.Label(new Rect((float) left4, 15f, (float) (width1 + 10), 18f), LocalizedStrings.Level, BlueStonez.label_interparkmed_11pt_left);
    int left5 = left4 + width1;
    GUI.Label(new Rect((float) left5, 10f, (float) (num3 + width3), 18f), GUIContent.none, BlueStonez.label_interparkbold_16pt_left);
    int num5 = left5 + (num3 + width3);
    GUI.Label(new Rect(position.width - (float) width7, 10f, (float) width7, 18f), LocalizedStrings.Ping, BlueStonez.label_interparkmed_18pt_left);
    GUI.Label(new Rect(10f, 32f, position.width - 20f, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
    scroll = GUITools.BeginScrollView(new Rect(10f, 36f, position.width - 20f, position.height - 45f), scroll, new Rect(0.0f, 0.0f, position.width - 40f, (float) (players.Count * 36)));
    int num6 = 0;
    foreach (UberStrike.Realtime.UnitySdk.CharacterInfo player in players)
    {
      int left6 = num1;
      GUI.BeginGroup(new Rect(0.0f, (float) (num6 * 36), position.width, 36f));
      if (player.ActorId == GameState.CurrentPlayerID)
      {
        GUI.color = new Color(1f, 1f, 1f, 0.3f);
        GUI.Box(new Rect(0.0f, 0.0f, position.width - 21f, 36f), GUIContent.none, BlueStonez.box_white_rounded);
        GUI.color = Color.white;
      }
      GUI.DrawTexture(new Rect((float) left6, 10f, 16f, 16f), (Texture) UberstrikeIconsHelper.GetIconForChannel(player.Channel));
      int left7 = left6 + num2;
      Color contentColor = GUI.contentColor;
      GUI.color = Color.white;
      if (!GameState.CurrentGame.HasAvatarLoaded(player.ActorId))
        GUI.color = Color.gray;
      else if (player.TeamID == TeamID.BLUE)
        GUI.color = ColorScheme.HudTeamBlue;
      else if (player.TeamID == TeamID.RED)
        GUI.color = ColorScheme.HudTeamRed;
      GUI.Label(new Rect((float) left7, 0.0f, (float) width8, 36f), player.PlayerName, BlueStonez.label_interparkbold_11pt_left_wrap);
      GUI.color = contentColor;
      int left8 = left7 + width8;
      GUI.Label(new Rect((float) left8, 0.0f, (float) width2, 36f), player.Kills.ToString(), BlueStonez.label_interparkbold_11pt_left);
      int left9 = left8 + width2;
      if (width4 > 0)
      {
        GUI.Label(new Rect((float) left9, 0.0f, (float) width4, 36f), player.Deaths.ToString("N0"), BlueStonez.label_interparkbold_11pt_left);
        left9 += width4;
      }
      if (width5 > 0)
      {
        GUI.Label(new Rect((float) left9, 0.0f, (float) width5, 36f), player.KDR.ToString("N1"), BlueStonez.label_interparkbold_11pt_left);
        left9 += width5;
      }
      if (!player.IsAlive)
        GUI.Label(new Rect((float) left9, 6f, 25f, 25f), (Texture) this._iconSplatted, BlueStonez.label_interparkbold_11pt_right);
      int num7 = left9 + width6;
      GUI.Label(new Rect((float) (num7 + 5), 0.0f, (float) width1, 36f), player.Level.ToString(), BlueStonez.label_interparkbold_11pt_left);
      int left10 = num7 + (width1 + 5);
      WeaponItem weaponItemInShop = Singleton<ItemManager>.Instance.GetWeaponItemInShop(player.CurrentWeaponID);
      if ((UnityEngine.Object) weaponItemInShop != (UnityEngine.Object) null)
      {
        GUI.Label(new Rect((float) left10, 2f, 32f, 32f), (Texture) weaponItemInShop.Icon, BlueStonez.box_black);
        int num8 = left10 + num3;
        if (width3 > 0)
        {
          GUI.Label(new Rect((float) (num8 + 10), 0.0f, (float) width3, 36f), weaponItemInShop.Name, BlueStonez.label_interparkbold_11pt_left);
          num5 = num8 + width3;
        }
      }
      else
        num5 = left10 + num3;
      GUI.Label(new Rect(position.width - 40f - (float) width7, 0.0f, (float) width7, 36f), player.Ping.ToString(), BlueStonez.label_interparkbold_11pt_right);
      GUI.EndGroup();
      ++num6;
    }
    GUITools.EndScrollView();
    GUI.EndGroup();
    return scroll;
  }

  public void SetPlayerListRed(List<UberStrike.Realtime.UnitySdk.CharacterInfo> redPlayers) => this._redTeam = redPlayers;

  public void SetPlayerListBlue(List<UberStrike.Realtime.UnitySdk.CharacterInfo> bluePlayers) => this._blueTeam = bluePlayers;

  public static bool Enabled
  {
    get => TabScreenPanelGUI._isEnabled;
    set
    {
      if (TabScreenPanelGUI._isEnabled == value)
        return;
      TabScreenPanelGUI._isEnabled = value;
      Singleton<HudDrawFlagGroup>.Instance.TuningTabScreen = value;
    }
  }

  public int BluePlayerCount => this._blueTeam.Count;

  public int RedPlayerCount => this._redTeam.Count;

  public bool ForceShow { get; set; }
}
