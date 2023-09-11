// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeAppSettings
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Runtime.InteropServices;

namespace UberStrike.DataCenter.Common.Entities
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct UberstrikeAppSettings
  {
    public const string PortalCookieName = "PortalCookieName";
    public const string FacebookCookieName = "FacebookCookieName";
    public const string MySpaceCookieName = "MySpaceCookieName";
    public const string KongregateCookieName = "KongregateCookieName";
    public const string LocalhostIP = "LocalhostIP";
    public const string DatabaseDataSourceOverride = "DbAddressMvPP";
    public const string DatabaseDataSource = "appsAPIKey";
  }
}
