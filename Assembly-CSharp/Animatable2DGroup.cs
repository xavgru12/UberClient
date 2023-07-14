// Decompiled with JetBrains decompiler
// Type: Animatable2DGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Animatable2DGroup : IAnimatable2D
{
  private List<IAnimatable2D> _group;
  private Vector2 _center;
  private Rect _rect;
  private Vector2 _position;
  private bool _isEnabled = true;
  private bool _isAnimatingPosition;
  private Vector2 _positionAnimSrc;
  private Vector2 _positionAnimDest;
  private float _positionAnimTime;
  private float _positionAnimStartTime;
  private EaseType _positionAnimEaseType;

  public Animatable2DGroup() => this._group = new List<IAnimatable2D>();

  public void Draw(float offsetX = 0.0f, float offsetY = 0.0f)
  {
    this.AnimPosition();
    foreach (IAnimatable2D animatable2D in this._group)
      animatable2D.Draw(this._position.x + offsetX, this._position.y + offsetY);
  }

  public Vector2 GetPosition() => this.Position;

  public Vector2 GetCenter() => this.Center;

  public Rect GetRect() => this.Rect;

  public void Show()
  {
    if (!this.IsEnabled)
      return;
    foreach (IAnimatable2D animatable2D in this._group)
      animatable2D.Show();
    this.IsVisible = true;
  }

  public void Hide()
  {
    foreach (IAnimatable2D animatable2D in this._group)
      animatable2D.Hide();
    this.IsVisible = false;
  }

  public void FadeColorTo(Color destColor, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    foreach (IAnimatable2D animatable2D in this._group)
      animatable2D.FadeColorTo(destColor, time, easeType);
  }

  public void FadeColor(Color deltaColor, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    foreach (IAnimatable2D animatable2D in this._group)
      animatable2D.FadeColorTo(deltaColor, time, easeType);
  }

  public void FadeAlphaTo(float destAlpha, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    foreach (IAnimatable2D animatable2D in this._group)
      animatable2D.FadeAlphaTo(destAlpha, time, easeType);
  }

  public void FadeAlpha(float deltaAlpha, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    foreach (IAnimatable2D animatable2D in this._group)
      animatable2D.FadeAlpha(deltaAlpha, time, easeType);
  }

  public void MoveTo(Vector2 destPosition, float time = 0, EaseType easeType = EaseType.None, float startDelay = 0)
  {
    if ((double) time <= 0.0)
    {
      this._position = destPosition;
      this.UpdateRect();
    }
    else
    {
      this._isAnimatingPosition = true;
      this._positionAnimSrc = this._position;
      this._positionAnimDest = destPosition;
      this._positionAnimTime = time;
      this._positionAnimEaseType = easeType;
      this._positionAnimStartTime = Time.time + startDelay;
    }
  }

  public void Move(Vector2 deltaPosition, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    foreach (IAnimatable2D animatable2D in this._group)
      animatable2D.Move(deltaPosition, time, easeType);
  }

  public void ScaleTo(Vector2 destScale, float time = 0.0f, EaseType easeType = EaseType.None) => this.ScaleDelta(destScale, time, easeType);

  public void ScaleDelta(Vector2 scaleFactor, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    foreach (IAnimatable2D animatable2D in this._group)
      animatable2D.ScaleDelta(scaleFactor, time, easeType);
  }

  public void ScaleToAroundPivot(Vector2 destScale, Vector2 pivot, float time = 0.0f, EaseType easeType = EaseType.None) => this.ScaleAroundPivot(destScale, pivot, time, easeType);

  public void ScaleAroundPivot(Vector2 scaleFactor, Vector2 pivot, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    foreach (IAnimatable2D animatable2D in this._group)
    {
      if (animatable2D is Animatable2DGroup)
        animatable2D.ScaleAroundPivot(scaleFactor, pivot - animatable2D.GetPosition(), time, easeType);
      else
        animatable2D.ScaleAroundPivot(scaleFactor, pivot, time, easeType);
    }
  }

  public void Flicker(float time, float flickerInterval = 0.02f)
  {
    foreach (IAnimatable2D animatable2D in this._group)
      animatable2D.Flicker(time, flickerInterval);
  }

  public void StopFading()
  {
    foreach (IAnimatable2D animatable2D in this.Group)
      animatable2D.StopFading();
  }

  public void StopMoving()
  {
    this._isAnimatingPosition = false;
    foreach (IAnimatable2D animatable2D in this.Group)
      animatable2D.StopMoving();
  }

  public void StopScaling()
  {
    foreach (IAnimatable2D animatable2D in this.Group)
      animatable2D.StopScaling();
  }

  public void StopFlickering()
  {
    foreach (IAnimatable2D animatable2D in this.Group)
      animatable2D.StopFlickering();
  }

  public void RemoveAndFree(int index)
  {
    if (index < 0 || index >= this._group.Count)
      return;
    this._group[index].FreeObject();
    this._group.RemoveAt(index);
  }

  public void RemoveAndFree(IAnimatable2D animatable)
  {
    if (!this._group.Contains(animatable))
      return;
    animatable.FreeObject();
    this._group.Remove(animatable);
  }

  public void ClearAndFree()
  {
    this.FreeObject();
    this._group.Clear();
  }

  public void UpdateMeshGUIPosition(float offsetX = 0.0f, float offsetY = 0.0f)
  {
    foreach (IAnimatable2D animatable2D in this._group)
    {
      if (animatable2D is MeshGUIBase)
        (animatable2D as MeshGUIBase).ParentPosition = this._position + new Vector2(offsetX, offsetY);
      else if (animatable2D is Animatable2DGroup)
        (animatable2D as Animatable2DGroup).UpdateMeshGUIPosition(this._position.x + offsetX, this._position.y + offsetY);
    }
  }

  public void FreeObject()
  {
    foreach (IAnimatable2D animatable2D in this._group)
      animatable2D.FreeObject();
  }

  private void AnimPosition()
  {
    if (!this._isAnimatingPosition)
      return;
    float num = Time.time - this._positionAnimStartTime;
    if ((double) num <= (double) this._positionAnimTime)
    {
      this._position = Vector2.Lerp(this._positionAnimSrc, this._positionAnimDest, Mathfx.Ease(Mathf.Clamp01(num * (1f / this._positionAnimTime)), this._positionAnimEaseType));
      this.UpdateRect();
    }
    else
    {
      this._isAnimatingPosition = false;
      this._position = this._positionAnimDest;
    }
  }

  private void UpdateCenter()
  {
    if (this._group.Count <= 0)
      return;
    this._center = Vector2.zero;
    foreach (IAnimatable2D animatable2D in this._group)
      this._center += animatable2D.GetCenter();
    this._center /= (float) this._group.Count;
  }

  private void UpdateRect()
  {
    if (this._group.Count > 0)
    {
      Vector2 zero1 = Vector2.zero;
      Vector2 zero2 = Vector2.zero;
      foreach (IAnimatable2D animatable2D in this._group)
      {
        Rect rect = animatable2D.GetRect();
        Vector2 vector2 = new Vector2(rect.x + rect.width, rect.y + rect.height);
        zero1.x = (double) zero1.x >= (double) rect.x ? rect.x : zero1.x;
        zero1.y = (double) zero1.y >= (double) rect.y ? rect.y : zero1.y;
        zero2.x = (double) zero2.x <= (double) vector2.x ? vector2.x : zero2.x;
        zero2.y = (double) zero2.y <= (double) vector2.y ? vector2.y : zero2.y;
      }
      this._rect.x = zero1.x;
      this._rect.y = zero1.y;
      this._rect.width = zero2.x - zero1.x;
      this._rect.height = zero2.y - zero1.y;
      this._rect.x += this.Position.x;
      this._rect.y += this.Position.y;
    }
    else
    {
      this._rect.x = this._position.x;
      this._rect.y = this._position.y;
      ref Rect local = ref this._rect;
      float num1 = 0.0f;
      this._rect.height = num1;
      double num2 = (double) num1;
      local.width = (float) num2;
    }
  }

  public List<IAnimatable2D> Group
  {
    get => this._group;
    set => this._group = value;
  }

  public Vector2 Center
  {
    get
    {
      this.UpdateCenter();
      return this._center;
    }
  }

  public Rect Rect
  {
    get
    {
      this.UpdateRect();
      return this._rect;
    }
  }

  public Vector2 Position
  {
    get => this._position;
    set => this._position = value;
  }

  public bool IsEnabled
  {
    get => this._isEnabled;
    set
    {
      this._isEnabled = value;
      if (this._isEnabled)
        this.Show();
      else
        this.Hide();
    }
  }

  public bool IsVisible { get; private set; }
}
