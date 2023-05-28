// Decompiled with JetBrains decompiler
// Type: CharacterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class CharacterConfig : MonoBehaviour, IShootable
{
  private ICharacterState _state;
  private PlayerSound _sound;
  private DamageInfo _lastShotInfo;
  private float _graceTimeAfterSpawn;
  private bool _graceTimeOut = true;
  [SerializeField]
  private bool _isLocalPlayer;
  [SerializeField]
  private PlayerDamageEffect _damageFeedback;
  [SerializeField]
  private CharacterTrigger _aimTrigger;
  [SerializeField]
  private float _walkingSoundSpeed = 0.38f;
  [SerializeField]
  private PlayerDropPickupItem _playerDropWeapon;

  private void Awake()
  {
    this.IsAnimationEnabled = true;
    this.MoveSimulator = new CharacterMoveSimulator(this.transform);
    this.WeaponSimulator = new WeaponSimulator(this);
    this.StateController = new CharacterStateAnimationController();
  }

  private void LateUpdate()
  {
    if (this._state != null)
    {
      if (this.Decorator.AnimationController != null)
        this.StateController.Update(this._state.Info, this.Decorator.AnimationController);
      this.WeaponSimulator.Update(this._state.Info, this.IsLocal);
      this.MoveSimulator.Update(this._state.Info);
    }
    if (this._sound != null)
      this._sound.Update();
    if ((double) this._graceTimeAfterSpawn > 0.0)
      this._graceTimeAfterSpawn -= Time.deltaTime;
    if (this._graceTimeOut || (double) this._graceTimeAfterSpawn > 0.0)
      return;
    this._graceTimeOut = true;
  }

  public void Initialize(ICharacterState state, AvatarDecorator decorator)
  {
    this._state = state;
    this._sound = new PlayerSound(state.Info);
    this._sound.SetCharacter(this);
    this._state.SubscribeToEvents(this);
    this.transform.position = this._state.LastPosition;
    this.SetAvatarDecorator(decorator);
    this.OnCharacterStateUpdated(SyncObjectBuilder.GetSyncData((CmuneDeltaSync) state.Info, true));
  }

  public void OnCharacterStateUpdated(SyncObject delta)
  {
    try
    {
      if (delta.Contains(8388608))
      {
        this.WeaponSimulator.UpdateWeapons((int) this._state.Info.CurrentWeaponSlot, (IList<int>) this._state.Info.Weapons.ItemIDs, (IList<int>) this._state.Info.QuickItems);
        this.WeaponSimulator.UpdateWeaponSlot((int) this._state.Info.CurrentWeaponSlot, this._isLocalPlayer);
      }
      else if (delta.Contains(16777216))
        this.WeaponSimulator.UpdateWeaponSlot((int) this._state.Info.CurrentWeaponSlot, this._isLocalPlayer);
      if (delta.Contains(134217728) && !this.IsLocal)
        Singleton<AvatarBuilder>.Instance.UpdateRemoteAvatar(this.Decorator, this._state.Info.Gear.ToArray(), this._state.Info.SkinColor);
      if (delta.Contains(256) && this._state.Info.Is(PlayerStates.GROUNDED) && (double) this.TimeLastGrounded + 0.5 < (double) Time.time && !this._state.Info.Is(PlayerStates.DIVING))
      {
        this.TimeLastGrounded = Time.time;
        if ((bool) (UnityEngine.Object) this.Decorator)
          this.Decorator.PlayFootSound(this.WalkingSoundSpeed);
      }
      if (!delta.Contains(2097152))
        return;
      int num = (int) (short) ((Dictionary<int, object>) delta.Data)[2097152];
      if (this.IsDead && num > 0)
      {
        this.Decorator.DisableRagdoll();
        this.IsDead = false;
        this._graceTimeOut = false;
        this._graceTimeAfterSpawn = 2f;
      }
      else if (!this.IsDead && num <= 0)
      {
        this.IsDead = true;
        Singleton<QuickItemSfxController>.Instance.DestroytSfxFromPlayer(this.State.PlayerNumber);
        this.Decorator.HudInformation.Hide();
        AvatarDecoratorConfig decorator = this.Decorator.SpawnDeadRagdoll(this._lastShotInfo);
        if (this.IsLocal)
          GameState.LocalPlayer.SetPlayerControlState(LocalPlayer.PlayerState.Death, decorator);
        if (GameState.CurrentGame.IsLocalAvatarLoaded)
        {
          Vector3 position = this.Decorator.transform.position + Vector3.up;
          if ((UnityEngine.Object) this._playerDropWeapon != (UnityEngine.Object) null && !this.IsLocal && this._state.Info.CurrentWeaponSlot != (byte) 4)
          {
            PlayerDropPickupItem playerDropPickupItem = UnityEngine.Object.Instantiate((UnityEngine.Object) this._playerDropWeapon, position, Quaternion.identity) as PlayerDropPickupItem;
            if ((bool) (UnityEngine.Object) playerDropPickupItem)
            {
              playerDropPickupItem.PickupID = (int) this._state.Info.PlayerNumber;
              playerDropPickupItem.WeaponItemId = this._state.Info.CurrentWeaponID;
            }
          }
          this.Decorator.PlayDieSound();
        }
        if (!this._isLocalPlayer)
          this.Decorator.HideWeapons();
      }
      this.Decorator.HudInformation.SetHealthBarValue((float) this._state.Info.Health / 100f);
    }
    catch (Exception ex)
    {
      ex.Data.Add((object) nameof (OnCharacterStateUpdated), (object) delta);
      throw;
    }
  }

  public bool IsDead { get; private set; }

  public void ApplyDamage(DamageInfo d)
  {
    this._lastShotInfo = d;
    if (this._state == null || !GameState.HasCurrentGame)
      return;
    if (this._state.Info.Health > (short) 0)
      GameState.CurrentGame.PlayerHit(this._state.Info.ActorId, d.Damage, d.BodyPart, d.Force, d.ShotCount, d.WeaponID, d.WeaponClass, d.DamageEffectFlag, d.DamageEffectValue);
    if (!this.IsLocal && GameState.LocalCharacter != null)
    {
      if (this._state.Info.TeamID == TeamID.NONE || this._state.Info.TeamID != GameState.LocalCharacter.TeamID)
        Singleton<ReticleHud>.Instance.EnableEnemyReticle();
      if (this._state.Info.TeamID == TeamID.NONE || this._state.Info.TeamID != GameState.LocalCharacter.TeamID)
        this.ShowDamageFeedback(d);
    }
    this.PlayDamageSound();
  }

  public virtual void ApplyForce(Vector3 position, Vector3 force)
  {
    if (this.IsLocal)
      GameState.LocalPlayer.MoveController.ApplyForce(force, CharacterMoveController.ForceType.Additive);
    else
      GameState.CurrentGame.SendPlayerHitFeedback(this.ActorID, force);
  }

  public float WalkingSoundSpeed => this._walkingSoundSpeed;

  private void SetAvatarDecorator(AvatarDecorator decorator)
  {
    this.Decorator = decorator;
    decorator.renderer.receiveShadows = false;
    decorator.renderer.castShadows = true;
    decorator.transform.parent = this.transform;
    decorator.SetPosition(new Vector3(0.0f, -0.98f, 0.0f), Quaternion.identity);
    decorator.HudInformation.SetCharacterInfo(this._state.Info);
    decorator.SetFootStep(!GameState.HasCurrentSpace ? FootStepSoundType.Rock : GameState.CurrentSpace.DefaultFootStep);
    decorator.SetSkinColor(this._state.Info.SkinColor);
    decorator.SetLayers(!this.IsLocal ? UberstrikeLayer.RemotePlayer : UberstrikeLayer.LocalPlayer);
    this.WeaponSimulator.SetAvatarDecorator(decorator);
    this.WeaponSimulator.UpdateWeapons((int) this._state.Info.CurrentWeaponSlot, (IList<int>) this._state.Info.Weapons.ItemIDs, (IList<int>) this._state.Info.QuickItems);
    this.WeaponSimulator.UpdateWeaponSlot((int) this._state.Info.CurrentWeaponSlot, this._isLocalPlayer);
    this.gameObject.name = string.Format("Player{0}_{1}", (object) this._state.Info.ActorId, (object) this._state.Info.PlayerName);
    foreach (CharacterHitArea hitArea in decorator.HitAreas)
    {
      if ((bool) (UnityEngine.Object) hitArea)
        hitArea.Shootable = (IShootable) this;
    }
    if (!GameState.HasCurrentGame)
      return;
    GameState.CurrentGame.ChangePlayerOutline(this);
  }

  public void AddFollowCamera() => this.MoveSimulator.AddPositionObserver((IObserver) LevelCamera.Instance);

  public void RemoveFollowCamera() => this.MoveSimulator.RemovePositionObserver();

  internal void Destroy()
  {
    try
    {
      Singleton<ProjectileManager>.Instance.RemoveAllProjectilesFromPlayer(this.State.PlayerNumber);
      Singleton<QuickItemSfxController>.Instance.DestroytSfxFromPlayer(this.State.PlayerNumber);
      this._state.UnSubscribeAll();
      if ((bool) (UnityEngine.Object) this.Decorator)
      {
        this.Decorator.DestroyCurrentRagdoll();
        if (this.IsLocal)
          this.Decorator.transform.parent = (Transform) null;
      }
      AvatarBuilder.Destroy(this.gameObject);
    }
    catch
    {
      Debug.LogWarning((object) "Character already destroyed");
    }
  }

  private void PlayDamageSound()
  {
    if (!this.IsLocal)
      return;
    if (this._state.Info.Armor.HasArmor && this._state.Info.Armor.HasArmorPoints)
      SfxManager.Play2dAudioClip(GameAudio.LocalPlayerHitArmorRemaining);
    else if (this._state.Info.Health < (short) 25)
      SfxManager.Play2dAudioClip(GameAudio.LocalPlayerHitNoArmorLowHealth);
    else
      SfxManager.Play2dAudioClip(GameAudio.LocalPlayerHitNoArmor);
  }

  private void ShowDamageFeedback(DamageInfo shot)
  {
    PlayerDamageEffect playerDamageEffect = UnityEngine.Object.Instantiate((UnityEngine.Object) this._damageFeedback, shot.Hitpoint, (double) shot.Force.magnitude <= 0.0 ? Quaternion.identity : Quaternion.LookRotation(shot.Force)) as PlayerDamageEffect;
    if (!(bool) (UnityEngine.Object) playerDamageEffect)
      return;
    playerDamageEffect.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    playerDamageEffect.Show(shot);
  }

  public bool IsLocal => this._isLocalPlayer;

  public bool IsVulnerable => (double) this._graceTimeAfterSpawn <= 0.0;

  public bool IsAnimationEnabled { get; set; }

  public float TimeLastGrounded { get; private set; }

  public AvatarDecorator Decorator { get; private set; }

  public CharacterMoveSimulator MoveSimulator { get; private set; }

  public WeaponSimulator WeaponSimulator { get; private set; }

  public CharacterStateAnimationController StateController { get; private set; }

  public int ActorID => this.State != null ? this.State.ActorId : 0;

  public TeamID Team => this.State != null ? this.State.TeamID : TeamID.NONE;

  public UberStrike.Realtime.UnitySdk.CharacterInfo State => this._state != null ? this._state.Info : (UberStrike.Realtime.UnitySdk.CharacterInfo) null;

  public CharacterTrigger AimTrigger => this._aimTrigger;
}
