// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.BoxTransactionView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
