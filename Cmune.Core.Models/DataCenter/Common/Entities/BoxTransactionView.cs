// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.BoxTransactionView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  public class BoxTransactionView
  {
    public int Id { get; set; }

    public BoxType BoxType { get; set; }

    public BundleCategoryType Category { get; set; }

    public int BoxId { get; set; }

    public int Cmid { get; set; }

    public DateTime TransactionDate { get; set; }

    public bool IsAdmin { get; set; }

    public int CreditPrice { get; set; }

    public int PointPrice { get; set; }

    public int TotalCreditsAttributed { get; set; }

    public int TotalPointsAttributed { get; set; }
  }
}
