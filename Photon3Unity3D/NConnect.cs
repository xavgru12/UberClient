// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.NConnect
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace ExitGames.Client.Photon
{
  internal class NConnect
  {
    private IPAddress serverIpAddress = (IPAddress) null;
    private int serverPort;
    internal bool obsolete;
    internal bool isRunning;
    internal EnetPeer peer;
    private Socket sock;
    private object syncer = new object();

    internal NConnect(EnetPeer npeer, string ipPort)
    {
      if (npeer.debugOut >= DebugLevel.ALL)
        npeer.Listener.DebugReturn(DebugLevel.ALL, "new NConnect UDP.");
      this.peer = npeer;
      int length = ipPort.IndexOf(':');
      string str = ipPort.Substring(0, length);
      this.serverPort = (int) short.Parse(ipPort.Substring(length + 1));
      if (npeer.debugOut >= DebugLevel.INFO)
        this.peer.Listener.DebugReturn(DebugLevel.INFO, "Remote IP: " + str + ":" + (object) this.serverPort);
      this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      try
      {
        this.serverIpAddress = IPAddress.Parse(str);
      }
      catch (FormatException ex)
      {
      }
      if (this.serverIpAddress != null)
        return;
      try
      {
        foreach (IPAddress address in Dns.GetHostEntry(str).AddressList)
        {
          if (address.AddressFamily == AddressFamily.InterNetwork)
          {
            this.serverIpAddress = address;
            break;
          }
        }
      }
      catch (Exception ex)
      {
        if (this.peer.debugOut >= DebugLevel.ERROR)
          this.peer.Listener.DebugReturn(DebugLevel.ERROR, "Dns.GetHostEntry(" + str + ") failed: " + ex.ToString());
      }
    }

    internal bool StartConnection()
    {
      if (this.isRunning)
      {
        if (this.peer.debugOut >= DebugLevel.ERROR)
          this.peer.Listener.DebugReturn(DebugLevel.ERROR, "StartConnection() failed: connection still open.");
        return false;
      }
      try
      {
        lock (this.syncer)
          this.sock.Connect(this.serverIpAddress, this.serverPort);
      }
      catch (SecurityException ex)
      {
        if (this.peer.debugOut >= DebugLevel.ERROR)
          this.peer.Listener.DebugReturn(DebugLevel.ERROR, "Connect() failed: " + ex.ToString());
        this.peer.Listener.OnStatusChanged(StatusCode.SecurityExceptionOnConnect);
        this.peer.Listener.OnStatusChanged(StatusCode.Disconnect);
        return false;
      }
      catch (Exception ex)
      {
        if (this.peer.debugOut >= DebugLevel.ERROR)
          this.peer.Listener.DebugReturn(DebugLevel.ERROR, "Connect() failed: " + ex.ToString());
        this.peer.Listener.OnStatusChanged(StatusCode.ExceptionOnConnect);
        this.peer.Listener.OnStatusChanged(StatusCode.Disconnect);
        return false;
      }
      this.obsolete = false;
      new Thread(new ThreadStart(this.Run)).Start();
      return true;
    }

    internal void StopConnection()
    {
      if (this.peer.debugOut >= DebugLevel.INFO)
        this.peer.Listener.DebugReturn(DebugLevel.INFO, "StopConnection()");
      lock (this.syncer)
      {
        this.obsolete = true;
        this.sock.Close();
      }
    }

    internal void SendUdpPackage(byte[] data, int length)
    {
      lock (this.syncer)
      {
        if (!this.sock.Connected)
          return;
        this.sock.Send(data, 0, length, SocketFlags.None);
      }
    }

    public void Run()
    {
      this.peer.queueOutgoingReliableCommand(new NCommand(this.peer, (byte) 2, (byte[]) null, byte.MaxValue));
      this.isRunning = true;
      while (!this.obsolete)
      {
        try
        {
          while (this.sock.Available <= 0)
            Thread.Sleep(1);
          byte[] inBuffer;
          lock (this.syncer)
          {
            inBuffer = new byte[this.sock.Available];
            this.sock.Receive(inBuffer);
          }
          if (this.peer.NetworkSimulationSettings.IsSimulationEnabled)
            this.peer.ReceiveNetworkSimulated((PeerBase.MyAction) (() => this.peer.receiveIncomingCommands(inBuffer)));
          else
            this.peer.receiveIncomingCommands(inBuffer);
        }
        catch (Exception ex)
        {
          if (!this.obsolete)
          {
            if (this.peer.debugOut >= DebugLevel.ERROR)
              this.peer.EnqueueDebugReturn(DebugLevel.ERROR, "Error trying to receive. Exception: " + (object) ex);
            this.peer.EnqueueStatusCallback(StatusCode.Exception);
          }
          this.obsolete = true;
        }
      }
      if (this.sock != null)
      {
        lock (this.syncer)
          this.sock.Close();
      }
      this.isRunning = false;
      this.obsolete = true;
      this.peer.EnqueueActionForDispatch((PeerBase.MyAction) (() => this.peer.Disconnected()));
    }
  }
}
