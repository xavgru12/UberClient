// Decompiled with JetBrains decompiler
// Type: Mathfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class Mathfx
{
  public const float PI = 3.14159f;
  public const float FOUR_PI = 12.56636f;
  public const float TWO_PI = 6.28318f;
  public const float PI_HALF = 1.570795f;
  public const float PI_FOURTH = 0.7853975f;
  private static readonly Quaternion _rotate90 = Quaternion.AngleAxis(90f, Vector3.up);

  public static float NormAmplitude(float a) => (float) (((double) a + 1.0) * 0.5);

  public static float Hermite(float start, float end, float value) => Mathf.Lerp(start, end, (float) ((double) value * (double) value * (3.0 - 2.0 * (double) value)));

  public static float Gauss(float start, float end, float value) => Mathf.Lerp(start, end, (float) ((1.0 + (double) Mathf.Cos(value - 3.14159274f)) / 2.0));

  public static float Sinerp(float start, float end, float value) => Mathf.Lerp(start, end, Mathf.Sin((float) ((double) value * 3.1415927410125732 * 0.5)));

  public static float Berp(float start, float end, float value)
  {
    value = Mathf.Clamp01(value);
    value = (float) (((double) Mathf.Sin((float) ((double) value * 3.1415927410125732 * (0.20000000298023224 + 2.5 * (double) value * (double) value * (double) value))) * (double) Mathf.Pow(1f - value, 2.2f) + (double) value) * (1.0 + 1.2000000476837158 * (1.0 - (double) value)));
    return start + (end - start) * value;
  }

  public static float SmoothStep(float x, float min, float max)
  {
    x = Mathf.Clamp(x, min, max);
    float num1 = (float) (((double) x - (double) min) / ((double) max - (double) min));
    float num2 = (float) (((double) x - (double) min) / ((double) max - (double) min));
    return (float) (-2.0 * (double) num1 * (double) num1 * (double) num1 + 3.0 * (double) num2 * (double) num2);
  }

  public static float Lerp(float start, float end, float value) => (float) ((1.0 - (double) value) * (double) start + (double) value * (double) end);

  public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
  {
    Vector3 vector3 = Vector3.Normalize(lineEnd - lineStart);
    float num = Vector3.Dot(point - lineStart, vector3) / Vector3.Dot(vector3, vector3);
    return lineStart + num * vector3;
  }

  public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
  {
    Vector3 a = lineEnd - lineStart;
    Vector3 vector3 = Vector3.Normalize(a);
    float num = Vector3.Dot(point - lineStart, vector3) / Vector3.Dot(vector3, vector3);
    return lineStart + Mathf.Clamp(num, 0.0f, Vector3.Magnitude(a)) * vector3;
  }

  public static float Bounce(float x) => Mathf.Abs(Mathf.Sin((float) (6.28000020980835 * ((double) x + 1.0) * ((double) x + 1.0))) * (1f - x));

  public static bool Approx(float val, float about, float range) => (double) Mathf.Abs(val - about) < (double) range;

  public static bool Approx(Vector3 val, Vector3 about, float range) => (double) (val - about).sqrMagnitude < (double) range * (double) range;

  public static float ProjectedAngle(Vector3 a, Vector3 b)
  {
    float num = Vector3.Angle(a, b);
    return (double) Vector3.Dot(a, Mathfx._rotate90 * b) < 0.0 ? 360f - num : num;
  }

  public static Vector3 ProjectVector3(Vector3 v, Vector3 normal) => v - Vector3.Dot(v, normal) * normal;

  public static float ClampAngle(float angle, float min, float max) => Mathf.Clamp(angle % 360f, min, max);

  public static int Sign(float s)
  {
    if ((double) s == 0.0)
      return 0;
    return (double) s < 0.0 ? -1 : 1;
  }

  public static short Clamp(short v, short min, short max)
  {
    if ((int) v < (int) min)
      return min;
    return (int) v > (int) max ? max : v;
  }

  public static int Clamp(int v, int min, int max)
  {
    if (v < min)
      return min;
    return v > max ? max : v;
  }

  public static float Clamp(float v, float min, float max)
  {
    if ((double) v < (double) min)
      return min;
    return (double) v > (double) max ? max : v;
  }

  public static byte Clamp(byte v, byte min, byte max)
  {
    if ((int) v < (int) min)
      return min;
    return (int) v > (int) max ? max : v;
  }

  public static float Ease(float t, EaseType easeType)
  {
    switch (easeType)
    {
      case EaseType.In:
        return Mathf.Lerp(0.0f, 1f, 1f - Mathf.Cos((float) ((double) t * 3.1415927410125732 * 0.5)));
      case EaseType.Out:
        return Mathf.Lerp(0.0f, 1f, Mathf.Sin((float) ((double) t * 3.1415927410125732 * 0.5)));
      case EaseType.InOut:
        return Mathf.SmoothStep(0.0f, 1f, t);
      case EaseType.Berp:
        return Mathfx.Berp(0.0f, 1f, t);
      default:
        return t;
    }
  }

  public static Vector2 RotateVector2AboutPoint(Vector2 input, Vector2 center, float degRotate)
  {
    Vector3 vector3 = Quaternion.AngleAxis(degRotate, new Vector3(0.0f, 0.0f, 1f)) * (Vector3) (input - center);
    return center + new Vector2(vector3.x, vector3.y);
  }
}
