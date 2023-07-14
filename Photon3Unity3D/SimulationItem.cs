// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.SimulationItem
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System.Diagnostics;

namespace ExitGames.Client.Photon
{
  internal class SimulationItem
  {
    internal readonly Stopwatch stopw;
    public int TimeToExecute;
    public PeerBase.MyAction ActionToExecute;

    public SimulationItem()
    {
      this.stopw = new Stopwatch();
      this.stopw.Start();
    }

    public int Delay { get; internal set; }
  }
}
