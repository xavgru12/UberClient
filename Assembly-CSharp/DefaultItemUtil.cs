using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;

public static class DefaultItemUtil
{
	public const string HeadName = "LutzDefaultGearHead";

	public const string GlovesName = "LutzDefaultGearGloves";

	public const string UpperbodyName = "LutzDefaultGearUpperBody";

	public const string LowerbodyName = "LutzDefaultGearLowerBody";

	public const string BootsName = "LutzDefaultGearBoots";

	public const string FaceName = "LutzDefaultGearFace";

	public const string MeleeName = "TheSplatbat";

	public const string HandGunName = "HandGun";

	public const string MachineGunName = "MachineGun";

	public const string SplatterGunName = "SplatterGun";

	public const string CannonName = "Cannon";

	public const string SniperRifleName = "SniperRifle";

	public const string LauncherName = "Launcher";

	public const string ShotGunName = "ShotGun";

	public const int HeadId = 1084;

	public const int GlovesId = 1086;

	public const int UpperbodyId = 1087;

	public const int LowerbodyId = 1088;

	public const int BootsId = 1089;

	public const int MeleeId = 1000;

	public const int HandgunId = 1001;

	public const int MachineGunId = 1002;

	public const int ShotGunId = 1003;

	public const int SniperRifleId = 1004;

	public const int CannonId = 1005;

	public const int SplatterGunId = 1006;

	public const int LauncherId = 1007;

	public static void ConfigureDefaultGearAndWeapons()
	{
		Singleton<ItemManager>.Instance.AddDefaultItem(new UberStrikeItemGearView
		{
			ID = 1084,
			PrefabName = "LutzDefaultGearHead"
		});
		Singleton<ItemManager>.Instance.AddDefaultItem(new UberStrikeItemGearView
		{
			ID = 1086,
			PrefabName = "LutzDefaultGearGloves"
		});
		Singleton<ItemManager>.Instance.AddDefaultItem(new UberStrikeItemGearView
		{
			ID = 1087,
			PrefabName = "LutzDefaultGearUpperBody"
		});
		Singleton<ItemManager>.Instance.AddDefaultItem(new UberStrikeItemGearView
		{
			ID = 1088,
			PrefabName = "LutzDefaultGearLowerBody"
		});
		Singleton<ItemManager>.Instance.AddDefaultItem(new UberStrikeItemGearView
		{
			ID = 1089,
			PrefabName = "LutzDefaultGearBoots"
		});
		for (UberstrikeItemClass uberstrikeItemClass = UberstrikeItemClass.WeaponMelee; uberstrikeItemClass <= UberstrikeItemClass.WeaponLauncher; uberstrikeItemClass++)
		{
			Singleton<ItemManager>.Instance.AddDefaultItem(GetDefaultWeaponView(uberstrikeItemClass));
		}
	}

