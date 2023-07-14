// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ContactRequestView
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class ContactRequestView
  {
    public int RequestId { get; set; }

    public int InitiatorCmid { get; set; }

    public string InitiatorName { get; set; }

    public int ReceiverCmid { get; set; }

    public string InitiatorMessage { get; set; }

    public ContactRequestStatus Status { get; set; }

    public DateTime SentDate { get; set; }

    public ContactRequestView()
    {
    }

    public ContactRequestView(int initiatorCmid, int receiverCmid, string initiatorMessage)
    {
      this.InitiatorCmid = initiatorCmid;
      this.ReceiverCmid = receiverCmid;
      this.InitiatorMessage = initiatorMessage;
    }

    public ContactRequestView(
      int requestID,
      int initiatorCmid,
      string initiatorName,
      int receiverCmid,
      string initiatorMessage,
      ContactRequestStatus status,
      DateTime sentDate)
    {
      this.SetContactRequest(requestID, initiatorCmid, initiatorName, receiverCmid, initiatorMessage, status, sentDate);
    }

    public void SetContactRequest(
      int requestID,
      int initiatorCmid,
      string initiatorName,
      int receiverCmid,
      string initiatorMessage,
      ContactRequestStatus status,
      DateTime sentDate)
    {
      this.RequestId = requestID;
      this.InitiatorCmid = initiatorCmid;
      this.InitiatorName = initiatorName;
      this.ReceiverCmid = receiverCmid;
      this.InitiatorMessage = initiatorMessage;
      this.Status = status;
      this.SentDate = sentDate;
    }

    public override string ToString() => "[Request contact: [Request ID: " + (object) this.RequestId + "][Initiator Cmid :" + (object) this.InitiatorCmid + "][Initiator Name:" + this.InitiatorName + "][Receiver Cmid: " + (object) this.ReceiverCmid + "]" + "[Initiator Message: " + this.InitiatorMessage + "][Status: " + (object) this.Status + "][Sent Date: " + (object) this.SentDate + "]]";
  }
}
