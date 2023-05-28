// Decompiled with JetBrains decompiler
// Type: LocalCharacterState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class LocalCharacterState : ICharacterState
{
  private const int PlayerSyncMask = 337920000;
  private int posSyncFrame;
  private int bagSyncFrame;
  private Vector3 lastPosition;
  private int sendCounter;
  private UberStrike.Realtime.UnitySdk.CharacterInfo _myInfo;
  private FpsGameMode _game;

  public LocalCharacterState(UberStrike.Realtime.UnitySdk.CharacterInfo info, FpsGameMode game)
  {
    this._myInfo = info;
    this._game = game;
    this.posSyncFrame = SystemTime.Running;
    this.bagSyncFrame = SystemTime.Running;
  }

  private event Action<SyncObject> _updateRecievedEvent;

  public void RecieveDeltaUpdate(SyncObject data)
  {
    data.DeltaCode &= 337920000;
    if (data.DeltaCode == 0)
      return;
    this._myInfo.ReadSyncData(data, 337920000);
    if (this._updateRecievedEvent == null)
      return;
    this._updateRecievedEvent(data);
  }

  public void SubscribeToEvents(CharacterConfig config)
  {
    this._updateRecievedEvent = (Action<SyncObject>) null;
    this._updateRecievedEvent += new Action<SyncObject>(config.OnCharacterStateUpdated);
  }

  public void UnSubscribeAll() => this._updateRecievedEvent = (Action<SyncObject>) null;

  public void SendUpdates()
  {
    if (SystemTime.Running >= this.posSyncFrame)
    {
      this.posSyncFrame = SystemTime.Running + 50;
      if (this.lastPosition != GameState.LocalCharacter.Position)
        this.sendCounter = 0;
      else
        ++this.sendCounter;
      if (this.sendCounter < 10)
      {
        this.lastPosition = GameState.LocalCharacter.Position;
        this._game.SendPositionUpdate();
      }
    }
    if (SystemTime.Running < this.bagSyncFrame)
      return;
    this.bagSyncFrame = SystemTime.Running + 100;
    this._game.SendCharacterInfoUpdate();
  }

  public UberStrike.Realtime.UnitySdk.CharacterInfo Info => this._myInfo;

  public Vector3 LastPosition => this._myInfo.Position;
}
