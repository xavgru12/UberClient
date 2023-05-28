// Decompiled with JetBrains decompiler
// Type: GizmoHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GizmoHelper : AutoMonoBehaviour<GizmoHelper>
{
  [SerializeField]
  private List<GizmoHelper.Gizmo> gizmos = new List<GizmoHelper.Gizmo>();
  public GizmoHelper.Gizmo CollisionTest = new GizmoHelper.Gizmo()
  {
    Color = Color.red,
    Type = GizmoHelper.GizmoType.Sphere
  };

  public void AddGizmo(Vector3 position, [Optional] Color color, GizmoHelper.GizmoType type = GizmoHelper.GizmoType.Sphere, float size = 0.1f)
  {
    Debug.Log((object) ("AddGizmo " + (object) position));
    this.gizmos.Add(new GizmoHelper.Gizmo()
    {
      Position = position,
      Type = type,
      Size = size,
      Color = color
    });
  }

  private void OnDrawGizmos()
  {
    if ((double) this.CollisionTest.Size > 0.0)
    {
      Gizmos.color = this.CollisionTest.Color;
      Gizmos.DrawSphere(this.CollisionTest.Position, this.CollisionTest.Size);
    }
    foreach (GizmoHelper.Gizmo gizmo in this.gizmos)
    {
      Gizmos.color = gizmo.Color;
      switch (gizmo.Type)
      {
        case GizmoHelper.GizmoType.Cube:
          Gizmos.DrawCube(gizmo.Position, Vector3.one * gizmo.Size);
          continue;
        case GizmoHelper.GizmoType.Sphere:
          Gizmos.DrawSphere(gizmo.Position, gizmo.Size);
          continue;
        default:
          Gizmos.DrawSphere(gizmo.Position, gizmo.Size);
          continue;
      }
    }
    Gizmos.color = Color.white;
  }

  public enum GizmoType
  {
    Cube,
    Sphere,
    WiredSphere,
  }

  [Serializable]
  public class Gizmo
  {
    public GizmoHelper.GizmoType Type;
    public Vector3 Position;
    public float Size;
    public Color Color;
  }
}
