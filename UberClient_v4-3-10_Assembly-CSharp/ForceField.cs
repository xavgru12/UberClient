// Decompiled with JetBrains decompiler
// Type: ForceField
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class ForceField : MonoBehaviour
{
  [SerializeField]
  private Vector3 _direction;
  [SerializeField]
  private int _force = 1000;
  private float gizmofactor = 0.0055f;

  private void Awake()
  {
    this.collider.isTrigger = true;
    this.gameObject.layer = 2;
  }

  private void OnTriggerEnter(Collider collider)
  {
    if (collider.tag == "Player")
    {
      GameState.LocalPlayer.MoveController.ApplyForce(this._direction.normalized * (float) this._force, CharacterMoveController.ForceType.Exclusive);
      SfxManager.Play2dAudioClip(GameAudio.JumpPad2D);
    }
    else
    {
      if (collider.gameObject.layer != 20)
        return;
      SfxManager.Play3dAudioClip(GameAudio.JumpPad, 1f, 0.1f, 10f, AudioRolloffMode.Linear, this.transform.position);
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.DrawSphere(this.transform.localPosition, 0.2f);
    Vector3 normalized = this._direction.normalized;
    normalized.y *= 0.6f;
    Gizmos.DrawLine(this.transform.localPosition, this.transform.localPosition + normalized * Mathf.Log((float) this._force) * (float) this._force * this.gizmofactor);
  }
}
