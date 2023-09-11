
using Cmune.Realtime.Common.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cmune.Realtime.Common
{
  public struct ServerLoadData : IByteArray
  {
    public int Latency;
    public DateTime TimeStamp;
    public ServerLoadData.Status State;

    private ServerLoadData(int d)
      : this()
    {
      this.Latency = 0;
      this.TimeStamp = DateTime.MinValue;
      this.State = ServerLoadData.Status.None;
      this.MaxPlayerCount = 250f;
      this.PeersConnected = 0;
      this.PlayersConnected = 0;
      this.RoomsCreated = 0;
    }

    public ServerLoadData(byte[] data)
      : this(0)
    {
      this.FromBytes(data, 0);
    }

    public ServerLoadData(byte[] data, ref int idx)
      : this(0)
    {
      idx = this.FromBytes(data, idx);
    }

    public static ServerLoadData Empty => new ServerLoadData(0);

    public int FromBytes(byte[] bytes, int idx)
    {
      this.PlayersConnected = DefaultByteConverter.ToInt(bytes, ref idx);
      this.PeersConnected = DefaultByteConverter.ToInt(bytes, ref idx);
      this.RoomsCreated = DefaultByteConverter.ToInt(bytes, ref idx);
      this.MaxPlayerCount = DefaultByteConverter.ToFloat(bytes, ref idx);
      return idx;
    }

    public byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>(12);
      DefaultByteConverter.FromInt(this.PlayersConnected, ref bytes);
      DefaultByteConverter.FromInt(this.PeersConnected, ref bytes);
      DefaultByteConverter.FromInt(this.RoomsCreated, ref bytes);
      DefaultByteConverter.FromFloat(this.MaxPlayerCount, ref bytes);
      return bytes.ToArray();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("PlayersConnected: {0}\n", (object) this.PlayersConnected);
      stringBuilder.AppendFormat("PeersConnected: {0}\n", (object) this.PeersConnected);
      stringBuilder.AppendFormat("RoomsCreated: {0}\n", (object) this.RoomsCreated);
      stringBuilder.AppendFormat("MaxPlayerCount: {0}\n", (object) this.MaxPlayerCount);
      stringBuilder.AppendFormat("Ping: {0}\n", (object) this.Latency);
      return stringBuilder.ToString();
    }

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
