// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.MapView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
