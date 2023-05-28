// Decompiled with JetBrains decompiler
// Type: DebugGameState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DebugGameState : IDebugPage
{
  private Vector2 v1;

  public string Title => "Game";

  public void Draw()
  {
    if (GameState.CurrentGame == null)
      return;
    this.v1 = GUILayout.BeginScrollView(this.v1);
    GUILayout.Label("Type:" + ((object) GameState.CurrentGame).GetType().ToString());
    GUILayout.Label("Room:" + GameState.CurrentGame.GameData.RoomID.ToString());
    GUILayout.Label("IsGameStarted:" + GameState.CurrentGame.IsGameStarted.ToString());
    GUILayout.Label("IsRoundRunning:" + GameState.CurrentGame.IsMatchRunning.ToString());
    GUILayout.Label("GameTime:" + GameState.CurrentGame.GameTime.ToString("N2"));
    GUILayout.Label("Latency:" + GameConnectionManager.Client.Latency.ToString("N0"));
    GUILayout.Label("Ping:" + GameConnectionManager.Client.PeerListener.Ping.ToString("N0"));
    GUILayout.Label("CameraState:" + GameState.LocalPlayer.CurrentCameraControl.ToString());
    GUILayout.Label("IsHudEnabled:" + HudController.Instance.enabled.ToString());
    GUILayout.Label("HudDrawFlags:" + HudController.Instance.DrawFlagString);
    GUILayout.Label("IsGamePaused:" + GameState.LocalPlayer.IsGamePaused.ToString());
    GUILayout.Label("IsInputEnabled:" + AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled.ToString());
    GUILayout.Label("PlayerSpectator:" + Singleton<PlayerSpectatorControl>.Instance.IsEnabled.ToString());
    GUILayout.Label("MyPlayerID:" + GameState.CurrentGame.MyActorId.ToString());
    GUILayout.Label("lockCursor:" + Screen.lockCursor.ToString());
    GUILayout.Label("IsMouseLockStateConsistent: " + GameState.LocalPlayer.IsMouseLockStateConsistent.ToString());
    GUILayout.Label("IsShootingEnabled: " + GameState.LocalPlayer.IsShootingEnabled.ToString());
    GUILayout.Label("IsWalkingEnabled: " + GameState.LocalPlayer.IsWalkingEnabled.ToString());
    GUILayout.Label("IsWeaponControlEnabled: " + (object) Singleton<WeaponController>.Instance.IsEnabled);
    GUILayout.Label("Players: " + (!GameState.HasCurrentGame ? 0 : GameState.CurrentGame.Players.Count).ToString());
    GUILayout.EndScrollView();
  }
}
