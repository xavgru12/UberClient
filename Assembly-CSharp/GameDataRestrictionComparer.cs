using System.Collections.Generic;
using UberStrike.Core.Models;

public class GameDataRestrictionComparer : GameDataBaseComparer
{
	private int _playerLevel;

	private IComparer<GameRoomData> _baseComparer;

	public GameDataRestrictionComparer(int playerLevel, IComparer<GameRoomData> baseComparer)
	{
		_playerLevel = playerLevel;
		_baseComparer = baseComparer;
	}

	protected override int OnCompare(GameRoomData x, GameRoomData y)
	{
		if (GameRoomHelper.HasLevelRestriction(x) || GameRoomHelper.HasLevelRestriction(y))
		{
			if (_playerLevel < 5)
			{
				return NoobLevelsUp(x, y);
			}
			return VeteranLevelsUp(x, y);
		}
		return _baseComparer.Compare(x, y);
	}

	private int NoobLevelsUp(GameRoomData x, GameRoomData y)
	{
		return ((x.LevelMin >= 5 || x.LevelMin == 0) ? x.LevelMin : (x.LevelMin - 100)) - ((y.LevelMin >= 5 || y.LevelMin == 0) ? y.LevelMin : (y.LevelMin - 100));
	}

	private int VeteranLevelsUp(GameRoomData x, GameRoomData y)
	{
		return ((x.LevelMin >= 5) ? x.LevelMin : (x.LevelMin + 100)) - ((y.LevelMin >= 5) ? y.LevelMin : (y.LevelMin + 100));
	}
}
