// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.BoxTransactionView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
