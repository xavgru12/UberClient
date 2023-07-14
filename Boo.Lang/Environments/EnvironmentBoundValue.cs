// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Environments.EnvironmentBoundValue
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

namespace Boo.Lang.Environments
{
  public static class EnvironmentBoundValue
  {
    public static EnvironmentBoundValue<T> Capture<T>() where T : class => EnvironmentBoundValue.Return<T>(My<T>.Instance);

    public static EnvironmentBoundValue<T> Return<T>(T value) => EnvironmentBoundValue.Create<T>(ActiveEnvironment.Instance, value);

    public static EnvironmentBoundValue<T> Create<T>(IEnvironment environment, T value) => new EnvironmentBoundValue<T>(environment, value);
  }
}
