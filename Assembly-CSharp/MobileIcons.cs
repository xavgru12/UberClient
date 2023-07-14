// Decompiled with JetBrains decompiler
// Type: MobileIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class MobileIcons
{
  static MobileIcons()
  {
    Texture2DConfigurator texture2Dconfigurator1 = (Texture2DConfigurator) null;
    foreach (Texture2DConfigurator texture2Dconfigurator2 in UnityEngine.Object.FindSceneObjectsOfType(typeof (Texture2DConfigurator)))
    {
      if (texture2Dconfigurator2.name == nameof (MobileIcons))
      {
        texture2Dconfigurator1 = texture2Dconfigurator2;
        break;
      }
    }
    MobileIcons.TouchFireButton = !((UnityEngine.Object) texture2Dconfigurator1 == (UnityEngine.Object) null) ? texture2Dconfigurator1.Assets[0] : throw new Exception("Missing instance of the prefab with name: MobileIcons!");
    MobileIcons.TouchJumpButton = texture2Dconfigurator1.Assets[1];
    MobileIcons.TouchCrouchButton = texture2Dconfigurator1.Assets[2];
    MobileIcons.TouchSecondFireButton = texture2Dconfigurator1.Assets[3];
    MobileIcons.TouchKeyboardDpad = texture2Dconfigurator1.Assets[4];
    MobileIcons.TouchZoomScrollbar = texture2Dconfigurator1.Assets[5];
    MobileIcons.TouchChatButton = texture2Dconfigurator1.Assets[6];
    MobileIcons.TouchMenuButton = texture2Dconfigurator1.Assets[7];
    MobileIcons.TouchScoreboardButton = texture2Dconfigurator1.Assets[8];
    MobileIcons.TouchArrowLeft = texture2Dconfigurator1.Assets[9];
    MobileIcons.TouchArrowRight = texture2Dconfigurator1.Assets[10];
    MobileIcons.TouchMoveInner = texture2Dconfigurator1.Assets[11];
    MobileIcons.TouchMoveOuter = texture2Dconfigurator1.Assets[12];
    MobileIcons.TouchWeaponMelee = texture2Dconfigurator1.Assets[13];
    MobileIcons.TouchWeaponHandgun = texture2Dconfigurator1.Assets[14];
    MobileIcons.TouchWeaponMachinegun = texture2Dconfigurator1.Assets[15];
    MobileIcons.TouchWeaponShotgun = texture2Dconfigurator1.Assets[16];
    MobileIcons.TouchWeaponSniperrifle = texture2Dconfigurator1.Assets[17];
    MobileIcons.TouchWeaponCannon = texture2Dconfigurator1.Assets[18];
    MobileIcons.TouchWeaponSplattergun = texture2Dconfigurator1.Assets[19];
    MobileIcons.TouchWeaponLauncher = texture2Dconfigurator1.Assets[20];
  }

  public static Texture2D TouchFireButton { get; private set; }

  public static Texture2D TouchJumpButton { get; private set; }

  public static Texture2D TouchCrouchButton { get; private set; }

  public static Texture2D TouchSecondFireButton { get; private set; }

  public static Texture2D TouchKeyboardDpad { get; private set; }

  public static Texture2D TouchZoomScrollbar { get; private set; }

  public static Texture2D TouchChatButton { get; private set; }

  public static Texture2D TouchMenuButton { get; private set; }

  public static Texture2D TouchScoreboardButton { get; private set; }

  public static Texture2D TouchArrowLeft { get; private set; }

  public static Texture2D TouchArrowRight { get; private set; }

  public static Texture2D TouchMoveInner { get; private set; }

  public static Texture2D TouchMoveOuter { get; private set; }

  public static Texture2D TouchWeaponMelee { get; private set; }

  public static Texture2D TouchWeaponHandgun { get; private set; }

  public static Texture2D TouchWeaponMachinegun { get; private set; }

  public static Texture2D TouchWeaponShotgun { get; private set; }

  public static Texture2D TouchWeaponSniperrifle { get; private set; }

  public static Texture2D TouchWeaponCannon { get; private set; }

  public static Texture2D TouchWeaponSplattergun { get; private set; }

  public static Texture2D TouchWeaponLauncher { get; private set; }
}
