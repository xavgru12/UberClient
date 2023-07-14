// Decompiled with JetBrains decompiler
// Type: Boo.Lang.GenericGeneratorEnumerator`1
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Boo.Lang
{
  public abstract class GenericGeneratorEnumerator<T> : IEnumerator, IDisposable, IEnumerator<T>
  {
    protected T _current;
    public int _state;

    public GenericGeneratorEnumerator() => this._state = 0;

    object IEnumerator.Current => (object) this._current;

    public T Current => this._current;

    public virtual void Dispose()
    {
    }

    public void Reset() => throw new NotSupportedException();

    public abstract bool MoveNext();

    protected bool Yield(int state, T value)
    {
      this._state = state;
      this._current = value;
      return true;
    }

    protected bool YieldDefault(int state)
    {
      this._state = state;
      this._current = default (T);
      return true;
    }
  }
}
