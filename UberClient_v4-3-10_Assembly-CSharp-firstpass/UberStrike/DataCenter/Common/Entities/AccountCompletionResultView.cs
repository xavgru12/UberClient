// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.AccountCompletionResultView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
