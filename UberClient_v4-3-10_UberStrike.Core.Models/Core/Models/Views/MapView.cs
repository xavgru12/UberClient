// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.MapView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using System;
using System.Collections.Generic;
using UberStrike.Core.Types;

namespace UberStrike.Core.Models.Views
{
  [Serializable]
  public class MapView
  {
    public int MapId { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public string SceneName { get; set; }

    public string FileName { get; set; }

    public bool IsBlueBox { get; set; }

    public int RecommendedItemId { get; set; }

    public int SupportedGameModes { get; set; }

    public int MaxPlayers { get; set; }

    public Dictionary<GameModeType, MapSettings> Settings { get; set; }
  }
}
