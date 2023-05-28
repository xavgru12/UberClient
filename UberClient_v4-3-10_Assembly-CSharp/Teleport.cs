// Decompiled with JetBrains decompiler
// Type: Teleport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class Teleport : MonoBehaviour
{
  [SerializeField]
  private Transform _spawnPoint;
  [SerializeField]
  private AudioClip _sound;
  private AudioSource _source;

  private void Awake() => this._source = this.GetComponent<AudioSource>();

  private void OnTriggerEnter(Collider c)
  {
    if (c.tag == "Player")
    {
      if ((bool) (Object) this._source)
        this._source.PlayOneShot(this._sound);
      GameState.LocalPlayer.SpawnPlayerAt(this._spawnPoint.position, this._spawnPoint.rotation);
    }
    else
    {
      if (!(c.tag == "Prop"))
        return;
      c.transform.position = this._spawnPoint.position;
    }
  }
}
