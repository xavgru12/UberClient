// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.EpinView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
