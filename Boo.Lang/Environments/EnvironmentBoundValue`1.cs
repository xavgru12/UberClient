// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Environments.EnvironmentBoundValue`1
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

namespace Boo.Lang.Environments
{
  public struct EnvironmentBoundValue<T>
  {
    public readonly T Value;
    public readonly IEnvironment Environment;

    public EnvironmentBoundValue(IEnvironment environment, T value)
    {
      this.Environment = environment;
      this.Value = value;
    }

    public EnvironmentBoundValue<TResult> Select<TResult>(Function<T, TResult> selector)
    {
      T v = this.Value;
      EnvironmentBoundValue<TResult> r = new EnvironmentBoundValue<TResult>();
      ActiveEnvironment.With(this.Environment, (Procedure) (() => r = EnvironmentBoundValue.Return<TResult>(selector(v))));
      return r;
    }
  }
}
