// Decompiled with JetBrains decompiler
// Type: WeaponFeedbackManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Core.Models.Views;
using UnityEngine;

public class WeaponFeedbackManager : MonoBehaviour
{
  private WeaponFeedbackManager.WeaponBobManager _bobManager;
  private WeaponFeedbackManager.WeaponMode _currentWeaponMode;
  private WeaponFeedbackManager.WeaponState _pickupWeaponState;
  private WeaponFeedbackManager.WeaponState _putDownWeaponState;
  public WeaponFeedbackManager.FeedbackData WeaponDip;
  public WeaponFeedbackManager.FeedbackData WeaponFire;
  protected WeaponFeedbackManager.Feedback _fire;
  protected WeaponFeedbackManager.Feedback _dip;
  private bool _needLerp;
  public WeaponFeedbackManager.WeaponAnimData WeaponAnimation;
  private float _angleY;
  private float _angleX;
  private float _time;
  private float _sign;
  [SerializeField]
  private Transform _pivotPoint;
  private bool _isIronSight;
  private bool _isIronSightPosDone;

  public static WeaponFeedbackManager Instance { get; private set; }

  public static bool Exists => (UnityEngine.Object) WeaponFeedbackManager.Instance != (UnityEngine.Object) null;

  private void Awake()
  {
    WeaponFeedbackManager.Instance = this;
    this._bobManager = new WeaponFeedbackManager.WeaponBobManager();
  }

  private void OnEnable()
  {
    this._dip.Reset();
    this._fire.Reset();
    this.CurrentWeaponMode = WeaponFeedbackManager.WeaponMode.PutDown;
  }

  private void Update()
  {
    if (this._putDownWeaponState != null)
      this._putDownWeaponState.Update();
    if (this._pickupWeaponState == null)
      return;
    this._pickupWeaponState.Update();
  }

  private Quaternion CalculateBobDip()
  {
    if ((double) this._dip.time <= (double) this._dip.Duration)
      this._dip.HandleFeedback();
    else if (this._needLerp)
    {
      this._angleX = Mathf.Lerp(this._angleX, 0.0f, Time.deltaTime * 9f);
      this._angleY = Mathf.Lerp(this._angleY, 0.0f, Time.deltaTime * 9f);
      if ((double) this._angleX < 0.0099999997764825821 && (double) this._angleY < 0.0099999997764825821)
      {
        this._time = 0.0f;
        this._needLerp = false;
      }
    }
    else
    {
      float num = Mathf.Sin(this._bobManager.Data.Frequency * this._time);
      this._angleX = Mathf.Abs(this._bobManager.Data.XAmplitude * num);
      this._angleY = this._bobManager.Data.YAmplitude * num * this._sign;
      this._time += Time.deltaTime;
    }
    return Quaternion.Euler(this._angleX, this._angleY, 0.0f);
  }

  public void SetBobMode(BobMode mode)
  {
    if (this._bobManager.Mode == mode)
      return;
    this._bobManager.Mode = mode;
    if (mode == BobMode.Run)
    {
      this._needLerp = false;
      this._sign = !AutoMonoBehaviour<InputManager>.Instance.IsDown(GameInputKey.Right) ? 1f : -1f;
      this._time = Mathf.Asin(this._angleX / this._bobManager.Data.XAmplitude) / this._bobManager.Data.Frequency;
    }
    else
      this._needLerp = true;
  }

  public void LandingDip()
  {
    if ((double) this._fire.time > 0.0 && (double) this._fire.time < (double) this._fire.Duration || this.CurrentWeaponMode == WeaponFeedbackManager.WeaponMode.PutDown)
      return;
    this._dip.time = 0.0f;
    this._dip.angle = this.WeaponDip.angle;
    this._dip.noise = this.WeaponDip.noise;
    this._dip.strength = this.WeaponDip.strength;
    this._dip.timeToPeak = this.WeaponDip.timeToPeak;
    this._dip.timeToEnd = this.WeaponDip.timeToEnd;
    this._dip.direction = Vector3.down;
    this._dip.rotationAxis = Vector3.right;
  }

