using Cmune.DataCenter.Common.Entities;
using System;
using System.IO;
using UberStrike.Core.Serialization;
using UberStrike.Core.ViewModel;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
	public static class AuthenticationWebServiceClient
	{
		public static Coroutine CreateUser(string emailAddress, string password, ChannelType channel, string locale, string machineId, Action<MemberRegistrationResult> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, emailAddress);
				StringProxy.Serialize(memoryStream, password);
				EnumProxy<ChannelType>.Serialize(memoryStream, channel);
				StringProxy.Serialize(memoryStream, locale);
				StringProxy.Serialize(memoryStream, machineId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", "CreateUser", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(EnumProxy<MemberRegistrationResult>.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine CompleteAccount(int cmid, string name, ChannelType channel, string locale, string machineId, Action<AccountCompletionResultView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				StringProxy.Serialize(memoryStream, name);
				EnumProxy<ChannelType>.Serialize(memoryStream, channel);
				StringProxy.Serialize(memoryStream, locale);
				StringProxy.Serialize(memoryStream, machineId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", "CompleteAccount", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(AccountCompletionResultViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine LoginMemberEmail(string email, string password, ChannelType channelType, string machineId, Action<MemberAuthenticationResultView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, email);
				StringProxy.Serialize(memoryStream, password);
				EnumProxy<ChannelType>.Serialize(memoryStream, channelType);
				StringProxy.Serialize(memoryStream, machineId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", "LoginMemberEmail", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(MemberAuthenticationResultViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine LoginMemberFacebookUnitySdk(string facebookPlayerAccessToken, ChannelType channelType, string machineId, Action<MemberAuthenticationResultView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, facebookPlayerAccessToken);
				EnumProxy<ChannelType>.Serialize(memoryStream, channelType);
				StringProxy.Serialize(memoryStream, machineId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", "LoginMemberFacebookUnitySdk", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(MemberAuthenticationResultViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine LoginSteam(string clientVersion, string steamId, string authToken, string machineId, string hwid, bool isMac, Action<MemberAuthenticationResultView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, clientVersion);
				StringProxy.Serialize(memoryStream, steamId);
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, machineId);
				StringProxy.Serialize(memoryStream, hwid);
				BooleanProxy.Serialize(memoryStream, isMac);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", "LoginSteam", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(MemberAuthenticationResultViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine LoginMemberPortal(int cmid, string hash, string machineId, Action<MemberAuthenticationResultView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				StringProxy.Serialize(memoryStream, hash);
				StringProxy.Serialize(memoryStream, machineId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", "LoginMemberPortal", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(MemberAuthenticationResultViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine LinkSteamMember(string email, string password, string steamId, string machineId, Action<MemberAuthenticationResultView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, email);
				StringProxy.Serialize(memoryStream, password);
				StringProxy.Serialize(memoryStream, steamId);
				StringProxy.Serialize(memoryStream, machineId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", "LinkSteamMember", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(MemberAuthenticationResultViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}
	}
}
