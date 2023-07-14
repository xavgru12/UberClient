// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.NCommand
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;

namespace ExitGames.Client.Photon
{
  internal class NCommand : IComparable<NCommand>
  {
    internal const int FLAG_RELIABLE = 1;
    internal const int FLAG_UNSEQUENCED = 2;
    internal const byte FV_UNRELIABLE = 0;
    internal const byte FV_RELIABLE = 1;
    internal const byte FV_UNRELIBALE_UNSEQUENCED = 2;
    internal const byte CT_NONE = 0;
    internal const byte CT_ACK = 1;
    internal const byte CT_CONNECT = 2;
    internal const byte CT_VERIFYCONNECT = 3;
    internal const byte CT_DISCONNECT = 4;
    internal const byte CT_PING = 5;
    internal const byte CT_SENDRELIABLE = 6;
    internal const byte CT_SENDUNRELIABLE = 7;
    internal const byte CT_SENDFRAGMENT = 8;
    internal const byte CT_EG_SERVERTIME = 12;
    internal const int HEADER_UDP_PACK_LENGTH = 12;
    internal const int HEADER_LENGTH = 12;
    internal const int HEADER_FRAGMENT_LENGTH = 32;
    internal const int CmdSizeMinimum = 12;
    internal const int CmdSizeAck = 20;
    internal const int CmdSizeConnect = 44;
    internal const int CmdSizeVerifyConnect = 44;
    internal const int CmdSizeDisconnect = 12;
    internal const int CmdSizePing = 12;
    internal const int CmdSizeReliableHeader = 12;
    internal const int CmdSizeUnreliableHeader = 16;
    internal const int CmdSizeFragmentHeader = 32;
    internal byte commandFlags;
    internal byte commandType;
    internal byte commandChannelID;
    internal int reliableSequenceNumber;
    internal int unreliableSequenceNumber;
    internal int unsequencedGroupNumber;
    internal byte reservedByte;
    internal int startSequenceNumber;
    internal int fragmentCount;
    internal int fragmentNumber;
    internal int totalLength;
    internal int fragmentOffset;
    internal int fragmentsRemaining;
    internal byte[] Payload;
    internal int commandSentTime;
    internal byte commandSentCount;
    internal int roundTripTimeout;
    internal int timeoutTime;
    internal int ackReceivedReliableSequenceNumber;
    internal int ackReceivedSentTime;
    private byte[] completeCommand;
    internal int Size;

    internal NCommand(EnetPeer peer, byte commandType, byte[] payload, byte channel)
    {
      this.commandType = commandType;
      this.commandFlags = (byte) 1;
      this.commandChannelID = channel;
      this.Payload = payload;
      this.Size = 12;
      switch (this.commandType)
      {
        case 1:
          this.Size = 20;
          this.commandFlags = (byte) 0;
          break;
        case 2:
          this.Size = 44;
          this.Payload = new byte[32];
          this.Payload[0] = (byte) 0;
          this.Payload[1] = (byte) 0;
          SupportClass.NumberToByteArray(this.Payload, 2, (short) peer.mtu);
          this.Payload[4] = (byte) 0;
          this.Payload[5] = (byte) 0;
          this.Payload[6] = (byte) 128;
          this.Payload[7] = (byte) 0;
          this.Payload[11] = peer.ChannelCount;
          this.Payload[15] = (byte) 0;
          this.Payload[19] = (byte) 0;
          this.Payload[22] = (byte) 19;
          this.Payload[23] = (byte) 136;
          this.Payload[27] = (byte) 2;
          this.Payload[31] = (byte) 2;
          break;
        case 4:
          this.Size = 12;
          if (peer.peerConnectionState == PeerBase.ConnectionStateValue.Connected)
            break;
          this.commandFlags = (byte) 2;
          break;
        case 6:
          this.Size = 12 + payload.Length;
          break;
        case 7:
          this.Size = 16 + payload.Length;
          this.commandFlags = (byte) 0;
          break;
        case 8:
          this.Size = 32 + payload.Length;
          break;
      }
    }

    internal static NCommand CreateAck(EnetPeer peer, NCommand commandToAck, int sentTime)
    {
      byte[] numArray = new byte[8];
      int targetOffset = 0;
      Protocol.Serialize(commandToAck.reliableSequenceNumber, numArray, ref targetOffset);
      Protocol.Serialize(sentTime, numArray, ref targetOffset);
      return new NCommand(peer, (byte) 1, numArray, commandToAck.commandChannelID);
    }