  public void Fire()
  {
    if (this.CurrentWeaponMode == WeaponFeedbackManager.WeaponMode.PutDown)
      return;
    this._fire.noise = this.WeaponFire.noise;
    this._fire.strength = this.WeaponFire.strength;
    this._fire.timeToPeak = this.WeaponFire.timeToPeak;
    this._fire.timeToEnd = this.WeaponFire.timeToEnd;
    this._fire.direction = Vector3.back;
    this._fire.rotationAxis = Vector3.left;
    this._fire.recoilTime = this.WeaponFire.recoilTime;
    if ((double) this._dip.time < (double) this._dip.Duration)
      this._dip.Reset();
    if ((double) this._fire.time > (double) this._fire.recoilTime && (double) this._fire.time < (double) this._fire.Duration)
    {
      this._fire.time = this.WeaponFire.timeToPeak / 3f;
      this._fire.angle = this.WeaponFire.angle / 3f;
    }
    else
    {
      if ((double) this._fire.time < (double) this._fire.Duration)
        return;
      this._fire.time = 0.0f;
      this._fire.angle = this.WeaponFire.angle;
    }
  }

  public void PutDown(bool destroy = false)
  {
    if (this._pickupWeaponState == null || !this._pickupWeaponState.IsValid)
      return;
    this.PutDownWeapon(this._pickupWeaponState.Weapon, this._pickupWeaponState.Decorator, destroy);
    this._pickupWeaponState = (WeaponFeedbackManager.WeaponState) null;
  }

  public void PickUp(BaseWeaponLogic weapon, BaseWeaponDecorator decorator)
  {
    if (this._pickupWeaponState != null && this._pickupWeaponState.IsValid)
    {
      if (this._pickupWeaponState.Weapon == weapon)
        return;
      this.PutDownWeapon(this._pickupWeaponState.Weapon, this._pickupWeaponState.Decorator);
    }
    else if (this._pickupWeaponState == null && this._putDownWeaponState != null && this._putDownWeaponState.Weapon == weapon)
      this._putDownWeaponState.Finish();
    this._pickupWeaponState = (WeaponFeedbackManager.WeaponState) new WeaponFeedbackManager.PickUpState(weapon, decorator);
    this.WeaponFire.recoilTime = WeaponConfigurationHelper.GetRateOfFire((UberStrikeItemWeaponView) weapon.Config);
    this.WeaponFire.strength = WeaponConfigurationHelper.GetRecoilMovement((UberStrikeItemWeaponView) weapon.Config);
    this.WeaponFire.angle = WeaponConfigurationHelper.GetRecoilKickback((UberStrikeItemWeaponView) weapon.Config);
  }

  public void BeginIronSight()
  {
    if (this._isIronSight)
      return;
    this._isIronSight = true;
  }

  public void EndIronSight() => this._isIronSight = false;

  public void ResetIronSight()
  {
    this._isIronSight = false;
    if (this._pickupWeaponState != null)
      this._pickupWeaponState.Reset();
    if (this._putDownWeaponState == null)
      return;
    this._putDownWeaponState.Reset();
  }

  public bool IsIronSighted => this._isIronSight;

  private void PutDownWeapon(BaseWeaponLogic weapon, BaseWeaponDecorator decorator, bool destroy = false)
  {
    if (this._putDownWeaponState != null)
      this._putDownWeaponState.Finish();
    this._putDownWeaponState = (WeaponFeedbackManager.WeaponState) new WeaponFeedbackManager.PutDownState(weapon, decorator, destroy);
  }

  public WeaponFeedbackManager.WeaponMode CurrentWeaponMode
  {
    get => this._currentWeaponMode;
    private set => this._currentWeaponMode = value;
  }

  public void SetFireFeedback(WeaponFeedbackManager.FeedbackData data)
  {
    this.WeaponFire.angle = data.angle;
    this.WeaponFire.noise = data.noise;
    this.WeaponFire.strength = data.strength;
    this.WeaponFire.timeToEnd = data.timeToEnd;
    this.WeaponFire.timeToPeak = data.timeToPeak;
    this.WeaponFire.recoilTime = data.recoilTime;
  }

