using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.Linq;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal static class GameStateHelper
{
	private class KillSorter : Comparer<GameActorInfo>
	{
		public override int Compare(GameActorInfo x, GameActorInfo y)
		{
			int num = y.Kills - x.Kills;
			if (num == 0)
			{
				num = x.Deaths - y.Deaths;
			}
			return num;
		}
	}

	public static bool IsLocalConnection(ConnectionAddress address)
	{
		if (!address.IpAddress.StartsWith("10.") && !address.IpAddress.StartsWith("172.16.") && !address.IpAddress.StartsWith("192.168."))
		{
			return address.IpAddress.StartsWith("127.");
		}
		return true;
	}

	public static void UpdateMatchTime()
	{
		GameState.Current.PlayerData.RemainingTime.Value = GameState.Current.RoomData.TimeLimit - Mathf.CeilToInt(GameState.Current.GameTime);
	}

	public static void EnterGameMode()
	{
		TabScreenPanelGUI.SetGameName(GameState.Current.RoomData.Name);
		TabScreenPanelGUI.SetServerName(Singleton<GameServerManager>.Instance.GetServerName(GameState.Current.RoomData));
		LevelCamera.SetLevelCamera(GameState.Current.Map.Camera, GameState.Current.Map.DefaultViewPoint.position, GameState.Current.Map.DefaultViewPoint.rotation);
		GameState.Current.Player.SetEnabled(enabled: true);
		GameState.Current.UpdateTeamCounter();
	}

	public static void ExitGameMode()
	{
		GameData.Instance.GameState.Value = GameStateId.None;
		GameData.Instance.PlayerState.Value = PlayerStateId.None;
		GameState.Current.Reset();
		Singleton<WeaponController>.Instance.StopInputHandler();
		Singleton<QuickItemController>.Instance.Clear();
		Singleton<ProjectileManager>.Instance.Clear();
		GameData.Instance.OnHUDChatClear.Fire();
		GameData.Instance.OnHUDChatClear.Fire();
		GameData.Instance.OnHUDStreamClear.Fire();
		Singleton<ChatManager>.Instance.UpdateLastgamePlayers();
		if (GameState.Current.Avatar != null)
		{
			GameState.Current.Avatar.CleanupRagdoll();
		}
		if ((bool)GameState.Current.Player.Character)
		{
			GameState.Current.Player.Character.Destroy();
			GameState.Current.Player.SetCurrentCharacterConfig(null);
		}
		GameState.Current.Player.SetEnabled(enabled: false);
	}

	public static void OnPlayerChangeTeam(int cmid, TeamID team)
	{
		if (GameState.Current.TryGetActorInfo(cmid, out GameActorInfo player))
		{
			player.TeamID = team;
			GameState.Current.UpdateTeamCounter();
			if (cmid == PlayerDataManager.Cmid)
			{
				GameState.Current.PlayerData.Player.TeamID = team;
				GameState.Current.PlayerData.FocusedPlayerTeam.Value = TeamID.NONE;
				GameState.Current.PlayerData.Team.Value = team;
			}
			string arg = (team != TeamID.BLUE) ? LocalizedStrings.Red : LocalizedStrings.Blue;
			GameData.Instance.OnHUDStreamMessage.Fire(player, string.Format(LocalizedStrings.ChangingToTeamN, arg), null);
		}
	}

	public static bool CanChangeTeam()
	{
		if (GameState.Current.MatchState.CurrentStateId != GameStateId.MatchRunning || GameState.Current.GameMode == GameModeType.DeathMatch)
		{
			return false;
		}
		int num = 0;
		int num2 = 0;
		foreach (GameActorInfo value in GameState.Current.Players.Values)
		{
			if (value.TeamID == TeamID.BLUE)
			{
				num++;
			}
			if (value.TeamID == TeamID.RED)
			{
				num2++;
			}
		}
		if (GameState.Current.PlayerData.Player.TeamID == TeamID.BLUE)
		{
			return num > num2;
		}
		return num < num2;
	}

	public static void SortDeathMatchPlayers(IEnumerable<GameActorInfo> toBeSortedPlayers)
	{
		List<GameActorInfo> list = toBeSortedPlayers.Where((GameActorInfo a) => a.TeamID == TeamID.NONE).ToList();
		list.Sort(new KillSorter());
		TabScreenPanelGUI.SetPlayerListAll(list);
	}

	public static void SortTeamMatchPlayers(IEnumerable<GameActorInfo> toBeSortedPlayers)
	{
		List<GameActorInfo> list = toBeSortedPlayers.Where((GameActorInfo a) => a.TeamID == TeamID.BLUE).ToList();
		List<GameActorInfo> list2 = toBeSortedPlayers.Where((GameActorInfo a) => a.TeamID == TeamID.RED).ToList();
		list.Sort(new KillSorter());
		list2.Sort(new KillSorter());
		TabScreenPanelGUI.SetPlayerListBlue(list);
		TabScreenPanelGUI.SetPlayerListRed(list2);
	}

	public static byte GetDamageDirectionAngle(Vector3 direction)
	{
		byte result = 0;
		Vector3 normalized = direction.normalized;
		normalized.y = 0f;
		if (normalized.magnitude != 0f)
		{
			Vector3 eulerAngles = Quaternion.LookRotation(normalized).eulerAngles;
			result = Conversion.Angle2Byte(eulerAngles.y);
		}
		return result;
	}

	public static void PlayerHit(int targetCmid, ushort damage, BodyPart part, Vector3 force)
	{
		PlayerData playerData = GameState.Current.PlayerData;
		playerData.GetArmorDamage((short)damage, part, out short healthDamage, out byte armorDamage);
		playerData.Health.Value -= healthDamage;
		playerData.ArmorPoints.Value -= armorDamage;
		GameState.Current.Player.MoveController.ApplyForce(force, CharacterMoveController.ForceType.Additive);
	}

	public static void RespawnLocalPlayerAtRandom()
	{
		Singleton<SpawnPointManager>.Instance.GetRandomSpawnPoint(GameState.Current.GameMode, GameState.Current.PlayerData.Player.TeamID, out Vector3 position, out Quaternion rotation);
		GameState.Current.RespawnLocalPlayerAt(position, rotation);
	}

	public static string GetModeName(GameModeType gameMode)
	{
		switch (gameMode)
		{
		case GameModeType.DeathMatch:
			return LocalizedStrings.DeathMatch;
		case GameModeType.TeamDeathMatch:
			return LocalizedStrings.TeamDeathMatch;
		case GameModeType.EliminationMode:
			return LocalizedStrings.TeamElimination;
		default:
			return LocalizedStrings.TrainingCaps;
		}
	}

	internal static void OnChatMessage(int cmid, string name, string message, MemberAccessLevel accessLevel, byte context)
	{
		if (ChatManager.CanShowMessage((ChatContext)context))
		{
			try
			{
				GameState.Current.Players.TryGetValue(cmid, out GameActorInfo value);
				if (value != null && !string.IsNullOrEmpty(value.ClanTag))
				{
					name = $"[{value.ClanTag}] {value.PlayerName}";
				}
			}
			catch
			{
			}
			GameData.Instance.OnHUDChatMessage.Fire(name, message, accessLevel);
		}
		Singleton<ChatManager>.Instance.InGameDialog.AddMessage(new InstantMessage(cmid, name, message, accessLevel, (ChatContext)context));
	}
}
