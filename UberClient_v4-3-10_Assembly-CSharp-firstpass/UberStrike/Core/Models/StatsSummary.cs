// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.StatsSummary
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Core.Models
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
