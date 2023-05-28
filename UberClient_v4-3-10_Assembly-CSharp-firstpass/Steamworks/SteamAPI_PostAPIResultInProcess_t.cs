// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPI_PostAPIResultInProcess_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void SteamAPI_PostAPIResultInProcess_t(
    SteamAPICall_t callHandle,
    IntPtr pUnknown,
    uint unCallbackSize,
    int iCallbackNum);
}
