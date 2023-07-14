// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Environments.ActiveEnvironment
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;

namespace Boo.Lang.Environments
{
  public static class ActiveEnvironment
  {
    [ThreadStatic]
    private static IEnvironment _instance;

    public static IEnvironment Instance => ActiveEnvironment._instance;

    public static void With(IEnvironment environment, Procedure code)
    {
      IEnvironment instance = ActiveEnvironment._instance;
      try
      {
        ActiveEnvironment._instance = environment;
        code();
      }
      finally
      {
        ActiveEnvironment._instance = instance;
      }
    }
  }
}
