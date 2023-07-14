// Decompiled with JetBrains decompiler
// Type: Cmune.Core.Models.Views.PhotonsClusterView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
