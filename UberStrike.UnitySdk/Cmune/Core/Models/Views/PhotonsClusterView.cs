// Decompiled with JetBrains decompiler
// Type: Cmune.Core.Models.Views.PhotonsClusterView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Collections.Generic;

namespace Cmune.Core.Models.Views
{
  public class PhotonsClusterView
  {
    public int PhotonsClusterId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public List<PhotonView> Photons { get; set; }

    public PhotonsClusterView(
      int photonsClusterId,
      string name,
      string description,
      List<PhotonView> photons)
    {
      this.PhotonsClusterId = photonsClusterId;
      this.Name = name;
      this.Description = description;
      this.Photons = photons;
    }

    public PhotonsClusterView(int photonsClusterId, string name, List<PhotonView> photons)
      : this(photonsClusterId, name, string.Empty, photons)
    {
    }
  }
}
