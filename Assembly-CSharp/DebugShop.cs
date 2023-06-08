using UnityEngine;

public class DebugShop : IDebugPage
{
	private Vector2 scroll1;

	private Vector2 scroll2;

	private Vector2 scroll3;

	public string Title => "Shop";

	public void Draw()
	{
		GUILayout.BeginHorizontal();
		scroll1 = GUILayout.BeginScrollView(scroll1, GUILayout.Width(Screen.width / 3));
		GUILayout.Label("SHOP");
		foreach (IUnityItem shopItem in Singleton<ItemManager>.Instance.ShopItems)
		{
			GUILayout.Label(shopItem.View.ID.ToString() + ": " + shopItem.Name);
		}
		GUILayout.EndScrollView();
		scroll2 = GUILayout.BeginScrollView(scroll2, GUILayout.Width(Screen.width / 3));
		GUILayout.Label("INVENTORY");
		foreach (InventoryItem inventoryItem in Singleton<InventoryManager>.Instance.InventoryItems)
		{
			GUILayout.Label(inventoryItem.Item.View.ID.ToString() + ": " + inventoryItem.Item.Name + ", Amount: " + inventoryItem.AmountRemaining.ToString() + ", Days: " + inventoryItem.DaysRemaining.ToString());
		}
		GUILayout.EndScrollView();
		scroll3 = GUILayout.BeginScrollView(scroll3, GUILayout.Width(Screen.width / 3));
		GUILayout.Label("LOADOUT");
		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();
	}
}
