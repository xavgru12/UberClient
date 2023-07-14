// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CmuneNetworkClass
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace UberStrike.Realtime.UnitySdk
{
  public abstract class CmuneNetworkClass : INetworkClass, IDisposable
  {
    protected bool _isDisposed = false;
    private Action<int> _castEvents;
    public static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
    protected Dictionary<string, byte> _lookupNameIndex;
    protected Dictionary<byte, MethodInfo> _lookupIndexMethod;
    protected int _instanceID;
    private short? _netid;
    private Type _myType;

    protected CmuneNetworkClass()
    {
      this._myType = this.GetType();
      this._instanceID = 0;
      this._lookupNameIndex = new Dictionary<string, byte>();
      this._lookupIndexMethod = new Dictionary<byte, MethodInfo>();
      AttributeFinder.FindNetworkMethods(this._myType, ref this._lookupNameIndex, ref this._lookupIndexMethod);
    }

    public void Initialize(short id)
    {
      this._netid = new short?(id);
      this.OnInitialized();
    }

    public void Uninitialize()
    {
      this.OnUninitialized();
      this._netid = new short?();
    }

    protected virtual void OnInitialized() => this.CastEvent(1);

    protected virtual void OnUninitialized() => this.CastEvent(0);

    public void SubscribeToEvents(Action<int> callback) => this._castEvents += callback;

    public void UnsubscribeToEvents(Action<int> callback) => this._castEvents -= callback;

    protected void CastEvent(int eventCode)
    {
      if (this._castEvents == null)
        return;
      this._castEvents(eventCode);
    }

    public void Dispose() => this.Dispose(true);

    protected virtual void Dispose(bool dispose)
    {
      if (this._isDisposed)
        return;
      if (dispose)
      {
        this.Uninitialize();
        this._castEvents = (Action<int>) null;
      }
      this._isDisposed = true;
    }

    public bool HasMethod(byte address) => this._lookupIndexMethod.ContainsKey(address);

    public string GetMethodName(byte address)
    {
      MethodInfo methodInfo;
      if (this._lookupIndexMethod.TryGetValue(address, out methodInfo))
        return methodInfo.Name;
      CmuneDebug.LogError("GetMethodName({0}) failed because not found: {1}", (object) address, (object) CmunePrint.Dictionary((IDictionary) this._lookupIndexMethod));
      return string.Format("<{0}>", (object) address);
    }

    protected virtual bool TryGetStaticNetworkClassId(out short netid)
    {
      object[] customAttributes = this._myType.GetCustomAttributes(typeof (NetworkClassAttribute), true);
      if (customAttributes.Length > 0)
      {
        netid = ((NetworkClassAttribute) customAttributes[0]).ID;
        return true;
      }
      netid = (short) -1;
      return false;
    }

    public void CallMethod(byte localAddress, params object[] args)
    {
      MethodInfo methodInfo;
      if (this._lookupIndexMethod.TryGetValue(localAddress, out methodInfo))
      {
        try
        {
          this._myType.InvokeMember(methodInfo.Name, CmuneNetworkClass.Flags, (Binder) null, (object) this, args);
        }
        catch (MissingMethodException ex)
        {
          CmuneDebug.LogWarning("{0}:CallMethod('{1}') failed when called with arguments: {2}", (object) this.GetType(), (object) localAddress, (object) CmunePrint.Types(args));
          throw;
        }
        catch (Exception ex)
        {
          if (args != null)
            CmuneDebug.LogWarning("Method with address '{0}' was called with {1} arguments: {2}", (object) localAddress, (object) args.Length, (object) CmunePrint.Types(args));
          else
            CmuneDebug.LogWarning("Method with address '{0}' was called NULL argument", (object) localAddress);
          throw CmuneDebug.Exception("Exception when calling {0}:{1}() by reflection:\n>{2}\n{3}", (object) this._myType.Name, (object) methodInfo.Name, (object) ex.InnerException.Message, (object) ex.InnerException.StackTrace);
        }
      }
      else
        CmuneDebug.LogError("{0}:CallMethod failed because local address '{1}' not linked to a function!", (object) this._myType.Name, (object) localAddress);
    }

    public short NetworkID => this._netid.HasValue ? this._netid.Value : (short) -1;

    public int InstanceID => this._instanceID;

    public bool IsInitialized => this._netid.HasValue;

    public bool IsGlobal => true;

    public class EventType : ExtendableEnum<int>
    {
      public const int Uninitialized = 0;
      public const int Initialized = 1;
    }
  }
}
