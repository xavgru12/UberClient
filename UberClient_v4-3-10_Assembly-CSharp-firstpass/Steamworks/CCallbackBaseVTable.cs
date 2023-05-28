// Decompiled with JetBrains decompiler
// Type: Steamworks.CCallbackBaseVTable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential)]
  internal class CCallbackBaseVTable
  {
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.RunCRDel m_RunCallResult;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.RunCBDel m_RunCallback;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.GetCallbackSizeBytesDel m_GetCallbackSizeBytes;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void RunCBDel(IntPtr pvParam);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void RunCRDel(IntPtr pvParam, [MarshalAs(UnmanagedType.I1)] bool bIOFailure, ulong hSteamAPICall);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int GetCallbackSizeBytesDel();
  }
}
