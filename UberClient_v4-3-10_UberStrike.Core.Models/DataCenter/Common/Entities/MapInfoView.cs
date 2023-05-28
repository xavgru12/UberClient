// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.MapInfoView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
