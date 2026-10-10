using Cmune.DataCenter.Common.Entities;
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
	public static class IpReputationProxy
	{
		public static void Serialize(Stream stream, IpReputationView instance)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BooleanProxy.Serialize(memoryStream, instance.IsMalicious);
				StringProxy.Serialize(memoryStream, instance.IpAddress);
				StringProxy.Serialize(memoryStream, instance.Reason);
				memoryStream.WriteTo(stream);
			}
		}

		public static IpReputationView Deserialize(Stream bytes)
		{
			IpReputationView ipReputationView = new IpReputationView();

			ipReputationView.IsMalicious = BooleanProxy.Deserialize(bytes);
			ipReputationView.IpAddress = StringProxy.Deserialize(bytes);
			ipReputationView.Reason = StringProxy.Deserialize(bytes);
			return ipReputationView;
		}
	}
}
