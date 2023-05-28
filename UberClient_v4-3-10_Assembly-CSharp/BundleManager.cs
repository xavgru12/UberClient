// Decompiled with JetBrains decompiler
// Type: BundleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using UberStrike.WebService.Unity;
using UnityEngine;

public class BundleManager : Singleton<BundleManager>
{
  private const string AndroidBillingPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAk70vgr/huuLCYSWpKwJQ/jocRi3AI+GD4oTKKnJdNasrcYt/nP2JzYOWz71ViN4HGgyyhJbgiQmr9cGkKAFxeEz5qirFqAbJuj/F9ZORI9ixdfKbWag1tsDvLtktvIcqdKbhORk0vX7s3/kmUFk1/xFZkILCboePXmCJQZZCn8zwr+IpITdSznDDcPYZb9YmrqYD1WtpwXPw/9RDlpGkJixpOzJPJbRAObOJSDQoQh0A0lKGUmUgkMsRWNJr3j5XYFL0HASlNMKH/bNhHT+lhiHKG+5QSkcTcE+rOMMFXl9EqtD8VLXGMJRtf81eAKVzeAYKX/sxNHBauzD5LwT4jwIDAQAB";
  private BasePopupDialog _appStorePopup;
  private Dictionary<BundleCategoryType, List<BundleUnityView>> _bundlesPerCategory;
  private string _lastErrorTransaction;
  private string _lastErrorBundleId;
  private float dialogTimer;

  private BundleManager() => this._bundlesPerCategory = new Dictionary<BundleCategoryType, List<BundleUnityView>>();

  public int Count { get; private set; }

  public bool CanMakeMasPayments { get; private set; }

  public bool WaitingForBundles { get; private set; }

  public List<BundleUnityView> GetCreditBundles()
  {
    List<BundleUnityView> bundleUnityViewList;
    return this._bundlesPerCategory.TryGetValue(BundleCategoryType.None, out bundleUnityViewList) ? bundleUnityViewList : new List<BundleUnityView>(0);
  }

  public List<BundleUnityView> GetBundlesInCategory(BundleCategoryType category)
  {
    List<BundleUnityView> bundleUnityViewList;
    return this._bundlesPerCategory.TryGetValue(category, out bundleUnityViewList) ? bundleUnityViewList : new List<BundleUnityView>(0);
  }

  public IEnumerable<BundleUnityView> AllItemBundles
  {
    get
    {
      BundleManager.\u003C\u003Ec__Iterator64 allItemBundles = new BundleManager.\u003C\u003Ec__Iterator64()
      {
        \u003C\u003Ef__this = this
      };
      allItemBundles.\u0024PC = -2;
      return (IEnumerable<BundleUnityView>) allItemBundles;
    }
  }

  public IEnumerable<BundleUnityView> AllBundles
  {
    get
    {
      BundleManager.\u003C\u003Ec__Iterator65 allBundles = new BundleManager.\u003C\u003Ec__Iterator65()
      {
        \u003C\u003Ef__this = this
      };
      allBundles.\u0024PC = -2;
      return (IEnumerable<BundleUnityView>) allBundles;
    }
  }

  public void Initialize()
  {
    this.WaitingForBundles = true;
    ShopWebServiceClient.GetBundles(ApplicationDataManager.Channel, (Action<List<BundleView>>) (bundles => this.SetBundles(bundles)), (Action<Exception>) (exception => UnityEngine.Debug.LogError((object) ("Error getting " + (object) ApplicationDataManager.Channel + " bundles from the server."))));
  }

  private void SetBundles(List<BundleView> bundleViews)
  {
    if (bundleViews != null && bundleViews.Count > 0)
    {
      foreach (BundleView bundleView in bundleViews)
      {
        if ((ApplicationDataManager.Channel != ChannelType.MacAppStore || !string.IsNullOrEmpty(bundleView.MacAppStoreUniqueId)) && (ApplicationDataManager.Channel != ChannelType.IPad || !string.IsNullOrEmpty(bundleView.IosAppStoreUniqueId)) && (ApplicationDataManager.Channel != ChannelType.IPhone || !string.IsNullOrEmpty(bundleView.IosAppStoreUniqueId)) && (ApplicationDataManager.Channel != ChannelType.Android || !string.IsNullOrEmpty(bundleView.AndroidStoreUniqueId)))
        {
          List<BundleUnityView> bundleUnityViewList;
          if (!this._bundlesPerCategory.TryGetValue(bundleView.Category, out bundleUnityViewList))
          {
            bundleUnityViewList = new List<BundleUnityView>();
            this._bundlesPerCategory[bundleView.Category] = bundleUnityViewList;
          }
          bundleUnityViewList.Add(new BundleUnityView(bundleView));
        }
      }
      this.GetStoreKitProductData();
    }
    else
      UnityEngine.Debug.LogError((object) "SetBundles: Bundles received from the server were null or empty!");
  }

