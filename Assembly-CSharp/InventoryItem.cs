using System;
using UnityEngine;

public class InventoryItem
{
	private IUnityItem _item;

	public IUnityItem Item => _item;

	public int DaysRemaining
	{
		get
		{
			if (IsPermanent || !ExpirationDate.HasValue)
			{
				return 0;
			}
			return Mathf.CeilToInt((float)ExpirationDate.Value.Subtract(ApplicationDataManager.ServerDateTime).TotalHours / 24f);
		}
	}

	public int AmountRemaining
	{
		get;
		set;
	}

	public bool IsPermanent
	{
		get;
		set;
	}

	public DateTime? ExpirationDate
	{
		get;
		set;
	}

	public bool IsHighlighted
	{
		get;
		set;
	}

	public bool IsValid
	{
		get
		{
			if (!IsPermanent)
			{
				return DaysRemaining > 0;
			}
			return true;
		}
	}

	public InventoryItem(IUnityItem item)
	{
		_item = item;
	}
}
