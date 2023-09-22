
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
