// Decompiled with JetBrains decompiler
// Type: QuickItemSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class QuickItemSfx : MonoBehaviour
{
  [SerializeField]
  private GameObject _robotPiecesPrefab;
  [SerializeField]
  private AudioClip _shortLoopAudio;
  [SerializeField]
  private AudioClip _normalLoopAudio;
  [SerializeField]
  private Transform _robotTransform;
  private bool _isShortAudio;

  public int ID { get; set; }

  public bool IsShortAudio
  {
    get => this._isShortAudio;
    set
    {
      this._isShortAudio = value;
      AudioSource componentInChildren = this.GetComponentInChildren<AudioSource>();
      if (!((Object) componentInChildren != (Object) null))
        return;
      componentInChildren.clip = !this._isShortAudio ? this._normalLoopAudio : this._shortLoopAudio;
    }
  }

  public Transform Parent { get; set; }

  public Vector3 Offset { get; set; }

  public void Play(int robotLifeTime, int scrapsLifeTime, bool isInstant)
  {
    this.IsShortAudio = isInstant;
    AudioSource componentInChildren = this.GetComponentInChildren<AudioSource>();
    if ((Object) componentInChildren != (Object) null)
      componentInChildren.Play();
    this.StartCoroutine(this.StopEffectAfterSeconds(robotLifeTime, scrapsLifeTime));
  }

  public void Explode(int scrapsLifeTime)
  {
    GameObject robotObject = Object.Instantiate((Object) this._robotPiecesPrefab, this._robotTransform.position, Quaternion.identity) as GameObject;
    if ((Object) robotObject != (Object) null)
      robotObject.GetComponentInChildren<RobotPiecesLogic>().ExplodeRobot(robotObject, scrapsLifeTime);
    this.Destroy();
  }

  public void Destroy() => Object.Destroy((Object) this.gameObject);

  [DebuggerHidden]
  private IEnumerator StopEffectAfterSeconds(int robotLifeTime, int scrapsLifeTime) => (IEnumerator) new QuickItemSfx.\u003CStopEffectAfterSeconds\u003Ec__Iterator56()
  {
    robotLifeTime = robotLifeTime,
    scrapsLifeTime = scrapsLifeTime,
    \u003C\u0024\u003ErobotLifeTime = robotLifeTime,
    \u003C\u0024\u003EscrapsLifeTime = scrapsLifeTime,
    \u003C\u003Ef__this = this
  };
}
