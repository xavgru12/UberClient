// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.PrivateMessageView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class PrivateMessageView
  {
    public int PrivateMessageId { get; set; }

    public int FromCmid { get; set; }

    public string FromName { get; set; }

    public int ToCmid { get; set; }

    public DateTime DateSent { get; set; }

    public string ContentText { get; set; }

    public bool IsRead { get; set; }

    public bool HasAttachment { get; set; }

    public bool IsDeletedBySender { get; set; }

    public bool IsDeletedByReceiver { get; set; }

    public override string ToString() => "[Private Message: " + "[ID:" + (object) this.PrivateMessageId + "][From:" + (object) this.FromCmid + "][To:" + (object) this.ToCmid + "][Date:" + (object) this.DateSent + "][" + "[Content:" + this.ContentText + "][Is Read:" + (object) this.IsRead + "][Has attachment:" + (object) this.HasAttachment + "][Is deleted by sender:" + (object) this.IsDeletedBySender + "][Is deleted by receiver:" + (object) this.IsDeletedByReceiver + "]" + "]";
  }
}
