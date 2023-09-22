
using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;

namespace UberStrike.Core.ViewModel
{
  [Serializable]
  public class ItemTransactionsViewModel
  {
    public List<ItemTransactionView> ItemTransactions { get; set; }

    public int TotalCount { get; set; }
  }
}
