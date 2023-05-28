// Decompiled with JetBrains decompiler
// Type: IWeaponController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public interface IWeaponController
{
  int NextProjectileId();

  byte PlayerNumber { get; }

  bool IsLocal { get; }

  Vector3 ShootingPoint { get; }

  Vector3 ShootingDirection { get; }
}
