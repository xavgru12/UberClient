// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.EpinView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
