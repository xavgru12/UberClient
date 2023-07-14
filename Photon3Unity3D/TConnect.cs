// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.TConnect
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace ExitGames.Client.Photon
{
  internal class TConnect
  {
    internal const int TCP_HEADER_BYTES = 7;
    private const int MSG_HEADER_BYTES = 2;
    private const int ALL_HEADER_BYTES = 9;
    private EndPoint serverEndPoint;
    internal bool obsolete;
    internal bool isRunning;
    internal TPeer peer;
    private Socket socketConnection;

    internal TConnect(TPeer npeer, string ipPort)
    {
      if (npeer.debugOut >= DebugLevel.ALL)
        npeer.Listener.DebugReturn(DebugLevel.ALL, "new TConnect()");
      this.peer = npeer;
    }

    internal bool StartConnection()
    {
      if (this.isRunning)
      {
        if (this.peer.debugOut >= DebugLevel.ERROR)
          this.peer.Listener.DebugReturn(DebugLevel.ERROR, "startConnectionThread() failed: connection thread still running.");
        return false;
      }
      this.socketConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      this.socketConnection.NoDelay = true;
      new Thread(new ThreadStart(this.Run)).Start();
      return true;
    }

    internal void StopConnection()
    {
      if (this.peer.debugOut >= DebugLevel.ALL)
        this.peer.Listener.DebugReturn(DebugLevel.ALL, "StopConnection()");
      this.obsolete = true;
      if (this.socketConnection == null)
        return;
      this.socketConnection.Close();
    }

    public void sendTcp(byte[] opData)
    {
      if (this.obsolete)
      {
        if (this.peer.debugOut < DebugLevel.INFO)
          return;
        this.peer.Listener.DebugReturn(DebugLevel.INFO, "Sending was skipped because connection is obsolete. " + Environment.StackTrace);
      }
      else
      {
        try
        {
          this.socketConnection.Send(opData);
        }
        catch (NullReferenceException ex)
        {
          if (this.peer.debugOut >= DebugLevel.ERROR)
            this.peer.Listener.DebugReturn(DebugLevel.ERROR, ex.Message);
        }
        catch (SocketException ex)
        {
          if (this.peer.debugOut >= DebugLevel.ERROR)
            this.peer.Listener.DebugReturn(DebugLevel.ERROR, ex.Message);
        }
      }
    }

    public void Run()
    {
      try
      {
        this.serverEndPoint = PeerBase.GetEndpoint(this.peer.ServerAddress);
        if (this.serverEndPoint == null)
        {
          if (this.peer.debugOut < DebugLevel.ERROR)
            return;
          this.peer.Listener.DebugReturn(DebugLevel.ERROR, "StartConnection() failed. Address must be 'address:port'. Is: " + this.peer.ServerAddress);
          return;
        }
        this.socketConnection.Connect(this.serverEndPoint);
      }
      catch (SecurityException ex)
      {
        if (this.peer.debugOut >= DebugLevel.INFO)
          this.peer.Listener.DebugReturn(DebugLevel.INFO, "Connect() failed: " + ex.ToString());
        if (this.socketConnection != null)
          this.socketConnection.Close();
        this.isRunning = false;
        this.obsolete = true;
        this.peer.EnqueueStatusCallback(StatusCode.ExceptionOnConnect);
        this.peer.EnqueueActionForDispatch((PeerBase.MyAction) (() => this.peer.Disconnected()));
        return;
      }
      catch (SocketException ex)
      {
        if (this.peer.debugOut >= DebugLevel.INFO)
          this.peer.Listener.DebugReturn(DebugLevel.INFO, "Connect() failed: " + ex.ToString());
        if (this.socketConnection != null)
          this.socketConnection.Close();
        this.isRunning = false;
        this.obsolete = true;
        this.peer.EnqueueStatusCallback(StatusCode.ExceptionOnConnect);
        this.peer.EnqueueActionForDispatch((PeerBase.MyAction) (() => this.peer.Disconnected()));
        return;
      }
      this.obsolete = false;
      byte[] numArray = (byte[]) null;
      if (this.peer.ProxyNodeId > (byte) 0)
      {
        int offset = 0;
        byte[] buffer = new byte[2];
        while (offset < 2)
          offset += this.socketConnection.Receive(buffer, offset, 2 - offset, SocketFlags.None);
        if (buffer[0] == (byte) 241)
        {
          this.peer.ReceiveProxyResponse(buffer[1]);
        }
        else
        {
          if (this.peer.debugOut >= DebugLevel.ERROR)
            this.peer.EnqueueDebugReturn(DebugLevel.ERROR, string.Format("Routing response did not start with: {0:x} but with: {1:x}" + (object) 241, (object) buffer[1]));
          numArray = buffer;
        }
      }
      this.isRunning = true;
      while (!this.obsolete)
      {
        MemoryStream opCollectionStream = new MemoryStream(256);
        try
        {
          int offset = 0;
          byte[] inBuff = new byte[9];
          if (numArray != null)
          {
            offset = 2;
            inBuff[0] = numArray[0];
            inBuff[1] = numArray[1];
            numArray = (byte[]) null;
          }
          while (offset < 9)
          {
            offset += this.socketConnection.Receive(inBuff, offset, 9 - offset, SocketFlags.None);
            if (offset == 0)
            {
              this.peer.SendPing();
              Thread.Sleep(100);
            }
          }
          if (inBuff[0] == (byte) 240)
          {
            if (this.peer.TrafficStatsEnabled)
              this.peer.TrafficStatsIncoming.CountControlCommand(inBuff.Length);
            if (this.peer.NetworkSimulationSettings.IsSimulationEnabled)
              this.peer.ReceiveNetworkSimulated((PeerBase.MyAction) (() => this.peer.receiveIncomingCommands(inBuff)));
            else
              this.peer.receiveIncomingCommands(inBuff);
          }
          else
          {
            int size = (int) inBuff[1] << 24 | (int) inBuff[2] << 16 | (int) inBuff[3] << 8 | (int) inBuff[4];
            if (this.peer.TrafficStatsEnabled)
            {
              if (inBuff[5] == (byte) 0)
                this.peer.TrafficStatsIncoming.CountReliableOpCommand(size);
              else
                this.peer.TrafficStatsIncoming.CountUnreliableOpCommand(size);
            }
            if (this.peer.debugOut >= DebugLevel.ALL)
              this.peer.EnqueueDebugReturn(DebugLevel.ALL, "message length: " + (object) size);
            opCollectionStream.Write(inBuff, 7, offset - 7);
            int num = 0;
            int length = size - 9;
            inBuff = new byte[length];
            while (num < length)
              num += this.socketConnection.Receive(inBuff, num, length - num, SocketFlags.None);
            opCollectionStream.Write(inBuff, 0, num);
            if (opCollectionStream.Length > 0L)
            {
              if (this.peer.NetworkSimulationSettings.IsSimulationEnabled)
                this.peer.ReceiveNetworkSimulated((PeerBase.MyAction) (() => this.peer.receiveIncomingCommands(opCollectionStream.ToArray())));
              else
                this.peer.receiveIncomingCommands(opCollectionStream.ToArray());
            }
            if (this.peer.debugOut >= DebugLevel.ALL)
              this.peer.EnqueueDebugReturn(DebugLevel.ALL, "TCP < " + (object) opCollectionStream.Length);
          }
        }
        catch (SocketException ex)
        {
          if (!this.obsolete)
          {
            this.obsolete = true;
            if (this.peer.debugOut >= DebugLevel.ERROR)
              this.peer.EnqueueDebugReturn(DebugLevel.ERROR, "Receiving failed. SocketException: " + (object) ex.SocketErrorCode);
            switch (ex.SocketErrorCode)
            {
              case SocketError.ConnectionAborted:
              case SocketError.ConnectionReset:
                this.peer.EnqueueStatusCallback(StatusCode.DisconnectByServer);
                break;
              default:
                this.peer.EnqueueStatusCallback(StatusCode.Exception);
                break;
            }
          }
        }
        catch (Exception ex)
        {
          if (!this.obsolete)
          {
            if (this.peer.debugOut >= DebugLevel.ERROR)
              this.peer.EnqueueDebugReturn(DebugLevel.ERROR, "Receiving failed. Exception: " + ex.ToString());
          }
        }
      }
      if (this.socketConnection != null)
        this.socketConnection.Close();
      this.isRunning = false;
      this.obsolete = true;
      this.peer.EnqueueActionForDispatch((PeerBase.MyAction) (() => this.peer.Disconnected()));
    }
  }
}
