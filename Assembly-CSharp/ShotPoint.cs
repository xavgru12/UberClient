// Decompiled with JetBrains decompiler
// Type: ShotPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ShotPoint
{
  private Vector3 _aggregatedPoint;

  public ShotPoint(Vector3 point, int projectileId)
  {
    this.AddPoint(point);
    this.ProjectileId = projectileId;
  }

  public int ProjectileId { get; private set; }

  public int Count { get; private set; }

  public void AddPoint(Vector3 point)
  {
    this._aggregatedPoint += point;
    ++this.Count;
  }

  public Vector3 MidPoint => this._aggregatedPoint / (float) this.Count;
}
