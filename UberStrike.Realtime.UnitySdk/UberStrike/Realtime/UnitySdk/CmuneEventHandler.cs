// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CmuneEventHandler
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberStrike.Realtime.UnitySdk
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

      public void AddCallbackMethod(Action<T> callback) => this._dictionary[this.GetCallbackMethodId(callback)] = callback;

      public void RemoveCallbackMethod(Action<T> callback) => this._dictionary.Remove(this.GetCallbackMethodId(callback));

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

      public void CastEvent(object message)
      {
        foreach (Action<T> action in this._dictionary.Values.ToArray<Action<T>>())
          action((T) message);
      }
    }
  }
}
