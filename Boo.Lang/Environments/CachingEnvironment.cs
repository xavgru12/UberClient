// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Environments.CachingEnvironment
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Collections.Generic;

namespace Boo.Lang.Environments
{
  public class CachingEnvironment : IEnvironment
  {
    private readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();
    private readonly IEnvironment _source;

    public CachingEnvironment(IEnvironment source) => this._source = source;

    public event Action<object> InstanceCached;

    public TNeed Provide<TNeed>() where TNeed : class
    {
      object obj1;
      if (this._cache.TryGetValue(typeof (TNeed), out obj1))
        return (TNeed) obj1;
      foreach (object obj2 in this._cache.Values)
      {
        if (obj2 is TNeed need)
        {
          this._cache.Add(typeof (TNeed), obj2);
          return need;
        }
      }
      TNeed instance = this._source.Provide<TNeed>();
      if ((object) instance != null)
        this.Add(typeof (TNeed), (object) instance);
      return instance;
    }

    public void Add(Type type, object instance)
    {
      if (!type.IsInstanceOfType(instance))
        throw new ArgumentException(string.Format("{0} is not an instance of {1}", instance, (object) type));
      this._cache.Add(type, instance);
      if (this.InstanceCached == null)
        return;
      this.InstanceCached(instance);
    }
  }
}
