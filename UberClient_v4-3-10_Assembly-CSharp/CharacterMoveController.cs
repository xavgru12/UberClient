// Decompiled with JetBrains decompiler
// Type: CharacterMoveController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class CharacterMoveController
{
  public const float POWERUP_HASTE_SCALE = 1.3f;
  public const float PLAYER_WADE_SCALE = 0.8f;
  public const float PLAYER_SWIM_SCALE = 0.6f;
  public const float PLAYER_DUCK_SCALE = 0.7f;
  public const float PLAYER_TERMINAL_GRAVITY = -100f;
  public const float PLAYER_INITIAL_GRAVITY = -1f;
  public const float PLAYER_ZOOM_SCALE = 0.7f;
  public const float PLAYER_MIN_SCALE = 0.5f;
  public const float PLAYER_IRON_SIGHT = 1f;
  public bool IsLowGravity;
  private readonly CharacterController _controller;
  private readonly PlayerAttributes _attributes;
  private readonly Transform _transform;
  private readonly Transform _playerBase;
  private MovingPlatform _platform;
  private EnviromentSettings _currentEnviroment;
  private CollisionFlags _collisionFlag;
  private Vector3 _currentVelocity;
  private Vector3 _acceleration;
  private bool _isOnLatter;
  private bool _isGrounded = true;
  private bool _canJump = true;
  private int _ungroundedCount;
  private int _waterLevel;
  private float _waterEnclosure;
  private CharacterMoveController.ForceType _forceType;
  private Vector3 _externalForce;
  private bool _hasExternalForce;

  public CharacterMoveController(CharacterController controller, Transform characterBase)
  {
    this._controller = controller;
    this._transform = this._controller.transform;
    this._attributes = new PlayerAttributes();
    this._playerBase = characterBase;
    CmuneEventHandler.AddListener<InputChangeEvent>(new Action<InputChangeEvent>(this.OnInputChanged));
  }

  public event Action<float> CharacterLanded;

  public bool IsJumpDisabled { get; set; }

  public float DamageSlowDown { get; set; }

  public float PlayerHeight => GameState.LocalCharacter.Is(PlayerStates.DUCKED) ? 0.9f : 1.6f;

  public Vector3 Velocity => this._currentVelocity;

  public float SpeedModifier
  {
    get
    {
      float b = (float) (1.0 * (double) Mathf.Min(1f, !Singleton<WeaponController>.Instance.IsSecondaryAction ? 1f : 0.7f, !WeaponFeedbackManager.Instance.IsIronSighted ? 1f : 1f, (float) (1.0 - (double) Mathf.Clamp01(Singleton<PlayerDataManager>.Instance.GearWeight) * 0.30000001192092896), (float) (1.0 - (double) Mathf.Clamp01(GameState.LocalPlayer.DamageFactor) * 0.30000001192092896)));
      if (this.WaterLevel > 0)
      {
        if (this.WaterLevel == 3)
          b *= 0.6f;
        else
          b *= 0.8f;
      }
      else if (this.IsGrounded && GameState.LocalCharacter.Is(PlayerStates.DUCKED))
        b *= 0.7f;
      return Mathf.Max(0.5f, b);
    }
  }

  public MovingPlatform Platform
  {
    get => this._platform;
    set => this._platform = value;
  }

  public Vector3 CurrentVelocity => this._currentVelocity;

  public int WaterLevel
  {
    get => this._waterLevel;
    private set => this._waterLevel = value;
  }

  public bool IsGrounded
  {
    get => this._isGrounded;
    private set => this._isGrounded = value;
  }

  public CharacterController DebugController => this._controller;

  public UnityEngine.Bounds DebugEnvBounds => this._currentEnviroment.EnviromentBounds;

  public void Init()
  {
    if ((UnityEngine.Object) LevelEnviroment.Instance != (UnityEngine.Object) null)
      this._currentEnviroment = LevelEnviroment.Instance.Settings;
    else
      Debug.LogWarning((object) "You are trying to access the LevelEnvironment Instance that has not had Awake called.");
  }

  public void Start() => this.Reset();

  public void UpdatePlayerMovement()
  {
    if (!GameState.HasCurrentPlayer)
      return;
    this.UpdateMovementStates();
    this.UpdateMovement();
  }

  public void ResetDuckMode()
  {
    this._controller.height = 1.6f;
    this._controller.center = new Vector3(0.0f, 0.0f, 0.0f);
  }

  public static bool HasCollision(Vector3 pos, float height) => Physics.CheckSphere(pos + Vector3.up * (height - 0.5f), 0.6f, UberstrikeLayerMasks.CrouchMask);

  public void ApplyForce(Vector3 v, CharacterMoveController.ForceType type)
  {
    this._hasExternalForce = true;
    this._externalForce = v;
    this._forceType = type;
  }

  public void ClearForce() => this._externalForce = Vector3.zero;

  public void ResetEnviroment()
  {
    this._currentEnviroment = LevelEnviroment.Instance.Settings;
    this._currentEnviroment.EnviromentBounds = new UnityEngine.Bounds();
    this._isOnLatter = false;
  }

  public void SetEnviroment(EnviromentSettings settings, UnityEngine.Bounds bounds)
  {
    this._currentEnviroment = settings;
    this._currentEnviroment.EnviromentBounds = new UnityEngine.Bounds(bounds.center, bounds.size);
    this._isOnLatter = this._currentEnviroment.Type == EnviromentSettings.TYPE.LATTER;
  }

  private void UpdateMovementStates()
  {
    if (this._currentEnviroment.Type == EnviromentSettings.TYPE.LATTER && !this._currentEnviroment.EnviromentBounds.Intersects(this._controller.bounds))
      this.ResetEnviroment();
    if (this._currentEnviroment.Type == EnviromentSettings.TYPE.WATER)
    {
      this._currentEnviroment.CheckPlayerEnclosure(this._playerBase.position, this.PlayerHeight, out this._waterEnclosure);
      int level = 1;
      if ((double) this._waterEnclosure >= 0.800000011920929)
        level = 3;
      else if ((double) this._waterEnclosure >= 0.40000000596046448)
        level = 2;
      if (this.WaterLevel != level)
        this.SetWaterlevel(level);
    }
    else if (this.WaterLevel != 0)
      this.SetWaterlevel(0);
    if ((GameState.LocalCharacter.Keys & KeyState.Jump) == KeyState.Still)
      this._canJump = true;
    if (!GameState.LocalCharacter.Is(PlayerStates.GROUNDED | PlayerStates.JUMPING))
      return;
    GameState.LocalCharacter.Set(PlayerStates.JUMPING, false);
  }

  private void UpdateMovement()
  {
    this.CheckDuck();
    if (GameState.LocalCharacter.Is(PlayerStates.FLYING))
      this.FlyInAir();
    else if (this.WaterLevel > 2)
      this.MoveInWater();
    else if (this._isOnLatter)
      this.MoveOnLatter();
    else if (this.IsGrounded)
      this.MoveOnGround();
    else if (this.WaterLevel == 2)
      this.MoveOnWaterRim();
    else
      this.MoveInAir();
    if (this._hasExternalForce)
    {
      switch (this._forceType)
      {
        case CharacterMoveController.ForceType.Additive:
          this._currentVelocity = Vector3.Scale(this._currentVelocity, new Vector3(1f, 0.5f, 1f)) + this._externalForce * 0.035f;
          break;
        case CharacterMoveController.ForceType.Exclusive:
          this._currentVelocity = this._externalForce * 0.035f;
          break;
      }
      this._externalForce *= 0.0f;
      this._hasExternalForce = false;
      GameState.LocalCharacter.Set(PlayerStates.JUMPING, true);
    }
    Vector3 vector3;
    if (this.IsGrounded && (bool) (UnityEngine.Object) this.Platform)
    {
      vector3 = this.Platform.GetMovementDelta();
      if ((double) vector3.y > 0.0)
        this._currentVelocity.y = 0.0f;
    }
    else
      vector3 = Vector3.zero;
    this._currentVelocity[1] = Mathf.Clamp(this._currentVelocity[1], -150f, 150f);
    this._collisionFlag = this._controller.Move(this._currentVelocity * Time.deltaTime);
    if (this.IsGrounded && (bool) (UnityEngine.Object) this.Platform)
      this._transform.localPosition += vector3;
    this._currentVelocity = this._controller.velocity;
    if ((this._collisionFlag & CollisionFlags.Below) != CollisionFlags.None)
    {
      if (this._ungroundedCount > 5 && this.CharacterLanded != null)
        this.CharacterLanded(this._currentVelocity.y);
      this._ungroundedCount = 0;
      this.IsGrounded = true;
    }
    else if (GameState.LocalCharacter.Is(PlayerStates.JUMPING))
    {
      ++this._ungroundedCount;
      this.IsGrounded = false;
    }
    else if (this._ungroundedCount > 5)
    {
      this.IsGrounded = false;
    }
    else
    {
      ++this._ungroundedCount;
      this.IsGrounded = true;
    }
    GameState.LocalCharacter.Set(PlayerStates.GROUNDED, this.IsGrounded);
    GameState.LocalCharacter.Position = this._controller.transform.position;
  }

  private void OnInputChanged(InputChangeEvent ev)
  {
    if (GameState.LocalCharacter == null || !GameState.LocalPlayer.IsWalkingEnabled)
      return;
    if (ev.IsDown)
      GameState.LocalCharacter.Keys |= UserInput.GetkeyState(ev.Key);
    else
      GameState.LocalCharacter.Keys &= ~UserInput.GetkeyState(ev.Key);
  }

  private void Reset()
  {
    this.SetWaterlevel(0);
    this._currentVelocity = Vector3.zero;
    this._forceType = CharacterMoveController.ForceType.Additive;
    this._hasExternalForce = false;
    this._externalForce = Vector3.zero;
    this._canJump = true;
    this._isGrounded = true;
    this._ungroundedCount = 0;
    this._platform = (MovingPlatform) null;
    this.IsJumpDisabled = false;
  }

  private void ApplyFriction()
  {
    float magnitude = this._currentVelocity.magnitude;
    if ((double) magnitude == 0.0)
      return;
    if ((double) magnitude < 0.5 && (double) this._acceleration.sqrMagnitude == 0.0)
    {
      if (this._isOnLatter)
        this._currentVelocity[1] = 0.0f;
      this._currentVelocity[0] = 0.0f;
      this._currentVelocity[2] = 0.0f;
    }
    else
    {
      float num1 = 0.0f;
      if (this.WaterLevel < 3)
      {
        if (this._isOnLatter || GameState.LocalCharacter.Is(PlayerStates.GROUNDED))
        {
          float num2 = Mathf.Max(this._currentEnviroment.StopSpeed, magnitude);
          num1 += num2 * this._currentEnviroment.GroundFriction;
        }
      }
      else if (this.WaterLevel > 0)
        num1 += (float) ((double) Mathf.Max(this._currentEnviroment.StopSpeed, magnitude) * (double) this._currentEnviroment.WaterFriction * (double) this.WaterLevel / 3.0);
      if (GameState.LocalCharacter.Is(PlayerStates.FLYING))
      {
        float num3 = Mathf.Max(this._currentEnviroment.StopSpeed, magnitude);
        num1 += num3 * this._currentEnviroment.FlyFriction;
      }
      float num4 = num1 * Time.deltaTime;
      float num5 = magnitude - num4;
      if ((double) num5 < 0.0)
        num5 = 0.0f;
      this._currentVelocity *= num5 / magnitude;
    }
  }

  private void ApplyAcceleration(Vector3 wishdir, float wishspeed, float accel, bool clamp = false)
  {
    float num = Vector3.Dot(this._currentVelocity, wishdir);
    if ((double) (wishspeed - num) <= 0.0)
    {
      this._acceleration = Vector3.zero;
    }
    else
    {
      this._acceleration = accel * wishspeed * wishdir * Time.deltaTime;
      Vector3 vector3 = this._currentVelocity + this._acceleration;
      float magnitude = vector3.magnitude;
      if ((double) magnitude < (double) wishspeed)
        this._currentVelocity += this._acceleration;
      else if (clamp)
      {
        this._currentVelocity = (this._currentVelocity + this._acceleration).normalized * wishspeed;
        this._currentVelocity[1] = vector3[1];
      }
      else
        this._currentVelocity = (this._currentVelocity + this._acceleration).normalized * magnitude;
    }
  }

  private void CheckDuck()
  {
    if (this.WaterLevel >= 3 || !GameState.HasCurrentPlayer || GameState.LocalCharacter.Is(PlayerStates.JUMPING) || GameState.LocalCharacter.Is(PlayerStates.FLYING))
      return;
    if (UserInput.IsPressed(KeyState.Crouch) && !GameState.LocalCharacter.Is(PlayerStates.DUCKED))
    {
      GameState.LocalCharacter.Set(PlayerStates.DUCKED, true);
      this._controller.height = 0.9f;
      this._controller.center = new Vector3(0.0f, -0.4f, 0.0f);
    }
    else
    {
      if (UserInput.IsPressed(KeyState.Crouch) || !GameState.LocalCharacter.Is(PlayerStates.DUCKED) || CharacterMoveController.HasCollision(this._playerBase.position, 1.6f))
        return;
      GameState.LocalCharacter.Set(PlayerStates.DUCKED, false);
      this._controller.height = 1.6f;
      this._controller.center = new Vector3(0.0f, -0.1f, 0.0f);
    }
  }

  private bool CheckJump()
  {
    if (this.IsJumpDisabled || GameState.LocalCharacter.Is(PlayerStates.DUCKED) || (GameState.LocalCharacter.Keys & KeyState.Jump) == KeyState.Still)
      return false;
    if (this._isOnLatter)
      return true;
    if (this._canJump)
    {
      this._canJump = false;
      GameState.LocalCharacter.Set(PlayerStates.GROUNDED, false);
      GameState.LocalCharacter.Set(PlayerStates.JUMPING, true);
      this._currentVelocity.y = this._attributes.JumpForce;
      return true;
    }
    UserInput.HorizontalDirection.y = 0.0f;
    return false;
  }

  private bool CheckWaterJump()
  {
    if ((GameState.LocalCharacter.Keys & KeyState.Jump) == KeyState.Still || (this._collisionFlag & CollisionFlags.Sides) == CollisionFlags.None)
      return false;
    if (!this._canJump)
    {
      UserInput.HorizontalDirection.y = 0.0f;
      return false;
    }
    GameState.LocalCharacter.Set(PlayerStates.JUMPING, true);
    this._currentVelocity.y = this._attributes.JumpForce;
    return true;
  }

  private void FlyInAir()
  {
    this.ApplyFriction();
    Vector3 wishdir = Vector3.zero;
    if (UserInput.IsWalking)
      wishdir = UserInput.Rotation * UserInput.HorizontalDirection;
    if ((double) UserInput.VerticalDirection.y != 0.0)
      wishdir.y = UserInput.VerticalDirection.y;
    this.ApplyAcceleration(wishdir, this._attributes.Speed, this._currentEnviroment.FlyAcceleration);
  }

  private void MoveInWater()
  {
    this.ApplyFriction();
    Vector3 wishdir = Vector3.zero;
    if (UserInput.IsWalking)
      wishdir = UserInput.Rotation * UserInput.HorizontalDirection;
    if (UserInput.IsMovingVertically)
      wishdir.y = UserInput.VerticalDirection.y;
    this.ApplyAcceleration(wishdir, this._attributes.Speed * this.SpeedModifier, this._currentEnviroment.WaterAcceleration);
    if ((double) this._currentVelocity[1] > -3.0)
      this._currentVelocity[1] -= this.Gravity * 0.1f;
    else
      this._currentVelocity[1] = Mathf.Lerp(this._currentVelocity[1], -3f, Time.deltaTime * 6f);
  }

  private void MoveOnLatter()
  {
    this.ApplyFriction();
    Vector3 wishdir = Vector3.zero;
    if (UserInput.IsWalking)
      wishdir = UserInput.Rotation * UserInput.HorizontalDirection;
    if (UserInput.IsMovingVertically)
      wishdir.y = UserInput.VerticalDirection.y;
    this.ApplyAcceleration(wishdir, this._attributes.Speed * this.SpeedModifier, this._currentEnviroment.GroundAcceleration);
  }

  private void MoveOnWaterRim()
  {
    this.ApplyFriction();
    Vector3 wishdir = Vector3.zero;
    if (UserInput.IsWalking)
      wishdir = UserInput.Rotation * UserInput.HorizontalDirection;
    wishdir.y = !UserInput.IsMovingDown ? (!UserInput.IsMovingUp || (double) this._waterEnclosure <= 0.800000011920929 ? 0.0f : UserInput.VerticalDirection.y * 0.5f) : UserInput.VerticalDirection.y;
    this.ApplyAcceleration(wishdir, this._attributes.Speed * this.SpeedModifier, this._currentEnviroment.WaterAcceleration, true);
    if ((double) this._waterEnclosure < 0.699999988079071 || !UserInput.IsMovingVertically)
    {
      if ((double) this._currentVelocity[1] > -3.0)
        this._currentVelocity[1] -= this.Gravity * 0.1f;
      else
        this._currentVelocity[1] = Mathf.Lerp(this._currentVelocity[1], -3f, Time.deltaTime * 6f);
    }
    else if ((double) this._currentVelocity[1] > 0.0 && (double) this._waterEnclosure < 0.800000011920929)
      this._currentVelocity[1] = Mathf.Lerp(this._currentVelocity[1], -1f, Time.deltaTime * 4f);
    this.CheckWaterJump();
  }

  private void MoveInAir()
  {
    this.ApplyFriction();
    Vector3 wishdir = UserInput.Rotation * UserInput.HorizontalDirection;
    wishdir[1] = 0.0f;
    this.ApplyAcceleration(wishdir, this._attributes.Speed, this._currentEnviroment.AirAcceleration);
    this._currentVelocity[1] -= this.Gravity;
  }

  private void MoveOnGround()
  {
    if (this.CheckJump())
    {
      if (this.WaterLevel > 1)
        this.MoveInWater();
      else
        this.MoveInAir();
    }
    else
    {
      this.ApplyFriction();
      Vector3 wishdir = Quaternion.Euler(0.0f, UserInput.Rotation.eulerAngles.y, 0.0f) * UserInput.HorizontalDirection;
      wishdir[1] = 0.0f;
      if ((double) wishdir.sqrMagnitude > 1.0)
        wishdir.Normalize();
      this.ApplyAcceleration(wishdir, this._attributes.Speed * this.SpeedModifier, this._currentEnviroment.GroundAcceleration);
      this._currentVelocity[1] = -this.Gravity;
    }
  }

  private void SetWaterlevel(int level)
  {
    this._waterLevel = level;
    if (GameState.HasCurrentPlayer)
    {
      GameState.LocalCharacter.Set(PlayerStates.DIVING, level == 3);
      GameState.LocalCharacter.Set(PlayerStates.SWIMMING, level == 2);
      GameState.LocalCharacter.Set(PlayerStates.WADING, level == 1);
    }
    else
      Debug.LogError((object) "Failed to set water level!");
  }

  private float Gravity => (!this.IsLowGravity ? 1f : 0.4f) * this._currentEnviroment.Gravity * Time.deltaTime;

  public enum ForceType
  {
    Additive,
    Exclusive,
  }
}
