using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Serialization;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
	public static class ClanWebServiceClient
	{
		public static Coroutine GetOwnClan(string authToken, int groupId, Action<ClanView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, groupId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "GetOwnClan", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ClanViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine UpdateMemberPosition(MemberPositionUpdateView updateMemberPositionData, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				MemberPositionUpdateViewProxy.Serialize(memoryStream, updateMemberPositionData);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "UpdateMemberPosition", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine InviteMemberToJoinAGroup(int clanId, string authToken, int inviteeCmid, string message, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, clanId);
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, inviteeCmid);
				StringProxy.Serialize(memoryStream, message);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "InviteMemberToJoinAGroup", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine AcceptClanInvitation(int clanInvitationId, string authToken, Action<ClanRequestAcceptView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, clanInvitationId);
				StringProxy.Serialize(memoryStream, authToken);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "AcceptClanInvitation", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ClanRequestAcceptViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine DeclineClanInvitation(int clanInvitationId, string authToken, Action<ClanRequestDeclineView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, clanInvitationId);
				StringProxy.Serialize(memoryStream, authToken);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "DeclineClanInvitation", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ClanRequestDeclineViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine KickMemberFromClan(int groupId, string authToken, int cmidToKick, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, groupId);
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, cmidToKick);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "KickMemberFromClan", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine DisbandGroup(int groupId, string authToken, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, groupId);
				StringProxy.Serialize(memoryStream, authToken);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "DisbandGroup", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine LeaveAClan(int groupId, string authToken, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, groupId);
				StringProxy.Serialize(memoryStream, authToken);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "LeaveAClan", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine GetMyClanId(string authToken, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "GetMyClanId", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine CancelInvitation(int groupInvitationId, string authToken, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, groupInvitationId);
				StringProxy.Serialize(memoryStream, authToken);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "CancelInvitation", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine GetAllGroupInvitations(string authToken, Action<List<GroupInvitationView>> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "GetAllGroupInvitations", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ListProxy<GroupInvitationView>.Deserialize(new MemoryStream(data), GroupInvitationViewProxy.Deserialize));
					}
				}, handler));
			}
		}

		public static Coroutine GetPendingGroupInvitations(int groupId, string authToken, Action<List<GroupInvitationView>> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, groupId);
				StringProxy.Serialize(memoryStream, authToken);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "GetPendingGroupInvitations", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ListProxy<GroupInvitationView>.Deserialize(new MemoryStream(data), GroupInvitationViewProxy.Deserialize));
					}
				}, handler));
			}
		}

		public static Coroutine CreateClan(GroupCreationView createClanData, Action<ClanCreationReturnView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				GroupCreationViewProxy.Serialize(memoryStream, createClanData);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "CreateClan", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ClanCreationReturnViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine TransferOwnership(int groupId, string authToken, int newLeaderCmid, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, groupId);
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, newLeaderCmid);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "TransferOwnership", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine CanOwnAClan(string authToken, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "CanOwnAClan", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine test(Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", "test", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}
	}
}
