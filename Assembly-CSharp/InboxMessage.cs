// Decompiled with JetBrains decompiler
// Type: InboxMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;

public class InboxMessage
{
  public InboxMessage(PrivateMessageView view, string senderName)
  {
    this.MessageView = view;
    this.SenderName = senderName;
  }

  public bool IsMine => this.MessageView.FromCmid == PlayerDataManager.Cmid;

  public bool IsAdmin => this.MessageView.FromCmid == 767;

  public string Content => this.MessageView.ContentText;

  public string SentDateString => this.MessageView.DateSent.ToString("MMM") + " " + this.MessageView.DateSent.Day.ToString() + " at " + this.MessageView.DateSent.ToShortTimeString();

  public string SenderName { get; private set; }

  public PrivateMessageView MessageView { get; private set; }
}
