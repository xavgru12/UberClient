
using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class CheckApplicationVersionView
  {
    public ApplicationView ClientVersion { get; set; }

    public ApplicationView CurrentVersion { get; set; }

    public CheckApplicationVersionView()
    {
    }

    public CheckApplicationVersionView(ApplicationView clienVersion, ApplicationView currentVersion)
    {
      this.ClientVersion = clienVersion;
      this.CurrentVersion = currentVersion;
    }

    public override string ToString() => "[CheckApplicationVersionView: [ClientVersion: " + (object) this.ClientVersion + "][CurrentVersion: " + (object) this.CurrentVersion + "]]";
  }
}
