
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
