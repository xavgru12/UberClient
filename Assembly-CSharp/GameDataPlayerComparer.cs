using UberStrike.Core.Models;

public class GameDataPlayerComparer : GameDataBaseComparer
{
	protected override int OnCompare(GameRoomData a, GameRoomData b)
	{
		int num = a.ConnectedPlayers - b.ConnectedPlayers;
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
