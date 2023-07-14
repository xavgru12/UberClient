// Decompiled with JetBrains decompiler
// Type: ProjectileInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ProjectileInfo
{
  public ProjectileInfo(int id, Ray ray)
  {
    this.Id = id;
    this.Position = ray.origin;
    this.Direction = ray.direction;
  }

  public int Id { get; set; }

  public Vector3 Position { get; set; }

  public Vector3 Direction { get; set; }

  public Projectile Projectile { get; set; }
}
