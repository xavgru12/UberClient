// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.AuthenticateApplicationView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
