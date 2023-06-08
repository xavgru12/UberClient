using UberStrike.Core.Models;

public class GameDataMapComparer : GameDataBaseComparer
{
	protected override int OnCompare(GameRoomData a, GameRoomData b)
	{
		int num = a.MapID - b.MapID;
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
