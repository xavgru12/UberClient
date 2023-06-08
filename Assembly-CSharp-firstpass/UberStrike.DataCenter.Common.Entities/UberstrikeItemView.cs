using Cmune.DataCenter.Common.Entities;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
	public class UberstrikeItemView : ItemView
	{
		public int LevelRequired
		{
			get;
			set;
		}

		public UberstrikeItemView()
		{
		}

		public UberstrikeItemView(ItemView item, int levelRequired)
			: base(item)
		{
			LevelRequired = levelRequired;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[UberstrikeItemView: ");
			stringBuilder.Append(base.ToString());
			stringBuilder.Append("[LevelRequired: ");
			stringBuilder.Append(LevelRequired);
			stringBuilder.Append("]]");
			return stringBuilder.ToString();
		}
	}
}
