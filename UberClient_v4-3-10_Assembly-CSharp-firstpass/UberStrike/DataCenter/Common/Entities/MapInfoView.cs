// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.MapInfoView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace UberStrike.DataCenter.Common.Entities
{
  public class MapInfoView
  {
    public int MapId { get; set; }

    public string DisplayName { get; set; }

    public string SceneName { get; set; }

    public string Description { get; set; }

    public bool InUse { get; set; }

    public int PlayerMax { get; set; }

    public int SupportedGameModes { get; set; }

    public int SupportedItemClass { get; set; }

    public MapInfoView(int mapId, string displayName, string sceneName, string description)
    {
      this.MapId = mapId;
      this.DisplayName = displayName;
      this.SceneName = sceneName;
      this.Description = description;
    }
  }
}
