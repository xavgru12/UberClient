// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Environments.DeferredEnvironment
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Boo.Lang.Environments
{
  public class DeferredEnvironment : 
    IEnumerable,
    IEnvironment,
    IEnumerable<KeyValuePair<Type, ObjectFactory>>
  {
    private readonly Boo.Lang.List<KeyValuePair<Type, ObjectFactory>> _bindings = new Boo.Lang.List<KeyValuePair<Type, ObjectFactory>>();

    IEnumerator<KeyValuePair<Type, ObjectFactory>> IEnumerable<KeyValuePair<Type, ObjectFactory>>.GetEnumerator() => this._bindings.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._bindings.GetEnumerator();

    TNeed IEnvironment.Provide<TNeed>()
    {
      foreach (KeyValuePair<Type, ObjectFactory> binding in this._bindings)
      {
        if (typeof (TNeed).IsAssignableFrom(binding.Key))
          return (TNeed) binding.Value();
      }
      return (TNeed) null;
    }

    public void Add(Type need, ObjectFactory binder) => this._bindings.Add(new KeyValuePair<Type, ObjectFactory>(need, binder));
  }
}
