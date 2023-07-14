// Decompiled with JetBrains decompiler
// Type: OptionsPanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPanelGUI : PanelGuiBase
{
  private const int MasterTextureLimit = 5;
  private const int TAB_GENERAL = 0;
  private const int TAB_CONTROL = 1;
  private const int TAB_AUDIO = 2;
  private const int TAB_VIDEO = 3;
  private const int TAB_SYSINFO = 4;
  private const int GroupMarginX = 8;
  private bool showResolutions;
  private bool graphicsChanged;
  private string[] qualitySet;
  private string[] vsyncSet = new string[3]
  {
    "Off",
    "Low",
    "High"
  };
  private string[] antiAliasingSet = new string[4]
  {
    "Off",
    "2x",
    "4x",
    "8x"
  };
  private string[] waterSet = new string[3]
  {
    "Low",
    "Medium",
    "High"
  };
  private int _currentQuality;
  private float _targetFrameRate = -1f;
  private float _textureQuality;
  private float _queuedFrames;
  private int _vsync;
  private int _antiAliasing;
  private int _waterQuality;
  private Rect _rect;
  private Vector2 _scrollVideo;
  private Vector2 _scrollControls;
  private int _desiredWidth;
  private int _selectedOptionsTab = 3;
  private GUIContent[] _optionsTabs;
  private UserInputMap _targetMap;
  private bool _showWaterModeMenu;
  private int _keyCount;
  private string[] _screenResText;
  private bool INSTANT_SCREEN_RES_CHANGE = true;
  private bool _isFullscreenBefore;
  private float _screenResChangeDelay;
  private int _newScreenResIndex;

  private void Awake()
  {
    List<string> stringList = new List<string>();
    int num = 0;
    string str = string.Empty;
    foreach (Resolution resolution in ScreenResolutionManager.Resolutions)
    {
      if (++num == ScreenResolutionManager.Resolutions.Count)
        str = string.Format("({0})", (object) LocalizedStrings.FullscreenOnly);
      stringList.Add(string.Format("{0} X {1} {2}", (object) resolution.width, (object) resolution.height, (object) str));
    }
    ArrayList arrayList = new ArrayList((ICollection) QualitySettings.names);
    if (arrayList.Contains((object) "Mobile"))
      arrayList.Remove((object) "Mobile");
    this.qualitySet = new string[arrayList.Count + 1];
    for (int index = 0; index < this.qualitySet.Length; ++index)
      this.qualitySet[index] = index >= arrayList.Count ? LocalizedStrings.Custom : arrayList[index].ToString();
    this._screenResText = stringList.ToArray();
  }

  private void OnEnable() => this.SyncGraphicsSettings();

  private void Start()
  {
    if (ApplicationDataManager.IsMobile)
    {
      this._optionsTabs = new GUIContent[4]
      {
        new GUIContent(LocalizedStrings.GeneralCaps),
        new GUIContent(LocalizedStrings.ControlsCaps),
        new GUIContent(LocalizedStrings.AudioCaps),
        new GUIContent(LocalizedStrings.SysInfoCaps)
      };
      this._selectedOptionsTab = 1;
    }
    else
    {
      this._optionsTabs = new GUIContent[5]
      {
        new GUIContent(LocalizedStrings.GeneralCaps),
        new GUIContent(LocalizedStrings.ControlsCaps),
        new GUIContent(LocalizedStrings.AudioCaps),
        new GUIContent(LocalizedStrings.VideoCaps),
        new GUIContent(LocalizedStrings.SysInfoCaps)
      };
      this._keyCount = AutoMonoBehaviour<InputManager>.Instance.KeyMapping.Values.Count;
    }
  }

  private void OnGUI()
  {
    GUI.depth = -97;
    this._rect = new Rect((float) ((Screen.width - 528) / 2), (float) ((Screen.height - 300) / 2), 528f, 300f);
    GUI.BeginGroup(this._rect, GUIContent.none, BlueStonez.window_standard_grey38);
    if ((double) this._screenResChangeDelay > 0.0)
      this.DrawScreenResChangePanel();
    else
      this.DrawOptionsPanel();
    GUI.EndGroup();
    GuiManager.DrawTooltip();
  }

  private void DrawOptionsPanel()
  {
    GUI.SetNextControlName("OptionPanelHeading");
    GUI.Label(new Rect(0.0f, 0.0f, this._rect.width, 56f), LocalizedStrings.OptionsCaps, BlueStonez.tab_strip);
    if (GUI.GetNameOfFocusedControl() != "OptionPanelHeading")
      GUI.FocusControl("OptionPanelHeading");
    this._selectedOptionsTab = UnityGUI.Toolbar(new Rect(2f, 31f, this._rect.width - 5f, 22f), this._selectedOptionsTab, this._optionsTabs, this._optionsTabs.Length, BlueStonez.tab_medium);
    if (GUI.changed)
    {
      GUI.changed = false;
      SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
    }
    GUI.BeginGroup(new Rect(16f, 55f, this._rect.width - 32f, (float) ((double) this._rect.height - 56.0 - 44.0)), string.Empty, BlueStonez.window_standard_grey38);
    switch (this._selectedOptionsTab)
    {
      case 0:
        this.DoGeneralGroup();
        break;
      case 1:
        this.DoControlsGroup();
        break;
      case 2:
        this.DoAudioGroup();
        break;
      case 3:
        this.DoVideoGroup();
        break;
      case 4:
        this.DoSysInfoGroup();
        break;
    }
    GUI.EndGroup();
    GUI.enabled = !this._showWaterModeMenu;
    if (GUI.Button(new Rect(this._rect.width - 136f, this._rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button))
    {
      ApplicationDataManager.ApplicationOptions.SaveApplicationOptions();
      PanelManager.Instance.ClosePanel(PanelType.Options);
    }
    if (AutoMonoBehaviour<InputManager>.Instance.HasUnassignedKeyMappings)
    {
      GUI.contentColor = Color.red;
      GUI.Label(new Rect(166f, this._rect.height - 40f, (float) ((double) this._rect.width - 136.0 - 166.0), 32f), LocalizedStrings.UnassignedKeyMappingsWarningMsg, BlueStonez.label_interparkmed_11pt);
      GUI.contentColor = Color.white;
    }
    if (this._selectedOptionsTab == 1 && !ApplicationDataManager.IsMobile && GUITools.Button(new Rect(16f, this._rect.height - 40f, 150f, 32f), new GUIContent(LocalizedStrings.ResetDefaults), BlueStonez.button))
      AutoMonoBehaviour<InputManager>.Instance.Reset();
    else if (this._selectedOptionsTab == 3)
      GUI.Label(new Rect(16f, this._rect.height - 40f, 150f, 32f), "FPS: " + (1f / Time.smoothDeltaTime).ToString("F1"), BlueStonez.label_interparkbold_16pt_left);
    GUI.enabled = true;
  }

  private void DrawScreenResChangePanel()
  {
    GUI.depth = 1;
    GUI.Label(new Rect(0.0f, 0.0f, this._rect.width, 56f), LocalizedStrings.ChangingScreenResolution, BlueStonez.tab_strip);
    GUI.BeginGroup(new Rect(16f, 55f, this._rect.width - 32f, (float) ((double) this._rect.height - 56.0 - 54.0)), string.Empty, BlueStonez.window_standard_grey38);
    GUI.Label(new Rect(24f, 18f, 460f, 20f), LocalizedStrings.ChooseNewResolution + this._screenResText[this._newScreenResIndex] + " ?", BlueStonez.label_interparkbold_16pt_left);
    GUI.Label(new Rect(0.0f, 0.0f, this._rect.width - 32f, (float) ((double) this._rect.height - 56.0 - 54.0)), ((int) this._screenResChangeDelay).ToString(), BlueStonez.label_interparkbold_48pt);
    GUI.EndGroup();
    if (GUITools.Button(new Rect((float) ((double) this._rect.width - 136.0 - 140.0), this._rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button))
    {
      ScreenResolutionManager.SetResolution(this._newScreenResIndex, true);
      this._screenResChangeDelay = 0.0f;
      GuiLockController.ReleaseLock(GuiDepth.Popup);
    }
    if (!GUITools.Button(new Rect(this._rect.width - 136f, this._rect.height - 40f, 120f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button))
      return;
    this._screenResChangeDelay = 0.0f;
    GuiLockController.ReleaseLock(GuiDepth.Popup);
    if (!this._isFullscreenBefore)
      return;
    ScreenResolutionManager.IsFullScreen = true;
  }

  private void DoGeneralGroup()
  {
    float height = 100f;
    float width = (double) this._rect.height - 55.0 - 46.0 >= (double) height ? this._rect.width - 50f : this._rect.width - 65f;
    this._scrollControls = GUITools.BeginScrollView(new Rect(1f, 1f, this._rect.width - 33f, (float) ((double) this._rect.height - 55.0 - 46.0)), this._scrollControls, new Rect(0.0f, 0.0f, this._rect.width - 50f, height));
    this.DrawGroupControl(new Rect(8f, 20f, width, 85f), LocalizedStrings.Misc, BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(new Rect(8f, 20f, width, 85f));
    ApplicationDataManager.ApplicationOptions.GameplayAutoPickupEnabled = GUI.Toggle(new Rect(12f, 15f, 200f, 20f), ApplicationDataManager.ApplicationOptions.GameplayAutoPickupEnabled, LocalizedStrings.AutoPickupWeapons, BlueStonez.toggle);
    ApplicationDataManager.ApplicationOptions.GameplayAutoEquipEnabled = GUI.Toggle(new Rect(12f, 35f, 200f, 20f), ApplicationDataManager.ApplicationOptions.GameplayAutoEquipEnabled, LocalizedStrings.AutoEquipWeapons, BlueStonez.toggle);
    GUI.EndGroup();
    GUITools.EndScrollView();
  }

  private void Update()
  {
    if ((double) this._screenResChangeDelay > 0.0)
    {
      this._screenResChangeDelay -= Time.deltaTime;
      if ((double) this._screenResChangeDelay <= 0.0)
        GuiLockController.ReleaseLock(GuiDepth.Popup);
    }
    if (!Input.GetMouseButtonUp(0) || !this.graphicsChanged)
      return;
    this.UpdateApplicationFrameRate();
    this.UpdateMaxQueuedFrames();
    this.UpdateTextureQuality();
    this.UpdateVSyncCount();
    this.UpdateAntiAliasing();
    this.graphicsChanged = false;
  }

  private void SyncGraphicsSettings()
  {
    this._currentQuality = QualitySettings.GetQualityLevel();
    this._targetFrameRate = (float) Application.targetFrameRate;
    this._textureQuality = (float) (5 - QualitySettings.masterTextureLimit);
    this._queuedFrames = (float) QualitySettings.maxQueuedFrames;
    int antiAliasing = QualitySettings.antiAliasing;
    switch (antiAliasing)
    {
      case 2:
        this._antiAliasing = 1;
        break;
      case 4:
        this._antiAliasing = 2;
        break;
      default:
        this._antiAliasing = antiAliasing == 8 ? 3 : 0;
        break;
    }
    this._waterQuality = ApplicationDataManager.ApplicationOptions.VideoWaterMode;
    this._vsync = QualitySettings.vSyncCount;
  }

  public static bool HorizontalScrollbar(
    Rect rect,
    string title,
    ref float value,
    float min,
    float max)
  {
    float num = value;
    GUI.BeginGroup(rect);
    GUI.Label(new Rect(0.0f, 4f, rect.width, rect.height), title, BlueStonez.label_interparkbold_11pt_left);
    value = GUI.HorizontalSlider(new Rect(150f, 10f, rect.width - 200f, 30f), value, min, max, BlueStonez.horizontalSlider, BlueStonez.horizontalSliderThumb);
    GUI.Label(new Rect(rect.width - 40f, 4f, 50f, rect.height), (double) value >= 0.0 ? Mathf.RoundToInt(value).ToString() : LocalizedStrings.Auto, BlueStonez.label_interparkbold_11pt_left);
    GUI.EndGroup();
    return (double) value != (double) num;
  }

  public static bool HorizontalGridbar(Rect rect, string title, ref int value, string[] set)
  {
    int num = value;
    GUI.BeginGroup(rect);
    GUI.Label(new Rect(0.0f, 5f, rect.width, rect.height), title, BlueStonez.label_interparkbold_11pt_left);
    value = UnityGUI.Toolbar(new Rect(150f, 5f, rect.width - 200f, 30f), value, set, set.Length, BlueStonez.tab_medium);
    GUI.EndGroup();
    return value != num;
  }

  private void DoVideoGroup()
  {
    GUI.skin = BlueStonez.Skin;
    Rect position = new Rect(1f, 1f, this._rect.width - 33f, (float) ((double) this._rect.height - 55.0 - 47.0));
    Rect contentRect = new Rect(0.0f, 0.0f, (float) this._desiredWidth, (float) ((double) this._rect.height + 200.0 - 55.0 - 46.0 - 20.0));
    int num1 = 10;
    int num2 = 150;
    int height = this._screenResText.Length * 16 + 16;
    float width = (float) ((double) position.width - 8.0 - 8.0 - 20.0);
    if (!Application.isWebPlayer || this.showResolutions)
      contentRect.height += (float) (this._screenResText.Length * 16);
    this._scrollVideo = GUITools.BeginScrollView(position, this._scrollVideo, contentRect);
    GUI.enabled = true;
    int qualityLevel = UnityGUI.Toolbar(new Rect(0.0f, 5f, position.width - 10f, 22f), this._currentQuality, this.qualitySet, this.qualitySet.Length, BlueStonez.tab_medium);
    if (qualityLevel != this._currentQuality)
    {
      this.SetCurrentQuality(qualityLevel);
      SfxManager.Play2dAudioClip(GameAudio.ButtonClick);
    }
    if (OptionsPanelGUI.HorizontalScrollbar(new Rect(8f, 30f, width, 30f), LocalizedStrings.TargetFramerate, ref this._targetFrameRate, -1f, 200f))
    {
      this._vsync = 0;
      this.graphicsChanged = true;
    }
    if (OptionsPanelGUI.HorizontalScrollbar(new Rect(8f, 60f, width, 30f), LocalizedStrings.MaxQueuedFrames, ref this._queuedFrames, 0.0f, 10f))
      this.graphicsChanged = true;
    GUI.Label(new Rect(8f, 90f, width, 30f), new GUIContent(string.Empty, LocalizedStrings.SettingsTakeEffectAfterReloading));
    if (OptionsPanelGUI.HorizontalScrollbar(new Rect(8f, 90f, width, 30f), LocalizedStrings.TextureQuality, ref this._textureQuality, 0.0f, 5f))
    {
      this.graphicsChanged = true;
      this.SetCurrentQuality(this.qualitySet.Length - 1);
    }
    if (OptionsPanelGUI.HorizontalGridbar(new Rect(8f, 120f, width, 30f), LocalizedStrings.VSync, ref this._vsync, this.vsyncSet))
    {
      this._targetFrameRate = -1f;
      this.graphicsChanged = true;
      this.SetCurrentQuality(this.qualitySet.Length - 1);
    }
    if (OptionsPanelGUI.HorizontalGridbar(new Rect(8f, 150f, width, 30f), LocalizedStrings.AntiAliasing, ref this._antiAliasing, this.antiAliasingSet))
    {
      this.graphicsChanged = true;
      this.SetCurrentQuality(this.qualitySet.Length - 1);
    }
    ApplicationDataManager.ApplicationOptions.VideoBloomHitEffect = GUI.Toggle(new Rect(8f, 190f, width, 30f), ApplicationDataManager.ApplicationOptions.VideoBloomHitEffect, LocalizedStrings.HitEffect, BlueStonez.toggle);
    int top = 240;
    if (!Application.isWebPlayer || this.showResolutions)
    {
      this.DrawGroupControl(new Rect(8f, (float) top, width, (float) height), LocalizedStrings.ScreenResolution, BlueStonez.label_group_interparkbold_18pt);
      GUI.BeginGroup(new Rect(8f, (float) top, width, (float) height));
      GUI.changed = false;
      int num3 = GUI.SelectionGrid(new Rect(10f, 10f, (float) (num1 + num2 * 2), (float) height), ScreenResolutionManager.CurrentResolutionIndex, this._screenResText, 1, BlueStonez.radiobutton);
      if (num3 != ScreenResolutionManager.CurrentResolutionIndex)
      {
        if (this.INSTANT_SCREEN_RES_CHANGE)
          ScreenResolutionManager.SetResolution(num3, Screen.fullScreen);
        else
          this.ShowScreenResChangeConfirmation(ScreenResolutionManager.CurrentResolutionIndex, num3);
      }
      GUI.EndGroup();
    }
    GUITools.EndScrollView();
  }

  private void DoAudioGroup()
  {
    float height = 130f;
    float width = (double) this._rect.height - 55.0 - 46.0 >= (double) height ? this._rect.width - 50f : this._rect.width - 65f;
    this._scrollControls = GUITools.BeginScrollView(new Rect(1f, 1f, this._rect.width - 33f, (float) ((double) this._rect.height - 55.0 - 46.0)), this._scrollControls, new Rect(0.0f, 0.0f, this._rect.width - 50f, height));
    this.DrawGroupControl(new Rect(8f, 20f, width, 130f), LocalizedStrings.Volume, BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(new Rect(8f, 20f, width, 130f));
    ApplicationDataManager.ApplicationOptions.AudioEnabled = !GUI.Toggle(new Rect(15f, 105f, 100f, 30f), !ApplicationDataManager.ApplicationOptions.AudioEnabled, LocalizedStrings.Mute, BlueStonez.toggle);
    if (GUI.changed)
    {
      GUI.changed = false;
      AutoMonoBehaviour<SfxManager>.Instance.EnableAudio(ApplicationDataManager.ApplicationOptions.AudioEnabled);
    }
    GUITools.PushGUIState();
    GUI.enabled = ApplicationDataManager.ApplicationOptions.AudioEnabled;
    GUI.Label(new Rect(15f, 10f, 100f, 30f), LocalizedStrings.MasterVolume, BlueStonez.label_interparkbold_11pt_left);
    ApplicationDataManager.ApplicationOptions.AudioMasterVolume = GUI.HorizontalSlider(new Rect(115f, 17f, 200f, 30f), Mathf.Clamp01(ApplicationDataManager.ApplicationOptions.AudioMasterVolume), 0.0f, 1f, BlueStonez.horizontalSlider, BlueStonez.horizontalSliderThumb);
    if (GUI.changed)
    {
      GUI.changed = false;
      AutoMonoBehaviour<SfxManager>.Instance.UpdateMasterVolume();
    }
    GUI.Label(new Rect(320f, 10f, 100f, 30f), (ApplicationDataManager.ApplicationOptions.AudioMasterVolume * 100f).ToString("f0") + " %", BlueStonez.label_interparkbold_11pt_left);
    GUI.Label(new Rect(15f, 40f, 100f, 30f), LocalizedStrings.MusicVolume, BlueStonez.label_interparkbold_11pt_left);
    ApplicationDataManager.ApplicationOptions.AudioMusicVolume = GUI.HorizontalSlider(new Rect(115f, 47f, 200f, 30f), Mathf.Clamp01(ApplicationDataManager.ApplicationOptions.AudioMusicVolume), 0.0f, 1f, BlueStonez.horizontalSlider, BlueStonez.horizontalSliderThumb);
    if (GUI.changed)
    {
      GUI.changed = false;
      AutoMonoBehaviour<SfxManager>.Instance.UpdateMusicVolume();
    }
    GUI.Label(new Rect(320f, 40f, 100f, 30f), (ApplicationDataManager.ApplicationOptions.AudioMusicVolume * 100f).ToString("f0") + " %", BlueStonez.label_interparkbold_11pt_left);
    GUI.Label(new Rect(15f, 70f, 100f, 30f), LocalizedStrings.EffectsVolume, BlueStonez.label_interparkbold_11pt_left);
    ApplicationDataManager.ApplicationOptions.AudioEffectsVolume = GUI.HorizontalSlider(new Rect(115f, 77f, 200f, 30f), Mathf.Clamp01(ApplicationDataManager.ApplicationOptions.AudioEffectsVolume), 0.0f, 1f, BlueStonez.horizontalSlider, BlueStonez.horizontalSliderThumb);
    if (GUI.changed)
    {
      GUI.changed = false;
      AutoMonoBehaviour<SfxManager>.Instance.UpdateEffectsVolume();
    }
    GUI.Label(new Rect(320f, 70f, 100f, 30f), (ApplicationDataManager.ApplicationOptions.AudioEffectsVolume * 100f).ToString("f0") + " %", BlueStonez.label_interparkbold_11pt_left);
    GUITools.PopGUIState();
    GUI.EndGroup();
    GUITools.EndScrollView();
  }

  private void DoControlsGroup()
  {
    GUITools.PushGUIState();
    GUI.enabled = this._targetMap == null;
    GUI.skin = BlueStonez.Skin;
    this._scrollControls = GUITools.BeginScrollView(new Rect(1f, 3f, this._rect.width - 33f, (float) ((double) this._rect.height - 55.0 - 50.0)), this._scrollControls, new Rect(0.0f, 0.0f, this._rect.width - 50f, (float) (210 + this._keyCount * 21)));
    this.DrawGroupControl(new Rect(8f, 20f, this._rect.width - 65f, 65f), LocalizedStrings.Mouse, BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(new Rect(8f, 20f, this._rect.width - 65f, 65f));
    GUI.Label(new Rect(15f, 10f, 130f, 30f), LocalizedStrings.MouseSensitivity, BlueStonez.label_interparkbold_11pt_left);
    float num = GUI.HorizontalSlider(new Rect(155f, 17f, 200f, 30f), ApplicationDataManager.ApplicationOptions.InputXMouseSensitivity, 1f, 10f, BlueStonez.horizontalSlider, BlueStonez.horizontalSliderThumb);
    GUI.Label(new Rect(370f, 10f, 100f, 30f), ApplicationDataManager.ApplicationOptions.InputXMouseSensitivity.ToString("N1"), BlueStonez.label_interparkbold_11pt_left);
    if ((double) num != (double) ApplicationDataManager.ApplicationOptions.InputXMouseSensitivity)
      ApplicationDataManager.ApplicationOptions.InputXMouseSensitivity = num;
    bool flag1 = GUI.Toggle(new Rect(15f, 38f, 200f, 30f), ApplicationDataManager.ApplicationOptions.InputInvertMouse, LocalizedStrings.InvertMouseButtons, BlueStonez.toggle);
    if (flag1 != ApplicationDataManager.ApplicationOptions.InputInvertMouse)
      ApplicationDataManager.ApplicationOptions.InputInvertMouse = flag1;
    GUI.EndGroup();
    int top = 105;
    if (Input.GetJoystickNames().Length > 0)
    {
      this.DrawGroupControl(new Rect(8f, 105f, this._rect.width - 65f, 50f), LocalizedStrings.Gamepad, BlueStonez.label_group_interparkbold_18pt);
      GUI.BeginGroup(new Rect(8f, 105f, this._rect.width - 65f, 50f));
      bool flag2 = GUI.Toggle(new Rect(15f, 15f, 400f, 30f), AutoMonoBehaviour<InputManager>.Instance.IsGamepadEnabled, Input.GetJoystickNames()[0], BlueStonez.toggle);
      if (flag2 != AutoMonoBehaviour<InputManager>.Instance.IsGamepadEnabled)
        AutoMonoBehaviour<InputManager>.Instance.IsGamepadEnabled = flag2;
      GUI.EndGroup();
      top += 70;
    }
    else if (AutoMonoBehaviour<InputManager>.Instance.IsGamepadEnabled)
      AutoMonoBehaviour<InputManager>.Instance.IsGamepadEnabled = false;
    this.DrawGroupControl(new Rect(8f, (float) top, this._rect.width - 65f, (float) (this._keyCount * 21 + 20)), LocalizedStrings.Keyboard, BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(new Rect(8f, (float) top, this._rect.width - 65f, (float) (this._keyCount * 21 + 20)));
    this.DoInputControlMapping(new Rect(5f, 5f, this._rect.width - 60f, (float) (this._keyCount * 21 + 20)));
    GUI.EndGroup();
    GUITools.EndScrollView();
    GUITools.PopGUIState();
  }

  private void UseMultiTouch()
  {
    ApplicationDataManager.ApplicationOptions.UseMultiTouch = true;
    PanelManager.Instance.OpenPanel(PanelType.Options);
  }

  private void DoSysInfoGroup()
  {
    GUI.skin = BlueStonez.Skin;
    float height = 1100f;
    float width = Mathf.Max(this._rect.width, BlueStonez.label_interparkbold_11pt_left.CalcSize(new GUIContent("Absolute URL : " + ApplicationDataManager.LocalSystemInfo.AbsoluteURL)).x);
    this._scrollControls = GUITools.BeginScrollView(new Rect(1f, 1f, this._rect.width - 33f, (float) ((double) this._rect.height - 55.0 - 46.0)), this._scrollControls, new Rect(0.0f, 0.0f, width + 15f, height));
    int top1 = 20;
    Rect rect1 = new Rect(8f, (float) top1, width, 130f);
    this.DrawGroupControl(rect1, "Application", BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(rect1);
    this.DrawContent(new Rect(16f, 20f, 400f, 20f), "Version", "4.3.10");
    this.DrawContent(new Rect(16f, 40f, 400f, 20f), "Revision", ApplicationDataManager.BuildNumber);
    this.DrawContent(new Rect(16f, 60f, 400f, 20f), "Platform", ((Enum) Application.platform).ToString());
    this.DrawContent(new Rect(16f, 80f, 400f, 20f), "Channel", ((Enum) ApplicationDataManager.Channel).ToString());
    this.DrawContent(new Rect(16f, 100f, 400f, 20f), "System Language", ApplicationDataManager.LocalSystemInfo.SystemLanguage);
    GUI.EndGroup();
    int top2 = top1 + 150;
    Rect rect2 = new Rect(8f, (float) top2, width, 170f);
    this.DrawGroupControl(rect2, "Unity", BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(rect2);
    this.DrawContent(new Rect(16f, 20f, 400f, 20f), "Unity Version", ApplicationDataManager.LocalSystemInfo.UnityVersion);
    this.DrawContent(new Rect(16f, 40f, width, 20f), "Src Value", ApplicationDataManager.LocalSystemInfo.SrcValue);
    this.DrawContent(new Rect(16f, 60f, width, 20f), "Absolute URL", ApplicationDataManager.LocalSystemInfo.AbsoluteURL);
    this.DrawContent(new Rect(16f, 80f, width, 20f), "Data Path", ApplicationDataManager.LocalSystemInfo.DataPath);
    this.DrawContent(new Rect(16f, 100f, 400f, 20f), "Background Loading Priority", ApplicationDataManager.LocalSystemInfo.BackgroundLoadingPriority);
    this.DrawContent(new Rect(16f, 120f, 400f, 20f), "Run In Background", ApplicationDataManager.LocalSystemInfo.RunInBackground);
    this.DrawContent(new Rect(16f, 140f, 400f, 20f), "Target Frame Rate", ApplicationDataManager.LocalSystemInfo.TargetFrameRate);
    GUI.EndGroup();
    int top3 = top2 + 190;
    Rect rect3 = new Rect(8f, (float) top3, width, 330f);
    this.DrawGroupControl(rect3, "System", BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(rect3);
    this.DrawContent(new Rect(16f, 20f, 400f, 20f), "Operating System", ApplicationDataManager.LocalSystemInfo.OperatingSystem);
    this.DrawContent(new Rect(16f, 40f, 400f, 20f), "Processor Type", ApplicationDataManager.LocalSystemInfo.ProcessorType);
    this.DrawContent(new Rect(16f, 60f, 400f, 20f), "Processor Count", ApplicationDataManager.LocalSystemInfo.ProcessorCount);
    this.DrawContent(new Rect(16f, 80f, 400f, 20f), "System Memory Size", ApplicationDataManager.LocalSystemInfo.SystemMemorySize);
    this.DrawContent(new Rect(16f, 120f, 400f, 20f), "Graphics Device Name", ApplicationDataManager.LocalSystemInfo.GraphicsDeviceName);
    this.DrawContent(new Rect(16f, 140f, 400f, 20f), "Graphics Device Vendor", ApplicationDataManager.LocalSystemInfo.GraphicsDeviceVendor);
    this.DrawContent(new Rect(16f, 160f, 400f, 20f), "Graphics Device Version", ApplicationDataManager.LocalSystemInfo.GraphicsDeviceVersion);
    this.DrawContent(new Rect(16f, 180f, 400f, 20f), "Graphics Memory Size", ApplicationDataManager.LocalSystemInfo.GraphicsMemorySize);
    this.DrawContent(new Rect(16f, 200f, 400f, 20f), "Graphics Shader Level", ApplicationDataManager.LocalSystemInfo.GraphicsShaderLevel);
    this.DrawContent(new Rect(16f, 220f, 400f, 20f), "Graphics Pixel Fill Rate", ApplicationDataManager.LocalSystemInfo.GraphicsPixelFillRate + " Megapixels/Sec");
    this.DrawContent(new Rect(16f, 240f, 400f, 20f), "Supports Image Effects", ApplicationDataManager.LocalSystemInfo.SupportsImageEffects);
    this.DrawContent(new Rect(16f, 260f, 400f, 20f), "Supports Render Textures", ApplicationDataManager.LocalSystemInfo.SupportsRenderTextures);
    this.DrawContent(new Rect(16f, 280f, 400f, 20f), "Supports Shadows", ApplicationDataManager.LocalSystemInfo.SupportsShadows);
    this.DrawContent(new Rect(16f, 300f, 400f, 20f), "Supports Vertex Programs", ApplicationDataManager.LocalSystemInfo.SupportsVertexPrograms);
    GUI.EndGroup();
    int top4 = top3 + 350;
    Rect rect4 = new Rect(8f, (float) top4, width, 170f);
    this.DrawGroupControl(rect4, "Render Settings", BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(rect4);
    this.DrawContent(new Rect(16f, 20f, 400f, 20f), "Current Resolution", ApplicationDataManager.LocalSystemInfo.CurrentResolution);
    this.DrawContent(new Rect(16f, 40f, 400f, 20f), "Ambient Light", ApplicationDataManager.LocalSystemInfo.AmbientLight);
    this.DrawContent(new Rect(16f, 60f, 400f, 20f), "Flare Strength", ApplicationDataManager.LocalSystemInfo.FlareStrength);
    this.DrawContent(new Rect(16f, 80f, 400f, 20f), "Fog Enabled", ApplicationDataManager.LocalSystemInfo.FogEnabled);
    this.DrawContent(new Rect(16f, 100f, 400f, 20f), "Fog Color", ApplicationDataManager.LocalSystemInfo.FogColor);
    this.DrawContent(new Rect(16f, 120f, 400f, 20f), "Fog Density", ApplicationDataManager.LocalSystemInfo.FogDensity);
    this.DrawContent(new Rect(16f, 140f, 400f, 20f), "Halo Strength", ApplicationDataManager.LocalSystemInfo.HaloStrength);
    GUI.EndGroup();
    Rect rect5 = new Rect(8f, (float) (top4 + 190), width, 190f);
    this.DrawGroupControl(rect5, "Quality Settings", BlueStonez.label_group_interparkbold_18pt);
    GUI.BeginGroup(rect5);
    this.DrawContent(new Rect(16f, 20f, 400f, 20f), "Current Quality Level", ApplicationDataManager.LocalSystemInfo.CurrentQualityLevel);
    this.DrawContent(new Rect(16f, 40f, 400f, 20f), "Anisotropic Filtering", ApplicationDataManager.LocalSystemInfo.AnisotropicFiltering);
    this.DrawContent(new Rect(16f, 60f, 400f, 20f), "Master Texture Limit", ApplicationDataManager.LocalSystemInfo.MasterTextureLimit);
    this.DrawContent(new Rect(16f, 80f, 400f, 20f), "Max Queued Frames", ApplicationDataManager.LocalSystemInfo.MaxQueuedFrames);
    this.DrawContent(new Rect(16f, 100f, 400f, 20f), "Pixel Light Count", ApplicationDataManager.LocalSystemInfo.PixelLightCount);
    this.DrawContent(new Rect(16f, 120f, 400f, 20f), "Shadow Cascades", ApplicationDataManager.LocalSystemInfo.ShadowCascades);
    this.DrawContent(new Rect(16f, 140f, 400f, 20f), "Shadow Distance", ApplicationDataManager.LocalSystemInfo.ShadowDistance);
    this.DrawContent(new Rect(16f, 160f, 400f, 20f), "Soft Vegetation Enabled", ApplicationDataManager.LocalSystemInfo.SoftVegetationEnabled);
    GUI.EndGroup();
    GUITools.EndScrollView();
  }

  private void DoInputControlMapping(Rect rect)
  {
    int num = 0;
    GUI.Label(new Rect(20f, 13f, 150f, 20f), LocalizedStrings.Movement, BlueStonez.label_interparkbold_11pt_left);
    GUI.Label(new Rect(220f, 13f, 150f, 20f), LocalizedStrings.KeyButton, BlueStonez.label_interparkbold_11pt_left);
    foreach (UserInputMap userInputMap in AutoMonoBehaviour<InputManager>.Instance.KeyMapping.Values)
    {
      bool flag = userInputMap == this._targetMap;
      GUI.Label(new Rect(20f, (float) (35 + num * 20), 140f, 20f), userInputMap.Description, BlueStonez.label_interparkmed_10pt_left);
      if (userInputMap.IsConfigurable && GUI.Toggle(new Rect(180f, (float) (35 + num * 20), 20f, 20f), flag, string.Empty, BlueStonez.radiobutton))
      {
        this._targetMap = userInputMap;
        Screen.lockCursor = true;
      }
      if (flag)
      {
        GUI.enabled = true;
        GUI.TextField(new Rect(220f, (float) (35 + num * 20), 100f, 20f), string.Empty);
        GUI.enabled = false;
      }
      else
      {
        GUI.contentColor = userInputMap.Channel == null ? Color.red : Color.white;
        GUI.Label(new Rect(220f, (float) (35 + num * 20), 150f, 20f), userInputMap.Assignment, BlueStonez.label_interparkmed_10pt_left);
        GUI.contentColor = Color.white;
      }
      ++num;
    }
    if (this._targetMap == null || Event.current.type != UnityEngine.EventType.Layout || !AutoMonoBehaviour<InputManager>.Instance.ListenForNewKeyAssignment(this._targetMap))
      return;
    this._targetMap = (UserInputMap) null;
    Screen.lockCursor = false;
    Event.current.Use();
  }

  private void DrawGroupLabel(Rect position, string label, string text) => GUI.Label(position, label + ": " + text, BlueStonez.label_interparkbold_16pt_left);

  private void DrawContent(Rect position, string label, string text) => GUI.Label(position, label + ": " + text, BlueStonez.label_interparkbold_11pt_left);

  private void DrawGroupLabelWithWidth(Rect position, string label, string text)
  {
    string text1 = label + ": " + text;
    int width = Mathf.RoundToInt(BlueStonez.label_interparkbold_16pt.CalcSize(new GUIContent(text1)).x);
    GUI.Label(new Rect(position.x, position.y, (float) width, position.height), text1, BlueStonez.label_interparkbold_16pt_left);
    this._desiredWidth = width <= this._desiredWidth ? this._desiredWidth : width;
  }

  private void DrawGroupControl(Rect rect, string title, GUIStyle style)
  {
    GUI.BeginGroup(rect, string.Empty, BlueStonez.group_grey81);
    GUI.EndGroup();
    GUI.Label(new Rect(rect.x + 18f, rect.y - 8f, this.GetWidth(title, style), 16f), title, style);
  }

  private float GetWidth(string content) => this.GetWidth(content, BlueStonez.label_group_interparkbold_18pt);

  private float GetWidth(string content, GUIStyle style) => style.CalcSize(new GUIContent(content)).x + 10f;

  private void ShowScreenResChangeConfirmation(int oldRes, int newRes)
  {
    this._screenResChangeDelay = 15f;
    this._newScreenResIndex = newRes;
    this._isFullscreenBefore = ScreenResolutionManager.IsFullScreen;
    ScreenResolutionManager.IsFullScreen = false;
  }

  private void SetCurrentQuality(int qualityLevel)
  {
    this._currentQuality = qualityLevel;
    if (this._currentQuality < QualitySettings.names.Length)
    {
      ApplicationDataManager.ApplicationOptions.IsUsingCustom = false;
      GraphicSettings.SetQualityLevel(this._currentQuality);
      this.SyncGraphicsSettings();
    }
    else
      ApplicationDataManager.ApplicationOptions.IsUsingCustom = true;
  }

  private void UpdateApplicationFrameRate()
  {
    this._targetFrameRate = (float) Mathf.RoundToInt(this._targetFrameRate);
    if ((double) this._targetFrameRate >= 0.0)
      this._targetFrameRate = Mathf.Max(this._targetFrameRate, 20f);
    Application.targetFrameRate = (int) this._targetFrameRate;
    ApplicationDataManager.ApplicationOptions.GeneralTargetFrameRate = Application.targetFrameRate;
  }

  private void UpdateMaxQueuedFrames()
  {
    this._queuedFrames = (float) Mathf.RoundToInt(this._queuedFrames);
    QualitySettings.maxQueuedFrames = (int) this._queuedFrames;
    ApplicationDataManager.ApplicationOptions.VideoMaxQueuedFrames = QualitySettings.maxQueuedFrames;
  }

  private void UpdateTextureQuality()
  {
    this._textureQuality = (float) Mathf.RoundToInt(this._textureQuality);
    QualitySettings.masterTextureLimit = 5 - (int) this._textureQuality;
    ApplicationDataManager.ApplicationOptions.VideoTextureQuality = QualitySettings.masterTextureLimit;
  }

  private void UpdateVSyncCount()
  {
    ApplicationDataManager.ApplicationOptions.VideoVSyncCount = this._vsync;
    QualitySettings.vSyncCount = this._vsync;
  }

  private void UpdateAntiAliasing()
  {
    switch (this._antiAliasing)
    {
      case 1:
        QualitySettings.antiAliasing = 2;
        break;
      case 2:
        QualitySettings.antiAliasing = 4;
        break;
      case 3:
        QualitySettings.antiAliasing = 8;
        break;
      default:
        QualitySettings.antiAliasing = 0;
        break;
    }
    ApplicationDataManager.ApplicationOptions.VideoAntiAliasing = QualitySettings.antiAliasing;
  }

  public override void Show()
  {
    base.Show();
    if (ApplicationDataManager.ApplicationOptions.IsUsingCustom)
      this._currentQuality = this.qualitySet.Length - 1;
    else
      this._currentQuality = ApplicationDataManager.ApplicationOptions.VideoQualityLevel;
  }

  private class ScreenRes
  {
    public int Index;
    public string Resolution;

    public ScreenRes(int index, string res)
    {
      this.Index = index;
      this.Resolution = res;
    }
  }
}
