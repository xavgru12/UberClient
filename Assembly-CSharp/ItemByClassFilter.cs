// Decompiled with JetBrains decompiler
// Type: ItemByClassFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Types;

public class ItemByClassFilter : IShopItemFilter
{
  private UberstrikeItemType _itemType;
  private UberstrikeItemClass _itemClass;

  public ItemByClassFilter(UberstrikeItemType itemType, UberstrikeItemClass itemClass)
  {
    this._itemType = itemType;
    this._itemClass = itemClass;
  }

  public bool CanPass(IUnityItem item) => item.ItemType == this._itemType && item.ItemClass == this._itemClass;
}