  public bool _isWeaponInIronSightPosition => this._isIronSight && this._isIronSightPosDone;

  private class WeaponBobManager
  {
    private readonly Dictionary<BobMode, WeaponFeedbackManager.WeaponBobManager.BobData> _bobData;
    private BobMode _bobMode;
    private WeaponFeedbackManager.WeaponBobManager.BobData _data;

    public WeaponBobManager()
    {
      this._bobData = new Dictionary<BobMode, WeaponFeedbackManager.WeaponBobManager.BobData>();
      foreach (int num in Enum.GetValues(typeof (BobMode)))
      {
        BobMode key = (BobMode) num;
        switch (key)
        {
          case BobMode.Walk:
            this._bobData[key] = new WeaponFeedbackManager.WeaponBobManager.BobData(0.5f, 3f, 6f);
            continue;
          case BobMode.Run:
            this._bobData[key] = new WeaponFeedbackManager.WeaponBobManager.BobData(1f, 3f, 8f);
            continue;
          case BobMode.Crouch:
            this._bobData[key] = new WeaponFeedbackManager.WeaponBobManager.BobData(0.5f, 3f, 12f);
            continue;
          default:
            this._bobData[key] = new WeaponFeedbackManager.WeaponBobManager.BobData(0.0f, 0.0f, 0.0f);
            continue;
        }
      }
      this._data = this._bobData[BobMode.Idle];
    }

    public WeaponFeedbackManager.WeaponBobManager.BobData Data => this._data;

    public BobMode Mode
    {
      get => this._bobMode;
      set
      {
        if (this._bobMode == value)
          return;
        this._bobMode = value;
        this._data = this._bobData[value];
      }
    }

    public struct BobData
    {
      private float _xAmplitude;
      private float _yAmplitude;
      private float _frequency;

      public BobData(float xamp, float yamp, float freq)
      {
        this._xAmplitude = xamp;
        this._yAmplitude = yamp;
        this._frequency = freq;
      }

      public float XAmplitude => this._xAmplitude;

      public float YAmplitude => this._yAmplitude;

      public float Frequency => this._frequency;
    }
  }

  public enum WeaponMode
  {
    Primary,
    Second,
    PutDown,
  }

  private abstract class WeaponState
  {
    protected bool _isRunning;
    protected float _time;
    private BaseWeaponLogic _weapon;
    private BaseWeaponDecorator _decorator;
    protected Vector3 _pivotOffset;
    protected float _currentRotation;
    protected float _transitionTime;
    protected Vector3 _targetPosition;
    protected Quaternion _targetRotation;

    protected WeaponState(BaseWeaponLogic weapon, BaseWeaponDecorator decorator)
    {
      this._time = 0.0f;
      this._weapon = weapon;
      this._decorator = decorator;
      this._isRunning = this._weapon != null;
    }

    public abstract void Update();

    public abstract void Finish();

    public void Reset() => this._pivotOffset = new Vector3(0.0f, 0.0f, 0.2f);

    public Vector3 PivotVector => this._pivotOffset + (!WeaponFeedbackManager.Instance._isIronSight ? this.Decorator.DefaultPosition : this.Decorator.IronSightPosition);

    public virtual bool CanTransit(WeaponFeedbackManager.WeaponMode mode) => WeaponFeedbackManager.Instance.CurrentWeaponMode != mode;

    public bool IsRunning => this._isRunning;

    public bool IsValid => this._weapon != null && (UnityEngine.Object) this._decorator != (UnityEngine.Object) null;

    public BaseWeaponDecorator Decorator => this._decorator;

    public BaseWeaponLogic Weapon => this._weapon;

    public Vector3 TargetPosition => this._targetPosition;

    public Quaternion TargetRotation => this._targetRotation;
  }

  private class PickUpState : WeaponFeedbackManager.WeaponState
  {
    private bool _isFiring;

