// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.PrivateMessageView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public override string ToString() => "[Private Message: " + "[ID:" + this.PrivateMessageId.ToString() + "][From:" + this.FromCmid.ToString() + "][To:" + this.ToCmid.ToString() + "][Date:" + this.DateSent.ToString() + "][" + "[Content:" + this.ContentText + "][Is Read:" + this.IsRead.ToString() + "][Has attachment:" + this.HasAttachment.ToString() + "][Is deleted by sender:" + this.IsDeletedBySender.ToString() + "][Is deleted by receiver:" + this.IsDeletedByReceiver.ToString() + "]" + "]";
  }
}
