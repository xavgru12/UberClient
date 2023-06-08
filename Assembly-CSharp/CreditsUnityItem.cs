using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class CreditsUnityItem : IUnityItem
{
	private class DummyItemView : BaseUberStrikeItemView
	{
		public override UberstrikeItemType ItemType => UberstrikeItemType.Special;
	}

	public bool Equippable => false;

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

	public bool IsLoaded => true;

	public GameObject Prefab => null;

	public CreditsUnityItem(int credits)
	{
		Name = credits.ToString("N0") + " Credits";
		View = new DummyItemView
		{
			Description = $"An extra {credits:N0} Credits to fatten up your UberWallet!"
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
		GUI.DrawTexture(position, ShopIcons.CreditsIcon48x48);
	}
}
