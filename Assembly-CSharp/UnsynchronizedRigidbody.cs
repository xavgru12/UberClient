// Decompiled with JetBrains decompiler
// Type: UnsynchronizedRigidbody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class UnsynchronizedRigidbody : BaseGameProp
{
  public override void ApplyDamage(DamageInfo d) => this.Rigidbody.AddForceAtPosition(d.Force, d.Hitpoint);
}
