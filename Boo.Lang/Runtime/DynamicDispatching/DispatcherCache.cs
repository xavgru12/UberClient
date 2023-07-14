// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.DispatcherCache
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System.Collections.Generic;

namespace Boo.Lang.Runtime.DynamicDispatching
{
  public class DispatcherCache
  {
    private static Dictionary<DispatcherKey, Dispatcher> _cache = new Dictionary<DispatcherKey, Dispatcher>(DispatcherKey.EqualityComparer);

    public Dispatcher Get(DispatcherKey key, DispatcherCache.DispatcherFactory factory)
    {
      Dispatcher dispatcher;
      if (!DispatcherCache._cache.TryGetValue(key, out dispatcher))
      {
        lock (DispatcherCache._cache)
        {
          if (!DispatcherCache._cache.TryGetValue(key, out dispatcher))
          {
            dispatcher = factory();
            DispatcherCache._cache.Add(key, dispatcher);
          }
        }
      }
      return dispatcher;
    }

    public void Clear()
    {
      lock (DispatcherCache._cache)
        DispatcherCache._cache.Clear();
    }

    public delegate Dispatcher DispatcherFactory();
  }
}
