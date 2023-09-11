// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.PointDepositView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class PointDepositView
  {
    public int PointDepositId { get; set; }

    public DateTime DepositDate { get; set; }

    public int Points { get; set; }

    public int Cmid { get; set; }

    public bool IsAdminAction { get; set; }

    public PointsDepositType DepositType { get; set; }

    public PointDepositView()
    {
    }

    public PointDepositView(
      int pointDepositId,
      DateTime depositDate,
      int points,
      int cmid,
      bool isAdminAction,
      PointsDepositType despositType)
    {
      this.PointDepositId = pointDepositId;
      this.DepositDate = depositDate;
      this.Points = points;
      this.Cmid = cmid;
      this.IsAdminAction = isAdminAction;
      this.DepositType = despositType;
    }
  }
}
