using UberStrike.Core.Types;

public class TemporaryLoadoutManager : Singleton<TemporaryLoadoutManager>
{
	public bool IsGearLoadoutModified => !Singleton<LoadoutManager>.Instance.Loadout.Compare(GameState.Current.Avatar.Loadout);

	private TemporaryLoadoutManager()
	{
	}

	public void SetLoadoutItem(LoadoutSlotType slot, IUnityItem item)
	{
		if (item != null)
		{
			if (GameState.Current.Avatar.Loadout.TryGetItem(slot, out IUnityItem item2) && item2 != item && !Singleton<InventoryManager>.Instance.Contains(item2.View.ID) && item2.View.ItemType != UberstrikeItemType.QuickUse)
			{
				item2.Unload();
			}
			GameState.Current.Avatar.Loadout.SetSlot(slot, item);
		}
	}

	public bool IsGearLoadoutModifiedOnSlot(LoadoutSlotType slot)
	{
		if (GameState.Current.Avatar.Loadout.TryGetItem(slot, out IUnityItem item))
		{
			return item != Singleton<LoadoutManager>.Instance.GetItemOnSlot(slot);
		}
		return false;
	}

	public void ResetLoadout(LoadoutSlotType slot)
	{
		GameState.Current.Avatar.Loadout.ClearSlot(slot);
	}

	public void ResetLoadout()
	{
		if (!Singleton<LoadoutManager>.Instance.Loadout.Compare(GameState.Current.Avatar.Loadout))
		{
			GameState.Current.Avatar.Loadout.ClearAllSlots();
			GameState.Current.Avatar.SetLoadout(new Loadout(Singleton<LoadoutManager>.Instance.Loadout));
		}
	}

	public void TryGear(IUnityItem item)
	{
		if (item.View.ItemType == UberstrikeItemType.Gear)
		{
			if (item.View.ItemClass == UberstrikeItemClass.GearHolo)
			{
				SetLoadoutItem(LoadoutSlotType.GearHolo, item);
			}
			else
			{
				SetLoadoutItem(InventoryManager.GetSlotTypeForGear(item), item);
			}
			GameState.Current.Avatar.Decorator.AnimationController.TriggerGearAnimation(item.View.ItemClass);
			switch (item.View.ItemType)
			{
			case UberstrikeItemType.WeaponMod:
				break;
			case UberstrikeItemType.Gear:
				EventHandler.Global.Fire(new ShopEvents.SelectLoadoutArea
				{
					Area = LoadoutArea.Gear
				});
				break;
			case UberstrikeItemType.Weapon:
				EventHandler.Global.Fire(new ShopEvents.SelectLoadoutArea
				{
					Area = LoadoutArea.Weapons
				});
				break;
			case UberstrikeItemType.QuickUse:
				EventHandler.Global.Fire(new ShopEvents.SelectLoadoutArea
				{
					Area = LoadoutArea.QuickItems
				});
				break;
			}
		}
	}
}
