// Decompiled with JetBrains decompiler
// Type: UberstrikeIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class UberstrikeIcons
{
  static UberstrikeIcons()
  {
    Texture2DConfigurator component = GameObject.Find(nameof (UberstrikeIcons)).GetComponent<Texture2DConfigurator>();
    UberstrikeIcons.Waiting = !((UnityEngine.Object) component == (UnityEngine.Object) null) ? component.Assets[0] : throw new Exception("Missing instance of the prefab with name: UberstrikeIcons!");
    UberstrikeIcons.IconXP20x20 = component.Assets[1];
    UberstrikeIcons.BlueLevel32 = component.Assets[2];
    UberstrikeIcons.LevelUpPopup = component.Assets[3];
    UberstrikeIcons.FacebookCreditsIcon = component.Assets[4];
    UberstrikeIcons.LevelMastered = component.Assets[5];
  }

  public static Texture2D Waiting { get; private set; }

  public static Texture2D IconXP20x20 { get; private set; }

  public static Texture2D BlueLevel32 { get; private set; }

  public static Texture2D LevelUpPopup { get; private set; }

  public static Texture2D FacebookCreditsIcon { get; private set; }

  public static Texture2D LevelMastered { get; private set; }
}
