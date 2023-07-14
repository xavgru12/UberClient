// Decompiled with JetBrains decompiler
// Type: Boo.Lang.ResourceManager
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using Boo.Lang.Resources;

namespace Boo.Lang
{
  public static class ResourceManager
  {
    public static string Format(string name, params object[] args) => string.Format(ResourceManager.GetString(name), args);

    private static string GetString(string name) => (string) typeof (StringResources).GetField(name).GetValue((object) null);
  }
}