  [DebuggerHidden]
  public IEnumerator StartCancelDialogTimer() => (IEnumerator) new BundleManager.\u003CStartCancelDialogTimer\u003Ec__Iterator66()
  {
    \u003C\u003Ef__this = this
  };

  public void BuyStoreKitItem(BundleUnityView bundle)
  {
    this._appStorePopup = PopupSystem.ShowMessage("In App Purchase", "Opening the Store, please wait...", PopupSystem.AlertType.None) as BasePopupDialog;
    MonoRoutine.Start(this.StartCancelDialogTimer());
  }

  public void BuyFacebookBundle(int bundleId)
  {
    this._appStorePopup = PopupSystem.ShowMessage("Facebook Purchase", "Opening Facebook Credits, please wait...", PopupSystem.AlertType.None) as BasePopupDialog;
    MonoRoutine.Start(this.StartCancelDialogTimer());
    Application.ExternalCall("usHelper.purchase", (object) bundleId);
  }

  public void OnFacebookPayment(string status)
  {
    if (this._appStorePopup != null)
      PopupSystem.HideMessage((IPopupDialog) this._appStorePopup);
    string key = status;
    if (key == null)
      return;
    // ISSUE: reference to a compiler-generated field
    if (BundleManager.\u003C\u003Ef__switch\u0024map1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BundleManager.\u003C\u003Ef__switch\u0024map1 = new Dictionary<string, int>(3)
      {
        {
          "success",
          0
        },
        {
          "fail",
          1
        },
        {
          "cancel",
          2
        }
      };
    }
    int num;
    // ISSUE: reference to a compiler-generated field
    if (!BundleManager.\u003C\u003Ef__switch\u0024map1.TryGetValue(key, out num))
      return;
    switch (num)
    {
      case 0:
        PopupSystem.ShowMessage("Purchase Successful", "Thank you, your purchase was successful.", PopupSystem.AlertType.OK, (Action) (() => ApplicationDataManager.RefreshWallet()));
        break;
      case 1:
        PopupSystem.ShowMessage("Purchase Failed", "Sorry, there was a problem processing your payment. Please visit support.uberstrike.com for help.", PopupSystem.AlertType.OK);
        break;
      case 2:
        UnityEngine.Debug.Log((object) "Facebook payment cancelled.");
        break;
    }
  }

  private void AndroidBillingSupportedEvent(bool supported)
  {
    this.CanMakeMasPayments = supported;
    this.Count = this.AllBundles.Count<BundleUnityView>();
    this.WaitingForBundles = false;
  }

  private void GetStoreKitProductData()
  {
    this.Count = 0;
    foreach (BundleUnityView allBundle in this.AllBundles)
    {
      allBundle.CurrencySymbol = "FB";
      allBundle.Price = (allBundle.BundleView.USDPrice * 10M).ToString("N0");
      allBundle.IsOwned = this.IsItemPackOwned(allBundle.BundleView.BundleItemViews);
      ++this.Count;
    }
    this.WaitingForBundles = false;
  }

  private void BuyBundle(BundleUnityView bundle, string transactionIdentifier) => MonoRoutine.Start(this.StartBuyBundle(transactionIdentifier, bundle));

  [DebuggerHidden]
  private IEnumerator StartBuyBundle(string transactionIdentifier, BundleUnityView bundle) => (IEnumerator) new BundleManager.\u003CStartBuyBundle\u003Ec__Iterator67()
  {
    bundle = bundle,
    transactionIdentifier = transactionIdentifier,
    \u003C\u0024\u003Ebundle = bundle,
    \u003C\u0024\u003EtransactionIdentifier = transactionIdentifier,
    \u003C\u003Ef__this = this
  };

