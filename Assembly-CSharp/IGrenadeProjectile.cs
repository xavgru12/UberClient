// Decompiled with JetBrains decompiler
// Type: IGrenadeProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IGrenadeProjectile : IProjectile
{
  event Action<IGrenadeProjectile> OnProjectileEmitted;

  event Action<IGrenadeProjectile> OnProjectileExploded;

  Vector3 Position { get; }

  Vector3 Velocity { get; }

  IGrenadeProjectile Throw(Vector3 position, Vector3 velocity);

  void SetLayer(UberstrikeLayer layer);
}
