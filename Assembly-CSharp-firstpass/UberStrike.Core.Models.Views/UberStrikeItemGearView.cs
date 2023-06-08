using System;
using UberStrike.Core.Types;

namespace UberStrike.Core.Models.Views
{
	[Serializable]
	public class UberStrikeItemGearView : BaseUberStrikeItemView
	{
		public override UberstrikeItemType ItemType => UberstrikeItemType.Gear;

		public int ArmorPoints
		{
			get;
			set;
		}

		public int ArmorWeight
		{
			get;
			set;
		}
	}
}
