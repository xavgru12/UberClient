// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Client.BaseGameRoom
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Serialization;
using UberStrike.Core.Types;
using UnityEngine;

namespace UberStrike.Realtime.Client
{
  public abstract class BaseGameRoom : IEventDispatcher, IRoomLogic
  {
    IOperationSender IRoomLogic.Operations => (IOperationSender) this.Operations;

    public GameRoomOperations Operations { get; private set; }

    protected BaseGameRoom() => this.Operations = new GameRoomOperations();

    public void OnEvent(byte id, byte[] data)
    {
      switch (id)
      {
        case 12:
          this.PowerUpPicked(data);
          break;
        case 13:
          this.SetPowerupState(data);
          break;
        case 14:
          this.ResetAllPowerups(data);
          break;
        case 15:
          this.DoorOpen(data);
          break;
        case 16:
          this.DisconnectCountdown(data);
          break;
        case 17:
          this.MatchStartCountdown(data);
          break;
        case 18:
          this.MatchStart(data);
          break;
        case 19:
          this.MatchEnd(data);
          break;
        case 20:
          this.TeamWins(data);
          break;
        case 21:
          this.WaitingForPlayers(data);
          break;
        case 22:
          this.PrepareNextRound(data);
          break;
        case 23:
          this.AllPlayers(data);
          break;
        case 24:
          this.AllPlayerDeltas(data);
          break;
        case 25:
          this.AllPlayerPositions(data);
          break;
        case 26:
          this.PlayerDelta(data);
          break;
        case 27:
          this.PlayerJumped(data);
          break;
        case 28:
          this.PlayerRespawnCountdown(data);
          break;
        case 29:
          this.PlayerRespawned(data);
          break;
        case 30:
          this.PlayerJoinedGame(data);
          break;
        case 31:
          this.JoinGameFailed(data);
          break;
        case 32:
          this.PlayerLeftGame(data);
          break;
        case 33:
          this.PlayerChangedTeam(data);
          break;
        case 34:
          this.JoinedAsSpectator(data);
          break;
        case 35:
          this.PlayersReadyUpdated(data);
          break;
        case 36:
          this.DamageEvent(data);
          break;
        case 37:
          this.PlayerKilled(data);
          break;
        case 38:
          this.UpdateRoundScore(data);
          break;
        case 39:
          this.KillsRemaining(data);
          break;
        case 40:
          this.LevelUp(data);
          break;
        case 41:
          this.KickPlayer(data);
          break;
        case 42:
          this.QuickItemEvent(data);
          break;
        case 43:
          this.SingleBulletFire(data);
          break;
        case 44:
          this.PlayerHit(data);
          break;
        case 45:
          this.RemoveProjectile(data);
          break;
        case 46:
          this.EmitProjectile(data);
          break;
        case 47:
          this.EmitQuickItem(data);
          break;
        case 48:
          this.ActivateQuickItem(data);
          break;
        case 49:
          this.ChatMessage(data);
          break;
      }
    }

    protected abstract void OnPowerUpPicked(int id, byte flag);

    protected abstract void OnSetPowerupState(List<int> states);

    protected abstract void OnResetAllPowerups();

    protected abstract void OnDoorOpen(int id);

    protected abstract void OnDisconnectCountdown(byte countdown);

    protected abstract void OnMatchStartCountdown(byte countdown);

    protected abstract void OnMatchStart(int roundNumber, int endTime);

    protected abstract void OnMatchEnd(EndOfMatchData data);

    protected abstract void OnTeamWins(TeamID team);

    protected abstract void OnWaitingForPlayers();

    protected abstract void OnPrepareNextRound();

    protected abstract void OnAllPlayers(
      List<GameActorInfo> allPlayers,
      List<PlayerMovement> allPositions,
      ushort gameframe);

    protected abstract void OnAllPlayerDeltas(List<GameActorInfoDelta> allDeltas);

    protected abstract void OnAllPlayerPositions(
      List<PlayerMovement> allPositions,
      ushort gameframe);

    protected abstract void OnPlayerDelta(GameActorInfoDelta delta);

    protected abstract void OnPlayerJumped(int cmid, Vector3 position);

    protected abstract void OnPlayerRespawnCountdown(byte countdown);

    protected abstract void OnPlayerRespawned(int cmid, Vector3 position, byte rotation);

