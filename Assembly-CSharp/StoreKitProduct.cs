// Decompiled with JetBrains decompiler
// Type: StoreKitProduct
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Prime31;
using System.Collections;
using System.Collections.Generic;

public class StoreKitProduct
{
  public string productIdentifier;
  public string title;
  public string description;
  public string price;
  public string currencySymbol;
  public string currencyCode;
  public string formattedPrice;

  public static List<StoreKitProduct> productsFromJson(string json)
  {
    List<StoreKitProduct> storeKitProductList = new List<StoreKitProduct>();
    foreach (Hashtable ht in MiniJsonExtensions.arrayListFromJson(json))
      storeKitProductList.Add(StoreKitProduct.productFromHashtable(ht));
    return storeKitProductList;
  }

  public static StoreKitProduct productFromHashtable(Hashtable ht)
  {
    StoreKitProduct storeKitProduct = new StoreKitProduct();
    if (ht.ContainsKey((object) "productIdentifier"))
      storeKitProduct.productIdentifier = ht[(object) "productIdentifier"].ToString();
    if (ht.ContainsKey((object) "localizedTitle"))
      storeKitProduct.title = ht[(object) "localizedTitle"].ToString();
    if (ht.ContainsKey((object) "localizedDescription"))
      storeKitProduct.description = ht[(object) "localizedDescription"].ToString();
    if (ht.ContainsKey((object) "price"))
      storeKitProduct.price = ht[(object) "price"].ToString();
    if (ht.ContainsKey((object) "currencySymbol"))
      storeKitProduct.currencySymbol = ht[(object) "currencySymbol"].ToString();
    if (ht.ContainsKey((object) "currencyCode"))
      storeKitProduct.currencyCode = ht[(object) "currencyCode"].ToString();
    if (ht.ContainsKey((object) "formattedPrice"))
      storeKitProduct.formattedPrice = ht[(object) "formattedPrice"].ToString();
    return storeKitProduct;
  }

  public override string ToString() => string.Format("<StoreKitProduct>\nID: {0}\nTitle: {1}\nDescription: {2}\nPrice: {3}\nCurrency Symbol: {4}\nFormatted Price: {5}\nCurrency Code: {6}", (object) this.productIdentifier, (object) this.title, (object) this.description, (object) this.price, (object) this.currencySymbol, (object) this.formattedPrice, (object) this.currencyCode);
}
