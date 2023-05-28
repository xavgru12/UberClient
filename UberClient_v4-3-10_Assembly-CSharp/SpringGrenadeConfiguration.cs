// Decompiled with JetBrains decompiler
// Type: SpringGrenadeConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class SpringGrenadeConfiguration : QuickItemConfiguration
{
  [SerializeField]
  private Vector3 _jumpDirection = Vector3.up;
  [CustomProperty("Force")]
  [SerializeField]
  private int _force = 1250;
  [SerializeField]
  [CustomProperty("LifeTime")]
  private int _lifeTime = 15;
  [CustomProperty("Sticky")]
  [SerializeField]
  private bool _isSticky = true;
  [SerializeField]
  private int _speed = 10;

  public Vector3 JumpDirection => this._jumpDirection;

  public int Force => this._force;

  public int LifeTime => this._lifeTime;

  public bool IsSticky => this._isSticky;

  public int Speed => this._speed;
}
