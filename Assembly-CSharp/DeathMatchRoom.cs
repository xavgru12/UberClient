using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Realtime.Client;
using UnityEngine;

public class DeathMatchRoom : BaseGameRoom, IDisposable, IGameMode
{
	private bool isDisposed;

	public GameMode Type => GameMode.DeathMatch;

	public DeathMatchRoom(GameRoomData gameData, GamePeer peer)
	{
		GameState.Current.MatchState.RegisterState(GameStateId.MatchRunning, new MatchRunningState(GameState.Current.MatchState));
		GameState.Current.MatchState.RegisterState(GameStateId.PregameLoadout, new PregameLoadoutState(GameState.Current.MatchState));
		GameState.Current.MatchState.RegisterState(GameStateId.PrepareNextRound, new PrepareNextRoundState(GameState.Current.MatchState));
		GameState.Current.MatchState.RegisterState(GameStateId.EndOfMatch, new EndOfMatchState(GameState.Current.MatchState));
		GameState.Current.MatchState.RegisterState(GameStateId.WaitingForPlayers, new WaitingForPlayersState(GameState.Current.MatchState));
		GameState.Current.MatchState.RegisterState(GameStateId.AfterRound, new AfterRoundState(GameState.Current.MatchState));
		GameState.Current.PlayerState.RegisterState(PlayerStateId.Overview, new PlayerOverviewState(GameState.Current.PlayerState));
		GameState.Current.PlayerState.RegisterState(PlayerStateId.Playing, new PlayerPlayingState(GameState.Current.PlayerState));
		GameState.Current.PlayerState.RegisterState(PlayerStateId.PrepareForMatch, new PlayerPrepareState(GameState.Current.PlayerState));
		GameState.Current.PlayerState.RegisterState(PlayerStateId.Killed, new PlayerKilledState(GameState.Current.PlayerState));
		GameState.Current.PlayerState.RegisterState(PlayerStateId.Paused, new PlayerPausedState(GameState.Current.PlayerState));
		GameState.Current.PlayerState.RegisterState(PlayerStateId.AfterRound, new PlayerAfterRoundState(GameState.Current.PlayerState));
		GameState.Current.PlayerState.RegisterState(PlayerStateId.Spectating, new PlayerSpectatingState(GameState.Current.PlayerState));
		GameState.Current.PlayerData.SendJumpUpdate += base.Operations.SendJump;
		GameState.Current.PlayerData.SendMovementUpdate += base.Operations.SendUpdatePositionAndRotation;
		GameState.Current.RoomData = gameData;
		GameState.Current.Actions.IncreaseHealthAndArmor = delegate(int health, int armor)
		{
			base.Operations.SendIncreaseHealthAndArmor((byte)health, (byte)armor);
		};
		GameState.Current.Actions.RequestRespawn = base.Operations.SendRespawnRequest;
		GameState.Current.Actions.PickupPowerup = delegate(int pickupID, PickupItemType type, int value)
		{
			base.Operations.SendPowerUpPicked(pickupID, (byte)type, (byte)value);
		};
		GameState.Current.Actions.OpenDoor = base.Operations.SendOpenDoor;
		GameState.Current.Actions.EmitQuickItem = base.Operations.SendEmitQuickItem;
		GameState.Current.Actions.EmitProjectile = delegate(Vector3 origin, Vector3 direction, LoadoutSlotType slot, int projectileID, bool explode)
		{
			base.Operations.SendEmitProjectile(origin, direction, (byte)slot, projectileID, explode);
		};
		GameState.Current.Actions.RemoveProjectile = base.Operations.SendRemoveProjectile;
		GameState.Current.Actions.SingleBulletFire = base.Operations.SendSingleBulletFire;
		GameState.Current.Actions.KillPlayer = delegate
		{
			if (GameState.Current.IsInGame && GameState.Current.PlayerData.IsAlive)
			{
				base.Operations.SendDirectDeath();
			}
		};
		GameState.Current.Actions.DirectHitDamage = delegate(int targetCmid, ushort damage, BodyPart part, Vector3 force, byte slot, byte bullets)
		{
			base.Operations.SendDirectHitDamage(targetCmid, (byte)part, bullets, slot);
			if (PlayerDataManager.Cmid == targetCmid && GameState.Current.IsMatchRunning)
			{
				GameStateHelper.PlayerHit(targetCmid, damage, part, force);
			}
		};
		GameState.Current.Actions.ExplosionHitDamage = delegate(int targetCmid, ushort damage, Vector3 force, byte slot, byte distance)
		{
			base.Operations.SendExplosionDamage(targetCmid, slot, distance, force);
			if (PlayerDataManager.Cmid == targetCmid && GameState.Current.IsMatchRunning)
			{
				GameStateHelper.PlayerHit(targetCmid, damage, BodyPart.Body, force);
			}
		};
		GameState.Current.Actions.PlayerHitFeeback = base.Operations.SendHitFeedback;
		GameState.Current.Actions.ActivateQuickItem = base.Operations.SendActivateQuickItem;
		GameState.Current.Actions.JoinTeam = delegate(TeamID team)
		{
			base.Operations.SendJoinGame(team);
			GameState.Current.MatchState.PopAllStates();
		};
		GameState.Current.Actions.JoinAsSpectator = delegate
		{
			base.Operations.SendJoinAsSpectator();
			GameState.Current.MatchState.PopAllStates();
		};
		GameState.Current.Actions.KickPlayer = base.Operations.SendKickPlayer;
		GameState.Current.Actions.ChatMessage = base.Operations.SendChatMessage;
		GameState.Current.PlayerData.Actions.Clear();
		PlayerActions actions = GameState.Current.PlayerData.Actions;
		actions.UpdateKeyState = (Action<byte>)Delegate.Combine(actions.UpdateKeyState, new Action<byte>(peer.Operations.SendUpdateKeyState));
		PlayerActions actions2 = GameState.Current.PlayerData.Actions;
		actions2.SwitchWeapon = (Action<byte>)Delegate.Combine(actions2.SwitchWeapon, new Action<byte>(base.Operations.SendSwitchWeapon));
		PlayerActions actions3 = GameState.Current.PlayerData.Actions;
		actions3.UpdatePing = (Action<ushort>)Delegate.Combine(actions3.UpdatePing, new Action<ushort>(peer.Operations.SendUpdatePing));
		PlayerActions actions4 = GameState.Current.PlayerData.Actions;
		actions4.PausePlayer = (Action<bool>)Delegate.Combine(actions4.PausePlayer, new Action<bool>(base.Operations.SendIsPaused));
		PlayerActions actions5 = GameState.Current.PlayerData.Actions;
		actions5.SniperMode = (Action<bool>)Delegate.Combine(actions5.SniperMode, new Action<bool>(base.Operations.SendIsInSniperMode));
		PlayerActions actions6 = GameState.Current.PlayerData.Actions;
		actions6.AutomaticFire = (Action<bool>)Delegate.Combine(actions6.AutomaticFire, new Action<bool>(base.Operations.SendIsFiring));
		PlayerActions actions7 = GameState.Current.PlayerData.Actions;
		actions7.SetReadyForNextGame = (Action<bool>)Delegate.Combine(actions7.SetReadyForNextGame, new Action<bool>(base.Operations.SendIsReadyForNextMatch));
		TabScreenPanelGUI.SortPlayersByRank = GameStateHelper.SortDeathMatchPlayers;
		Singleton<QuickItemController>.Instance.IsConsumptionEnabled = true;
		Singleton<QuickItemController>.Instance.Restriction.IsEnabled = true;
		Singleton<QuickItemController>.Instance.Restriction.RenewGameUses();
		AutoMonoBehaviour<UnityRuntime>.Instance.OnUpdate += OnUpdate;
	}

