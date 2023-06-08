using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;

namespace UberStrike.Realtime.Client
{
	public sealed class LobbyRoomOperations : IOperationSender
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

		public LobbyRoomOperations(byte id = 0)
		{
			__id = id;
		}

		public void SendFullPlayerListUpdate()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(1, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdatePlayerRoom(GameRoom room)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				GameRoomProxy.Serialize(memoryStream, room);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(2, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendResetPlayerRoom()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(3, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdateFriendsList(int cmid)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(4, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdateClanData(int cmid)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(5, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdateInboxMessages(int cmid, int messageId)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				Int32Proxy.Serialize(memoryStream, messageId);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(6, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdateInboxRequests(int cmid)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(7, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdateClanMembers(List<int> clanMembers)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				ListProxy<int>.Serialize(memoryStream, clanMembers, Int32Proxy.Serialize);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(8, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendGetPlayersWithMatchingName(string search)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, search);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(9, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendChatMessageToAll(string message)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, message);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(10, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendChatMessageToPlayer(int cmid, string message)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				StringProxy.Serialize(memoryStream, message);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(11, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendChatMessageToClan(List<int> clanMembers, string message)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				ListProxy<int>.Serialize(memoryStream, clanMembers, Int32Proxy.Serialize);
				StringProxy.Serialize(memoryStream, message);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(12, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendModerationMutePlayer(int durationInMinutes, int mutedCmid, bool disableChat)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, durationInMinutes);
				Int32Proxy.Serialize(memoryStream, mutedCmid);
				BooleanProxy.Serialize(memoryStream, disableChat);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(13, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendModerationPermanentBan(int cmid)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(14, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendModerationBanPlayer(int cmid)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(15, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendModerationKickGame(int cmid)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(16, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendModerationUnbanPlayer(int cmid)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(17, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendModerationCustomMessage(int cmid, string message)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				StringProxy.Serialize(memoryStream, message);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(18, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendSpeedhackDetection()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(19, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendSpeedhackDetectionNew(List<float> timeDifferences)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				ListProxy<float>.Serialize(memoryStream, timeDifferences, SingleProxy.Serialize);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(20, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendPlayersReported(List<int> cmids, int type, string details, string logs)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				ListProxy<int>.Serialize(memoryStream, cmids, Int32Proxy.Serialize);
				Int32Proxy.Serialize(memoryStream, type);
				StringProxy.Serialize(memoryStream, details);
				StringProxy.Serialize(memoryStream, logs);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(21, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdateNaughtyList()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(22, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendClearModeratorFlags(int cmid)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, cmid);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(23, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendSetContactList(List<int> cmids)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				ListProxy<int>.Serialize(memoryStream, cmids, Int32Proxy.Serialize);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(24, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdateAllActors()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(25, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUpdateContacts()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(26, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendTrustedModules(string modules)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, modules);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(27, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendAuthentication(string hwid)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, hwid);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(28, customOpParameters, sendReliable: true, 0);
				}
			}
		}

		public void SendUberBeatReport(string report)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, report);
				Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
				dictionary.Add(__id, memoryStream.ToArray());
				Dictionary<byte, object> customOpParameters = dictionary;
				if (sendOperation != null)
				{
					sendOperation(29, customOpParameters, sendReliable: true, 0);
				}
			}
		}
	}
}
