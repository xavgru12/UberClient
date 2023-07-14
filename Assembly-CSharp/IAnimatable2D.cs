// Decompiled with JetBrains decompiler
// Type: IAnimatable2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public interface IAnimatable2D
{
  void Draw(float offsetX = 0.0f, float offsetY = 0.0f);

  Vector2 GetPosition();

  Vector2 GetCenter();

  Rect GetRect();

  void Show();

  void Hide();

  void FadeColorTo(Color destColor, float time, EaseType easeType);

  void FadeColor(Color deltaColor, float time, EaseType easeType);

  void FadeAlphaTo(float destAlpha, float time, EaseType easeType);

  void FadeAlpha(float deltaAlpha, float time, EaseType easeType);

  void MoveTo(Vector2 destPosition, float time, EaseType easeType, float startDelay);

  void Move(Vector2 deltaPosition, float time, EaseType easeType);

  void ScaleTo(Vector2 destScale, float time, EaseType easeType);

  void ScaleDelta(Vector2 scaleFactor, float time, EaseType easeType);

  void ScaleToAroundPivot(Vector2 destScale, Vector2 pivot, float time, EaseType easeType);

  void ScaleAroundPivot(Vector2 scaleFactor, Vector2 pivot, float time, EaseType easeType);

  void Flicker(float time, float flickerInterval = 0.02f);

  void StopFading();

  void StopMoving();

  void StopScaling();

  void StopFlickering();

  void FreeObject();
}
