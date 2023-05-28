// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.MapSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace UberStrike.Core.Models.Views
{
  [Serializable]
  public class MapSettings
  {
    public int KillsMin { get; set; }

    public int KillsMax { get; set; }

    public int KillsCurrent { get; set; }

    public int PlayersMin { get; set; }

    public int PlayersMax { get; set; }

    public int PlayersCurrent { get; set; }

    public int TimeMin { get; set; }

    public int TimeMax { get; set; }

    public int TimeCurrent { get; set; }
  }
}
