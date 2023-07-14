// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.PrivateMessageView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
