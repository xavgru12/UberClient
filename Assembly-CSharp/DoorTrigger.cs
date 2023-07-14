// Decompiled with JetBrains decompiler
// Type: DoorTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DoorTrigger : BaseGameProp
{
  private DoorBehaviour _doorLogic;

  private void Awake() => this.gameObject.layer = 21;

  public void SetDoorLogic(DoorBehaviour logic) => this._doorLogic = logic;

  public override void ApplyDamage(DamageInfo shot)
  {
    if ((bool) (Object) this._doorLogic)
      this._doorLogic.Open();
    else
      Debug.LogError((object) ("The DoorCollider " + this.gameObject.name + " is not assigned to a DoorMechanism!"));
  }
}
