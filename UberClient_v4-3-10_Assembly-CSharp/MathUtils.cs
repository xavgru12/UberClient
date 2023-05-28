// Decompiled with JetBrains decompiler
// Type: MathUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MathUtils
{
  public static float GetQuatLength(Quaternion q) => Mathf.Sqrt((float) ((double) q.x * (double) q.x + (double) q.y * (double) q.y + (double) q.z * (double) q.z + (double) q.w * (double) q.w));

  public static Quaternion GetQuatConjugate(Quaternion q) => new Quaternion(-q.x, -q.y, -q.z, q.w);

  public static Quaternion GetQuatLog(Quaternion q)
  {
    Quaternion quatLog = q with { w = 0.0f };
    if ((double) Mathf.Abs(q.w) < 1.0)
    {
      float f1 = Mathf.Acos(q.w);
      float f2 = Mathf.Sin(f1);
      if ((double) Mathf.Abs(f2) > 0.0001)
      {
        float num = f1 / f2;
        quatLog.x = q.x * num;
        quatLog.y = q.y * num;
        quatLog.z = q.z * num;
      }
    }
    return quatLog;
  }

  public static Quaternion GetQuatExp(Quaternion q)
  {
    Quaternion quatExp = q;
    float f1 = Mathf.Sqrt((float) ((double) q.x * (double) q.x + (double) q.y * (double) q.y + (double) q.z * (double) q.z));
    float f2 = Mathf.Sin(f1);
    quatExp.w = Mathf.Cos(f1);
    if ((double) Mathf.Abs(f2) > 0.0001)
    {
      float num = f2 / f1;
      quatExp.x = num * q.x;
      quatExp.y = num * q.y;
      quatExp.z = num * q.z;
    }
    return quatExp;
  }

  public static Quaternion GetQuatSquad(
    float t,
    Quaternion q0,
    Quaternion q1,
    Quaternion a0,
    Quaternion a1)
  {
    float t1 = (float) (2.0 * (double) t * (1.0 - (double) t));
    return MathUtils.Slerp(MathUtils.Slerp(q0, q1, t), MathUtils.Slerp(a0, a1, t), t1);
  }

  public static Quaternion GetSquadIntermediate(Quaternion q0, Quaternion q1, Quaternion q2)
  {
    Quaternion quatConjugate = MathUtils.GetQuatConjugate(q1);
    Quaternion quatLog1 = MathUtils.GetQuatLog(quatConjugate * q0);
    Quaternion quatLog2 = MathUtils.GetQuatLog(quatConjugate * q2);
    Quaternion q = new Quaternion((float) (-0.25 * ((double) quatLog1.x + (double) quatLog2.x)), (float) (-0.25 * ((double) quatLog1.y + (double) quatLog2.y)), (float) (-0.25 * ((double) quatLog1.z + (double) quatLog2.z)), (float) (-0.25 * ((double) quatLog1.w + (double) quatLog2.w)));
    return q1 * MathUtils.GetQuatExp(q);
  }

  public static float Ease(float t, float k1, float k2)
  {
    float num = (float) ((double) k1 * 2.0 / 3.1415927410125732 + (double) k2 - (double) k1 + (1.0 - (double) k2) * 2.0 / 3.1415927410125732);
    return ((double) t >= (double) k1 ? ((double) t >= (double) k2 ? (float) (2.0 * (double) k1 / 3.1415927410125732 + (double) k2 - (double) k1 + (1.0 - (double) k2) * 0.63661974668502808 * (double) Mathf.Sin((float) (((double) t - (double) k2) / (1.0 - (double) k2) * 3.1415927410125732 / 2.0))) : (float) (2.0 * (double) k1 / 3.1415927410125732) + t - k1) : (float) ((double) k1 * 0.63661974668502808 * ((double) Mathf.Sin((float) ((double) t / (double) k1 * 3.1415927410125732 / 2.0 - 1.5707963705062866)) + 1.0))) / num;
  }

  public static Quaternion Slerp(Quaternion p, Quaternion q, float t)
  {
    float f1 = Quaternion.Dot(p, q);
    Quaternion quaternion;
    if (1.0 + (double) f1 > 1E-05)
    {
      float num1;
      float num2;
      if (1.0 - (double) f1 > 1E-05)
      {
        float f2 = Mathf.Acos(f1);
        float num3 = 1f / Mathf.Sin(f2);
        num1 = Mathf.Sin((1f - t) * f2) * num3;
        num2 = Mathf.Sin(t * f2) * num3;
      }
      else
      {
        num1 = 1f - t;
        num2 = t;
      }
      quaternion.x = (float) ((double) num1 * (double) p.x + (double) num2 * (double) q.x);
      quaternion.y = (float) ((double) num1 * (double) p.y + (double) num2 * (double) q.y);
      quaternion.z = (float) ((double) num1 * (double) p.z + (double) num2 * (double) q.z);
      quaternion.w = (float) ((double) num1 * (double) p.w + (double) num2 * (double) q.w);
    }
    else
    {
      float num4 = Mathf.Sin((float) ((1.0 - (double) t) * 3.1415927410125732 * 0.5));
      float num5 = Mathf.Sin((float) ((double) t * 3.1415927410125732 * 0.5));
      quaternion.x = (float) ((double) num4 * (double) p.x - (double) num5 * (double) p.y);
      quaternion.y = (float) ((double) num4 * (double) p.y + (double) num5 * (double) p.x);
      quaternion.z = (float) ((double) num4 * (double) p.z - (double) num5 * (double) p.w);
      quaternion.w = p.z;
    }
    return quaternion;
  }
}
