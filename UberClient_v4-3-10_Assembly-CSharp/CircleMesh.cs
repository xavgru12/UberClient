// Decompiled with JetBrains decompiler
// Type: CircleMesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CircleMesh : CustomMesh
{
  private static Vector3 Normal = new Vector3(0.0f, 0.0f, -1f);
  private static Vector3 TexShift = new Vector3(0.5f, 0.5f, 0.5f);
  private Vector3 _center;
  private float _radius;
  private float _startAngle;
  private float _fillAngle;

  public float Radius => this._radius;

  public float StartAngle
  {
    get => this._startAngle;
    set
    {
      this._startAngle = value;
      this.Reset();
    }
  }

  public float FillAngle
  {
    get => this._fillAngle;
    set
    {
      this._fillAngle = value;
      this.Reset();
    }
  }

  protected override Mesh GenerateMesh()
  {
    Vector3 vector3 = Quaternion.Euler(0.0f, 0.0f, -this._startAngle) * Vector3.up;
    int num1 = (int) Mathf.Clamp(Mathf.Abs(this._fillAngle) * 0.1f, 5f, 30f);
    Quaternion quaternion = Quaternion.AngleAxis(this._fillAngle * (1f / (float) (num1 - 1)), CircleMesh.Normal);
    Vector3 a1 = vector3 * this._radius;
    float num2 = (float) (1.0 / (2.0 * (double) this._radius));
    Vector3 b = new Vector3(num2, num2, num2);
    List<int> intList = new List<int>();
    List<Vector3> vector3List1 = new List<Vector3>();
    List<Vector3> vector3List2 = new List<Vector3>();
    List<Vector2> vector2List = new List<Vector2>();
    for (int index = 0; index < num1 - 1; ++index)
    {
      Vector3 a2 = a1;
      a1 = quaternion * a1;
      vector2List.Add((Vector2) CircleMesh.TexShift);
      vector3List1.Add(this._center);
      vector3List2.Add(CircleMesh.Normal);
      intList.Add(index * 3);
      vector2List.Add((Vector2) (CircleMesh.TexShift + Vector3.Scale(a2, b)));
      vector3List1.Add(this._center + a2);
      vector3List2.Add(CircleMesh.Normal);
      intList.Add(index * 3 + 1);
      vector2List.Add((Vector2) (CircleMesh.TexShift + Vector3.Scale(a1, b)));
      vector3List1.Add(this._center + a1);
      vector3List2.Add(CircleMesh.Normal);
      intList.Add(index * 3 + 2);
    }
    Mesh mesh = new Mesh();
    mesh.vertices = vector3List1.ToArray();
    mesh.normals = vector3List2.ToArray();
    mesh.uv = vector2List.ToArray();
    mesh.subMeshCount = 1;
    mesh.SetTriangles(intList.ToArray(), 0);
    return mesh;
  }

  private void Awake()
  {
    this._radius = 0.5f;
    this._center = Vector3.zero;
    this._startAngle = 0.0f;
    this._fillAngle = 360f;
    this.Reset();
  }
}
