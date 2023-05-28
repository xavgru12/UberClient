// Decompiled with JetBrains decompiler
// Type: RobotPiecesLogic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

internal class RobotPiecesLogic : MonoBehaviour
{
  [SerializeField]
  private AudioClip[] _robotExplosionAudios;
  [SerializeField]
  private AudioClip[] _robotScrapsDestructionAudios;
  [SerializeField]
  private GameObject _robotPieces;

  public void ExplodeRobot(GameObject robotObject, int lifeTimeMilliSeconds)
  {
    if ((Object) this._robotPieces != (Object) null)
    {
      foreach (Rigidbody componentsInChild in this._robotPieces.GetComponentsInChildren<Rigidbody>())
        componentsInChild.AddExplosionForce(5f, this.transform.position, 2f, 0.0f, ForceMode.Impulse);
    }
    if (this._robotScrapsDestructionAudios != null && this._robotScrapsDestructionAudios.Length > 0)
    {
      AudioClip destructionAudio = this._robotScrapsDestructionAudios[Random.Range(0, this._robotScrapsDestructionAudios.Length)];
      if ((bool) (Object) destructionAudio)
      {
        this.audio.clip = destructionAudio;
        this.audio.Play();
      }
    }
    MonoRoutine.Start(this.DestroyRobotPieces(robotObject, lifeTimeMilliSeconds));
  }

  public void PlayRobotScrapsDestructionAudio()
  {
    if (this._robotScrapsDestructionAudios == null || this._robotScrapsDestructionAudios.Length <= 0)
      return;
    AudioClip destructionAudio = this._robotScrapsDestructionAudios[Random.Range(0, this._robotScrapsDestructionAudios.Length)];
    if (!(bool) (Object) destructionAudio)
      return;
    this.audio.clip = destructionAudio;
    this.audio.Play();
  }

  [DebuggerHidden]
  private IEnumerator DestroyRobotPieces(GameObject robotObject, int lifeTimeMilliSeconds) => (IEnumerator) new RobotPiecesLogic.\u003CDestroyRobotPieces\u003Ec__Iterator57()
  {
    lifeTimeMilliSeconds = lifeTimeMilliSeconds,
    robotObject = robotObject,
    \u003C\u0024\u003ElifeTimeMilliSeconds = lifeTimeMilliSeconds,
    \u003C\u0024\u003ErobotObject = robotObject,
    \u003C\u003Ef__this = this
  };
}
