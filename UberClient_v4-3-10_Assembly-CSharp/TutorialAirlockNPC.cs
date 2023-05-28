// Decompiled with JetBrains decompiler
// Type: TutorialAirlockNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TutorialAirlockNPC : MonoBehaviour
{
  private Transform _transform;
  private Vector3 _finalPos = Vector3.zero;
  private TutorialAirlockNPC.State _state;

  private void Awake()
  {
    UnityEngine.AnimationState animationState = this.animation[AnimationIndex.TutorialGuideWalk.ToString()];
    animationState.enabled = true;
    animationState.weight = 1f;
    animationState.speed = 1f;
    this._transform = this.transform;
  }

  private void Update()
  {
    if (!(bool) (Object) this._transform || this._state != TutorialAirlockNPC.State.Moving)
      return;
    if ((double) Vector3.SqrMagnitude(this._transform.position - this._finalPos) < 0.10000000149011612)
    {
      this._state = TutorialAirlockNPC.State.Talking;
      this.animation.Stop(AnimationIndex.heavyGunUpDown.ToString());
      this.animation.Blend(AnimationIndex.TutorialGuideWalk.ToString(), 0.0f);
      this.animation.Blend(AnimationIndex.TutorialGuideAirlock.ToString(), 1f);
      this.StartCoroutine(this.StartIdleAnimation());
    }
    else
      this._transform.position += this._transform.forward * Time.deltaTime * 0.7f;
  }

  [DebuggerHidden]
  private IEnumerator StartIdleAnimation() => (IEnumerator) new TutorialAirlockNPC.\u003CStartIdleAnimation\u003Ec__Iterator3C()
  {
    \u003C\u003Ef__this = this
  };

  public void SetFinalPosition(Vector3 pos) => this._finalPos = pos;

  private enum State
  {
    Moving,
    Talking,
  }
}