  private void ShowSupportPortal()
  {
    string url = string.Format("http://support.uberstrike.com/customer/widget/emails/new?t=177975&interaction[name]={0}&email[subject]=Bundle%20Purchase%20failed&email[body]=Attempt%20to%20purchase%20bundle%20failed.%20Channel:{1}%20Identifier:{2}%20Receipt:{3}", (object) WWW.EscapeURL(PlayerDataManager.Name), (object) WWW.EscapeURL(((Enum) ApplicationDataManager.Channel).ToString()), (object) WWW.EscapeURL(this._lastErrorBundleId), (object) WWW.EscapeURL(this._lastErrorTransaction));
    ApplicationDataManager.OpenUrl(string.Empty, url);
  }

  private void OnBundlePurchased(BundleUnityView bundle)
  {
    if (bundle.BundleView.Credits > 0 || bundle.BundleView.Points > 0)
    {
      ApplicationDataManager.RefreshWallet();
    }
    else
    {
      List<IUnityItem> items = new List<IUnityItem>(8);
      for (int index = 0; index < bundle.BundleView.BundleItemViews.Count && index < 8; ++index)
        items.Add(Singleton<ItemManager>.Instance.GetItemInShop(bundle.BundleView.BundleItemViews[index].ItemId));
      PopupSystem.ShowItems("Purchase Successful", "New Items have been added to your inventory!", items, ShopArea.Inventory);
      UserWebServiceClient.GetInventory(PlayerDataManager.CmidSecure, (Action<List<ItemInventoryView>>) (inventory =>
      {
        Singleton<InventoryManager>.Instance.UpdateInventoryItems(inventory);
        foreach (BundleUnityView allBundle in this.AllBundles)
          allBundle.IsOwned = this.IsItemPackOwned(allBundle.BundleView.BundleItemViews);
      }), (Action<Exception>) (exception => UnityEngine.Debug.LogError((object) ("Exception getting inventory: " + exception.Message))));
    }
  }

  private bool IsItemPackOwned(List<BundleItemView> items)
  {
    if (items.Count <= 0)
      return false;
    foreach (BundleItemView bundleItemView in items)
    {
      if (!Singleton<InventoryManager>.Instance.IsItemInInventory(bundleItemView.ItemId))
        return false;
    }
    return true;
  }

  public BundleUnityView GetNextItem(BundleUnityView currentItem)
  {
    List<BundleUnityView> bundleUnityViewList = new List<BundleUnityView>(this.AllItemBundles);
    if (bundleUnityViewList.Count <= 0)
      return currentItem;
    int index1 = bundleUnityViewList.FindIndex((Predicate<BundleUnityView>) (i => i == currentItem));
    if (index1 < 0)
      return bundleUnityViewList[UnityEngine.Random.Range(0, bundleUnityViewList.Count)];
    int index2 = (index1 + 1) % bundleUnityViewList.Count;
    return bundleUnityViewList[index2];
  }

  public BundleUnityView GetPreviousItem(BundleUnityView currentItem)
  {
    List<BundleUnityView> bundleUnityViewList = new List<BundleUnityView>(this.AllItemBundles);
    if (bundleUnityViewList.Count <= 0)
      return currentItem;
    int index1 = bundleUnityViewList.FindIndex((Predicate<BundleUnityView>) (i => i == currentItem));
    if (index1 < 0)
      return bundleUnityViewList[UnityEngine.Random.Range(0, bundleUnityViewList.Count)];
    int index2 = (index1 - 1 + bundleUnityViewList.Count) % bundleUnityViewList.Count;
    return bundleUnityViewList[index2];
  }

  private void OnStoreKitPurchaseFailed(string error)
  {
    if (this._appStorePopup != null)
      PopupSystem.HideMessage((IPopupDialog) this._appStorePopup);
    PopupSystem.ShowMessage("Purchase Failed", "Sorry, it seems your purchase failed.\n Please visit support.uberstrike.com");
  }

  private void AndroidPurchaseFailedEvent(string arg1, string arg2)
  {
    if (this._appStorePopup != null)
      PopupSystem.HideMessage((IPopupDialog) this._appStorePopup);
    PopupSystem.ShowMessage("Purchase Failed", "Sorry, it seems your purchase failed.\n Please visit support.uberstrike.com");
  }

