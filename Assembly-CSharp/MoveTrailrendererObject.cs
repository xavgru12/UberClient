// Decompiled with JetBrains decompiler
// Type: MoveTrailrendererObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MoveTrailrendererObject : MonoBehaviour
{
  [SerializeField]
  private LineRenderer _lineRenderer;
  [SerializeField]
  private float _duration = 0.1f;
  private float _locationOnPath;
  private bool _move;
  private float _timeToArrive = 1f;
  private float _alpha = 1f;

  public void MoveTrail(Vector3 destination, Vector3 muzzlePosition, float distance)
  {
    if (!((Object) this._lineRenderer != (Object) null))
      return;
    this._alpha = 1f;
    this._move = true;
    this._lineRenderer.SetPosition(0, muzzlePosition);
    this._lineRenderer.SetPosition(1, destination);
    this._timeToArrive = Time.time + this._duration;
  }

  private void Update()
  {
    if (!this._move)
      return;
    this._locationOnPath = (float) (1.0 - ((double) this._timeToArrive - (double) Time.time));
    this._alpha = Mathf.Lerp(this._alpha, 0.0f, this._locationOnPath);
    this._lineRenderer.materials[0].SetColor("_TintColor", this._lineRenderer.material.GetColor("_TintColor") with
    {
      a = this._alpha
    });
    if ((double) Time.time < (double) this._timeToArrive)
      return;
    this._move = false;
    Object.Destroy((Object) this.gameObject);
  }
}
