using Cmune.Core.Models;
using System.Collections.Generic;

public class GameServerPlayerCountComparer : IComparer<PhotonServer>
{
	public int Compare(PhotonServer a, PhotonServer b)
	{
		return StaticCompare(a, b);
	}

	public static int StaticCompare(PhotonServer a, PhotonServer b)
	{
		int num = 1;
		if (a.Data.PlayersConnected == b.Data.PlayersConnected)
		{
			return string.Compare(b.Name, a.Name);
		}
		if (((a.Data.State != PhotonServerLoad.Status.Alive) ? 1000 : a.Data.PlayersConnected) > ((b.Data.State != PhotonServerLoad.Status.Alive) ? 1000 : b.Data.PlayersConnected))
		{
			return num;
		}
		return num * -1;
	}
}