  private void OnStoreKitPurchaseCancelled(string error)
  {
    if (this._appStorePopup != null)
      PopupSystem.HideMessage((IPopupDialog) this._appStorePopup);
    PopupSystem.ShowMessage("Purchase Cancelled", "Your purchase was cancelled.");
  }

  private void AndroidPurchaseCancelledEvent(string arg1, string arg2)
  {
    if (this._appStorePopup != null)
      PopupSystem.HideMessage((IPopupDialog) this._appStorePopup);
    PopupSystem.ShowMessage("Purchase Cancelled", "Your purchase was cancelled.");
  }

  private void OnStoreKitPurchaseSuccessful(StoreKitTransaction transaction)
  {
    if (this._appStorePopup != null)
      PopupSystem.HideMessage((IPopupDialog) this._appStorePopup);
    string empty = string.Empty;
    UnityEngine.Debug.Log((object) string.Format("OnStoreKitPurchaseSuccessful: ProductIdenitifier={0} Receipt={1} Quantity={2}", (object) transaction.productIdentifier, (object) empty, (object) transaction.quantity));
    BundleUnityView bundle = (BundleUnityView) null;
    if (bundle != null)
      this.BuyBundle(bundle, empty);
    else
      UnityEngine.Debug.LogError((object) ("No MasBundle found with ProductIdentifier: " + transaction.productIdentifier));
  }

  private void AndroidPurchaseSucceededEvent(string itemId, string payload)
  {
    if (this._appStorePopup != null)
      PopupSystem.HideMessage((IPopupDialog) this._appStorePopup);
    string empty = string.Empty;
    UnityEngine.Debug.Log((object) string.Format("OnAndroidPurchaseSuccessful: ProductIdenitifier={0}", (object) itemId, (object) empty));
    BundleUnityView bundle = this.AllBundles.FirstOrDefault<BundleUnityView>((Func<BundleUnityView, bool>) (p => p.BundleView.AndroidStoreUniqueId == itemId));
    if (bundle != null)
      this.BuyBundle(bundle, Guid.NewGuid().ToString());
    else
      UnityEngine.Debug.LogError((object) ("No AndroidBundle found with ProductIdentifier: " + itemId));
  }

  private void OnStoreKitProductListRequestFailed(string error) => UnityEngine.Debug.LogError((object) ("Error Getting Store Kit Product List (" + error + ")"));

  private void OnStoreKitProductListReceived(List<StoreKitProduct> productList)
  {
    List<BundleUnityView> bundleUnityViewList1 = new List<BundleUnityView>();
    foreach (BundleUnityView allBundle in this.AllBundles)
    {
      this.Count = 0;
      StoreKitProduct storeKitProduct = (StoreKitProduct) null;
      if (storeKitProduct != null)
      {
        allBundle.CurrencySymbol = storeKitProduct.currencySymbol;
        allBundle.Price = storeKitProduct.price;
        allBundle.IsOwned = this.IsItemPackOwned(allBundle.BundleView.BundleItemViews);
        ++this.Count;
      }
      else
      {
        allBundle.Price = string.Empty;
        bundleUnityViewList1.Add(allBundle);
      }
    }
    foreach (BundleUnityView bundleUnityView in bundleUnityViewList1)
    {
      foreach (List<BundleUnityView> bundleUnityViewList2 in this._bundlesPerCategory.Values)
      {
        if (bundleUnityViewList2.Contains(bundleUnityView))
        {
          bundleUnityViewList2.Remove(bundleUnityView);
          break;
        }
      }
    }
    this.WaitingForBundles = false;
  }

  private List<BundleManager.InAppPurchase> GetInAppPurchases(
    List<KeyValuePair<string, string>> receiptList)
  {
    List<BundleManager.InAppPurchase> inAppPurchases = new List<BundleManager.InAppPurchase>();
    if (receiptList.Exists((Predicate<KeyValuePair<string, string>>) (p => p.Key == "InApp")))
    {
      foreach (KeyValuePair<string, string> keyValuePair in receiptList.FindAll((Predicate<KeyValuePair<string, string>>) (p => p.Key == "InApp")))
        inAppPurchases.Add(this.ParseInAppPurchase(keyValuePair.Value));
    }
    return inAppPurchases;
  }

