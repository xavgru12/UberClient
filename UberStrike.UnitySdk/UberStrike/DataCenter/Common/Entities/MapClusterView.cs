
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
