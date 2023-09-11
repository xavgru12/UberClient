
namespace UberStrike.UnitySdk
{
  public static class Api
  {
    private static readonly string _version = typeof (Api).Assembly.GetName().Version.ToString(3);

    public static string Version => Api._version;
  }
}
