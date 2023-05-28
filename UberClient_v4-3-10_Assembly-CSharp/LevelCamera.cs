// Decompiled with JetBrains decompiler
// Type: LevelCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class LevelCamera : MonoBehaviour, IObserver
{
  private static LevelCamera _instance;
  private LevelCamera.CameraConfiguration _cameraConfiguration = new LevelCamera.CameraConfiguration();
  private LevelCamera.FeedbackData _jumpFeedback = new LevelCamera.FeedbackData();
  private LevelCamera.Feedback _feedback;
  private LevelCamera.CameraBobManager _bobManager;
  private CameraCollisionDetector _ccd;
  private LevelCamera.ZoomData _zoomData;
  private LevelCamera.CameraState _currentState;
  private float _height;
  private float _orbitDistance;
  private float _orbitSpeed;
  private bool _isZoomedIn;
  private Vector3 _eyePosition;
  private Transform _transform;
  private Transform _targetTransform;
  private LevelCamera.CameraMode _currentMode;
  private Quaternion _userInputCache;
  private Quaternion _userInputRotation;
  private AudioLowPassFilter _lowpassFilter;

  public static LevelCamera Instance
  {
    get
    {
      if ((UnityEngine.Object) LevelCamera._instance == (UnityEngine.Object) null)
      {
        GameObject gameObject = new GameObject(nameof (LevelCamera), new System.Type[2]
        {
          typeof (AudioListener),
          typeof (global::DontDestroyOnLoad)
        });
        gameObject.layer = 18;
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = 0.01f;
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        LevelCamera._instance = gameObject.AddComponent<LevelCamera>();
      }
      return LevelCamera._instance;
    }
  }

  private void Awake()
  {
    this._transform = this.transform;
    this._currentState = (LevelCamera.CameraState) new LevelCamera.NoneState();
    this._bobManager = new LevelCamera.CameraBobManager();
    this._ccd = new CameraCollisionDetector();
    this._ccd.Offset = 1f;
    this._ccd.LayerMask = 1;
    this._lowpassFilter = this.gameObject.AddComponent<AudioLowPassFilter>();
    if (!(bool) (UnityEngine.Object) this._lowpassFilter)
      return;
    this._lowpassFilter.cutoffFrequency = 755f;
  }

  private void LateUpdate()
  {
    if (this._currentMode == LevelCamera.CameraMode.SmoothFollow)
      return;
    this._currentState.Update();
  }

  private void OnDrawGizmos()
  {
    if (!((UnityEngine.Object) this._targetTransform != (UnityEngine.Object) null))
      return;
    Gizmos.color = Color.cyan;
    Gizmos.DrawSphere(this._targetTransform.position, 0.1f);
    Gizmos.color = Color.white;
  }

  public void Notify()
  {
    if (this._currentMode != LevelCamera.CameraMode.SmoothFollow)
      return;
    this._currentState.Update();
  }

  private void InitUserInput()
  {
    Vector3 eulerAngles = UserInput.Rotation.eulerAngles;
    this._userInputCache = UserInput.Rotation;
    eulerAngles.x = Mathf.Clamp(eulerAngles.x, 0.0f, 60f);
    this._userInputRotation = Quaternion.Euler(eulerAngles);
  }

  private void UpdateUserInput()
  {
    Vector3 eulerAngles = UserInput.Rotation.eulerAngles;
    float x1 = this._userInputCache.eulerAngles.x;
    float x2 = UserInput.Rotation.eulerAngles.x;
    if ((double) x1 > 180.0)
      x1 -= 360f;
    if ((double) x2 > 180.0)
      x2 -= 360f;
    eulerAngles.x = Mathf.Clamp(this._userInputRotation.eulerAngles.x + (x2 - x1), 0.0f, 60f);
    this._userInputCache = UserInput.Rotation;
    this._userInputRotation = Quaternion.Euler(eulerAngles);
  }

  private void TransformFollowCamera(
    Vector3 targetPosition,
    Quaternion targetRotation,
    float distance,
    ref float collideDistance)
  {
    Vector3 v = this._userInputRotation * Vector3.back * collideDistance;
    Matrix4x4 matrix4x4 = Matrix4x4.TRS(targetPosition, targetRotation, Vector3.one);
    Vector3 vector3 = matrix4x4.MultiplyPoint3x4(v);
    Quaternion quaternion = Quaternion.LookRotation(targetPosition - vector3);
    Vector3 to = matrix4x4.MultiplyPoint3x4(this._userInputRotation * Vector3.back * distance);
    if (this._ccd.Detect(targetPosition, to, quaternion * Vector3.right))
    {
      float distance1 = this._ccd.Distance;
      collideDistance = (double) distance1 >= (double) collideDistance ? Mathf.Lerp(collideDistance, distance1, Time.deltaTime * 3f) : Mathf.Clamp(distance1, 1f, distance);
    }
    else
      collideDistance = Mathf.Approximately(collideDistance, distance) ? distance : Mathf.Lerp(collideDistance, distance, Time.deltaTime * 5f);
    this._transform.position = vector3;
    this._transform.rotation = quaternion;
  }

  private void TransformDeathCamera(
    Vector3 targetPosition,
    Quaternion targetRotation,
    float distance,
    ref float collideDistance)
  {
    Vector3 v = Vector3.back * collideDistance;
    Matrix4x4 matrix4x4 = Matrix4x4.TRS(targetPosition, targetRotation, Vector3.one);
    Vector3 vector3 = matrix4x4.MultiplyPoint3x4(v);
    Quaternion quaternion = Quaternion.LookRotation(targetPosition - vector3);
    Vector3 to = matrix4x4.MultiplyPoint3x4(Vector3.back * distance);
    if (this._ccd.Detect(targetPosition, to, quaternion * Vector3.right))
    {
      float distance1 = this._ccd.Distance;
      collideDistance = (double) distance1 >= (double) collideDistance ? Mathf.Lerp(collideDistance, distance1, Time.deltaTime * 3f) : Mathf.Clamp(distance1, 1f, distance);
    }
    else
      collideDistance = Mathf.Approximately(collideDistance, distance) ? distance : Mathf.Lerp(collideDistance, distance, Time.deltaTime * 5f);
    this._transform.position = vector3;
    this._transform.rotation = quaternion;
  }

  public void SetTarget(Transform target) => this._targetTransform = target;

  public void SetLevelCamera(Camera camera, Vector3 position, Quaternion rotation)
  {
    if (!((UnityEngine.Object) camera != (UnityEngine.Object) this.MainCamera) || !((UnityEngine.Object) camera != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.MainCamera != (UnityEngine.Object) null)
      this.ResetCamera(this.MainCamera, this._cameraConfiguration);
    this._cameraConfiguration.Parent = camera.transform.parent;
    this._cameraConfiguration.Fov = camera.fov;
    this._cameraConfiguration.CullingMask = camera.cullingMask;
    this.MainCamera = camera;
    this.MainCamera.cullingMask = LayerUtil.AddToLayerMask(camera.cullingMask, UberstrikeLayer.LocalProjectile);
    this.ReparentCamera(camera, this.transform);
    camera.transform.localPosition = Vector3.zero;
    camera.transform.localRotation = Quaternion.identity;
    this._zoomData.TargetFOV = camera.fov;
    this._transform.position = position;
    this._transform.rotation = rotation;
  }

  public void ReleaseCamera()
  {
    if (!((UnityEngine.Object) this.MainCamera != (UnityEngine.Object) null))
      return;
    this.ResetCamera(this.MainCamera, this._cameraConfiguration);
    this.MainCamera = (Camera) null;
  }

  private void ResetCamera(Camera camera, LevelCamera.CameraConfiguration config)
  {
    camera.fov = config.Fov;
    camera.cullingMask = config.CullingMask;
    this.ReparentCamera(camera, config.Parent);
  }

  private void ReparentCamera(Camera camera, Transform parent) => camera.transform.parent = parent;

  public void SetMode(LevelCamera.CameraMode mode)
  {
    if (mode == this._currentMode)
      return;
    this._feedback.timeToEnd = 0.0f;
    this._currentMode = mode;
    this._currentState.Finish();
    if (!((UnityEngine.Object) this.MainCamera != (UnityEngine.Object) null))
      return;
    switch (mode)
    {
      case LevelCamera.CameraMode.None:
        this._currentState = (LevelCamera.CameraState) new LevelCamera.NoneState();
        break;
      case LevelCamera.CameraMode.Spectator:
        this.MainCamera.cullingMask = LayerUtil.AddToLayerMask(this.MainCamera.cullingMask, UberstrikeLayer.LocalPlayer);
        this._currentState = (LevelCamera.CameraState) new LevelCamera.SpectatorState();
        break;
      case LevelCamera.CameraMode.OrbitAround:
        this.MainCamera.cullingMask = LayerUtil.AddToLayerMask(this.MainCamera.cullingMask, UberstrikeLayer.LocalPlayer);
        this._currentState = (LevelCamera.CameraState) new LevelCamera.OrbitAroundState();
        break;
      case LevelCamera.CameraMode.FirstPerson:
        this.MainCamera.cullingMask = LayerUtil.RemoveFromLayerMask(this.MainCamera.cullingMask, UberstrikeLayer.LocalPlayer, UberstrikeLayer.Weapons);
        this.MainCamera.cullingMask = LayerUtil.AddToLayerMask(this.MainCamera.cullingMask, UberstrikeLayer.RemoteProjectile);
        this._currentState = (LevelCamera.CameraState) new LevelCamera.FirstPersonState();
        break;
      case LevelCamera.CameraMode.ThirdPerson:
        this.MainCamera.cullingMask = LayerUtil.AddToLayerMask(this.MainCamera.cullingMask, UberstrikeLayer.LocalPlayer, UberstrikeLayer.Weapons);
        this._currentState = (LevelCamera.CameraState) new LevelCamera.ThirdPersonState();
        break;
      case LevelCamera.CameraMode.SmoothFollow:
        this.MainCamera.cullingMask = LayerUtil.AddToLayerMask(this.MainCamera.cullingMask, UberstrikeLayer.LocalPlayer);
        this._currentState = (LevelCamera.CameraState) new LevelCamera.SmoothFollowState();
        break;
      case LevelCamera.CameraMode.FollowTurret:
        this._currentState = (LevelCamera.CameraState) new LevelCamera.TurretState();
        break;
      case LevelCamera.CameraMode.Ragdoll:
        this.MainCamera.cullingMask = LayerUtil.AddToLayerMask(this.MainCamera.cullingMask, UberstrikeLayer.LocalPlayer);
        this._currentState = (LevelCamera.CameraState) new LevelCamera.RagdollState();
        break;
      case LevelCamera.CameraMode.Death:
        this._currentState = (LevelCamera.CameraState) new LevelCamera.DeadState();
        break;
      case LevelCamera.CameraMode.Overview:
        this._currentState = (LevelCamera.CameraState) new LevelCamera.OverviewState();
        break;
      default:
        Debug.LogError((object) ("Camera does not support " + mode.ToString()));
        break;
    }
  }

  public void SetEyePosition(float x, float y, float z) => this._eyePosition = new Vector3(x, y, z);

  public void SetLookAtHeight(float height) => this._height = height;

  public void SetOrbitDistance(float distance) => this._orbitDistance = distance;

  public void SetOrbitSpeed(float speed) => this._orbitSpeed = speed;

  public static void SetBobMode(BobMode mode)
  {
    if (!((UnityEngine.Object) LevelCamera.Instance != (UnityEngine.Object) null))
      return;
    LevelCamera.Instance._bobManager.Mode = mode;
    if (!WeaponFeedbackManager.Exists)
      return;
    WeaponFeedbackManager.Instance.SetBobMode(mode);
  }

  public void DoFeedback(
    LevelCamera.FeedbackType type,
    Vector3 direction,
    float strength,
    float noise,
    float timeToPeak,
    float timeToEnd,
    float angle,
    Vector3 axis)
  {
    this._feedback.time = 0.0f;
    this._feedback.noise = noise / 4f;
    this._feedback.strength = strength;
    this._feedback.timeToPeak = timeToPeak;
    this._feedback.timeToEnd = timeToEnd;
    this._feedback.direction = direction;
    this._feedback.angle = angle;
    this._feedback.rotationAxis = axis;
  }

  public bool DoLandFeedback(bool shake)
  {
    if (this._currentMode != LevelCamera.CameraMode.FirstPerson || !this.CanDip || (double) this._feedback.time != 0.0 && (double) this._feedback.time < (double) this._feedback.Duration)
      return false;
    this._feedback.time = 0.0f;
    this._feedback.angle = this._jumpFeedback.angle;
    this._feedback.noise = !shake ? 0.0f : this._jumpFeedback.noise;
    this._feedback.strength = this._jumpFeedback.strength;
    this._feedback.timeToPeak = this._jumpFeedback.timeToPeak;
    this._feedback.timeToEnd = this._jumpFeedback.timeToEnd;
    this._feedback.direction = Vector3.down;
    this._feedback.rotationAxis = Vector3.right;
    WeaponFeedbackManager.Instance.LandingDip();
    return true;
  }

  public void DoZoomIn(float fov, float speed)
  {
    if ((double) fov < 1.0 || (double) fov > 100.0 || (double) speed < 1.0 / 1000.0 || (double) speed > 100.0)
    {
      Debug.LogError((object) ("Invalid parameters specified!\n FOV should be >1 & <100, Speed should be >0.001 & <100.\nFOV = " + (object) fov + " Speed = " + (object) speed));
    }
    else
    {
      if (this._isZoomedIn && (double) fov == (double) this.FOV)
        return;
      this._zoomData.Speed = speed;
      this._zoomData.TargetFOV = fov;
      this._zoomData.TargetAlpha = 1f;
      this._isZoomedIn = true;
    }
  }

  public bool IsZoomedIn
  {
    get => this._isZoomedIn;
    set => this._isZoomedIn = false;
  }

  public void DoZoomOut(float fov, float speed)
  {
    if ((double) fov < 1.0 || (double) fov > 100.0 || (double) speed < 1.0 / 1000.0 || (double) speed > 100.0)
    {
      Debug.LogError((object) ("Invalid parameters specified!\n FOV should be >1 & <100, Speed should be >0.001 & <100.\nFOV = " + (object) fov + " Speed = " + (object) speed));
    }
    else
    {
      if (!this._isZoomedIn)
        return;
      this._zoomData.Speed = speed;
      this._zoomData.TargetFOV = fov;
      this._zoomData.TargetAlpha = 0.0f;
      this._isZoomedIn = false;
      CmuneEventHandler.Route((object) new OnCameraZoomOutEvent());
    }
  }

  public void ResetZoom()
  {
    this._isZoomedIn = false;
    this._zoomData.ResetZoom();
  }

  public Ray ScreenPointToRay(Vector3 point) => (bool) (UnityEngine.Object) this.MainCamera ? this.MainCamera.ScreenPointToRay(point) : new Ray();

  public void EnableLowPassFilter(bool enabled)
  {
    if (!(bool) (UnityEngine.Object) this._lowpassFilter)
      return;
    this._lowpassFilter.enabled = enabled;
  }

  public void ResetFeedback() => this._feedback.Reset();

  public static bool HasCamera => (UnityEngine.Object) LevelCamera.Instance.MainCamera != (UnityEngine.Object) null;

  public Camera MainCamera { get; private set; }

  public Transform TransformCache => this._transform;

  public float FOV => (UnityEngine.Object) this.MainCamera != (UnityEngine.Object) null ? this.MainCamera.fov : 65f;

  public Vector3 EyePosition => this._eyePosition;

  public float LookAtHeight => this._height;

  public float OrbitDistance => this._orbitDistance;

  public float OrbitSpeed => this._orbitSpeed;

  public LevelCamera.CameraMode CurrentMode => this._currentMode;

  public BobMode CurrentBob => this._bobManager.Mode;

  public bool LowpassFilterEnabled => (bool) (UnityEngine.Object) this._lowpassFilter && this._lowpassFilter.enabled;

  public bool CanDip { get; set; }

  private class CameraConfiguration
  {
    public Transform Parent;
    public int CullingMask;
    public float Fov;
  }

  private class CameraBobManager
  {
    private float _strength;
    private LevelCamera.CameraBobManager.BobData _data;
    private BobMode _bobMode;
    private readonly Dictionary<BobMode, LevelCamera.CameraBobManager.BobData> _bobData;

    public CameraBobManager()
    {
      this._bobData = new Dictionary<BobMode, LevelCamera.CameraBobManager.BobData>();
      foreach (int num in Enum.GetValues(typeof (BobMode)))
      {
        BobMode key = (BobMode) num;
        switch (key)
        {
          case BobMode.Idle:
            this._bobData[key] = new LevelCamera.CameraBobManager.BobData(0.2f, 0.0f, 2f);
            continue;
          case BobMode.Walk:
            this._bobData[key] = new LevelCamera.CameraBobManager.BobData(0.3f, 0.3f, 6f);
            continue;
          case BobMode.Run:
            this._bobData[key] = new LevelCamera.CameraBobManager.BobData(0.5f, 0.3f, 8f);
            continue;
          case BobMode.Crouch:
            this._bobData[key] = new LevelCamera.CameraBobManager.BobData(0.8f, 0.8f, 12f);
            continue;
          default:
            this._bobData[key] = new LevelCamera.CameraBobManager.BobData(0.0f, 0.0f, 0.0f);
            continue;
        }
      }
      this._data = this._bobData[BobMode.Idle];
    }

    public void Update()
    {
      Transform transform = LevelCamera.Instance._transform;
      switch (this._bobMode)
      {
        case BobMode.Idle:
          float num1 = Mathf.Sin(Time.time * this._data.Frequency);
          transform.rotation = Quaternion.AngleAxis(num1 * this._data.XAmplitude * this._strength, transform.right) * Quaternion.AngleAxis(num1 * this._data.ZAmplitude, transform.forward) * transform.rotation;
          break;
        case BobMode.Walk:
          float num2 = Mathf.Sin(Time.time * this._data.Frequency);
          transform.rotation = Quaternion.AngleAxis(Mathf.Abs(num2 * this._data.XAmplitude), transform.right) * Quaternion.AngleAxis(num2 * this._data.ZAmplitude, transform.forward) * transform.rotation;
          break;
        case BobMode.Run:
          float num3 = Mathf.Sin(Time.time * this._data.Frequency);
          transform.rotation = Quaternion.AngleAxis(Mathf.Abs(num3 * this._data.XAmplitude * this._strength), transform.right) * Quaternion.AngleAxis(num3 * this._data.ZAmplitude, transform.forward) * transform.rotation;
          break;
        case BobMode.Fly:
          float angle1 = Mathf.Sin(Time.time * this._data.Frequency) * this._data.ZAmplitude;
          transform.rotation = Quaternion.AngleAxis(angle1, transform.forward) * transform.rotation;
          break;
        case BobMode.Swim:
          float angle2 = Mathf.Sin(Time.time * this._data.Frequency) * this._data.ZAmplitude;
          transform.rotation = Quaternion.AngleAxis(angle2, transform.forward) * transform.rotation;
          break;
        case BobMode.Crouch:
          float num4 = Mathf.Sin(Time.time * this._data.Frequency);
          transform.rotation = Quaternion.AngleAxis(Mathf.Abs(num4 * this._data.XAmplitude), transform.right) * Quaternion.AngleAxis(num4 * this._data.ZAmplitude, transform.forward) * transform.rotation;
          break;
      }
      this._strength = Mathf.Clamp01(this._strength + Time.deltaTime);
    }

    public BobMode Mode
    {
      get => this._bobMode;
      set
      {
        if (this._bobMode == value)
          return;
        this._strength = 0.0f;
        this._bobMode = value;
        this._data = this._bobData[value];
      }
    }

    private struct BobData
    {
      private float _xAmplitude;
      private float _zAmplitude;
      private float _frequency;

      public BobData(float xamp, float zamp, float freq)
      {
        this._xAmplitude = xamp;
        this._zAmplitude = zamp;
        this._frequency = freq;
      }

      public float XAmplitude => this._xAmplitude;

      public float ZAmplitude => this._zAmplitude;

      public float Frequency => this._frequency;
    }
  }

  public class FeedbackData
  {
    public float timeToPeak = 0.2f;
    public float timeToEnd = 0.15f;
    public float noise = 0.5f;
    public float angle;
    public float strength = 0.3f;
  }

  private struct Feedback
  {
    public float time;
    public float noise;
    public float angle;
    public float timeToPeak;
    public float timeToEnd;
    public float strength;
    public Vector3 direction;
    public Vector3 rotationAxis;
    private float _angle;
    private float _currentNoise;
    private Vector3 shakePos;

    public float DebugAngle => this._angle;

    public float Duration => this.timeToPeak + this.timeToEnd;

    public void HandleFeedback()
    {
      if ((double) this.Duration == 0.0)
        return;
      float angle = UnityEngine.Random.Range(-this.noise, this.noise);
      if ((double) this.time < (double) this.Duration)
      {
        float num;
        if ((double) this.time < (double) this.timeToPeak)
        {
          num = this.strength * Mathf.Sin((float) ((double) this.time * 3.1415927410125732 * 0.5) / this.timeToPeak);
          this._angle = Mathf.Lerp(0.0f, this.angle, this.time / this.timeToPeak);
        }
        else
        {
          float t = (this.time - this.timeToPeak) / this.timeToEnd;
          num = this.strength * Mathf.Cos((float) (((double) this.time - (double) this.timeToPeak) * 3.1415927410125732 * 0.5) / this.timeToEnd);
          this._angle = Mathf.Lerp(this._angle, 0.0f, t);
          if ((double) this.time != 0.0)
            angle = 0.0f;
        }
        this._currentNoise = Mathf.Lerp(this.noise, 0.0f, this.time / this.Duration);
        this.shakePos = Vector3.Lerp(this.shakePos, UnityEngine.Random.insideUnitSphere * this._currentNoise, Time.deltaTime * 30f);
        this.time += Time.deltaTime;
        LevelCamera.Instance._transform.position += num * this.direction;
        LevelCamera.Instance._transform.rotation = LevelCamera.Instance._transform.rotation * Quaternion.AngleAxis(this._angle, this.rotationAxis) * Quaternion.AngleAxis(angle, LevelCamera.Instance._transform.forward);
      }
      else
      {
        this.time = 0.0f;
        this.timeToEnd = 0.0f;
        this.timeToPeak = 0.0f;
        this._angle = 0.0f;
      }
    }

    public void Reset()
    {
      this._angle = 0.0f;
      this.time = 0.0f;
      this.timeToEnd = 0.0f;
      this.timeToPeak = 0.0f;
    }
  }

  private struct ZoomData
  {
    public float TargetAlpha;
    public float TargetFOV;
    public float Speed;
    private float _alpha;

    public bool IsFovChanged => (double) this.TargetFOV != (double) LevelCamera.Instance.FOV;

    public void Update()
    {
      this._alpha = Mathf.Lerp(this._alpha, this.TargetAlpha, Time.deltaTime * this.Speed);
      if (!(bool) (UnityEngine.Object) LevelCamera.Instance.MainCamera)
        return;
      LevelCamera.Instance.MainCamera.fov = Mathf.Lerp(LevelCamera.Instance.MainCamera.fov, this.TargetFOV, Time.deltaTime * this.Speed);
    }

    public void ResetZoom()
    {
      if (!(bool) (UnityEngine.Object) LevelCamera.Instance.MainCamera)
        return;
      this.TargetFOV = 60f;
      LevelCamera.Instance.MainCamera.fov = this.TargetFOV;
    }
  }

  public enum CameraMode
  {
    None,
    Spectator,
    OrbitAround,
    FirstPerson,
    ThirdPerson,
    SmoothFollow,
    FollowTurret,
    Ragdoll,
    Death,
    Overview,
  }

  public enum FeedbackType
  {
    JumpLand,
    GetDamage,
    ShootWeapon,
  }

  public abstract class CameraState
  {
    protected Vector3 LookAtPosition(
      Transform target,
      Quaternion lookRot,
      Quaternion xRot,
      Quaternion yRot,
      float distance,
      float height)
    {
      Vector3 position = lookRot * Vector3.back * distance;
      return target.up * LevelCamera.Instance.LookAtHeight + target.TransformPoint(position);
    }

    protected Quaternion LookAtRotation(Transform target, Quaternion rotation)
    {
      Vector3 direction = rotation * Vector3.forward;
      return Quaternion.LookRotation(target.TransformDirection(direction));
    }

    public abstract void Update();

    public virtual void Finish()
    {
    }

    public virtual void OnDrawGizmos()
    {
    }

    public override string ToString() => "Abstract state";
  }

  private class NoneState : LevelCamera.CameraState
  {
    public override void Update()
    {
    }
  }

  private class FirstPersonState : LevelCamera.CameraState
  {
    private const float _duration = 1f;
    private bool _handleFeedback = true;

    public override void Update()
    {
      LevelCamera.Instance._transform.position = LevelCamera.Instance._targetTransform.position + LevelCamera.Instance.EyePosition;
      LevelCamera.Instance._transform.rotation = LevelCamera.Instance._targetTransform.rotation;
      if (this._handleFeedback)
      {
        LevelCamera.Instance._feedback.HandleFeedback();
        LevelCamera.Instance._bobManager.Update();
      }
      if (!LevelCamera.Instance._zoomData.IsFovChanged)
        return;
      LevelCamera.Instance._zoomData.Update();
    }

    public override void Finish() => LevelCamera.Instance._zoomData.ResetZoom();

    public override void OnDrawGizmos() => Gizmos.DrawRay(LevelCamera.Instance._transform.position, LevelCamera.Instance._transform.TransformDirection(LevelCamera.Instance._feedback.rotationAxis));

    public override string ToString() => "FPS state";
  }

  private class ThirdPersonState : LevelCamera.CameraState
  {
    private float _right = 1f;
    private float _collideDistance;
    private float _distance = 2.5f;
    private CameraCollisionDetector _ccd;

    public ThirdPersonState()
    {
      this._collideDistance = this._distance / 2f;
      this._ccd = new CameraCollisionDetector();
      this._ccd.Offset = 1f;
    }

    private Vector3 TargetCheckPoint => LevelCamera.Instance._targetTransform.position + LevelCamera.Instance._targetTransform.up * LevelCamera.Instance.LookAtHeight;

    private Vector3 LeftCheckPoint => LevelCamera.Instance._transform.position - LevelCamera.Instance._transform.right * this._right;

    private Vector3 RightCheckPoint => LevelCamera.Instance._transform.position + LevelCamera.Instance._transform.right * this._right;

    public override void Update()
    {
      this.TransformCamera();
      if (!LevelCamera.Instance._zoomData.IsFovChanged)
        return;
      LevelCamera.Instance._zoomData.Update();
    }

    public override void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      Vector3 up = LevelCamera.Instance._targetTransform.up;
      if ((UnityEngine.Object) LevelCamera.Instance._targetTransform != (UnityEngine.Object) null)
      {
        Gizmos.DrawWireSphere(this.TargetCheckPoint, 0.1f);
        Vector3 vector3 = this.LookAtPosition(LevelCamera.Instance._targetTransform, Quaternion.identity, Quaternion.Euler(LevelCamera.Instance._targetTransform.rotation.eulerAngles.x, 0.0f, 0.0f), Quaternion.Euler(0.0f, LevelCamera.Instance._targetTransform.rotation.eulerAngles.y, 0.0f), this._distance, LevelCamera.Instance.LookAtHeight);
        Gizmos.DrawLine(this.TargetCheckPoint, vector3 - LevelCamera.Instance._targetTransform.right);
        Gizmos.DrawLine(this.TargetCheckPoint, vector3 + LevelCamera.Instance._targetTransform.right);
      }
      this._ccd.OnDrawGizmos();
      Gizmos.color = Color.red;
      Gizmos.DrawRay(LevelCamera.Instance._targetTransform.position, LevelCamera.Instance._targetTransform.right);
      Gizmos.color = Color.green;
      Gizmos.DrawRay(LevelCamera.Instance._targetTransform.position, up);
      Gizmos.color = Color.blue;
      Gizmos.DrawRay(LevelCamera.Instance._targetTransform.position, LevelCamera.Instance._targetTransform.forward);
    }

    private void TransformCamera()
    {
      Vector3 eulerAngles = LevelCamera.Instance._targetTransform.rotation.eulerAngles;
      eulerAngles.x = (double) eulerAngles.x <= 90.0 ? Mathf.Clamp(eulerAngles.x, 0.0f, 60f) : Mathf.Clamp(eulerAngles.x, 320f, 360f);
      Quaternion xRot = Quaternion.Euler(eulerAngles.x, 0.0f, 0.0f);
      Quaternion yRot = Quaternion.Euler(0.0f, eulerAngles.y, 0.0f);
      if (this._ccd.Detect(this.TargetCheckPoint, this.LookAtPosition(LevelCamera.Instance._targetTransform, Quaternion.identity, xRot, yRot, this._distance, LevelCamera.Instance.LookAtHeight), LevelCamera.Instance._targetTransform.right))
      {
        float distance = this._ccd.Distance;
        this._collideDistance = (double) distance >= (double) this._collideDistance ? Mathf.Lerp(this._collideDistance, distance, Time.deltaTime) : Mathf.Clamp(distance, 0.5f, this._distance);
      }
      else
        this._collideDistance = Mathf.Lerp(this._collideDistance, this._distance, Time.deltaTime);
      LevelCamera.Instance._transform.position = this.LookAtPosition(LevelCamera.Instance._targetTransform, Quaternion.identity, xRot, yRot, this._collideDistance, LevelCamera.Instance.LookAtHeight);
      LevelCamera.Instance._transform.rotation = Quaternion.LookRotation(this.TargetCheckPoint - LevelCamera.Instance._transform.position);
    }

    public override string ToString() => "3rd person state";
  }

  private class SmoothFollowState : LevelCamera.CameraState
  {
    private const float _zoomSpeed = 40f;
    private float _collideDistance;
    private float _distance = 1.5f;
    private Quaternion _targetRotationY = Quaternion.identity;

    public SmoothFollowState()
    {
      this._collideDistance = this._distance / 2f;
      LevelCamera.Instance.InitUserInput();
    }

    private Vector3 TargetCheckPoint => LevelCamera.Instance._targetTransform.position + LevelCamera.Instance._targetTransform.up * LevelCamera.Instance.LookAtHeight;

    public override void Update()
    {
      float f = AutoMonoBehaviour<InputManager>.Instance.RawValue(GameInputKey.NextWeapon);
      if ((double) f != 0.0)
      {
        this._distance -= Mathf.Sign(f) * 40f * Time.deltaTime;
        this._distance = Mathf.Clamp(this._distance, 1f, 4f);
      }
      this._targetRotationY = Quaternion.Lerp(this._targetRotationY, Quaternion.Euler(0.0f, LevelCamera.Instance._targetTransform.eulerAngles.y, 0.0f), Time.deltaTime * 2f);
      Vector3 targetPosition = LevelCamera.Instance._targetTransform.position + Vector3.up * LevelCamera.Instance.LookAtHeight;
      LevelCamera.Instance.UpdateUserInput();
      LevelCamera.Instance.TransformFollowCamera(targetPosition, this._targetRotationY, this._distance, ref this._collideDistance);
    }

    public override string ToString() => "Smooth follow state";
  }

  private class OrbitAroundState : LevelCamera.CameraState
  {
    private float _distance;
    private float _angle;
    private CameraCollisionDetector _ccd;

    public OrbitAroundState()
    {
      this._distance = 1f;
      this._angle = 0.0f;
      this._ccd = new CameraCollisionDetector();
      this._ccd.Offset = 1f;
    }

    public override void Update()
    {
      Quaternion yRot = Quaternion.Euler(0.0f, this._angle += Time.deltaTime * LevelCamera.Instance.OrbitSpeed, 0.0f);
      Vector3 from = LevelCamera.Instance._targetTransform.position + Vector3.up * LevelCamera.Instance.LookAtHeight;
      Vector3 to = this.LookAtPosition(LevelCamera.Instance._targetTransform, Quaternion.identity, Quaternion.identity, yRot, 1f, LevelCamera.Instance.LookAtHeight);
      this._distance = !this._ccd.Detect(from, to, LevelCamera.Instance._transform.right) ? Mathf.Lerp(this._distance, LevelCamera.Instance.OrbitDistance, Time.deltaTime) : ((double) this._distance >= (double) this._ccd.Distance ? Mathf.Lerp(this._distance, this._ccd.Distance, Time.deltaTime) : this._ccd.Distance);
      LevelCamera.Instance._transform.position = this.LookAtPosition(LevelCamera.Instance._targetTransform, Quaternion.identity, Quaternion.identity, yRot, this._distance, LevelCamera.Instance.LookAtHeight);
      LevelCamera.Instance._transform.rotation = Quaternion.LookRotation(from - LevelCamera.Instance._transform.position);
    }
  }

  private class RagdollState : LevelCamera.CameraState
  {
    private const float MaxDuration = 1f;
    private const float MinimalCameraHeight = -20f;
    private Vector3 _targetPosition;

    public RagdollState()
    {
      if ((UnityEngine.Object) GameState.LocalDecorator != (UnityEngine.Object) null && (UnityEngine.Object) GameState.LocalDecorator.CurrentRagdoll != (UnityEngine.Object) null)
        LevelCamera.Instance.SetTarget(GameState.LocalDecorator.CurrentRagdoll.GetBone(BoneIndex.Hips));
      if (!((UnityEngine.Object) LevelCamera.Instance._targetTransform != (UnityEngine.Object) null))
        return;
      this._targetPosition = LevelCamera.Instance._targetTransform.position;
    }

    public override void Update()
    {
      if (!((UnityEngine.Object) LevelCamera.Instance._targetTransform != (UnityEngine.Object) null))
        return;
      if ((double) this._targetPosition.y > -20.0 && (double) Mathf.Abs(this._targetPosition.y - LevelCamera.Instance._targetTransform.position.y) > 0.20000000298023224)
        this._targetPosition = LevelCamera.Instance._targetTransform.position;
      LevelCamera.Instance._transform.rotation = Quaternion.Slerp(LevelCamera.Instance._transform.rotation, Quaternion.LookRotation(this._targetPosition - LevelCamera.Instance.transform.position), Time.deltaTime * 4f);
      LevelCamera.Instance._transform.position = Vector3.Lerp(LevelCamera.Instance._transform.position, this._targetPosition + new Vector3(0.0f, 2f, 0.0f) - LevelCamera.Instance._transform.forward * 3f, Time.deltaTime * 4f);
    }
  }

  private class SpectatorState : LevelCamera.CameraState
  {
    private const int MaxSpeed = 22;
    private const float verticalSpeed = 0.8f;
    private Vector3 _targetPosition;
    private float _speed = 11f;

    public SpectatorState()
    {
      Vector3 eulerAngles = LevelCamera.Instance._transform.rotation.eulerAngles;
      UserInput.SetRotation(eulerAngles.y, eulerAngles.x);
      this._targetPosition = LevelCamera.Instance._transform.position;
    }

    public override void Update()
    {
      if (Singleton<InGameChatHud>.Instance.CanInput || !Screen.lockCursor)
        return;
      UserInput.UpdateDirections();
      int num = !UserInput.IsWalking ? 4 : 6;
      this._speed = Mathf.Lerp(this._speed, !UserInput.IsWalking ? 11f : 22f, Time.deltaTime);
      this._targetPosition += (UserInput.Rotation * UserInput.HorizontalDirection + UserInput.VerticalDirection * 0.8f) * this._speed * Time.deltaTime;
      LevelCamera.Instance._transform.position = Vector3.Lerp(LevelCamera.Instance._transform.position, this._targetPosition, Time.deltaTime * (float) num);
      LevelCamera.Instance._transform.rotation = UserInput.Rotation;
    }
  }

  private class DeadState : LevelCamera.CameraState
  {
    private const float _zoomSpeed = 40f;
    private bool _isFollowing;
    private float _distance = 1.5f;

    private Vector3 TargetCheckPoint => LevelCamera.Instance._targetTransform.position + LevelCamera.Instance._targetTransform.up * LevelCamera.Instance.LookAtHeight;

    public override void Update()
    {
      float num = 1f;
      if ((UnityEngine.Object) LevelCamera.Instance._targetTransform == (UnityEngine.Object) null)
        return;
      Vector3 vector3_1 = LevelCamera.Instance._targetTransform.position + Vector3.up * num;
      Vector3 vector3_2 = vector3_1 + Vector3.Normalize(LevelCamera.Instance._transform.position - vector3_1) * this._distance;
      Quaternion to = Quaternion.LookRotation(vector3_1 - LevelCamera.Instance._transform.position);
      if (!this._isFollowing)
      {
        LevelCamera.Instance._transform.position = Vector3.Lerp(LevelCamera.Instance._transform.position, vector3_2, Time.deltaTime * 4f);
        if ((double) Vector3.Distance(vector3_2, LevelCamera.Instance._transform.position) <= (double) this._distance)
          this._isFollowing = true;
      }
      LevelCamera.Instance._transform.rotation = Quaternion.Lerp(LevelCamera.Instance._transform.rotation, to, Time.deltaTime * 4f);
    }

    public override string ToString() => "Smooth follow state";
  }

  private class TurretState : LevelCamera.CameraState
  {
    public override void Update() => LevelCamera.Instance._transform.position = Vector3.Lerp(LevelCamera.Instance._transform.position, LevelCamera.Instance._targetTransform.position, Time.deltaTime * 2f);
  }

  public class OverviewState : LevelCamera.CameraState
  {
    public const float InitialDistance = 7f;
    private const float FinalDistance = 4f;
    private const float InterpolationSpeed = 3f;
    public static readonly Vector3 ViewDirection = new Vector3(-0.5f, -0.1f, -1f);
    public static readonly Vector3 Offset = new Vector3(0.0f, 1.5f, 0.0f);
    private Quaternion _finalRotation;
    private float _distance;

    public OverviewState()
    {
      if ((UnityEngine.Object) GameState.LocalDecorator != (UnityEngine.Object) null)
        LevelCamera.Instance.SetTarget(GameState.LocalDecorator.transform);
      if (!(bool) (UnityEngine.Object) LevelCamera.Instance._targetTransform)
        return;
      this._distance = 7f;
      this._finalRotation = Quaternion.LookRotation(LevelCamera.Instance._targetTransform.TransformDirection(LevelCamera.OverviewState.ViewDirection));
      if ((double) Vector3.Distance(LevelCamera.Instance._transform.position, LevelCamera.Instance._targetTransform.TransformPoint(LevelCamera.Instance._targetTransform.InverseTransformDirection(this._finalRotation * Vector3.back * 4f))) <= 1.0)
        return;
      Quaternion quaternion = Quaternion.LookRotation(LevelCamera.Instance._targetTransform.TransformDirection(new Vector3(-1f, -1f, 1f)));
      LevelCamera.Instance._transform.rotation = quaternion;
      LevelCamera.Instance._transform.position = LevelCamera.Instance._targetTransform.TransformPoint(LevelCamera.Instance._targetTransform.InverseTransformDirection(quaternion * Vector3.back * 7f));
    }

    public override void Update()
    {
      this._distance = Mathf.Lerp(this._distance, 4f, Time.deltaTime * 3f);
      if (!(bool) (UnityEngine.Object) LevelCamera.Instance._targetTransform)
        return;
      this._finalRotation = Quaternion.LookRotation(LevelCamera.Instance._targetTransform.TransformDirection(LevelCamera.OverviewState.ViewDirection));
      Quaternion quaternion = Quaternion.Slerp(LevelCamera.Instance._transform.rotation, this._finalRotation, Time.deltaTime * 3f);
      LevelCamera.Instance._transform.position = Vector3.Lerp(LevelCamera.Instance._transform.position, LevelCamera.Instance._targetTransform.TransformPoint(LevelCamera.Instance._targetTransform.InverseTransformDirection(quaternion * Vector3.back * this._distance)) + LevelCamera.OverviewState.Offset, Time.deltaTime * 3f);
      LevelCamera.Instance._transform.rotation = quaternion;
    }

    public override string ToString() => "Overview State";
  }
}
