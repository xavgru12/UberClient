// Decompiled with JetBrains decompiler
// Type: TransactionHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Core.ViewModel;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class TransactionHistory : Singleton<TransactionHistory>
{
  private const float RowHeight = 23f;
  private const float ButtonWidth = 100f;
  private const float ButtonHeight = 32f;
  private const int ElementsPerPage = 15;
  private static string DATE_FORMAT = "yyyy/MM/dd";
  private int _selectedTab;
  private GUIContent[] _tabs;
  private Vector2 _scrollControls;
  private string[] _itemsTableColumnHeadingArray;
  private string[] _pointsTableColumnHeadingArray;
  private string[] _creditsTableColumnHeadingArray;
  private string _prevPageButtonLabel;
  private string _nextPageButtonLabel;
  private TransactionHistory.TransactionCache<ItemTransactionsViewModel> _itemTransactions;
  private TransactionHistory.TransactionCache<PointDepositsViewModel> _pointTransactions;
  private TransactionHistory.TransactionCache<CurrencyDepositsViewModel> _creditTransactions;

  private TransactionHistory()
  {
    this._itemTransactions = new TransactionHistory.TransactionCache<ItemTransactionsViewModel>();
    this._pointTransactions = new TransactionHistory.TransactionCache<PointDepositsViewModel>();
    this._creditTransactions = new TransactionHistory.TransactionCache<CurrencyDepositsViewModel>();
    this._tabs = new GUIContent[3]
    {
      new GUIContent("Items"),
      new GUIContent("Points"),
      new GUIContent("Credits")
    };
    this._itemsTableColumnHeadingArray = new string[5]
    {
      "Date",
      "Item Name",
      "Points",
      "Credits",
      "Duration"
    };
    this._pointsTableColumnHeadingArray = new string[3]
    {
      "Date",
      "Points",
      "Type"
    };
    this._creditsTableColumnHeadingArray = new string[6]
    {
      "Transaction Key",
      "Date",
      "Cost",
      "Credits",
      "Points",
      "Bundle Name"
    };
    this._prevPageButtonLabel = "Prev Page";
    this._nextPageButtonLabel = "Next Page";
  }

  public void DrawPanel(Rect panelRect)
  {
    GUI.BeginGroup(panelRect, GUIContent.none, BlueStonez.window_standard_grey38);
    this.DrawTabs(new Rect(2f, 5f, panelRect.width - 4f, 30f));
    this.DrawTable(new Rect(2f, 35f, panelRect.width - 4f, panelRect.height - 35f));
    GUI.EndGroup();
  }

  private void DrawTable(Rect panelRect)
  {
    Rect headingRect = new Rect(panelRect.x + 5f, panelRect.y, panelRect.width - 10f, 25f);
    Rect scrollViewRect = new Rect(panelRect.x + 5f, panelRect.y + 30f, panelRect.width - 10f, (float) ((double) panelRect.height - 35.0 - 32.0 - 5.0));
    Rect rect = new Rect(0.0f, scrollViewRect.y + scrollViewRect.height, panelRect.width, panelRect.height - scrollViewRect.height);
    switch ((TransactionHistory.TransactionType) this._selectedTab)
    {
      case TransactionHistory.TransactionType.Item:
        this.DrawItemsTableHeadingBar(headingRect);
        this.DrawItemsTableContent(scrollViewRect);
        this.DrawItemsButtons(rect);
        break;
      case TransactionHistory.TransactionType.Point:
        this.DrawPointsTableHeadingBar(headingRect);
        this.DrawPointsTableContent(scrollViewRect);
        this.DrawPointsButtons(rect);
        break;
      case TransactionHistory.TransactionType.Credit:
        this.DrawCreditsTableHeadingBar(headingRect);
        this.DrawCreditsTableContent(scrollViewRect);
        this.DrawCreditsButtons(rect);
        break;
    }
  }

  private void DrawTabs(Rect tabRect)
  {
    int num = UnityGUI.Toolbar(tabRect, this._selectedTab, this._tabs, this._tabs.Length, BlueStonez.tab_medium);
    if (num == this._selectedTab)
      return;
    this._selectedTab = num;
    this.GetCurrentTransactions();
  }

  private float GetColumnOffset(TransactionHistory.AccountArea area, int index, float totalWidth)
  {
    switch (area)
    {
      case TransactionHistory.AccountArea.Items:
        switch (index)
        {
          case 0:
            return 0.0f;
          case 1:
            return 100f;
          case 2:
            return (float) (100.0 + (double) Mathf.Max(totalWidth, 400f) - 300.0);
          case 3:
            return (float) (100.0 + (double) Mathf.Max(totalWidth, 400f) - 300.0 + 50.0);
          case 4:
            return (float) (100.0 + (double) Mathf.Max(totalWidth, 400f) - 300.0 + 50.0 + 50.0);
          default:
            return 0.0f;
        }
      case TransactionHistory.AccountArea.Points:
        return (float) (index * Mathf.RoundToInt(totalWidth / 3f));
      case TransactionHistory.AccountArea.Credits:
        switch (index)
        {
          case 0:
            return 0.0f;
          case 1:
            return 150f;
          case 2:
            return 220f;
          case 3:
            return 270f;
          case 4:
            return 320f;
          case 5:
            return 370f;
          default:
            return 0.0f;
        }
      default:
        return 0.0f;
    }
  }

  private float GetColumnWidth(TransactionHistory.AccountArea area, int index, float totalWidth)
  {
    switch (area)
    {
      case TransactionHistory.AccountArea.Items:
        switch (index)
        {
          case 0:
            return 151f;
          case 1:
            return (float) ((double) Mathf.Max(totalWidth, 400f) - 300.0 + 1.0);
          case 2:
            return 51f;
          case 3:
            return 51f;
          case 4:
            return 100f;
          default:
            return 0.0f;
        }
      case TransactionHistory.AccountArea.Points:
        switch (index)
        {
          case 0:
            return (float) (Mathf.RoundToInt(totalWidth / 3f) + 1);
          case 1:
            return (float) (Mathf.RoundToInt(totalWidth / 3f) + 1);
          case 2:
            return (float) Mathf.RoundToInt(totalWidth / 3f);
          default:
            return 0.0f;
        }
      case TransactionHistory.AccountArea.Credits:
        switch (index)
        {
          case 0:
            return 151f;
          case 1:
            return 71f;
          case 2:
            return 51f;
          case 3:
            return 51f;
          case 4:
            return 51f;
          case 5:
            return Mathf.Max(totalWidth, 450f) - 370f;
          default:
            return 0.0f;
        }
      default:
        return 0.0f;
    }
  }

  private void DrawItemsTableHeadingBar(Rect headingRect)
  {
    GUI.BeginGroup(headingRect);
    for (int index = 0; index < this._itemsTableColumnHeadingArray.Length; ++index)
    {
      Rect position = new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Items, index, headingRect.width), 0.0f, this.GetColumnWidth(TransactionHistory.AccountArea.Items, index, headingRect.width), headingRect.height);
      GUI.Button(position, string.Empty, BlueStonez.box_grey50);
      GUI.Label(position, new GUIContent(this._itemsTableColumnHeadingArray[index]), BlueStonez.label_interparkmed_11pt);
    }
    GUI.EndGroup();
  }

  private void DrawItemsTableContent(Rect scrollViewRect)
  {
    GUI.Box(scrollViewRect, GUIContent.none, BlueStonez.window_standard_grey38);
    if (this._itemTransactions.CurrentPage == null)
      return;
    this._scrollControls = GUITools.BeginScrollView(scrollViewRect.Expand(0, -1), this._scrollControls, new Rect(0.0f, 0.0f, scrollViewRect.width - 17f, (float) this._itemTransactions.CurrentPage.ItemTransactions.Count * 23f));
    float top = 0.0f;
    foreach (ItemTransactionView itemTransaction in this._itemTransactions.CurrentPage.ItemTransactions)
    {
      IUnityItem itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(itemTransaction.ItemId);
      string text = itemInShop == null ? string.Format("item[{0}]", (object) itemTransaction.ItemId) : TextUtility.ShortenText(itemInShop.Name, 20, true);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Items, 0, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Items, 0, scrollViewRect.width), 23f), itemTransaction.WithdrawalDate.ToString(TransactionHistory.DATE_FORMAT), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Items, 1, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Items, 1, scrollViewRect.width), 23f), text, BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Items, 2, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Items, 2, scrollViewRect.width), 23f), itemTransaction.Points.ToString(), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Items, 3, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Items, 3, scrollViewRect.width), 23f), itemTransaction.Credits.ToString(), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Items, 4, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Items, 4, scrollViewRect.width), 23f), ShopUtils.PrintDuration(itemTransaction.Duration), BlueStonez.label_interparkmed_11pt);
      top += 23f;
    }
    GUITools.EndScrollView();
  }

  private void DrawItemsButtons(Rect rect)
  {
    GUIStyle button = BlueStonez.button;
    GUI.enabled = this._itemTransactions.CurrentPageIndex != 0;
    if (GUITools.Button(new Rect(rect.x + 6f, rect.y + 5f, 100f, 32f), new GUIContent(this._prevPageButtonLabel), button))
    {
      --this._itemTransactions.CurrentPageIndex;
      this.AsyncGetItemTransactions();
    }
    GUI.enabled = true;
    if (this._itemTransactions.ElementCount <= 0)
      return;
    GUI.Label(new Rect((float) (((double) rect.x + (double) rect.width) / 2.0 - 100.0), rect.y + 5f, 200f, 32f), string.Format("Page {0} of {1}", (object) (this._itemTransactions.CurrentPageIndex + 1), (object) this._itemTransactions.PageCount), BlueStonez.label_interparkbold_11pt);
    GUI.enabled = this._itemTransactions.CurrentPageIndex + 1 < this._itemTransactions.PageCount;
    if (GUITools.Button(new Rect((float) ((double) rect.x + (double) rect.width - 100.0 - 2.0), rect.y + 5f, 100f, 32f), new GUIContent(this._nextPageButtonLabel), button))
    {
      ++this._itemTransactions.CurrentPageIndex;
      this.AsyncGetItemTransactions();
    }
    GUI.enabled = true;
  }

  private void DrawPointsTableHeadingBar(Rect headingRect)
  {
    GUI.BeginGroup(headingRect);
    for (int index = 0; index < this._pointsTableColumnHeadingArray.Length; ++index)
    {
      Rect position = new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Points, index, headingRect.width), 0.0f, this.GetColumnWidth(TransactionHistory.AccountArea.Points, index, headingRect.width), headingRect.height);
      GUI.Button(position, string.Empty, BlueStonez.box_grey50);
      GUI.Label(position, new GUIContent(this._pointsTableColumnHeadingArray[index]), BlueStonez.label_interparkmed_11pt);
    }
    GUI.EndGroup();
  }

  private void DrawPointsTableContent(Rect scrollViewRect)
  {
    GUI.Box(scrollViewRect, GUIContent.none, BlueStonez.window_standard_grey38);
    if (this._pointTransactions.CurrentPage == null)
      return;
    this._scrollControls = GUITools.BeginScrollView(scrollViewRect.Expand(0, -1), this._scrollControls, new Rect(0.0f, 0.0f, scrollViewRect.width - 17f, (float) this._pointTransactions.CurrentPage.PointDeposits.Count * 23f));
    float top = 0.0f;
    foreach (PointDepositView pointDeposit in this._pointTransactions.CurrentPage.PointDeposits)
    {
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Points, 0, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Points, 0, scrollViewRect.width), 23f), pointDeposit.DepositDate.ToString(TransactionHistory.DATE_FORMAT), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Points, 1, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Points, 1, scrollViewRect.width), 23f), pointDeposit.Points.ToString(), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Points, 2, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Points, 2, scrollViewRect.width), 23f), ((Enum) pointDeposit.DepositType).ToString(), BlueStonez.label_interparkmed_11pt);
      top += 23f;
    }
    GUITools.EndScrollView();
  }

  private void DrawPointsButtons(Rect rect)
  {
    GUIStyle button = BlueStonez.button;
    GUI.enabled = this._pointTransactions.CurrentPageIndex != 0;
    if (GUITools.Button(new Rect(rect.x + 6f, rect.y + 5f, 100f, 32f), new GUIContent(this._prevPageButtonLabel), button))
    {
      --this._pointTransactions.CurrentPageIndex;
      this.AsyncGetPointsDeposits();
    }
    GUI.enabled = true;
    if (this._pointTransactions.ElementCount <= 0)
      return;
    GUI.Label(new Rect((float) (((double) rect.x + (double) rect.width) / 2.0 - 100.0), rect.y + 5f, 200f, 32f), string.Format("Page {0} of {1}", (object) (this._pointTransactions.CurrentPageIndex + 1), (object) this._pointTransactions.PageCount), BlueStonez.label_interparkbold_11pt);
    GUI.enabled = this._pointTransactions.CurrentPageIndex + 1 < this._pointTransactions.PageCount;
    if (GUITools.Button(new Rect((float) ((double) rect.x + (double) rect.width - 100.0 - 2.0), rect.y + 5f, 100f, 32f), new GUIContent(this._nextPageButtonLabel), button))
    {
      ++this._pointTransactions.CurrentPageIndex;
      this.AsyncGetPointsDeposits();
    }
    GUI.enabled = true;
  }

  private void DrawCreditsTableHeadingBar(Rect headingRect)
  {
    GUI.BeginGroup(headingRect);
    for (int index = 0; index < this._creditsTableColumnHeadingArray.Length; ++index)
    {
      Rect position = new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Credits, index, headingRect.width), 0.0f, this.GetColumnWidth(TransactionHistory.AccountArea.Credits, index, headingRect.width), headingRect.height);
      GUI.Button(position, string.Empty, BlueStonez.box_grey50);
      GUI.Label(position, new GUIContent(this._creditsTableColumnHeadingArray[index]), BlueStonez.label_interparkmed_11pt);
    }
    GUI.EndGroup();
  }

  private void DrawCreditsTableContent(Rect scrollViewRect)
  {
    GUI.Box(scrollViewRect, GUIContent.none, BlueStonez.window_standard_grey38);
    if (this._creditTransactions.CurrentPage == null)
      return;
    this._scrollControls = GUITools.BeginScrollView(scrollViewRect.Expand(0, -1), this._scrollControls, new Rect(0.0f, 0.0f, scrollViewRect.width - 17f, (float) this._creditTransactions.CurrentPage.CurrencyDeposits.Count * 23f));
    float top = 0.0f;
    foreach (CurrencyDepositView currencyDeposit in this._creditTransactions.CurrentPage.CurrencyDeposits)
    {
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Credits, 0, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Credits, 0, scrollViewRect.width), 23f), TextUtility.ShortenText(currencyDeposit.TransactionKey, 20, true), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Credits, 1, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Credits, 1, scrollViewRect.width), 23f), currencyDeposit.DepositDate.ToString(TransactionHistory.DATE_FORMAT), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Credits, 2, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Credits, 2, scrollViewRect.width), 23f), currencyDeposit.CurrencyLabel + currencyDeposit.Cash.ToString("#0.00"), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Credits, 3, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Credits, 3, scrollViewRect.width), 23f), currencyDeposit.Credits.ToString(), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Credits, 4, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Credits, 4, scrollViewRect.width), 23f), currencyDeposit.Points.ToString(), BlueStonez.label_interparkmed_11pt);
      GUI.Label(new Rect(this.GetColumnOffset(TransactionHistory.AccountArea.Credits, 5, scrollViewRect.width), top, this.GetColumnWidth(TransactionHistory.AccountArea.Credits, 5, scrollViewRect.width), 23f), TextUtility.ShortenText(currencyDeposit.BundleName, 14, true), BlueStonez.label_interparkmed_11pt);
      top += 23f;
    }
    GUITools.EndScrollView();
  }

  private void DrawCreditsButtons(Rect rect)
  {
    GUIStyle button = BlueStonez.button;
    GUI.enabled = this._creditTransactions.CurrentPageIndex != 0;
    if (GUITools.Button(new Rect(rect.x + 6f, rect.y + 5f, 100f, 32f), new GUIContent(this._prevPageButtonLabel), button))
    {
      --this._creditTransactions.CurrentPageIndex;
      this.AsyncGetCurrencyDeposits();
    }
    GUI.enabled = true;
    if (this._creditTransactions.ElementCount <= 0)
      return;
    GUI.Label(new Rect((float) (((double) rect.x + (double) rect.width) / 2.0 - 100.0), rect.y + 5f, 200f, 32f), string.Format("Page {0} of {1}", (object) (this._creditTransactions.CurrentPageIndex + 1), (object) this._creditTransactions.PageCount), BlueStonez.label_interparkbold_11pt);
    GUI.enabled = this._creditTransactions.CurrentPageIndex + 1 < this._creditTransactions.PageCount;
    if (GUITools.Button(new Rect((float) ((double) rect.x + (double) rect.width - 100.0 - 2.0), rect.y + 5f, 100f, 32f), new GUIContent(this._nextPageButtonLabel), button))
    {
      ++this._creditTransactions.CurrentPageIndex;
      this.AsyncGetCurrencyDeposits();
    }
    GUI.enabled = true;
  }

  private void AsyncGetItemTransactions()
  {
    if (!this._itemTransactions.CurrentPageNeedsRefresh)
      return;
    int nextPageIndex = this._itemTransactions.CurrentPageIndex;
    UserWebServiceClient.GetItemTransactions(PlayerDataManager.CmidSecure, nextPageIndex + 1, 15, (Action<ItemTransactionsViewModel>) (ev =>
    {
      this._itemTransactions.SetPage(nextPageIndex, ev);
      this._itemTransactions.ElementCount = ev.TotalCount;
    }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
  }

  private void AsyncGetCurrencyDeposits()
  {
    if (!this._creditTransactions.CurrentPageNeedsRefresh)
      return;
    int nextPageIndex = this._creditTransactions.CurrentPageIndex;
    UserWebServiceClient.GetCurrencyDeposits(PlayerDataManager.CmidSecure, nextPageIndex + 1, 15, (Action<CurrencyDepositsViewModel>) (ev =>
    {
      this._creditTransactions.SetPage(nextPageIndex, ev);
      this._creditTransactions.ElementCount = ev.TotalCount;
    }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
  }

  private void AsyncGetPointsDeposits()
  {
    if (!this._pointTransactions.CurrentPageNeedsRefresh)
      return;
    int nextPageIndex = this._pointTransactions.CurrentPageIndex;
    UserWebServiceClient.GetPointsDeposits(PlayerDataManager.CmidSecure, nextPageIndex + 1, 15, (Action<PointDepositsViewModel>) (ev =>
    {
      this._pointTransactions.SetPage(nextPageIndex, ev);
      this._pointTransactions.ElementCount = ev.TotalCount;
    }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
  }

  public void GetCurrentTransactions()
  {
    switch (this._selectedTab)
    {
      case 0:
        this.AsyncGetItemTransactions();
        break;
      case 1:
        this.AsyncGetPointsDeposits();
        break;
      case 2:
        this.AsyncGetCurrencyDeposits();
        break;
    }
  }

  private enum TransactionType
  {
    Item,
    Point,
    Credit,
  }

  private enum AccountArea
  {
    Items,
    Points,
    Credits,
  }

  public class TransactionCache<T>
  {
    private float _refreshLastPage;

    public TransactionCache() => this.PageCache = new SortedList<int, T>();

    public SortedList<int, T> PageCache { get; private set; }

    public int CurrentPageIndex { get; set; }

    public T CurrentPage => this.PageCache.ContainsKey(this.CurrentPageIndex) ? this.PageCache[this.CurrentPageIndex] : default (T);

    public bool CurrentPageNeedsRefresh
    {
      get
      {
        if ((object) this.CurrentPage == null)
          return true;
        return this.CurrentPageIndex > 0 && this.CurrentPageIndex == this.PageCount - 1 && (double) this._refreshLastPage < (double) Time.time;
      }
    }

    public int ElementCount { get; set; }

    public int PageCount => Mathf.CeilToInt((float) (this.ElementCount / 15)) + 1;

    public void SetPage(int index, T page)
    {
      this.PageCache[index] = page;
      if (index + 1 != this.PageCount)
        return;
      this._refreshLastPage = Time.time + 30f;
    }
  }
}
