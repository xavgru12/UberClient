// Decompiled with JetBrains decompiler
// Type: TouchButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class TouchButton : TouchControl
{
  public const float LongPressTime = 0.4f;
  public GUIContent Content;
  public GUIStyle Style;
  public float MinGUIAlpha;
  private bool _touchStarted;
  private bool _touchSent;

  public TouchButton()
  {
    this.OnTouchBegan += new Action<Vector2>(this.OnTouchButtonBegan);
    this.OnTouchEnded += new Action<Vector2>(this.OnTouchButtonEnded);
  }

  public TouchButton(string title, GUIStyle style)
    : this()
  {
    this.Content = new GUIContent(title);
    this.Style = style;
  }

  public TouchButton(Texture texture)
    : this()
  {
    this.Content = new GUIContent(texture);
  }

  public event Action OnPushed;

  public event Action OnLongPress;

  ~TouchButton()
  {
    this.OnTouchBegan -= new Action<Vector2>(this.OnTouchButtonBegan);
    this.OnTouchEnded -= new Action<Vector2>(this.OnTouchButtonEnded);
  }

  public override void UpdateTouches(Touch touch)
  {
    base.UpdateTouches(touch);
    if (!this._touchStarted || this._touchSent || (double) this.finger.StartTouchTime + 0.40000000596046448 >= (double) Time.time)
      return;
    if (this.OnLongPress != null)
      this.OnLongPress();
    else if (this.OnPushed != null)
      this.OnPushed();
    this._touchSent = true;
  }

  public override void Draw()
  {
    if (this.Content == null)
      return;
    GUI.color = new Color(1f, 1f, 1f, Mathf.Clamp(Singleton<TouchController>.Instance.GUIAlpha, this.MinGUIAlpha, 1f));
    if ((double) this._rotationAngle != 0.0)
      GUIUtility.RotateAroundPivot(this._rotationAngle, this._rotationPoint);
    if (this.Style != null)
      GUI.Label(this.Boundary, this.Content, this.Style);
    else
      GUI.Label(this.Boundary, this.Content);
    if ((double) this._rotationAngle != 0.0)
      GUI.matrix = Matrix4x4.identity;
    GUI.color = Color.white;
  }

  private void OnTouchButtonEnded(Vector2 pos)
  {
    if (this._touchSent || this.OnPushed == null)
      return;
    this.OnPushed();
  }

  private void OnTouchButtonBegan(Vector2 pos)
  {
    this._touchSent = false;
    this._touchStarted = true;
  }

  protected override void ResetTouch()
  {
    base.ResetTouch();
    this._touchStarted = false;
    this._touchSent = false;
  }
}
