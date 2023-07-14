// Decompiled with JetBrains decompiler
// Type: Boo.Lang.AbstractGeneratorEnumerator
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System.Collections;

namespace Boo.Lang
{
  public abstract class AbstractGeneratorEnumerator : IEnumerator
  {
    protected object _current;
    protected int _state;

    public object Current => this._current;

    public void Reset() => this._state = 0;

    public abstract bool MoveNext();

    protected bool Yield(int state, object value)
    {
      this._state = state;
      this._current = value;
      return true;
    }
  }
}
