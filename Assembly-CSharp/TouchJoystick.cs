// Decompiled with JetBrains decompiler
// Type: TouchJoystick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class TouchJoystick : TouchBaseControl
{
  public Texture JoystickTexture;
  public Texture BackgroundTexture;
  public Vector2 MoveInteriaRolloff = new Vector2(6f, 5f);
  public float MinGUIAlpha = 0.4f;
  private bool enabled;
  private TouchBaseControl.TouchFinger _finger;
  private Rect _joystickBoundary;
  private Vector2 _joystickPos;

  public TouchJoystick() => this._finger = new TouchBaseControl.TouchFinger();

  public TouchJoystick(Texture joystick, Texture background)
    : this()
  {
    this.JoystickTexture = joystick;
    this.BackgroundTexture = background;
  }

  public event Action<Vector2> OnJoystickMoved;

  public event Action OnJoystickStopped;

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
      if (this._finger.FingerId != -1 && this.OnJoystickStopped != null)
        this.OnJoystickStopped();
      this._finger.Reset();
    }
  }

  public override void UpdateTouches(Touch touch)
  {
    if (touch.phase == TouchPhase.Began && this._finger.FingerId == -1 && this.Boundary.ContainsTouch(touch.position))
    {
      this._finger = new TouchBaseControl.TouchFinger()
      {
        StartPos = touch.position,
        StartTouchTime = Time.time,
        LastPos = touch.position,
        FingerId = touch.fingerId
      };
      this._joystickBoundary = new Rect(touch.position.x - (float) (this.JoystickTexture.width / 2), touch.position.y - (float) (this.JoystickTexture.height / 2), (float) this.JoystickTexture.width, (float) this.JoystickTexture.height);
    }
    else
    {
      if (this._finger.FingerId != touch.fingerId)
        return;
      if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
      {
        this._joystickPos.x = Mathf.Clamp(touch.position.x, this._joystickBoundary.x, this._joystickBoundary.x + this._joystickBoundary.width);
        this._joystickPos.y = Mathf.Clamp(touch.position.y, this._joystickBoundary.y, this._joystickBoundary.y + this._joystickBoundary.height);
        this._finger.LastPos = touch.position;
        Vector2 vector2 = Vector2.zero with
        {
          x = (float) (((double) this._joystickPos.x - (double) this._finger.StartPos.x) * 2.0) / this._joystickBoundary.width,
          y = (float) (((double) this._joystickPos.y - (double) this._finger.StartPos.y) * 2.0) / this._joystickBoundary.height
        } * ApplicationDataManager.ApplicationOptions.TouchMoveSensitivity;
        if (touch.phase != TouchPhase.Moved || this.OnJoystickMoved == null)
          return;
        this.OnJoystickMoved(vector2);
      }
      else
      {
        if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
          return;
        if (this.OnJoystickStopped != null)
          this.OnJoystickStopped();
        this._finger.Reset();
      }
    }
  }

  public override void Draw()
  {
    if (this._finger.FingerId == -1)
      return;
    GUI.Label(new Rect(this._joystickPos.x - (float) (this.JoystickTexture.width / 2), (float) Screen.height - this._joystickPos.y - (float) (this.JoystickTexture.height / 2), (float) this.JoystickTexture.width, (float) this.JoystickTexture.height), this.JoystickTexture);
    GUI.color = new Color(1f, 1f, 1f, Mathf.Clamp(Singleton<TouchController>.Instance.GUIAlpha, this.MinGUIAlpha, 1f));
    GUI.Label(new Rect(this._finger.StartPos.x - (float) (this.BackgroundTexture.width / 2), (float) Screen.height - this._finger.StartPos.y - (float) (this.BackgroundTexture.height / 2), (float) this.BackgroundTexture.width, (float) this.BackgroundTexture.height), this.BackgroundTexture);
    GUI.color = Color.white;
  }
}
