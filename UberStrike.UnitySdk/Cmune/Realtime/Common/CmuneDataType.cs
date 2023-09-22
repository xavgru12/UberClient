
using Cmune.Core.Types;

namespace Cmune.Realtime.Common
{
  public class CmuneDataType : ExtendableEnum<byte>
  {
    public const byte None = 0;
    public const byte Byte = 1;
    public const byte SByte = 2;
    public const byte Bool = 3;
    public const byte Int16 = 4;
    public const byte UInt16 = 5;
    public const byte Int32 = 6;
    public const byte Long = 8;
    public const byte Float = 10;
    public const byte String = 12;
    public const byte Array_Byte = 15;
    public const byte Array_Short = 18;
    public const byte Array_UShort = 19;
    public const byte Array_Int = 20;
    public const byte Array_Long = 22;
    public const byte Array_Float = 24;
    public const byte Array_String = 26;
    public const byte Vector3 = 30;
    public const byte Quaternion = 31;
    public const byte Color = 32;
    public const byte Array_Vector3 = 35;
    public const byte Array_Quaternion = 36;
    public const byte RoomData = 40;
    public const byte Transform = 42;
    public const byte AssetType = 43;
    public const byte PhysicsPack = 44;
    public const byte RoomId = 45;
    public const byte CommActorInfo = 46;
    public const byte PerformanceData = 47;
    public const byte SyncObject = 48;
    public const byte Array_RoomId = 51;
    public const byte Array_SyncObject = 53;
  }
}
