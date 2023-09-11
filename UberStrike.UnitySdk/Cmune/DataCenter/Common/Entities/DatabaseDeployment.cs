
using System.Runtime.InteropServices;

namespace Cmune.DataCenter.Common.Entities
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct DatabaseDeployment
  {
    public const string Dev = "dev";
    public const string Staging = "staging";
    public const string Prod = "prod";
  }
}
