// Decompiled with JetBrains decompiler
// Type: FpsGameMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public abstract class FpsGameMode : ClientGameMode
{
  protected bool _hasGameStarted;
  protected bool _isLocalAvatarLoaded;
  protected bool _isGameClosed;
  protected int _nextSpawnPoint;
  protected int _nextSpawnCountdown;
  private int _playerReadyForNextRound;
  protected LocalCharacterState _localStateSender;
  protected GameStateInterpolator _stateInterpolator;
  protected Dictionary<int, CharacterConfig> _characterByActorId;
  protected int _roundStartTime;
  private int[] _previousLoadoutWeaponIds;
  protected Dictionary<GameFlags.GAME_FLAGS, int[]> _singleWeaponSettings;
  private ShotCountSender _shotCountSender;

  protected FpsGameMode(RemoteMethodInterface rmi, GameMetaData gameData)
    : base(rmi, gameData)
  {
    this._singleWeaponSettings = new Dictionary<GameFlags.GAME_FLAGS, int[]>(4);
    this._singleWeaponSettings.Add(GameFlags.GAME_FLAGS.CannonArena, new int[4]
    {
      0,
      1020,
      0,
      0
    });
    this._singleWeaponSettings.Add(GameFlags.GAME_FLAGS.Instakill, new int[4]
    {
      0,
      1147,
      0,
      0
    });
    this._singleWeaponSettings.Add(GameFlags.GAME_FLAGS.NinjaArena, new int[4]
    {
      1136,
      0,
      0,
      0
    });
    this._singleWeaponSettings.Add(GameFlags.GAME_FLAGS.SniperArena, new int[4]
    {
      0,
      1018,
      0,
      0
    });
    this.ApplyGameFlags();
    this._characterByActorId = new Dictionary<int, CharacterConfig>(16);
    this._stateInterpolator = new GameStateInterpolator();
    this._localStateSender = new LocalCharacterState(GameState.LocalCharacter, this);
    this._shotCountSender = new ShotCountSender(this);
  }

  public void FixedUpdate()
  {
    if (!this.IsGameStarted || !this.HasJoinedGame)
      return;
    this._localStateSender.SendUpdates();
  }

  public void Update()
  {
    if (!this.IsGameStarted)
      return;
    this._stateInterpolator.Interpolate();
    if ((double) this.GameData.RoundTime - (double) this.GameTime > 10.0)
      this._shotCountSender.UpdateEveryTenSeconds();
    else
      this._shotCountSender.UpdateEverySecond();
  }

  public static bool IsSingleWeapon(GameMetaData data) => GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.NinjaArena, data.GameModifierFlags) || GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.CannonArena, data.GameModifierFlags) || GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.SniperArena, data.GameModifierFlags) || GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.Instakill, data.GameModifierFlags);

  public static string GetGameFlagText(GameMetaData data)
  {
    string gameFlagText = string.Empty;
    if (GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.CannonArena, data.GameModifierFlags))
      gameFlagText = LocalizedStrings.CannonArena;
    else if (GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.Instakill, data.GameModifierFlags))
      gameFlagText = LocalizedStrings.Instakill;
    else if (GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.LowGravity, data.GameModifierFlags))
      gameFlagText = LocalizedStrings.LowGravity;
    else if (GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.NinjaArena, data.GameModifierFlags))
      gameFlagText = LocalizedStrings.NinjaArena;
    else if (GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.SniperArena, data.GameModifierFlags))
      gameFlagText = LocalizedStrings.SniperArena;
    return gameFlagText;
  }

  public virtual void RespawnPlayer()
  {
    GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Playing);
    this.IsWaitingForSpawn = false;
    Vector3 position = Vector3.zero;
    Quaternion rotation = Quaternion.identity;
    Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(this._nextSpawnPoint, (GameMode) this.GameData.GameMode, TeamID.NONE, out position, out rotation);
    this.SpawnPlayerAt(position, rotation);
  }

  protected override void OnInitialized()
  {
    base.OnInitialized();
    if (this.GameData != null)
      this.SendMethodToServer((byte) 70, (object) (byte) Singleton<SpawnPointManager>.Instance.GetSpawnPointCount((GameMode) this.GameData.GameMode, TeamID.NONE), (object) (byte) Singleton<SpawnPointManager>.Instance.GetSpawnPointCount((GameMode) this.GameData.GameMode, TeamID.RED), (object) (byte) Singleton<SpawnPointManager>.Instance.GetSpawnPointCount((GameMode) this.GameData.GameMode, TeamID.BLUE));
    if (!this._hasGameStarted)
      return;
    this.InitializeMode(GameState.LocalCharacter.TeamID, GameState.LocalCharacter.IsSpectator);
  }

  public void InitializeMode(TeamID team = TeamID.NONE, bool isSpectator = false)
  {
    this._hasGameStarted = true;
    GameState.LocalCharacter.ResetState();
    GameState.LocalCharacter.ActorId = this._rmi.Messenger.PeerListener.ActorIdSecure;
    GameState.LocalCharacter.CurrentRoom = this._rmi.Messenger.PeerListener.CurrentRoom;
    GameState.LocalCharacter.Channel = ApplicationDataManager.Channel;
    GameState.LocalCharacter.TeamID = team;
    GameState.LocalCharacter.Cmid = PlayerDataManager.CmidSecure;
    GameState.LocalCharacter.PlayerName = !PlayerDataManager.IsPlayerInClan ? PlayerDataManager.NameSecure : string.Format("[{0}] {1}", (object) PlayerDataManager.ClanTag, (object) PlayerDataManager.NameSecure);
    GameState.LocalCharacter.ClanTag = PlayerDataManager.GroupTag;
    GameState.LocalCharacter.Level = PlayerDataManager.PlayerLevelSecure;
    if (isSpectator)
    {
      GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Spectating);
      GameState.LocalCharacter.Set(PlayerStates.SPECTATOR);
    }
    else
    {
      if (Singleton<PlayerSpectatorControl>.Instance.IsEnabled)
        GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Spectating);
      else
        GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.AfterRound);
      GameState.LocalPlayer.UpdateLocalCharacterLoadout();
    }
    this.SendMethodToServer((byte) 1, (object) GameState.LocalCharacter);
    if (LevelCamera.HasCamera)
      LevelCamera.Instance.EnableLowPassFilter(false);
    this.OnModeInitialized();
    CmuneEventHandler.Route((object) new OnModeInitializedEvent());
  }

  protected virtual void OnModeInitialized()
  {
  }

  public void DebugAll()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendFormat("_avatars: {0}", (object) this._characterByActorId.Count);
    stringBuilder.AppendFormat("_coolDownTime: {0}", (object) this._nextSpawnCountdown);
    stringBuilder.AppendFormat("_instanceID: {0}", (object) this._instanceID);
    stringBuilder.AppendFormat("_lookupIndexMethod: {0}", (object) ((Dictionary<byte, MethodInfo>) this._lookupIndexMethod).Count);
    stringBuilder.AppendFormat("_lookupNameIndex: {0}", (object) ((Dictionary<string, byte>) this._lookupNameIndex).Count);
    stringBuilder.AppendFormat("_nextSpawnPoint: {0}", (object) this._nextSpawnPoint);
    stringBuilder.AppendFormat("IsGameStarted: {0}", (object) this.IsGameStarted);
    stringBuilder.AppendFormat("IsGlobal: {0}", (object) this.IsGlobal);
    stringBuilder.AppendFormat("IsInitialized: {0}", (object) this.IsInitialized);
    stringBuilder.AppendFormat("IsRoundRunning: {0}", (object) this.IsMatchRunning);
    stringBuilder.AppendFormat("NetworkID: {0}", (object) this.NetworkID);
    stringBuilder.AppendFormat("Players: {0}", (object) this.Players.Count);
    Debug.Log((object) stringBuilder.ToString());
  }

  protected override void OnUninitialized()
  {
    Singleton<ChatManager>.Instance.UpdateLastgamePlayers();
    this.SendMethodToServer((byte) 2, (object) this.MyActorId);
    foreach (int actorId in Conversion.ToArray<int>((ICollection<int>) this.Players.Keys))
      this.OnPlayerLeft(actorId);
    base.OnUninitialized();
  }

  protected override void Dispose(bool dispose)
  {
    Singleton<PlayerSpectatorControl>.Instance.IsEnabled = false;
    if (this._isDisposed)
      return;
    if (this._previousLoadoutWeaponIds != null)
      Singleton<LoadoutManager>.Instance.SetLoadoutWeapons(this._previousLoadoutWeaponIds);
    if (dispose)
    {
      this.IsMatchRunning = false;
      Singleton<InGameChatHud>.Instance.ClearHistory();
      PopupSystem.ClearAll();
      if ((bool) (UnityEngine.Object) GameState.LocalDecorator)
        GameState.LocalDecorator.MeshRenderer.enabled = true;
      this.UnloadAllPlayers();
    }
    base.Dispose(dispose);
  }

  private void ApplyGameFlags()
  {
    GameFlags.GAME_FLAGS gameModifierFlags = (GameFlags.GAME_FLAGS) this.GameData.GameModifierFlags;
    if (!this._singleWeaponSettings.ContainsKey(gameModifierFlags))
      return;
    this._previousLoadoutWeaponIds = Singleton<LoadoutManager>.Instance.SetLoadoutWeapons(this._singleWeaponSettings[gameModifierFlags]);
  }

  private void ConfigureCharacter(UberStrike.Realtime.UnitySdk.CharacterInfo info, CharacterConfig character, bool isLocal)
  {
    if ((UnityEngine.Object) character != (UnityEngine.Object) null && info != null)
    {
      if (isLocal)
      {
        GameState.LocalPlayer.SetCurrentCharacterConfig(character);
        this._localStateSender.Info.Position = GameState.LocalDecorator.transform.position + Vector3.up;
        this._localStateSender.Info.HorizontalRotation = GameState.LocalDecorator.transform.rotation;
        character.Initialize((ICharacterState) this._localStateSender, GameState.LocalDecorator);
        GameState.LocalPlayer.MoveController.IsLowGravity = GameFlags.IsFlagSet(GameFlags.GAME_FLAGS.LowGravity, this.GameData.GameModifierFlags);
      }
      else
      {
        AvatarDecorator remoteAvatar = Singleton<AvatarBuilder>.Instance.CreateRemoteAvatar(new GearLoadout(info.Gear), info.SkinColor);
        character.Initialize((ICharacterState) this._stateInterpolator.GetState(info.ActorId), remoteAvatar);
        if (info.ActorId > this.MyActorId)
          Singleton<HudUtil>.Instance.AddInGameEvent(info.PlayerName, LocalizedStrings.JoinedTheGame, UberstrikeItemClass.FunctionalGeneral, InGameEventFeedbackType.CustomMessage, info.TeamID, TeamID.NONE);
      }
      this.OnCharacterLoaded();
    }
    else
      Debug.LogError((object) string.Format("OnAvatarLoaded failed because loaded Avatar is {0} and Info is {1}", (object) ((UnityEngine.Object) character != (UnityEngine.Object) null), (object) (info != null)));
  }

  protected virtual void OnCharacterLoaded()
  {
  }

  protected override sealed void OnPlayerJoined(SyncObject data, Vector3 position)
  {
    base.OnPlayerJoined(data, position);
    Singleton<ChatManager>.Instance.SetGameSection(this.GameData.RoomID, (IEnumerable<UberStrike.Realtime.UnitySdk.CharacterInfo>) this.Players.Values);
    UberStrike.Realtime.UnitySdk.CharacterInfo info;
    if (this.Players.TryGetValue(data.Id, out info))
      this.OnNormalJoin(info);
    else
      GameState.LocalPlayer.UnPausePlayer();
  }

  protected virtual void OnNormalJoin(UberStrike.Realtime.UnitySdk.CharacterInfo info)
  {
    if (info.ActorId != this.MyActorId)
      this._stateInterpolator.AddCharacterInfo(info);
    else
      this.SendMethodToServer((byte) 62, (object) this.MyActorId, (object) PickupItem.GetRespawnDurations());
    this.InstantiateCharacter(info);
  }

  protected override void OnPlayerLeft(int actorId)
  {
    try
    {
      UberStrike.Realtime.UnitySdk.CharacterInfo playerWithId = this.GetPlayerWithID(actorId);
      if (playerWithId != null)
        Singleton<EventStreamHud>.Instance.AddEventText(playerWithId.PlayerName, playerWithId.TeamID, LocalizedStrings.LeftTheGame, string.Empty);
      CharacterConfig characterConfig;
      if (this._characterByActorId.TryGetValue(actorId, out characterConfig))
      {
        if ((bool) (UnityEngine.Object) characterConfig)
          characterConfig.Destroy();
        this._characterByActorId.Remove(actorId);
      }
      if (actorId == GameState.LocalCharacter.ActorId)
      {
        GameState.LocalCharacter.ResetState();
        GameState.LocalPlayer.SetCurrentCharacterConfig((CharacterConfig) null);
      }
      else
        this._stateInterpolator.RemoveCharacterInfo(actorId);
    }
    catch
    {
      Debug.LogError((object) string.Format("OnPlayerLeft with actorId={0}", (object) actorId));
      throw;
    }
    finally
    {
      base.OnPlayerLeft(actorId);
      Singleton<ChatManager>.Instance.SetGameSection(this.GameData.RoomID, (IEnumerable<UberStrike.Realtime.UnitySdk.CharacterInfo>) this.Players.Values);
    }
  }

  public void SendCharacterInfoUpdate()
  {
    if (!this.IsInitialized || GameState.LocalCharacter.ActorId <= 0 || GameState.LocalCharacter.IsSpectator)
      return;
    SyncObject syncData = SyncObjectBuilder.GetSyncData((CmuneDeltaSync) GameState.LocalCharacter, false);
    if (syncData.IsEmpty)
      return;
    this.SendMethodToServer((byte) 3, (object) syncData);
  }

  public void SendPositionUpdate()
  {
    if (!this.IsInitialized || GameState.LocalCharacter.PlayerNumber <= (byte) 0)
      return;
    List<byte> byteList = new List<byte>(14);
    DefaultByteConverter.FromInt(GameState.LocalCharacter.ActorId, ref byteList);
    ShortVector3.Bytes(byteList, GameState.LocalCharacter.Position);
    DefaultByteConverter.FromInt(GameConnectionManager.Client.PeerListener.ServerTimeTicks, ref byteList);
    this.SendUnreliableMethodToServer((byte) 83, (object) byteList);
  }

  [NetworkMethod(83)]
  protected void OnPositionsUpdate(List<byte> positions)
  {
    int num1 = 0;
    List<byte> byteList = positions;
    int index1 = num1;
    int num2 = index1 + 1;
    int capacity = (int) byteList[index1];
    byte[] array = positions.ToArray();
    List<PlayerPosition> all = new List<PlayerPosition>(capacity);
    for (int index2 = 0; index2 < capacity && num2 + 11 <= array.Length; ++index2)
    {
      byte id = array[num2++];
      int time = DefaultByteConverter.ToInt(array, ref num2);
      ShortVector3 p = new ShortVector3(array, ref num2);
      all.Add(new PlayerPosition(id, p, time));
    }
    this._stateInterpolator.UpdatePositionSmooth(all);
  }

  protected override void OnGameFrameUpdate(List<SyncObject> deltas)
  {
    try
    {
      bool flag = false;
      foreach (SyncObject delta in deltas)
      {
        if (!delta.IsEmpty)
        {
          if (delta.Id == this.MyActorId)
          {
            this.ApplyCurrentGameFrameUpdates(delta);
            this._localStateSender.RecieveDeltaUpdate(delta);
          }
          else
            this._stateInterpolator.UpdateCharacterInfo(delta);
          if (delta.Contains(262144))
            flag = true;
        }
      }
      if (!flag)
        return;
      this.UpdatePlayerCounters();
    }
    catch (Exception ex)
    {
      ex.Data.Add((object) nameof (OnGameFrameUpdate), (object) deltas.Count);
      throw;
    }
  }

  protected virtual void UpdatePlayerCounters()
  {
  }

  protected virtual void ApplyCurrentGameFrameUpdates(SyncObject delta)
  {
    try
    {
      if (delta.Contains(2097152))
      {
        int num = (int) (short) ((Dictionary<int, object>) delta.Data)[2097152];
        Singleton<HpApHud>.Instance.HP = num;
        if (num <= 0)
          GameState.LocalPlayer.SetPlayerDead();
      }
      if (delta.Contains(67108864))
        Singleton<HpApHud>.Instance.AP = ((ArmorInfo) ((Dictionary<int, object>) delta.Data)[67108864]).ArmorPoints;
      if (!delta.Contains(16384))
        return;
      StatsInfo statsInfo = (StatsInfo) ((Dictionary<int, object>) delta.Data)[16384];
      if (statsInfo.XP == (ushort) 0 && statsInfo.Points == (ushort) 0 && statsInfo.Kills == (short) 0 && statsInfo.Deaths == (short) 0)
      {
        HudController.Instance.XpPtsHud.OnGameStart();
      }
      else
      {
        HudController.Instance.XpPtsHud.GainXp((int) statsInfo.XP - (int) GameState.LocalCharacter.XP);
        HudController.Instance.XpPtsHud.GainPoints((int) statsInfo.Points - (int) GameState.LocalCharacter.Points);
      }
    }
    catch
    {
      Debug.LogError((object) string.Format("ApplyCurrentGameFrameUpdates with delta.Id={0} and DeltaCode={1}", (object) delta.Id, (object) delta.DeltaCode));
      throw;
    }
  }

  protected override void OnStartGame()
  {
    base.OnStartGame();
    this._stateInterpolator.Run();
  }

  [NetworkMethod(76)]
  protected virtual void OnMatchStart(int matchCount, int matchEndServerTicks)
  {
    GameConnectionManager.Client.PeerListener.UpdateServerTime();
    CheatDetection.SyncSystemTime();
    this.IsMatchRunning = true;
    this._roundStartTime = matchEndServerTicks - this.GameData.RoundTime * 1000;
    this._stateInterpolator.Run();
    LevelCamera.Instance.ResetFeedback();
    GameState.LocalPlayer.UpdateWeaponController();
    foreach (CharacterConfig characterConfig in this._characterByActorId.Values)
      characterConfig.IsAnimationEnabled = true;
    CmuneEventHandler.Route((object) new OnMatchStartEvent()
    {
      MatchCount = matchCount,
      MatchEndServerTicks = matchEndServerTicks
    });
  }

  [NetworkMethod(77)]
  protected void OnMatchEnd(EndOfMatchData endOfMatchData)
  {
    if (GameState.LocalPlayer.IsDead)
    {
      if ((UnityEngine.Object) GameState.LocalDecorator != (UnityEngine.Object) null)
        GameState.LocalDecorator.DisableRagdoll();
      Vector3 position = Vector3.zero;
      Quaternion rotation = Quaternion.identity;
      Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(this._nextSpawnPoint, (GameMode) this.GameData.GameMode, GameState.LocalCharacter.TeamID, out position, out rotation);
      GameState.LocalPlayer.SpawnPlayerAt(position, rotation);
    }
    if ((UnityEngine.Object) GameState.LocalPlayer.Character != (UnityEngine.Object) null)
    {
      switch (Singleton<WeaponController>.Instance.CurrentSlot)
      {
        case LoadoutSlotType.WeaponMelee:
          GameState.LocalPlayer.Character.WeaponSimulator.UpdateWeaponSlot(0, false);
          break;
        case LoadoutSlotType.WeaponPrimary:
          GameState.LocalPlayer.Character.WeaponSimulator.UpdateWeaponSlot(1, false);
          break;
        case LoadoutSlotType.WeaponSecondary:
          GameState.LocalPlayer.Character.WeaponSimulator.UpdateWeaponSlot(2, false);
          break;
        case LoadoutSlotType.WeaponTertiary:
          GameState.LocalPlayer.Character.WeaponSimulator.UpdateWeaponSlot(3, false);
          break;
      }
    }
    LevelCamera.SetBobMode(BobMode.Idle);
    if ((bool) (UnityEngine.Object) Singleton<WeaponController>.Instance.CurrentDecorator)
      Singleton<WeaponController>.Instance.CurrentDecorator.StopSound();
    foreach (CharacterConfig characterConfig in this._characterByActorId.Values)
      characterConfig.State.Set(PlayerStates.PAUSED, true);
    GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.AfterRound);
    this.UpdatePlayerStatistics(endOfMatchData.PlayerStatsTotal, endOfMatchData.PlayerStatsBestPerLife);
    if (ApplicationDataManager.Channel == ChannelType.WebFacebook)
    {
      Debug.Log((object) string.Format("Match:{0} Total:{1}", (object) endOfMatchData.PlayerStatsTotal.GetKills(), (object) Singleton<PlayerDataManager>.Instance.ServerLocalPlayerStatisticsView.Splats));
      if (endOfMatchData.PlayerStatsTotal.GetKills() > 0)
      {
        AutoMonoBehaviour<FacebookInterface>.Instance.PublishFbScore(Singleton<PlayerDataManager>.Instance.ServerLocalPlayerStatisticsView.Splats);
        AchievementType firstAchievement = this.GetPlayersFirstAchievement(endOfMatchData);
        if (firstAchievement != AchievementType.None)
          AutoMonoBehaviour<FacebookInterface>.Instance.PublishFbAchievement(firstAchievement);
      }
    }
    Singleton<EndOfMatchStats>.Instance.Data = endOfMatchData;
    GameState.LocalPlayer.SetPlayerControlState(LocalPlayer.PlayerState.Overview);
    CmuneEventHandler.Route((object) new OnMatchEndEvent());
    Singleton<ProjectileManager>.Instance.ClearAll();
    this.OnEndOfMatch();
  }

  protected virtual void OnEndOfMatch()
  {
  }

  private AchievementType GetPlayersFirstAchievement(EndOfMatchData endOfMatchData)
  {
    AchievementType firstAchievement = AchievementType.None;
    StatsSummary statsSummary = endOfMatchData.MostValuablePlayers.Find((Predicate<StatsSummary>) (p => p.Cmid == PlayerDataManager.Cmid));
    if (statsSummary != null)
    {
      List<AchievementType> achievementTypeList = new List<AchievementType>();
      foreach (KeyValuePair<byte, ushort> achievement in statsSummary.Achievements)
        achievementTypeList.Add((AchievementType) achievement.Key);
      if (achievementTypeList.Count > 0)
        firstAchievement = achievementTypeList[0];
    }
    return firstAchievement;
  }

  private void UpdatePlayerStatistics(StatsCollection totalStats, StatsCollection bestPerLife)
  {
    int playerLevelSecure = PlayerDataManager.PlayerLevelSecure;
    if (playerLevelSecure <= 0 || playerLevelSecure >= PlayerXpUtil.MaxPlayerLevel)
      return;
    int experienceSecure = PlayerDataManager.PlayerExperienceSecure;
    Singleton<PlayerDataManager>.Instance.SetPlayerStatisticsView(ConvertStatistics.UpdatePlayerStatisticsView(Singleton<PlayerDataManager>.Instance.ServerLocalPlayerStatisticsView, playerLevelSecure, totalStats, bestPerLife));
    if (PlayerDataManager.PlayerLevel != PlayerXpUtil.GetLevelForXp(experienceSecure))
      PopupSystem.Show((IPopupDialog) new LevelUpPopup(PlayerDataManager.PlayerLevel, playerLevelSecure));
    GlobalUIRibbon.Instance.AddXPEvent(totalStats.Xp);
    if (totalStats.Points > 0)
    {
      PlayerDataManager.AddPointsSecure(totalStats.Points);
      GlobalUIRibbon.Instance.AddPointsEvent(totalStats.Points);
    }
    GameState.LocalCharacter.Level = PlayerDataManager.PlayerLevelSecure;
  }

  [NetworkMethod(97)]
  protected void OnSetPowerupState(List<int> pickedPowerupIds)
  {
    for (int index = 0; pickedPowerupIds != null && index < pickedPowerupIds.Count; ++index)
      CmuneEventHandler.Route((object) new PickupItemEvent(pickedPowerupIds[index], false));
  }

  [NetworkMethod(61)]
  protected void OnPowerUpPicked(int powerupID, byte state) => CmuneEventHandler.Route((object) new PickupItemEvent(powerupID, state == (byte) 0));

  [NetworkMethod(84)]
  protected void OnDoorOpened(int doorID) => CmuneEventHandler.Route((object) new DoorOpenedEvent(doorID));

  [NetworkMethod(71)]
  protected virtual void OnSetNextSpawnPoint(int index, int coolDownTime)
  {
    if (GameState.LocalCharacter.IsSpectator)
      return;
    this.RespawnPlayerInSeconds(index, coolDownTime);
  }

  public void RespawnPlayerInSeconds(int index, int seconds)
  {
    this._nextSpawnPoint = index;
    if (seconds > 0)
    {
      this._nextSpawnCountdown = seconds;
      this.NextSpawnTime = Time.time + (float) seconds;
      this.IsWaitingForSpawn = true;
    }
    else
      this.RespawnPlayer();
  }

  public bool HasAvatarLoaded(int actorId) => this._characterByActorId.ContainsKey(actorId);

  [NetworkMethod(75)]
  protected void OnSetEndOfRoundCountdown(int secondsUntilNextRound) => CmuneEventHandler.Route((object) new OnSetEndOfMatchCountdownEvent()
  {
    SecondsUntilNextMatch = secondsUntilNextRound
  });

  [NetworkMethod(78)]
  protected virtual void OnDamageEvent(DamageEvent ev)
  {
    if (!GameState.HasCurrentPlayer)
      return;
    foreach (KeyValuePair<byte, byte> keyValuePair in (Dictionary<byte, byte>) ev.Damage)
    {
      CmuneEventHandler.Route((object) new OnPlayerDamageEvent()
      {
        Angle = Conversion.Byte2Angle(keyValuePair.Key),
        DamageValue = (float) keyValuePair.Value
      });
      if ((ev.DamageEffectFlag & 1) != 0)
        GameState.LocalPlayer.DamageFactor = ev.DamgeEffectValue;
    }
  }

  [NetworkMethod(80)]
  protected virtual void OnSplatGameEvent(
    int shooter,
    int target,
    byte weaponClass,
    byte bodyPart)
  {
    UberStrike.Realtime.UnitySdk.CharacterInfo characterInfo1;
    UberStrike.Realtime.UnitySdk.CharacterInfo characterInfo2;
    if (!this.Players.TryGetValue(shooter, out characterInfo1) || !this.Players.TryGetValue(target, out characterInfo2))
      return;
    UberstrikeItemClass weaponClass1 = (UberstrikeItemClass) weaponClass;
    BodyPart bodyPart1 = (BodyPart) bodyPart;
    if (shooter != target)
    {
      if (shooter == this.MyActorId)
        CmuneEventHandler.Route((object) new OnPlayerKillEnemyEvent()
        {
          EmemyInfo = characterInfo2,
          WeaponCategory = weaponClass1,
          BodyHitPart = bodyPart1
        });
      else if (target == this.MyActorId)
      {
        CmuneEventHandler.Route((object) new OnPlayerKilledEvent()
        {
          ShooterInfo = characterInfo1,
          WeaponCategory = weaponClass1,
          BodyHitPart = bodyPart1
        });
      }
      else
      {
        InGameEventFeedbackType eventType = InGameEventFeedbackType.None;
        if (weaponClass1 == UberstrikeItemClass.WeaponMelee)
        {
          eventType = InGameEventFeedbackType.Humiliation;
        }
        else
        {
          switch (bodyPart)
          {
            case 2:
              eventType = InGameEventFeedbackType.HeadShot;
              break;
            case 4:
              eventType = InGameEventFeedbackType.NutShot;
              break;
          }
        }
        Singleton<HudUtil>.Instance.AddInGameEvent(characterInfo1.PlayerName, characterInfo2.PlayerName, weaponClass1, eventType, characterInfo1.TeamID, characterInfo2.TeamID);
      }
    }
    else
      CmuneEventHandler.Route((object) new OnPlayerSuicideEvent()
      {
        PlayerInfo = characterInfo1
      });
  }

  [NetworkMethod(85)]
  protected void OnSetPlayerSpawnPosition(byte playerNumber, Vector3 pos) => this._stateInterpolator.UpdatePositionHard(playerNumber, pos);

  public void SendPlayerTeamChange() => this.SendMethodToServer((byte) 54, (object) GameState.CurrentPlayerID);

  public void SetReadyForNextMatch(bool isReady)
  {
    if (!isReady)
      return;
    this.SendMethodToServer((byte) 74, (object) this.MyActorId);
  }

  public void SendPlayerSpawnPosition(Vector3 position) => this.SendMethodToServer((byte) 85, (object) this.MyActorId, (object) position);

  public void UpdatePlayerReadyForNextRound()
  {
    this._playerReadyForNextRound = 0;
    foreach (UberStrike.Realtime.UnitySdk.CharacterInfo characterInfo in this.Players.Values)
    {
      if (characterInfo.IsReadyForGame)
        ++this._playerReadyForNextRound;
    }
  }

  protected void SpawnPlayerAt(Vector3 position, Quaternion rotation)
  {
    try
    {
      GameState.LocalPlayer.SpawnPlayerAt(position, rotation);
      GameState.LocalPlayer.InitializePlayer();
      CmuneEventHandler.Route((object) new OnPlayerRespawnEvent());
      GameState.LocalPlayer.UpdateWeaponController();
      this.SendMethodToServer((byte) 6, (object) this.MyActorId);
    }
    catch
    {
      Debug.LogError((object) string.Format("SpawnPlayerAt with game {0}", (object) CmunePrint.Properties((object) this)));
      throw;
    }
  }

  public virtual void RequestRespawn() => this.SendMethodToServer((byte) 72, (object) this.MyActorId);

  public virtual void IncreaseHealthAndArmor(int health, int armor) => this.SendMethodToServer((byte) 98, (object) this.MyActorId, (object) (byte) health, (object) (byte) armor);

  public virtual void PickupPowerup(int pickupID, PickupItemType type, int value) => this.SendMethodToServer((byte) 61, (object) this.MyActorId, (object) pickupID, (object) (byte) type, (object) (byte) value);

  public void OpenDoor(int doorID) => this.SendMethodToServer((byte) 84, (object) doorID);

  public void EmitQuickItem(
    Vector3 origin,
    Vector3 direction,
    int itemId,
    byte playerNumber,
    int projectileID)
  {
    this.SendMethodToAll((byte) 100, (object) origin, (object) direction, (object) itemId, (object) playerNumber, (object) projectileID);
  }

  [NetworkMethod(100)]
  protected void OnEmitQuickItem(
    Vector3 origin,
    Vector3 direction,
    int itemId,
    byte playerNumber,
    int projectileID)
  {
    if (!GameState.CurrentGame.IsGameStarted)
      return;
    IUnityItem itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(itemId);
    if (itemInShop != null)
    {
      if (itemInShop.Prefab is IGrenadeProjectile prefab)
      {
        IGrenadeProjectile p = prefab.Throw(origin, direction);
        if ((int) playerNumber == GameState.CurrentPlayerID)
          p.SetLayer(UberstrikeLayer.LocalProjectile);
        else
          p.SetLayer(UberstrikeLayer.RemoteProjectile);
        Singleton<ProjectileManager>.Instance.AddProjectile((IProjectile) p, projectileID);
      }
      else
        Debug.LogWarning((object) ("OnEmitQuickItem failed because Item is not a projectile: " + (object) itemId + "/" + (object) playerNumber + "/" + (object) projectileID));
    }
    else
      Debug.LogWarning((object) ("OnEmitQuickItem failed because item not found: " + (object) itemId + "/" + (object) playerNumber + "/" + (object) projectileID));
  }

  public void EmitProjectile(
    Vector3 origin,
    Vector3 direction,
    LoadoutSlotType slot,
    int projectileID,
    bool explode)
  {
    this.SendMethodToAll((byte) 86, (object) this.MyActorId, (object) origin, (object) direction, (object) (byte) slot, (object) projectileID, (object) explode);
  }

  [NetworkMethod(86)]
  protected void OnEmitProjectile(
    int actorId,
    Vector3 origin,
    Vector3 direction,
    byte slot,
    int projectileID,
    bool explode)
  {
    CharacterConfig character;
    if (!this.TryGetCharacter(actorId, out character))
      return;
    IProjectile p = character.WeaponSimulator.EmitProjectile(actorId, character.State.PlayerNumber, origin, direction, (LoadoutSlotType) slot, projectileID, explode);
    if (p == null)
      return;
    Singleton<ProjectileManager>.Instance.AddProjectile(p, projectileID);
  }

  public void RemoveProjectile(int projectileId, bool explode) => this.SendMethodToAll((byte) 87, (object) projectileId, (object) explode);

  [NetworkMethod(87)]
  protected virtual void OnRemoveProjectile(int projectileId, bool explode) => Singleton<ProjectileManager>.Instance.RemoveProjectile(projectileId, explode);

  public void SingleBulletFire() => this.SendMethodToAll((byte) 89, (object) this.MyActorId);

  [NetworkMethod(89)]
  protected virtual void OnSingleBulletFire(int actorId)
  {
    CharacterConfig character;
    if (!this.TryGetCharacter(actorId, out character) || !character.State.IsAlive || character.IsLocal)
      return;
    character.WeaponSimulator.Shoot(character.State);
  }

  public virtual void PlayerHit(
    int targetPlayer,
    short damage,
    BodyPart part,
    Vector3 force,
    int shotCount,
    int weaponID,
    UberstrikeItemClass weaponClass,
    DamageEffectType damageEffectFlag,
    float damageEffectValue)
  {
    byte num = 0;
    Vector3 normalized = force.normalized with { y = 0.0f };
    if ((double) normalized.magnitude != 0.0)
      num = Conversion.Angle2Byte(Quaternion.LookRotation(normalized).eulerAngles.y);
    this.SendMethodToServer((byte) 68, (object) this.MyActorId, (object) targetPlayer, (object) damage, (object) (byte) part, (object) shotCount, (object) num, (object) weaponID, (object) (byte) weaponClass, (object) (int) damageEffectFlag, (object) damageEffectValue);
    if (this.MyActorId != targetPlayer)
      return;
    short finalDamage;
    byte finalArmorPoints;
    GameState.LocalCharacter.Armor.SimulateAbsorbDamage(damage, part, out finalDamage, out finalArmorPoints);
    Singleton<HpApHud>.Instance.HP = (int) GameState.LocalCharacter.Health - (int) finalDamage;
    Singleton<HpApHud>.Instance.AP = (int) finalArmorPoints;
    if (ApplicationDataManager.ApplicationOptions.VideoBloomHitEffect)
      RenderSettingsController.Instance.ShowAgonyTint((float) damage / 50f);
    GameState.LocalPlayer.MoveController.ApplyForce(force, CharacterMoveController.ForceType.Additive);
  }

  public void SendPlayerHitFeedback(int targetPlayer, Vector3 force) => this.SendMethodToPlayer(targetPlayer, (byte) 68, (object) force);

  [NetworkMethod(68)]
  public void OnPlayerHit(Vector3 force) => GameState.LocalPlayer.MoveController.ApplyForce(force, CharacterMoveController.ForceType.Additive);

  public void ActivateQuickItem(
    QuickItemLogic logic,
    int robotLifeTime,
    int scrapsLifeTime,
    bool isInstant = false)
  {
    this.SendMethodToAll((byte) 99, (object) GameState.LocalCharacter.ActorId, (object) (byte) logic, (object) robotLifeTime, (object) scrapsLifeTime, (object) isInstant);
  }

  [NetworkMethod(99)]
  public void OnQuickItemEvent(
    int actorId,
    byte eventType,
    int robotLifeTime,
    int scrapsLifeTime,
    bool isInstant)
  {
    CharacterConfig character;
    if (!this.TryGetCharacter(actorId, out character))
      return;
    Singleton<QuickItemSfxController>.Instance.ShowThirdPersonEffect(character, (QuickItemLogic) eventType, robotLifeTime, scrapsLifeTime, isInstant);
  }

  [NetworkMethod(101)]
  protected void KickPlayer(string message)
  {
    Singleton<GameStateController>.Instance.UnloadGameMode();
    MenuPageManager.Instance.LoadPage(PageType.Home);
    PopupSystem.ShowMessage("Cheat Detection", message, PopupSystem.AlertType.OK, (Action) (() => { }));
  }

  [NetworkMethod(31)]
  protected void OnModCustomMessage(string message) => CommConnectionManager.CommCenter.OnModerationCustomMessage(message);

  [NetworkMethod(30)]
  protected void OnMutePlayer(bool mute) => CommConnectionManager.CommCenter.OnModerationMutePlayer(mute);

  public bool TryGetCharacter(int actorId, out CharacterConfig character) => this._characterByActorId.TryGetValue(actorId, out character) && (UnityEngine.Object) character != (UnityEngine.Object) null;

  public bool TryGetDecorator(int actorId, out AvatarDecorator decorator)
  {
    CharacterConfig characterConfig;
    decorator = !this._characterByActorId.TryGetValue(actorId, out characterConfig) || !((UnityEngine.Object) characterConfig != (UnityEngine.Object) null) ? (AvatarDecorator) null : characterConfig.Decorator;
    return (UnityEngine.Object) decorator != (UnityEngine.Object) null;
  }

  protected void HideRemotePlayerHudFeedback()
  {
    foreach (CharacterConfig characterConfig in this._characterByActorId.Values)
    {
      if ((UnityEngine.Object) characterConfig != (UnityEngine.Object) null && (UnityEngine.Object) characterConfig.Decorator != (UnityEngine.Object) null)
        characterConfig.Decorator.HudInformation.Hide();
    }
  }

  public bool TryGetPlayerWithCmid(int cmid, out UberStrike.Realtime.UnitySdk.CharacterInfo config)
  {
    config = (UberStrike.Realtime.UnitySdk.CharacterInfo) null;
    foreach (UberStrike.Realtime.UnitySdk.CharacterInfo characterInfo in this.Players.Values)
    {
      if (characterInfo.Cmid == cmid)
      {
        config = characterInfo;
        break;
      }
    }
    return config != null;
  }

  protected void InstantiateCharacter(UberStrike.Realtime.UnitySdk.CharacterInfo info)
  {
    if (!this._characterByActorId.ContainsKey(info.ActorId))
    {
      if (info.ActorId == this.MyActorId)
      {
        this._isLocalAvatarLoaded = true;
        CharacterConfig character = PrefabManager.Instance.InstantiateLocalCharacter();
        this._characterByActorId.Add(info.ActorId, character);
        this.ConfigureCharacter(info, character, true);
      }
      else
      {
        CharacterConfig character = PrefabManager.Instance.InstantiateRemoteCharacter();
        this._characterByActorId.Add(info.ActorId, character);
        this.ConfigureCharacter(info, character, false);
      }
    }
    else
      Debug.LogError((object) string.Format("Failed call of LoadAvatarAsset {0} because already loaded!", (object) info.ActorId));
  }

  protected void LeaveClientGameMode(int playerId) => base.OnPlayerLeft(playerId);

  public void ChangeAllPlayerOutline(TeamID myTeam)
  {
    if (myTeam == TeamID.NONE)
      return;
    foreach (KeyValuePair<int, CharacterConfig> keyValuePair in this._characterByActorId)
    {
      if (keyValuePair.Key != this.MyActorId)
        this.UpdatePlayerOutlineByTeamID(keyValuePair.Value, myTeam);
    }
  }

  public void ChangePlayerOutlineById(int playerID)
  {
    CharacterConfig character;
    if (!this.TryGetCharacter(playerID, out character))
      return;
    this.ChangePlayerOutline(character);
  }

  public void ChangePlayerOutline(CharacterConfig player) => this.UpdatePlayerOutlineByTeamID(player, GameState.LocalCharacter.TeamID);

  public void UpdatePlayerOutlineByTeamID(CharacterConfig player, TeamID id)
  {
    if ((UnityEngine.Object) player != (UnityEngine.Object) null)
    {
      if (id != TeamID.NONE && player.Team == id)
        player.Decorator.EnableOutline(true);
      else
        player.Decorator.EnableOutline(false);
    }
    else
      Debug.LogError((object) "Failed to Change player outline");
  }

  public void UnloadAllPlayers()
  {
    foreach (CharacterConfig characterConfig in this._characterByActorId.Values)
      characterConfig.Destroy();
    this._characterByActorId.Clear();
  }

  public void SendShotCounts(List<int> shotCounts) => this.SendMethodToServer((byte) 102, (object) PlayerDataManager.CmidSecure, (object) shotCounts);

  public virtual bool IsWaitingForPlayers => this.IsGameStarted && this.Players.Count <= 1;

  public bool IsGameAboutToEnd => (double) this.GameTime >= (double) (this.GameData.RoundTime - 1);

  public virtual bool CanShowTabscreen => this.IsGameStarted || GameState.LocalCharacter.IsSpectator;

  public virtual float GameTime => (float) (GameConnectionManager.Client.PeerListener.ServerTimeTicks - this._roundStartTime) / 1000f;

  public bool IsWaitingForSpawn { get; protected set; }

  public float NextSpawnTime { get; private set; }

  public bool IsGameClosed
  {
    get => this._isGameClosed;
    set => this._isGameClosed = value;
  }

  public ICharacterState MyCharacterState => (ICharacterState) this._localStateSender;

  public int PlayerReadyForNextRound => this._playerReadyForNextRound;

  public GameMode GameMode => (GameMode) this.GameData.GameMode;

  public bool IsLocalAvatarLoaded => this._isLocalAvatarLoaded;

  public int CurrentSpawnPoint => this._nextSpawnPoint;

  public int PlayerCount => this._characterByActorId.Count;

  public IEnumerable<CharacterConfig> AllCharacters => (IEnumerable<CharacterConfig>) this._characterByActorId.Values;
}
