// Decompiled with JetBrains decompiler
// Type: Texture2DExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class Texture2DExtension
{
  public static Texture2D ChangeHSV(
    this Texture2D texture,
    float hue,
    float saturation = 0,
    float value = 0)
  {
    if (Texture2DExtension.IsSupported(texture.format))
    {
      try
      {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, texture.format, false);
        Color[] pixels = texture.GetPixels();
        for (int index = 0; index < pixels.Length; ++index)
          pixels[index] = HsvColor.FromColor(pixels[index]).Add(hue, saturation, value).ToColor();
        texture2D.SetPixels(pixels);
        texture2D.Apply();
        return texture2D;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("ChangeHue failed on '" + texture.name + "' with Exception: " + ex.Message));
        return texture;
      }
    }
    else
    {
      Debug.LogError((object) ("ChangeHue failed on '" + texture.name + "' because texture format not supported: " + (object) texture.format));
      return texture;
    }
  }

  private static bool IsSupported(TextureFormat format)
  {
    switch (format)
    {
      case TextureFormat.Alpha8:
      case TextureFormat.RGB24:
      case TextureFormat.RGBA32:
      case TextureFormat.ARGB32:
        return true;
      default:
        return false;
    }
  }
}
