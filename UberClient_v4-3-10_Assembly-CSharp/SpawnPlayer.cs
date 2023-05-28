// Decompiled with JetBrains decompiler
// Type: SpawnPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Collider))]
public class SpawnPlayer : MonoBehaviour
{
  [SerializeField]
  private float _amplitude = 20f;
  [SerializeField]
  private float _velocity = 0.1f;
  [SerializeField]
  private Vector3 _startPosition;
  private Vector3 _currentPosition;
  [SerializeField]
  private Transform _spawnTo;
  private Transform _transform;
  private float _nextTeleport;

  private void Awake()
  {
    this._transform = this.transform;
    this._startPosition = this._currentPosition = this._transform.localPosition;
  }

  private void Update()
  {
    this._currentPosition.y = this._startPosition.y + Mathf.Sin(Time.time * this._velocity) * this._amplitude;
    this._transform.localPosition = this._currentPosition;
  }

  private void OnTriggerEnter(Collider c)
  {
    if (!(c.tag == "Player") || (double) this._nextTeleport >= (double) Time.time)
      return;
    this._nextTeleport = Time.time + 5f;
    GameState.LocalPlayer.SpawnPlayerAt(this._spawnTo.position, this._spawnTo.rotation);
  }
}
