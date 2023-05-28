// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.PointDepositView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
