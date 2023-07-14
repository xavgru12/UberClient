// Decompiled with JetBrains decompiler
// Type: ShipBob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ShipBob : MonoBehaviour
{
  [SerializeField]
  private float rotateAmount = 1f;
  [SerializeField]
  private float moveAmount = 0.005f;
  private Transform _transform;
  private Vector3 shipRotation;

  private void Awake()
  {
    this._transform = this.transform;
    this.shipRotation = this._transform.localRotation.eulerAngles;
  }

  private void Update()
  {
    this._transform.position = new Vector3(this._transform.position.x, this._transform.position.y + Mathf.Sin(Time.time) * this.moveAmount, this._transform.position.z);
    float num = Mathf.Sin(Time.time) * this.rotateAmount;
    this._transform.localRotation = Quaternion.Euler(this.shipRotation + new Vector3(num, num, num));
  }
}
