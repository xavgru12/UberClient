// Decompiled with JetBrains decompiler
// Type: BuyPanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.Core.Models.Views;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class BuyPanelGUI : PanelGuiBase
{
  private const int WIDTH = 300;
  private const int BORDER = 10;
  private const int TITLE_HEIGHT = 100;
  private int Height;
  private IUnityItem _item;
  private ItemPriceGUI _price;
  private bool _autoEquip;
  private static bool _isBuyingItem;
  private Texture _priceIcon;
  private string _priceTag;
  private BuyingLocationType _buyingLocation;
  private BuyingRecommendationType _buyingRecommendation;

  private void OnGUI()
  {
    GUI.skin = BlueStonez.Skin;
    GUI.depth = 3;
    this.Height = 100 + this._price.Height + 100;
    this.DrawUnityItem(new Rect((float) ((Screen.width - 300) / 2), (float) ((Screen.height - this.Height) / 2), 300f, (float) this.Height));
    GuiManager.DrawTooltip();
  }

  private void DrawUnityItem(Rect rect)
  {
    GUI.BeginGroup(rect, GUIContent.none, BlueStonez.window_standard_grey38);
    int num = 20;
    if (ApplicationDataManager.Channel == ChannelType.Android || ApplicationDataManager.Channel == ChannelType.IPad || ApplicationDataManager.Channel == ChannelType.IPhone)
      num = 45;
    if (GUI.Button(new Rect(rect.width - (float) num, 0.0f, (float) num, (float) num), "X", BlueStonez.friends_hidden_button))
      this.Hide();
    this.DrawTitle(new Rect(10f, 10f, rect.width - 20f, 100f));
    this.DrawPrice(new Rect(30f, 110f, rect.width - 60f, rect.height - 100f));
    this.DrawBuyButton(new Rect(0.0f, rect.height - 90f, rect.width, 90f));
    GUI.EndGroup();
    if (Event.current.type != UnityEngine.EventType.MouseDown || rect.Contains(Event.current.mousePosition))
      return;
    this.Hide();
    Event.current.Use();
  }

  private void DrawTitle(Rect rect)
  {
    GUI.BeginGroup(rect);
    GUI.Label(new Rect(0.0f, 0.0f, 48f, 48f), (Texture) this._item.Icon, BlueStonez.item_slot_large);
    GUI.Label(new Rect(58f, 0.0f, (float) ((double) rect.width - 48.0 - 20.0 - 32.0), 48f), this._item.Name, BlueStonez.label_interparkmed_18pt_left_wrap);
    if (this._item.ItemView.LevelLock > PlayerDataManager.PlayerLevel)
    {
      GUI.color = new Color(1f, 1f, 1f, 0.5f);
      int num = 0;
      if (ApplicationDataManager.Channel == ChannelType.Android || ApplicationDataManager.Channel == ChannelType.IPad || ApplicationDataManager.Channel == ChannelType.IPhone)
        num = 25;
      GUI.Label(new Rect((float) ((double) rect.width - 10.0 - 32.0) - (float) num, 8f, 32f, 32f), (Texture) ShopIcons.BlankItemFrame);
      GUI.Label(new Rect((float) ((double) rect.width - 10.0 - 31.0) - (float) num, 16f, 24f, 24f), this._item.ItemView.LevelLock.ToString(), BlueStonez.label_interparkmed_11pt);
      GUI.color = Color.white;
    }
    GUI.Label(new Rect(0.0f, 58f, rect.width, (float) ((double) rect.height - 48.0 - 10.0)), this._item.ItemView.Description, BlueStonez.label_itemdescription);
    GUI.EndGroup();
  }

  private void DrawPrice(Rect rect) => this._price.Draw(rect);

  private void DrawBuyButton(Rect rect)
  {
    GUI.BeginGroup(rect);
    Rect position = new Rect((float) (((double) rect.width - 120.0) / 2.0), (float) (((double) rect.height - 30.0) / 2.0), 120f, 30f);
    GUITools.PushGUIState();
    GUI.enabled = !BuyPanelGUI._isBuyingItem && this._price.SelectedPriceOption != null;
    if (GUI.Button(position, GUIContent.none, BlueStonez.buttongold_large) && !BuyPanelGUI._isBuyingItem)
    {
      BuyPanelGUI._isBuyingItem = true;
      BuyPanelGUI.BuyItem(this._item, this._price.SelectedPriceOption, this._buyingLocation, this._buyingRecommendation, this._autoEquip);
    }
    GUITools.PopGUIState();
    GUI.Label(new Rect((float) (((double) rect.width - 120.0) / 2.0), (float) (((double) rect.height - 20.0) / 2.0), 120f, 20f), new GUIContent(this._priceTag, this._priceIcon), BlueStonez.label_interparkbold_13pt_black);
    GUI.EndGroup();
  }

  private void OnPriceOptionSelected(ItemPrice price)
  {
    this._priceTag = price.Price != 0 ? string.Format("{0:N0}", (object) price.Price) : "FREE";
    this._priceIcon = price.Currency != UberStrikeCurrencyType.Points ? (Texture) ShopIcons.IconCredits20x20 : (Texture) ShopIcons.IconPoints20x20;
  }

  public static void BuyItem(
    IUnityItem item,
    ItemPrice price,
    BuyingLocationType buyingLocation = BuyingLocationType.Shop,
    BuyingRecommendationType recommendation = BuyingRecommendationType.Manual,
    bool autoEquip = false)
  {
    if (item.ItemView.IsConsumable)
      ShopWebServiceClient.BuyPack(item.ItemId, PlayerDataManager.CmidSecure, price.PackType, price.Currency, item.ItemType, buyingLocation, recommendation, (Action<int>) (result => BuyPanelGUI.HandleBuyItem(item, result, autoEquip)), (Action<Exception>) (ex =>
      {
        BuyPanelGUI._isBuyingItem = false;
        PanelManager.Instance.ClosePanel(PanelType.BuyItem);
        DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please check your internet connection and try again.");
      }));
    else
      ShopWebServiceClient.BuyItem(item.ItemId, PlayerDataManager.CmidSecure, price.Currency, price.Duration, item.ItemType, buyingLocation, recommendation, (Action<int>) (result => BuyPanelGUI.HandleBuyItem(item, result, autoEquip)), (Action<Exception>) (ex =>
      {
        BuyPanelGUI._isBuyingItem = false;
        PanelManager.Instance.ClosePanel(PanelType.BuyItem);
        DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please check your internet connection and try again.");
      }));
  }

  private static void HandleBuyItem(IUnityItem item, int result, bool autoEquip)
  {
    BuyPanelGUI._isBuyingItem = false;
    CmuneEventHandler.Route((object) new BuyItemEvent()
    {
      Result = result
    });
    int num = result;
    switch (num)
    {
      case 0:
        MonoRoutine.Start(Singleton<InventoryManager>.Instance.StartUpdateInventoryAndEquipNewItem(item.ItemId, autoEquip));
        break;
      case 1:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemCannotBeRented, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      case 3:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemCannotBeRented, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      case 4:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemCannotBePurchasedPermanently, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      case 5:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemCannotBePurchasedForDuration, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      case 6:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisPackIsDisabled, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      case 7:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemIsNotForSale, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      case 8:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.YouDontHaveEnoughPointsOrCreditsToPurchaseThisItem, PopupSystem.AlertType.OKCancel, new Action(BuyPanelGUI.HandleWebServiceError), LocalizedStrings.OkCaps, new Action(ApplicationDataManager.OpenBuyCredits), "GET CREDITS");
        break;
      case 9:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.AccountIsInvalid, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      case 10:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, string.Format(LocalizedStrings.YouCannotPurchaseThisItemForMoreThanNDays, (object) 90.ToString()), PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      case 11:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.YouAlreadyOwnThisItem, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      case 12:
        int maxOwnableAmount = (item.ItemView as UberStrikeItemQuickView).MaxOwnableAmount;
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, string.Format(LocalizedStrings.TheAmountYouTriedToPurchaseIsInvalid, (object) maxOwnableAmount), PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      case 13:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.ThisItemIsOutOfStock, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      case 14:
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.InvalidData, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
      default:
        if (num == 100)
        {
          PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.YourLevelIsTooLowToBuyThisItem, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
          break;
        }
        PopupSystem.ShowMessage(LocalizedStrings.ProblemBuyingItem, LocalizedStrings.DataError, PopupSystem.AlertType.OK, new Action(BuyPanelGUI.HandleWebServiceError));
        break;
    }
    PanelManager.Instance.ClosePanel(PanelType.BuyItem);
  }

  private static void HandleWebServiceError()
  {
  }

  public void SetItem(
    IUnityItem item,
    BuyingLocationType location,
    BuyingRecommendationType recommendation,
    bool autoEquip = false)
  {
    this._autoEquip = autoEquip;
    this._item = item;
    this._buyingLocation = location;
    this._buyingRecommendation = recommendation;
    BuyPanelGUI._isBuyingItem = false;
    if (item != null && item.ItemView.Prices.Count > 0)
    {
      if (item.ItemView.IsConsumable)
        this._price = (ItemPriceGUI) new PackItemPriceGUI(item, new Action<ItemPrice>(this.OnPriceOptionSelected));
      else
        this._price = (ItemPriceGUI) new RentItemPriceGUI(item, new Action<ItemPrice>(this.OnPriceOptionSelected));
    }
    else
      Debug.LogError((object) "Item is null or not for sale");
  }
}
