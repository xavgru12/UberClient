// Decompiled with JetBrains decompiler
// Type: Cmune.Core.Models.Views.PhotonView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
