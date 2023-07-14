// Decompiled with JetBrains decompiler
// Type: ItemByTypeFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Types;

public class ItemByTypeFilter : IShopItemFilter
{
  private UberstrikeItemType _itemType;

  public ItemByTypeFilter(UberstrikeItemType itemType) => this._itemType = itemType;

  public bool CanPass(IUnityItem item) => item.ItemType == this._itemType;
}
