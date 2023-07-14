// Decompiled with JetBrains decompiler
// Type: HsvColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public struct HsvColor
{
  public float h;
  public float s;
  public float v;
  public float a;

  public HsvColor(float h, float s, float b, float a)
  {
    this.h = h;
    this.s = s;
    this.v = b;
    this.a = a;
  }

  public HsvColor(float h, float s, float b)
  {
    this.h = h;
    this.s = s;
    this.v = b;
    this.a = 1f;
  }

  public HsvColor(Color col)
  {
    HsvColor hsv = ColorConverter.RgbToHsv(col);
    this.h = hsv.h;
    this.s = hsv.s;
    this.v = hsv.v;
    this.a = hsv.a;
  }

  public static HsvColor FromColor(Color color) => ColorConverter.RgbToHsv(color);

  public Color ToColor() => ColorConverter.HsvToRgb(this);

  public HsvColor Add(float hue, float saturation, float value)
  {
    this.h += hue;
    this.s += saturation;
    this.v += value;
    while ((double) this.h > 1.0)
      --this.h;
    while ((double) this.h < 0.0)
      ++this.h;
    return this;
  }

  public HsvColor AddHue(float hue)
  {
    this.h += hue;
    while ((double) this.h > 1.0)
      --this.h;
    while ((double) this.h < 0.0)
      ++this.h;
    return this;
  }

  public HsvColor AddSaturation(float saturation)
  {
    this.s += saturation;
    return this;
  }

  public HsvColor AddValue(float value)
  {
    this.v += value;
    return this;
  }
}