    public PickUpState(BaseWeaponLogic weapon, BaseWeaponDecorator decorator)
      : base(weapon, decorator)
    {
      this._transitionTime = Mathf.Max(WeaponFeedbackManager.Instance.WeaponAnimation.PickUpDuration, (float) (weapon.Config.SwitchDelayMilliSeconds / 1000));
      if (decorator.IsMelee)
      {
        this._currentRotation = -90f;
        if ((bool) (UnityEngine.Object) this.Decorator)
        {
          this.Decorator.CurrentRotation = Quaternion.Euler(0.0f, 0.0f, this._currentRotation);
          this.Decorator.CurrentPosition = decorator.DefaultPosition;
          this.Decorator.IsEnabled = true;
        }
      }
      else
      {
        this._currentRotation = WeaponFeedbackManager.Instance.WeaponAnimation.PutDownAngles;
        this._pivotOffset = -WeaponFeedbackManager.Instance._pivotPoint.localPosition;
        if ((bool) (UnityEngine.Object) this.Decorator)
        {
          this.Decorator.CurrentRotation = Quaternion.Euler(WeaponFeedbackManager.Instance.WeaponAnimation.PutDownAngles, 0.0f, 0.0f);
          this.Decorator.CurrentPosition = Quaternion.AngleAxis(this._currentRotation, Vector3.right) * this.PivotVector;
          this.Decorator.IsEnabled = true;
        }
      }
      LevelCamera.Instance.ResetZoom();
    }

    public override void Update()
    {
      if (!this.IsValid)
        return;
      if (this.IsRunning)
      {
        if ((double) this._time <= (double) this._transitionTime)
        {
          this._currentRotation = Mathf.Lerp(this._currentRotation, WeaponFeedbackManager.Instance.WeaponAnimation.PickUpAngles, this._time / this._transitionTime);
          if (this.Decorator.IsMelee)
          {
            this._targetPosition = this.Decorator.DefaultPosition;
            this._targetRotation = Quaternion.Euler(0.0f, 0.0f, this._currentRotation);
          }
          else
          {
            this._targetPosition = Quaternion.AngleAxis(this._currentRotation, Vector3.right) * this.PivotVector;
            this._targetRotation = Quaternion.Euler(this._currentRotation + this.Decorator.DefaultAngles.x, this.Decorator.DefaultAngles.y, this.Decorator.DefaultAngles.z);
          }
          if (!WeaponFeedbackManager.Instance._isIronSight)
          {
            this.Decorator.CurrentPosition = this._targetPosition;
            this.Decorator.CurrentRotation = this._targetRotation;
          }
          this._time += Time.deltaTime;
        }
        if ((double) this._time > (double) this._transitionTime * 0.25)
          this.Weapon.IsWeaponActive = true;
        if ((double) this._time > (double) this._transitionTime)
          this.Finish();
      }
      if ((double) this._time <= (double) this._transitionTime * 0.25)
        return;
      if (WeaponFeedbackManager.Instance._isIronSight)
      {
        this._pivotOffset = Vector3.Lerp(this._pivotOffset, (Vector3) Vector2.zero, Time.deltaTime * 20f);
        WeaponFeedbackManager.Instance._isIronSightPosDone = this.Decorator.CurrentPosition == this.Decorator.IronSightPosition;
      }
      else
        this._pivotOffset = Vector3.Lerp(this._pivotOffset, new Vector3(0.0f, 0.0f, 0.2f), Time.deltaTime * 10f);
      if ((double) WeaponFeedbackManager.Instance._fire.time < (double) WeaponFeedbackManager.Instance._fire.Duration)
      {
        if (this.IsRunning)
          return;
        if (!WeaponFeedbackManager.Instance._isIronSight && this._pivotOffset == new Vector3(0.0f, 0.0f, 0.2f))
        {
          WeaponFeedbackManager.Instance._fire.HandleFeedback();
          this.Decorator.CurrentPosition = this._targetPosition + WeaponFeedbackManager.Instance._fire.PositionOffset;
          this.Decorator.CurrentRotation = this._targetRotation * WeaponFeedbackManager.Instance._fire.RotationOffset;
        }
        else
        {
          this.Decorator.CurrentPosition = this.PivotVector + WeaponFeedbackManager.Instance._dip.PositionOffset;
          this.Decorator.CurrentRotation = this._targetRotation * WeaponFeedbackManager.Instance._dip.RotationOffset;
        }
        this._isFiring = true;
      }
      else
      {
        if (this._isFiring)
        {
          this._isFiring = false;
          WeaponFeedbackManager.Instance._time = 0.0f;
          WeaponFeedbackManager.Instance._angleX = 0.0f;
          WeaponFeedbackManager.Instance._angleY = 0.0f;
        }
        Quaternion identity = Quaternion.identity;
        Quaternion quaternion = !WeaponFeedbackManager.Instance._isIronSight || !(WeaponFeedbackManager.Instance._dip.PositionOffset == Vector3.zero) ? WeaponFeedbackManager.Instance.CalculateBobDip() : Quaternion.identity;
        if (!this.Decorator.IsMelee)
        {
          this.Decorator.CurrentPosition = quaternion * this.PivotVector + WeaponFeedbackManager.Instance._dip.PositionOffset;
          this.Decorator.CurrentRotation = this._targetRotation * WeaponFeedbackManager.Instance._dip.RotationOffset * quaternion;
        }
        else
          this.Decorator.CurrentRotation = this._targetRotation * WeaponFeedbackManager.Instance._dip.RotationOffset * quaternion;
      }
    }

