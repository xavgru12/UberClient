using Cmune.DataCenter.Common.Entities;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
	public class UberstrikeItemSpecialView : UberstrikeItemView
	{
		public UberstrikeSpecialConfigView Config
		{
			get;
			set;
		}

		public UberstrikeItemSpecialView()
		{
		}

		public UberstrikeItemSpecialView(ItemView item, int levelRequired, UberstrikeSpecialConfigView config)
			: base(item, levelRequired)
		{
			Config = config;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[UberstrikeSpecialView: ");
			stringBuilder.Append(base.ToString());
			stringBuilder.Append(Config);
			stringBuilder.Append("]]");
			return stringBuilder.ToString();
		}
	}
}
