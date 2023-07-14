// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ParameterKeys
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  [ExtendableEnumBounds(0, 255)]
  public class ParameterKeys : ExtendableEnum<byte>
  {
    public const byte GameId = 4;
    public const byte ActorNr = 9;
    public const byte Actors = 11;
    public const byte Data = 42;
    public const byte InvocationId = 61;
    public const byte MethodId = 100;
    public const byte InstanceId = 101;
    public const byte ActorId = 102;
    public const byte Bytes = 103;
    public const byte RoomId = 120;
    public const byte LobbyRoomUpdate = 122;
    public const byte LobbyRoomDelete = 123;
    public const byte InitRoom = 200;
    public const byte ServerTicks = 201;
    public const byte AccessLevel = 205;
    public const byte Cmid = 206;
  }
}
