using UberStrike.Core.Models;

public class GameDataTimeComparer : GameDataBaseComparer
{
	protected override int OnCompare(GameRoomData a, GameRoomData b)
	{
		int num = a.TimeLimit - b.TimeLimit;
		if (GameDataComparer.SortAscending)
		{
			return num;
		}
		return -num;
	}
}
