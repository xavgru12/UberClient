// Decompiled with JetBrains decompiler
// Type: Cmune.Core.Models.Views.PhotonsClusterView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
