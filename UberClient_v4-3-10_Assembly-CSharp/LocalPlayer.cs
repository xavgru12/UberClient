// Decompiled with JetBrains decompiler
// Type: LocalPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class LocalPlayer : MonoBehaviour
{
  private const float RundownThreshold = -300f;
  public const float Threshold = 0.5f;
  [SerializeField]
  private Transform _cameraTarget;
  [SerializeField]
  private Transform _characterBase;
  [SerializeField]
  private Transform _firstPersonView;
  [SerializeField]
  private Transform _weaponAttachPoint;
  [SerializeField]
  private WeaponCamera _weaponCamera;
  protected PlayerHudState _weaponControlState;
  private CharacterMoveController _moveController;
  private CharacterConfig _currentCharacter;
  private LocalPlayer.PlayerState _controlState;
  private Quaternion _viewPointRotation = Quaternion.identity;
  private bool _isWalkingEnabled = true;
  private bool _isShootingEnabled = true;
  private bool? _isPaused;
  private bool _isQuitting;
  private float _damageFactor;
  private float _damageFactorDuration;
  private float _lastGrounded;
  public static readonly Vector3 EyePosition = new Vector3(0.0f, -0.1f, 0.0f);

  private void Awake() => this.Initialize();

  private void Start()
  {
    if (!this.gameObject.activeSelf)
      return;
    this.gameObject.SetActive(false);
  }

  internal void Initialize()
  {
    this._moveController = new CharacterMoveController(this.GetComponent<CharacterController>(), this._characterBase);
    this._moveController.CharacterLanded += new Action<float>(this.OnCharacterGrounded);
  }

  private void OnEnable()
  {
    this._moveController.Init();
    if (HudController.Exists)
      HudController.Instance.enabled = true;
    this.StartCoroutine(this.StartPlayerIdentification());
    this.StartCoroutine(this.StartUpdatePlayerPingTime(5));
  }

  private void OnApplicationQuit() => this._isQuitting = true;

  private void OnDisable()
  {
    if (this._isQuitting)
      return;
    this._isPaused = new bool?();
    Screen.lockCursor = false;
    AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
    if (HudController.Exists)
      HudController.Instance.enabled = false;
    if ((UnityEngine.Object) GlobalUIRibbon.Instance != (UnityEngine.Object) null)
      GlobalUIRibbon.Instance.Show();
    this.IsInitialized = false;
  }

  private void OnGUI()
  {
    GUI.depth = 100;
    if (!GameState.HasCurrentPlayer || !GameState.HasCurrentGame || Screen.lockCursor || this.IsGamePaused)
      return;
    this.Pause();
    GlobalUIRibbon.Instance.Show();
    Singleton<InGameChatHud>.Instance.Pause();
  }

  private void FixedUpdate()
  {
    if (this._moveController == null || !GameState.HasCurrentPlayer)
      return;
    this._moveController.UpdatePlayerMovement();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape) && Singleton<GameStateController>.Instance.StateMachine.CurrentStateId != 13)
    {
      if (!this.IsGamePaused)
        this.Pause();
      if ((UnityEngine.Object) GlobalUIRibbon.Instance != (UnityEngine.Object) null)
        GlobalUIRibbon.Instance.Show();
    }
    else if (!Singleton<InGameChatHud>.Instance.CanInput && Input.GetKeyDown(KeyCode.Backspace))
    {
      if (!this.IsGamePaused)
        this.Pause();
      if ((UnityEngine.Object) GlobalUIRibbon.Instance != (UnityEngine.Object) null)
        GlobalUIRibbon.Instance.Show();
    }
    if (AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled)
    {
      if (Screen.lockCursor)
        UserInput.UpdateMouse();
      if (GameState.LocalCharacter.IsAlive)
        UserInput.UpdateDirections();
      if (!GameState.HasCurrentPlayer)
        return;
      this._cameraTarget.localPosition = Vector3.Lerp(this._cameraTarget.localPosition, GameState.LocalCharacter.CurrentOffset, 10f * Time.deltaTime);
      this.DoCameraBob();
      if (this.IsMouseLockStateConsistent)
        this.UpdateRotation();
      if ((double) this._damageFactor == 0.0)
        return;
      if ((double) this._damageFactorDuration > 0.0)
        this._damageFactorDuration -= Time.deltaTime;
      if ((double) this._damageFactorDuration > 0.0 && GameState.LocalCharacter.IsAlive)
        return;
      this._damageFactor = 0.0f;
      this._damageFactorDuration = 0.0f;
    }
    else
      UserInput.ResetDirection();
  }

  private void LateUpdate() => Singleton<WeaponController>.Instance.LateUpdate();

  private void UpdateRotation()
  {
    this._cameraTarget.localRotation = this._viewPointRotation * UserInput.Rotation;
    GameState.LocalCharacter.HorizontalRotation = UserInput.Rotation;
    GameState.LocalCharacter.VerticalRotation = (float) (((double) UserInput.Mouse.y + 90.0) / 180.0);
  }

  [DebuggerHidden]
  private IEnumerator StartPlayerIdentification() => (IEnumerator) new LocalPlayer.\u003CStartPlayerIdentification\u003Ec__Iterator7D()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartUpdatePlayerPingTime(int sec) => (IEnumerator) new LocalPlayer.\u003CStartUpdatePlayerPingTime\u003Ec__Iterator7E()
  {
    sec = sec,
    \u003C\u0024\u003Esec = sec
  };

  private void OnCharacterGrounded(float velocity)
  {
    if (!GameState.HasCurrentGame || !GameState.CurrentGame.IsMatchRunning || LevelCamera.Instance.CurrentBob != BobMode.None || (double) this._lastGrounded + 0.5 >= (double) Time.time || GameState.LocalCharacter.Is(PlayerStates.DIVING))
      return;
    this._lastGrounded = Time.time;
    if (!(bool) (UnityEngine.Object) this._currentCharacter || !(bool) (UnityEngine.Object) this._currentCharacter.Decorator)
      return;
    this._currentCharacter.Decorator.PlayFootSound(this._currentCharacter.WalkingSoundSpeed);
    if ((double) velocity < -20.0)
    {
      LevelCamera.Instance.DoLandFeedback(true);
      SfxManager.Play2dAudioClip(GameAudio.LandingGrunt);
    }
    else
      LevelCamera.Instance.DoLandFeedback(false);
  }

  private void DoCameraBob()
  {
    switch (GameState.LocalCharacter.PlayerState)
    {
      case PlayerStates.IDLE:
        if (UserInput.IsWalking && (double) this._moveController.CurrentVelocity.y >= -300.0)
          break;
        LevelCamera.SetBobMode(BobMode.None);
        break;
      case PlayerStates.GROUNDED:
        if (UserInput.IsWalking)
        {
          if (Singleton<WeaponController>.Instance.IsSecondaryAction)
          {
            LevelCamera.SetBobMode(BobMode.None);
            break;
          }
          LevelCamera.SetBobMode(BobMode.Run);
          break;
        }
        if (Singleton<WeaponController>.Instance.IsSecondaryAction)
        {
          LevelCamera.SetBobMode(BobMode.None);
          break;
        }
        if (UserInput.IsWalking && (double) this._moveController.CurrentVelocity.y >= -300.0)
          break;
        LevelCamera.SetBobMode(BobMode.Idle);
        break;
      case PlayerStates.JUMPING:
        LevelCamera.SetBobMode(BobMode.None);
        break;
      case PlayerStates.FLYING:
        LevelCamera.SetBobMode(BobMode.Fly);
        break;
      case PlayerStates.GROUNDED | PlayerStates.DUCKED:
        if (UserInput.IsWalking)
        {
          if (Singleton<WeaponController>.Instance.IsSecondaryAction)
          {
            LevelCamera.SetBobMode(BobMode.None);
            break;
          }
          LevelCamera.SetBobMode(BobMode.Crouch);
          break;
        }
        if (Singleton<WeaponController>.Instance.IsSecondaryAction)
        {
          LevelCamera.SetBobMode(BobMode.None);
          break;
        }
        LevelCamera.SetBobMode(BobMode.Idle);
        break;
      case PlayerStates.SWIMMING:
        LevelCamera.SetBobMode(BobMode.Swim);
        break;
      default:
        if (UserInput.IsWalking && (double) this._moveController.CurrentVelocity.y >= -300.0)
          break;
        LevelCamera.SetBobMode(BobMode.None);
        break;
    }
  }

  public bool IsInitialized { get; private set; }

  public void InitializePlayer()
  {
    this.IsInitialized = true;
    try
    {
      if ((UnityEngine.Object) LevelCamera.Instance != (UnityEngine.Object) null)
      {
        LevelCamera.SetBobMode(BobMode.None);
        LevelCamera.Instance.CanDip = true;
        LevelCamera.Instance.IsZoomedIn = false;
        LevelCamera.Instance.EnableLowPassFilter(false);
      }
      if (GameState.LocalCharacter != null)
      {
        GameState.LocalCharacter.ResetState();
        if (HudAssets.Exists)
        {
          Singleton<HpApHud>.Instance.AP = GameState.LocalCharacter.Armor.ArmorPoints;
          Singleton<HpApHud>.Instance.HP = (int) GameState.LocalCharacter.Health;
        }
        Singleton<WeaponController>.Instance.InitializeAllWeapons(this._weaponAttachPoint);
        this.UpdateLocalCharacterLoadout();
        this.UpdateRotation();
      }
      else
        UnityEngine.Debug.LogError((object) "CurrentPlayer is null!");
      if ((UnityEngine.Object) this.Decorator != (UnityEngine.Object) null)
      {
        this.SetPlayerControlState(LocalPlayer.PlayerState.FirstPerson, this.Decorator.Configuration);
        this.Decorator.DisableRagdoll();
        this.Decorator.UpdateLayers();
        this.Decorator.MeshRenderer.enabled = false;
        this.Decorator.HudInformation.enabled = false;
      }
      this.IsDead = false;
      this._moveController.Start();
      this._moveController.ResetDuckMode();
      Singleton<QuickItemController>.Instance.Reset();
      if (!PanelManager.IsAnyPanelOpen)
        this.UnPausePlayer();
      this.DamageFactor = 0.0f;
    }
    catch
    {
      UnityEngine.Debug.LogError((object) string.Format("InitializePlayer with {0}", (object) CmunePrint.Properties((object) this)));
      throw;
    }
  }

  public void UpdateLocalCharacterLoadout()
  {
    UberStrike.Realtime.UnitySdk.CharacterInfo localCharacter = GameState.LocalCharacter;
    localCharacter.Gear[0] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearHead);
    localCharacter.Gear[1] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearFace);
    localCharacter.Gear[2] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearGloves);
    localCharacter.Gear[3] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearUpperBody);
    localCharacter.Gear[4] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearLowerBody);
    localCharacter.Gear[5] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearBoots);
    localCharacter.Gear[6] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.GearHolo);
    localCharacter.QuickItems[0] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem1);
    localCharacter.QuickItems[1] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem2);
    localCharacter.QuickItems[2] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.QuickUseItem3);
    localCharacter.FunctionalItems[0] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem1);
    localCharacter.FunctionalItems[1] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem2);
    localCharacter.FunctionalItems[2] = Singleton<LoadoutManager>.Instance.GetItemIdOnSlot(LoadoutSlotType.FunctionalItem3);
    IUnityItem unityItem1 = Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponMelee).Item;
    IUnityItem unityItem2 = Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponPrimary).Item;
    IUnityItem unityItem3 = Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponSecondary).Item;
    IUnityItem unityItem4 = Singleton<LoadoutManager>.Instance.GetItemOnSlot(LoadoutSlotType.WeaponTertiary).Item;
    localCharacter.Weapons.SetWeaponSlot(WeaponInfo.SlotType.Melee, unityItem1 == null ? 0 : unityItem1.ItemId, unityItem1 == null ? (UberstrikeItemClass) 0 : unityItem1.ItemClass);
    localCharacter.Weapons.SetWeaponSlot(WeaponInfo.SlotType.Primary, unityItem2 == null ? 0 : unityItem2.ItemId, unityItem2 == null ? (UberstrikeItemClass) 0 : unityItem2.ItemClass);
    localCharacter.Weapons.SetWeaponSlot(WeaponInfo.SlotType.Secondary, unityItem3 == null ? 0 : unityItem3.ItemId, unityItem3 == null ? (UberstrikeItemClass) 0 : unityItem3.ItemClass);
    localCharacter.Weapons.SetWeaponSlot(WeaponInfo.SlotType.Tertiary, unityItem4 == null ? 0 : unityItem4.ItemId, unityItem4 == null ? (UberstrikeItemClass) 0 : unityItem4.ItemClass);
    localCharacter.Weapons.SetWeaponSlot(WeaponInfo.SlotType.Pickup, 0, (UberstrikeItemClass) 0);
    localCharacter.SkinColor = PlayerDataManager.SkinColor;
    int armorPoints = 0;
    int absorbtionRatio = 0;
    Singleton<LoadoutManager>.Instance.GetArmorValues(out armorPoints, out absorbtionRatio);
    localCharacter.Armor.AbsorbtionPercentage = (byte) absorbtionRatio;
    localCharacter.Armor.ArmorPointCapacity = armorPoints;
    localCharacter.Armor.ArmorPoints = armorPoints;
  }

  public void SetPlayerDead()
  {
    if (this.IsDead)
      return;
    this.IsDead = true;
    this.Killer = (AvatarDecoratorConfig) null;
    if (!Singleton<PlayerSpectatorControl>.Instance.IsEnabled)
      AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
    this.UpdateWeaponController();
    CmuneEventHandler.Route((object) new OnPlayerDeadEvent());
  }

  public void SpawnPlayerAt(Vector3 pos, Quaternion rot)
  {
    try
    {
      this.transform.position = pos + Vector3.up;
      this._cameraTarget.localRotation = rot;
      UserInput.SetRotation(rot.eulerAngles.y, 0.0f);
      if ((UnityEngine.Object) LevelCamera.Instance != (UnityEngine.Object) null)
        LevelCamera.Instance.ResetFeedback();
      if (GameState.HasCurrentGame)
        GameState.CurrentGame.SendPlayerSpawnPosition(pos);
      if (GameState.HasCurrentPlayer)
        GameState.LocalCharacter.Position = pos;
      this.MoveController.ResetEnviroment();
      this.MoveController.Platform = (MovingPlatform) null;
    }
    catch
    {
      UnityEngine.Debug.LogError((object) string.Format("SpawnPlayerAt with LocalPlayer {0}", (object) CmunePrint.Properties((object) GameState.LocalPlayer)));
      throw;
    }
  }

  public void SetCurrentCharacterConfig(CharacterConfig character) => this._currentCharacter = character;

  public void SetPlayerControlState(LocalPlayer.PlayerState s, AvatarDecoratorConfig decorator = null)
  {
    this._controlState = s;
    switch (this._controlState)
    {
      case LocalPlayer.PlayerState.FirstPerson:
        this._viewPointRotation = this._firstPersonView.localRotation;
        LevelCamera.Instance.SetTarget(this._cameraTarget);
        LevelCamera.Instance.SetMode(LevelCamera.CameraMode.FirstPerson);
        LevelCamera.Instance.SetEyePosition(LocalPlayer.EyePosition.x, LocalPlayer.EyePosition.y, LocalPlayer.EyePosition.z);
        if (LevelCamera.HasCamera)
        {
          LevelCamera.Instance.MainCamera.transform.localPosition = Vector3.zero;
          LevelCamera.Instance.MainCamera.transform.localRotation = Quaternion.identity;
        }
        if (!(bool) (UnityEngine.Object) GameState.LocalPlayer.WeaponCamera)
          break;
        GameState.LocalPlayer.WeaponCamera.SetCameraEnabled(true);
        break;
      case LocalPlayer.PlayerState.ThirdPerson:
        this._viewPointRotation = this._firstPersonView.localRotation * Quaternion.Euler(10f, 0.0f, 0.0f);
        LevelCamera.Instance.SetTarget(this._cameraTarget);
        LevelCamera.Instance.SetMode(LevelCamera.CameraMode.ThirdPerson);
        LevelCamera.Instance.MainCamera.transform.localPosition = new Vector3(2f, 3f, 0.0f);
        LevelCamera.Instance.MainCamera.transform.localRotation = Quaternion.Euler(45f, 0.0f, 0.0f);
        if (!(bool) (UnityEngine.Object) GameState.LocalPlayer.WeaponCamera)
          break;
        GameState.LocalPlayer.WeaponCamera.SetCameraEnabled(false);
        break;
      case LocalPlayer.PlayerState.Death:
        LevelCamera.Instance.SetMode(LevelCamera.CameraMode.Ragdoll);
        LevelCamera.Instance.SetLookAtHeight(1f);
        if (!(bool) (UnityEngine.Object) GameState.LocalPlayer.WeaponCamera)
          break;
        GameState.LocalPlayer.WeaponCamera.SetCameraEnabled(false);
        break;
      case LocalPlayer.PlayerState.FreeMove:
        LevelCamera.Instance.SetLookAtHeight(0.0f);
        LevelCamera.Instance.SetMode(LevelCamera.CameraMode.Spectator);
        if (!(bool) (UnityEngine.Object) GameState.LocalPlayer.WeaponCamera)
          break;
        GameState.LocalPlayer.WeaponCamera.SetCameraEnabled(false);
        break;
      case LocalPlayer.PlayerState.Overview:
        LevelCamera.Instance.SetMode(LevelCamera.CameraMode.Overview);
        LevelCamera.Instance.SetLookAtHeight(1f);
        if ((UnityEngine.Object) GameState.LocalDecorator != (UnityEngine.Object) null)
        {
          GameState.LocalDecorator.SetLayers(UberstrikeLayer.RemotePlayer);
          GameState.LocalDecorator.MeshRenderer.enabled = true;
        }
        if (!(bool) (UnityEngine.Object) GameState.LocalPlayer.WeaponCamera)
          break;
        GameState.LocalPlayer.WeaponCamera.SetCameraEnabled(false);
        break;
      default:
        if ((bool) (UnityEngine.Object) Camera.main && (bool) (UnityEngine.Object) GameState.CurrentSpace && (bool) (UnityEngine.Object) GameState.CurrentSpace.DefaultViewPoint)
          Camera.main.transform.rotation = GameState.CurrentSpace.DefaultViewPoint.rotation;
        LevelCamera.Instance.SetMode(LevelCamera.CameraMode.None);
        if (!(bool) (UnityEngine.Object) GameState.LocalPlayer.WeaponCamera)
          break;
        GameState.LocalPlayer.WeaponCamera.SetCameraEnabled(false);
        break;
    }
  }

  public void Pause(bool force = false)
  {
    if (!force && this._isPaused.HasValue && this._isPaused.Value)
      return;
    this._isPaused = new bool?(true);
    AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
    LevelCamera.SetBobMode(BobMode.Idle);
    if (GameState.HasCurrentPlayer)
    {
      GameState.LocalCharacter.Keys = KeyState.Still;
      GameState.LocalCharacter.IsFiring = false;
    }
    if (GameState.HasCurrentGame && GameState.HasCurrentPlayer)
      Singleton<WeaponController>.Instance.StopInputHandler();
    Screen.lockCursor = false;
    this.UpdateWeaponController();
    if (!GameState.HasCurrentGame)
      return;
    CmuneEventHandler.Route((object) new OnPlayerPauseEvent());
  }

  public void UnPausePlayer()
  {
    PopupSystem.ClearAll();
    this._isPaused = new bool?(false);
    AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = true;
    Screen.lockCursor = true;
    if ((UnityEngine.Object) GlobalUIRibbon.Instance != (UnityEngine.Object) null)
      GlobalUIRibbon.Instance.Hide();
    this.UpdateWeaponController();
    if (!GameState.HasCurrentGame)
      return;
    CmuneEventHandler.Route((object) new OnPlayerUnpauseEvent());
  }

  public void SetWeaponControlState(PlayerHudState state)
  {
    this._weaponControlState = state;
    this.UpdateWeaponController();
  }

  public void UpdateWeaponController()
  {
    switch (this._weaponControlState)
    {
      case PlayerHudState.None:
      case PlayerHudState.Spectating:
      case PlayerHudState.AfterRound:
        Singleton<WeaponController>.Instance.IsEnabled = false;
        break;
      case PlayerHudState.Playing:
        Singleton<WeaponController>.Instance.IsEnabled = !this.IsGamePaused && GameState.LocalCharacter.IsAlive;
        break;
    }
  }

  public CharacterConfig Character => this._currentCharacter;

  public AvatarDecorator Decorator => (bool) (UnityEngine.Object) this._currentCharacter ? this._currentCharacter.Decorator : (AvatarDecorator) null;

  public AvatarDecoratorConfig Killer { get; set; }

  public bool IsGamePaused
  {
    get
    {
      bool? isPaused = this._isPaused;
      return isPaused.HasValue && isPaused.Value;
    }
  }

  public float DamageFactor
  {
    get => this._damageFactor;
    set
    {
      this._damageFactor = Mathf.Clamp01(value);
      this._damageFactorDuration = this._damageFactor * 15f;
      this._damageFactor /= 0.15f;
    }
  }

  public bool IsPlayerRespawned { get; set; }

  public void SetEnabled(bool enabled) => this.gameObject.SetActive(enabled);

  public bool IsMouseLockStateConsistent => Screen.lockCursor;

  public bool IsWalkingEnabled
  {
    get => this._isWalkingEnabled && AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled;
    set => this._isWalkingEnabled = value;
  }

  public bool IsShootingEnabled
  {
    get => this._isShootingEnabled && AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled;
    set
    {
      this._isShootingEnabled = value;
      Singleton<WeaponController>.Instance.IsEnabled = value;
    }
  }

  public LocalPlayer.PlayerState CurrentCameraControl => this._controlState;

  public CharacterMoveController MoveController => this._moveController;

  public WeaponCamera WeaponCamera => this._weaponCamera;

  public Transform WeaponAttachPoint => this._weaponAttachPoint;

  public bool IsDead { get; private set; }

  public enum PlayerState
  {
    None,
    FirstPerson,
    ThirdPerson,
    Death,
    FreeMove,
    Overview,
  }
}
