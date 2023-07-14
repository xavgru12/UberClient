// Decompiled with JetBrains decompiler
// Type: CameraFeedback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
public class CameraFeedback : MonoBehaviour
{
  public bool DEBUG = true;
  private static CameraFeedback _instance;
  public CameraFeedback.Feedback[] Feedbacks;
  private CameraFeedback.Feedback _currentFeedback;
  private Transform _transformCache;
  private Quaternion _tmpRotation;
  private Vector3 _rotationAxis;
  private float _timer;
  private float _angle;
  private float _testAngle;

  public static CameraFeedback Instance => CameraFeedback._instance;

  private void Awake()
  {
    CameraFeedback._instance = this;
    this._transformCache = this.transform;
    this._currentFeedback = (CameraFeedback.Feedback) null;
    this._tmpRotation = Quaternion.identity;
  }

  private void Update()
  {
    if (this._currentFeedback == null)
    {
      if ((double) this._transformCache.localPosition.sqrMagnitude <= 1.0 / 1000.0)
        return;
      this._transformCache.localPosition = Vector3.Lerp(this._transformCache.localPosition, Vector3.zero, Time.deltaTime);
      this._transformCache.localRotation = Quaternion.Lerp(this._transformCache.localRotation, Quaternion.identity, Time.deltaTime);
    }
    else
    {
      Vector3 direction = this._currentFeedback.GetDirection();
      float peak = this._currentFeedback.Peak;
      float from = UnityEngine.Random.Range(-this._currentFeedback.MaxNoise, this._currentFeedback.MaxNoise);
      if ((double) this._timer < (double) this._currentFeedback.TimeToEnd + (double) this._currentFeedback.TimeToPeak)
      {
        float num1;
        float num2;
        if ((double) this._timer < (double) this._currentFeedback.TimeToPeak)
        {
          num1 = peak * Mathf.Sin((float) ((double) this._timer * 3.1415927410125732 * 0.5) / this._currentFeedback.TimeToPeak);
          num2 = Mathf.Lerp(from, 0.0f, this._timer / this._currentFeedback.TimeToPeak);
          this._angle = Mathf.Lerp(0.0f, this._currentFeedback.MaxAngle, this._timer / this._currentFeedback.TimeToPeak);
        }
        else
        {
          float t = (this._timer - this._currentFeedback.TimeToPeak) / this._currentFeedback.TimeToEnd;
          num1 = Mathf.Lerp(peak, 0.0f, t);
          this._angle = Mathf.Lerp(this._angle, 0.0f, t);
          num2 = 0.0f;
        }
        this._timer += Time.deltaTime;
        this._transformCache.localPosition = num1 * direction + this._transformCache.right * num2 + this._transformCache.up * num2;
        this._tmpRotation = Quaternion.AngleAxis(this._angle, this._rotationAxis);
        this._testAngle = this._angle;
      }
      else
      {
        this._timer = 0.0f;
        this._tmpRotation = Quaternion.identity;
        this._currentFeedback = (CameraFeedback.Feedback) null;
      }
    }
  }

  private void OnGUI()
  {
    if (!this.DEBUG)
      return;
    this.DoApplyFeedback();
  }

  private void DoApplyFeedback()
  {
    GUI.Label(new Rect(10f, 50f, 300f, 20f), "Camera local position = " + (object) this._transformCache.localPosition);
    GUI.Label(new Rect(10f, 60f, 300f, 20f), "Camera world position = " + (object) this._transformCache.position);
    GUI.Label(new Rect(10f, 70f, 300f, 20f), "Rotation Axis = " + (object) this._rotationAxis);
    GUI.Label(new Rect(10f, 80f, 300f, 20f), "Rotation Angle = " + (object) this._testAngle);
    if (GUI.Button(new Rect(10f, 100f, 60f, 25f), "Land"))
      this.onPlayerLand(new PlayerLandEvent());
    if (GUI.Button(new Rect(80f, 100f, 60f, 25f), "Damage"))
      this.onDamage(new GetDamageEvent(Vector3.back));
    if (GUI.Button(new Rect(150f, 100f, 60f, 25f), "Weapon"))
      this.onWeaponShoot(new WeaponShootEvent(Vector3.back, this.Feedbacks[2].MaxNoise, this.Feedbacks[2].MaxAngle));
    Vector3[] vector3Array = new Vector3[3]
    {
      new Vector3(-0.8f, -0.3f, 0.6f),
      new Vector3(-0.8f, -0.1f, 0.6f),
      new Vector3(0.5f, -0.7f, 0.5f)
    };
    for (int index = 0; index < vector3Array.Length; ++index)
    {
      if (GUI.Button(new Rect(10f, (float) (125 + 25 * index), 100f, 25f), "Damage " + (object) index))
        this.onDamage(new GetDamageEvent(vector3Array[index]));
    }
  }

  private void OnDrawGizmos()
  {
    if (!(bool) (UnityEngine.Object) this._transformCache)
      this._transformCache = this.transform;
    Gizmos.color = Color.green;
    Gizmos.DrawRay(this._transformCache.position, this.Feedbacks[1].GetDirection());
  }

  private void onPlayerLand(PlayerLandEvent ev) => this.ApplyFeedback(CameraFeedback.FeedbackType.Land, Vector3.down, Vector3.right);

  private void onDamage(GetDamageEvent ev) => this.ApplyFeedback(CameraFeedback.FeedbackType.Damage, ev.Force, Vector3.zero);

  private void onWeaponShoot(WeaponShootEvent ev)
  {
    this.Feedbacks[2].Peak = 1f;
    this.Feedbacks[2].MaxNoise = ev.Noise;
    this.Feedbacks[2].MaxAngle = ev.Angle;
    this.ApplyFeedback(CameraFeedback.FeedbackType.Weapon, ev.Force, Vector3.left);
  }

  public void ApplyFeedback(CameraFeedback.FeedbackType t, Vector3 dir, Vector3 rotAxis)
  {
    this._timer = 0.0f;
    this._currentFeedback = this.Feedbacks[(int) t];
    this._currentFeedback.SetDirection(dir);
    this._rotationAxis = !(rotAxis == Vector3.zero) ? rotAxis : this._transformCache.InverseTransformDirection(Vector3.Cross(Vector3.up, dir));
  }

  public void ApplyFeedback(Vector3 dir, float noise, float angle)
  {
    this.Feedbacks[2].Peak = 1f;
    this.Feedbacks[2].MaxNoise = noise;
    this.Feedbacks[2].MaxAngle = angle;
    this.ApplyFeedback(CameraFeedback.FeedbackType.Weapon, dir, Vector3.left);
  }

  public Quaternion GetFeedbackRoation() => this._tmpRotation;

  public enum FeedbackType
  {
    Land,
    Damage,
    Weapon,
  }

  [Serializable]
  public class Feedback
  {
    public CameraFeedback.FeedbackType Type;
    public float Peak;
    public float TimeToPeak;
    public float TimeToEnd;
    public float MaxNoise;
    public float MaxAngle;
    private Vector3 _dir;

    public Feedback(CameraFeedback.Feedback f)
    {
      this._dir = Vector3.zero;
      this.Type = f.Type;
      this.Peak = f.Peak;
      this.TimeToPeak = f.TimeToPeak;
      this.TimeToEnd = f.TimeToEnd;
    }

    public void SetDirection(Vector3 dir) => this._dir = dir;

    public Vector3 GetDirection() => this._dir;
  }
}
