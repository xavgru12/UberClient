
using System;
using System.Collections.Generic;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class AccountCompletionResultView
  {
    public int Result { get; set; }

    public Dictionary<int, int> ItemsAttributed { get; set; }

    public List<string> NonDuplicateNames { get; set; }

    public AccountCompletionResultView()
    {
      this.ItemsAttributed = new Dictionary<int, int>();
      this.NonDuplicateNames = new List<string>();
    }

    public AccountCompletionResultView(int result)
      : this()
    {
      this.Result = result;
    }

    public AccountCompletionResultView(
      int result,
      Dictionary<int, int> itemsAttributed,
      List<string> nonDuplicateNames)
    {
      this.Result = result;
      this.ItemsAttributed = itemsAttributed;
      this.NonDuplicateNames = nonDuplicateNames;
    }
  }
}
