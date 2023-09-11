// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.BugView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
