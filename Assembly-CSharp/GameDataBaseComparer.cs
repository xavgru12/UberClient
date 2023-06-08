using System.Collections.Generic;
using UberStrike.Core.Models;

public abstract class GameDataBaseComparer : IComparer<GameRoomData>
{
	public int Compare(GameRoomData x, GameRoomData y)
	{
		int num = GameRoomHelper.CanJoinGame(y).CompareTo(GameRoomHelper.CanJoinGame(x));
		if (num == 0)
		{
			return OnCompare(x, y);
		}
		return num;
	}

	protected abstract int OnCompare(GameRoomData x, GameRoomData y);
}
