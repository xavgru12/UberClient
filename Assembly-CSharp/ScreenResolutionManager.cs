// Decompiled with JetBrains decompiler
// Type: ScreenResolutionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenResolutionManager
{
  private static List<Resolution> resolutions = new List<Resolution>();

  static ScreenResolutionManager()
  {
    foreach (Resolution resolution in Screen.resolutions)
    {
      if (resolution.width > 800)
        ScreenResolutionManager.resolutions.Add(resolution);
    }
    if (ScreenResolutionManager.resolutions.Count != 0)
      return;
    ScreenResolutionManager.resolutions.Add(Screen.currentResolution);
  }

  private static bool IsHighestResolution => ScreenResolutionManager.CurrentResolutionIndex == ScreenResolutionManager.resolutions.Count - 1;

  public static List<Resolution> Resolutions => ScreenResolutionManager.resolutions;

  public static int InitialResolutionIndex => ScreenResolutionManager.resolutions.Count - 1;

  public static int CurrentResolutionIndex => ScreenResolutionManager.resolutions.FindIndex((Predicate<Resolution>) (r => r.width == Screen.width && r.height == Screen.height));

  public static bool IsFullScreen
  {
    get => Screen.fullScreen;
    set
    {
      if (!Application.isWebPlayer && !value && ScreenResolutionManager.IsHighestResolution)
        ScreenResolutionManager.SetTwoMinusMaxResolution();
      else
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, value);
      ApplicationDataManager.ApplicationOptions.IsFullscreen = value;
      ApplicationDataManager.ApplicationOptions.SaveApplicationOptions();
    }
  }

  public static void SetResolution(int index, bool fullscreen)
  {
    int max = ScreenResolutionManager.resolutions.Count - 1;
    int index1 = Mathf.Clamp(index, 0, max);
    if (!Application.isWebPlayer && index1 == max && !fullscreen)
      fullscreen = true;
    if (index1 < 0 || index1 >= ScreenResolutionManager.resolutions.Count)
      return;
    Screen.SetResolution(ScreenResolutionManager.resolutions[index1].width, ScreenResolutionManager.resolutions[index1].height, fullscreen);
    ApplicationDataManager.ApplicationOptions.ScreenResolution = index1;
  }

  public static void SetTwoMinusMaxResolution()
  {
    if (Application.isWebPlayer)
      Debug.LogError((object) "SetOneMinusMaxResolution() should only be called from the desktop client");
    else if (ScreenResolutionManager.resolutions.Count > 2)
    {
      Vector2 vector2 = new Vector2((float) ScreenResolutionManager.resolutions[ScreenResolutionManager.resolutions.Count - 3].width, (float) ScreenResolutionManager.resolutions[ScreenResolutionManager.resolutions.Count - 3].height);
      Screen.SetResolution((int) vector2.x, (int) vector2.y, false);
    }
    else if (ScreenResolutionManager.resolutions.Count == 2)
    {
      Vector2 vector2 = new Vector2((float) ScreenResolutionManager.resolutions[1].width, (float) ScreenResolutionManager.resolutions[1].height);
      Screen.SetResolution((int) vector2.x, (int) vector2.y, false);
    }
    else if (ScreenResolutionManager.resolutions.Count == 1)
    {
      Vector2 vector2 = new Vector2((float) ScreenResolutionManager.resolutions[0].width, (float) ScreenResolutionManager.resolutions[0].height);
      Screen.SetResolution((int) vector2.x, (int) vector2.y, false);
    }
    else
      Debug.LogError((object) "ScreenResolutionManager: Screen.resolutions does not contain any supported resolutions.");
  }

  public static void SetFullScreenMaxResolution()
  {
    if (ScreenResolutionManager.resolutions.Count == 0)
    {
      Debug.LogError((object) "SetFullScreenMaxResolution: No suitable resolution available in the Resolutions array.");
    }
    else
    {
      int index = ScreenResolutionManager.resolutions.Count - 1;
      Vector2 vector2 = new Vector2((float) ScreenResolutionManager.resolutions[index].width, (float) ScreenResolutionManager.resolutions[index].height);
      if (Screen.fullScreen)
        return;
      Screen.SetResolution((int) vector2.x, (int) vector2.y, true);
      ApplicationDataManager.ApplicationOptions.ScreenResolution = index;
    }
  }

  public static void DecreaseResolution() => ScreenResolutionManager.SetResolution(ScreenResolutionManager.CurrentResolutionIndex - 1, Screen.fullScreen);

  public static void IncreaseResolution() => ScreenResolutionManager.SetResolution(ScreenResolutionManager.CurrentResolutionIndex + 1, Screen.fullScreen);
}
