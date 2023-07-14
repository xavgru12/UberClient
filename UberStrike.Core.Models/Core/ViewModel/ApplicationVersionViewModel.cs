// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.ApplicationVersionViewModel
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
      invalidStates = "";
      if (!flag)
        invalidStates = "Invalid Model, unknown version";
      return flag;
    }
  }
}