    public override void Finish()
    {
      if (!this._isRunning)
        return;
      this._isRunning = false;
      if (this.Weapon != null)
      {
        this.Weapon.IsWeaponActive = true;
        WeaponFeedbackManager.Instance._currentWeaponMode = WeaponFeedbackManager.WeaponMode.Primary;
      }
      if (this.Decorator.IsMelee)
      {
        this._targetRotation = Quaternion.Euler(0.0f, 0.0f, WeaponFeedbackManager.Instance.WeaponAnimation.PickUpAngles);
        this._targetPosition = this.Decorator.DefaultPosition;
      }
      else
      {
        this._targetRotation = Quaternion.Euler(WeaponFeedbackManager.Instance.WeaponAnimation.PickUpAngles + this.Decorator.DefaultAngles.x, this.Decorator.DefaultAngles.y, this.Decorator.DefaultAngles.z);
        this._targetPosition = Quaternion.AngleAxis(WeaponFeedbackManager.Instance.WeaponAnimation.PickUpAngles, Vector3.right) * this.PivotVector;
      }
    }

    public override string ToString() => "Pick Up State";
  }

  private class PutDownState : WeaponFeedbackManager.WeaponState
  {
    private bool _destroy;

    public PutDownState(BaseWeaponLogic weapon, BaseWeaponDecorator decorator, bool destroy = false)
      : base(weapon, decorator)
    {
      this._destroy = destroy;
      this._currentRotation = decorator.CurrentRotation.eulerAngles.x;
      if ((double) this._currentRotation > 300.0)
        this._currentRotation = 360f - this._currentRotation;
      if (!decorator.IsMelee)
        this._pivotOffset = -WeaponFeedbackManager.Instance._pivotPoint.localPosition;
      this._transitionTime = WeaponFeedbackManager.Instance.WeaponAnimation.PutDownDuration;
      if (this.Weapon == null)
        return;
      this.Weapon.IsWeaponActive = false;
    }

    public override void Update()
    {
      if (!this.IsRunning || !this.IsValid || (double) this._time > (double) this._transitionTime)
        return;
      if (this.Decorator.IsMelee)
      {
        this._currentRotation = Mathf.Lerp(this._currentRotation, -90f, this._time / this._transitionTime);
        this._targetPosition = this.Decorator.DefaultPosition;
        this._targetRotation = Quaternion.Euler(0.0f, 0.0f, this._currentRotation);
      }
      else
      {
        this._currentRotation = Mathf.Lerp(this._currentRotation, WeaponFeedbackManager.Instance.WeaponAnimation.PutDownAngles, this._time / this._transitionTime);
        this._targetPosition = Quaternion.AngleAxis(this._currentRotation, Vector3.right) * this.PivotVector;
        this._targetRotation = Quaternion.Euler(this._currentRotation, 0.0f, 0.0f);
      }
      this.Decorator.CurrentPosition = this._targetPosition;
      this.Decorator.CurrentRotation = this._targetRotation;
      this._time += Time.deltaTime;
      if ((double) this._time <= (double) this._transitionTime)
        return;
      this.Finish();
    }

