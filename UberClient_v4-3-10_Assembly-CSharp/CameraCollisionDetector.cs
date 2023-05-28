// Decompiled with JetBrains decompiler
// Type: CameraCollisionDetector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CameraCollisionDetector
{
  private bool _lHitResult;
  private bool _rHitResult;
  private RaycastHit _lRaycastInfo;
  private RaycastHit _rRaycastInfo;
  private float _collidedDistance;
  public float Offset;
  public int LayerMask;

  public float Distance => this._collidedDistance;

  public bool Detect(Vector3 from, Vector3 to, Vector3 right)
  {
    this._collidedDistance = Vector3.Distance(from, to);
    if ((Time.frameCount & 1) == 0)
    {
      to -= right * this.Offset;
      this._lHitResult = Physics.Linecast(from, to, out this._lRaycastInfo, this.LayerMask);
    }
    else
    {
      to += right * this.Offset;
      this._rHitResult = Physics.Linecast(from, to, out this._rRaycastInfo, this.LayerMask);
    }
    if (this._lHitResult)
    {
      float num = Vector3.Distance(this._lRaycastInfo.point, from);
      if ((double) num < (double) this._collidedDistance)
        this._collidedDistance = num;
    }
    if (this._rHitResult)
    {
      float num = Vector3.Distance(this._rRaycastInfo.point, from);
      if ((double) num < (double) this._collidedDistance)
        this._collidedDistance = num;
    }
    return this._lHitResult || this._rHitResult;
  }

  public void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    if (this._lHitResult)
      Gizmos.DrawWireSphere(this._lRaycastInfo.point, 0.1f);
    if (!this._rHitResult)
      return;
    Gizmos.DrawWireSphere(this._rRaycastInfo.point, 0.1f);
  }

  public void OnGUI()
  {
    GUI.Label(new Rect(200f, 200f, 200f, 20f), "Left HitResult: " + (object) this._lHitResult);
    if (this._lHitResult)
      GUI.Label(new Rect(200f, 220f, 200f, 20f), "Hit point: " + (object) this._lRaycastInfo.point);
    GUI.Label(new Rect(400f, 200f, 200f, 20f), "Right HitResult: " + (object) this._rHitResult);
    if (!this._rHitResult)
      return;
    GUI.Label(new Rect(400f, 220f, 200f, 20f), "Hit point: " + (object) this._rRaycastInfo.point);
  }
}
