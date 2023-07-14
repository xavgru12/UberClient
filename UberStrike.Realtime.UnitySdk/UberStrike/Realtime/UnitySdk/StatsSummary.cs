// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.StatsSummary
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
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
