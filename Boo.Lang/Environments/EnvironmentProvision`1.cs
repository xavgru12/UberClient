// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Environments.EnvironmentProvision`1
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

namespace Boo.Lang.Environments
{
  public struct EnvironmentProvision<T> where T : class
  {
    private T _instance;

    public T Instance => (object) this._instance != null ? this._instance : (this._instance = My<T>.Instance);

    public static implicit operator T(EnvironmentProvision<T> provision) => provision.Instance;
  }
}
