using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UnityEngine;

public class AmmoPickupItem : PickupItem
{
	[SerializeField]
	private AmmoType _ammoType;

	protected override bool CanPlayerPickup => AmmoDepot.CanAddAmmo(_ammoType);

	protected override bool OnPlayerPickup()
	{
		bool flag = false;
		flag = AmmoDepot.CanAddAmmo(_ammoType);
		if (flag)
		{
			AmmoDepot.AddDefaultAmmoOfType(_ammoType);
			switch (_ammoType)
			{
			case AmmoType.Cannon:
				GameData.Instance.OnItemPickup.Fire("Cannon Rockets", PickUpMessageType.AmmoCannon);
				break;
			case AmmoType.Handgun:
				GameData.Instance.OnItemPickup.Fire("Handgun Rounds", PickUpMessageType.AmmoHandgun);
				break;
			case AmmoType.Launcher:
				GameData.Instance.OnItemPickup.Fire("Launcher Grenades", PickUpMessageType.AmmoLauncher);
				break;
			case AmmoType.Machinegun:
				GameData.Instance.OnItemPickup.Fire("Machinegun Ammo", PickUpMessageType.AmmoMachinegun);
				break;
			case AmmoType.Shotgun:
				GameData.Instance.OnItemPickup.Fire("Shotgun Shells", PickUpMessageType.AmmoShotgun);
				break;
			case AmmoType.Snipergun:
				GameData.Instance.OnItemPickup.Fire("Sniper Bullets", PickUpMessageType.AmmoSnipergun);
				break;
			case AmmoType.Splattergun:
				GameData.Instance.OnItemPickup.Fire("Splattergun Cells", PickUpMessageType.AmmoSplattergun);
				break;
			}
			PlayLocalPickupSound(GameAudio.AmmoPickup2D);
			if (AmmoDepot.TryGetAmmoTypeFromItemClass(_ammoType, out UberstrikeItemClass itemClass))
			{
				GameState.Current.Actions.PickupPowerup(base.PickupID, PickupItemType.Ammo, (byte)itemClass);
			}
			if (GameState.Current.IsSinglePlayer)
			{
				StartCoroutine(StartHidingPickupForSeconds(_respawnTime));
			}
		}
		return flag;
	}

	protected override void OnRemotePickup()
	{
		PlayRemotePickupSound(GameAudio.AmmoPickup, base.transform.position);
	}
}
