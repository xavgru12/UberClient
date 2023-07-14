// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.EnetChannel
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System.Collections.Generic;

namespace ExitGames.Client.Photon
{
  internal class EnetChannel
  {
    internal byte ChannelNumber;
    internal Dictionary<int, NCommand> incomingReliableCommandsList;
    internal Dictionary<int, NCommand> incomingUnreliableCommandsList;
    internal Queue<NCommand> outgoingReliableCommandsList;
    internal Queue<NCommand> outgoingUnreliableCommandsList;
    internal int incomingReliableSequenceNumber;
    internal int incomingUnreliableSequenceNumber;
    internal int outgoingReliableSequenceNumber;
    internal int outgoingUnreliableSequenceNumber;

    public EnetChannel(byte channelNumber, int commandBufferSize)
    {
      this.ChannelNumber = channelNumber;
      this.incomingReliableCommandsList = new Dictionary<int, NCommand>(commandBufferSize);
      this.incomingUnreliableCommandsList = new Dictionary<int, NCommand>(commandBufferSize);
      this.outgoingReliableCommandsList = new Queue<NCommand>(commandBufferSize);
      this.outgoingUnreliableCommandsList = new Queue<NCommand>(commandBufferSize);
    }

    public bool ContainsUnreliableSequenceNumber(int unreliableSequenceNumber) => this.incomingUnreliableCommandsList.ContainsKey(unreliableSequenceNumber);

    public NCommand FetchUnreliableSequenceNumber(int unreliableSequenceNumber) => this.incomingUnreliableCommandsList[unreliableSequenceNumber];

    public bool ContainsReliableSequenceNumber(int reliableSequenceNumber) => this.incomingReliableCommandsList.ContainsKey(reliableSequenceNumber);

    public NCommand FetchReliableSequenceNumber(int reliableSequenceNumber) => this.incomingReliableCommandsList[reliableSequenceNumber];

    public void clearAll()
    {
      this.incomingReliableCommandsList.Clear();
      this.incomingUnreliableCommandsList.Clear();
      this.outgoingReliableCommandsList.Clear();
      this.outgoingUnreliableCommandsList.Clear();
    }
  }
}
