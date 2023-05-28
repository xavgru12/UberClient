// Decompiled with JetBrains decompiler
// Type: Rotate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Rotate : MonoBehaviour
{
  private Transform _t;

  private void Start() => this._t = this.transform;

  private void Update() => this._t.Rotate(Vector3.up, Time.deltaTime * 2f, Space.Self);

  private void OnDrawGizmos()
  {
    if (!(bool) (Object) this._t)
      this._t = this.transform;
    Gizmos.color = Color.cyan;
    Gizmos.DrawRay(this._t.position, this._t.forward);
  }
}
