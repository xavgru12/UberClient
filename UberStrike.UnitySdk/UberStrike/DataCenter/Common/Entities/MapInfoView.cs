// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.MapInfoView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Collections.Generic;
using UberStrike.Core.Types;

namespace UberStrike.DataCenter.Common.Entities
{
  public class MapInfoView
  {
    public int MapId { get; private set; }

    public string DisplayName { get; private set; }

    public string SceneName { get; private set; }

    public string Description { get; private set; }

    public bool InUse { get; private set; }

    public bool IsBlueBox { get; private set; }

    public int ItemId { get; private set; }

    public Dictionary<DefinitionType, MapVersionView> Assets { get; private set; }

    public MapInfoView(
      int mapId,
      string displayName,
      string sceneName,
      string description,
      bool inUse,
      bool isBlueBox,
      int itemId,
      Dictionary<DefinitionType, MapVersionView> assets)
    {
      this.MapId = mapId;
      this.DisplayName = displayName;
      this.SceneName = sceneName;
      this.Description = description;
      this.InUse = inUse;
      this.IsBlueBox = isBlueBox;
      this.Assets = assets;
      this.ItemId = itemId;
    }
  }
}
