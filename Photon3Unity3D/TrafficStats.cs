// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.TrafficStats
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

namespace ExitGames.Client.Photon
{
  public class TrafficStats
  {
    public int PackageHeaderSize { get; internal set; }

    public int ReliableCommandCount { get; internal set; }

    public int UnreliableCommandCount { get; internal set; }

    public int FragmentCommandCount { get; internal set; }

    public int ControlCommandCount { get; internal set; }

    public int TotalPacketCount { get; internal set; }

    public int TotalCommandsInPackets { get; internal set; }

    public int ReliableCommandBytes { get; internal set; }

    public int UnreliableCommandBytes { get; internal set; }

    public int FragmentCommandBytes { get; internal set; }

    public int ControlCommandBytes { get; internal set; }

    internal TrafficStats(int packageHeaderSize) => this.PackageHeaderSize = packageHeaderSize;

    public int TotalCommandCount => this.ReliableCommandCount + this.UnreliableCommandCount + this.FragmentCommandCount + this.ControlCommandCount;

    public int TotalCommandBytes => this.ReliableCommandBytes + this.UnreliableCommandBytes + this.FragmentCommandBytes + this.ControlCommandBytes;

    public int TotalPacketBytes => this.TotalCommandBytes + this.TotalPacketCount * this.PackageHeaderSize;

    internal void CountControlCommand(int size)
    {
      this.ControlCommandBytes += size;
      ++this.ControlCommandCount;
    }

    internal void CountReliableOpCommand(int size)
    {
      this.ReliableCommandBytes += size;
      ++this.ReliableCommandCount;
    }

    internal void CountUnreliableOpCommand(int size)
    {
      this.UnreliableCommandBytes += size;
      ++this.UnreliableCommandCount;
    }

    internal void CountFragmentOpCommand(int size)
    {
      this.FragmentCommandBytes += size;
      ++this.FragmentCommandCount;
    }

    public override string ToString() => string.Format("TotalPacketBytes: {0} TotalCommandBytes: {1} TotalPacketCount: {2} TotalCommandsInPackets: {3}", (object) this.TotalPacketBytes, (object) this.TotalCommandBytes, (object) this.TotalPacketCount, (object) this.TotalCommandsInPackets);
  }
}
