// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.Lite.LiteOpKey
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;

namespace ExitGames.Client.Photon.Lite
{
  public static class LiteOpKey
  {
    [Obsolete("Use GameId")]
    public const byte Asid = 255;
    [Obsolete("Use GameId")]
    public const byte RoomName = 255;
    public const byte GameId = 255;
    public const byte ActorNr = 254;
    public const byte TargetActorNr = 253;
    public const byte ActorList = 252;
    public const byte Properties = 251;
    public const byte Broadcast = 250;
    public const byte ActorProperties = 249;
    public const byte GameProperties = 248;
    public const byte Cache = 247;
    public const byte ReceiverGroup = 246;
    public const byte Data = 245;
    public const byte Code = 244;
  }
}
