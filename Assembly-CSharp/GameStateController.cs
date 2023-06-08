using System;
using System.Collections;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class GameStateController : Singleton<GameStateController>
{
	private IGameMode currentGameMode;

	public GameMode CurrentGameMode
	{
		get
		{
			if (currentGameMode == null)
			{
				return GameMode.None;
			}
			return currentGameMode.Type;
		}
	}

	public GameRoomData GameRoom;

	public GamePeer Client
	{
		get;
		private set;
	}

	private GameStateController()
	{
		Client = new GamePeer();
	}

	public void CreateNetworkGame(string server, int mapId, GameModeType mode, GameBoxType boxType, string name, string password, int timeMinutes, int killLimit, int playerLimit, int minLevel, int maxLevel, GameFlags.GAME_FLAGS flags)
	{
		GameRoomData gameRoomData = new GameRoomData();
		gameRoomData.Name = name;
		gameRoomData.Server = new ConnectionAddress(server);
		gameRoomData.MapID = mapId;
		gameRoomData.TimeLimit = timeMinutes;
		gameRoomData.PlayerLimit = playerLimit;
		gameRoomData.GameMode = mode;
		gameRoomData.GameFlags = (int)flags;
		gameRoomData.KillLimit = killLimit;
		gameRoomData.LevelMin = (byte)Mathf.Clamp(minLevel, 0, 255);
		gameRoomData.LevelMax = (byte)Mathf.Clamp(maxLevel, 0, 255);
		gameRoomData.BoxType = boxType;
		GameRoomData data = gameRoomData;
		UnityRuntime.StartRoutine(BeginGameCreation(data, password));
	}

	private IEnumerator BeginGameCreation(GameRoomData data,string password)
	{
		var popup = new ProgressPopupDialog("Authentication", "Connecting to Server & Syncing Loadout");
		PopupSystem.Show(popup);
		yield return AutoMonoBehaviour<UnityRuntime>.Instance.StartCoroutine(Singleton<PlayerDataManager>.Instance.StartSetLoadout());
		Client.CreateGame(data, password);
	}

	private IEnumerator BeginJoinGame(GameRoomData data)
	{
		ProgressPopupDialog dialog = PopupSystem.ShowProgress("Authentication", "Connecting to Server & Syncing Loadout");
		PopupSystem.Show(dialog);
		yield return AutoMonoBehaviour<UnityRuntime>.Instance.StartCoroutine(Singleton<PlayerDataManager>.Instance.StartSetLoadout());
		Singleton<ChatManager>.Instance.InGameDialog.Clear();
		Client.JoinGame(data.Server.ConnectionString, data.Number, string.Empty);
	}

	public void JoinNetworkGame(GameRoomData data)
	{
		if (data.Server != null)
		{
			UnityRuntime.StartRoutine(BeginJoinGame(data));
		}
		else
		{
			PopupSystem.ShowError("Game not found", "The game doesn't exist anymore.", PopupSystem.AlertType.OK);
		}
	}

	private IEnumerator BeginJoinGame(GameRoom data)
	{
		yield return AutoMonoBehaviour<UnityRuntime>.Instance.StartCoroutine(Singleton<PlayerDataManager>.Instance.StartSetLoadout());
		Singleton<ChatManager>.Instance.InGameDialog.Clear();
		Client.JoinGame(data.Server.ConnectionString, data.Number, string.Empty);
	}

	public void JoinNetworkGame(GameRoom data)
	{
		if (data.Server != null)
		{
			UnityRuntime.StartRoutine(BeginJoinGame(data));
		}
		else
		{
			PopupSystem.ShowError("Game not found", "The game doesn't exist anymore.", PopupSystem.AlertType.OK);
		}
	}

	public Action Rejoin;

	public void LeaveGame(bool warnBeforeLeaving = false)
	{
		if (warnBeforeLeaving && GameState.Current.IsMultiplayer && GameState.Current.IsMatchRunning)
		{
			PopupSystem.ShowMessage(LocalizedStrings.LeavingGame, LocalizedStrings.LeaveGameWarningMsg, PopupSystem.AlertType.OKCancel, BackToMenu, LocalizedStrings.LeaveCaps, null, LocalizedStrings.CancelCaps, PopupSystem.ActionType.Negative);
		}
		else if(Rejoin != null)
		{
			Rejoin();
		}
		else
		{
			BackToMenu();
		}
	}

	public void ResetClient()
	{
		Client.Dispose();
		Client = new GamePeer();
	}

	private void BackToMenu()
	{
		GamePageManager.Instance.UnloadCurrentPage();
		UnloadGameMode();
		if (Singleton<SceneLoader>.Instance.CurrentScene != "Menu")
		{
			Singleton<SceneLoader>.Instance.LoadLevel("Menu");
		}
	}

	public void UnloadGameMode()
	{
		SetGameMode(null);
	}

	public void SetGameMode(IGameMode mode)
	{
		if (currentGameMode != null)
		{
			Client.LeaveGame();
			currentGameMode.Dispose();
		}
		currentGameMode = mode;
	}
}
