// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.GameApplicationView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