	public void Dispose()
	{
		if (!isDisposed)
		{
			isDisposed = true;
			AutoMonoBehaviour<UnityRuntime>.Instance.OnUpdate -= OnUpdate;
			GameState.Current.PlayerData.SendJumpUpdate -= base.Operations.SendJump;
			GameState.Current.PlayerData.SendMovementUpdate -= base.Operations.SendUpdatePositionAndRotation;
			GameStateHelper.ExitGameMode();
		}
	}

	private void OnUpdate()
	{
		Singleton<QuickItemController>.Instance.Update();
	}

	protected override void OnPlayerJoinedGame(GameActorInfo player, PlayerMovement position)
	{
		Debug.Log("OnPlayerJoinedGame " + player.PlayerName + " " + GameState.Current.MatchState.CurrentStateId.ToString());
		GameState.Current.Players[player.Cmid] = player;
		if (player.Cmid == PlayerDataManager.Cmid)
		{
			GameState.Current.PlayerData.Player = player;
			GameState.Current.PlayerData.Team.Value = player.TeamID;
		}
		else
		{
			GameState.Current.RemotePlayerStates.AddCharacterInfo(player, position);
		}
		if (GameState.Current.MatchState.CurrentStateId != 0 && !player.IsSpectator)
		{
			GameState.Current.InstantiateAvatar(player);
		}
	}

