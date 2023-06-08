using UberStrike.Core.Types;
using UnityEngine;

internal class WaitingForPlayersState : IState
{
	public WaitingForPlayersState(StateMachine<GameStateId> stateMachine)
	{
	}

	public void OnEnter()
	{
		GamePageManager.Instance.UnloadCurrentPage();
		if (GameState.Current.Players.ContainsKey(PlayerDataManager.Cmid))
		{
			GameStateHelper.RespawnLocalPlayerAtRandom();
			GameState.Current.PlayerState.SetState(PlayerStateId.Playing);
		}
		else
		{
			GameState.Current.PlayerState.SetState(PlayerStateId.Spectating);
		}
		EventHandler.Global.Fire(new GameEvents.PlayerIngame());
		EventHandler.Global.AddListener<GameEvents.PlayerDied>(OnPlayerKilled);
		EventHandler.Global.AddListener<GameEvents.PlayerRespawn>(OnPlayerRespawn);
	}

	public void OnResume()
	{
	}

	public void OnExit()
	{
		Singleton<QuickItemController>.Instance.IsEnabled = true;
		GamePageManager.Instance.UnloadCurrentPage();
		EventHandler.Global.RemoveListener<GameEvents.PlayerDied>(OnPlayerKilled);
		EventHandler.Global.RemoveListener<GameEvents.PlayerRespawn>(OnPlayerRespawn);
	}

	public void OnUpdate()
	{
		Singleton<QuickItemController>.Instance.IsEnabled = false;
		string v = string.Empty;
		if (GameState.Current.GameMode == GameModeType.DeathMatch)
		{
			v = "Get as many kills as you can before the time runs out";
		}
		else if (GameState.Current.GameMode == GameModeType.EliminationMode)
		{
			v = "Eliminate all players on the enemy team.\nThe team with players standing at the end of the round wins.";
		}
		else if (GameState.Current.GameMode == GameModeType.TeamDeathMatch)
		{
			v = "Get as many kills for your team as you can\nbefore the time runs out";
		}
		GameData.Instance.OnNotificationFull.Fire(LocalizedStrings.WaitingForOtherPlayers, v, 0f);
	}

	private void OnPlayerRespawn(GameEvents.PlayerRespawn ev)
	{
		GameState.Current.RespawnLocalPlayerAt(ev.Position, Quaternion.Euler(0f, ev.Rotation, 0f));
		GameState.Current.PlayerState.SetState(PlayerStateId.Playing);
	}

	private void OnPlayerKilled(GameEvents.PlayerDied ev)
	{
		GameState.Current.PlayerState.SetState(PlayerStateId.Killed);
	}
}
