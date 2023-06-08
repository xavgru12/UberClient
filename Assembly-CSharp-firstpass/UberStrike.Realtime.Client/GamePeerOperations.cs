using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client
{
	public sealed class GamePeerOperations : IOperationSender
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

		public GamePeerOperations(byte id = 0)
		{
			__id = id;
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
					sendOperation(1, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendGetServerLoad()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(2, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendGetGameInformation(int number)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, number);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(3, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendGetGameListUpdates()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(4, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendEnterRoom(int roomId, string password, string clientVersion, string authToken, string magicHash, bool isMac)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, roomId);
				StringProxy.Serialize(memoryStream, password);
				StringProxy.Serialize(memoryStream, clientVersion);
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);
				BooleanProxy.Serialize(memoryStream, isMac);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(5, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendCreateRoom(GameRoomData metaData, string password, string clientVersion, string authToken, string magicHash, bool isMac)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				GameRoomDataProxy.Serialize(memoryStream, metaData);
				StringProxy.Serialize(memoryStream, password);
				StringProxy.Serialize(memoryStream, clientVersion);
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);
				BooleanProxy.Serialize(memoryStream, isMac);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(6, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendLeaveRoom()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(7, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendCloseRoom(int roomId, string authToken, string magicHash)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, roomId);
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(8, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendInspectRoom(int roomId, string authToken)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, roomId);
				StringProxy.Serialize(memoryStream, authToken);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(9, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendReportPlayer(int cmid, string authToken)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				StringProxy.Serialize(memoryStream, authToken);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(10, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendKickPlayer(int cmid, string authToken, string magicHash)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(11, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdateLoadout()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(12, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdatePing(ushort ping)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				UInt16Proxy.Serialize(memoryStream, ping);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(13, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdateKeyState(byte state)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				ByteProxy.Serialize(memoryStream, state);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(14, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendRefreshBackendData(string authToken, string magicHash)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				StringProxy.Serialize(memoryStream, magicHash);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(15, customOpParameters, sendReliable: true, 0);
				}
			}
		}
	}
}