	protected override void OnJoinGameFailed(string message)
	{
		Debug.LogError("OnJoinGameFailed " + message);
		PopupSystem.ClearAll();
		PopupSystem.ShowMessage("Join Game failed", message, PopupSystem.AlertType.OK, delegate
		{
			GameState.Current.MatchState.SetState(GameStateId.PregameLoadout);
		});
	}

	protected override void OnJoinedAsSpectator()
	{
		GameState.Current.PlayerData.Set(PlayerStates.Spectator, on: true);
	}

	protected override void OnPlayerLeftGame(int cmid)
	{
		GameState.Current.PlayerLeftGame(cmid);
	}

	protected override void OnPrepareNextRound()
	{
		GameState.Current.MatchState.SetState(GameStateId.PrepareNextRound);
		if (GameState.Current.Players.ContainsKey(PlayerDataManager.Cmid))
		{
			GameState.Current.PlayerState.SetState(PlayerStateId.PrepareForMatch);
		}
		else
		{
			GameState.Current.PlayerState.SetState(PlayerStateId.Spectating);
		}
	}

	protected override void OnWaitingForPlayers()
	{
		GameState.Current.MatchState.SetState(GameStateId.WaitingForPlayers);
		if (GameState.Current.Players.ContainsKey(PlayerDataManager.Cmid))
		{
			GameState.Current.PlayerState.SetState(PlayerStateId.Playing);
		}
		else
		{
			GameState.Current.PlayerState.SetState(PlayerStateId.Spectating);
		}
	}

	protected override void OnMatchStartCountdown(byte countdown)
	{
		EventHandler.Global.Fire(new GameEvents.MatchCountdown
		{
			Countdown = countdown
		});
	}

	protected override void OnMatchStart(int roundNumber, int endTime)
	{
		GameState.Current.StartMatch(roundNumber, endTime);
		GameState.Current.MatchState.SetState(GameStateId.MatchRunning);
	}

	protected override void OnMatchEnd(EndOfMatchData data)
	{
		GameState.Current.Statistics.Update(data);
		GameState.Current.MatchState.SetState(GameStateId.EndOfMatch);
		GameState.Current.UpdatePlayersReady();
	}

