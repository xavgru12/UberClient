// Decompiled with JetBrains decompiler
// Type: SplineInterpolator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SplineInterpolator : MonoBehaviour
{
  private eEndPointsMode mEndPointsMode;
  public float Speed = 10f;
  private List<SplineInterpolator.SplineNode> mNodes = new List<SplineInterpolator.SplineNode>();
  private string mState = string.Empty;
  private bool mRotations;
  private OnEndCallback mOnEndCallback;
  private GameObject mTarget;
  private float mCurrentTime;
  private int mCurrentIdx = 1;
  private Vector3 _currentPosition;
  private Quaternion _currentRotation;

  public bool IsStopped => this.mState == "Stopped";

  public Vector3 Position => this._currentPosition;

  public Quaternion Rotation => this._currentRotation;

  private void Awake() => this.Reset();

  public void StartInterpolation(
    GameObject target,
    OnEndCallback endCallback,
    bool bRotations,
    eWrapMode mode)
  {
    this.mState = mode != eWrapMode.ONCE ? "Loop" : "Once";
    this.mRotations = bRotations;
    this.mOnEndCallback = endCallback;
    this.mTarget = target;
    this.SetInput();
  }

  public void Reset()
  {
    this.mNodes.Clear();
    this.mState = nameof (Reset);
    this.mCurrentIdx = 1;
    this.mCurrentTime = 0.0f;
    this.mRotations = false;
    this.mTarget = (GameObject) null;
    this.mEndPointsMode = eEndPointsMode.AUTO;
  }

  public void AddPoint(Vector3 pos, Quaternion quat, float timeInSeconds, Vector2 easeInOut)
  {
    if (this.mState != "Reset")
      throw new Exception("Cannot add points after start");
    this.mNodes.Add(new SplineInterpolator.SplineNode(pos, quat, timeInSeconds, easeInOut));
  }

  private void SetInput()
  {
    if (this.mNodes.Count < 2)
      throw new Exception("Invalid number of points");
    if (this.mRotations)
    {
      for (int index = 1; index < this.mNodes.Count; ++index)
      {
        SplineInterpolator.SplineNode mNode1 = this.mNodes[index];
        SplineInterpolator.SplineNode mNode2 = this.mNodes[index - 1];
        if ((double) Quaternion.Dot(mNode1.Rot, mNode2.Rot) < 0.0)
        {
          mNode1.Rot.x = -mNode1.Rot.x;
          mNode1.Rot.y = -mNode1.Rot.y;
          mNode1.Rot.z = -mNode1.Rot.z;
          mNode1.Rot.w = -mNode1.Rot.w;
        }
      }
    }
    if (this.mEndPointsMode == eEndPointsMode.AUTO)
    {
      this.mNodes.Insert(0, this.mNodes[0]);
      this.mNodes.Add(this.mNodes[this.mNodes.Count - 1]);
    }
    else if (this.mEndPointsMode == eEndPointsMode.EXPLICIT && this.mNodes.Count < 4)
      throw new Exception("Invalid number of points");
  }

  private void SetExplicitMode()
  {
    if (this.mState != "Reset")
      throw new Exception("Cannot change mode after start");
    this.mEndPointsMode = eEndPointsMode.EXPLICIT;
  }

  public void SetAutoCloseMode(float joiningPointTime)
  {
    if (this.mState != "Reset")
      throw new Exception("Cannot change mode after start");
    this.mEndPointsMode = eEndPointsMode.AUTOCLOSED;
    this.mNodes.Add(new SplineInterpolator.SplineNode(this.mNodes[0]));
    this.mNodes[this.mNodes.Count - 1].Time = joiningPointTime;
    Vector3 normalized1 = (this.mNodes[1].Point - this.mNodes[0].Point).normalized;
    Vector3 normalized2 = (this.mNodes[this.mNodes.Count - 2].Point - this.mNodes[this.mNodes.Count - 1].Point).normalized;
    float magnitude1 = (this.mNodes[1].Point - this.mNodes[0].Point).magnitude;
    float magnitude2 = (this.mNodes[this.mNodes.Count - 2].Point - this.mNodes[this.mNodes.Count - 1].Point).magnitude;
    SplineInterpolator.SplineNode splineNode1 = new SplineInterpolator.SplineNode(this.mNodes[0]);
    splineNode1.Point = this.mNodes[0].Point + normalized2 * magnitude1;
    SplineInterpolator.SplineNode splineNode2 = new SplineInterpolator.SplineNode(this.mNodes[this.mNodes.Count - 1]);
    splineNode2.Point = this.mNodes[0].Point + normalized1 * magnitude2;
    this.mNodes.Insert(0, splineNode1);
    this.mNodes.Add(splineNode2);
  }

  private void Update()
  {
    if (this.mState == "Reset" || this.mState == "Stopped" || this.mNodes.Count < 4)
      return;
    this.mCurrentTime += Time.deltaTime;
    if ((double) this.mCurrentTime >= (double) this.mNodes[this.mCurrentIdx + 1].Time)
    {
      if (this.mCurrentIdx < this.mNodes.Count - 3)
        ++this.mCurrentIdx;
      else if (this.mState != "Loop")
      {
        this.Stop();
      }
      else
      {
        this.mCurrentIdx = 1;
        this.mCurrentTime = 0.0f;
      }
    }
    if (!(this.mState != "Stopped"))
      return;
    float t1 = (float) (((double) this.mCurrentTime - (double) this.mNodes[this.mCurrentIdx].Time) / ((double) this.mNodes[this.mCurrentIdx + 1].Time - (double) this.mNodes[this.mCurrentIdx].Time));
    float t2 = MathUtils.Ease(Mathf.Clamp(t1, 0.0f, 0.999999f), this.mNodes[this.mCurrentIdx].EaseIO.x, this.mNodes[this.mCurrentIdx].EaseIO.y);
    this._currentPosition = this.GetHermiteInternal(this.mCurrentIdx, t1);
    this._currentRotation = this.GetSquad(this.mCurrentIdx, t2);
    if (!(bool) (UnityEngine.Object) this.mTarget)
      return;
    this.mTarget.transform.position = this._currentPosition;
    if (!this.mRotations)
      return;
    this.mTarget.transform.rotation = this._currentRotation;
  }

  private Quaternion GetSquad(int idxFirstPoint, float t)
  {
    Quaternion rot1 = this.mNodes[idxFirstPoint - 1].Rot;
    Quaternion rot2 = this.mNodes[idxFirstPoint].Rot;
    Quaternion rot3 = this.mNodes[idxFirstPoint + 1].Rot;
    Quaternion rot4 = this.mNodes[idxFirstPoint + 2].Rot;
    Quaternion squadIntermediate1 = MathUtils.GetSquadIntermediate(rot1, rot2, rot3);
    Quaternion squadIntermediate2 = MathUtils.GetSquadIntermediate(rot2, rot3, rot4);
    return MathUtils.GetQuatSquad(t, rot2, rot3, squadIntermediate1, squadIntermediate2);
  }

  public Vector3 GetHermiteInternal(int idxFirstPoint, float t)
  {
    float num1 = t * t;
    float num2 = num1 * t;
    Vector3 point1 = this.mNodes[idxFirstPoint - 1].Point;
    Vector3 point2 = this.mNodes[idxFirstPoint].Point;
    Vector3 point3 = this.mNodes[idxFirstPoint + 1].Point;
    Vector3 point4 = this.mNodes[idxFirstPoint + 2].Point;
    float num3 = 0.5f;
    Vector3 vector3_1 = num3 * (point3 - point1);
    Vector3 vector3_2 = num3 * (point4 - point2);
    float num4 = (float) (2.0 * (double) num2 - 3.0 * (double) num1 + 1.0);
    float num5 = (float) (-2.0 * (double) num2 + 3.0 * (double) num1);
    float num6 = num2 - 2f * num1 + t;
    float num7 = num2 - num1;
    return num4 * point2 + num5 * point3 + num6 * vector3_1 + num7 * vector3_2;
  }

  public Vector3 GetHermiteAtTime(float timeParam)
  {
    if ((double) timeParam >= (double) this.mNodes[this.mNodes.Count - 2].Time)
      return this.mNodes[this.mNodes.Count - 2].Point;
    int index = 1;
    while (index < this.mNodes.Count - 2 && (double) this.mNodes[index].Time <= (double) timeParam)
      ++index;
    int num = index - 1;
    float t = MathUtils.Ease((float) (((double) timeParam - (double) this.mNodes[num].Time) / ((double) this.mNodes[num + 1].Time - (double) this.mNodes[num].Time)), this.mNodes[num].EaseIO.x, this.mNodes[num].EaseIO.y);
    return this.GetHermiteInternal(num, t);
  }

  public void Stop()
  {
    this.mState = "Stopped";
    if (this.mOnEndCallback == null)
      return;
    this.mOnEndCallback();
  }

  internal class SplineNode
  {
    internal Vector3 Point;
    internal Quaternion Rot;
    internal float Time;
    internal Vector2 EaseIO;

    internal SplineNode(Vector3 p, Quaternion q, float t, Vector2 io)
    {
      this.Point = p;
      this.Rot = q;
      this.Time = t;
      this.EaseIO = io;
    }

    internal SplineNode(SplineInterpolator.SplineNode o)
    {
      this.Point = o.Point;
      this.Rot = o.Rot;
      this.Time = o.Time;
      this.EaseIO = o.EaseIO;
    }
  }
}
