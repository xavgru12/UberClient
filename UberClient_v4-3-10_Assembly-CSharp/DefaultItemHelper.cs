// Decompiled with JetBrains decompiler
// Type: DefaultItemHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;

public static class DefaultItemHelper
{
  public static void ConfigureDefaultGearAndWeapons()
  {
    ItemManager instance1 = Singleton<ItemManager>.Instance;
    UberStrikeItemGearView strikeItemGearView1 = new UberStrikeItemGearView();
    strikeItemGearView1.ID = Singleton<ItemManager>.Instance.DefaultHeadItemId;
    strikeItemGearView1.PrefabName = "LutzDefaultGearHead";
    UberStrikeItemGearView itemView1 = strikeItemGearView1;
    instance1.AddItemToShop((BaseUberStrikeItemView) itemView1);
    ItemManager instance2 = Singleton<ItemManager>.Instance;
    UberStrikeItemGearView strikeItemGearView2 = new UberStrikeItemGearView();
    strikeItemGearView2.ID = Singleton<ItemManager>.Instance.DefaultGlovesItemId;
    strikeItemGearView2.PrefabName = "LutzDefaultGearGloves";
    UberStrikeItemGearView itemView2 = strikeItemGearView2;
    instance2.AddItemToShop((BaseUberStrikeItemView) itemView2);
    ItemManager instance3 = Singleton<ItemManager>.Instance;
    UberStrikeItemGearView strikeItemGearView3 = new UberStrikeItemGearView();
    strikeItemGearView3.ID = Singleton<ItemManager>.Instance.DefaultUpperBodyItemId;
    strikeItemGearView3.PrefabName = "LutzDefaultGearUpperBody";
    UberStrikeItemGearView itemView3 = strikeItemGearView3;
    instance3.AddItemToShop((BaseUberStrikeItemView) itemView3);
    ItemManager instance4 = Singleton<ItemManager>.Instance;
    UberStrikeItemGearView strikeItemGearView4 = new UberStrikeItemGearView();
    strikeItemGearView4.ID = Singleton<ItemManager>.Instance.DefaultLowerBodyItemId;
    strikeItemGearView4.PrefabName = "LutzDefaultGearLowerBody";
    UberStrikeItemGearView itemView4 = strikeItemGearView4;
    instance4.AddItemToShop((BaseUberStrikeItemView) itemView4);
    ItemManager instance5 = Singleton<ItemManager>.Instance;
    UberStrikeItemGearView strikeItemGearView5 = new UberStrikeItemGearView();
    strikeItemGearView5.ID = Singleton<ItemManager>.Instance.DefaultBootsItemId;
    strikeItemGearView5.PrefabName = "LutzDefaultGearBoots";
    UberStrikeItemGearView itemView5 = strikeItemGearView5;
    instance5.AddItemToShop((BaseUberStrikeItemView) itemView5);
    Singleton<ItemManager>.Instance.AddItemToShop((BaseUberStrikeItemView) DefaultItemHelper.GetDefaultWeaponView(UberstrikeItemClass.WeaponCannon));
    Singleton<ItemManager>.Instance.AddItemToShop((BaseUberStrikeItemView) DefaultItemHelper.GetDefaultWeaponView(UberstrikeItemClass.WeaponHandgun));
    Singleton<ItemManager>.Instance.AddItemToShop((BaseUberStrikeItemView) DefaultItemHelper.GetDefaultWeaponView(UberstrikeItemClass.WeaponLauncher));
    Singleton<ItemManager>.Instance.AddItemToShop((BaseUberStrikeItemView) DefaultItemHelper.GetDefaultWeaponView(UberstrikeItemClass.WeaponMachinegun));
    Singleton<ItemManager>.Instance.AddItemToShop((BaseUberStrikeItemView) DefaultItemHelper.GetDefaultWeaponView(UberstrikeItemClass.WeaponMelee));
    Singleton<ItemManager>.Instance.AddItemToShop((BaseUberStrikeItemView) DefaultItemHelper.GetDefaultWeaponView(UberstrikeItemClass.WeaponShotgun));
    Singleton<ItemManager>.Instance.AddItemToShop((BaseUberStrikeItemView) DefaultItemHelper.GetDefaultWeaponView(UberstrikeItemClass.WeaponSniperRifle));
    Singleton<ItemManager>.Instance.AddItemToShop((BaseUberStrikeItemView) DefaultItemHelper.GetDefaultWeaponView(UberstrikeItemClass.WeaponSplattergun));
  }

