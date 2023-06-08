using UberStrike.Core.Models;

public class GameDataRuleComparer : GameDataBaseComparer
{
	protected override int OnCompare(GameRoomData a, GameRoomData b)
	{
		int num = (short)a.GameMode - (short)b.GameMode;
		if (num != 0)
		{
			if (GameDataComparer.SortAscending)
			{
				return num;
			}
			return -num;
		}
		return GameDataNameComparer.StaticCompare(a, b);
	}
}
