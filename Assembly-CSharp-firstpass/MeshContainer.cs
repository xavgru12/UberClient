// Decompiled with JetBrains decompiler
// Type: MeshContainer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21AF7BBC-70B8-4BE8-9CDE-C2EC2144EAE4
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
