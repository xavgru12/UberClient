// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.MapSettings
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
