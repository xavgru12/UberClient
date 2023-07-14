// Decompiled with JetBrains decompiler
// Type: TabScreenPlayerSorter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

internal static class TabScreenPlayerSorter
{
  public static void SortDeathMatchPlayers(IEnumerable<CharacterInfo> toBeSortedPlayers)
  {
    List<CharacterInfo> players = new List<CharacterInfo>(toBeSortedPlayers);
    players.Sort((IComparer<CharacterInfo>) new TabScreenPlayerSorter.PlayerSplatSorter());
    TabScreenPanelGUI.Instance.SetPlayerListAll(players);
  }

  public static void SortTeamMatchPlayers(IEnumerable<CharacterInfo> toBeSortedPlayers)
  {
    List<CharacterInfo> bluePlayers = new List<CharacterInfo>();
    List<CharacterInfo> redPlayers = new List<CharacterInfo>();
    foreach (CharacterInfo toBeSortedPlayer in toBeSortedPlayers)
    {
      if (toBeSortedPlayer.TeamID == TeamID.BLUE)
        bluePlayers.Add(toBeSortedPlayer);
      else if (toBeSortedPlayer.TeamID == TeamID.RED)
        redPlayers.Add(toBeSortedPlayer);
    }
    bluePlayers.Sort((IComparer<CharacterInfo>) new TabScreenPlayerSorter.PlayerSplatSorter());
    redPlayers.Sort((IComparer<CharacterInfo>) new TabScreenPlayerSorter.PlayerSplatSorter());
    TabScreenPanelGUI.Instance.SetPlayerListBlue(bluePlayers);
    TabScreenPanelGUI.Instance.SetPlayerListRed(redPlayers);
  }

  private class PlayerSplatSorter : Comparer<CharacterInfo>
  {
    public override int Compare(CharacterInfo x, CharacterInfo y) => (int) y.Kills - (int) x.Kills;
  }
}
