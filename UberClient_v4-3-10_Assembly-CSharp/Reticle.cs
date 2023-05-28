// Decompiled with JetBrains decompiler
// Type: Reticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Reticle
{
  private const int STATE_NORMAL = 0;
  private const int STATE_BIG = 1;
  private const int STATE_SMALL = 2;
  private const float DURATION = 1f;
  private Texture _texRotate;
  private Texture _texScale1;
  private Texture _texScale2;
  private Texture _texTranslate;
  private float _innerScaleRatio;
  private float _outterScaleRatio;
  private float _translateDistance;
  private float _currentAngle;
  private float _currentDistance;
  private float _currentInnerRatio;
  private float _currentOutterRatio;
  private int _currentState;
  private float _timer;

  public Reticle()
  {
    this._texRotate = (Texture) null;
    this._texScale1 = (Texture) null;
    this._texScale2 = (Texture) null;
    this._texTranslate = (Texture) null;
    this._innerScaleRatio = 0.0f;
    this._outterScaleRatio = 0.0f;
    this._translateDistance = 0.0f;
  }

  public void SetRotate(Texture image, float angle) => this._texRotate = image;

  public void SetInnerScale(Texture image, float ratio)
  {
    this._texScale1 = image;
    this._innerScaleRatio = ratio;
  }

  public void SetOutterScale(Texture image, float ratio)
  {
    this._texScale2 = image;
    this._outterScaleRatio = ratio;
  }

  public void SetTranslate(Texture image, float distance)
  {
    this._texTranslate = image;
    this._translateDistance = distance;
  }

  public void Update()
  {
    switch (this._currentState)
    {
      case 0:
        this._timer = Mathf.Lerp(this._timer, 0.0f, Time.deltaTime);
        this._currentDistance = Mathf.Lerp(this._currentDistance, 0.0f, Time.deltaTime);
        this._currentInnerRatio = Mathf.Lerp(this._currentInnerRatio, 1f, Time.deltaTime);
        this._currentOutterRatio = Mathf.Lerp(this._currentOutterRatio, 1f, Time.deltaTime);
        break;
      case 1:
        if ((double) this._timer < 1.0)
        {
          this._timer += Time.deltaTime * 5f;
          this._currentAngle = Mathf.Lerp(0.0f, 60f, this._timer / 1f);
          this._currentDistance = (float) ((double) this._translateDistance * (double) this._timer / 1.0);
          this._currentInnerRatio = Mathf.Lerp(1f, this._innerScaleRatio, this._timer / 1f);
          this._currentOutterRatio = Mathf.Lerp(1f, this._outterScaleRatio, this._timer / 1f);
          break;
        }
        this._currentState = 2;
        break;
      case 2:
        if ((double) this._timer > 0.0)
        {
          this._timer -= Time.deltaTime * 5f;
          this._currentDistance = (float) ((double) this._translateDistance * (double) this._timer / 1.0);
          this._currentInnerRatio = Mathf.Lerp(1f, this._innerScaleRatio, this._timer / 1f);
          this._currentOutterRatio = Mathf.Lerp(1f, this._outterScaleRatio, this._timer / 1f);
          break;
        }
        this._currentState = 0;
        break;
    }
  }

  public void Trigger() => this._currentState = 1;

  public void Draw(Rect position)
  {
    Vector2 pivotPoint = new Vector2(position.x + position.width * 0.5f, position.y + position.height * 0.5f);
    if ((bool) (Object) this._texRotate)
    {
      GUIUtility.RotateAroundPivot(this._currentAngle, pivotPoint);
      GUI.DrawTexture(position, this._texRotate);
      GUI.matrix = Matrix4x4.identity;
    }
    if ((bool) (Object) this._texScale1)
    {
      GUIUtility.ScaleAroundPivot(new Vector2(this._currentInnerRatio, this._currentInnerRatio), pivotPoint);
      GUI.DrawTexture(position, this._texScale1);
      GUI.matrix = Matrix4x4.identity;
    }
    if ((bool) (Object) this._texScale2)
    {
      GUIUtility.ScaleAroundPivot(new Vector2(this._currentOutterRatio, this._currentOutterRatio), pivotPoint);
      GUI.DrawTexture(position, this._texScale2);
      GUI.matrix = Matrix4x4.identity;
    }
    if (!(bool) (Object) this._texTranslate)
      return;
    position.x += this._currentDistance;
    GUI.DrawTexture(position, this._texTranslate);
    GUIUtility.RotateAroundPivot(-90f, pivotPoint);
    GUI.DrawTexture(position, this._texTranslate);
    GUIUtility.RotateAroundPivot(-90f, pivotPoint);
    GUI.DrawTexture(position, this._texTranslate);
    GUIUtility.RotateAroundPivot(-90f, pivotPoint);
    GUI.DrawTexture(position, this._texTranslate);
    GUIUtility.RotateAroundPivot(-90f, pivotPoint);
    GUI.matrix = Matrix4x4.identity;
  }
}