	protected override void OnTeamWins(TeamID team)
	{
		GameData.Instance.OnHUDChatClear.Fire();
		GameData.Instance.OnHUDStreamClear.Fire();
		GameState.Current.MatchState.SetState(GameStateId.AfterRound);
		switch (GameState.Current.LeadStatus.CurrentLead)
		{
		case PlayerLeadAudio.LeadState.Me:
			GameData.Instance.OnNotification.Fire("YOU WIN");
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.YouWin, 1000uL);
			break;
		case PlayerLeadAudio.LeadState.Others:
			GameData.Instance.OnNotification.Fire("Game Over");
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.GameOver, 1000uL);
			break;
		default:
			GameData.Instance.OnNotification.Fire("Draw");
			AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.Draw, 1000uL);
			break;
		}
	}

	protected override void OnAllPlayers(List<GameActorInfo> allPlayers, List<PlayerMovement> allPositions, ushort gameFrame)
	{
		for (int i = 0; i < allPlayers.Count && i < allPositions.Count; i++)
		{
			OnPlayerJoinedGame(allPlayers[i], allPositions[i]);
		}
	}

	protected override void OnAllPlayerDeltas(List<GameActorInfoDelta> players)
	{
		GameState.Current.AllPlayerDeltas(players);
	}

	protected override void OnAllPlayerPositions(List<PlayerMovement> allPositions, ushort gameFrame)
	{
		GameState.Current.AllPositionUpdate(allPositions, gameFrame);
	}

	protected override void OnPlayerDelta(GameActorInfoDelta delta)
	{
		GameState.Current.PlayerDelta(delta);
	}

	protected override void OnPlayerJumped(int cmid, Vector3 position)
	{
		if (GameState.Current.TryGetPlayerAvatar(cmid, out CharacterConfig character))
		{
			character.OnJump();
		}
	}

	protected override void OnPlayersReadyUpdated()
	{
		GameState.Current.UpdatePlayersReady();
	}

	protected override void OnPlayerRespawned(int cmid, Vector3 position, byte rotation)
	{
		GameState.Current.PlayerRespawned(cmid, position, rotation);
	}

	protected override void OnPlayerRespawnCountdown(byte countdown)
	{
		GameState.Current.PlayerState.Events.Fire(new GameEvents.RespawnCountdown
		{
			Countdown = countdown
		});
	}

	protected override void OnPlayerKilled(int shooter, int target, byte weaponClass, ushort damage, byte bodyPart, Vector3 direction)
	{
		GameState.Current.PlayerKilled(shooter, target, (UberstrikeItemClass)weaponClass, (BodyPart)bodyPart, direction);
	}

	protected override void OnDamageEvent(DamageEvent damageEvent)
	{
		GameState.Current.PlayerDamaged(damageEvent);
	}

	protected override void OnDoorOpen(int id)
	{
		EventHandler.Global.Fire(new GameEvents.DoorOpened(id));
	}

	protected override void OnResetAllPowerups()
	{
		EventHandler.Global.Fire(new GameEvents.PickupItemReset());
	}

	protected override void OnPowerUpPicked(int id, byte flag)
	{
		EventHandler.Global.Fire(new GameEvents.PickupItemChanged(id, (flag == 0) ? true : false));
	}

	protected override void OnSetPowerupState(List<int> states)
	{
		int num = 0;
		while (states != null && num < states.Count)
		{
			EventHandler.Global.Fire(new GameEvents.PickupItemChanged(states[num], enable: false));
			num++;
		}
	}

	protected override void OnEmitProjectile(int cmid, Vector3 origin, Vector3 direction, byte slot, int projectileID, bool explode)
	{
		GameState.Current.EmitRemoteProjectile(cmid, origin, direction, slot, projectileID, explode);
	}

	protected override void OnRemoveProjectile(int projectileId, bool explode)
	{
		Singleton<ProjectileManager>.Instance.RemoveProjectile(projectileId, explode);
	}

	protected override void OnEmitQuickItem(Vector3 origin, Vector3 direction, int itemId, byte playerNumber, int projectileID)
	{
		GameState.Current.EmitRemoteQuickItem(origin, direction, itemId, playerNumber, projectileID);
	}

	protected override void OnKickPlayer(string message)
	{
		Singleton<GameStateController>.Instance.LeaveGame();
		PopupSystem.ShowMessage("Cheat Detection", message, PopupSystem.AlertType.OK, delegate
		{
		});
	}

	protected override void OnPlayerHit(Vector3 force)
	{
		GameState.Current.Player.MoveController.ApplyForce(force, CharacterMoveController.ForceType.Additive);
	}

	protected override void OnQuickItemEvent(int cmid, byte eventType, int robotLifeTime, int scrapsLifeTime, bool isInstant)
	{
		GameState.Current.QuickItemEvent(cmid, eventType, robotLifeTime, scrapsLifeTime, isInstant);
	}

	protected override void OnSingleBulletFire(int cmid)
	{
		GameState.Current.SingleBulletFire(cmid);
	}

	protected override void OnActivateQuickItem(int cmid, QuickItemLogic logic, int robotLifeTime, int scrapsLifeTime, bool isInstant)
	{
		GameState.Current.ActivateQuickItem(cmid, logic, robotLifeTime, scrapsLifeTime, isInstant);
	}

	protected override void OnPlayerChangedTeam(int cmid, TeamID team)
	{
	}

	protected override void OnUpdateRoundScore(int round, short blue, short red)
	{
	}

	protected override void OnKillsRemaining(int killsRemaining, int leaderCmid)
	{
		int num = 0;
		foreach (GameActorInfo value in GameState.Current.Players.Values)
		{
			if (value.Cmid != PlayerDataManager.Cmid && num < value.Kills)
			{
				num = value.Kills;
			}
		}
		GameState.Current.PlayerData.RemainingKills.Value = killsRemaining;
		GameState.Current.LeadStatus.PlayKillsLeftAudio(killsRemaining);
		GameState.Current.LeadStatus.UpdateLeadStatus(GameState.Current.PlayerData.Player.Kills, num, killsRemaining > 0 && GameState.Current.MatchState.CurrentStateId == GameStateId.MatchRunning);
	}

	protected override void OnLevelUp(int newLevel)
	{
		Debug.LogError("TODO: trigger level up in the future");
	}

	protected override void OnChatMessage(int cmid, string name, string message, MemberAccessLevel accessLevel, byte context)
	{
		GameStateHelper.OnChatMessage(cmid, name, message, accessLevel, context);
	}

	protected override void OnDisconnectCountdown(byte countdown)
	{
		GameData.Instance.OnWarningNotification.Fire(LocalizedStrings.DisconnectionIn + " " + countdown.ToString());
	}
}
