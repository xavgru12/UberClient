﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.ISteamMatchmakingServerListResponse
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public class ISteamMatchmakingServerListResponse
  {
    private ISteamMatchmakingServerListResponse.VTable m_VTable;
    private IntPtr m_pVTable;
    private GCHandle m_pGCHandle;
    private ISteamMatchmakingServerListResponse.ServerResponded m_ServerResponded;
    private ISteamMatchmakingServerListResponse.ServerFailedToRespond m_ServerFailedToRespond;
    private ISteamMatchmakingServerListResponse.RefreshComplete m_RefreshComplete;

    public ISteamMatchmakingServerListResponse(
      ISteamMatchmakingServerListResponse.ServerResponded onServerResponded,
      ISteamMatchmakingServerListResponse.ServerFailedToRespond onServerFailedToRespond,
      ISteamMatchmakingServerListResponse.RefreshComplete onRefreshComplete)
    {
      if (onServerResponded == null || onServerFailedToRespond == null || onRefreshComplete == null)
        throw new ArgumentNullException();
      this.m_ServerResponded = onServerResponded;
      this.m_ServerFailedToRespond = onServerFailedToRespond;
      this.m_RefreshComplete = onRefreshComplete;
      this.m_VTable = new ISteamMatchmakingServerListResponse.VTable()
      {
        m_VTServerResponded = new ISteamMatchmakingServerListResponse.InternalServerResponded(this.InternalOnServerResponded),
        m_VTServerFailedToRespond = new ISteamMatchmakingServerListResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond),
        m_VTRefreshComplete = new ISteamMatchmakingServerListResponse.InternalRefreshComplete(this.InternalOnRefreshComplete)
      };
      this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (ISteamMatchmakingServerListResponse.VTable)));
      Marshal.StructureToPtr((object) this.m_VTable, this.m_pVTable, false);
      this.m_pGCHandle = GCHandle.Alloc((object) this.m_pVTable, GCHandleType.Pinned);
    }

    ~ISteamMatchmakingServerListResponse()
    {
      if (this.m_pVTable != IntPtr.Zero)
        Marshal.FreeHGlobal(this.m_pVTable);
      if (!this.m_pGCHandle.IsAllocated)
        return;
      this.m_pGCHandle.Free();
    }

    private void InternalOnServerResponded(HServerListRequest hRequest, int iServer) => this.m_ServerResponded(hRequest, iServer);

    private void InternalOnServerFailedToRespond(HServerListRequest hRequest, int iServer) => this.m_ServerFailedToRespond(hRequest, iServer);

    private void InternalOnRefreshComplete(
      HServerListRequest hRequest,
      EMatchMakingServerResponse response)
    {
      this.m_RefreshComplete(hRequest, response);
    }

    public static explicit operator IntPtr(ISteamMatchmakingServerListResponse that) => that.m_pGCHandle.AddrOfPinnedObject();

    [StructLayout(LayoutKind.Sequential)]
    private class VTable
    {
      [MarshalAs(UnmanagedType.FunctionPtr)]
      [NonSerialized]
      public ISteamMatchmakingServerListResponse.InternalServerResponded m_VTServerResponded;
      [MarshalAs(UnmanagedType.FunctionPtr)]
      [NonSerialized]
      public ISteamMatchmakingServerListResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
      [MarshalAs(UnmanagedType.FunctionPtr)]
      [NonSerialized]
      public ISteamMatchmakingServerListResponse.InternalRefreshComplete m_VTRefreshComplete;
    }

    public delegate void ServerResponded(HServerListRequest hRequest, int iServer);

    public delegate void ServerFailedToRespond(HServerListRequest hRequest, int iServer);

    public delegate void RefreshComplete(
      HServerListRequest hRequest,
      EMatchMakingServerResponse response);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void InternalServerResponded(HServerListRequest hRequest, int iServer);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void InternalServerFailedToRespond(HServerListRequest hRequest, int iServer);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void InternalRefreshComplete(
      HServerListRequest hRequest,
      EMatchMakingServerResponse response);
  }
}
