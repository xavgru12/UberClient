// Decompiled with JetBrains decompiler
// Type: Cmune.Util.CmuneEventHandler
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace Cmune.Util
{
  public static class CmuneEventHandler
  {
    private static Dictionary<Type, CmuneEventHandler.IEventContainer> eventContainer = new Dictionary<Type, CmuneEventHandler.IEventContainer>();

    public static void AddListener<T>(Action<T> callback)
    {
      CmuneEventHandler.IEventContainer eventContainer1;
      if (!CmuneEventHandler.eventContainer.TryGetValue(typeof (T), out eventContainer1))
      {
        eventContainer1 = (CmuneEventHandler.IEventContainer) new CmuneEventHandler.EventContainer<T>();
        CmuneEventHandler.eventContainer.Add(typeof (T), eventContainer1);
      }
      if (!(eventContainer1 is CmuneEventHandler.EventContainer<T> eventContainer2))
        return;
      eventContainer2.AddCallbackMethod(callback);
    }

    public static void RemoveListener<T>(Action<T> callback)
    {
      CmuneEventHandler.IEventContainer eventContainer1;
      if (!CmuneEventHandler.eventContainer.TryGetValue(typeof (T), out eventContainer1) || !(eventContainer1 is CmuneEventHandler.EventContainer<T> eventContainer2))
        return;
      eventContainer2.RemoveCallbackMethod(callback);
    }

    public static void Route(object message)
    {
      CmuneEventHandler.IEventContainer eventContainer;
      if (!CmuneEventHandler.eventContainer.TryGetValue(message.GetType(), out eventContainer))
        return;
      eventContainer.CastEvent(message);
    }

    private interface IEventContainer
    {
      void CastEvent(object m);
    }

    private class EventContainer<T> : CmuneEventHandler.IEventContainer
    {
      private Dictionary<string, Action<T>> _dictionary = new Dictionary<string, Action<T>>();

      private event Action<T> _sender;

      public event Action<T> Sender
      {
        add => this._sender += value;
        remove => this._sender -= value;
      }

      public void AddCallbackMethod(Action<T> callback)
      {
        string callbackMethodId = this.GetCallbackMethodId(callback);
        if (this._dictionary.ContainsKey(callbackMethodId))
          return;
        this._dictionary.Add(callbackMethodId, callback);
        this.Sender += callback;
      }

      public void RemoveCallbackMethod(Action<T> callback)
      {
        if (!this._dictionary.Remove(this.GetCallbackMethodId(callback)))
          return;
        this.Sender -= callback;
      }

      public string DebugCheck(Action<T> callback)
      {
        StringBuilder stringBuilder = new StringBuilder("Check for ");
        stringBuilder.AppendLine("function: " + this.GetCallbackMethodId(callback));
        foreach (string key in this._dictionary.Keys)
          stringBuilder.AppendLine("- Found: " + key);
        return stringBuilder.ToString();
      }

      private string GetCallbackMethodId(Action<T> callback)
      {
        string callbackMethodId = string.Format("{0}{1}", (object) callback.Method.DeclaringType.FullName, (object) callback.Method.Name);
        if (callback.Target != null)
          callbackMethodId = string.Format("{0}{1}", (object) callbackMethodId, (object) callback.Target.GetHashCode().ToString());
        return callbackMethodId;
      }

      public void CastEvent(object m)
      {
        if (this._sender == null)
          return;
        this._sender((T) m);
      }
    }
  }
}
