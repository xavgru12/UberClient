// Decompiled with JetBrains decompiler
// Type: QuadMesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class QuadMesh : CustomMesh
{
  private Vector3[] quadVerts = new Vector3[4]
  {
    new Vector3(0.0f, -1f),
    new Vector3(0.0f, 0.0f),
    new Vector3(1f, 0.0f),
    new Vector3(1f, -1f)
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
  private TextAnchor _anchor;
  private Vector2 _offset;

  public TextAnchor Anchor
  {
    get => this._anchor;
    set
    {
      this._anchor = value;
      this.Reset();
    }
  }

  public Vector2 OffsetToUpperLeft { get; private set; }

  protected override Mesh GenerateMesh()
  {
    this._bounds = Vector2.one;
    this.CalculateOffset();
    Mesh mesh = new Mesh();
    List<Vector3> vector3List = new List<Vector3>();
    for (int index = 0; index < this.quadVerts.Length; ++index)
      vector3List.Add(new Vector3()
      {
        x = this.quadVerts[index].x * this._bounds.x - this._offset.x,
        y = (float) ((double) this.quadVerts[index].y * (double) this._bounds.y + (1.0 - (double) this._offset.y))
      });
    mesh.vertices = vector3List.ToArray();
    mesh.uv = this.quadUvs;
    mesh.subMeshCount = 1;
    mesh.SetTriangles(this.quadTriangles, 0);
    return mesh;
  }

  private void Awake() => this.Reset();

  private void CalculateOffset()
  {
    this._offset = Vector2.zero;
    if (this.Anchor == TextAnchor.UpperCenter || this.Anchor == TextAnchor.UpperLeft || this.Anchor == TextAnchor.UpperRight)
      this._offset.y = this._bounds.y;
    if (this.Anchor == TextAnchor.MiddleCenter || this.Anchor == TextAnchor.MiddleLeft || this.Anchor == TextAnchor.MiddleRight)
      this._offset.y = this._bounds.y / 2f;
    if (this.Anchor == TextAnchor.UpperRight || this.Anchor == TextAnchor.MiddleRight || this.Anchor == TextAnchor.LowerRight)
      this._offset.x = this._bounds.x;
    if (this.Anchor == TextAnchor.UpperCenter || this.Anchor == TextAnchor.MiddleCenter || this.Anchor == TextAnchor.LowerCenter)
      this._offset.x = this._bounds.x / 2f;
    this.OffsetToUpperLeft = new Vector2(this._offset.x, this._bounds.y - this._offset.y);
  }
}
