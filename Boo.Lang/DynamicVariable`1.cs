// Decompiled with JetBrains decompiler
// Type: Boo.Lang.DynamicVariable`1
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;

namespace Boo.Lang
{
  public class DynamicVariable<T>
  {
    private T _current;

    public DynamicVariable() => this._current = default (T);

    public DynamicVariable(T initialValue) => this._current = initialValue;

    public T Value => this._current;

    [Obsolete("Use With(T, System.Action) and access the variable value directly from the closure")]
    public void With(T value, Action<T> code) => this.With(value, (Procedure) (() => code(value)));

    public void With(T value, Procedure code)
    {
      T current = this._current;
      this._current = value;
      try
      {
        code();
      }
      finally
      {
        this._current = current;
      }
    }
  }
}
