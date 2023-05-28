// Decompiled with JetBrains decompiler
// Type: Cmune.Core.Models.Views.PhotonView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System;

namespace Cmune.Core.Models.Views
{
  [Serializable]
  public class PhotonView
  {
    public int PhotonId { get; set; }

    public string IP { get; set; }

    public string Name { get; set; }

    public RegionType Region { get; set; }

    public int Port { get; set; }

    public PhotonUsageType UsageType { get; set; }

    public int MinLatency { get; set; }
  }
}
