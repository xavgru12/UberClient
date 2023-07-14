// Decompiled with JetBrains decompiler
// Type: LotteryManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.WebService.Unity;

public class LotteryManager : Singleton<LotteryManager>
{
  public const int IMG_WIDTH = 282;
  public const int IMG_HEIGHT = 317;
  private Dictionary<BundleCategoryType, List<LotteryShopItem>> _lotteryItems;
  private List<LuckyDrawShopItem> _luckyDrawItems;

  private LotteryManager()
  {
    this._luckyDrawItems = new List<LuckyDrawShopItem>();
    this._lotteryItems = new Dictionary<BundleCategoryType, List<LotteryShopItem>>();
  }

  [DebuggerHidden]
  private IEnumerable<LotteryShopItem> GetAllItemsOfType(System.Type type)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    LotteryManager.\u003CGetAllItemsOfType\u003Ec__Iterator70 allItemsOfType = new LotteryManager.\u003CGetAllItemsOfType\u003Ec__Iterator70()
    {
      type = type,
      \u003C\u0024\u003Etype = type,
      \u003C\u003Ef__this = this
    };
    // ISSUE: reference to a compiler-generated field
    allItemsOfType.\u0024PC = -2;
    return (IEnumerable<LotteryShopItem>) allItemsOfType;
  }

  public void RefreshLotteryItems()
  {
    this.GetAllLuckyDraws();
    this.GetAllMysteryBoxes();
  }

  public bool TryGetBundle(BundleCategoryType bundle, out List<LotteryShopItem> items) => this._lotteryItems.TryGetValue(bundle, out items);

  public void ShowNextItem(LotteryShopItem currentItem)
  {
    List<LotteryShopItem> lotteryShopItemList = new List<LotteryShopItem>(this.GetAllItemsOfType(currentItem.GetType()));
    if (lotteryShopItemList.Count <= 0)
      return;
    int index1 = lotteryShopItemList.FindIndex((Predicate<LotteryShopItem>) (i => i == currentItem));
    if (index1 < 0)
    {
      lotteryShopItemList[UnityEngine.Random.Range(0, lotteryShopItemList.Count)].Use();
    }
    else
    {
      int index2 = (index1 + 1) % lotteryShopItemList.Count;
      lotteryShopItemList[index2].Use();
    }
  }

  public void ShowPreviousItem(LotteryShopItem currentItem)
  {
    List<LotteryShopItem> lotteryShopItemList = new List<LotteryShopItem>(this.GetAllItemsOfType(currentItem.GetType()));
    if (lotteryShopItemList.Count <= 0)
      return;
    int index1 = lotteryShopItemList.FindIndex((Predicate<LotteryShopItem>) (i => i == currentItem));
    if (index1 < 0)
    {
      lotteryShopItemList[UnityEngine.Random.Range(0, lotteryShopItemList.Count)].Use();
    }
    else
    {
      int index2 = (index1 - 1 + lotteryShopItemList.Count) % lotteryShopItemList.Count;
      lotteryShopItemList[index2].Use();
    }
  }

  public LuckyDrawPopup RunLuckyDraw(LuckyDrawShopItem item)
  {
    LuckyDrawPopup dialog = new LuckyDrawPopup(item);
    this.StartTask((LotteryPopupDialog) dialog);
    return dialog;
  }

  public void RunMysteryBox(MysteryBoxShopItem item) => this.StartTask((LotteryPopupDialog) new MysteryBoxPopup(item));

  private void StartTask(LotteryPopupDialog dialog)
  {
    LotteryPopupTask lotteryPopupTask = new LotteryPopupTask(dialog);
    PopupSystem.Show((IPopupDialog) dialog);
  }

  private void GetAllMysteryBoxes() => ShopWebServiceClient.GetAllMysteryBoxs((Action<List<MysteryBoxUnityView>>) (list =>
  {
    foreach (MysteryBoxUnityView mysteryBox in list)
    {
      List<LotteryShopItem> lotteryShopItemList;
      if (!this._lotteryItems.TryGetValue(mysteryBox.Category, out lotteryShopItemList))
      {
        lotteryShopItemList = new List<LotteryShopItem>();
        this._lotteryItems[mysteryBox.Category] = lotteryShopItemList;
      }
      MysteryBoxShopItem unityItem = mysteryBox.ToUnityItem();
      lotteryShopItemList.Add((LotteryShopItem) unityItem);
    }
  }), (Action<Exception>) (ex => UnityEngine.Debug.LogError((object) ("MysteryBoxManager failed with: " + ex.Message))));

  private void GetAllLuckyDraws() => ShopWebServiceClient.GetAllLuckyDraws((Action<List<LuckyDrawUnityView>>) (list =>
  {
    foreach (LuckyDrawUnityView luckyDraw in list)
    {
      List<LotteryShopItem> lotteryShopItemList;
      if (!this._lotteryItems.TryGetValue(luckyDraw.Category, out lotteryShopItemList))
      {
        lotteryShopItemList = new List<LotteryShopItem>();
        this._lotteryItems[luckyDraw.Category] = lotteryShopItemList;
      }
      LuckyDrawShopItem unityItem = luckyDraw.ToUnityItem();
      lotteryShopItemList.Add((LotteryShopItem) unityItem);
      this._luckyDrawItems.Add(unityItem);
    }
  }), (Action<Exception>) (ex => UnityEngine.Debug.LogError((object) ("MysteryBoxManager failed with: " + ex.Message))));
}
