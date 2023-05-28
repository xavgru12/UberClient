// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamControllerState_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct SteamControllerState_t
  {
    public uint unPacketNum;
    public ulong ulButtons;
    public short sLeftPadX;
    public short sLeftPadY;
    public short sRightPadX;
    public short sRightPadY;
  }
}
