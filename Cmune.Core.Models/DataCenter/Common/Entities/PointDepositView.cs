// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.PointDepositView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
