// Decompiled with JetBrains decompiler
// Type: BaseItemGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public abstract class BaseItemGUI
{
  private int _armorPoints;
  private string _description = "No description available.";
  private BuyingLocationType _location;
  private BuyingRecommendationType _recommendation;

  public BaseItemGUI(
    IUnityItem item,
    BuyingLocationType location,
    BuyingRecommendationType recommendation)
  {
    this._location = location;
    this._recommendation = recommendation;
    if (item != null)
    {
      this.Item = item;
      if (this.Item.ItemType == UberstrikeItemType.Weapon)
        this.DetailGUI = (IBaseItemDetailGUI) new WeaponItemDetailGUI(item as WeaponItem);
      else if (this.Item.ItemClass == UberstrikeItemClass.GearUpperBody || this.Item.ItemClass == UberstrikeItemClass.GearLowerBody)
      {
        this._armorPoints = ((GearItem) item).Configuration.ArmorPoints;
        this.DetailGUI = (IBaseItemDetailGUI) new ArmorItemDetailGUI(item as GearItem, ShopIcons.ItemarmorpointsIcon);
      }
      if (this.Item.ItemView == null || string.IsNullOrEmpty(this.Item.ItemView.Description))
        return;
      this._description = this.Item.ItemView.Description;
    }
    else
    {
      this.Item = (IUnityItem) new BaseItemGUI.NullItem();
      Debug.LogError((object) "BaseItemGUI creation failed because item is NULL");
    }
  }

  public IUnityItem Item { get; private set; }

  public IBaseItemDetailGUI DetailGUI { get; private set; }

  public abstract void Draw(Rect rect, bool selected);

  public void DrawIcon(Rect rect) => GUI.Label(rect, (Texture) this.Item.Icon, BlueStonez.item_slot_small);

  public void DrawName(Rect rect)
  {
    if (string.IsNullOrEmpty(this.Item.Name))
      return;
    GUI.Label(rect, this.Item.Name, BlueStonez.label_interparkbold_11pt_left_wrap);
  }

  public void DrawHintArrow(Rect rect)
  {
    if (!rect.Contains(Event.current.mousePosition))
      return;
    GUI.color = new Color(1f, 1f, 1f, 0.1f);
    GUI.Label(new Rect((float) ((double) rect.width / 2.0 - 16.0), rect.yMin, (float) ShopIcons.ArrowBigShop.width, (float) ShopIcons.ArrowBigShop.height), (Texture) ShopIcons.ArrowBigShop, GUIStyle.none);
    GUI.color = new Color(1f, 1f, 1f, 1f);
  }

  public void DrawArmorOverlay()
  {
    if (this.Item.ItemClass != UberstrikeItemClass.GearUpperBody && this.Item.ItemClass != UberstrikeItemClass.GearLowerBody)
      return;
    if (this._armorPoints > 0)
      GUI.DrawTexture(new Rect(4f, 35f, 16f, 16f), (Texture) ShopIcons.ItemarmorpointsIcon);
    if (this._armorPoints > 15)
      GUI.DrawTexture(new Rect(8f, 35f, 16f, 16f), (Texture) ShopIcons.ItemarmorpointsIcon);
    if (this._armorPoints > 30)
      GUI.DrawTexture(new Rect(12f, 35f, 16f, 16f), (Texture) ShopIcons.ItemarmorpointsIcon);
    if (this._armorPoints <= 45)
      return;
    GUI.DrawTexture(new Rect(16f, 35f, 16f, 16f), (Texture) ShopIcons.ItemarmorpointsIcon);
  }

  public void DrawPromotionalTag()
  {
    if (this.Item.ItemView == null)
      return;
    switch (this.Item.ItemView.ShopHighlightType)
    {
      case ItemShopHighlightType.Featured:
        GUI.DrawTexture(new Rect(0.0f, -3f, 32f, 32f), (Texture) ShopIcons.Sale);
        break;
      case ItemShopHighlightType.Popular:
        GUI.DrawTexture(new Rect(0.0f, -3f, 32f, 32f), (Texture) ShopIcons.Hot);
        break;
      case ItemShopHighlightType.New:
        GUI.DrawTexture(new Rect(0.0f, -3f, 32f, 32f), (Texture) ShopIcons.New);
        break;
    }
  }

  public void DrawClassIcon()
  {
    GUI.color = new Color(1f, 1f, 1f, 0.5f);
    if (this.Item.ItemType != UberstrikeItemType.Weapon && this.Item.ItemType != UberstrikeItemType.Gear)
      return;
    GUI.DrawTexture(new Rect(54f, 4f, 24f, 24f), (Texture) UberstrikeIconsHelper.GetIconForItemClass(this.Item.ItemClass));
  }

  public void DrawLevelRequirement()
  {
    if (this.Item.ItemView == null)
      return;
    GUI.color = this.Item.ItemView.LevelLock <= 1 ? new Color(1f, 1f, 1f, 0.2f) : new Color(1f, 1f, 1f, 0.5f);
    GUI.DrawTexture(new Rect(54f, 29f, 24f, 24f), (Texture) ShopIcons.BlankItemFrame);
    if (this.Item.ItemView.LevelLock > 1)
      GUI.Label(new Rect(54f, 33f, 24f, 24f), this.Item.ItemView.LevelLock.ToString(), BlueStonez.label_interparkbold_11pt);
    GUI.color = Color.white;
  }

  protected void DrawPrice(Rect rect, ItemPrice points, ItemPrice credits)
  {
    float num = 0.0f;
    if (points != null)
    {
      string text = string.Format("{0}", points.Price != 0 ? (object) points.Price.ToString("N0") : (object) "FREE");
      GUI.DrawTexture(new Rect(rect.x, rect.y, 16f, 16f), (Texture) ShopUtils.CurrencyIcon(points.Currency));
      GUI.Label(new Rect(rect.x + 20f, rect.y + 3f, rect.width - 20f, 16f), text, BlueStonez.label_interparkmed_11pt_left);
      num += 40f + BlueStonez.label_interparkmed_11pt_left.CalcSize(new GUIContent(text)).x;
    }
    if (credits == null)
      return;
    string text1 = string.Format("{0}", credits.Price != 0 ? (object) credits.Price.ToString("N0") : (object) "FREE");
    if ((double) num > 0.0)
      GUI.Label(new Rect((float) ((double) rect.x + (double) num - 10.0), rect.y + 3f, 10f, 16f), "/", BlueStonez.label_interparkmed_11pt_left);
    GUI.DrawTexture(new Rect(rect.x + num, rect.y, 16f, 16f), (Texture) ShopUtils.CurrencyIcon(credits.Currency));
    GUI.Label(new Rect((float) ((double) rect.x + (double) num + 20.0), rect.y + 3f, rect.width - 20f, 16f), text1, BlueStonez.label_interparkmed_11pt_left);
  }

  protected void DrawRecommendPrice(Rect rect, ItemPrice price)
  {
    if (price == null)
      return;
    string text = price.Price.ToString("N0");
    Vector2 vector2 = BlueStonez.label_interparkmed_11pt_left.CalcSize(new GUIContent(text));
    GUI.Label(new Rect((float) ((double) rect.width - (double) vector2.x - (double) sbyte.MaxValue), rect.y + 1f, 100f, 16f), "FROM", BlueStonez.label_interparkbold_11pt_right);
    GUI.DrawTexture(new Rect((float) ((double) rect.width - (double) vector2.x - 20.0), rect.y + 1f, 16f, 16f), (Texture) ShopUtils.CurrencyIcon(price.Currency));
    GUI.Label(new Rect(rect.width - vector2.x, rect.y + 1f, vector2.x, 16f), text, BlueStonez.label_interparkmed_11pt_right);
  }

  public void DrawEquipButton(Rect rect, string content)
  {
    if (this.Item.ItemType != UberstrikeItemType.Weapon && this.Item.ItemType != UberstrikeItemType.Gear && this.Item.ItemType != UberstrikeItemType.QuickUse || !GUI.Button(rect, new GUIContent(content), BlueStonez.buttondark_medium) || this.Item == null)
      return;
    switch (this.Item.ItemType)
    {
      case UberstrikeItemType.Weapon:
        CmuneEventHandler.Route((object) new SelectLoadoutAreaEvent()
        {
          Area = LoadoutArea.Weapons
        });
        break;
      case UberstrikeItemType.Gear:
        CmuneEventHandler.Route((object) new SelectLoadoutAreaEvent()
        {
          Area = LoadoutArea.Gear
        });
        break;
      case UberstrikeItemType.QuickUse:
        CmuneEventHandler.Route((object) new SelectLoadoutAreaEvent()
        {
          Area = LoadoutArea.QuickItems
        });
        break;
    }
    if (Singleton<InventoryManager>.Instance.EquipItem(this.Item.ItemId))
    {
      CmuneEventHandler.Route((object) new UpdateRecommendationEvent());
    }
    else
    {
      BuyPanelGUI buyPanelGui = PanelManager.Instance.OpenPanel(PanelType.BuyItem) as BuyPanelGUI;
      if (!(bool) (Object) buyPanelGui)
        return;
      buyPanelGui.SetItem(this.Item, this._location, this._recommendation);
    }
  }

  public void DrawTryButton(Rect position)
  {
    if (!GUI.Button(position, new GUIContent(LocalizedStrings.Try), BlueStonez.buttondark_medium))
      return;
    CmuneEventHandler.Route((object) new ShopTryEvent()
    {
      Item = this.Item
    });
  }

  public void DrawBuyButton(Rect position, string text, ShopArea area = ShopArea.Shop)
  {
    GUI.contentColor = ColorScheme.UberStrikeYellow;
    if (GUITools.Button(position, new GUIContent(text), BlueStonez.buttondark_medium))
    {
      BuyPanelGUI buyPanelGui = PanelManager.Instance.OpenPanel(PanelType.BuyItem) as BuyPanelGUI;
      if ((bool) (Object) buyPanelGui)
        buyPanelGui.SetItem(this.Item, this._location, this._recommendation);
    }
    GUI.contentColor = Color.white;
  }

  public void DrawGrayLine(Rect position) => GUI.Label(new Rect(4f, position.height - 1f, position.width - 4f, 1f), string.Empty, BlueStonez.horizontal_line_grey95);

  public void DrawDescription(Rect position) => GUI.Label(position, this._description, BlueStonez.label_itemdescription);

  public void DrawUseButton(Rect position)
  {
    if (!GUITools.Button(position, new GUIContent("Use"), BlueStonez.buttondark_medium))
      return;
    PanelManager.Instance.OpenPanel(PanelType.NameChange);
  }

  private class NullItem : IUnityItem
  {
    public Texture2D Icon { get; set; }

    public int ItemId { get; set; }

    public string Name => "Unsupported Item";

    public UberstrikeItemType ItemType => (UberstrikeItemType) 0;

    public UberstrikeItemClass ItemClass => (UberstrikeItemClass) 0;

    public BaseUberStrikeItemView ItemView { get; private set; }

    public MonoBehaviour Prefab => (MonoBehaviour) null;

    public string PrefabName => string.Empty;
  }
}
