// Decompiled with JetBrains decompiler
// Type: TouchShooter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public class TouchShooter : TouchBaseControl
{
  public float SecondaryFireTapDelay = 0.4f;
  public float SecondaryFireTapMaxDistanceSqr = 10000f;
  private bool enabled;
  private TouchBaseControl.TouchFinger _primaryFinger;
  private TouchBaseControl.TouchFinger _secondaryFinger;
  private float _lastFireTouch;
  private Vector2 _lastFirePosition = Vector2.zero;
  private ArrayList _ignoreTouches;

  public TouchShooter()
  {
    this._primaryFinger = new TouchBaseControl.TouchFinger();
    this._secondaryFinger = new TouchBaseControl.TouchFinger();
    this._ignoreTouches = new ArrayList();
  }

  public event Action<Vector2> OnDoubleTap;

  public event Action OnFireStart;

  public event Action OnFireEnd;

  public Vector2 Aim { get; private set; }

  public override bool Enabled
  {
    get => this.enabled;
    set
    {
      if (value == this.enabled)
        return;
      this.enabled = value;
      if (this.enabled)
        return;
      this._primaryFinger = new TouchBaseControl.TouchFinger();
      this._secondaryFinger = new TouchBaseControl.TouchFinger();
      this.Aim = Vector2.zero;
    }
  }

  public override void UpdateTouches(Touch touch)
  {
    if (touch.phase == TouchPhase.Began && this.Boundary.ContainsTouch(touch.position) && this.ValidArea(touch.position))
    {
      if (this._primaryFinger.FingerId == -1)
      {
        this._primaryFinger = new TouchBaseControl.TouchFinger()
        {
          StartPos = touch.position,
          StartTouchTime = Time.time,
          LastPos = touch.position,
          FingerId = touch.fingerId
        };
        if ((double) this._lastFireTouch + (double) this.SecondaryFireTapDelay > (double) Time.time && (double) (this._lastFirePosition - touch.position).sqrMagnitude < (double) this.SecondaryFireTapMaxDistanceSqr)
        {
          if (this.OnDoubleTap == null)
            return;
          this.OnDoubleTap(touch.position);
        }
        else
        {
          this._lastFireTouch = Time.time;
          this._lastFirePosition = touch.position;
        }
      }
      else
      {
        if (this._primaryFinger.FingerId == touch.fingerId || this._secondaryFinger.FingerId != -1)
          return;
        this._secondaryFinger = new TouchBaseControl.TouchFinger()
        {
          StartPos = touch.position,
          StartTouchTime = Time.time,
          LastPos = touch.position,
          FingerId = touch.fingerId
        };
        if (this.OnFireStart == null)
          return;
        this.OnFireStart();
      }
    }
    else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
    {
      if (this._primaryFinger.FingerId != touch.fingerId)
        return;
      this.Aim = touch.deltaPosition * 500f / (float) Screen.width;
    }
    else
    {
      if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
        return;
      if (this._primaryFinger.FingerId == touch.fingerId)
      {
        this._primaryFinger.Reset();
        this.Aim = Vector2.zero;
      }
      else
      {
        if (this._secondaryFinger.FingerId != touch.fingerId)
          return;
        if (this.OnFireEnd != null)
          this.OnFireEnd();
        this._secondaryFinger.Reset();
      }
    }
  }

  public void IgnoreRect(Rect r)
  {
    if (this._ignoreTouches.Contains((object) r))
      return;
    this._ignoreTouches.Add((object) r);
  }

  public void UnignoreRect(Rect r)
  {
    if (!this._ignoreTouches.Contains((object) r))
      return;
    this._ignoreTouches.Remove((object) r);
  }

  private bool ValidArea(Vector2 pos)
  {
    if (this._ignoreTouches.Count == 0)
      return true;
    foreach (Rect ignoreTouch in this._ignoreTouches)
    {
      if (ignoreTouch.ContainsTouch(pos))
        return false;
    }
    return true;
  }
}
