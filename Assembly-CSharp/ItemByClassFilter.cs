using UberStrike.Core.Types;

public class ItemByClassFilter : IShopItemFilter
{
	private UberstrikeItemType _itemType;

	private UberstrikeItemClass _itemClass;

	public ItemByClassFilter(UberstrikeItemType itemType, UberstrikeItemClass itemClass)
	{
		_itemType = itemType;
		_itemClass = itemClass;
	}

	public bool CanPass(IUnityItem item)
	{
		if (item.View.ItemType == _itemType)
		{
			return item.View.ItemClass == _itemClass;
		}
		return false;
	}
}
