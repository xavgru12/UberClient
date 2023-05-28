// Decompiled with JetBrains decompiler
// Type: Cmune.Core.Models.PhotonServerLoad
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Cmune.Core.Models
{
  [Serializable]
  public class PhotonServerLoad
  {
    public int Latency;
    public DateTime TimeStamp;
    public PhotonServerLoad.Status State;

    public int PeersConnected { get; set; }

    public int PlayersConnected { get; set; }

    public int RoomsCreated { get; set; }

    public float MaxPlayerCount { get; set; }

    public enum Status
    {
      None,
      Alive,
      NotReachable,
    }
  }
}
