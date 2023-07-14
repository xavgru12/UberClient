// Decompiled with JetBrains decompiler
// Type: BitmapFont
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BitmapFont : MonoBehaviour
{
  public Color Color = Color.white;
  public float AlphaMin = 0.49f;
  public float AlphaMax = 0.51f;
  public Color ShadowColor = Color.black;
  public float ShadowAlphaMin = 0.28f;
  public float ShadowAlphaMax = 0.49f;
  public Vector2 ShadowOffset = new Vector2(-0.05f, 0.08f);
  public float Size;
  public float LineHeight;
  public float Base;
  public float ScaleW;
  public float ScaleH;
  public BitmapChar[] Chars;
  public Rect[] PageOffsets;
  public Texture2D PageAtlas;
  public BitmapCharKerning[] Kernings;
  private Material pageMaterial;
  private Dictionary<int, Material> fontMaterials = new Dictionary<int, Material>();

  public BitmapChar GetBitmapChar(int c)
  {
    foreach (BitmapChar bitmapChar in this.Chars)
    {
      if (c == bitmapChar.Id)
        return bitmapChar;
    }
    return this.Chars[0];
  }

  public Rect GetUVRect(int c) => this.GetUVRect(this.GetBitmapChar(c));

  public Rect GetUVRect(BitmapChar bitmapChar)
  {
    Vector2 vector2_1 = new Vector2(bitmapChar.Size.x / this.ScaleW, bitmapChar.Size.y / this.ScaleH);
    Vector2 vector2_2 = new Vector2(bitmapChar.Position.x / this.ScaleW, bitmapChar.Position.y / this.ScaleH);
    Vector2 vector2_3 = new Vector2(vector2_2.x, (float) (1.0 - ((double) vector2_2.y + (double) vector2_1.y)));
    Rect pageOffset = this.PageOffsets[bitmapChar.Page];
    vector2_3 = new Vector2(vector2_3.x * pageOffset.width + pageOffset.xMin, vector2_3.y * pageOffset.height + pageOffset.yMin);
    vector2_1 = new Vector2(vector2_1.x * pageOffset.width, vector2_1.y * pageOffset.height);
    return new Rect(vector2_3.x, vector2_3.y, vector2_1.x, vector2_1.y);
  }

  public Material CreateFontMaterial(Shader shader) => new Material(shader);

  public void UpdateFontMaterial(Material fontMaterial)
  {
    fontMaterial.color = this.Color;
    fontMaterial.mainTexture = (Texture) this.PageAtlas;
    fontMaterial.SetFloat("_AlphaMin", this.AlphaMin);
    fontMaterial.SetFloat("_AlphaMax", this.AlphaMax);
    fontMaterial.SetColor("_ShadowColor", this.ShadowColor);
    fontMaterial.SetFloat("_ShadowAlphaMin", this.ShadowAlphaMin);
    fontMaterial.SetFloat("_ShadowAlphaMax", this.ShadowAlphaMax);
    fontMaterial.SetFloat("_ShadowOffsetU", this.ShadowOffset.x);
    fontMaterial.SetFloat("_ShadowOffsetV", this.ShadowOffset.y);
  }

  public Material GetPageMaterial(int page, Shader shader)
  {
    if ((Object) this.pageMaterial == (Object) null)
      this.pageMaterial = this.CreateFontMaterial(shader);
    this.UpdateFontMaterial(this.pageMaterial);
    return this.pageMaterial;
  }

  public Material GetCharacterMaterial(int c, Shader shader)
  {
    if (!this.fontMaterials.ContainsKey(c) || (Object) this.fontMaterials[c] == (Object) null)
    {
      Material fontMaterial = this.CreateFontMaterial(shader);
      Rect uvRect = this.GetUVRect(this.GetBitmapChar(c));
      fontMaterial.mainTextureScale = new Vector2(uvRect.width, uvRect.height);
      fontMaterial.mainTextureOffset = new Vector2(uvRect.xMin, uvRect.yMin);
      this.fontMaterials[c] = fontMaterial;
    }
    Material fontMaterial1 = this.fontMaterials[c];
    this.UpdateFontMaterial(fontMaterial1);
    return fontMaterial1;
  }

  public float GetKerning(char first, char second)
  {
    if (this.Kernings != null)
    {
      foreach (BitmapCharKerning kerning in this.Kernings)
      {
        if (kerning.FirstChar == (int) first && kerning.SecondChar == (int) second)
          return kerning.Amount;
      }
    }
    return 0.0f;
  }

  public Vector2 CalculateSize(string str, Vector2 renderSize)
  {
    Vector2 size = new Vector2(0.0f, renderSize.y);
    Vector2 vector2 = renderSize / this.Size;
    for (int index = 0; !string.IsNullOrEmpty(str) && index < str.Length; ++index)
    {
      char ch = str[index];
      BitmapChar bitmapChar = this.GetBitmapChar((int) ch);
      float num = 0.0f;
      if (index < str.Length - 1)
        num = this.GetKerning(ch, str[index + 1]);
      size.x += (bitmapChar.XAdvance + num) * vector2.x;
    }
    return size;
  }

  public Vector2 CalculateSize(string str, float renderSize) => this.CalculateSize(str, new Vector2(renderSize, renderSize));
}
