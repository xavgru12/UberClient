// Decompiled with JetBrains decompiler
// Type: Cmune.Core.Models.Views.PhotonView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
