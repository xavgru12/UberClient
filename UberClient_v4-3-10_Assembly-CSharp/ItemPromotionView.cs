// Decompiled with JetBrains decompiler
// Type: ItemPromotionView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.DataCenter.Common.Entities;

public class ItemPromotionView
{
  public ItemPromotionView(WeeklySpecialView view)
  {
    this.Texture = new DynamicTexture(view.ImageUrl);
    this.Title = view.Title;
    this.ItemGui = new PromotionItemGUI(Singleton<ItemManager>.Instance.GetItemInShop(view.ItemId), BuyingLocationType.HomeScreen);
  }

  public PromotionItemGUI ItemGui { get; private set; }

  public DynamicTexture Texture { get; private set; }

  public string Title { get; private set; }
}
