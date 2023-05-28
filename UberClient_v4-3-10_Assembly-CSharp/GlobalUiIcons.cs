// Decompiled with JetBrains decompiler
// Type: GlobalUiIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class GlobalUiIcons
{
  static GlobalUiIcons()
  {
    Texture2DConfigurator component;
    try
    {
      component = GameObject.Find(nameof (GlobalUiIcons)).GetComponent<Texture2DConfigurator>();
    }
    catch
    {
      Debug.LogError((object) "Missing instance of the prefab with name: GlobalUiIcons!");
      return;
    }
    GlobalUiIcons.QuadpanelButtonFullscreen = component.Assets[0];
    GlobalUiIcons.QuadpanelButtonNormalize = component.Assets[1];
    GlobalUiIcons.QuadpanelButtonModerate = component.Assets[2];
    GlobalUiIcons.QuadpanelButtonSoundoff = component.Assets[3];
    GlobalUiIcons.QuadpanelButtonSoundon = component.Assets[4];
    GlobalUiIcons.QuadpanelButtonReportplayer = component.Assets[5];
    GlobalUiIcons.QuadpanelButtonOptions = component.Assets[6];
    GlobalUiIcons.QuadpanelButtonHelp = component.Assets[7];
    GlobalUiIcons.NewInboxMessage = component.Assets[8];
    GlobalUiIcons.QuadpanelButtonLogout = component.Assets[9];
  }

  public static Texture2D QuadpanelButtonFullscreen { get; private set; }

  public static Texture2D QuadpanelButtonNormalize { get; private set; }

  public static Texture2D QuadpanelButtonModerate { get; private set; }

  public static Texture2D QuadpanelButtonSoundoff { get; private set; }

  public static Texture2D QuadpanelButtonSoundon { get; private set; }

  public static Texture2D QuadpanelButtonReportplayer { get; private set; }

  public static Texture2D QuadpanelButtonOptions { get; private set; }

  public static Texture2D QuadpanelButtonHelp { get; private set; }

  public static Texture2D NewInboxMessage { get; private set; }

  public static Texture2D QuadpanelButtonLogout { get; private set; }
}
