// Decompiled with JetBrains decompiler
// Type: TouchSwipeBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class TouchSwipeBar : TouchControl
{
  private bool enabled;
  public int SwipeThreshold = 60;
  public GUIContent Content;
  public GUIStyle Style;
  private Vector2 _touchStartPos;

  public TouchSwipeBar()
  {
    this.OnTouchBegan += new Action<Vector2>(this.OnSwipeBarTouchBegan);
    this.OnTouchMoved += new Action<Vector2, Vector2>(this.OnSwipeBarTouchMoved);
  }

  public TouchSwipeBar(Texture tex)
    : this()
  {
    this.Content = new GUIContent(tex);
  }

  public event Action OnSwipeUp;

  public event Action OnSwipeDown;

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
    }
  }

  public bool Active => this.Enabled && this.finger.FingerId != -1;

  private void OnSwipeBarTouchBegan(Vector2 obj) => this._touchStartPos = this.finger.StartPos;

  private void OnSwipeBarTouchMoved(Vector2 pos, Vector2 delta)
  {
    if ((double) this._touchStartPos.y - (double) pos.y > (double) this.SwipeThreshold)
    {
      this._touchStartPos = pos;
      if (this.OnSwipeDown == null)
        return;
      this.OnSwipeDown();
    }
    else
    {
      if ((double) this._touchStartPos.y - (double) pos.y >= (double) -this.SwipeThreshold)
        return;
      this._touchStartPos = pos;
      if (this.OnSwipeUp == null)
        return;
      this.OnSwipeUp();
    }
  }

  public override void Draw()
  {
    if (this.Content == null)
      return;
    GUI.Label(this.Boundary, this.Content);
  }
}
