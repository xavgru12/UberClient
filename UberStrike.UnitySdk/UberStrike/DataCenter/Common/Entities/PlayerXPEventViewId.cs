
using System.Runtime.InteropServices;

namespace UberStrike.DataCenter.Common.Entities
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct PlayerXPEventViewId
  {
    public const int Splat = 1;
    public const int HeadShot = 2;
    public const int Nutshot = 3;
    public const int Humiliation = 4;
    public const int Damage = 5;
  }
}
