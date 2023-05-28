// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.ApplicationConfigurationView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Core.Models.Views
{
  [Serializable]
  public class ApplicationConfigurationView
  {
    public Dictionary<int, int> XpRequiredPerLevel { get; set; }

    public int MaxLevel { get; set; }

    public int MaxXp { get; set; }

    public int XpKill { get; set; }

    public int XpSmackdown { get; set; }

    public int XpHeadshot { get; set; }

    public int XpNutshot { get; set; }

    public int XpPerMinuteLoser { get; set; }

    public int XpPerMinuteWinner { get; set; }

    public int XpBaseLoser { get; set; }

    public int XpBaseWinner { get; set; }

    public int PointsKill { get; set; }

    public int PointsSmackdown { get; set; }

    public int PointsHeadshot { get; set; }

    public int PointsNutshot { get; set; }

    public int PointsPerMinuteLoser { get; set; }

    public int PointsPerMinuteWinner { get; set; }

    public int PointsBaseLoser { get; set; }

    public int PointsBaseWinner { get; set; }
  }
}
