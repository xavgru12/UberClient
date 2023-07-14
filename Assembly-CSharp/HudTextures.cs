// Decompiled with JetBrains decompiler
// Type: HudTextures
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class HudTextures
{
  static HudTextures()
  {
    Texture2DConfigurator component = GameObject.Find(nameof (HudTextures)).GetComponent<Texture2DConfigurator>();
    HudTextures.AmmoBlue = !((UnityEngine.Object) component == (UnityEngine.Object) null) ? component.Assets[0] : throw new Exception("Missing instance of the prefab with name: HudTextures!");
    HudTextures.AmmoRed = component.Assets[1];
    HudTextures.WhiteBlur128 = component.Assets[2];
    HudTextures.XPBarEmptyBlue = component.Assets[3];
    HudTextures.XPBarEmptyRed = component.Assets[4];
    HudTextures.XPBarFull = component.Assets[5];
    HudTextures.DeathScreenTab = component.Assets[6];
    HudTextures.DamageFeedbackMark = component.Assets[7];
    HudTextures.LevelUp = component.Assets[8];
    HudTextures.MGScale = component.Assets[9];
    HudTextures.MGTranslate = component.Assets[10];
    HudTextures.CNRotate = component.Assets[11];
    HudTextures.CNScale = component.Assets[12];
    HudTextures.HGTraslate = component.Assets[13];
    HudTextures.LRScale = component.Assets[14];
    HudTextures.LRTranslate = component.Assets[15];
    HudTextures.MWTranslate = component.Assets[16];
    HudTextures.SGScaleInside = component.Assets[17];
    HudTextures.SGScaleOutside = component.Assets[18];
    HudTextures.SPScale = component.Assets[19];
    HudTextures.SPTranslate = component.Assets[20];
    HudTextures.SRScale = component.Assets[21];
    HudTextures.SRTranslate = component.Assets[22];
    HudTextures.TargetCircle = component.Assets[23];
    HudTextures.ReticleSRZoom = component.Assets[24];
    HudTextures.QIBlue3 = component.Assets[25];
    HudTextures.QIBlue1 = component.Assets[26];
    HudTextures.QIRed3 = component.Assets[27];
    HudTextures.QIRed1 = component.Assets[28];
  }

  public static Texture2D AmmoBlue { get; private set; }

  public static Texture2D AmmoRed { get; private set; }

  public static Texture2D WhiteBlur128 { get; private set; }

  public static Texture2D XPBarEmptyBlue { get; private set; }

  public static Texture2D XPBarEmptyRed { get; private set; }

  public static Texture2D XPBarFull { get; private set; }

  public static Texture2D DeathScreenTab { get; private set; }

  public static Texture2D DamageFeedbackMark { get; private set; }

  public static Texture2D LevelUp { get; private set; }

  public static Texture2D MGScale { get; private set; }

  public static Texture2D MGTranslate { get; private set; }

  public static Texture2D CNRotate { get; private set; }

  public static Texture2D CNScale { get; private set; }

  public static Texture2D HGTraslate { get; private set; }

  public static Texture2D LRScale { get; private set; }

  public static Texture2D LRTranslate { get; private set; }

  public static Texture2D MWTranslate { get; private set; }

  public static Texture2D SGScaleInside { get; private set; }

  public static Texture2D SGScaleOutside { get; private set; }

  public static Texture2D SPScale { get; private set; }

  public static Texture2D SPTranslate { get; private set; }

  public static Texture2D SRScale { get; private set; }

  public static Texture2D SRTranslate { get; private set; }

  public static Texture2D TargetCircle { get; private set; }

  public static Texture2D ReticleSRZoom { get; private set; }

  public static Texture2D QIBlue3 { get; private set; }

  public static Texture2D QIBlue1 { get; private set; }

  public static Texture2D QIRed3 { get; private set; }

  public static Texture2D QIRed1 { get; private set; }
}
