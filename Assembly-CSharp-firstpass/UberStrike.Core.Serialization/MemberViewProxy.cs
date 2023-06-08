using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
	public static class MemberViewProxy
	{
		public static void Serialize(Stream stream, MemberView instance)
		{
			int num = 0;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				if (instance.MemberItems != null)
				{
					ListProxy<int>.Serialize(memoryStream, instance.MemberItems, Int32Proxy.Serialize);
				}
				else
				{
					num |= 1;
				}
				if (instance.MemberWallet != null)
				{
					MemberWalletViewProxy.Serialize(memoryStream, instance.MemberWallet);
				}
				else
				{
					num |= 2;
				}
				if (instance.PublicProfile != null)
				{
					PublicProfileViewProxy.Serialize(memoryStream, instance.PublicProfile);
				}
				else
				{
					num |= 4;
				}
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static MemberView Deserialize(Stream bytes)
		{
			int num = Int32Proxy.Deserialize(bytes);
			MemberView memberView = new MemberView();
			if ((num & 1) != 0)
			{
				memberView.MemberItems = ListProxy<int>.Deserialize(bytes, Int32Proxy.Deserialize);
			}
			if ((num & 2) != 0)
			{
				memberView.MemberWallet = MemberWalletViewProxy.Deserialize(bytes);
			}
			if ((num & 4) != 0)
			{
				memberView.PublicProfile = PublicProfileViewProxy.Deserialize(bytes);
			}
			return memberView;
		}
	}
}
