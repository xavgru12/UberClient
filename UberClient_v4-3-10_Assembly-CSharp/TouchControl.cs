// Decompiled with JetBrains decompiler
// Type: TouchControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class TouchControl : TouchBaseControl
{
  public TouchBaseControl.TouchFinger finger;
  private bool enabled;
  protected float _rotationAngle;
  protected Vector2 _rotationPoint = Vector2.zero;
  private bool _inside;

  public TouchControl() => this.finger = new TouchBaseControl.TouchFinger();

  public event Action<Vector2> OnTouchBegan;

  public event Action<Vector2, Vector2> OnTouchLeftBoundary;

  public event Action<Vector2, Vector2> OnTouchMoved;

  public event Action<Vector2, Vector2> OnTouchEnteredBoundary;

  public event Action<Vector2> OnTouchEnded;

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
      this.finger.Reset();
      this._inside = false;
    }
  }

  public bool IsActive => this.finger.FingerId != -1;

  public void SetRotation(float angle, Vector2 point)
  {
    this._rotationAngle = angle;
    this._rotationPoint = point;
  }

  public override void UpdateTouches(Touch touch)
  {
    if (this.finger.FingerId != -1 && touch.fingerId != this.finger.FingerId || this.finger.FingerId == -1 && touch.phase != TouchPhase.Began)
      return;
    Vector2 position = touch.position;
    if ((double) this._rotationAngle != 0.0)
      position = Mathfx.RotateVector2AboutPoint(touch.position, new Vector2(this._rotationPoint.x, (float) Screen.height - this._rotationPoint.y), this._rotationAngle);
    switch (touch.phase)
    {
      case TouchPhase.Began:
        if (!this.TouchInside(position))
          break;
        this.finger.StartPos = position;
        this.finger.LastPos = position;
        this.finger.StartTouchTime = Time.time;
        this.finger.FingerId = touch.fingerId;
        this._inside = true;
        if (this.OnTouchBegan == null)
          break;
        this.OnTouchBegan(position);
        break;
      case TouchPhase.Moved:
      case TouchPhase.Stationary:
        bool flag = this.TouchInside(position);
        if (this._inside && !flag)
        {
          this._inside = false;
          if (this.OnTouchLeftBoundary != null)
            this.OnTouchLeftBoundary(position, touch.deltaPosition);
        }
        else if (!this._inside && flag)
        {
          this._inside = true;
          if (this.OnTouchEnteredBoundary != null)
            this.OnTouchEnteredBoundary(position, touch.deltaPosition);
        }
        if (this.OnTouchMoved != null)
          this.OnTouchMoved(position, touch.deltaPosition);
        this.finger.LastPos = position;
        break;
      case TouchPhase.Ended:
      case TouchPhase.Canceled:
        if (this.OnTouchEnded != null)
          this.OnTouchEnded(position);
        this.ResetTouch();
        break;
    }
  }

  protected virtual void ResetTouch()
  {
    this.finger.Reset();
    this._inside = false;
  }

  protected virtual bool TouchInside(Vector2 position) => this.Boundary.ContainsTouch(position);
}
