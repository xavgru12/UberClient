using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class PointsUnityItem : IUnityItem
{
	private class DummyItemView : BaseUberStrikeItemView
	{
		public override UberstrikeItemType ItemType => UberstrikeItemType.Special;
	}

	public bool Equippable => false;

	public bool IsLoaded => true;

	public string Name
	{
		get;
		private set;
	}

	public BaseUberStrikeItemView View
	{
		get;
		private set;
	}

	public GameObject Prefab => null;

	public PointsUnityItem(int points)
	{
		Name = points.ToString("N0") + " Points";
		View = new DummyItemView
		{
			Description = $"An extra {points:N0} Points to fatten up your UberWallet!"
		};
	}

	public void Unload()
	{
	}

	public GameObject Create(Vector3 position, Quaternion rotation)
	{
		return null;
	}

	public void DrawIcon(Rect position)
	{
		GUI.DrawTexture(position, ShopIcons.Points48x48);
	}
}
