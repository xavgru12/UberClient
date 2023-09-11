// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Photon.Client.RemoteMethodInterface
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common;
using Cmune.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cmune.Realtime.Photon.Client
{
  public class RemoteMethodInterface
  {
    public const int IncomingMessageQueueLimit = 100;
    private Dictionary<short, INetworkClass> _registeredClasses;
    private Dictionary<short, Queue<RemoteMethodInterface.RemoteProcedureCall>> _incomingRpcBuffer;
    private Dictionary<int, RemoteMethodInterface.RegistrationJob> _waitingClasses;
    private Dictionary<int, short> _networkInstantiatedObjects;
    private NetworkMessenger _messenger;
    private SynchronizationCenter _syncCenter;

    public RemoteMethodInterface(NetworkMessenger messenger)
    {
      this._messenger = messenger;
      this._registeredClasses = new Dictionary<short, INetworkClass>();
      this._waitingClasses = new Dictionary<int, RemoteMethodInterface.RegistrationJob>();
      this._networkInstantiatedObjects = new Dictionary<int, short>();
      this._incomingRpcBuffer = new Dictionary<short, Queue<RemoteMethodInterface.RemoteProcedureCall>>();
      this._syncCenter = new SynchronizationCenter(this);
    }

    public void RegisterGlobalNetworkClass(INetworkClass netClass, short networkID)
    {
      if (!netClass.IsGlobal)
        CmuneDebug.LogError("Use RegisterMonoNetworkClass(ClientNetworkClass) for Network classes with UNSET network ID", new object[0]);
      else if (networkID == (short) 1 || networkID < (short) 0)
      {
        this._registeredClasses[networkID] = netClass;
        netClass.Initialize(networkID);
      }
      else
      {
        this._waitingClasses[netClass.InstanceID] = new RemoteMethodInterface.RegistrationJob(netClass, networkID);
        this.RegisterAllClasses();
      }
    }

    public void RegisterMonoNetworkClass(INetworkClass netClass)
    {
      if (netClass.IsGlobal)
      {
        CmuneDebug.LogError("Use RegisterNetworkClass(ClientNetworkClass,int,short) for Network classes with FIXED network ID", new object[0]);
      }
      else
      {
        short networkID;
        if (this._networkInstantiatedObjects.TryGetValue(netClass.InstanceID, out networkID))
        {
          this.RegisterNetworkClass(netClass, networkID);
          this._networkInstantiatedObjects.Remove(netClass.InstanceID);
        }
        else
        {
          this._waitingClasses[netClass.InstanceID] = new RemoteMethodInterface.RegistrationJob(netClass);
          this.RegisterAllClasses();
        }
      }
    }

    public void RegisterAllClasses()
    {
      foreach (RemoteMethodInterface.RegistrationJob registrationJob in this._waitingClasses.Values)
      {
        if (this._messenger.PeerListener.HasJoinedRoom && !registrationJob.IsRequestSent)
        {
          if (registrationJob.NetworkID.HasValue)
          {
            this._messenger.SendMessageToServer((short) 2, (byte) 3, (object) this._messenger.PeerListener.ActorIdSecure, (object) registrationJob.LocalID, (object) registrationJob.NetworkID);
            registrationJob.IsRequestSent = true;
          }
          else
            CmuneDebug.LogError("RegisterAllClasses failed because NetworkClass is not static", new object[0]);
        }
      }
    }

    public void UnregisterAllClasses()
    {
      if (CmuneNetworkState.DebugMessaging)
        CmuneDebug.Log("{0} - UnregisterAllClasses", (object) this._messenger.PeerListener.SessionID);
      List<INetworkClass> networkClassList = new List<INetworkClass>();
      foreach (RemoteMethodInterface.RegistrationJob registrationJob in this._waitingClasses.Values)
        registrationJob.IsRequestSent = false;
      foreach (INetworkClass netClass in this._registeredClasses.Values)
      {
        if (netClass.IsGlobal)
        {
          if (netClass.NetworkID == (short) 1 || netClass.NetworkID < (short) 0)
            networkClassList.Add(netClass);
          else if (!this._waitingClasses.ContainsKey(netClass.InstanceID))
          {
            this._waitingClasses.Add(netClass.InstanceID, new RemoteMethodInterface.RegistrationJob(netClass, netClass.NetworkID));
            netClass.Uninitialize();
          }
        }
        else if (!this._waitingClasses.ContainsKey(netClass.InstanceID))
        {
          this._waitingClasses.Add(netClass.InstanceID, new RemoteMethodInterface.RegistrationJob(netClass));
          netClass.Uninitialize();
        }
      }
      this._registeredClasses.Clear();
      foreach (INetworkClass networkClass in networkClassList)
        this._registeredClasses.Add(networkClass.NetworkID, networkClass);
      this._incomingRpcBuffer.Clear();
    }

    internal void RecieveRegistrationConfirmation(int instanceID, short networkID)
    {
      RemoteMethodInterface.RegistrationJob registrationJob;
      if (this._waitingClasses.TryGetValue(instanceID, out registrationJob))
      {
        if (registrationJob.NetworkClass == null)
          return;
        try
        {
          if (CmuneNetworkState.DebugMessaging)
            CmuneDebug.Log("ServerStaticRegistrationConfirmation " + (object) networkID, new object[0]);
          this.RegisterNetworkClass(registrationJob.NetworkClass, networkID);
        }
        catch (Exception ex)
        {
          CmuneDebug.LogError("Failed Registering Static NetworkClass ({0}): {1}", (object) networkID, (object) ex.Message);
          CmuneDebug.LogError("Registered Classes: {0}", (object) CmunePrint.Values((object) this._registeredClasses.Keys));
        }
        finally
        {
          this._waitingClasses.Remove(instanceID);
        }
      }
      else
        CmuneDebug.LogError(string.Format("RecieveRegistrationConfirmation({0}, {1}) failed because Instance was deleted!", (object) instanceID, (object) networkID), new object[0]);
    }

    public void RegisterNetworkClass(INetworkClass netClass, short networkID)
    {
      this._registeredClasses.Add(networkID, netClass);
      netClass.Initialize(networkID);
      this.ExcecuteWaitingFunctionCalls(netClass);
    }

    internal void ExcecuteWaitingFunctionCalls(INetworkClass netClass)
    {
      Queue<RemoteMethodInterface.RemoteProcedureCall> remoteProcedureCallQueue;
      if (!this._incomingRpcBuffer.TryGetValue(netClass.NetworkID, out remoteProcedureCallQueue))
        return;
      while (remoteProcedureCallQueue.Count > 0)
      {
        RemoteMethodInterface.RemoteProcedureCall remoteProcedureCall = remoteProcedureCallQueue.Dequeue();
        netClass.CallMethod(remoteProcedureCall.FunctionID, remoteProcedureCall.Args);
      }
    }

    public void DisposeNetworkClass(INetworkClass netClass)
    {
      if (CmuneNetworkState.DebugMessaging)
        CmuneDebug.Log("DisposeNetworkClass " + netClass.GetType().Name, new object[0]);
      this._waitingClasses.Remove(netClass.InstanceID);
      this._registeredClasses.Remove(netClass.NetworkID);
      this._networkInstantiatedObjects.Remove(netClass.InstanceID);
      this._incomingRpcBuffer.Remove(netClass.NetworkID);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (INetworkClass networkClass in this._registeredClasses.Values)
      {
        if (networkClass.IsGlobal)
          stringBuilder.AppendFormat("REG GL {0} with LID {1} and NID {2}\n", (object) networkClass.GetType().Name, (object) networkClass.InstanceID, (object) networkClass.NetworkID);
        else
          stringBuilder.AppendFormat("REG {0} with LID {1} and NID {2}\n", (object) networkClass.GetType().Name, (object) networkClass.InstanceID, (object) networkClass.NetworkID);
      }
      foreach (RemoteMethodInterface.RegistrationJob registrationJob in this._waitingClasses.Values)
      {
        if (registrationJob.NetworkClass.IsGlobal)
          stringBuilder.AppendFormat("WAIT GL {0} with LID {1} ({2})\n", (object) registrationJob.NetworkClass.GetType().Name, (object) registrationJob.LocalID, (object) registrationJob.IsRequestSent);
        else
          stringBuilder.AppendFormat("WAIT {0} with LID {1} ({2})\n", (object) registrationJob.NetworkClass.GetType().Name, (object) registrationJob.LocalID, (object) registrationJob.IsRequestSent);
      }
      foreach (KeyValuePair<int, short> instantiatedObject in this._networkInstantiatedObjects)
        stringBuilder.AppendFormat("Net Instance with LID {0} and NID {1}\n", (object) instantiatedObject.Key, (object) instantiatedObject.Value);
      return stringBuilder.ToString();
    }

    public string GetAddress(short networkID, byte address)
    {
      string.Format("<{0}.{1}>", (object) networkID, (object) address);
      string address1;
      if (this._registeredClasses.ContainsKey(networkID) && this._registeredClasses[networkID] != null)
      {
        INetworkClass registeredClass = this._registeredClasses[networkID];
        address1 = string.Format("{0}.{1}(registered)", (object) registeredClass.GetType().Name, (object) registeredClass.GetMethodName(address));
      }
      else
        address1 = string.Format("{0}.{1}(waiting)", (object) "asdf", (object) "asdf");
      return address1;
    }

    public void Clear()
    {
      if (CmuneNetworkState.DebugMessaging)
        CmuneDebug.Log("Clear RMI", new object[0]);
      this._networkInstantiatedObjects.Clear();
      this._incomingRpcBuffer.Clear();
    }

    public bool TryGetNetworkClassWithID(short id, out INetworkClass net) => this._registeredClasses.TryGetValue(id, out net);

    internal void AddRemoteNetworkClassInstance(int instanceID, short networkID) => this._networkInstantiatedObjects.Add(instanceID, networkID);

    internal void RecieveMessage(short networkID, byte functionID, object[] args)
    {
      if (this._registeredClasses.ContainsKey(networkID))
      {
        this._registeredClasses[networkID].CallMethod(functionID, args);
      }
      else
      {
        if (!this._incomingRpcBuffer.ContainsKey(networkID))
          this._incomingRpcBuffer.Add(networkID, new Queue<RemoteMethodInterface.RemoteProcedureCall>());
        if (this._incomingRpcBuffer[networkID].Count >= 100)
          throw new CmuneException(string.Format("Recieved Message {0} for NetworkID {1} but instance not registered. QUEUE FULL!", (object) functionID, (object) networkID), new object[0]);
        this._incomingRpcBuffer[networkID].Enqueue(new RemoteMethodInterface.RemoteProcedureCall(functionID, args));
        CmuneDebug.LogWarning(string.Format("Recieved Message {0} for NetworkID {1} but instance not registered. #{2}", (object) functionID, (object) networkID, (object) this._incomingRpcBuffer[networkID].Count), new object[0]);
      }
    }

    public NetworkMessenger Messenger => this._messenger;

    public ICollection<INetworkClass> RegisteredClasses => (ICollection<INetworkClass>) this._registeredClasses.Values;

    public ICollection<RemoteMethodInterface.RegistrationJob> RegistrationJobs => (ICollection<RemoteMethodInterface.RegistrationJob>) this._waitingClasses.Values;

    public ICollection<short> NetworkInstantiatedObjects => (ICollection<short>) this._networkInstantiatedObjects.Values;

    private class RemoteProcedureCall
    {
      public byte FunctionID;
      public object[] Args;

      public RemoteProcedureCall(byte functionID, object[] args)
      {
        this.FunctionID = functionID;
        if (args != null)
          this.Args = args;
        else
          this.Args = new object[0];
      }

      public override string ToString() => string.Format("RPC {0} with {1} args", (object) this.FunctionID, (object) this.Args.Length);
    }

    public class RegistrationJob
    {
      public INetworkClass NetworkClass;
      public bool IsRequestSent;
      public short? NetworkID;
      public int LocalID;

      public RegistrationJob(INetworkClass netClass)
      {
        this.NetworkClass = netClass;
        this.LocalID = netClass.InstanceID;
      }

      public RegistrationJob(INetworkClass netClass, short networkID)
        : this(netClass)
      {
        this.NetworkID = new short?(networkID);
      }

      public override string ToString() => this.NetworkClass != null ? string.Format("Iid {0}, Nid {1}, ReqSent {2}", (object) this.NetworkClass.InstanceID, (object) (short) ((int) this.NetworkID ?? -1), (object) this.IsRequestSent) : string.Format("RegistrationJob for {0} is NULL", (object) this.LocalID);
    }
  }
}
