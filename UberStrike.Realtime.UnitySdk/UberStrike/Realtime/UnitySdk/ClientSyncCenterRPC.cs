// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ClientSyncCenterRPC
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  [ExtendableEnumBounds(1, 20)]
  public class ClientSyncCenterRPC : ExtendableEnum<byte>
  {
    public const byte RecieveNetworkID = 1;
    public const byte LoadAssetWithID = 2;
    public const byte RemoveInstanceWithID = 3;
    public const byte RecieveStaticClassRegistration = 4;
    public const byte SynchronizeServerTime = 5;
    public const byte ClearRoom = 6;
    public const byte RoomInitialized = 7;
    public const byte RefreshMemberInfo = 8;
    public const byte KickPlayer = 9;
  }
}
