// Decompiled with JetBrains decompiler
// Type: PopupSkin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class PopupSkin
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
  public static GUIStyle title = GUIStyle.none;
  public static GUIStyle button_green = GUIStyle.none;
  public static GUIStyle button_red = GUIStyle.none;
  public static GUIStyle label_loading = GUIStyle.none;

  public static void Initialize(GUISkin skin)
  {
    PopupSkin.Skin = skin;
    PopupSkin.box = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("box"));
    PopupSkin.label = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("label"));
    PopupSkin.textField = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("textField"));
    PopupSkin.textArea = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("textArea"));
    PopupSkin.button = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("button"));
    PopupSkin.toggle = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("toggle"));
    PopupSkin.window = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("window"));
    PopupSkin.horizontalSlider = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("horizontalSlider"));
    PopupSkin.horizontalSliderThumb = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("horizontalSliderThumb"));
    PopupSkin.verticalSlider = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("verticalSlider"));
    PopupSkin.verticalSliderThumb = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("verticalSliderThumb"));
    PopupSkin.horizontalScrollbar = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("horizontalScrollbar"));
    PopupSkin.horizontalScrollbarThumb = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("horizontalScrollbarThumb"));
    PopupSkin.horizontalScrollbarLeftButton = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("horizontalScrollbarLeftButton"));
    PopupSkin.horizontalScrollbarRightButton = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("horizontalScrollbarRightButton"));
    PopupSkin.verticalScrollbar = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("verticalScrollbar"));
    PopupSkin.verticalScrollbarThumb = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("verticalScrollbarThumb"));
    PopupSkin.verticalScrollbarUpButton = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("verticalScrollbarUpButton"));
    PopupSkin.verticalScrollbarDownButton = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("verticalScrollbarDownButton"));
    PopupSkin.scrollView = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("scrollView"));
    PopupSkin.title = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("title"));
    PopupSkin.button_green = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("button_green"));
    PopupSkin.button_red = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("button_red"));
    PopupSkin.label_loading = LocalizationHelper.GetLocalizedStyle(PopupSkin.Skin.GetStyle("label_loading"));
  }

  public static GUISkin Skin { get; private set; }
}
