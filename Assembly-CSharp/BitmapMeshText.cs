// Decompiled with JetBrains decompiler
// Type: BitmapMeshText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BitmapMeshText : CustomMesh
{
  private string _text;
  private Vector3 _offset;
  private Color _mainColor;
  private Color _shadowColor;
  private float _alphaMin;
  private float _alphaMax;
  private float _shadowAlphaMin;
  private float _shadowAlphaMax;
  private float _shadowOffsetU;
  private float _shadowOffsetV;
  private Vector3[] quadVerts = new Vector3[4]
  {
    new Vector3(0.0f, 0.0f),
    new Vector3(0.0f, 1f),
    new Vector3(1f, 1f),
    new Vector3(1f, 0.0f)
  };
  private Vector2[] quadUvs = new Vector2[4]
  {
    new Vector2(0.0f, 0.0f),
    new Vector2(0.0f, 1f),
    new Vector2(1f, 1f),
    new Vector2(1f, 0.0f)
  };
  private int[] quadTriangles = new int[6]
  {
    0,
    1,
    2,
    2,
    3,
    0
  };

  public BitmapFont Font { get; set; }

  public string Text
  {
    get => this._text;
    set
    {
      this._text = value;
      this.Reset();
    }
  }

  public TextAnchor Anchor { get; set; }

  public override Color Color
  {
    get => this.MainColor;
    set => this.MainColor = value;
  }

  public override float Alpha
  {
    get => this.MainColor.a;
    set
    {
      this.MainColor = this.MainColor with { a = value };
      this.ShadowColor = this.ShadowColor with { a = value };
    }
  }

  public Vector2 OffsetToUpperLeft { get; private set; }

  public Color MainColor
  {
    get => this._mainColor;
    set
    {
      this._mainColor = value;
      if (!((Object) this._meshRenderer != (Object) null))
        return;
      foreach (Material material in this._meshRenderer.materials)
        material.SetColor("_Color", value);
    }
  }

  public Color ShadowColor
  {
    get => this._shadowColor;
    set
    {
      this._shadowColor = value;
      if (!((Object) this._meshRenderer != (Object) null))
        return;
      foreach (Material material in this._meshRenderer.materials)
        material.SetColor("_ShadowColor", value);
    }
  }

  public float AlphaMin
  {
    get => this._alphaMin;
    set
    {
      this._alphaMin = value;
      if (!((Object) this._meshRenderer != (Object) null))
        return;
      foreach (Material material in this._meshRenderer.materials)
        material.SetFloat("_AlphaMin", value);
    }
  }

  public float AlphaMax
  {
    get => this._alphaMax;
    set
    {
      this._alphaMax = value;
      if (!((Object) this._meshRenderer != (Object) null))
        return;
      foreach (Material material in this._meshRenderer.materials)
        material.SetFloat("_AlphaMax", value);
    }
  }

  public float ShadowAlphaMin
  {
    get => this._shadowAlphaMin;
    set
    {
      this._shadowAlphaMin = value;
      if (!(bool) (Object) this._meshRenderer)
        return;
      foreach (Material material in this._meshRenderer.materials)
        material.SetFloat("_ShadowAlphaMin", value);
    }
  }

  public float ShadowAlphaMax
  {
    get => this._shadowAlphaMax;
    set
    {
      this._shadowAlphaMax = value;
      if (!(bool) (Object) this._meshRenderer)
        return;
      foreach (Material material in this._meshRenderer.materials)
        material.SetFloat("_ShadowAlphaMax", value);
    }
  }

  public float ShadowOffsetU
  {
    get => this._shadowOffsetU;
    set
    {
      this._shadowOffsetU = value;
      if (!(bool) (Object) this._meshRenderer)
        return;
      foreach (Material material in this._meshRenderer.materials)
        material.SetFloat("_ShadowOffsetU", value);
    }
  }

  public float ShadowOffsetV
  {
    get => this._shadowOffsetV;
    set
    {
      this._shadowOffsetV = value;
      if (!(bool) (Object) this._meshRenderer)
        return;
      foreach (Material material in this._meshRenderer.materials)
        material.SetFloat("_ShadowOffsetV", value);
    }
  }

  protected override void Reset()
  {
    if ((Object) this.Font == (Object) null)
      return;
    if ((Object) this._meshRenderer == (Object) null)
      this._meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
    if ((Object) this._meshFilter == (Object) null)
      this._meshFilter = this.gameObject.AddComponent<MeshFilter>();
    Vector3 vector3 = new Vector3(1f, 1f, 1f);
    this._bounds = this.Font.CalculateSize(this.Text, new Vector2(vector3.x, vector3.y));
    this._offset = new Vector3(0.0f, 0.0f);
    if (this.Anchor == TextAnchor.UpperCenter || this.Anchor == TextAnchor.UpperLeft || this.Anchor == TextAnchor.UpperRight)
      this._offset.y = this._bounds.y;
    if (this.Anchor == TextAnchor.MiddleCenter || this.Anchor == TextAnchor.MiddleLeft || this.Anchor == TextAnchor.MiddleRight)
      this._offset.y = this._bounds.y / 2f;
    if (this.Anchor == TextAnchor.UpperRight || this.Anchor == TextAnchor.MiddleRight || this.Anchor == TextAnchor.LowerRight)
      this._offset.x = this._bounds.x;
    if (this.Anchor == TextAnchor.UpperCenter || this.Anchor == TextAnchor.MiddleCenter || this.Anchor == TextAnchor.LowerCenter)
      this._offset.x = this._bounds.x / 2f;
    this.OffsetToUpperLeft = new Vector2(this._offset.x, this._bounds.y - this._offset.y);
    Mesh mesh = this.GenerateMesh();
    if ((Object) this._meshFilter.sharedMesh != (Object) null)
    {
      if (Application.isPlaying)
        Object.Destroy((Object) this._meshFilter.sharedMesh);
      else
        Object.DestroyImmediate((Object) this._meshFilter.sharedMesh);
    }
    this._meshFilter.mesh = mesh;
    Material[] materialArray = new Material[mesh.subMeshCount];
    for (int page = 0; page < mesh.subMeshCount; ++page)
      materialArray[page] = this.Font.GetPageMaterial(page, this._shader);
    foreach (Object material in this._meshRenderer.materials)
      Object.Destroy(material);
    this._meshRenderer.materials = materialArray;
    this.ResetMaterial();
  }

  protected override Mesh GenerateMesh()
  {
    Vector3 vector3_1 = new Vector3(1f, 1f, 1f);
    string text = this.Text;
    Vector3 vector3_2 = new Vector3(0.0f, 0.0f, 0.0f) - this._offset;
    List<int> intList = new List<int>();
    List<Vector3> vector3List = new List<Vector3>();
    List<Vector2> vector2List = new List<Vector2>();
    Vector3 vector3_3 = vector3_2;
    Vector3 b1 = vector3_1 / this.Font.Size;
    for (int index1 = 0; !string.IsNullOrEmpty(text) && index1 < text.Length; ++index1)
    {
      char ch = text[index1];
      BitmapChar bitmapChar = this.Font.GetBitmapChar((int) ch);
      int count = vector3List.Count;
      Rect uvRect = this.Font.GetUVRect(bitmapChar);
      Vector2 b2 = new Vector2(uvRect.width, uvRect.height);
      Vector2 vector2 = new Vector2(uvRect.x, uvRect.y);
      for (int index2 = 0; index2 < this.quadUvs.Length; ++index2)
        vector2List.Add(Vector2.Scale(this.quadUvs[index2], b2) + vector2);
      Vector3 b3 = (Vector3) Vector2.Scale(bitmapChar.Size, (Vector2) b1);
      Vector3 vector3_4 = (Vector3) Vector2.Scale(bitmapChar.Offset, (Vector2) b1);
      vector3_4.y = vector3_1.y - (vector3_4.y + b3.y);
      for (int index3 = 0; index3 < this.quadVerts.Length; ++index3)
      {
        Vector3 vector3_5 = Vector3.Scale(this.quadVerts[index3], b3) + vector3_3 + vector3_4;
        vector3List.Add(vector3_5);
      }
      for (int index4 = 0; index4 < this.quadTriangles.Length; ++index4)
        intList.Add(this.quadTriangles[index4] + count);
      float num = 0.0f;
      if (index1 < this.Text.Length - 1)
        num = this.Font.GetKerning(ch, this.Text[index1 + 1]);
      vector3_3.x += (bitmapChar.XAdvance + num) * b1.x;
    }
    Mesh mesh = new Mesh();
    mesh.vertices = vector3List.ToArray();
    mesh.uv = vector2List.ToArray();
    mesh.subMeshCount = 1;
    mesh.SetTriangles(intList.ToArray(), 0);
    return mesh;
  }

  private void ResetMaterial()
  {
    if (!((Object) this._meshRenderer != (Object) null))
      return;
    foreach (Material material in this._meshRenderer.materials)
    {
      material.SetColor("_Color", this._mainColor);
      material.SetColor("_ShadowColor", this._shadowColor);
      material.SetFloat("_AlphaMin", this._alphaMin);
      material.SetFloat("_AlphaMax", this._alphaMax);
      material.SetFloat("_ShadowAlphaMin", this._shadowAlphaMin);
      material.SetFloat("_ShadowAlphaMax", this._shadowAlphaMax);
      material.SetFloat("_ShadowOffsetU", this._shadowOffsetU);
      material.SetFloat("_ShadowOffsetV", this._shadowOffsetV);
    }
  }
}
