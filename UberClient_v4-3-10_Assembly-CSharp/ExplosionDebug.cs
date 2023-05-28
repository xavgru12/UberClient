// Decompiled with JetBrains decompiler
// Type: ExplosionDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ExplosionDebug : MonoBehaviour
{
  public static Vector3 ImpactPoint;
  public static Vector3 TestPoint;
  public static float Radius;
  public static List<Vector3> Hits = new List<Vector3>();
  public static List<ExplosionDebug.Line> Protections = new List<ExplosionDebug.Line>();

  public static ExplosionDebug Instance { get; private set; }

  public static bool Exists => (Object) ExplosionDebug.Instance != (Object) null;

  private void Awake() => ExplosionDebug.Instance = this;

  public static void Reset()
  {
    ExplosionDebug.Hits.Clear();
    ExplosionDebug.Protections.Clear();
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(ExplosionDebug.ImpactPoint, ExplosionDebug.Radius);
    Gizmos.color = Color.blue;
    Gizmos.DrawSphere(ExplosionDebug.TestPoint, 0.1f);
    for (int index = 0; index < ExplosionDebug.Hits.Count; ++index)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(ExplosionDebug.Hits[index], 0.1f);
    }
    for (int index = 0; index < ExplosionDebug.Protections.Count; ++index)
    {
      Gizmos.color = Color.green;
      Gizmos.DrawLine(ExplosionDebug.Protections[index].Start, ExplosionDebug.Protections[index].End);
    }
  }

  public struct Line
  {
    public Vector3 Start;
    public Vector3 End;

    public Line(Vector3 start, Vector3 end)
    {
      this.Start = start;
      this.End = end;
    }
  }
}
