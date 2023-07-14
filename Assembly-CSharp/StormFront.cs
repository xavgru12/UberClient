// Decompiled with JetBrains decompiler
// Type: StormFront
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class StormFront
{
  public static GUIStyle box = GUIStyle.none;
  public static GUIStyle label = GUIStyle.none;
  public static GUIStyle textField = GUIStyle.none;
  public static GUIStyle textArea = GUIStyle.none;
  public static GUIStyle button = GUIStyle.none;
  public static GUIStyle toggle = GUIStyle.none;
  public static GUIStyle window = GUIStyle.none;
  public static GUIStyle horizontalSlider = GUIStyle.none;
  public static GUIStyle horizontalSliderThumb = GUIStyle.none;
  public static GUIStyle verticalSlider = GUIStyle.none;
  public static GUIStyle verticalSliderThumb = GUIStyle.none;
  public static GUIStyle horizontalScrollbar = GUIStyle.none;
  public static GUIStyle horizontalScrollbarThumb = GUIStyle.none;
  public static GUIStyle horizontalScrollbarLeftButton = GUIStyle.none;
  public static GUIStyle horizontalScrollbarRightButton = GUIStyle.none;
  public static GUIStyle verticalScrollbar = GUIStyle.none;
  public static GUIStyle verticalScrollbarThumb = GUIStyle.none;
  public static GUIStyle verticalScrollbarUpButton = GUIStyle.none;
  public static GUIStyle verticalScrollbarDownButton = GUIStyle.none;
  public static GUIStyle scrollView = GUIStyle.none;
  public static GUIStyle BlueBox = GUIStyle.none;
  public static GUIStyle RedBox = GUIStyle.none;
  public static GUIStyle BluePanelBox = GUIStyle.none;
  public static GUIStyle GrayPanelBox = GUIStyle.none;
  public static GUIStyle GrayPanelBlankBox = GUIStyle.none;
  public static GUIStyle RedPanelBox = GUIStyle.none;
  public static GUIStyle ButtonRed = GUIStyle.none;
  public static GUIStyle ButtonBlue = GUIStyle.none;
  public static GUIStyle ButtonGray = GUIStyle.none;
  public static GUIStyle ButtonJoinBlue = GUIStyle.none;
  public static GUIStyle ButtonJoinRed = GUIStyle.none;
  public static GUIStyle ButtonJoinGray = GUIStyle.none;
  public static GUIStyle ButtonSpectator = GUIStyle.none;
  public static GUIStyle ButtonLoadout = GUIStyle.none;
  public static GUIStyle ButtonLoadoutRed = GUIStyle.none;
  public static GUIStyle InGameChatBlue = GUIStyle.none;
  public static GUIStyle InGameChatRed = GUIStyle.none;
  public static GUIStyle DotBlue = GUIStyle.none;
  public static GUIStyle DotRed = GUIStyle.none;
  public static GUIStyle DotGray = GUIStyle.none;
  public static GUIStyle ButtonCam = GUIStyle.none;
  public static GUIStyle Interpark32Center = GUIStyle.none;
  public static GUIStyle Interpark16Left = GUIStyle.none;
  public static GUIStyle ProgressBackground = GUIStyle.none;
  public static GUIStyle ProgressForeground = GUIStyle.none;
  public static GUIStyle ProgressThumb = GUIStyle.none;
  public static GUIStyle MenuTile = GUIStyle.none;

  public static void Initialize(GUISkin skin)
  {
    StormFront.Skin = skin;
    StormFront.box = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("box"));
    StormFront.label = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("label"));
    StormFront.textField = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("textField"));
    StormFront.textArea = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("textArea"));
    StormFront.button = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("button"));
    StormFront.toggle = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("toggle"));
    StormFront.window = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("window"));
    StormFront.horizontalSlider = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("horizontalSlider"));
    StormFront.horizontalSliderThumb = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("horizontalSliderThumb"));
    StormFront.verticalSlider = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("verticalSlider"));
    StormFront.verticalSliderThumb = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("verticalSliderThumb"));
    StormFront.horizontalScrollbar = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("horizontalScrollbar"));
    StormFront.horizontalScrollbarThumb = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("horizontalScrollbarThumb"));
    StormFront.horizontalScrollbarLeftButton = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("horizontalScrollbarLeftButton"));
    StormFront.horizontalScrollbarRightButton = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("horizontalScrollbarRightButton"));
    StormFront.verticalScrollbar = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("verticalScrollbar"));
    StormFront.verticalScrollbarThumb = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("verticalScrollbarThumb"));
    StormFront.verticalScrollbarUpButton = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("verticalScrollbarUpButton"));
    StormFront.verticalScrollbarDownButton = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("verticalScrollbarDownButton"));
    StormFront.scrollView = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("scrollView"));
    StormFront.BlueBox = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("BlueBox"));
    StormFront.RedBox = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("RedBox"));
    StormFront.BluePanelBox = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("BluePanelBox"));
    StormFront.GrayPanelBox = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("GrayPanelBox"));
    StormFront.GrayPanelBlankBox = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("GrayPanelBlankBox"));
    StormFront.RedPanelBox = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("RedPanelBox"));
    StormFront.ButtonRed = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ButtonRed"));
    StormFront.ButtonBlue = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ButtonBlue"));
    StormFront.ButtonGray = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ButtonGray"));
    StormFront.ButtonJoinBlue = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ButtonJoinBlue"));
    StormFront.ButtonJoinRed = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ButtonJoinRed"));
    StormFront.ButtonJoinGray = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ButtonJoinGray"));
    StormFront.ButtonSpectator = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ButtonSpectator"));
    StormFront.ButtonLoadout = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ButtonLoadout"));
    StormFront.ButtonLoadoutRed = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ButtonLoadoutRed"));
    StormFront.InGameChatBlue = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("InGameChatBlue"));
    StormFront.InGameChatRed = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("InGameChatRed"));
    StormFront.DotBlue = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("DotBlue"));
    StormFront.DotRed = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("DotRed"));
    StormFront.DotGray = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("DotGray"));
    StormFront.ButtonCam = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ButtonCam"));
    StormFront.Interpark32Center = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("Interpark32Center"));
    StormFront.Interpark16Left = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("Interpark16Left"));
    StormFront.ProgressBackground = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ProgressBackground"));
    StormFront.ProgressForeground = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ProgressForeground"));
    StormFront.ProgressThumb = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("ProgressThumb"));
    StormFront.MenuTile = LocalizationHelper.GetLocalizedStyle(StormFront.Skin.GetStyle("MenuTile"));
  }

  public static GUISkin Skin { get; private set; }
}
