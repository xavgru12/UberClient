﻿
using System.Runtime.InteropServices;

namespace UberStrike.DataCenter.Common.Entities
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct UberStrikeCacheKeys
  {
    public const string ShopView = "UberstrikeItemShopView";
    public const string ShopFunctionalConfigParameters = "UberstrikeFunctionalConfig";
    public const string ShopGearConfigParameters = "UberstrikeGearConfig";
    public const string ShopQuickUseConfigParameters = "UberStrikeQuickUseConfig";
    public const string ShopSpecialConfigParameters = "UberStrikeSpecialConfig";
    public const string ShopWeaponModConfigParameters = "UberStrikeWeaponModConfig";
    public const string ShopWeaponConfigParameters = "UberStrikeShopWeaponConfig";
    public const string CheckDeprecatedApplicationVersionViewParameters = "UberStrikeDeprecatedCheckApplicationVersionView";
    public const string GetDeprecatedPhotonServersViewParameters = "UberStrikeDeprecatedPhotonServersView";
    public const string CheckApplicationVersionViewParameters = "UberStrikeCheckApplicationVersionView";
    public const string GetPhotonServersViewParameters = "UberStrikePhotonServersView";
    public const string XPEvents = "UberStrikeXPEvents";
    public const string LevelCaps = "UberStrikeLevelCaps";
    public const string ReferrerSource = "UberStrikeReferrerSource";
    public const string IsFacebookComingFromSixWavesParameters = "UberStrikeFacebookFromSixWaves";
    public const string Bundles = "UberstrikeBundles";
    public const string GetLiveFeed = "UberstrikeLiveFeedView";
  }
}