  public static UberStrikeItemWeaponView GetDefaultWeaponView(UberstrikeItemClass itemClass)
  {
    switch (itemClass)
    {
      case UberstrikeItemClass.WeaponMelee:
        UberStrikeItemWeaponView defaultWeaponView1 = new UberStrikeItemWeaponView();
        defaultWeaponView1.ID = 1000;
        defaultWeaponView1.ItemClass = UberstrikeItemClass.WeaponMelee;
        defaultWeaponView1.PrefabName = "TheSplatbat";
        defaultWeaponView1.DamageKnockback = 1000;
        defaultWeaponView1.DamagePerProjectile = 99;
        defaultWeaponView1.AccuracySpread = 0;
        defaultWeaponView1.RecoilKickback = 0;
        defaultWeaponView1.StartAmmo = 0;
        defaultWeaponView1.MaxAmmo = 0;
        defaultWeaponView1.MissileTimeToDetonate = 0;
        defaultWeaponView1.MissileForceImpulse = 0;
        defaultWeaponView1.MissileBounciness = 0;
        defaultWeaponView1.RateOfFire = 500;
        defaultWeaponView1.SplashRadius = 100;
        defaultWeaponView1.ProjectilesPerShot = 1;
        defaultWeaponView1.ProjectileSpeed = 0;
        defaultWeaponView1.RecoilMovement = 0;
        return defaultWeaponView1;
      case UberstrikeItemClass.WeaponHandgun:
        UberStrikeItemWeaponView defaultWeaponView2 = new UberStrikeItemWeaponView();
        defaultWeaponView2.ID = 1001;
        defaultWeaponView2.ItemClass = UberstrikeItemClass.WeaponHandgun;
        defaultWeaponView2.PrefabName = "HandGun";
        defaultWeaponView2.DamageKnockback = 80;
        defaultWeaponView2.DamagePerProjectile = 24;
        defaultWeaponView2.AccuracySpread = 3;
        defaultWeaponView2.RecoilKickback = 8;
        defaultWeaponView2.StartAmmo = 25;
        defaultWeaponView2.MaxAmmo = 50;
        defaultWeaponView2.MissileTimeToDetonate = 0;
        defaultWeaponView2.MissileForceImpulse = 0;
        defaultWeaponView2.MissileBounciness = 0;
        defaultWeaponView2.RateOfFire = 200;
        defaultWeaponView2.SplashRadius = 100;
        defaultWeaponView2.ProjectilesPerShot = 1;
        defaultWeaponView2.ProjectileSpeed = 0;
        defaultWeaponView2.RecoilMovement = 8;
        return defaultWeaponView2;
      case UberstrikeItemClass.WeaponMachinegun:
        UberStrikeItemWeaponView defaultWeaponView3 = new UberStrikeItemWeaponView();
        defaultWeaponView3.ID = 1002;
        defaultWeaponView3.ItemClass = UberstrikeItemClass.WeaponMachinegun;
        defaultWeaponView3.PrefabName = "MachineGun";
        defaultWeaponView3.DamageKnockback = 50;
        defaultWeaponView3.DamagePerProjectile = 13;
        defaultWeaponView3.AccuracySpread = 3;
        defaultWeaponView3.RecoilKickback = 4;
        defaultWeaponView3.StartAmmo = 100;
        defaultWeaponView3.MaxAmmo = 300;
        defaultWeaponView3.MissileTimeToDetonate = 0;
        defaultWeaponView3.MissileForceImpulse = 0;
        defaultWeaponView3.MissileBounciness = 0;
        defaultWeaponView3.RateOfFire = 125;
        defaultWeaponView3.SplashRadius = 100;
        defaultWeaponView3.ProjectilesPerShot = 1;
        defaultWeaponView3.ProjectileSpeed = 0;
        defaultWeaponView3.RecoilMovement = 5;
        return defaultWeaponView3;
      case UberstrikeItemClass.WeaponShotgun:
        UberStrikeItemWeaponView defaultWeaponView4 = new UberStrikeItemWeaponView();
        defaultWeaponView4.ID = 1003;
        defaultWeaponView4.ItemClass = UberstrikeItemClass.WeaponShotgun;
        defaultWeaponView4.PrefabName = "ShotGun";
        defaultWeaponView4.DamageKnockback = 160;
        defaultWeaponView4.DamagePerProjectile = 9;
        defaultWeaponView4.AccuracySpread = 8;
        defaultWeaponView4.RecoilKickback = 15;
        defaultWeaponView4.StartAmmo = 20;
        defaultWeaponView4.MaxAmmo = 50;
        defaultWeaponView4.MissileTimeToDetonate = 0;
        defaultWeaponView4.MissileForceImpulse = 0;
        defaultWeaponView4.MissileBounciness = 0;
        defaultWeaponView4.RateOfFire = 1000;
        defaultWeaponView4.SplashRadius = 100;
        defaultWeaponView4.ProjectilesPerShot = 11;
        defaultWeaponView4.ProjectileSpeed = 0;
        defaultWeaponView4.RecoilMovement = 10;
        return defaultWeaponView4;
      case UberstrikeItemClass.WeaponSniperRifle:
        UberStrikeItemWeaponView defaultWeaponView5 = new UberStrikeItemWeaponView();
        defaultWeaponView5.ID = 1004;
        defaultWeaponView5.ItemClass = UberstrikeItemClass.WeaponSniperRifle;
        defaultWeaponView5.PrefabName = "SniperRifle";
        defaultWeaponView5.DamageKnockback = 150;
        defaultWeaponView5.DamagePerProjectile = 70;
        defaultWeaponView5.AccuracySpread = 0;
        defaultWeaponView5.RecoilKickback = 12;
        defaultWeaponView5.StartAmmo = 20;
        defaultWeaponView5.MaxAmmo = 50;
        defaultWeaponView5.MissileTimeToDetonate = 0;
        defaultWeaponView5.MissileForceImpulse = 0;
        defaultWeaponView5.MissileBounciness = 0;
        defaultWeaponView5.RateOfFire = 1500;
        defaultWeaponView5.SplashRadius = 100;
        defaultWeaponView5.ProjectilesPerShot = 1;
        defaultWeaponView5.ProjectileSpeed = 0;
        defaultWeaponView5.RecoilMovement = 15;
        return defaultWeaponView5;
      case UberstrikeItemClass.WeaponCannon:
        UberStrikeItemWeaponView defaultWeaponView6 = new UberStrikeItemWeaponView();
        defaultWeaponView6.ID = 1005;
        defaultWeaponView6.ItemClass = UberstrikeItemClass.WeaponCannon;
        defaultWeaponView6.PrefabName = "Cannon";
        defaultWeaponView6.DamageKnockback = 600;
        defaultWeaponView6.DamagePerProjectile = 65;
        defaultWeaponView6.AccuracySpread = 0;
        defaultWeaponView6.RecoilKickback = 12;
        defaultWeaponView6.StartAmmo = 10;
        defaultWeaponView6.MaxAmmo = 25;
        defaultWeaponView6.MissileTimeToDetonate = 5000;
        defaultWeaponView6.MissileForceImpulse = 0;
        defaultWeaponView6.MissileBounciness = 0;
        defaultWeaponView6.RateOfFire = 1000;
        defaultWeaponView6.SplashRadius = 250;
        defaultWeaponView6.ProjectilesPerShot = 1;
        defaultWeaponView6.ProjectileSpeed = 50;
        defaultWeaponView6.RecoilMovement = 32;
        return defaultWeaponView6;
      case UberstrikeItemClass.WeaponSplattergun:
        UberStrikeItemWeaponView defaultWeaponView7 = new UberStrikeItemWeaponView();
        defaultWeaponView7.ID = 1006;
        defaultWeaponView7.ItemClass = UberstrikeItemClass.WeaponSplattergun;
        defaultWeaponView7.PrefabName = "SplatterGun";
        defaultWeaponView7.DamageKnockback = 150;
        defaultWeaponView7.DamagePerProjectile = 16;
        defaultWeaponView7.AccuracySpread = 0;
        defaultWeaponView7.RecoilKickback = 0;
        defaultWeaponView7.StartAmmo = 60;
        defaultWeaponView7.MaxAmmo = 200;
        defaultWeaponView7.MissileTimeToDetonate = 5000;
        defaultWeaponView7.MissileForceImpulse = 0;
        defaultWeaponView7.MissileBounciness = 80;
        defaultWeaponView7.RateOfFire = 90;
        defaultWeaponView7.SplashRadius = 80;
        defaultWeaponView7.ProjectilesPerShot = 1;
        defaultWeaponView7.ProjectileSpeed = 70;
        defaultWeaponView7.RecoilMovement = 0;
        return defaultWeaponView7;
      case UberstrikeItemClass.WeaponLauncher:
        UberStrikeItemWeaponView defaultWeaponView8 = new UberStrikeItemWeaponView();
        defaultWeaponView8.ID = 1007;
        defaultWeaponView8.ItemClass = UberstrikeItemClass.WeaponLauncher;
        defaultWeaponView8.PrefabName = "Launcher";
        defaultWeaponView8.DamageKnockback = 450;
        defaultWeaponView8.DamagePerProjectile = 70;
        defaultWeaponView8.AccuracySpread = 0;
        defaultWeaponView8.RecoilKickback = 15;
        defaultWeaponView8.StartAmmo = 15;
        defaultWeaponView8.MaxAmmo = 30;
        defaultWeaponView8.MissileTimeToDetonate = 1250;
        defaultWeaponView8.MissileForceImpulse = 0;
        defaultWeaponView8.MissileBounciness = 0;
        defaultWeaponView8.RateOfFire = 1000;
        defaultWeaponView8.SplashRadius = 400;
        defaultWeaponView8.ProjectilesPerShot = 1;
        defaultWeaponView8.ProjectileSpeed = 20;
        defaultWeaponView8.RecoilMovement = 9;
        return defaultWeaponView8;
      default:
        return (UberStrikeItemWeaponView) null;
    }
  }
}
