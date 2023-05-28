// Decompiled with JetBrains decompiler
// Type: CreateGamePanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class CreateGamePanelGUI : MonoBehaviour, IPanelGui
{
  private const int OFFSET = 6;
  private const int BUTTON_HEIGHT = 50;
  private const int MAP_WIDTH = 200;
  private const int MODE_WIDTH = 160;
  private const int DESC_WIDTH = 255;
  private const int MODS_WIDTH = 360;
  private const int MIN_WIDTH = 640;
  private const int MAX_WIDTH = 960;
  private const int MIN_NAME_FIELD_WIDTH = 115;
  private const int MAX_NAME_FIELD_WIDTH = 150;
  private const int LEFT_X = 0;
  private const int RIGHT_X = 370;
  private bool _animatingWidth;
  private bool _animatingIndex;
  private float _xOffset;
  private bool _viewingLeft = true;
  private GameFlags.GAME_FLAGS _gameFlags;
  private UberstrikeMap _mapSelected;
  private SelectionGroup<GameModeType> _modeSelection = new SelectionGroup<GameModeType>();
  private Rect _windowRect;
  private Vector2 _scroll = Vector2.zero;
  private string _gameName = string.Empty;
  private string _password = string.Empty;
  private float _textFieldWidth = 170f;
  private float _sliderWidth = 130f;
  private string _dmDescMsg = string.Empty;
  private string _tdmDescMsg = string.Empty;
  private string _elmDescMsg = string.Empty;

  private void Awake() => this._gameName = string.Empty;

  private void Start()
  {
    this._dmDescMsg = LocalizedStrings.DMModeDescriptionMsg;
    this._tdmDescMsg = LocalizedStrings.TDMModeDescriptionMsg;
    this._elmDescMsg = LocalizedStrings.ELMModeDescriptionMsg;
    this._modeSelection.Add(GameModeType.TeamDeathMatch, new GUIContent(LocalizedStrings.TeamDeathMatch));
    this._modeSelection.Add(GameModeType.EliminationMode, new GUIContent(LocalizedStrings.TeamElimination));
    this._modeSelection.Add(GameModeType.DeathMatch, new GUIContent(LocalizedStrings.DeathMatch));
    this._modeSelection.OnSelectionChange += (Action<GameModeType>) (mode => { });
  }

  private void Update()
  {
    if ((double) this._windowRect.width != 960.0 && Screen.width >= 989 || (double) this._windowRect.width != 640.0 && Screen.width < 989)
      this._animatingWidth = true;
    if (this._animatingWidth)
    {
      if (Screen.width < 989)
      {
        this._sliderWidth = Mathf.Lerp(this._sliderWidth, 160f, Time.deltaTime * 8f);
        this._textFieldWidth = Mathf.Lerp(this._textFieldWidth, 150f, Time.deltaTime * 8f);
        this._windowRect.width = Mathf.Lerp(this._windowRect.width, 640f, Time.deltaTime * 8f);
        if (Mathf.Approximately(this._windowRect.width, 640f))
        {
          this._animatingWidth = false;
          this._sliderWidth = 160f;
          this._textFieldWidth = 150f;
          this._windowRect.width = 640f;
        }
      }
      else
      {
        this._sliderWidth = Mathf.Lerp(this._sliderWidth, 130f, Time.deltaTime * 8f);
        this._textFieldWidth = Mathf.Lerp(this._textFieldWidth, 115f, Time.deltaTime * 8f);
        this._windowRect.width = Mathf.Lerp(this._windowRect.width, 960f, Time.deltaTime * 8f);
        if (Mathf.Approximately(this._windowRect.width, 960f))
        {
          this._animatingWidth = false;
          this._sliderWidth = 130f;
          this._textFieldWidth = 115f;
          this._windowRect.width = 960f;
        }
      }
    }
    if (this._animatingIndex)
    {
      if (this._viewingLeft)
      {
        this._xOffset = Mathf.Lerp(this._xOffset, 0.0f, Time.deltaTime * 8f);
        if ((double) Mathf.Abs(this._xOffset) < 2.0)
        {
          this._xOffset = 0.0f;
          this._animatingIndex = false;
        }
      }
      else
      {
        this._xOffset = Mathf.Lerp(this._xOffset, 370f, Time.deltaTime * 8f);
        if ((double) Mathf.Abs(370f - this._xOffset) < 2.0)
        {
          this._xOffset = 370f;
          this._animatingIndex = false;
        }
      }
    }
    this._windowRect.x = (float) (((double) Screen.width - (double) this._windowRect.width) * 0.5);
    this._windowRect.y = (float) (((double) Screen.height - (double) this._windowRect.height) * 0.5 + 25.0);
  }

  private void OnGUI()
  {
    GUI.BeginGroup(this._windowRect, GUIContent.none, BlueStonez.window);
    this.DrawCreateGamePanel();
    GUI.EndGroup();
    GuiManager.DrawTooltip();
  }

  private void OnEnable()
  {
    this._windowRect.width = Screen.width >= 989 ? 960f : 640f;
    this._windowRect.height = 420f;
    this._password = string.Empty;
    if (Screen.width < 989)
    {
      this._sliderWidth = 160f;
      this._windowRect.width = 640f;
      this._textFieldWidth = 150f;
    }
    else
    {
      this._sliderWidth = 130f;
      this._windowRect.width = 960f;
      this._textFieldWidth = 115f;
    }
  }

  public void Show()
  {
    this.enabled = true;
    this._viewingLeft = true;
    this._gameName = PlayerDataManager.Name;
    if (this._gameName.Length <= 18)
      return;
    this._gameName = this._gameName.Remove(18);
  }

  public void Hide() => this.enabled = false;

  private void DrawCreateGamePanel()
  {
    GUI.skin = BlueStonez.Skin;
    GUI.depth = 3;
    GUI.Label(new Rect(0.0f, 0.0f, this._windowRect.width, 56f), LocalizedStrings.CreateGameCaps, BlueStonez.tab_strip);
    Rect rect = new Rect(0.0f, 60f, this._windowRect.width, this._windowRect.height - 60f);
    if (Screen.width < 989)
      this.DrawRestrictedPanel(rect);
    else
      this.DrawFullPanel(rect);
  }

  private void SelectMap(UberstrikeMap map)
  {
    this._mapSelected = map;
    foreach (GameModeType mode in this._modeSelection.Items)
    {
      if (this._mapSelected.IsGameModeSupported(mode))
      {
        this._modeSelection.Select(mode);
        break;
      }
    }
  }

  private void DrawMapSelection(Rect rect)
  {
    float width = Singleton<MapManager>.Instance.Count <= 8 ? rect.width : rect.width - 18f;
    this._scroll = GUITools.BeginScrollView(rect, this._scroll, new Rect(0.0f, 0.0f, rect.width - 18f, (float) (10 + Singleton<MapManager>.Instance.Count * 35)));
    int num = 0;
    foreach (UberstrikeMap allMap in Singleton<MapManager>.Instance.AllMaps)
    {
      if (allMap.IsVisible)
      {
        if (this._mapSelected == null)
          this.SelectMap(allMap);
        GUIContent content = !allMap.IsBluebox ? new GUIContent(allMap.Name) : new GUIContent(" " + allMap.Name, (Texture) UberstrikeIcons.BlueLevel32);
        if (GUI.Toggle(new Rect(0.0f, (float) (num * 35), width, 35f), allMap == this._mapSelected, content, BlueStonez.tab_large_left) && this._mapSelected != allMap)
        {
          SfxManager.Play2dAudioClip(GameAudio.CreateGame);
          this.SelectMap(allMap);
        }
        ++num;
      }
    }
    GUITools.EndScrollView();
  }

  private void DrawGameModeSelection(Rect rect)
  {
    GUI.BeginGroup(rect);
    for (int index = 0; index < this._modeSelection.Items.Length; ++index)
    {
      GUITools.PushGUIState();
      if (this._mapSelected != null && !this._mapSelected.IsGameModeSupported(this._modeSelection.Items[index]))
        GUI.enabled = false;
      if (GUI.Toggle(new Rect(0.0f, (float) (index * 20), rect.width, 20f), index == this._modeSelection.Index, this._modeSelection.GuiContent[index], BlueStonez.tab_medium) && this._modeSelection.Index != index)
      {
        this._modeSelection.SetIndex(index);
        if (GUI.changed)
        {
          GUI.changed = false;
          SfxManager.Play2dAudioClip(GameAudio.CreateGame);
        }
      }
      GUI.enabled = true;
      GUITools.PopGUIState();
    }
    GUI.EndGroup();
  }

  private void DrawGameDescription(Rect rect)
  {
    string text = string.Empty;
    switch (this._modeSelection.Current)
    {
      case GameModeType.DeathMatch:
        text = this._dmDescMsg;
        break;
      case GameModeType.TeamDeathMatch:
        text = this._tdmDescMsg;
        break;
      case GameModeType.EliminationMode:
        text = this._elmDescMsg;
        break;
    }
    GUI.BeginGroup(rect);
    if (this._mapSelected != null)
    {
      int num = 0;
      this._mapSelected.Icon.Draw(new Rect(0.0f, 6f, rect.width, rect.width * this._mapSelected.Icon.Aspect));
      int top1 = num + (6 + Mathf.RoundToInt(rect.width * this._mapSelected.Icon.Aspect));
      GUI.Label(new Rect(6f, (float) top1, rect.width - 12f, 20f), "Mission", BlueStonez.label_interparkbold_11pt_left);
      int top2 = top1 + 20;
      GUI.Label(new Rect(6f, (float) top2, rect.width - 12f, 60f), text, BlueStonez.label_itemdescription);
      int top3 = top2 + 36;
      GUI.Label(new Rect(6f, (float) top3, rect.width - 12f, 20f), "Location", BlueStonez.label_interparkbold_11pt_left);
      GUI.Label(new Rect(6f, (float) (top3 + 20), rect.width - 12f, 100f), this._mapSelected.Description, BlueStonez.label_itemdescription);
    }
    else
      GUI.Label(new Rect(6f, 100f, rect.width - 12f, 100f), "Please select a map", BlueStonez.label_interparkbold_16pt);
    GUI.EndGroup();
  }

  private void DrawGameConfiguration(Rect rect)
  {
    if (this.IsModeSupported)
    {
      MapSettings setting = this._mapSelected.View.Settings[this._modeSelection.Current];
      if (ApplicationDataManager.IsMobile)
        setting.PlayersMax = Mathf.Min(setting.PlayersMax, 8);
      GUI.BeginGroup(rect);
      GUI.Label(new Rect(6f, 3f, 100f, 30f), LocalizedStrings.GameName, BlueStonez.label_interparkbold_18pt_left);
      GUI.SetNextControlName("GameName");
      this._gameName = GUI.TextField(new Rect(120f, 8f, this._textFieldWidth, 24f), this._gameName, 18, BlueStonez.textField);
      if (string.IsNullOrEmpty(this._gameName) && !GUI.GetNameOfFocusedControl().Equals("GameName"))
      {
        GUI.color = new Color(1f, 1f, 1f, 0.3f);
        GUI.Label(new Rect(128f, 15f, 200f, 24f), LocalizedStrings.EnterGameName, BlueStonez.label_interparkmed_11pt_left);
        GUI.color = Color.white;
      }
      if (this._gameName.Length > 18)
        this._gameName = this._gameName.Remove(18);
      GUI.Label(new Rect((float) (120.0 + (double) this._textFieldWidth + 16.0), 8f, 100f, 24f), "(" + (object) this._gameName.Length + "/18)", BlueStonez.label_interparkbold_11pt_left);
      GUI.Label(new Rect(6f, 36f, 100f, 30f), LocalizedStrings.Password, BlueStonez.label_interparkbold_18pt_left);
      GUI.SetNextControlName("GamePasswd");
      this._password = GUI.PasswordField(new Rect(120f, 38f, this._textFieldWidth, 24f), this._password, '*', 8);
      this._password = this._password.Trim('\n');
      if (string.IsNullOrEmpty(this._password) && !GUI.GetNameOfFocusedControl().Equals("GamePasswd"))
      {
        GUI.color = new Color(1f, 1f, 1f, 0.3f);
        GUI.Label(new Rect(128f, 45f, 200f, 24f), "No password", BlueStonez.label_interparkmed_11pt_left);
        GUI.color = Color.white;
      }
      if (this._password.Length > 8)
        this._password = this._password.Remove(8);
      GUI.Label(new Rect((float) (120.0 + (double) this._textFieldWidth + 16.0), 38f, 100f, 24f), "(" + (object) this._password.Length + "/8)", BlueStonez.label_interparkbold_11pt_left);
      GUI.Label(new Rect(6f, 70f, 100f, 30f), LocalizedStrings.MaxPlayers, BlueStonez.label_interparkbold_18pt_left);
      GUI.Label(new Rect(120f, 73f, 33f, 20f), Mathf.RoundToInt((float) setting.PlayersCurrent).ToString(), BlueStonez.label_dropdown);
      setting.PlayersCurrent = !ApplicationDataManager.IsMobile ? setting.PlayersCurrent : Mathf.Clamp(setting.PlayersCurrent, 0, 8);
      setting.PlayersCurrent = (int) GUI.HorizontalSlider(new Rect(160f, 77f, this._sliderWidth, 20f), (float) setting.PlayersCurrent, (float) setting.PlayersMin, (float) setting.PlayersMax);
      int num = Mathf.RoundToInt((float) (setting.TimeCurrent / 60));
      GUI.Label(new Rect(6f, 100f, 100f, 30f), LocalizedStrings.TimeLimit, BlueStonez.label_interparkbold_18pt_left);
      GUI.Label(new Rect(120f, 103f, 33f, 20f), num.ToString(), BlueStonez.label_dropdown);
      setting.TimeCurrent = 60 * (int) GUI.HorizontalSlider(new Rect(160f, 107f, this._sliderWidth, 20f), (float) num, (float) (setting.TimeMin / 60), (float) (setting.TimeMax / 60));
      GUI.Label(new Rect(6f, 132f, 100f, 30f), this._modeSelection.Current != GameModeType.EliminationMode ? LocalizedStrings.MaxKills : LocalizedStrings.MaxRounds, BlueStonez.label_interparkbold_18pt_left);
      GUI.Label(new Rect(120f, 135f, 33f, 20f), Mathf.RoundToInt((float) setting.KillsCurrent).ToString(), BlueStonez.label_dropdown);
      setting.KillsCurrent = (int) GUI.HorizontalSlider(new Rect(160f, 139f, this._sliderWidth, 20f), (float) setting.KillsCurrent, (float) setting.KillsMin, (float) setting.KillsMax);
      GUI.EndGroup();
    }
    else
      GUI.Label(rect, "Unsupported Game Mode!", BlueStonez.label_interparkbold_18pt);
  }

  private void ToggleGameFlag(GameFlags.GAME_FLAGS flag, int y, string content)
  {
    if (GUI.Toggle(new Rect(6f, (float) y, 160f, 16f), this._gameFlags == flag, content, BlueStonez.toggle))
    {
      this._gameFlags = flag;
    }
    else
    {
      if (this._gameFlags != flag)
        return;
      this._gameFlags = GameFlags.GAME_FLAGS.None;
    }
  }

  private bool IsModeSupported => this._mapSelected != null && this._mapSelected.IsGameModeSupported(this._modeSelection.Current);

  private void DrawFullPanel(Rect rect)
  {
    int left1 = 6;
    int height = (int) rect.height - 50;
    GUI.BeginGroup(rect);
    GUI.Box(new Rect(6f, 0.0f, rect.width - 12f, (float) height), GUIContent.none, BlueStonez.window_standard_grey38);
    this.DrawMapSelection(new Rect((float) left1, 0.0f, 200f, (float) height));
    int left2 = left1 + 206;
    this.DrawVerticalLine((float) (left2 - 3), 2f, 300f);
    this.DrawGameModeSelection(new Rect((float) left2, 0.0f, 160f, (float) height));
    int left3 = left2 + 166;
    this.DrawVerticalLine((float) (left3 - 3), 2f, 300f);
    this.DrawGameDescription(new Rect((float) left3, 0.0f, (float) byte.MaxValue, (float) height));
    int left4 = left3 + 261;
    this.DrawVerticalLine((float) (left4 - 3), 2f, 300f);
    this.DrawGameConfiguration(new Rect((float) left4, 0.0f, 360f, (float) height));
    if (GUITools.Button(new Rect(rect.width - 138f, rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button))
      PanelManager.Instance.ClosePanel(PanelType.CreateGame);
    GUITools.PushGUIState();
    GUI.enabled = this.IsModeSupported && Singleton<GameServerController>.Instance.SelectedServer.IsValid && LocalizationHelper.ValidateMemberName(this._gameName, ApplicationDataManager.CurrentLocale) && (string.IsNullOrEmpty(this._password) || this.ValidateGamePassword(this._password));
    if (GUITools.Button(new Rect((float) ((double) rect.width - 138.0 - 125.0), rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.CreateCaps), BlueStonez.button_green, GameAudio.JoinGame))
    {
      PanelManager.Instance.ClosePanel(PanelType.CreateGame);
      MapSettings setting = this._mapSelected.View.Settings[this._modeSelection.Current];
      this._gameName = TextUtilities.Trim(this._gameName);
      Singleton<GameStateController>.Instance.CreateGame(this._mapSelected, this._gameName, this._password, Mathf.RoundToInt((float) (setting.TimeCurrent / 60)) * 60, setting.KillsCurrent, setting.PlayersCurrent, this._modeSelection.Current, this._gameFlags);
    }
    GUITools.PopGUIState();
    GUI.EndGroup();
  }

  private void DrawRestrictedPanel(Rect rect)
  {
    float left1 = 6f - this._xOffset;
    int height = (int) rect.height - 50;
    GUI.BeginGroup(rect);
    GUI.Box(new Rect(6f, 0.0f, rect.width - 12f, (float) height), GUIContent.none, BlueStonez.window_standard_grey38);
    if (this._animatingIndex || this._viewingLeft)
      this.DrawMapSelection(new Rect(left1, 0.0f, 200f, (float) height));
    float left2 = left1 + 206f;
    if (this._animatingIndex || this._viewingLeft)
    {
      this.DrawVerticalLine(left2 - 3f, 2f, 300f);
      this.DrawGameModeSelection(new Rect(left2, 0.0f, 160f, (float) height));
    }
    float left3 = left2 + 166f;
    if (this._animatingIndex || this._viewingLeft)
      this.DrawVerticalLine(left3 - 3f, 2f, 300f);
    this.DrawGameDescription(new Rect(left3, 0.0f, (float) byte.MaxValue, (float) height));
    float left4 = left3 + 261f;
    if (this._animatingIndex || !this._viewingLeft)
      this.DrawVerticalLine(left4 - 3f, 2f, 300f);
    this.DrawGameConfiguration(new Rect(left4, 0.0f, 360f, (float) height));
    if (GUITools.Button(new Rect(rect.width - 138f, rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button))
      PanelManager.Instance.ClosePanel(PanelType.CreateGame);
    GUITools.PushGUIState();
    GUI.enabled = !this._animatingIndex && !this._animatingWidth;
    string text = !this._viewingLeft ? "Back" : "Customize";
    if (GUITools.Button(new Rect((float) ((double) rect.width - 138.0 - 125.0), rect.height - 40f, 120f, 32f), new GUIContent(text), BlueStonez.button))
    {
      this._animatingIndex = true;
      this._viewingLeft = !this._viewingLeft;
    }
    GUITools.PopGUIState();
    GUITools.PushGUIState();
    GUI.enabled = this.IsModeSupported && Singleton<GameServerController>.Instance.SelectedServer.IsValid && LocalizationHelper.ValidateMemberName(this._gameName, ApplicationDataManager.CurrentLocale) && (string.IsNullOrEmpty(this._password) || this.ValidateGamePassword(this._password));
    if (GUITools.Button(new Rect((float) ((double) rect.width - 138.0 - 250.0), rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.CreateCaps), BlueStonez.button_green))
    {
      PanelManager.Instance.ClosePanel(PanelType.CreateGame);
      MapSettings setting = this._mapSelected.View.Settings[this._modeSelection.Current];
      Singleton<GameStateController>.Instance.CreateGame(this._mapSelected, this._gameName, this._password, setting.TimeCurrent, setting.KillsCurrent, setting.PlayersCurrent, this._modeSelection.Current, this._gameFlags);
    }
    GUITools.PopGUIState();
    GUI.EndGroup();
  }

  private void DrawVerticalLine(float x, float y, float height) => GUI.Label(new Rect(x, y, 1f, height), GUIContent.none, BlueStonez.vertical_line_grey95);

  private bool ValidateGamePassword(string psv)
  {
    bool flag = false;
    if (!string.IsNullOrEmpty(psv) && psv.Length <= 8)
      flag = true;
    return flag;
  }

  public bool IsEnabled => this.enabled;
}
