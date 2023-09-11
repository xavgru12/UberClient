
using System.Runtime.InteropServices;

namespace Cmune.DataCenter.Common.Entities
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct CommonAppSettings
  {
    public const string PassPhrase = "CmunePassPhrase";
    public const string InitVector = "CmuneInitVector";
    public const string DatabaseDataSourceOverride = "DbAddressCore";
    public const string DatabaseDataSource = "CmuneAPIKey";
    public const string DatabaseForumDataSourceOverride = "DbAddressMvForum";
    public const string DatabaseForumDataSource = "appsAPIKey";
  }
}
