// Decompiled with JetBrains decompiler
// Type: JoinGameUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;

internal class JoinGameUtil
{
  private static bool IsGameFull(GameMetaData data) => data.ConnectedPlayers >= data.MaxPlayers;

  private static bool IsAccessAllowed => PlayerDataManager.AccessLevelSecure >= MemberAccessLevel.SeniorModerator;

  public static bool IsMobileChannel(ChannelType channel) => channel == ChannelType.Android || channel == ChannelType.IPad || channel == ChannelType.IPhone;

  public static bool CanJoinGame(FpsGameMode game)
  {
    if (!game.IsInitialized)
      return false;
    return JoinGameUtil.IsAccessAllowed || !JoinGameUtil.IsGameFull(game.GameData);
  }

  public static bool CanJoinBlueTeam(TeamDeathMatchGameMode game)
  {
    if (!game.IsInitialized)
      return false;
    if (JoinGameUtil.IsAccessAllowed)
      return true;
    return !JoinGameUtil.IsGameFull(game.GameData) && game.CanJoinBlueTeam;
  }

  public static bool CanJoinRedTeam(TeamDeathMatchGameMode game)
  {
    if (!game.IsInitialized)
      return false;
    if (JoinGameUtil.IsAccessAllowed)
      return true;
    return !JoinGameUtil.IsGameFull(game.GameData) && game.CanJoinRedTeam;
  }
}
