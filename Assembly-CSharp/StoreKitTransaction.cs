// Decompiled with JetBrains decompiler
// Type: StoreKitTransaction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Prime31;
using System.Collections;
using System.Collections.Generic;

public class StoreKitTransaction
{
  public string productIdentifier;
  public string base64EncodedTransactionReceipt;
  public int quantity;

  public static List<StoreKitTransaction> transactionsFromJson(string json)
  {
    List<StoreKitTransaction> storeKitTransactionList = new List<StoreKitTransaction>();
    foreach (Hashtable ht in MiniJsonExtensions.arrayListFromJson(json))
      storeKitTransactionList.Add(StoreKitTransaction.transactionFromHashtable(ht));
    return storeKitTransactionList;
  }

  public static StoreKitTransaction transactionFromJson(string json) => StoreKitTransaction.transactionFromHashtable(MiniJsonExtensions.hashtableFromJson(json));

  public static StoreKitTransaction transactionFromHashtable(Hashtable ht)
  {
    StoreKitTransaction storeKitTransaction = new StoreKitTransaction();
    if (ht.ContainsKey((object) "productIdentifier"))
      storeKitTransaction.productIdentifier = ht[(object) "productIdentifier"].ToString();
    if (ht.ContainsKey((object) "base64EncodedReceipt"))
      storeKitTransaction.base64EncodedTransactionReceipt = ht[(object) "base64EncodedReceipt"].ToString();
    if (ht.ContainsKey((object) "quantity"))
      storeKitTransaction.quantity = int.Parse(ht[(object) "quantity"].ToString());
    return storeKitTransaction;
  }

  public override string ToString() => string.Format("<StoreKitTransaction>\nID: {0}\nReceipt: {1}\nQuantity: {2}", (object) this.productIdentifier, (object) this.base64EncodedTransactionReceipt, (object) this.quantity);
}
