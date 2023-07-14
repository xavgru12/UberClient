// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.DynamicDispatching.DispatcherKey
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Collections.Generic;

namespace Boo.Lang.Runtime.DynamicDispatching
{
  public class DispatcherKey
  {
    public static readonly IEqualityComparer<DispatcherKey> EqualityComparer = (IEqualityComparer<DispatcherKey>) new DispatcherKey._EqualityComparer();
    private readonly Type _type;
    private readonly string _name;
    private readonly Type[] _arguments;

    public DispatcherKey(Type type, string name)
      : this(type, name, Type.EmptyTypes)
    {
    }

    public DispatcherKey(Type type, string name, Type[] arguments)
    {
      this._type = type;
      this._name = name;
      this._arguments = arguments;
    }

    public Type[] Arguments => this._arguments;

    private sealed class _EqualityComparer : IEqualityComparer<DispatcherKey>
    {
      public int GetHashCode(DispatcherKey key) => key._type.GetHashCode() ^ key._name.GetHashCode() ^ key._arguments.Length;

      public bool Equals(DispatcherKey x, DispatcherKey y)
      {
        if (x._type != y._type || x._arguments.Length != y._arguments.Length || x._name != y._name)
          return false;
        for (int index = 0; index < x._arguments.Length; ++index)
        {
          if (x._arguments[index] != y._arguments[index])
            return false;
        }
        return true;
      }
    }
  }
}
