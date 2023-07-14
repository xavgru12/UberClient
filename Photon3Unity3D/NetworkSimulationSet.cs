// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.NetworkSimulationSet
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;
using System.Threading;

namespace ExitGames.Client.Photon
{
  public class NetworkSimulationSet
  {
    private bool isSimulationEnabled = false;
    private int outgoingLag = 100;
    private int outgoingJitter = 0;
    private int outgoingLossPercentage = 1;
    private int incomingLag = 100;
    private int incomingJitter = 0;
    private int incomingLossPercentage = 1;

    internal ThreadStart SimulationMethod { get; set; }

    private void checkSimulationNeeded()
    {
      if (this.outgoingLag + this.outgoingJitter + this.outgoingLossPercentage + this.incomingLag + this.incomingJitter + this.incomingLossPercentage > 0)
        return;
      this.IsSimulationEnabled = false;
    }

    public bool IsSimulationEnabled
    {
      get => this.isSimulationEnabled;
      set
      {
        this.isSimulationEnabled = value;
        if (!this.isSimulationEnabled)
          return;
        new Thread(this.SimulationMethod)
        {
          IsBackground = true,
          Name = ("netSim" + (object) Environment.TickCount)
        }.Start();
      }
    }

    public int OutgoingLag
    {
      get => this.outgoingLag;
      set
      {
        this.outgoingLag = value;
        this.checkSimulationNeeded();
      }
    }

    public int OutgoingJitter
    {
      get => this.outgoingJitter;
      set
      {
        this.outgoingJitter = value;
        this.checkSimulationNeeded();
      }
    }

    public int OutgoingLossPercentage
    {
      get => this.outgoingLossPercentage;
      set
      {
        this.outgoingLossPercentage = value;
        this.checkSimulationNeeded();
      }
    }

    public int IncomingLag
    {
      get => this.incomingLag;
      set
      {
        this.incomingLag = value;
        this.checkSimulationNeeded();
      }
    }

    public int IncomingJitter
    {
      get => this.incomingJitter;
      set
      {
        this.incomingJitter = value;
        this.checkSimulationNeeded();
      }
    }

    public int IncomingLossPercentage
    {
      get => this.incomingLossPercentage;
      set
      {
        this.incomingLossPercentage = value;
        this.checkSimulationNeeded();
      }
    }

    public int LostPackagesOut { get; internal set; }

    public int LostPackagesIn { get; internal set; }

    public override string ToString() => string.Format("NetworkSimulationSet {6}.  Lag in={0} out={1}. Jitter in={2} out={3}. Loss in={4} out={5}.", (object) this.incomingLag, (object) this.outgoingLag, (object) this.incomingJitter, (object) this.outgoingJitter, (object) this.incomingLossPercentage, (object) this.outgoingLossPercentage, (object) this.IsSimulationEnabled);
  }
}
