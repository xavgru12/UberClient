using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
	public static class PackageViewProxy
	{
		public static void Serialize(Stream stream, PackageView instance)
		{
			int num = 0;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, instance.Bonus);
				if (instance.Items != null)
				{
					ListProxy<int>.Serialize(memoryStream, instance.Items, Int32Proxy.Serialize);
				}
				else
				{
					num |= 1;
				}
				if (instance.Name != null)
				{
					StringProxy.Serialize(memoryStream, instance.Name);
				}
				else
				{
					num |= 2;
				}
				DecimalProxy.Serialize(memoryStream, instance.Price);
				Int32Proxy.Serialize(stream, ~num);
				memoryStream.WriteTo(stream);
			}
		}

		public static PackageView Deserialize(Stream bytes)
		{
			int num = Int32Proxy.Deserialize(bytes);
			PackageView packageView = new PackageView();
			packageView.Bonus = Int32Proxy.Deserialize(bytes);
			if ((num & 1) != 0)
			{
				packageView.Items = ListProxy<int>.Deserialize(bytes, Int32Proxy.Deserialize);
			}
			if ((num & 2) != 0)
			{
				packageView.Name = StringProxy.Deserialize(bytes);
			}
			packageView.Price = DecimalProxy.Deserialize(bytes);
			return packageView;
		}
	}
}
