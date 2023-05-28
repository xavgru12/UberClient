// Decompiled with JetBrains decompiler
// Type: LookToTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LookToTarget : MonoBehaviour
{
  private Transform _follow;
  private Transform transformComponent;

  private void Start() => this.transformComponent = this.transform;

  public Transform follow
  {
    get => this._follow;
    set
    {
      this._follow = value;
      this.enabled = (Object) this._follow != (Object) null;
    }
  }

  private void Update()
  {
    if ((Object) this._follow != (Object) null)
    {
      this.transformComponent.position = Vector3.Lerp(this.transformComponent.position, this._follow.position, Time.deltaTime);
      this.transformComponent.rotation = Quaternion.Slerp(this.transformComponent.rotation, Quaternion.LookRotation(this._follow.position - this.transformComponent.position), 0.8f * Time.deltaTime);
    }
    else
      this.enabled = false;
  }
}
