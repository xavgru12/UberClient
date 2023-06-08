using System;
using UberStrike.Core.Types;

namespace UberStrike.Core.Models.Views
{
	[Serializable]
	public class UberStrikeItemFunctionalView : BaseUberStrikeItemView
	{
		public override UberstrikeItemType ItemType => UberstrikeItemType.Functional;
	}
}
