// Decompiled with JetBrains decompiler
// Type: ExplosiveGrenadeConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class ExplosiveGrenadeConfiguration : QuickItemConfiguration
{
  [CustomProperty("Damage")]
  [SerializeField]
  private int _damage = 100;
  [CustomProperty("SplashRadius")]
  [SerializeField]
  private int _splash = 2;
  [SerializeField]
  [CustomProperty("LifeTime")]
  private int _lifeTime = 15;
  [SerializeField]
  [CustomProperty("Bounciness")]
  private int _bounciness = 3;
  [SerializeField]
  [CustomProperty("Sticky")]
  private bool _isSticky = true;
  [SerializeField]
  private int _speed = 15;

  public int Damage => this._damage;

  public int SplashRadius => this._splash;

  public int LifeTime => this._lifeTime;

  public float Bounciness => (float) this._bounciness * 0.1f;

  public bool IsSticky => this._isSticky;

  public int Speed => this._speed;
}
