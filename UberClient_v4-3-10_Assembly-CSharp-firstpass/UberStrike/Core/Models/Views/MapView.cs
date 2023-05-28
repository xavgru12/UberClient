// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.MapView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public bool IsBlueBox { get; set; }

    public GameBoxType BoxType { get; set; }

    public int RecommendedItemId { get; set; }

    public int SupportedGameModes { get; set; }

    public int SupportedItemClass { get; set; }

    public int MaxPlayers { get; set; }

    public Dictionary<GameModeType, MapSettings> Settings { get; set; }
  }
}
