// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.AuthenticateApplicationView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using Cmune.Core.Models.Views;
using System;
using System.Collections.Generic;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class AuthenticateApplicationView
  {
    public List<PhotonView> GameServers { get; set; }

    public PhotonView CommServer { get; set; }

    public bool WarnPlayer { get; set; }

    public bool IsEnabled { get; set; }

    public string EncryptionInitVector { get; set; }

    public string EncryptionPassPhrase { get; set; }
  }
}
