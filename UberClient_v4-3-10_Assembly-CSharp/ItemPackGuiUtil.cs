// Decompiled with JetBrains decompiler
// Type: ItemPackGuiUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using UberStrike.Core.Models.Views;

public static class ItemPackGuiUtil
{
  public const int Columns = 6;
  public const int Rows = 2;

  public static BuyingDurationType GetDuration(IUnityItem item)
  {
    BuyingDurationType duration = BuyingDurationType.None;
    if (item != null && item.ItemView != null && item.ItemView.Prices != null)
    {
      IEnumerator<ItemPrice> enumerator = item.ItemView.Prices.GetEnumerator();
      if (enumerator.MoveNext())
        duration = enumerator.Current.Duration;
    }
    return duration;
  }
}
