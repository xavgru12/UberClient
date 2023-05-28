// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.ServerConnectionView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System;

namespace UberStrike.Core.ViewModel
{
  [Serializable]
  public class ServerConnectionView
  {
    public string ApiVersion { get; set; }

    public int Cmid { get; set; }

    public ChannelType Channel { get; set; }

    public MemberAccessLevel AccessLevel { get; set; }
  }
}
