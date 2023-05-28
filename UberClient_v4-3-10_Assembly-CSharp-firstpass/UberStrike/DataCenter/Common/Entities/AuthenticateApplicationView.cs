// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.AuthenticateApplicationView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
