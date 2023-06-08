using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Serialization;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
	public static class PrivateMessageWebServiceClient
	{
		public static Coroutine GetAllMessageThreadsForUser(string authToken, int pageNumber, Action<List<MessageThreadView>> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, pageNumber);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", "GetAllMessageThreadsForUser", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ListProxy<MessageThreadView>.Deserialize(new MemoryStream(data), MessageThreadViewProxy.Deserialize));
					}
				}, handler));
			}
		}

		public static Coroutine GetThreadMessages(string authToken, int otherCmid, int pageNumber, Action<List<PrivateMessageView>> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, otherCmid);
				Int32Proxy.Serialize(memoryStream, pageNumber);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", "GetThreadMessages", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ListProxy<PrivateMessageView>.Deserialize(new MemoryStream(data), PrivateMessageViewProxy.Deserialize));
					}
				}, handler));
			}
		}

		public static Coroutine SendMessage(string authToken, int receiverCmid, string content, Action<PrivateMessageView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, receiverCmid);
				StringProxy.Serialize(memoryStream, content);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", "SendMessage", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(PrivateMessageViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine GetMessageWithIdForCmid(string authToken, int messageId, Action<PrivateMessageView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, messageId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", "GetMessageWithIdForCmid", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(PrivateMessageViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine MarkThreadAsRead(string authToken, int otherCmid, Action callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, otherCmid);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", "MarkThreadAsRead", memoryStream.ToArray(), delegate
				{
					if (callback != null)
					{
						callback();
					}
				}, handler));
			}
		}

		public static Coroutine DeleteThread(string authToken, int otherCmid, Action callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, otherCmid);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", "DeleteThread", memoryStream.ToArray(), delegate
				{
					if (callback != null)
					{
						callback();
					}
				}, handler));
			}
		}
	}
}
