// Decompiled with JetBrains decompiler
// Type: GameModeUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal static class GameModeUtil
{
  private const int DisconnectionTimeout = 120;
  private const int DisconnectionTimeoutAdmin = 1200;

  public static void OnEnterGameMode(FpsGameMode gameMode)
  {
    GameState.CurrentGame = gameMode;
    TabScreenPanelGUI.Instance.SetGameName(gameMode.GameData.RoomName);
    TabScreenPanelGUI.Instance.SetServerName(Singleton<GameServerManager>.Instance.GetServerName(gameMode.GameData.ServerConnection));
    LevelCamera.Instance.SetLevelCamera(GameState.CurrentSpace.Camera, GameState.CurrentSpace.DefaultViewPoint.position, GameState.CurrentSpace.DefaultViewPoint.rotation);
    GameState.LocalPlayer.SetPlayerControlState(LocalPlayer.PlayerState.None);
    GameState.LocalPlayer.SetEnabled(true);
    ProjectileManager.CreateContainer();
  }

  public static void OnExitGameMode()
  {
    GameConnectionManager.Stop();
    LevelCamera.Instance.ReleaseCamera();
    Singleton<WeaponController>.Instance.StopInputHandler();
    Singleton<ProjectileManager>.Instance.ClearAll();
    ProjectileManager.DestroyContainer();
    GameState.CurrentGame.UnloadAllPlayers();
    GameState.CurrentGame = (FpsGameMode) null;
    GameState.LocalPlayer.SetEnabled(false);
    Singleton<HudUtil>.Instance.ClearAllHud();
    GameState.LocalPlayer.SetPlayerControlState(LocalPlayer.PlayerState.None);
    if ((bool) (Object) GameState.LocalDecorator)
    {
      if (!GameState.LocalDecorator.gameObject.activeSelf)
        GameState.LocalDecorator.DisableRagdoll();
      GameState.LocalDecorator.MeshRenderer.enabled = true;
      GameState.LocalDecorator.HudInformation.enabled = true;
    }
    Singleton<QuickItemController>.Instance.Clear();
  }

  public static void OnPlayerDamage(OnPlayerDamageEvent ev)
  {
    Singleton<DamageFeedbackHud>.Instance.AddDamageMark(Mathf.Clamp01(ev.DamageValue / 50f), ev.Angle);
    if (ApplicationDataManager.ApplicationOptions.VideoBloomHitEffect)
      RenderSettingsController.Instance.ShowAgonyTint(ev.DamageValue / 50f);
    if (GameState.LocalCharacter.Armor.ArmorPoints > 0)
      SfxManager.Play2dAudioClip(GameAudio.LocalPlayerHitArmorRemaining);
    else
      SfxManager.Play2dAudioClip(GameAudio.LocalPlayerHitNoArmor);
  }

  public static void OnPlayerKillEnemy(OnPlayerKillEnemyEvent ev)
  {
    InGameEventFeedbackType eventFeedbackType = InGameEventFeedbackType.None;
    if (ev.WeaponCategory == UberstrikeItemClass.WeaponMelee)
    {
      eventFeedbackType = InGameEventFeedbackType.Humiliation;
      Singleton<LocalShotFeedbackHud>.Instance.DisplayLocalShotFeedback(eventFeedbackType);
      SfxManager.Play2dAudioClip(GameAudio.KilledBySplatbat);
    }
    else if (ev.BodyHitPart == BodyPart.Head)
    {
      eventFeedbackType = InGameEventFeedbackType.HeadShot;
      Singleton<LocalShotFeedbackHud>.Instance.DisplayLocalShotFeedback(eventFeedbackType);
      SfxManager.Play2dAudioClip(GameAudio.GotHeadshotKill);
    }
    else if (ev.BodyHitPart == BodyPart.Nuts)
    {
      eventFeedbackType = InGameEventFeedbackType.NutShot;
      Singleton<LocalShotFeedbackHud>.Instance.DisplayLocalShotFeedback(eventFeedbackType);
      SfxManager.Play2dAudioClip(GameAudio.GotNutshotKill);
    }
    else
      Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Format(LocalizedStrings.YouKilledN, (object) ev.EmemyInfo.PlayerName));
    Singleton<HudUtil>.Instance.AddInGameEvent(GameState.LocalCharacter.PlayerName, ev.EmemyInfo.PlayerName, ev.WeaponCategory, eventFeedbackType, GameState.LocalCharacter.TeamID, ev.EmemyInfo.TeamID);
  }

  public static void OnPlayerKilled(OnPlayerKilledEvent ev)
  {
    InGameEventFeedbackType eventType = InGameEventFeedbackType.None;
    if (ev.WeaponCategory == UberstrikeItemClass.WeaponMelee)
    {
      eventType = InGameEventFeedbackType.Humiliation;
      Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Format(LocalizedStrings.SmackdownFromN, (object) ev.ShooterInfo.PlayerName));
      if (LevelCamera.Instance.IsZoomedIn)
        LevelCamera.Instance.DoZoomOut(60f, 10f);
    }
    else if (ev.BodyHitPart == BodyPart.Head)
    {
      eventType = InGameEventFeedbackType.HeadShot;
      Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Format(LocalizedStrings.HeadshotFromN, (object) ev.ShooterInfo.PlayerName), 6f);
    }
    else if (ev.BodyHitPart == BodyPart.Nuts)
    {
      eventType = InGameEventFeedbackType.NutShot;
      Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Format(LocalizedStrings.NutshotFromN, (object) ev.ShooterInfo.PlayerName), 6f);
    }
    else
      Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, string.Format(LocalizedStrings.KilledByN, (object) ev.ShooterInfo.PlayerName), 6f);
    Singleton<HudUtil>.Instance.AddInGameEvent(ev.ShooterInfo.PlayerName, GameState.LocalCharacter.PlayerName, ev.WeaponCategory, eventType, ev.ShooterInfo.TeamID, GameState.LocalCharacter.TeamID);
  }

  public static void OnPlayerSuicide(OnPlayerSuicideEvent ev)
  {
    if (ev.PlayerInfo.ActorId == GameState.CurrentPlayerID)
      Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, LocalizedStrings.CongratulationsYouKilledYourself);
    Singleton<HudUtil>.Instance.AddInGameEvent(ev.PlayerInfo.PlayerName, LocalizedStrings.NKilledThemself, UberstrikeItemClass.FunctionalGeneral, InGameEventFeedbackType.CustomMessage, ev.PlayerInfo.TeamID, TeamID.NONE);
  }

  public static void UpdatePlayerStateMsg(FpsGameMode gameMode, bool checkTimeout)
  {
    if (gameMode.IsWaitingForSpawn)
      GameModeUtil.UpdateWaitingForSpawnMsg(gameMode, checkTimeout);
    else if (gameMode.IsWaitingForPlayers)
      Singleton<PlayerStateMsgHud>.Instance.DisplayWaitingForOtherPlayerMsg();
    else
      Singleton<PlayerStateMsgHud>.Instance.DisplayNone();
  }

  public static void UpdateWaitingForSpawnMsg(FpsGameMode gameMode, bool checkTimeout)
  {
    int spawnFrozenTime = Mathf.CeilToInt(gameMode.NextSpawnTime - Time.time);
    if (spawnFrozenTime > 0)
    {
      Singleton<HudUtil>.Instance.ShowRespawnFrozenTimeText(spawnFrozenTime);
    }
    else
    {
      int timeout = Mathf.CeilToInt((float) ((double) gameMode.NextSpawnTime - (double) Time.time + (PlayerDataManager.AccessLevel != MemberAccessLevel.Default ? 1200.0 : 120.0)));
      if (!checkTimeout || timeout > 0)
      {
        Singleton<HudUtil>.Instance.ShowRespawnButton();
        if (!checkTimeout || timeout >= 10)
          return;
        Singleton<HudUtil>.Instance.ShowTimeOutText(gameMode, timeout);
      }
      else
      {
        if (!checkTimeout || gameMode.IsGameClosed)
          return;
        gameMode.IsGameClosed = true;
        if ((bool) (Object) GameState.LocalDecorator)
          GameState.LocalDecorator.DisableRagdoll();
        Singleton<GameStateController>.Instance.LeaveGame();
        PopupSystem.ShowMessage("Wake up!", "It looks like you were asleep.The server disconnected you because you were idle for more than one minute while waiting to respawn.");
      }
    }
  }
}
