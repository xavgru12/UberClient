// Decompiled with JetBrains decompiler
// Type: SplineController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof (SplineInterpolator))]
public class SplineController : MonoBehaviour
{
  public GameObject Target;
  public GameObject SplineRoot;
  public float Duration = 10f;
  public eOrientationMode OrientationMode;
  public eWrapMode WrapMode;
  public bool AutoStart;
  public bool AutoClose;
  public bool HideOnExecute = true;
  public bool LerpInitialPos;
  private SplineInterpolator mSplineInterp;
  private Transform[] mTransforms;

  public bool SplineMovementDone => (UnityEngine.Object) this.mSplineInterp != (UnityEngine.Object) null && this.mSplineInterp.IsStopped;

  private void Awake()
  {
    this.mSplineInterp = this.GetComponent<SplineInterpolator>();
    Profiler.enabled = true;
  }

  [DebuggerHidden]
  private IEnumerator Start() => (IEnumerator) new SplineController.\u003CStart\u003Ec__Iterator85()
  {
    \u003C\u003Ef__this = this
  };

  private void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans)
  {
    interp.Reset();
    float num = !this.AutoClose ? this.Duration / (float) (trans.Length - 1) : this.Duration / (float) trans.Length;
    int index;
    for (index = 0; index < trans.Length; ++index)
    {
      if (this.OrientationMode == eOrientationMode.NODE)
        interp.AddPoint(trans[index].position, trans[index].rotation, num * (float) index, new Vector2(0.0f, 1f));
      else if (this.OrientationMode == eOrientationMode.TANGENT)
      {
        Quaternion quat = index == trans.Length - 1 ? (!this.AutoClose ? trans[index].rotation : Quaternion.LookRotation(trans[0].position - trans[index].position, trans[index].up)) : Quaternion.LookRotation(trans[index + 1].position - trans[index].position, trans[index].up);
        interp.AddPoint(trans[index].position, quat, num * (float) index, new Vector2(0.0f, 1f));
      }
    }
    if (!this.AutoClose)
      return;
    interp.SetAutoCloseMode(num * (float) index);
  }

  private Transform[] GetTransforms()
  {
    if (!((UnityEngine.Object) this.SplineRoot != (UnityEngine.Object) null))
      return (Transform[]) null;
    List<Transform> transformList = new List<Transform>((IEnumerable<Transform>) this.SplineRoot.GetComponentsInChildren<Transform>(true));
    transformList.Remove(this.SplineRoot.transform);
    transformList.Sort((Comparison<Transform>) ((a, b) => a.name.CompareTo(b.name)));
    return transformList.ToArray();
  }

  private void DisableTransforms()
  {
    if (!((UnityEngine.Object) this.SplineRoot != (UnityEngine.Object) null))
      return;
    this.SplineRoot.SetActive(false);
  }

  public void FollowSpline(OnEndCallback callback = null)
  {
    this.mTransforms = this.GetTransforms();
    if (this.mTransforms.Length <= 0)
      return;
    if (this.LerpInitialPos)
      this.mTransforms[0] = this.Target.transform;
    this.SetupSplineInterpolator(this.mSplineInterp, this.mTransforms);
    this.mSplineInterp.StartInterpolation(this.Target, (OnEndCallback) null, true, this.WrapMode);
  }

  public void Reset()
  {
    if (!(bool) (UnityEngine.Object) this.Target || this.mTransforms.Length <= 0)
      return;
    this.Target.transform.position = this.mTransforms[0].position;
    this.Target.transform.rotation = this.mTransforms[0].rotation;
  }

  public void Stop()
  {
    if (!(bool) (UnityEngine.Object) this.mSplineInterp)
      return;
    this.mSplineInterp.Stop();
  }
}
