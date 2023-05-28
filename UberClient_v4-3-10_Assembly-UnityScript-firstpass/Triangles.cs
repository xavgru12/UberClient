// Decompiled with JetBrains decompiler
// Type: Triangles
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2B73E9D7-7440-409D-A393-CC5E4EE1D985
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[Serializable]
public class Triangles : MonoBehaviour
{
  [NonSerialized]
  public static Mesh[] meshes;
  [NonSerialized]
  public static int currentTris;

  public static bool HasMeshes()
  {
    if (Triangles.meshes == null)
      return false;
    int index = 0;
    Mesh[] meshes = Triangles.meshes;
    for (int length = meshes.Length; index < length; ++index)
    {
      if ((UnityEngine.Object) null == (UnityEngine.Object) meshes[index])
        return false;
    }
    return true;
  }

  public static void Cleanup()
  {
    if (Triangles.meshes == null)
      return;
    int index = 0;
    Mesh[] meshes = Triangles.meshes;
    for (int length = meshes.Length; index < length; ++index)
    {
      if ((UnityEngine.Object) null != (UnityEngine.Object) meshes[index])
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) meshes[index]);
        meshes[index] = (Mesh) null;
      }
    }
    Triangles.meshes = (Mesh[]) null;
  }

  public static Mesh[] GetMeshes(int totalWidth, int totalHeight)
  {
    if (Triangles.HasMeshes() && Triangles.currentTris == totalWidth * totalHeight)
      return Triangles.meshes;
    int max = 21666;
    int num = totalWidth * totalHeight;
    Triangles.currentTris = num;
    Triangles.meshes = new Mesh[Mathf.CeilToInt((float) (1.0 * (double) num / (1.0 * (double) max)))];
    int index = 0;
    for (int triOffset = 0; triOffset < num; triOffset += max)
    {
      int triCount = Mathf.FloorToInt((float) Mathf.Clamp(num - triOffset, 0, max));
      Triangles.meshes[index] = Triangles.GetMesh(triCount, triOffset, totalWidth, totalHeight);
      ++index;
    }
    return Triangles.meshes;
  }

  public static Mesh GetMesh(int triCount, int triOffset, int totalWidth, int totalHeight)
  {
    Mesh mesh = new Mesh();
    mesh.hideFlags = HideFlags.DontSave;
    Vector3[] vector3Array = new Vector3[triCount * 3];
    Vector2[] vector2Array1 = new Vector2[triCount * 3];
    Vector2[] vector2Array2 = new Vector2[triCount * 3];
    int[] numArray = new int[triCount * 3];
    for (int index = 0; index < triCount; ++index)
    {
      int num1 = index * 3;
      int num2 = triOffset + index;
      float x = Mathf.Floor((float) (num2 % totalWidth)) / (float) totalWidth;
      float y = Mathf.Floor((float) (num2 / totalWidth)) / (float) totalHeight;
      Vector3 vector3 = new Vector3(x * 2f - 1f, y * 2f - 1f, 1f);
      vector3Array[num1 + 0] = vector3;
      vector3Array[num1 + 1] = vector3;
      vector3Array[num1 + 2] = vector3;
      vector2Array1[num1 + 0] = new Vector2(0.0f, 0.0f);
      vector2Array1[num1 + 1] = new Vector2(1f, 0.0f);
      vector2Array1[num1 + 2] = new Vector2(0.0f, 1f);
      vector2Array2[num1 + 0] = new Vector2(x, y);
      vector2Array2[num1 + 1] = new Vector2(x, y);
      vector2Array2[num1 + 2] = new Vector2(x, y);
      numArray[num1 + 0] = num1 + 0;
      numArray[num1 + 1] = num1 + 1;
      numArray[num1 + 2] = num1 + 2;
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
