// Decompiled with JetBrains decompiler
// Type: ColorConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Globalization;
using UnityEngine;

public static class ColorConverter
{
  public static float GetHue(Color c) => (double) c.r != 0.0 ? ((double) c.g != 0.0 ? (float) (0.0 + ((double) c.g >= 1.0 ? 2.0 - (double) c.r : (double) c.g)) : (float) (4.0 + ((double) c.r >= 1.0 ? 2.0 - (double) c.b : (double) c.r))) : (float) (2.0 + ((double) c.b >= 1.0 ? 2.0 - (double) c.g : (double) c.b));

  public static Color GetColor(float hue)
  {
    hue %= 6f;
    Color color = Color.white;
    color = (double) hue >= 1.0 ? ((double) hue >= 2.0 ? ((double) hue >= 3.0 ? ((double) hue >= 4.0 ? ((double) hue >= 5.0 ? new Color(1f, 0.0f, 6f - hue) : new Color(hue - 4f, 0.0f, 1f)) : new Color(0.0f, 4f - hue, 1f)) : new Color(0.0f, 1f, hue - 2f)) : new Color(2f - hue, 1f, 0.0f)) : new Color(1f, hue, 0.0f);
    return color;
  }

  public static Color HexToColor(string hexString)
  {
    int maxValue1;
    try
    {
      maxValue1 = int.Parse(hexString.Substring(0, 2), NumberStyles.HexNumber);
    }
    catch
    {
      maxValue1 = (int) byte.MaxValue;
    }
    int maxValue2;
    try
    {
      maxValue2 = int.Parse(hexString.Substring(2, 2), NumberStyles.HexNumber);
    }
    catch
    {
      maxValue2 = (int) byte.MaxValue;
    }
    int maxValue3;
    try
    {
      maxValue3 = int.Parse(hexString.Substring(4, 2), NumberStyles.HexNumber);
    }
    catch
    {
      maxValue3 = (int) byte.MaxValue;
    }
    return new Color((float) maxValue1 / (float) byte.MaxValue, (float) maxValue2 / (float) byte.MaxValue, (float) maxValue3 / (float) byte.MaxValue);
  }

  public static string ColorToHex(Color color) => ((int) ((double) color.r * (double) byte.MaxValue)).ToString("X2") + ((int) ((double) color.g * (double) byte.MaxValue)).ToString("X2") + ((int) ((double) color.b * (double) byte.MaxValue)).ToString("X2");

  public static Color RgbToColor(float r, float g, float b) => new Color(r / (float) byte.MaxValue, g / (float) byte.MaxValue, b / (float) byte.MaxValue);

  public static Color RgbaToColor(float r, float g, float b, float a) => new Color(r / (float) byte.MaxValue, g / (float) byte.MaxValue, b / (float) byte.MaxValue, a / (float) byte.MaxValue);

  public static HsvColor RgbToHsv(Color color)
  {
    HsvColor hsv = new HsvColor(0.0f, 0.0f, 0.0f, color.a);
    float r = color.r;
    float g = color.g;
    float b = color.b;
    float num1 = Mathf.Max(r, Mathf.Max(g, b));
    if ((double) num1 <= 0.0)
      return hsv;
    float num2 = Mathf.Min(r, Mathf.Min(g, b));
    float num3 = num1 - num2;
    if ((double) num1 > (double) num2)
    {
      hsv.h = (double) g != (double) num1 ? ((double) b != (double) num1 ? ((double) b <= (double) g ? (float) (((double) g - (double) b) / (double) num3 * 60.0) : (float) (((double) g - (double) b) / (double) num3 * 60.0 + 360.0)) : (float) (((double) r - (double) g) / (double) num3 * 60.0 + 240.0)) : (float) (((double) b - (double) r) / (double) num3 * 60.0 + 120.0);
      if ((double) hsv.h < 0.0)
        hsv.h += 360f;
    }
    else
      hsv.h = 0.0f;
    hsv.h *= 1f / 360f;
    hsv.s = (float) ((double) num3 / (double) num1 * 1.0);
    hsv.v = num1;
    return hsv;
  }

  public static Color HsvToRgb(HsvColor color) => ColorConverter.HsvToRgb(color.h, color.s, color.v, color.a);

  public static Color HsvToRgb(float hue, float saturation, float value) => ColorConverter.HsvToRgb(hue, saturation, value, 1f);

  public static Color HsvToRgb(float hue, float saturation, float value, float alpha)
  {
    float num1 = value;
    float num2 = value;
    float num3 = value;
    if ((double) saturation != 0.0)
    {
      float num4 = value;
      float num5 = value * saturation;
      float num6 = value - num5;
      float num7 = hue * 360f;
      if ((double) num7 < 60.0)
      {
        num1 = num4;
        num2 = (float) ((double) num7 * (double) num5 / 60.0) + num6;
        num3 = num6;
      }
      else if ((double) num7 < 120.0)
      {
        num1 = (float) (-((double) num7 - 120.0) * (double) num5 / 60.0) + num6;
        num2 = num4;
        num3 = num6;
      }
      else if ((double) num7 < 180.0)
      {
        num1 = num6;
        num2 = num4;
        num3 = (float) (((double) num7 - 120.0) * (double) num5 / 60.0) + num6;
      }
      else if ((double) num7 < 240.0)
      {
        num1 = num6;
        num2 = (float) (-((double) num7 - 240.0) * (double) num5 / 60.0) + num6;
        num3 = num4;
      }
      else if ((double) num7 < 300.0)
      {
        num1 = (float) (((double) num7 - 240.0) * (double) num5 / 60.0) + num6;
        num2 = num6;
        num3 = num4;
      }
      else if ((double) num7 <= 360.0)
      {
        num1 = num4;
        num2 = num6;
        num3 = (float) (-((double) num7 - 360.0) * (double) num5 / 60.0) + num6;
      }
      else
      {
        num1 = 0.0f;
        num2 = 0.0f;
        num3 = 0.0f;
      }
    }
    return new Color(Mathf.Clamp01(num1), Mathf.Clamp01(num2), Mathf.Clamp01(num3), alpha);
  }
}
