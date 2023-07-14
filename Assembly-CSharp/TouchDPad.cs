// Decompiled with JetBrains decompiler
// Type: TouchDPad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class TouchDPad : TouchBaseControl
{
  private bool enabled;
  public Vector2 TapDelay = new Vector2(0.2f, 0.2f);
  public Vector2 MoveInteriaRolloff = new Vector2(12f, 10f);
  private float _rotation;
  public TouchButton JumpButton;
  public TouchButton CrouchButton;
  private GUIContent _dpad;
  private Vector2 _centerPosition;
  public float MinGUIAlpha = 0.3f;
  private Rect _leftRect;
  private Rect _rightRect;
  private Rect _forwardRect;
  private Rect _backwardRect;
  private Rect _dpadRect;
  private Dictionary<int, TouchBaseControl.TouchFinger> _fingers;
  private Vector2 _lastDirection;

  public TouchDPad()
  {
    this._fingers = new Dictionary<int, TouchBaseControl.TouchFinger>();
    this._lastDirection = Vector2.zero;
    this.Moving = false;
  }

  public TouchDPad(Texture dpad)
    : this()
  {
    this._dpad = new GUIContent(dpad);
    this.JumpButton = new TouchButton();
    this.CrouchButton = new TouchButton();
  }

  public Vector2 TopLeftPosition
  {
    set
    {
      this._dpadRect = new Rect(value.x, value.y, (float) this._dpad.image.width, (float) this._dpad.image.height);
      this._leftRect = new Rect(value.x, value.y, 104f, 209f);
      this._forwardRect = new Rect(value.x + 104f, value.y, 104f, 104f);
      this._backwardRect = new Rect(value.x + 104f, value.y + 104f, 104f, 106f);
      this._rightRect = new Rect(value.x + 207f, value.y + 103f, 104f, 106f);
      this._centerPosition = new Vector2(value.x + 155f, value.y + 103f);
      if (this.CrouchButton != null)
        this.CrouchButton.Boundary = new Rect(value.x + 311f, value.y + 103f, 88f, 106f);
      if (this.JumpButton == null)
        return;
      this.JumpButton.Boundary = new Rect(value.x + 207f, value.y, 192f, 104f);
    }
  }

  public override bool Enabled
  {
    get => this.enabled;
    set
    {
      if (value == this.enabled)
        return;
      this.enabled = value;
      if (this.JumpButton != null)
        this.JumpButton.Enabled = value;
      if (this.CrouchButton != null)
        this.CrouchButton.Enabled = value;
      this._lastDirection = Vector2.zero;
      this.Direction = Vector2.zero;
      if (this.enabled)
        return;
      this._fingers.Clear();
      this.Moving = false;
    }
  }

  public float Rotation
  {
    get => this._rotation;
    set
    {
      this._rotation = value;
      this.JumpButton.SetRotation(value, this._centerPosition);
      this.CrouchButton.SetRotation(value, this._centerPosition);
    }
  }

  public Vector2 Direction { get; private set; }

  public bool Moving { get; private set; }

  public bool InsideBoundary(Vector2 position) => this._forwardRect.ContainsTouch(position) || this._leftRect.ContainsTouch(position) || this._rightRect.ContainsTouch(position) || this._backwardRect.ContainsTouch(position);

  public override void UpdateTouches(Touch touch)
  {
    Vector2 position = Mathfx.RotateVector2AboutPoint(touch.position, new Vector2(this._centerPosition.x, (float) Screen.height - this._centerPosition.y), this._rotation);
    if (touch.phase == TouchPhase.Began && this.InsideBoundary(position))
    {
      this._fingers.Remove(touch.fingerId);
      this._fingers.Add(touch.fingerId, new TouchBaseControl.TouchFinger()
      {
        StartPos = position,
        StartTouchTime = Time.time,
        LastPos = position,
        FingerId = touch.fingerId
      });
    }
    else if (touch.phase == TouchPhase.Moved)
    {
      if (!this._fingers.ContainsKey(touch.fingerId))
        return;
      this._fingers[touch.fingerId].LastPos = position;
    }
    else
    {
      if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
        return;
      this._fingers.Remove(touch.fingerId);
    }
  }

  public override void FinalUpdate()
  {
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    foreach (TouchBaseControl.TouchFinger touchFinger in this._fingers.Values)
    {
      if (this._leftRect.ContainsTouch(touchFinger.LastPos))
        flag1 = true;
      else if (this._rightRect.ContainsTouch(touchFinger.LastPos))
        flag2 = true;
      else if (this._forwardRect.ContainsTouch(touchFinger.LastPos))
        flag3 = true;
      else if (this._backwardRect.ContainsTouch(touchFinger.LastPos))
        flag4 = true;
    }
    Vector2 zero = Vector2.zero;
    if (flag1)
      zero += new Vector2(-1f, 0.0f);
    if (flag2)
      zero += new Vector2(1f, 0.0f);
    if (flag3)
      zero += new Vector2(0.0f, 1f);
    if (flag4)
      zero += new Vector2(0.0f, -1f);
    this.Moving = flag1 || flag2 || flag3 || flag4;
    if ((double) zero.y == 0.0)
      zero.y = Mathf.Lerp(this._lastDirection.y, zero.y, Time.deltaTime * this.MoveInteriaRolloff.y);
    if ((double) zero.x == 0.0)
      zero.x = Mathf.Lerp(this._lastDirection.x, zero.x, Time.deltaTime * this.MoveInteriaRolloff.x);
    this._lastDirection = this.Direction;
    this.Direction = zero;
  }

  public override void Draw()
  {
    GUI.color = new Color(1f, 1f, 1f, Mathf.Clamp(Singleton<TouchController>.Instance.GUIAlpha, this.MinGUIAlpha, 1f));
    GUIUtility.RotateAroundPivot(this._rotation, this._centerPosition);
    GUI.Label(this._dpadRect, this._dpad);
    GUI.matrix = Matrix4x4.identity;
    GUI.color = Color.white;
  }
}
