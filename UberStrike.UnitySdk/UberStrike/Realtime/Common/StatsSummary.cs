// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.StatsSummary
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Realtime.Common
{
  [Serializable]
  public class StatsSummary
  {
    public string Name { get; set; }

    public int Kills { get; set; }

    public int Deaths { get; set; }

    public int Level { get; set; }

    public int Cmid { get; set; }

    public TeamID Team { get; set; }

    public Dictionary<byte, ushort> Achievements { get; set; }
  }
}
