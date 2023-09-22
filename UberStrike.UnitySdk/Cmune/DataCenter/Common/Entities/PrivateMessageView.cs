﻿
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
