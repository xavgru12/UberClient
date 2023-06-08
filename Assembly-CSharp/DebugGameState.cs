using UnityEngine;

public class DebugGameState : IDebugPage
{
	private Vector2 v1;

	public string Title => "States";

	public void Draw()
	{
		if (GameState.Current != null)
		{
			v1 = GUILayout.BeginScrollView(v1);
			GUILayout.Label("Mode:" + GameState.Current.RoomData.GameMode.ToString() + "/" + Singleton<GameStateController>.Instance.CurrentGameMode.ToString());
			GUILayout.Label("MatchState:" + GameState.Current.MatchState.CurrentStateId.ToString());
			GUILayout.Label("PlayerState:" + GameState.Current.PlayerState.CurrentStateId.ToString());
			if (GameState.Current.RoomData.Server != null)
			{
				GUILayout.Label("Server:" + GameState.Current.RoomData.Server?.ToString() + "/" + GameState.Current.RoomData.Number.ToString());
			}
			GUILayout.Label("IsSpectator:" + GameState.Current.PlayerData.IsSpectator.ToString());
			GUILayout.Label("HasJoinedGame:" + GameState.Current.HasJoinedGame.ToString());
			GUILayout.Label("IsMatchRunning:" + GameState.Current.IsMatchRunning.ToString());
			GUILayout.Label("CameraMode:" + LevelCamera.CurrentMode.ToString());
			GUILayout.Space(10f);
			GUILayout.Label("IsInputEnabled:" + AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled.ToString());
			GUILayout.Label("lockCursor:" + Screen.lockCursor.ToString());
			Vector2 mouse = UserInputt.Mouse;
			GUILayout.Label("Mouse:" + mouse.ToString() + " " + UserInputt.Rotation.ToString());
			GUILayout.Label("KeyState:" + GameState.Current.PlayerData.KeyState.ToString());
			GUILayout.Label("MovementState:" + GameState.Current.PlayerData.MovementState.ToString());
			GUILayout.Label("IsWalkingEnabled:" + GameState.Current.Player.IsWalkingEnabled.ToString());
			GUILayout.Label("WeaponCamera:" + GameState.Current.Player.WeaponCamera.IsEnabled.ToString());
			GUILayout.Label("Weapons:" + GameState.Current.Player.EnableWeaponControl.ToString());
			GUILayout.Space(10f);
			GUILayout.Label("GameTime:" + GameState.Current.GameTime.ToString("N2"));
			GUILayout.Label("Latency:" + Singleton<GameStateController>.Instance.Client.Peer.RoundTripTime.ToString("N0"));
			GUILayout.EndScrollView();
		}
	}
}
