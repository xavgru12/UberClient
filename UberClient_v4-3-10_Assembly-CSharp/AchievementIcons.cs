// Decompiled with JetBrains decompiler
// Type: AchievementIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class AchievementIcons
{
  static AchievementIcons()
  {
    Texture2DConfigurator component = GameObject.Find(nameof (AchievementIcons)).GetComponent<Texture2DConfigurator>();
    AchievementIcons.Achievement1MostValuablePlayer = !((UnityEngine.Object) component == (UnityEngine.Object) null) ? component.Assets[0] : throw new Exception("Missing instance of the prefab with name: AchievementIcons!");
    AchievementIcons.Achievement2MostAggressive = component.Assets[1];
    AchievementIcons.Achievement3SharpestShooter = component.Assets[2];
    AchievementIcons.Achievement4TriggerHappy = component.Assets[3];
    AchievementIcons.Achievement5HardestHitter = component.Assets[4];
    AchievementIcons.Achievement6CostEffective = component.Assets[5];
    AchievementIcons.AchievementDefault = component.Assets[6];
    AchievementIcons.RecommendationGear = component.Assets[7];
    AchievementIcons.RecommendationMostEfficientWeapon = component.Assets[8];
    AchievementIcons.RecommendationSale = component.Assets[9];
    AchievementIcons.RecommendationWeapon = component.Assets[10];
  }

  public static Texture2D Achievement1MostValuablePlayer { get; private set; }

  public static Texture2D Achievement2MostAggressive { get; private set; }

  public static Texture2D Achievement3SharpestShooter { get; private set; }

  public static Texture2D Achievement4TriggerHappy { get; private set; }

  public static Texture2D Achievement5HardestHitter { get; private set; }

  public static Texture2D Achievement6CostEffective { get; private set; }

  public static Texture2D AchievementDefault { get; private set; }

  public static Texture2D RecommendationGear { get; private set; }

  public static Texture2D RecommendationMostEfficientWeapon { get; private set; }

  public static Texture2D RecommendationSale { get; private set; }

  public static Texture2D RecommendationWeapon { get; private set; }
}
