// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ContactRequestView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public override string ToString() => "[Request contact: [Request ID: " + this.RequestId.ToString() + "][Initiator Cmid :" + this.InitiatorCmid.ToString() + "][Initiator Name:" + this.InitiatorName + "][Receiver Cmid: " + this.ReceiverCmid.ToString() + "]" + "[Initiator Message: " + this.InitiatorMessage + "][Status: " + this.Status.ToString() + "][Sent Date: " + this.SentDate.ToString() + "]]";
  }
}
