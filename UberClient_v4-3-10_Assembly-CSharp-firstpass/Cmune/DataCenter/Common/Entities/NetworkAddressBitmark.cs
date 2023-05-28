// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.NetworkAddressBitmark
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Cmune.DataCenter.Common.Entities
{
  public enum NetworkAddressBitmark
  {
    _255_255_255_255 = 0,
    _255_255_255_0 = 255, // 0x000000FF
    _255_255_0_0 = 65535, // 0x0000FFFF
    _255_0_0_0 = 16777215, // 0x00FFFFFF
  }
}
