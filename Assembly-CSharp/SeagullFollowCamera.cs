// Decompiled with JetBrains decompiler
// Type: SeagullFollowCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SeagullFollowCamera : MonoBehaviour
{
  public Transform Seagull;
  public float _positionDamping;
  public float _rotationDamping;
  private Transform _transformCache;

  private void LateUpdate()
  {
    if (!(bool) (Object) this.Seagull)
      return;
    if (!(bool) (Object) this._transformCache)
      this._transformCache = this.transform;
    this._transformCache.position = Vector3.Lerp(this._transformCache.position, this.Seagull.position, Time.deltaTime * this._positionDamping);
    this._transformCache.rotation = Quaternion.Lerp(this._transformCache.rotation, this.Seagull.rotation, Time.deltaTime * this._rotationDamping);
  }
}
