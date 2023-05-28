// Decompiled with JetBrains decompiler
// Type: WeaponRange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class WeaponRange
{
  static WeaponRange()
  {
    Texture2DConfigurator texture2Dconfigurator1 = (Texture2DConfigurator) null;
    foreach (Texture2DConfigurator texture2Dconfigurator2 in UnityEngine.Object.FindSceneObjectsOfType(typeof (Texture2DConfigurator)))
    {
      if (texture2Dconfigurator2.name == nameof (WeaponRange))
      {
        texture2Dconfigurator1 = texture2Dconfigurator2;
        break;
      }
    }
    WeaponRange.CombatRangeBackground = !((UnityEngine.Object) texture2Dconfigurator1 == (UnityEngine.Object) null) ? texture2Dconfigurator1.Assets[0] : throw new Exception("Missing instance of the prefab with name: WeaponRange!");
    WeaponRange.CombatRangeClose = texture2Dconfigurator1.Assets[1];
    WeaponRange.CombatRangeMedium = texture2Dconfigurator1.Assets[2];
    WeaponRange.CombatRangeFar = texture2Dconfigurator1.Assets[3];
    WeaponRange.CombatRangeMiniClose = texture2Dconfigurator1.Assets[4];
    WeaponRange.CombatRangeMiniCloseMed = texture2Dconfigurator1.Assets[5];
    WeaponRange.CombatRangeMiniFar = texture2Dconfigurator1.Assets[6];
    WeaponRange.CombatRangeMiniMed = texture2Dconfigurator1.Assets[7];
    WeaponRange.CombatRangeMiniMedFar = texture2Dconfigurator1.Assets[8];
    WeaponRange.IconRange02 = texture2Dconfigurator1.Assets[9];
    WeaponRange.IconRange03 = texture2Dconfigurator1.Assets[10];
    WeaponRange.IconRange04 = texture2Dconfigurator1.Assets[11];
    WeaponRange.IconRange05 = texture2Dconfigurator1.Assets[12];
    WeaponRange.IconRange06 = texture2Dconfigurator1.Assets[13];
    WeaponRange.IconRange07 = texture2Dconfigurator1.Assets[14];
    WeaponRange.IconRange08 = texture2Dconfigurator1.Assets[15];
    WeaponRange.IconRange09 = texture2Dconfigurator1.Assets[16];
    WeaponRange.IconRange10 = texture2Dconfigurator1.Assets[17];
    WeaponRange.IconRange11 = texture2Dconfigurator1.Assets[18];
    WeaponRange.IconRange12 = texture2Dconfigurator1.Assets[19];
    WeaponRange.IconRange13 = texture2Dconfigurator1.Assets[20];
    WeaponRange.ArmorIndicatorBackground = texture2Dconfigurator1.Assets[21];
    WeaponRange.ArmorIndicatorForeground = texture2Dconfigurator1.Assets[22];
  }

  public static Texture2D CombatRangeBackground { get; private set; }

  public static Texture2D CombatRangeClose { get; private set; }

  public static Texture2D CombatRangeMedium { get; private set; }

  public static Texture2D CombatRangeFar { get; private set; }

  public static Texture2D CombatRangeMiniClose { get; private set; }

  public static Texture2D CombatRangeMiniCloseMed { get; private set; }

  public static Texture2D CombatRangeMiniFar { get; private set; }

  public static Texture2D CombatRangeMiniMed { get; private set; }

  public static Texture2D CombatRangeMiniMedFar { get; private set; }

  public static Texture2D IconRange02 { get; private set; }

  public static Texture2D IconRange03 { get; private set; }

  public static Texture2D IconRange04 { get; private set; }

  public static Texture2D IconRange05 { get; private set; }

  public static Texture2D IconRange06 { get; private set; }

  public static Texture2D IconRange07 { get; private set; }

  public static Texture2D IconRange08 { get; private set; }

  public static Texture2D IconRange09 { get; private set; }

  public static Texture2D IconRange10 { get; private set; }

  public static Texture2D IconRange11 { get; private set; }

  public static Texture2D IconRange12 { get; private set; }

  public static Texture2D IconRange13 { get; private set; }

  public static Texture2D ArmorIndicatorBackground { get; private set; }

  public static Texture2D ArmorIndicatorForeground { get; private set; }
}
