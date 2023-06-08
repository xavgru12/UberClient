using System.Collections.Generic;
using UberStrike.Core.Types;

public class CustomUberstikeMap
{
	public int MapID;

	public string FileName;

	public List<GameModeType> SupportedModes;

	public CustomUberstikeMap(string file, int id, List<GameModeType> modes)
	{
		MapID = id;
		FileName = file;
		SupportedModes = modes;
	}
}
