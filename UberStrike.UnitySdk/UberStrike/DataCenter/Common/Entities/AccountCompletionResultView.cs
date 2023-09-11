// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.AccountCompletionResultView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
