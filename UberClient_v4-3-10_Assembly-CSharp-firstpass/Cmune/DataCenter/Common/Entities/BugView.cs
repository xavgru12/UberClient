// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.BugView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class BugView
  {
    public string Content { get; set; }

    public string Subject { get; set; }

    public BugView()
    {
    }

    public BugView(string subject, string content)
    {
      this.Subject = subject.Trim();
      this.Content = content.Trim();
    }

    public override string ToString() => "[Bug: [Subject: " + this.Subject + "][Content :" + this.Content + "]]";
  }
}
