// Decompiled with JetBrains decompiler
// Type: ConsumableHudTextures
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class ConsumableHudTextures
{
  static ConsumableHudTextures()
  {
    Texture2DConfigurator component;
    try
    {
      component = GameObject.Find(nameof (ConsumableHudTextures)).GetComponent<Texture2DConfigurator>();
    }
    catch
    {
      Debug.LogError((object) "Missing instance of the prefab with name: ConsumableHudTextures!");
      return;
    }
    ConsumableHudTextures.TooltipDown = component.Assets[0];
    ConsumableHudTextures.TooltipLeft = component.Assets[1];
    ConsumableHudTextures.TooltipRight = component.Assets[2];
    ConsumableHudTextures.TooltipUp = component.Assets[3];
    ConsumableHudTextures.AmmoBlue = component.Assets[4];
    ConsumableHudTextures.AmmoRed = component.Assets[5];
    ConsumableHudTextures.ArmorBlue = component.Assets[6];
    ConsumableHudTextures.ArmorRed = component.Assets[7];
    ConsumableHudTextures.HealthBlue = component.Assets[8];
    ConsumableHudTextures.HealthRed = component.Assets[9];
    ConsumableHudTextures.OffensiveGrenadeBlue = component.Assets[10];
    ConsumableHudTextures.OffensiveGrenadeRed = component.Assets[11];
    ConsumableHudTextures.SpringGrenadeBlue = component.Assets[12];
    ConsumableHudTextures.SpringGrenadeRed = component.Assets[13];
    ConsumableHudTextures.CircleBlue = component.Assets[14];
    ConsumableHudTextures.CircleRed = component.Assets[15];
    ConsumableHudTextures.CircleWhite = component.Assets[16];
  }

  public static Texture2D TooltipDown { get; private set; }

  public static Texture2D TooltipLeft { get; private set; }

  public static Texture2D TooltipRight { get; private set; }

  public static Texture2D TooltipUp { get; private set; }

  public static Texture2D AmmoBlue { get; private set; }

  public static Texture2D AmmoRed { get; private set; }

  public static Texture2D ArmorBlue { get; private set; }

  public static Texture2D ArmorRed { get; private set; }

  public static Texture2D HealthBlue { get; private set; }

  public static Texture2D HealthRed { get; private set; }

  public static Texture2D OffensiveGrenadeBlue { get; private set; }

  public static Texture2D OffensiveGrenadeRed { get; private set; }

  public static Texture2D SpringGrenadeBlue { get; private set; }

  public static Texture2D SpringGrenadeRed { get; private set; }

  public static Texture2D CircleBlue { get; private set; }

  public static Texture2D CircleRed { get; private set; }

  public static Texture2D CircleWhite { get; private set; }
}