    public override void Finish()
    {
      if (!this._isRunning)
        return;
      this._isRunning = false;
      if (!(bool) (UnityEngine.Object) this.Decorator)
        return;
      this.Decorator.IsEnabled = false;
      this.Decorator.CurrentPosition = this.Decorator.DefaultPosition;
      this.Decorator.CurrentRotation = this._targetRotation;
      if (!this._destroy)
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.Decorator.gameObject);
    }

    public override string ToString() => "Put down";
  }

  [Serializable]
  public class FeedbackData
  {
    public float timeToPeak;
    public float timeToEnd;
    public float noise;
    public float angle;
    public float strength;
    public float recoilTime;
  }

  protected struct Feedback
  {
    public float time;
    public float noise;
    public float angle;
    public float timeToPeak;
    public float timeToEnd;
    public float strength;
    public float recoilTime;
    public Vector3 direction;
    public Vector3 rotationAxis;
    private float _maxAngle;
    private float _angle;
    private Vector3 _positionOffset;
    private Quaternion _rotationOffset;

    public float DebugAngle => this._angle;

    public float Duration => this.timeToPeak + this.timeToEnd;

    public Vector3 PositionOffset => this._positionOffset;

    public Quaternion RotationOffset => this._rotationOffset;

    public void HandleFeedback()
    {
      float num1 = UnityEngine.Random.Range(-this.noise, this.noise);
      this._maxAngle = Mathf.Lerp(this._maxAngle, this.angle, Time.deltaTime * 10f);
      if ((double) this.time < (double) this.Duration)
      {
        this.time += Time.deltaTime;
        if ((double) this.time < (double) this.Duration)
        {
          float num2;
          if ((double) this.time < (double) this.timeToPeak)
          {
            num2 = this.strength * Mathf.Sin((float) ((double) this.time * 3.1415927410125732 * 0.5) / this.timeToPeak);
            this.noise = Mathf.Lerp(this.noise, 0.0f, this.time / this.timeToPeak);
            this._angle = Mathf.Lerp(0.0f, this._maxAngle, Mathf.Pow(this.time / this.timeToPeak, 2f));
          }
          else
          {
            float t = (this.time - this.timeToPeak) / this.timeToEnd;
            num2 = this.strength * Mathf.Cos((float) (((double) this.time - (double) this.timeToPeak) * 3.1415927410125732 * 0.5) / this.timeToEnd);
            this._angle = Mathf.Lerp(this._maxAngle, 0.0f, t);
            if ((double) this.time != 0.0)
              num1 = 0.0f;
          }
          if (!(bool) (UnityEngine.Object) Singleton<WeaponController>.Instance.CurrentDecorator)
            return;
          this._positionOffset = num2 * this.direction + Singleton<WeaponController>.Instance.CurrentDecorator.transform.right * num1 + Singleton<WeaponController>.Instance.CurrentDecorator.transform.up * num1;
          this._rotationOffset = Quaternion.AngleAxis(this._angle, this.rotationAxis);
        }
        else
        {
          this._angle = 0.0f;
          this._positionOffset = Vector3.zero;
          this._rotationOffset = Quaternion.identity;
        }
      }
      else
      {
        this.time = 0.0f;
        this._angle = 0.0f;
        this._positionOffset = Vector3.zero;
        this._rotationOffset = Quaternion.identity;
      }
    }

    public void Reset()
    {
      this.time = 0.0f;
      this.timeToEnd = 0.0f;
      this.timeToPeak = -1f;
      this.angle = 0.0f;
      this.direction = Vector3.zero;
      this._angle = 0.0f;
      this._positionOffset = Vector3.zero;
      this._rotationOffset = Quaternion.identity;
    }
  }

  [Serializable]
  public class WeaponAnimData
  {
    public float PutDownAngles = 30f;
    public float PutDownDuration;
    public float PickUpAngles;
    public float PickUpDuration;
  }
}
