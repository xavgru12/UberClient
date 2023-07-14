// Decompiled with JetBrains decompiler
// Type: ShopRefreshCurrentItemListEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Types;

public class ShopRefreshCurrentItemListEvent
{
  public ShopRefreshCurrentItemListEvent() => this.UseCurrentSelection = true;

  public ShopRefreshCurrentItemListEvent(UberstrikeItemClass itemClass, UberstrikeItemType itemType)
  {
    this.UseCurrentSelection = false;
    this.ItemClass = itemClass;
    this.ItemType = itemType;
  }

  public bool UseCurrentSelection { get; private set; }

  public UberstrikeItemClass ItemClass { get; private set; }

  public UberstrikeItemType ItemType { get; private set; }
}
