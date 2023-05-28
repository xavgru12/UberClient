// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ClaimFacebookGiftView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class ClaimFacebookGiftView
  {
    public ClaimFacebookGiftResult ClaimResult { get; set; }

    public int? ItemId { get; set; }

    public ClaimFacebookGiftView()
    {
    }

    public ClaimFacebookGiftView(ClaimFacebookGiftResult _claimResult, int? _itemId)
    {
      this.ClaimResult = _claimResult;
      this.ItemId = _itemId;
    }
  }
}
