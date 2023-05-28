// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.GameApplicationView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using Cmune.Core.Models.Views;
using System;
using System.Collections.Generic;

namespace UberStrike.Core.ViewModel
{
  [Serializable]
  public class GameApplicationView
  {
    public string Version { get; set; }

    public List<PhotonView> GameServers { get; set; }

    public PhotonView CommServer { get; set; }

    public string SupportUrl { get; set; }

    public string EncryptionInitVector { get; set; }

    public string EncryptionPassPhrase { get; set; }
  }
}
