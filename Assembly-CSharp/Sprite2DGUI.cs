// Decompiled with JetBrains decompiler
// Type: Sprite2DGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Sprite2DGUI : IAnimatable2D
{
  private ColorAnim _colorAnim;
  private Vector2Anim _positionAnim;
  private FlickerAnim _flickerAnim;
  private Vector2Anim _scaleAnim;
  private GUIContent _content;
  private GUIStyle _style;
  private Vector2 _size;
  private Vector2 _center;
  private Rect _rect;
  private bool _visible;
  private bool _isScaleAnimAroundPivot;
  private Vector2 _scaleAnimPivot;

  public Sprite2DGUI(GUIContent content, GUIStyle style)
  {
    this._content = content;
    this._style = style;
    this._positionAnim = new Vector2Anim();
    this._scaleAnim = new Vector2Anim(new Vector2Anim.OnValueChange(this.OnScaleChange));
    this._colorAnim = new ColorAnim();
    this._colorAnim.Color = Color.white;
    this._flickerAnim = new FlickerAnim();
    this._size = this.GUIBounds;
    this._rect = new Rect();
    this._visible = true;
  }

  public GUIStyle Style
  {
    get => this._style;
    set => this._style = value;
  }

  public void Draw(float offsetX = 0.0f, float offsetY = 0.0f)
  {
    if (!this._flickerAnim.IsAnimating ? this._visible : this._visible && this._flickerAnim.IsFlickerVisible)
    {
      GUITools.BeginGUIColor(this._colorAnim.Color);
      Rect rect = this.Rect;
      rect.x += offsetX;
      rect.y += offsetY;
      GUI.Label(rect, this._content, this._style);
      GUITools.EndGUIColor();
    }
    this._flickerAnim.Update();
    this._colorAnim.Update();
    this._positionAnim.Update();
    this._scaleAnim.Update();
  }

  public Vector2 GetPosition() => this.Position;

  public Vector2 GetCenter() => this.Center;

  public Rect GetRect() => this.Rect;

  public void Show() => this.Visible = true;

  public void Hide() => this.Visible = false;

  public void FreeObject()
  {
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

  public Vector2 Position
  {
    get => this._positionAnim.Vec2;
    set => this._positionAnim.Vec2 = value;
  }

  public Vector2 Scale
  {
    get => this._scaleAnim.Vec2;
    set => this._scaleAnim.Vec2 = value;
  }

  public Vector2 GUIBounds => this._style.CalcSize(this._content);

  public Vector2 Size
  {
    get
    {
      this._size.x = this.Scale.x * this.GUIBounds.x;
      this._size.y = this.Scale.y * this.GUIBounds.y;
      return this._size;
    }
  }

  public Vector2 Center
  {
    get
    {
      this._center = this.Position + this.Size / 2f;
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

  public bool Visible
  {
    get => this._visible;
    set => this._visible = value;
  }

  private void UpdateRect()
  {
    this._rect.x = this.Position.x - (float) ((double) Screen.width * (1.0 - (double) Singleton<CameraRectController>.Instance.Width) / 2.0);
    this._rect.y = this.Position.y;
    this._rect.width = this.Size.x;
    this._rect.height = this.Size.y;
  }

  private void OnScaleChange(Vector2 oldScale, Vector2 newScale)
  {
    if (!this._scaleAnim.IsAnimating || !this._isScaleAnimAroundPivot)
      return;
    Vector2 vector2;
    vector2.x = (double) oldScale.x <= 0.0 ? this.Position.x - this.Size.x / 2f : (this.Position.x - this._scaleAnimPivot.x) * newScale.x / oldScale.x + this._scaleAnimPivot.x;
    vector2.y = (double) oldScale.y <= 0.0 ? this.Position.y - this.Size.y / 2f : (this.Position.y - this._scaleAnimPivot.y) * newScale.y / oldScale.y + this._scaleAnimPivot.y;
    this.Position = vector2;
  }
}
