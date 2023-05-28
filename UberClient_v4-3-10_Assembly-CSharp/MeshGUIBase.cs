// Decompiled with JetBrains decompiler
// Type: MeshGUIBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public abstract class MeshGUIBase : IAnimatable2D
{
  protected GameObject _meshObject;
  protected CustomMesh _customeMesh;
  protected ColorAnim _colorAnim;
  protected Vector2Anim _positionAnim;
  protected FlickerAnim _flickerAnim;
  protected Vector2Anim _scaleAnim;
  protected Rect _rect;
  private float _depth;
  private Vector2 _size;
  private Vector2 _center;
  private Vector2 _parentPosition;
  private Vector2 _scaleAnimPivot;
  private bool _isEnabled = true;
  private bool _isScaleAnimAroundPivot;

  public MeshGUIBase(GameObject parentObject)
  {
    if (!MeshGUIManager.Exists)
      return;
    this._meshObject = this.AllocObject(parentObject);
    this._customeMesh = this.GetCustomMesh();
    this._positionAnim = new Vector2Anim(new Vector2Anim.OnValueChange(this.OnPositionChange));
    this._scaleAnim = new Vector2Anim(new Vector2Anim.OnValueChange(this.OnScaleChange));
    this._scaleAnim.Vec2 = Vector2.one;
    this._colorAnim = new ColorAnim(new ColorAnim.OnValueChange(this.OnColorChange));
    this._flickerAnim = new FlickerAnim(new Action<FlickerAnim>(this.UpdateVisible));
    this.ResetGUI();
  }

  public abstract void FreeObject();

  public abstract Vector2 GetOriginalBounds();

  protected abstract GameObject AllocObject(GameObject parentObject);

  protected abstract CustomMesh GetCustomMesh();

  protected abstract Vector2 GetAdjustScale();

  protected abstract void UpdateRect();

  public virtual void Draw(float offsetX = 0.0f, float offsetY = 0.0f)
  {
    if ((double) this._parentPosition.x != (double) offsetX || (double) this._parentPosition.y != (double) offsetY)
      this.ParentPosition = new Vector2(offsetX, offsetY);
    this._flickerAnim.Update();
    this._colorAnim.Update();
    this._positionAnim.Update();
    this._scaleAnim.Update();
  }

  public Vector2 GetPosition() => this.Position;

  public Vector2 GetCenter() => this.Center;

  public Rect GetRect() => this.Rect;

  public void Show()
  {
    if (!this.IsEnabled)
      return;
    this.IsVisible = true;
    if (!(bool) (UnityEngine.Object) this._customeMesh)
      return;
    this._customeMesh.IsVisible = true;
  }

  public void Hide()
  {
    this.IsVisible = false;
    if (!(bool) (UnityEngine.Object) this._customeMesh)
      return;
    this._customeMesh.IsVisible = false;
  }

  public void StopFading() => this._colorAnim.StopFading();

  public void StopMoving() => this._positionAnim.StopAnim();

  public void StopScaling() => this._scaleAnim.StopAnim();

  public void StopFlickering() => this._flickerAnim.StopAnim();

  public void FadeColorTo(Color destColor, float time = 0.0f, EaseType easeType = EaseType.None) => this._colorAnim.FadeColorTo(destColor, time, easeType);

  public void FadeColor(Color deltaColor, float time = 0.0f, EaseType easeType = EaseType.None) => this._colorAnim.FadeColor(deltaColor, time, easeType);

  public void FadeAlphaTo(float destAlpha, float time = 0.0f, EaseType easeType = EaseType.None) => this._colorAnim.FadeAlphaTo(destAlpha, time, easeType);

  public void FadeAlpha(float deltaAlpha, float time = 0.0f, EaseType easeType = EaseType.None) => this._colorAnim.FadeAlpha(deltaAlpha, time, easeType);

  public void MoveTo(Vector2 destPosition, float time = 0.0f, EaseType easeType = EaseType.None, float startDelay = 0) => this._positionAnim.AnimTo(destPosition, time, easeType, startDelay);

  public void Move(Vector2 deltaPosition, float time = 0.0f, EaseType easeType = EaseType.None) => this._positionAnim.AnimBy(deltaPosition, time, easeType);

  public void ScaleTo(Vector2 destScale, float time = 0.0f, EaseType easeType = EaseType.None) => this._scaleAnim.AnimTo(destScale, time, easeType, 0.0f);

  public void ScaleDelta(Vector2 scaleFactor, float time = 0.0f, EaseType easeType = EaseType.None) => this._scaleAnim.AnimBy(scaleFactor, time, easeType);

  public void ScaleToAroundPivot(Vector2 destScale, Vector2 pivot, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    this.ScaleTo(destScale, time, easeType);
    this._isScaleAnimAroundPivot = true;
    this._scaleAnimPivot = pivot;
  }

  public void ScaleAroundPivot(Vector2 scaleFactor, Vector2 pivot, float time = 0.0f, EaseType easeType = EaseType.None)
  {
    Vector2 destScale;
    destScale.x = this.Scale.x * scaleFactor.x;
    destScale.y = this.Scale.y * scaleFactor.y;
    this.ScaleToAroundPivot(destScale, pivot, time, easeType);
  }

  public void Flicker(float time, float flickerInterval = 0.02f) => this._flickerAnim.Flicker(time, flickerInterval);

  public bool IsFlickering() => this._flickerAnim.IsAnimating;

  protected void ResetGUI()
  {
    this._parentPosition = Vector2.zero;
    this._depth = 0.0f;
    this.Color = Color.white;
    this.Position = Vector2.zero;
    this.Scale = Vector2.one;
    this.Show();
    this.UpdateRect();
  }

  public string Name
  {
    get => this._customeMesh.name;
    set => this._customeMesh.name = value;
  }

  public Color Color
  {
    get => this._colorAnim.Color;
    set => this._colorAnim.Color = value;
  }

  public float Alpha
  {
    get => this._colorAnim.Alpha;
    set => this._colorAnim.Alpha = value;
  }

  public Vector2 ParentPosition
  {
    get => this._parentPosition;
    set
    {
      this._parentPosition = value;
      this.UpdatePosition();
    }
  }

  public Vector2 Position
  {
    get => this._positionAnim.Vec2;
    set => this._positionAnim.Vec2 = value;
  }

  public float Depth
  {
    get => this._depth;
    set
    {
      this._depth = value;
      this.UpdatePosition();
    }
  }

  public Vector2 Scale
  {
    get => this._scaleAnim.Vec2;
    set => this._scaleAnim.Vec2 = value;
  }

  public Vector2 Size
  {
    get
    {
      Vector2 originalBounds = this.GetOriginalBounds();
      this._size.x = this.Scale.x * originalBounds.x;
      this._size.y = this.Scale.y * originalBounds.y;
      return this._size;
    }
  }

  public Vector2 Center
  {
    get
    {
      this.UpdateRect();
      this._center.x = this._rect.x + this.Size.x / 2f;
      this._center.y = this._rect.y + this.Size.y / 2f;
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

  public Vector2 Rotation { get; set; }

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

  private void OnColorChange(Color oldColor, Color newColor)
  {
    this._customeMesh.Color = this._colorAnim.Color;
    this._customeMesh.Alpha = this._colorAnim.Alpha;
  }

  private void OnScaleChange(Vector2 oldScale, Vector2 newScale)
  {
    if (this._scaleAnim.IsAnimating && this._isScaleAnimAroundPivot)
    {
      Vector2 vector2;
      vector2.x = (double) oldScale.x <= 0.0 ? this.Position.x - this.Size.x / 2f : (this.Position.x - this._scaleAnimPivot.x) * newScale.x / oldScale.x + this._scaleAnimPivot.x;
      vector2.y = (double) oldScale.y <= 0.0 ? this.Position.y - this.Size.y / 2f : (this.Position.y - this._scaleAnimPivot.y) * newScale.y / oldScale.y + this._scaleAnimPivot.y;
      this.Position = vector2;
    }
    Vector2 adjustScale = this.GetAdjustScale();
    this._meshObject.transform.localScale = new Vector3(newScale.x * adjustScale.x, newScale.y * adjustScale.y, 1f);
    this.UpdateRect();
  }

  private void OnPositionChange(Vector2 oldPosition, Vector2 newPosition) => this.UpdatePosition();

  private void UpdateVisible(FlickerAnim animation)
  {
    if ((UnityEngine.Object) this._customeMesh == (UnityEngine.Object) null)
      return;
    if (animation.IsAnimating)
      this._customeMesh.IsVisible = this.IsVisible && animation.IsFlickerVisible;
    else
      this._customeMesh.IsVisible = this.IsVisible;
  }

  private void UpdatePosition()
  {
    this._meshObject.transform.position = MeshGUIManager.Instance.TransformPosFromScreenToWorld(this._parentPosition + this._positionAnim.Vec2) with
    {
      z = this._depth
    };
    this.UpdateRect();
  }
}
