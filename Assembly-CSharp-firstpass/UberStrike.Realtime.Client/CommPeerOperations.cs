using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Serialization;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Realtime.Client
{
	public sealed class CommPeerOperations : IOperationSender
	{
		private byte __id;

		private RemoteProcedureCall sendOperation;

		public event RemoteProcedureCall SendOperation
		{
			add
			{
				sendOperation = (RemoteProcedureCall)Delegate.Combine(sendOperation, value);
			}
			remove
			{
				sendOperation = (RemoteProcedureCall)Delegate.Remove(sendOperation, value);
			}
		}

		public CommPeerOperations(byte id = 0)
		{
			__id = id;
		}

		public void SendAuthenticationRequest(string authToken, string magicHash, bool isMac)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);
				BooleanProxy.Serialize(memoryStream, isMac);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(1, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdateLoadout(string authToken,LoadoutView view)
		{
			using(MemoryStream stream = new MemoryStream())
			{
				StringProxy.Serialize(stream, authToken);
				LoadoutViewProxy.Serialize(stream, view);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, stream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(3, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendSendHeartbeatResponse(string authToken, string responseHash)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, responseHash);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(2, customOpParameters, sendReliable: true, 0);
				}
			}
		}
	}
}