    protected abstract void OnPlayerJoinedGame(GameActorInfo player, PlayerMovement position);

    protected abstract void OnJoinGameFailed(string message);

    protected abstract void OnPlayerLeftGame(int cmid);

    protected abstract void OnPlayerChangedTeam(int cmid, TeamID team);

    protected abstract void OnJoinedAsSpectator();

    protected abstract void OnPlayersReadyUpdated();

    protected abstract void OnDamageEvent(UberStrike.Core.Models.DamageEvent damageEvent);

    protected abstract void OnPlayerKilled(
      int shooter,
      int target,
      byte weaponClass,
      ushort damage,
      byte bodyPart,
      Vector3 direction);

    protected abstract void OnUpdateRoundScore(int round, short blue, short red);

    protected abstract void OnKillsRemaining(int killsRemaining, int leaderCmid);

    protected abstract void OnLevelUp(int newLevel);

    protected abstract void OnKickPlayer(string message);

    protected abstract void OnQuickItemEvent(
      int cmid,
      byte eventType,
      int robotLifeTime,
      int scrapsLifeTime,
      bool isInstant);

    protected abstract void OnSingleBulletFire(int cmid);

    protected abstract void OnPlayerHit(Vector3 force);

    protected abstract void OnRemoveProjectile(int projectileId, bool explode);

    protected abstract void OnEmitProjectile(
      int cmid,
      Vector3 origin,
      Vector3 direction,
      byte slot,
      int projectileID,
      bool explode);

    protected abstract void OnEmitQuickItem(
      Vector3 origin,
      Vector3 direction,
      int itemId,
      byte playerNumber,
      int projectileID);

    protected abstract void OnActivateQuickItem(
      int cmid,
      QuickItemLogic logic,
      int robotLifeTime,
      int scrapsLifeTime,
      bool isInstant);

    protected abstract void OnChatMessage(
      int cmid,
      string name,
      string message,
      MemberAccessLevel accessLevel,
      byte context);

    private void PowerUpPicked(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPowerUpPicked(Int32Proxy.Deserialize((Stream) bytes), ByteProxy.Deserialize((Stream) bytes));
    }

    private void SetPowerupState(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnSetPowerupState(ListProxy<int>.Deserialize((Stream) bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize)));
    }

    private void ResetAllPowerups(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnResetAllPowerups();
    }

    private void DoorOpen(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnDoorOpen(Int32Proxy.Deserialize((Stream) bytes));
    }

    private void DisconnectCountdown(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnDisconnectCountdown(ByteProxy.Deserialize((Stream) bytes));
    }

    private void MatchStartCountdown(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnMatchStartCountdown(ByteProxy.Deserialize((Stream) bytes));
    }

