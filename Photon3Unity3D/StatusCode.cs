// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.StatusCode
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

namespace ExitGames.Client.Photon
{
  public enum StatusCode
  {
    SecurityExceptionOnConnect = 1022, // 0x000003FE
    ExceptionOnConnect = 1023, // 0x000003FF
    Connect = 1024, // 0x00000400
    Disconnect = 1025, // 0x00000401
    Exception = 1026, // 0x00000402
    QueueOutgoingReliableWarning = 1027, // 0x00000403
    QueueOutgoingUnreliableWarning = 1029, // 0x00000405
    SendError = 1030, // 0x00000406
    QueueOutgoingAcksWarning = 1031, // 0x00000407
    QueueIncomingReliableWarning = 1033, // 0x00000409
    QueueIncomingUnreliableWarning = 1035, // 0x0000040B
    QueueSentWarning = 1037, // 0x0000040D
    InternalReceiveException = 1039, // 0x0000040F
    TimeoutDisconnect = 1040, // 0x00000410
    DisconnectByServer = 1041, // 0x00000411
    DisconnectByServerUserLimit = 1042, // 0x00000412
    DisconnectByServerLogic = 1043, // 0x00000413
    TcpRouterResponseOk = 1044, // 0x00000414
    TcpRouterResponseNodeIdUnknown = 1045, // 0x00000415
    TcpRouterResponseEndpointUnknown = 1046, // 0x00000416
    TcpRouterResponseNodeNotReady = 1047, // 0x00000417
    EncryptionEstablished = 1048, // 0x00000418
    EncryptionFailedToEstablish = 1049, // 0x00000419
  }
}
