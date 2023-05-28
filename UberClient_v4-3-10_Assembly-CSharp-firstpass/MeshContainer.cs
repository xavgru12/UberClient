// Decompiled with JetBrains decompiler
// Type: MeshContainer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class MeshContainer
{
  public Mesh mesh;
  public Vector3[] vertices;
  public Vector3[] normals;

  public MeshContainer(Mesh m)
  {
    this.mesh = m;
    this.vertices = m.vertices;
    this.normals = m.normals;
  }

  public void Update()
  {
    this.mesh.vertices = this.vertices;
    this.mesh.normals = this.normals;
  }
}
