// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.EpinView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

namespace Cmune.DataCenter.Common.Entities
{
  public class EpinView
  {
    public int EpinId { get; private set; }

    public string Pin { get; private set; }

    public bool IsRedeemed { get; private set; }

    public int BatchId { get; private set; }

    public bool IsRetired { get; private set; }

    public EpinView(int epinId, string pin, bool isRedeemed, int batchId, bool isRetired)
    {
      this.EpinId = epinId;
      this.Pin = pin;
      this.IsRedeemed = isRedeemed;
      this.BatchId = batchId;
      this.IsRetired = isRetired;
    }
  }
}
