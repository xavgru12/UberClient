using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public class ISteamMatchmakingPlayersResponse
	{
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public InternalAddPlayerToList m_VTAddPlayerToList;

			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public InternalPlayersFailedToRespond m_VTPlayersFailedToRespond;

			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public InternalPlayersRefreshComplete m_VTPlayersRefreshComplete;
		}

		public delegate void AddPlayerToList([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Steamworks.UTF8Marshaler")] string pchName, int nScore, float flTimePlayed);

		public delegate void PlayersFailedToRespond();

		public delegate void PlayersRefreshComplete();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void InternalAddPlayerToList([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Steamworks.UTF8Marshaler")] string pchName, int nScore, float flTimePlayed);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void InternalPlayersFailedToRespond();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void InternalPlayersRefreshComplete();

		private VTable m_VTable;

		private IntPtr m_pVTable;

		private GCHandle m_pGCHandle;

		private AddPlayerToList m_AddPlayerToList;

		private PlayersFailedToRespond m_PlayersFailedToRespond;

		private PlayersRefreshComplete m_PlayersRefreshComplete;

		public ISteamMatchmakingPlayersResponse(AddPlayerToList onAddPlayerToList, PlayersFailedToRespond onPlayersFailedToRespond, PlayersRefreshComplete onPlayersRefreshComplete)
		{
			if (onAddPlayerToList == null || onPlayersFailedToRespond == null || onPlayersRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			m_AddPlayerToList = onAddPlayerToList;
			m_PlayersFailedToRespond = onPlayersFailedToRespond;
			m_PlayersRefreshComplete = onPlayersRefreshComplete;
			m_VTable = new VTable
			{
				m_VTAddPlayerToList = InternalOnAddPlayerToList,
				m_VTPlayersFailedToRespond = InternalOnPlayersFailedToRespond,
				m_VTPlayersRefreshComplete = InternalOnPlayersRefreshComplete
			};
			m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VTable)));
			Marshal.StructureToPtr(m_VTable, m_pVTable, fDeleteOld: false);
			m_pGCHandle = GCHandle.Alloc(m_pVTable, GCHandleType.Pinned);
		}

		~ISteamMatchmakingPlayersResponse()
		{
			if (m_pVTable != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(m_pVTable);
			}
			if (m_pGCHandle.IsAllocated)
			{
				m_pGCHandle.Free();
			}
		}

		private void InternalOnAddPlayerToList(string pchName, int nScore, float flTimePlayed)
		{
			m_AddPlayerToList(pchName, nScore, flTimePlayed);
		}

		private void InternalOnPlayersFailedToRespond()
		{
			m_PlayersFailedToRespond();
		}

		private void InternalOnPlayersRefreshComplete()
		{
			m_PlayersRefreshComplete();
		}

		public static explicit operator IntPtr(ISteamMatchmakingPlayersResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}
	}
}
