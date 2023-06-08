using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GameState
{
	public static readonly GameState Current = new GameState();

	public readonly Dictionary<int, CharacterConfig> Avatars = new Dictionary<int, CharacterConfig>();

	public readonly Dictionary<int, GameActorInfo> Players = new Dictionary<int, GameActorInfo>();

	public readonly StateMachine<GameStateId> MatchState = new StateMachine<GameStateId>();

	public readonly StateMachine<PlayerStateId> PlayerState = new StateMachine<PlayerStateId>();

	public readonly GameActions Actions = new GameActions();

	public readonly RemotePlayerInterpolator RemotePlayerStates = new RemotePlayerInterpolator();

	public readonly PlayerLeadAudio LeadStatus = new PlayerLeadAudio();

	public readonly Avatar Avatar = new Avatar(Loadout.Empty, local: true);

	public readonly EndOfMatchStats Statistics = new EndOfMatchStats();

	private LocalPlayer player;

	private int roundStartTime;

	private static bool isLoading = false;

	public LocalPlayer Player
	{
		get
		{
			if (player == null && PrefabManager.Instance != null)
			{
				player = PrefabManager.Instance.InstantiateLocalPlayer();
			}
			return player;
		}
	}

	public PlayerData PlayerData
	{
		get;
		private set;
	}

	public MapConfiguration Map
	{
		get;
		set;
	}

	public GameRoomData RoomData
	{
		get;
		set;
	}

	public int RoundsPlayed
	{
		get;
		set;
	}

	public int ScoreRed
	{
		get;
		private set;
	}

	public int ScoreBlue
	{
		get;
		private set;
	}

	public int BlueTeamPlayerCount
	{
		get;
		private set;
	}

	public int RedTeamPlayerCount
	{
		get;
		private set;
	}

	public int PlayerCountReadyForNextRound
	{
		get;
		private set;
	}

	public bool IsInGame
	{
		get
		{
			switch (MatchState.CurrentStateId)
			{
			case GameStateId.None:
			case GameStateId.PregameLoadout:
			case GameStateId.EndOfMatch:
				return false;
			default:
				return true;
			}
		}
	}

	public bool IsMatchRunning => MatchState.CurrentStateId == GameStateId.MatchRunning;

	public bool IsEndOfMatchState => MatchState.CurrentStateId == GameStateId.EndOfMatch;

	public bool IsInAnyGameState => MatchState.CurrentStateId != GameStateId.None;

	public bool IsPlayerPaused => PlayerState.CurrentStateId == PlayerStateId.Paused;

	public bool IsPlayerDead
	{
		get
		{
			if (PlayerState.CurrentStateId != PlayerStateId.Killed)
			{
				return PlayerState.CurrentStateId == PlayerStateId.Spectating;
			}
			return true;
		}
	}

	public bool IsPlaying
	{
		get
		{
			if (PlayerState.CurrentStateId != PlayerStateId.Playing)
			{
				return PlayerState.CurrentStateId == PlayerStateId.Spectating;
			}
			return true;
		}
	}

	public bool IsWaitingForPlayers => MatchState.CurrentStateId == GameStateId.WaitingForPlayers;

	public bool HasJoinedGame => MatchState.CurrentStateId != GameStateId.None;

	public bool IsLocalAvatarLoaded => Avatars.ContainsKey(PlayerDataManager.Cmid);

	public bool IsSinglePlayer => !IsMultiplayer;

	public bool IsGameAboutToEnd => GameTime >= (float)(RoomData.TimeLimit - 1);

	public bool CanJoinRedTeam
	{
		get
		{
			if (!IsAccessAllowed)
			{
				if (!IsGameFull)
				{
					return RedTeamPlayerCount <= BlueTeamPlayerCount;
				}
				return false;
			}
			return true;
		}
	}

	public bool CanJoinBlueTeam
	{
		get
		{
			if (!IsAccessAllowed)
			{
				if (!IsGameFull)
				{
					return BlueTeamPlayerCount <= RedTeamPlayerCount;
				}
				return false;
			}
			return true;
		}
	}

	public bool CanJoinGame
	{
		get
		{
			if (!IsAccessAllowed)
			{
				return !IsGameFull;
			}
			return true;
		}
	}

	public bool IsGameFull => Players.Count >= RoomData.PlayerLimit;

	public bool IsAccessAllowed => PlayerDataManager.AccessLevel >= MemberAccessLevel.Moderator;

	public float GameTime => Mathf.Max((float)((double)(Singleton<GameStateController>.Instance.Client.ServerTimeTicks - roundStartTime) / 1000.0), 0f);

	public GameModeType GameMode => RoomData.GameMode;

	public bool IsMultiplayer => RoomData.GameMode != GameModeType.None;

	public bool IsTeamGame
	{
		get
		{
			if (GameMode != GameModeType.TeamDeathMatch)
			{
				return GameMode == GameModeType.EliminationMode;
			}
			return true;
		}
	}

	private GameState()
	{
		MatchState.OnChanged += delegate(GameStateId el)
		{
			GameData.Instance.GameState.Value = el;
		};
		PlayerState.OnChanged += delegate(PlayerStateId el)
		{
			GameData.Instance.PlayerState.Value = el;
		};
		PlayerData = new PlayerData();
		Reset();
		AutoMonoBehaviour<UnityRuntime>.Instance.OnUpdate += delegate
		{
			if (IsInGame)
			{
				if (IsLocalAvatarLoaded)
				{
					PlayerData.SendUpdates();
				}
				RemotePlayerStates.Update();
			}
			MatchState.Update();
			PlayerState.Update();
		};
	}

	public bool HasAvatarLoaded(int cmid)
	{
		return Avatars.ContainsKey(cmid);
	}

	public void ResetRoundStartTime()
	{
		roundStartTime = Singleton<GameStateController>.Instance.Client.ServerTimeTicks;
	}

	public void Reset()
	{
		Actions.Clear();
		PlayerData.Reset();
		MatchState.Reset();
		PlayerState.Reset();
		RoomData = new GameRoomData
		{
			GameMode = GameModeType.None
		};
		foreach (CharacterConfig value in Avatars.Values)
		{
			value.Destroy();
		}
		RemotePlayerStates.Reset();
		Avatars.Clear();
		Players.Clear();
	}

	public bool TryGetPlayerAvatar(int cmid, out CharacterConfig character)
	{
		if (Avatars.TryGetValue(cmid, out character))
		{
			return character != null;
		}
		return false;
	}

	public bool TryGetActorInfo(int cmid, out GameActorInfo player)
	{
		if (Players.TryGetValue(cmid, out player))
		{
			return player != null;
		}
		return false;
	}

	public void UnloadAvatar(int cmid)
	{
		if (Avatars.TryGetValue(cmid, out CharacterConfig value))
		{
			if ((bool)value)
			{
				value.Destroy();
			}
			Avatars.Remove(cmid);
		}
		Players.Remove(cmid);
	}

	public void EmitRemoteProjectile(int cmid, Vector3 origin, Vector3 direction, byte slot, int projectileID, bool explode)
	{
		if (TryGetPlayerAvatar(cmid, out CharacterConfig character))
		{
			if ((bool)character.Avatar.Decorator.AnimationController)
			{
				character.Avatar.Decorator.AnimationController.Shoot();
			}
			IProjectile projectile = character.WeaponSimulator.EmitProjectile(cmid, character.State.Player.PlayerId, origin, direction, (LoadoutSlotType)slot, projectileID, explode);
			if (projectile != null)
			{
				Singleton<ProjectileManager>.Instance.AddProjectile(projectile, projectileID);
			}
		}
	}

	public void UpdateTeamCounter()
	{
		int num2 = BlueTeamPlayerCount = 0;
		int num4 = RedTeamPlayerCount = num2;
		int num5 = num4;
		foreach (GameActorInfo value in Players.Values)
		{
			if (value.TeamID == TeamID.BLUE)
			{
				BlueTeamPlayerCount++;
			}
			else if (value.TeamID == TeamID.RED)
			{
				RedTeamPlayerCount++;
			}
		}
	}

	public void SingleBulletFire(int cmid)
	{
		if (TryGetPlayerAvatar(cmid, out CharacterConfig character) && character.State.Player.IsAlive && !character.IsLocal)
		{
			if ((bool)character.Avatar.Decorator.AnimationController)
			{
				character.Avatar.Decorator.AnimationController.Shoot();
			}
			character.WeaponSimulator.Shoot(character.State);
		}
	}

	public void QuickItemEvent(int cmid, byte eventType, int robotLifeTime, int scrapsLifeTime, bool isInstant)
	{
		if (TryGetPlayerAvatar(cmid, out CharacterConfig character))
		{
			Singleton<QuickItemSfxController>.Instance.ShowThirdPersonEffect(character, (QuickItemLogic)eventType, robotLifeTime, scrapsLifeTime, isInstant);
		}
	}

	public void ActivateQuickItem(int cmid, QuickItemLogic logic, int robotLifeTime, int scrapsLifeTime, bool isInstant)
	{
		if (TryGetPlayerAvatar(cmid, out CharacterConfig character))
		{
			Singleton<QuickItemSfxController>.Instance.ShowThirdPersonEffect(character, logic, robotLifeTime, scrapsLifeTime, isInstant);
		}
	}

	public void UpdatePlayersReady()
	{
		PlayerCountReadyForNextRound = 0;
		foreach (GameActorInfo value in Players.Values)
		{
			if (value.IsReadyForGame)
			{
				PlayerCountReadyForNextRound++;
			}
		}
	}

	public void UpdateTeamScore(int blueScore, int redScore)
	{
		ScoreRed = redScore;
		ScoreBlue = blueScore;
		Current.PlayerData.BlueTeamScore.Value = blueScore;
		Current.PlayerData.RedTeamScore.Value = redScore;
		int num = RoomData.KillLimit - Math.Max(redScore, blueScore);
		Current.PlayerData.RemainingKills.Value = num;
		if (MatchState.CurrentStateId == GameStateId.MatchRunning)
		{
			LeadStatus.PlayKillsLeftAudio(num);
		}
		switch (PlayerData.Player.TeamID)
		{
		case TeamID.RED:
			LeadStatus.UpdateLeadStatus(redScore, blueScore, num > 0 && MatchState.CurrentStateId == GameStateId.MatchRunning);
			break;
		case TeamID.BLUE:
			LeadStatus.UpdateLeadStatus(blueScore, redScore, num > 0 && MatchState.CurrentStateId == GameStateId.MatchRunning);
			break;
		}
	}

	private void UpdateDeathmatchScore()
	{
		int num = 0;
		foreach (GameActorInfo value in Players.Values)
		{
			if (value.Cmid != PlayerDataManager.Cmid && num < value.Kills)
			{
				num = value.Kills;
			}
		}
	}

	public void PlayerKilled(int shooter, int target, UberstrikeItemClass weaponClass, BodyPart bodyPart, Vector3 direction)
	{
		if (Avatars.TryGetValue(target, out CharacterConfig value) && !value.IsDead)
		{
			Avatars[target].SetDead(direction, BodyPart.Body, target, weaponClass);
			GameActorInfo valueOrDefault = Players.GetValueOrDefault(shooter);
			GameActorInfo valueOrDefault2 = Players.GetValueOrDefault(target);
			if (valueOrDefault2 == null)
			{
				Debug.LogError("Kill target is null " + target.ToString());
			}
			GameData.Instance.OnPlayerKilled.Fire(valueOrDefault, valueOrDefault2, weaponClass, bodyPart);
			if (target == PlayerDataManager.Cmid)
			{
				EventHandler.Global.Fire(new GameEvents.PlayerDied());
			}
		}
	}

	public void PlayerDamaged(DamageEvent damageEvent)
	{
		if (Player != null)
		{
			foreach (KeyValuePair<byte, byte> item in damageEvent.Damage)
			{
				EventHandler.Global.Fire(new GameEvents.PlayerDamage
				{
					Angle = Conversion.Byte2Angle(item.Key),
					DamageValue = (int)item.Value
				});
				if ((damageEvent.DamageEffectFlag & 1) != 0)
				{
					Player.DamageFactor = damageEvent.DamgeEffectValue;
				}
			}
		}
	}

	public void StartMatch(int roundNumber, int endTime)
	{
		roundStartTime = endTime - RoomData.TimeLimit * 1000;
		LeadStatus.Reset();
		Singleton<GameStateController>.Instance.Client.Peer.FetchServerTimestamp();
		CheatDetection.SyncSystemTime();
		LevelCamera.ResetFeedback();
		int value = RoomData.KillLimit - Math.Max(Current.PlayerData.BlueTeamScore, Current.PlayerData.RedTeamScore);
		Current.PlayerData.RemainingKills.Value = value;
		Current.PlayerData.RemainingTime.Value = 0;
	}

	public void UpdatePlayerStatistics(StatsCollection totalStats, StatsCollection bestPerLife)
	{
		int playerLevel = PlayerDataManager.PlayerLevel;
		if (playerLevel > 0 && playerLevel < XpPointsUtil.Config.MaxLevel)
		{
			Singleton<PlayerDataManager>.Instance.UpdatePlayerStats(totalStats, bestPerLife);
			if (PlayerDataManager.PlayerLevel != playerLevel)
			{
				PopupSystem.Show(new LevelUpPopup(PlayerDataManager.PlayerLevel, playerLevel));
			}
			GlobalUIRibbon.Instance.AddXPEvent(totalStats.Xp);
		}
		if (totalStats.Points > 0)
		{
			PlayerDataManager.Points += totalStats.Points;
			GlobalUIRibbon.Instance.AddPointsEvent(totalStats.Points);
		}
	}

	public AchievementType GetPlayersFirstAchievement(EndOfMatchData endOfMatchData)
	{
		AchievementType result = AchievementType.None;
		StatsSummary statsSummary = endOfMatchData.MostValuablePlayers.Find((StatsSummary p) => p.Cmid == PlayerDataManager.Cmid);
		if (statsSummary != null)
		{
			List<AchievementType> list = new List<AchievementType>();
			foreach (KeyValuePair<byte, ushort> achievement in statsSummary.Achievements)
			{
				list.Add((AchievementType)achievement.Key);
			}
			if (list.Count > 0)
			{
				result = list[0];
			}
		}
		return result;
	}

	public void EmitRemoteQuickItem(Vector3 origin, Vector3 direction, int itemId, byte playerNumber, int projectileID)
	{
		IUnityItem itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(itemId);
		if (itemInShop != null)
		{
			if ((bool)itemInShop.Prefab)
			{
				IGrenadeProjectile grenadeProjectile = itemInShop.Prefab.GetComponent<QuickItem>() as IGrenadeProjectile;
				try
				{
					IGrenadeProjectile grenadeProjectile2 = grenadeProjectile.Throw(origin, direction);
					if (playerNumber == PlayerData.Player.PlayerId)
					{
						grenadeProjectile2.SetLayer(UberstrikeLayer.LocalProjectile);
					}
					else
					{
						grenadeProjectile2.SetLayer(UberstrikeLayer.RemoteProjectile);
					}
					Singleton<ProjectileManager>.Instance.AddProjectile(grenadeProjectile2, projectileID);
				}
				catch (Exception exception)
				{
					Debug.LogWarning("OnEmitQuickItem failed because Item is not a projectile: " + itemId.ToString() + "/" + playerNumber.ToString() + "/" + projectileID.ToString());
					Debug.LogException(exception);
				}
			}
		}
		else
		{
			Debug.LogError("OnEmitQuickItem failed because item not found: " + itemId.ToString() + "/" + playerNumber.ToString() + "/" + projectileID.ToString());
		}
	}

	public void PlayerLeftGame(int cmid)
	{
		try
		{
			EventHandler.Global.Fire(new GameEvents.PlayerLeft
			{
				Cmid = cmid
			});
			if (Players.TryGetValue(cmid, out GameActorInfo value))
			{
				GameData.Instance.OnHUDStreamMessage.Fire(value, LocalizedStrings.LeftTheGame, null);
				Debug.Log("<< OnPlayerLeftGame " + value.PlayerName + " " + MatchState.CurrentStateId.ToString());
				if (value.Cmid == PlayerDataManager.Cmid)
				{
					Player.SetCurrentCharacterConfig(null);
				}
				else
				{
					RemotePlayerStates.RemoveCharacterInfo(value.PlayerId);
				}
			}
			UnloadAvatar(cmid);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		finally
		{
			Singleton<ChatManager>.Instance.SetGameSection(RoomData.Server.ConnectionString, RoomData.Number, RoomData.MapID, Players.Values);
		}
	}

	public void AllPlayerDeltas(List<GameActorInfoDelta> players)
	{
		bool flag = false;
		bool flag2 = false;
		foreach (GameActorInfoDelta player2 in players)
		{
			try
			{
				if (player2.Changes.Count > 0)
				{
					PlayerDelta(player2);
					if (player2.Changes.ContainsKey(GameActorInfoDelta.Keys.TeamID))
					{
						flag = true;
					}
					if (player2.Changes.ContainsKey(GameActorInfoDelta.Keys.Kills))
					{
						flag2 = true;
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
		if (flag)
		{
			UpdateTeamCounter();
		}
		if (flag2 && GameMode == GameModeType.DeathMatch)
		{
			UpdateDeathmatchScore();
		}
	}

	public void PlayerDelta(GameActorInfoDelta update)
	{
		if (update.Id == PlayerData.Player.PlayerId)
		{
			PlayerData.DeltaUpdate(update);
		}
		else
		{
			RemotePlayerStates.DeltaUpdate(update);
		}
	}

	public void AllPositionUpdate(List<PlayerMovement> positions, ushort gameFrame)
	{
		foreach (PlayerMovement position in positions)
		{
			if (position.Number != PlayerData.Player.PlayerId)
			{
				RemotePlayerStates.PositionUpdate(position, gameFrame);
			}
		}
	}

	public void RespawnLocalPlayerAt(Vector3 position, Quaternion rotation)
	{
		Player.SpawnPlayerAt(position, rotation);
		GameActorInfo info;
		if (Avatars.TryGetValue(PlayerDataManager.Cmid, out CharacterConfig value))
		{
			value.Reset();
		}
		else if (TryGetActorInfo(PlayerDataManager.Cmid, out info))
		{
			InstantiateAvatar(info);
		}
	}

	public void PlayerRespawned(int cmid, Vector3 position, byte rotation)
	{
		if (TryGetActorInfo(cmid, out GameActorInfo gameActorInfo))
		{
			if (gameActorInfo.Cmid == PlayerDataManager.Cmid && gameActorInfo.TeamID == TeamID.NONE && GameMode != GameModeType.DeathMatch)
			{
				Debug.LogWarning("PlayerRespawned failed, invalid team for gamemode");
				Singleton<GameStateController>.Instance.LeaveGame();
				return;
			}
			if (!Avatars.ContainsKey(cmid))
			{
				InstantiateAvatar(gameActorInfo);
			}
			if (Avatars.TryGetValue(cmid, out CharacterConfig value))
			{
				RemotePlayerStates.UpdatePositionHard(value.State.Player.PlayerId, position);
				value.Reset();
			}
			if (cmid == PlayerDataManager.Cmid)
			{
				EventHandler.Global.Fire(new GameEvents.PlayerRespawn
				{
					Position = position,
					Rotation = Conversion.Byte2Angle(rotation)
				});
			}
		}
		else
		{
			Debug.LogError($"PlayerRespawned failed {cmid} because not found in the list of players!");
		}
	}

	public void InstantiateAvatar(GameActorInfo info, bool isTraining = false)
	{
		if (Avatars.ContainsKey(info.Cmid))
		{
			Debug.LogError($"Skipping Avatar instantiation for cmid {info.Cmid}");
		}
		else if (info.Cmid == PlayerDataManager.Cmid)
		{
			CharacterConfig characterConfig = PrefabManager.Instance.InstantiateLocalCharacter();
			Avatars.Add(info.Cmid, characterConfig);
			ConfigureAvatar(info, characterConfig, isLocal: true);
		}
		else
		{
			CharacterConfig characterConfig2 = PrefabManager.Instance.InstantiateRemoteCharacter();
			Avatars.Add(info.Cmid, characterConfig2);
			ConfigureAvatar(info, characterConfig2, isLocal: false);
		}
	}

	private void ConfigureAvatar(GameActorInfo info, CharacterConfig character, bool isLocal)
	{
		if (character != null && info != null)
		{
			if (isLocal)
			{
				Player.SetCurrentCharacterConfig(character);
				UberKill.IsLowGravity = GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.LowGravity, RoomData.GameFlags);
				character.Initialize(PlayerData, Avatar);
			}
			else
			{
				Avatar avatar = new Avatar(new Loadout(info.Gear, info.Weapons), local: false);
				avatar.SetDecorator(AvatarBuilder.CreateRemoteAvatar(avatar.Loadout.GetAvatarGear(), info.SkinColor));
				character.Initialize(RemotePlayerStates.GetState(info.PlayerId), avatar);
				GameData.Instance.OnHUDStreamMessage.Fire(info, LocalizedStrings.JoinedTheGame, null);
			}
			if (!info.IsAlive)
			{
				character.SetDead(Vector3.zero);
			}
		}
		else
		{
			Debug.LogError($"OnAvatarLoaded failed because loaded Avatar is {character != null} and Info is {info != null}");
		}
	}

	public bool SendChatMessage(string message, ChatContext context)
	{
		message = ChatMessageFilter.Cleanup(message);
		if (!string.IsNullOrEmpty(message) && !ChatMessageFilter.IsSpamming(message))
		{
			GameStateHelper.OnChatMessage(PlayerDataManager.Cmid, PlayerDataManager.Name, message, PlayerDataManager.AccessLevel, (byte)ChatManager.CurrentChatContext);
			Actions.ChatMessage(message, (byte)ChatManager.CurrentChatContext);
			return true;
		}
		return false;
	}

	public IEnumerator ConfigureAvatarAsync(GameActorInfo info, CharacterConfig character, bool isLocal, bool isTraining)
	{
		while (isLoading)
		{
			yield return new WaitForSeconds(0.1f);
		}
		isLoading = true;
		if (character != null && info != null)
		{
			if (isLocal)
			{
				Current.Player.SetCurrentCharacterConfig(character);
				if (!isTraining)
				{
					UberKill.IsLowGravity = GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.LowGravity, Current.RoomData.GameFlags);
				}
				character.Initialize(Current.PlayerData, Current.Avatar);
			}
			else
			{
				AvatarGearParts avatargearparts = new AvatarGearParts();
				Avatar avatar = new Avatar(new Loadout(info.Gear, info.Weapons), local: false);
				yield return UnityRuntime.StartRoutine(avatar.Loadout.GetGearPart(delegate(AvatarGearParts callback)
				{
					avatargearparts = callback;
				}));
				avatar.SetDecorator(AvatarBuilder.CreateRemoteAvatar(avatargearparts, info.SkinColor));
				character.Initialize(Current.RemotePlayerStates.GetState(info.PlayerId), avatar);
				GameData.Instance.OnHUDStreamMessage.Fire(info, LocalizedStrings.JoinedTheGame, null);
			}
			if (!info.IsAlive)
			{
				character.SetDead(Vector3.zero);
			}
		}
		else
		{
			Debug.LogError($"OnAvatarLoaded failed because loaded Avatar is {character != null} and Info is {info != null}");
		}
		isLoading = false;
	}
}
