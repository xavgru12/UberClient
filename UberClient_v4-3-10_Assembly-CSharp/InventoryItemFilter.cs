// Decompiled with JetBrains decompiler
// Type: InventoryItemFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

public class InventoryItemFilter : IShopItemFilter
{
  public bool CanPass(IUnityItem item) => !Singleton<LoadoutManager>.Instance.IsItemEquipped(item.ItemId) && item.PrefabName != "LutzDefaultGearHead" && item.PrefabName != "LutzDefaultGearGloves" && item.PrefabName != "LutzDefaultGearUpperBody" && item.PrefabName != "LutzDefaultGearLowerBody" && item.PrefabName != "LutzDefaultGearBoots";
}
