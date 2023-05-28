// Decompiled with JetBrains decompiler
// Type: UberstrikeIconsHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public static class UberstrikeIconsHelper
{
  private static Texture2D _white;

  public static Texture2D White
  {
    get
    {
      if ((Object) UberstrikeIconsHelper._white == (Object) null)
      {
        UberstrikeIconsHelper._white = new Texture2D(1, 1, TextureFormat.RGB24, false);
        UberstrikeIconsHelper._white.SetPixel(0, 0, Color.white);
        UberstrikeIconsHelper._white.Apply();
      }
      return UberstrikeIconsHelper._white;
    }
  }

  public static Texture2D GetIconForItemClass(UberstrikeItemClass itemClass)
  {
    switch (itemClass)
    {
      case UberstrikeItemClass.WeaponMelee:
        return ShopIcons.StatsMostWeaponSplatsMelee;
      case UberstrikeItemClass.WeaponHandgun:
        return ShopIcons.StatsMostWeaponSplatsHandgun;
      case UberstrikeItemClass.WeaponMachinegun:
        return ShopIcons.StatsMostWeaponSplatsMachinegun;
      case UberstrikeItemClass.WeaponShotgun:
        return ShopIcons.StatsMostWeaponSplatsShotgun;
      case UberstrikeItemClass.WeaponSniperRifle:
        return ShopIcons.StatsMostWeaponSplatsSniperRifle;
      case UberstrikeItemClass.WeaponCannon:
        return ShopIcons.StatsMostWeaponSplatsCannon;
      case UberstrikeItemClass.WeaponSplattergun:
        return ShopIcons.StatsMostWeaponSplatsSplattergun;
      case UberstrikeItemClass.WeaponLauncher:
        return ShopIcons.StatsMostWeaponSplatsLauncher;
      case UberstrikeItemClass.GearBoots:
        return ShopIcons.Boots;
      case UberstrikeItemClass.GearHead:
        return ShopIcons.Head;
      case UberstrikeItemClass.GearFace:
        return ShopIcons.Face;
      case UberstrikeItemClass.GearUpperBody:
        return ShopIcons.Upperbody;
      case UberstrikeItemClass.GearLowerBody:
        return ShopIcons.Lowerbody;
      case UberstrikeItemClass.GearGloves:
        return ShopIcons.Gloves;
      case UberstrikeItemClass.QuickUseGeneral:
        return ShopIcons.QuickItems;
      case UberstrikeItemClass.QuickUseGrenade:
        return ShopIcons.QuickItems;
      case UberstrikeItemClass.QuickUseMine:
        return ShopIcons.QuickItems;
      case UberstrikeItemClass.FunctionalGeneral:
        return ShopIcons.FunctionalItems;
      case UberstrikeItemClass.SpecialGeneral:
        return ShopIcons.FunctionalItems;
      case UberstrikeItemClass.GearHolo:
        return ShopIcons.Holos;
      default:
        return (Texture2D) null;
    }
  }

  public static Texture2D GetIconForChannel(ChannelType channel)
  {
    switch (channel)
    {
      case ChannelType.WebPortal:
        return CommunicatorIcons.ChannelPortal16x16;
      case ChannelType.WebFacebook:
        return CommunicatorIcons.ChannelFacebook16x16;
      case ChannelType.WindowsStandalone:
        return CommunicatorIcons.ChannelWindows16x16;
      case ChannelType.MacAppStore:
        return CommunicatorIcons.ChannelApple16x16;
      case ChannelType.OSXStandalone:
        return CommunicatorIcons.ChannelApple16x16;
      case ChannelType.IPhone:
      case ChannelType.IPad:
        return CommunicatorIcons.ChannelIos16x16;
      case ChannelType.Android:
        return CommunicatorIcons.ChannelAndroid16x16;
      case ChannelType.Kongregate:
        return CommunicatorIcons.ChannelKongregate16x16;
      default:
        return UberstrikeIconsHelper.White;
    }
  }

  public static Texture2D GetAchievementBadgeTexture(AchievementType achievement)
  {
    switch (achievement)
    {
      case AchievementType.MostValuable:
        return AchievementIcons.Achievement1MostValuablePlayer;
      case AchievementType.MostAggressive:
        return AchievementIcons.Achievement2MostAggressive;
      case AchievementType.SharpestShooter:
        return AchievementIcons.Achievement3SharpestShooter;
      case AchievementType.TriggerHappy:
        return AchievementIcons.Achievement4TriggerHappy;
      case AchievementType.HardestHitter:
        return AchievementIcons.Achievement5HardestHitter;
      case AchievementType.CostEffective:
        return AchievementIcons.Achievement6CostEffective;
      default:
        return AchievementIcons.AchievementDefault;
    }
  }

  public static Texture2D GetRecommendBadgeTexture(RecommendType recomType)
  {
    switch (recomType)
    {
      case RecommendType.MostEfficient:
        return AchievementIcons.RecommendationMostEfficientWeapon;
      case RecommendType.RecommendedWeapon:
        return AchievementIcons.RecommendationWeapon;
      case RecommendType.RecommendedArmor:
        return AchievementIcons.RecommendationGear;
      case RecommendType.StaffPick:
        return AchievementIcons.RecommendationSale;
      default:
        return UberstrikeIconsHelper.White;
    }
  }
}
