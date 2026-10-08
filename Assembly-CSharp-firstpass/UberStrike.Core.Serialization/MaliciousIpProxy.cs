using Cmune.DataCenter.Common.Entities;
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
	public static class MaliciousIpProxy
	{
		public static void Serialize(Stream stream, MaliciousIp instance)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, instance.IpAddress);
				StringProxy.Serialize(memoryStream, instance.Reason);
				memoryStream.WriteTo(stream);
			}
		}

		public static MaliciousIp Deserialize(Stream bytes)
		{
			MaliciousIp maliciousIp = new MaliciousIp();

			maliciousIp.IpAddress = StringProxy.Deserialize(bytes);
			maliciousIp.Reason = StringProxy.Deserialize(bytes);
			return maliciousIp;
		}
	}
}
