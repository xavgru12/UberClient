// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.Lite.LiteOpCode
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;

namespace ExitGames.Client.Photon.Lite
{
  public static class LiteOpCode
  {
    [Obsolete("Exchanging encrpytion keys is done internally in the lib now. Don't expect this operation-result.")]
    public const byte ExchangeKeysForEncryption = 250;
    public const byte Join = 255;
    public const byte Leave = 254;
    public const byte RaiseEvent = 253;
    public const byte SetProperties = 252;
    public const byte GetProperties = 251;
  }
}
