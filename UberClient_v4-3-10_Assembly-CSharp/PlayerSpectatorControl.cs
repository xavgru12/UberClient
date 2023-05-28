// Decompiled with JetBrains decompiler
// Type: PlayerSpectatorControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PlayerSpectatorControl : Singleton<PlayerSpectatorControl>
{
  private int _currentFollowActorId;
  private int _currentFollowIndex;
  private bool _isEnabled;

  private PlayerSpectatorControl()
  {
  }

  public bool IsEnabled
  {
    get => this._isEnabled;
    set
    {
      if (value)
      {
        this.EnterFreeMoveMode();
        if (!this._isEnabled)
        {
          CmuneEventHandler.AddListener<InputChangeEvent>(new Action<InputChangeEvent>(this.OnInputChanged));
          CmuneEventHandler.Route((object) new OnPlayerSpectatingEvent());
        }
      }
      else
      {
        this.ReleaseLastTarget();
        if (this._isEnabled)
        {
          if ((bool) (UnityEngine.Object) GameState.LocalPlayer && (bool) (UnityEngine.Object) GameState.LocalDecorator)
            GameState.LocalPlayer.SetPlayerControlState(LocalPlayer.PlayerState.FirstPerson, GameState.LocalDecorator.Configuration);
          CmuneEventHandler.RemoveListener<InputChangeEvent>(new Action<InputChangeEvent>(this.OnInputChanged));
          CmuneEventHandler.Route((object) new OnPlayerUnspecatingEvent());
        }
      }
      this._isEnabled = value;
    }
  }

  public void OnInputChanged(InputChangeEvent ev)
  {
    if (Singleton<InGameChatHud>.Instance.CanInput || !Screen.lockCursor)
      return;
    if (ev.Key == GameInputKey.PrimaryFire && ev.IsDown)
      this.FollowPrevPlayer();
    else if (ev.Key == GameInputKey.SecondaryFire && ev.IsDown)
    {
      this.FollowNextPlayer();
    }
    else
    {
      if (ev.Key != GameInputKey.Jump || !ev.IsDown)
        return;
      this.EnterFreeMoveMode();
    }
  }

  public void FollowNextPlayer()
  {
    try
    {
      if (!GameState.HasCurrentGame || GameState.CurrentGame.Players.Count <= 0)
        return;
      UberStrike.Realtime.UnitySdk.CharacterInfo[] characterInfoArray = GameState.CurrentGame.Players.ValueArray<int, UberStrike.Realtime.UnitySdk.CharacterInfo>();
      this._currentFollowIndex = (this._currentFollowIndex + 1) % characterInfoArray.Length;
      int currentFollowIndex = this._currentFollowIndex;
      while (characterInfoArray[this._currentFollowIndex].ActorId == GameState.CurrentPlayerID || !characterInfoArray[this._currentFollowIndex].IsAlive || !GameState.CurrentGame.HasAvatarLoaded(characterInfoArray[this._currentFollowIndex].ActorId))
      {
        this._currentFollowIndex = (this._currentFollowIndex + 1) % characterInfoArray.Length;
        if (this._currentFollowIndex == currentFollowIndex)
        {
          this.EnterFreeMoveMode();
          return;
        }
      }
      if (characterInfoArray[this._currentFollowIndex] != null)
        this.ChangeTarget(characterInfoArray[this._currentFollowIndex].ActorId);
      else
        this.EnterFreeMoveMode();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Failed to follow next player: " + ex.Message));
    }
  }

  public void FollowPrevPlayer()
  {
    try
    {
      if (!GameState.HasCurrentGame || GameState.CurrentGame.Players.Count <= 0)
        return;
      List<UberStrike.Realtime.UnitySdk.CharacterInfo> characterInfoList = new List<UberStrike.Realtime.UnitySdk.CharacterInfo>((IEnumerable<UberStrike.Realtime.UnitySdk.CharacterInfo>) GameState.CurrentGame.Players.Values);
      this._currentFollowIndex = (this._currentFollowIndex + characterInfoList.Count - 1) % characterInfoList.Count;
      int currentFollowIndex = this._currentFollowIndex;
      while (characterInfoList[this._currentFollowIndex].ActorId == GameState.CurrentPlayerID || !characterInfoList[this._currentFollowIndex].IsAlive || !GameState.CurrentGame.HasAvatarLoaded(characterInfoList[this._currentFollowIndex].ActorId))
      {
        this._currentFollowIndex = (this._currentFollowIndex + characterInfoList.Count - 1) % characterInfoList.Count;
        if (this._currentFollowIndex == currentFollowIndex)
        {
          this.EnterFreeMoveMode();
          return;
        }
      }
      if (characterInfoList[this._currentFollowIndex] != null)
        this.ChangeTarget(characterInfoList[this._currentFollowIndex].ActorId);
      else
        this.EnterFreeMoveMode();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Failed to follow prev player: " + ex.Message));
    }
  }

  public void ReleaseLastTarget()
  {
    CharacterConfig character;
    if (GameState.HasCurrentGame && this._currentFollowActorId > 0 && GameState.CurrentGame.TryGetCharacter(this._currentFollowActorId, out character))
      character.RemoveFollowCamera();
    this._currentFollowActorId = 0;
  }

  private void ChangeTarget(int actorId)
  {
    if (!GameState.HasCurrentGame || this._currentFollowActorId == actorId)
      return;
    this.ReleaseLastTarget();
    CharacterConfig character;
    if (!GameState.CurrentGame.TryGetCharacter(actorId, out character) || !(bool) (UnityEngine.Object) character.Decorator)
      return;
    this._currentFollowActorId = actorId;
    LevelCamera.Instance.SetTarget(character.Decorator.transform);
    LevelCamera.Instance.SetMode(LevelCamera.CameraMode.SmoothFollow);
    if (!character.State.IsAlive)
      LevelCamera.Instance.transform.position = character.State.Position;
    LevelCamera.Instance.SetLookAtHeight(1.3f);
    character.AddFollowCamera();
    character.Decorator.HudInformation.ForceShowInformation = true;
  }

  public void EnterFreeMoveMode()
  {
    this.ReleaseLastTarget();
    Screen.lockCursor = true;
    LevelCamera.Instance.SetLookAtHeight(0.0f);
    LevelCamera.Instance.SetMode(LevelCamera.CameraMode.Spectator);
  }

  public int CurrentActorId => this._currentFollowActorId;

  public bool IsFollowingPlayer => this._currentFollowActorId > 0;
}
