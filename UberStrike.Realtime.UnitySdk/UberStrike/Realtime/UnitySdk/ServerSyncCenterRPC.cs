// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.ServerSyncCenterRPC
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

namespace UberStrike.Realtime.UnitySdk
{
  [ExtendableEnumBounds(1, 20)]
  public class ServerSyncCenterRPC : ExtendableEnum<byte>
  {
    public const byte UnregisterNetworkClass = 1;
    public const byte RegisterMonoNetworkGameObject = 2;
    public const byte RegisterStaticNetworkClass = 3;
    public const byte InitializeRoomOnClient = 4;
    public const byte SynchronizeProperties = 5;
    public const byte NetworkPhysicsUpdate = 6;
    public const byte MailNotification = 7;
  }
}
