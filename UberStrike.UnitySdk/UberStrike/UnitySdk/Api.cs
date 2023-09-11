// Decompiled with JetBrains decompiler
// Type: UberStrike.UnitySdk.Api
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

namespace UberStrike.UnitySdk
{
  public static class Api
  {
    private static readonly string _version = typeof (Api).Assembly.GetName().Version.ToString(3);

    public static string Version => Api._version;
  }
}
