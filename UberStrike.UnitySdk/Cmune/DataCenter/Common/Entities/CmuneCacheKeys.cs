
using System.Runtime.InteropServices;

namespace Cmune.DataCenter.Common.Entities
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct CmuneCacheKeys
  {
    public const string MemberAccessParameters = "CmuneMemberAccess";
    public const string Separator = "_";
  }
}