    private void MatchStart(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnMatchStart(Int32Proxy.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes));
    }

    private void MatchEnd(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnMatchEnd(EndOfMatchDataProxy.Deserialize((Stream) bytes));
    }

    private void TeamWins(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnTeamWins(EnumProxy<TeamID>.Deserialize((Stream) bytes));
    }

    private void WaitingForPlayers(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnWaitingForPlayers();
    }

    private void PrepareNextRound(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnPrepareNextRound();
    }

    private void AllPlayers(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnAllPlayers(ListProxy<GameActorInfo>.Deserialize((Stream) bytes, new ListProxy<GameActorInfo>.Deserializer<GameActorInfo>(GameActorInfoProxy.Deserialize)), ListProxy<PlayerMovement>.Deserialize((Stream) bytes, new ListProxy<PlayerMovement>.Deserializer<PlayerMovement>(PlayerMovementProxy.Deserialize)), UInt16Proxy.Deserialize((Stream) bytes));
    }

    private void AllPlayerDeltas(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnAllPlayerDeltas(ListProxy<GameActorInfoDelta>.Deserialize((Stream) bytes, new ListProxy<GameActorInfoDelta>.Deserializer<GameActorInfoDelta>(GameActorInfoDeltaProxy.Deserialize)));
    }

    private void AllPlayerPositions(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnAllPlayerPositions(ListProxy<PlayerMovement>.Deserialize((Stream) bytes, new ListProxy<PlayerMovement>.Deserializer<PlayerMovement>(PlayerMovementProxy.Deserialize)), UInt16Proxy.Deserialize((Stream) bytes));
    }

    private void PlayerDelta(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerDelta(GameActorInfoDeltaProxy.Deserialize((Stream) bytes));
    }

    private void PlayerJumped(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerJumped(Int32Proxy.Deserialize((Stream) bytes), Vector3Proxy.Deserialize((Stream) bytes));
    }

    private void PlayerRespawnCountdown(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerRespawnCountdown(ByteProxy.Deserialize((Stream) bytes));
    }

    private void PlayerRespawned(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerRespawned(Int32Proxy.Deserialize((Stream) bytes), Vector3Proxy.Deserialize((Stream) bytes), ByteProxy.Deserialize((Stream) bytes));
    }

    private void PlayerJoinedGame(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerJoinedGame(GameActorInfoProxy.Deserialize((Stream) bytes), PlayerMovementProxy.Deserialize((Stream) bytes));
    }

    private void JoinGameFailed(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnJoinGameFailed(StringProxy.Deserialize((Stream) bytes));
    }

    private void PlayerLeftGame(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerLeftGame(Int32Proxy.Deserialize((Stream) bytes));
    }

    private void PlayerChangedTeam(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerChangedTeam(Int32Proxy.Deserialize((Stream) bytes), EnumProxy<TeamID>.Deserialize((Stream) bytes));
    }

    private void JoinedAsSpectator(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnJoinedAsSpectator();
    }

    private void PlayersReadyUpdated(byte[] _bytes)
    {
      using (new MemoryStream(_bytes))
        this.OnPlayersReadyUpdated();
    }

    private void DamageEvent(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnDamageEvent(DamageEventProxy.Deserialize((Stream) bytes));
    }

    private void PlayerKilled(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerKilled(Int32Proxy.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes), ByteProxy.Deserialize((Stream) bytes), UInt16Proxy.Deserialize((Stream) bytes), ByteProxy.Deserialize((Stream) bytes), Vector3Proxy.Deserialize((Stream) bytes));
    }

    private void UpdateRoundScore(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnUpdateRoundScore(Int32Proxy.Deserialize((Stream) bytes), Int16Proxy.Deserialize((Stream) bytes), Int16Proxy.Deserialize((Stream) bytes));
    }

    private void KillsRemaining(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnKillsRemaining(Int32Proxy.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes));
    }

    private void LevelUp(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnLevelUp(Int32Proxy.Deserialize((Stream) bytes));
    }

    private void KickPlayer(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnKickPlayer(StringProxy.Deserialize((Stream) bytes));
    }

    private void QuickItemEvent(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnQuickItemEvent(Int32Proxy.Deserialize((Stream) bytes), ByteProxy.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes), BooleanProxy.Deserialize((Stream) bytes));
    }

    private void SingleBulletFire(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnSingleBulletFire(Int32Proxy.Deserialize((Stream) bytes));
    }

    private void PlayerHit(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnPlayerHit(Vector3Proxy.Deserialize((Stream) bytes));
    }

    private void RemoveProjectile(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnRemoveProjectile(Int32Proxy.Deserialize((Stream) bytes), BooleanProxy.Deserialize((Stream) bytes));
    }

    private void EmitProjectile(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnEmitProjectile(Int32Proxy.Deserialize((Stream) bytes), Vector3Proxy.Deserialize((Stream) bytes), Vector3Proxy.Deserialize((Stream) bytes), ByteProxy.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes), BooleanProxy.Deserialize((Stream) bytes));
    }

    private void EmitQuickItem(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnEmitQuickItem(Vector3Proxy.Deserialize((Stream) bytes), Vector3Proxy.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes), ByteProxy.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes));
    }

    private void ActivateQuickItem(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnActivateQuickItem(Int32Proxy.Deserialize((Stream) bytes), EnumProxy<QuickItemLogic>.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes), Int32Proxy.Deserialize((Stream) bytes), BooleanProxy.Deserialize((Stream) bytes));
    }

    private void ChatMessage(byte[] _bytes)
    {
      using (MemoryStream bytes = new MemoryStream(_bytes))
        this.OnChatMessage(Int32Proxy.Deserialize((Stream) bytes), StringProxy.Deserialize((Stream) bytes), StringProxy.Deserialize((Stream) bytes), EnumProxy<MemberAccessLevel>.Deserialize((Stream) bytes), ByteProxy.Deserialize((Stream) bytes));
    }
  }
}
