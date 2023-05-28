// Decompiled with JetBrains decompiler
// Type: Quads
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[Serializable]
public class Quads : MonoBehaviour
{
  [NonSerialized]
  public static Mesh[] meshes;
  [NonSerialized]
  public static int currentQuads;

  public static bool HasMeshes()
  {
    if (Quads.meshes == null)
      return false;
    int index = 0;
    Mesh[] meshes = Quads.meshes;
    for (int length = meshes.Length; index < length; ++index)
    {
      if ((UnityEngine.Object) null == (UnityEngine.Object) meshes[index])
        return false;
    }
    return true;
  }

  public static void Cleanup()
  {
    if (Quads.meshes == null)
      return;
    int index = 0;
    Mesh[] meshes = Quads.meshes;
    for (int length = meshes.Length; index < length; ++index)
    {
      if ((UnityEngine.Object) null != (UnityEngine.Object) meshes[index])
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) meshes[index]);
        meshes[index] = (Mesh) null;
      }
    }
    Quads.meshes = (Mesh[]) null;
  }

  public static Mesh[] GetMeshes(int totalWidth, int totalHeight)
  {
    if (Quads.HasMeshes() && Quads.currentQuads == totalWidth * totalHeight)
      return Quads.meshes;
    int max = 10833;
    int num = totalWidth * totalHeight;
    Quads.currentQuads = num;
    Quads.meshes = new Mesh[Mathf.CeilToInt((float) (1.0 * (double) num / (1.0 * (double) max)))];
    int index = 0;
    for (int triOffset = 0; triOffset < num; triOffset += max)
    {
      int triCount = Mathf.FloorToInt((float) Mathf.Clamp(num - triOffset, 0, max));
      Quads.meshes[index] = Quads.GetMesh(triCount, triOffset, totalWidth, totalHeight);
      ++index;
    }
    return Quads.meshes;
  }

  public static Mesh GetMesh(int triCount, int triOffset, int totalWidth, int totalHeight)
  {
    Mesh mesh = new Mesh();
    mesh.hideFlags = HideFlags.DontSave;
    Vector3[] vector3Array = new Vector3[triCount * 4];
    Vector2[] vector2Array1 = new Vector2[triCount * 4];
    Vector2[] vector2Array2 = new Vector2[triCount * 4];
    int[] numArray = new int[triCount * 6];
    for (int index = 0; index < triCount; ++index)
    {
      int num1 = index * 4;
      int num2 = index * 6;
      int num3 = triOffset + index;
      float x = Mathf.Floor((float) (num3 % totalWidth)) / (float) totalWidth;
      float y = Mathf.Floor((float) (num3 / totalWidth)) / (float) totalHeight;
      Vector3 vector3 = new Vector3(x * 2f - 1f, y * 2f - 1f, 1f);
      vector3Array[num1 + 0] = vector3;
      vector3Array[num1 + 1] = vector3;
      vector3Array[num1 + 2] = vector3;
      vector3Array[num1 + 3] = vector3;
      vector2Array1[num1 + 0] = new Vector2(0.0f, 0.0f);
      vector2Array1[num1 + 1] = new Vector2(1f, 0.0f);
      vector2Array1[num1 + 2] = new Vector2(0.0f, 1f);
      vector2Array1[num1 + 3] = new Vector2(1f, 1f);
      vector2Array2[num1 + 0] = new Vector2(x, y);
      vector2Array2[num1 + 1] = new Vector2(x, y);
      vector2Array2[num1 + 2] = new Vector2(x, y);
      vector2Array2[num1 + 3] = new Vector2(x, y);
      numArray[num2 + 0] = num1 + 0;
      numArray[num2 + 1] = num1 + 1;
      numArray[num2 + 2] = num1 + 2;
      numArray[num2 + 3] = num1 + 1;
      numArray[num2 + 4] = num1 + 2;
      numArray[num2 + 5] = num1 + 3;
    }
    mesh.vertices = vector3Array;
    mesh.triangles = numArray;
    mesh.uv = vector2Array1;
    mesh.uv2 = vector2Array2;
    return mesh;
  }

  public virtual void Main()
  {
  }
}
