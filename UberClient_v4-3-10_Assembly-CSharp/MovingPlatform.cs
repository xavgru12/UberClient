// Decompiled with JetBrains decompiler
// Type: MovingPlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
  private Vector3 _lastPosition;
  private Vector3 _lastMovement;

  private void OnTriggerEnter(Collider c)
  {
    if (!(c.tag == "Player"))
      return;
    this._lastPosition = this.transform.position;
    GameState.LocalPlayer.MoveController.Platform = this;
  }

  private void OnTriggerExit(Collider c)
  {
    if (!(c.tag == "Player"))
      return;
    GameState.LocalPlayer.MoveController.Platform = (MovingPlatform) null;
  }

  public Vector3 LastMovement => this._lastMovement;

  public Vector3 GetMovementDelta()
  {
    this._lastMovement = this.transform.position - this._lastPosition;
    this._lastPosition = this.transform.position;
    return this._lastMovement;
  }
}