    internal NCommand(EnetPeer peer, byte[] inBuff, ref int readingOffset)
    {
      this.commandType = inBuff[readingOffset++];
      this.commandChannelID = inBuff[readingOffset++];
      this.commandFlags = inBuff[readingOffset++];
      this.reservedByte = inBuff[readingOffset++];
      Protocol.Deserialize(out this.Size, inBuff, ref readingOffset);
      Protocol.Deserialize(out this.reliableSequenceNumber, inBuff, ref readingOffset);
      peer.bytesIn += (long) this.Size;
      switch (this.commandType)
      {
        case 1:
          Protocol.Deserialize(out this.ackReceivedReliableSequenceNumber, inBuff, ref readingOffset);
          Protocol.Deserialize(out this.ackReceivedSentTime, inBuff, ref readingOffset);
          break;
        case 3:
          short num;
          Protocol.Deserialize(out num, inBuff, ref readingOffset);
          readingOffset += 30;
          if (peer.peerID == (short) -1)
          {
            peer.peerID = num;
            break;
          }
          break;
        case 6:
          this.Payload = new byte[this.Size - 12];
          break;
        case 7:
          Protocol.Deserialize(out this.unreliableSequenceNumber, inBuff, ref readingOffset);
          this.Payload = new byte[this.Size - 16];
          break;
        case 8:
          Protocol.Deserialize(out this.startSequenceNumber, inBuff, ref readingOffset);
          Protocol.Deserialize(out this.fragmentCount, inBuff, ref readingOffset);
          Protocol.Deserialize(out this.fragmentNumber, inBuff, ref readingOffset);
          Protocol.Deserialize(out this.totalLength, inBuff, ref readingOffset);
          Protocol.Deserialize(out this.fragmentOffset, inBuff, ref readingOffset);
          this.Payload = new byte[this.Size - 32];
          this.fragmentsRemaining = this.fragmentCount;
          break;
      }
      if (this.Payload == null)
        return;
      Buffer.BlockCopy((Array) inBuff, readingOffset, (Array) this.Payload, 0, this.Payload.Length);
      readingOffset += this.Payload.Length;
    }

    internal byte[] Serialize()
    {
      if (this.completeCommand != null)
        return this.completeCommand;
      int length = this.Payload == null ? 0 : this.Payload.Length;
      int dstOffset = 12;
      if (this.commandType == (byte) 7)
        dstOffset = 16;
      else if (this.commandType == (byte) 8)
        dstOffset = 32;
      this.completeCommand = new byte[dstOffset + length];
      this.completeCommand[0] = this.commandType;
      this.completeCommand[1] = this.commandChannelID;
      this.completeCommand[2] = this.commandFlags;
      this.completeCommand[3] = (byte) 4;
      int targetOffset = 4;
      Protocol.Serialize(this.completeCommand.Length, this.completeCommand, ref targetOffset);
      Protocol.Serialize(this.reliableSequenceNumber, this.completeCommand, ref targetOffset);
      if (this.commandType == (byte) 7)
        SupportClass.NumberToByteArray(this.completeCommand, 12, this.unreliableSequenceNumber);
      else if (this.commandType == (byte) 8)
      {
        SupportClass.NumberToByteArray(this.completeCommand, 12, this.startSequenceNumber);
        SupportClass.NumberToByteArray(this.completeCommand, 16, this.fragmentCount);
        SupportClass.NumberToByteArray(this.completeCommand, 20, this.fragmentNumber);
        SupportClass.NumberToByteArray(this.completeCommand, 24, this.totalLength);
        SupportClass.NumberToByteArray(this.completeCommand, 28, this.fragmentOffset);
      }
      if (length > 0)
        Buffer.BlockCopy((Array) this.Payload, 0, (Array) this.completeCommand, dstOffset, length);
      this.Payload = (byte[]) null;
      return this.completeCommand;
    }

    public int CompareTo(NCommand other) => ((int) this.commandFlags & 1) != 0 ? this.reliableSequenceNumber - other.reliableSequenceNumber : this.unreliableSequenceNumber - other.unreliableSequenceNumber;

    public override string ToString() => string.Format("NC({0}|{1} r/u: {2}/{3} st/r#/rt:{4}/{5}/{6})", (object) this.commandChannelID, (object) this.commandType, (object) this.reliableSequenceNumber, (object) this.unreliableSequenceNumber, (object) this.commandSentTime, (object) this.commandSentCount, (object) this.timeoutTime);
  }
}