	public static UberStrikeItemWeaponView GetDefaultWeaponView(UberstrikeItemClass itemClass)
	{
		switch (itemClass)
		{
		case UberstrikeItemClass.WeaponCannon:
		{
			UberStrikeItemWeaponView uberStrikeItemWeaponView7 = new UberStrikeItemWeaponView();
			uberStrikeItemWeaponView7.ID = 97;
			uberStrikeItemWeaponView7.ItemClass = UberstrikeItemClass.WeaponCannon;
			uberStrikeItemWeaponView7.PrefabName = "Cannon";
			uberStrikeItemWeaponView7.DamageKnockback = 600;
			uberStrikeItemWeaponView7.DamagePerProjectile = 65;
			uberStrikeItemWeaponView7.AccuracySpread = 0;
			uberStrikeItemWeaponView7.RecoilKickback = 12;
			uberStrikeItemWeaponView7.StartAmmo = 20;
			uberStrikeItemWeaponView7.MaxAmmo = 35;
			uberStrikeItemWeaponView7.MissileTimeToDetonate = 0;
			uberStrikeItemWeaponView7.MissileForceImpulse = 0;
			uberStrikeItemWeaponView7.MissileBounciness = 0;
			uberStrikeItemWeaponView7.RateOfFire = 850;
			uberStrikeItemWeaponView7.SplashRadius = 300;
			uberStrikeItemWeaponView7.ProjectilesPerShot = 1;
			uberStrikeItemWeaponView7.ProjectileSpeed = 50;
			uberStrikeItemWeaponView7.RecoilMovement = 32;
			uberStrikeItemWeaponView7.DefaultZoomMultiplier = 0;
			uberStrikeItemWeaponView7.MinZoomMultiplier = 0;
			uberStrikeItemWeaponView7.MaxZoomMultiplier = 0;
			return uberStrikeItemWeaponView7;
		}
		case UberstrikeItemClass.WeaponLauncher:
		{
			UberStrikeItemWeaponView uberStrikeItemWeaponView6 = new UberStrikeItemWeaponView();
			uberStrikeItemWeaponView6.ID = 111;
			uberStrikeItemWeaponView6.ItemClass = UberstrikeItemClass.WeaponLauncher;
			uberStrikeItemWeaponView6.PrefabName = "Launcher";
			uberStrikeItemWeaponView6.DamageKnockback = 500;
			uberStrikeItemWeaponView6.DamagePerProjectile = 70;
			uberStrikeItemWeaponView6.AccuracySpread = 0;
			uberStrikeItemWeaponView6.RecoilKickback = 15;
			uberStrikeItemWeaponView6.StartAmmo = 20;
			uberStrikeItemWeaponView6.MaxAmmo = 30;
			uberStrikeItemWeaponView6.MissileTimeToDetonate = 1100;
			uberStrikeItemWeaponView6.MissileForceImpulse = 0;
			uberStrikeItemWeaponView6.MissileBounciness = 0;
			uberStrikeItemWeaponView6.RateOfFire = 1000;
			uberStrikeItemWeaponView6.SplashRadius = 500;
			uberStrikeItemWeaponView6.ProjectilesPerShot = 1;
			uberStrikeItemWeaponView6.ProjectileSpeed = 30;
			uberStrikeItemWeaponView6.RecoilMovement = 9;
			uberStrikeItemWeaponView6.DefaultZoomMultiplier = 1;
			uberStrikeItemWeaponView6.MinZoomMultiplier = 1;
			uberStrikeItemWeaponView6.MaxZoomMultiplier = 1;
			return uberStrikeItemWeaponView6;
		}
		case UberstrikeItemClass.WeaponMachinegun:
		{
			UberStrikeItemWeaponView uberStrikeItemWeaponView5 = new UberStrikeItemWeaponView();
			uberStrikeItemWeaponView5.ID = 12;
			uberStrikeItemWeaponView5.ItemClass = UberstrikeItemClass.WeaponMachinegun;
			uberStrikeItemWeaponView5.PrefabName = "MachineGun";
			uberStrikeItemWeaponView5.DamageKnockback = 0;
			uberStrikeItemWeaponView5.DamagePerProjectile = 14;
			uberStrikeItemWeaponView5.AccuracySpread = 15;
			uberStrikeItemWeaponView5.RecoilKickback = 2;
			uberStrikeItemWeaponView5.StartAmmo = 200;
			uberStrikeItemWeaponView5.MaxAmmo = 300;
			uberStrikeItemWeaponView5.MissileTimeToDetonate = 0;
			uberStrikeItemWeaponView5.MissileForceImpulse = 0;
			uberStrikeItemWeaponView5.MissileBounciness = 0;
			uberStrikeItemWeaponView5.RateOfFire = 114;
			uberStrikeItemWeaponView5.SplashRadius = 0;
			uberStrikeItemWeaponView5.ProjectilesPerShot = 1;
			uberStrikeItemWeaponView5.ProjectileSpeed = 1;
			uberStrikeItemWeaponView5.RecoilMovement = 7;
			uberStrikeItemWeaponView5.WeaponSecondaryAction = 2;
			uberStrikeItemWeaponView5.HasAutomaticFire = true;
			uberStrikeItemWeaponView5.DefaultZoomMultiplier = 0;
			uberStrikeItemWeaponView5.MinZoomMultiplier = 0;
			uberStrikeItemWeaponView5.MaxZoomMultiplier = 0;
			return uberStrikeItemWeaponView5;
		}
		case UberstrikeItemClass.WeaponMelee:
		{
			UberStrikeItemWeaponView uberStrikeItemWeaponView4 = new UberStrikeItemWeaponView();
			uberStrikeItemWeaponView4.ID = 1;
			uberStrikeItemWeaponView4.ItemClass = UberstrikeItemClass.WeaponMelee;
			uberStrikeItemWeaponView4.PrefabName = "TheSplatbat";
			uberStrikeItemWeaponView4.DamageKnockback = 1000;
			uberStrikeItemWeaponView4.DamagePerProjectile = 85;
			uberStrikeItemWeaponView4.AccuracySpread = 0;
			uberStrikeItemWeaponView4.RecoilKickback = 0;
			uberStrikeItemWeaponView4.StartAmmo = 0;
			uberStrikeItemWeaponView4.MaxAmmo = 0;
			uberStrikeItemWeaponView4.MissileTimeToDetonate = 0;
			uberStrikeItemWeaponView4.MissileForceImpulse = 0;
			uberStrikeItemWeaponView4.MissileBounciness = 0;
			uberStrikeItemWeaponView4.RateOfFire = 900;
			uberStrikeItemWeaponView4.SplashRadius = 0;
			uberStrikeItemWeaponView4.ProjectilesPerShot = 1;
			uberStrikeItemWeaponView4.ProjectileSpeed = 1;
			uberStrikeItemWeaponView4.RecoilMovement = 0;
			uberStrikeItemWeaponView4.HasAutomaticFire = true;
			uberStrikeItemWeaponView4.DefaultZoomMultiplier = 1;
			uberStrikeItemWeaponView4.MinZoomMultiplier = 1;
			uberStrikeItemWeaponView4.MaxZoomMultiplier = 1;
			return uberStrikeItemWeaponView4;
		}
		case UberstrikeItemClass.WeaponShotgun:
		{
			UberStrikeItemWeaponView uberStrikeItemWeaponView3 = new UberStrikeItemWeaponView();
			uberStrikeItemWeaponView3.ID = 44;
			uberStrikeItemWeaponView3.ItemClass = UberstrikeItemClass.WeaponShotgun;
			uberStrikeItemWeaponView3.PrefabName = "ShotGun";
			uberStrikeItemWeaponView3.DamageKnockback = 0;
			uberStrikeItemWeaponView3.DamagePerProjectile = 10;
			uberStrikeItemWeaponView3.AccuracySpread = 80;
			uberStrikeItemWeaponView3.RecoilKickback = 9;
			uberStrikeItemWeaponView3.StartAmmo = 80;
			uberStrikeItemWeaponView3.MaxAmmo = 80;
			uberStrikeItemWeaponView3.MissileTimeToDetonate = 0;
			uberStrikeItemWeaponView3.MissileForceImpulse = 0;
			uberStrikeItemWeaponView3.MissileBounciness = 0;
			uberStrikeItemWeaponView3.RateOfFire = 850;
			uberStrikeItemWeaponView3.SplashRadius = 100;
			uberStrikeItemWeaponView3.ProjectilesPerShot = 10;
			uberStrikeItemWeaponView3.ProjectileSpeed = 0;
			uberStrikeItemWeaponView3.RecoilMovement = 15;
			uberStrikeItemWeaponView3.DefaultZoomMultiplier = 0;
			uberStrikeItemWeaponView3.MinZoomMultiplier = 0;
			uberStrikeItemWeaponView3.MaxZoomMultiplier = 0;
			return uberStrikeItemWeaponView3;
		}
		case UberstrikeItemClass.WeaponSniperRifle:
		{
			UberStrikeItemWeaponView uberStrikeItemWeaponView2 = new UberStrikeItemWeaponView();
			uberStrikeItemWeaponView2.ID = 68;
			uberStrikeItemWeaponView2.ItemClass = UberstrikeItemClass.WeaponSniperRifle;
			uberStrikeItemWeaponView2.PrefabName = "SniperRifle";
			uberStrikeItemWeaponView2.DamageKnockback = 150;
			uberStrikeItemWeaponView2.DamagePerProjectile = 80;
			uberStrikeItemWeaponView2.AccuracySpread = 0;
			uberStrikeItemWeaponView2.RecoilKickback = 12;
			uberStrikeItemWeaponView2.StartAmmo = 20;
			uberStrikeItemWeaponView2.MaxAmmo = 50;
			uberStrikeItemWeaponView2.MissileTimeToDetonate = 0;
			uberStrikeItemWeaponView2.MissileForceImpulse = 0;
			uberStrikeItemWeaponView2.MissileBounciness = 0;
			uberStrikeItemWeaponView2.RateOfFire = 1000;
			uberStrikeItemWeaponView2.SplashRadius = 100;
			uberStrikeItemWeaponView2.ProjectilesPerShot = 1;
			uberStrikeItemWeaponView2.ProjectileSpeed = 0;
			uberStrikeItemWeaponView2.RecoilMovement = 15;
			uberStrikeItemWeaponView2.WeaponSecondaryAction = 1;
			uberStrikeItemWeaponView2.DefaultZoomMultiplier = 2;
			uberStrikeItemWeaponView2.MinZoomMultiplier = 2;
			uberStrikeItemWeaponView2.MaxZoomMultiplier = 4;
			return uberStrikeItemWeaponView2;
		}
		case UberstrikeItemClass.WeaponSplattergun:
		{
			UberStrikeItemWeaponView uberStrikeItemWeaponView = new UberStrikeItemWeaponView();
			uberStrikeItemWeaponView.ID = 106;
			uberStrikeItemWeaponView.ItemClass = UberstrikeItemClass.WeaponSplattergun;
			uberStrikeItemWeaponView.PrefabName = "SplatterGun";
			uberStrikeItemWeaponView.DamageKnockback = 150;
			uberStrikeItemWeaponView.DamagePerProjectile = 15;
			uberStrikeItemWeaponView.AccuracySpread = 59;
			uberStrikeItemWeaponView.RecoilKickback = 4;
			uberStrikeItemWeaponView.StartAmmo = 100;
			uberStrikeItemWeaponView.MaxAmmo = 200;
			uberStrikeItemWeaponView.MissileTimeToDetonate = 5000;
			uberStrikeItemWeaponView.MissileForceImpulse = 0;
			uberStrikeItemWeaponView.MissileBounciness = 80;
			uberStrikeItemWeaponView.RateOfFire = 140;
			uberStrikeItemWeaponView.SplashRadius = 150;
			uberStrikeItemWeaponView.ProjectilesPerShot = 1;
			uberStrikeItemWeaponView.ProjectileSpeed = 70;
			uberStrikeItemWeaponView.RecoilMovement = 0;
			uberStrikeItemWeaponView.DefaultZoomMultiplier = 1;
			uberStrikeItemWeaponView.MinZoomMultiplier = 1;
			uberStrikeItemWeaponView.MaxZoomMultiplier = 1;
			return uberStrikeItemWeaponView;
		}
		default:
			return null;
		}
	}
}