  private List<KeyValuePair<string, string>> ParseXmlReceipt(string receiptXml)
  {
    List<KeyValuePair<string, string>> xmlReceipt = new List<KeyValuePair<string, string>>();
    if (!string.IsNullOrEmpty(receiptXml))
    {
      if (!receiptXml.Contains("<MASRECEIPT>Invalid</MASRECEIPT>"))
      {
        try
        {
          XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(receiptXml));
          while (xmlReader.Read())
          {
            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name != "MASRECEIPT")
              xmlReceipt.Add(new KeyValuePair<string, string>(xmlReader.Name, xmlReader.ReadString()));
          }
          goto label_8;
        }
        catch (Exception ex)
        {
          UnityEngine.Debug.LogError((object) ("Receipt XML was malformed.\n" + ex.Message));
          goto label_8;
        }
      }
    }
    UnityEngine.Debug.LogError((object) "Receipt XML is invalid.");
label_8:
    return xmlReceipt;
  }

  private BundleManager.InAppPurchase ParseInAppPurchase(string inAppPurchaseText)
  {
    BundleManager.InAppPurchase inAppPurchase = new BundleManager.InAppPurchase();
    try
    {
      string str = inAppPurchaseText.Replace("(", string.Empty).Replace(")", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty).Trim();
      char[] chArray1 = new char[1]{ ';' };
      foreach (string trimString in this.TrimStringArray(str.Split(chArray1)))
      {
        char[] chArray2 = new char[1]{ '=' };
        string[] strArray = this.TrimStringArray(trimString.Split(chArray2));
        if (!string.IsNullOrEmpty(strArray[0]))
        {
          string key = strArray[0];
          if (key != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (BundleManager.\u003C\u003Ef__switch\u0024map2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              BundleManager.\u003C\u003Ef__switch\u0024map2 = new Dictionary<string, int>(4)
              {
                {
                  "PurchaseDate",
                  0
                },
                {
                  "ProductIdentifier",
                  1
                },
                {
                  "TransactionIdentifier",
                  2
                },
                {
                  "Quantity",
                  3
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (BundleManager.\u003C\u003Ef__switch\u0024map2.TryGetValue(key, out num))
            {
              switch (num)
              {
                case 0:
                  inAppPurchase.PurchaseDate = strArray[1].Replace("\"", string.Empty);
                  continue;
                case 1:
                  inAppPurchase.ProductIdentifier = strArray[1].Replace("\"", string.Empty);
                  continue;
                case 2:
                  inAppPurchase.TransactionIdentifier = strArray[1];
                  continue;
                case 3:
                  inAppPurchase.Quantity = strArray[1];
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
    }
    catch (Exception ex)
    {
      UnityEngine.Debug.LogError((object) ("Unable to parse In App Purchase.\n" + ex.Message));
    }
    return inAppPurchase;
  }

  private string[] TrimStringArray(string[] stringArray)
  {
    string[] strArray = stringArray;
    for (int index = 0; index < strArray.Length; ++index)
      strArray[index] = strArray[index].Trim();
    return strArray;
  }

  public class InAppPurchase
  {
    public string PurchaseDate = string.Empty;
    public string TransactionIdentifier = string.Empty;
    public string ProductIdentifier = string.Empty;
    public string Quantity = string.Empty;

    public InAppPurchase()
    {
      this.PurchaseDate = string.Empty;
      this.TransactionIdentifier = string.Empty;
      this.ProductIdentifier = string.Empty;
      this.Quantity = string.Empty;
    }

    public InAppPurchase(
      string purchaseDate,
      string transactionIdentifier,
      string productIdentifier,
      string quantity)
    {
      this.PurchaseDate = purchaseDate;
      this.TransactionIdentifier = transactionIdentifier;
      this.ProductIdentifier = productIdentifier;
      this.Quantity = quantity;
    }

    public override string ToString() => string.Format("PurchaseDate={0} TransactionIdentifier={1} ProductIdentifier={2} Quantity={3}", (object) this.PurchaseDate, (object) this.TransactionIdentifier, (object) this.ProductIdentifier, (object) this.Quantity);
  }
}
