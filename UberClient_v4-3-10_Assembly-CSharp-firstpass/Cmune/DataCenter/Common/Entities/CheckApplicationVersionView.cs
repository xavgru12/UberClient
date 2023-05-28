// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.CheckApplicationVersionView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public override string ToString() => "[CheckApplicationVersionView: [ClientVersion: " + this.ClientVersion?.ToString() + "][CurrentVersion: " + this.CurrentVersion?.ToString() + "]]";
  }
}
