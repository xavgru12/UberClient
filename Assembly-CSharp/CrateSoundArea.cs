// Decompiled with JetBrains decompiler
// Type: CrateSoundArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (SoundArea))]
public class CrateSoundArea : MonoBehaviour
{
  [SerializeField]
  private BoxCollider _boxCollider;
  [SerializeField]
  private float _offset;
  private Transform _transform;

  private void Awake()
  {
    this._transform = this.transform;
    if ((bool) (Object) this._boxCollider)
      this._boxCollider.center = this.CalcTriggerCenter(this._transform, this._offset);
    else
      Debug.LogError((object) "There is no box collider attached to the crate!");
  }

  private void Update()
  {
    if (!(bool) (Object) this._transform || !(bool) (Object) this._boxCollider)
      return;
    this._boxCollider.center = this.CalcTriggerCenter(this._transform, this._offset);
  }

  private Vector3 CalcTriggerCenter(Transform t, float offset)
  {
    Vector3 vector3 = Vector3.zero;
    if ((bool) (Object) t)
      vector3 = t.InverseTransformDirection(Vector3.up * offset);
    return vector3;
  }
}
