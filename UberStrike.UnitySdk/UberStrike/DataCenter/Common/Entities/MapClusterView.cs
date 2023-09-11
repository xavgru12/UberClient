// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.MapClusterView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Collections.Generic;

namespace UberStrike.DataCenter.Common.Entities
{
  public class MapClusterView
  {
    public string ApplicationVersion { get; private set; }

    public List<MapInfoView> Maps { get; private set; }

    public MapClusterView(string appVersion, List<MapInfoView> maps)
    {
      this.ApplicationVersion = appVersion;
      this.Maps = maps;
    }
  }
}
