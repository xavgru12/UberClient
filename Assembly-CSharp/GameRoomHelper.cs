using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Models;

public static class GameRoomHelper
{
	public static bool HasLevelRestriction(GameRoomData room)
	{
		if (room.LevelMin == 0)
		{
			return room.LevelMax != 0;
		}
		return true;
	}

	public static bool IsLevelAllowed(int min, int max, int level)
	{
		if (level >= min)
		{
			if (max != 0)
			{
				return level <= max;
			}
			return true;
		}
		return false;
	}

	public static bool IsLevelAllowed(GameRoomData room, int level)
	{
		return IsLevelAllowed(room.LevelMin, room.LevelMax, level);
	}

	public static bool CanJoinGame(GameRoomData game)
	{
		bool flag = !game.IsFull && IsLevelAllowed(game, PlayerDataManager.PlayerLevel);
		flag |= (PlayerDataManager.AccessLevel >= MemberAccessLevel.Moderator);
		return flag & Singleton<MapManager>.Instance.HasMapWithId(game.MapID);
	}
}
