// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.ApplicationVersionViewModel
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System;

namespace UberStrike.Core.ViewModel
{
  public class ApplicationVersionViewModel
  {
    public int ApplicationVersionId { get; set; }

    public string Version { get; set; }

    public string WebPlayerFileName { get; set; }

    public ChannelType Channel { get; set; }

    public DateTime ModificationDate { get; set; }

    public bool IsEnabled { get; set; }

    public bool WarnPlayer { get; set; }

    public int PhotonClusterId { get; set; }

    public string PhotonClusterName { get; set; }

    public bool IsValid(out string invalidStates)
    {
      bool flag = !string.IsNullOrEmpty(this.Version) && this.Channel > ~ChannelType.WebPortal && this.ModificationDate > DateTime.MinValue && this.PhotonClusterId > 0;
      invalidStates = string.Empty;
      if (!flag)
        invalidStates = "Invalid Model, unknown version";
      return flag;
    }
  }
}
