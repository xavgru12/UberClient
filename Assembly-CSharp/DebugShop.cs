// Decompiled with JetBrains decompiler
// Type: DebugShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

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
    this.scroll1 = GUILayout.BeginScrollView(this.scroll1, GUILayout.Width((float) (Screen.width / 3)));
    GUILayout.Label("SHOP");
    foreach (IUnityItem shopItem in Singleton<ItemManager>.Instance.ShopItems)
      GUILayout.Label(shopItem.ItemId.ToString() + ": " + shopItem.Name);
    GUILayout.EndScrollView();
    this.scroll2 = GUILayout.BeginScrollView(this.scroll2, GUILayout.Width((float) (Screen.width / 3)));
    GUILayout.Label("INVENTORY");
    foreach (InventoryItem inventoryItem in Singleton<InventoryManager>.Instance.InventoryItems)
      GUILayout.Label(inventoryItem.Item.ItemId.ToString() + ": " + inventoryItem.Item.Name + ", Amount: " + (object) inventoryItem.AmountRemaining + ", Days: " + (object) inventoryItem.DaysRemaining);
    GUILayout.EndScrollView();
    this.scroll3 = GUILayout.BeginScrollView(this.scroll3, GUILayout.Width((float) (Screen.width / 3)));
    GUILayout.Label("LOADOUT");
    foreach (int equippedItem in Singleton<LoadoutManager>.Instance.EquippedItems)
      GUILayout.Label("Id: " + (object) equippedItem);
    GUILayout.EndScrollView();
    GUILayout.EndHorizontal();
  }
}
