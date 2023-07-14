// Decompiled with JetBrains decompiler
// Type: TutorialAirlockDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TutorialAirlockDoor : MonoBehaviour
{
  public TutorialAirlockDoor.AnimPlayMode PlayMode;
  public Collider BlockCollider;
  private bool _entered;

  public void Reset()
  {
    if (this._entered)
    {
      if (this.PlayMode == TutorialAirlockDoor.AnimPlayMode.Backward)
        this.transform.rotation = Quaternion.Euler(0.0f, 180f + this.transform.rotation.eulerAngles.y, 0.0f);
      else
        this.BlockCollider.enabled = true;
    }
    this._entered = false;
  }

  private void OnTriggerEnter(Collider c)
  {
    if (!(bool) (Object) LevelTutorial.Instance.AirlockDoorAnim || this._entered)
      return;
    UnityEngine.AnimationState animationState = LevelTutorial.Instance.AirlockDoorAnim["DoorOpen"];
    this._entered = true;
    if (this.PlayMode == TutorialAirlockDoor.AnimPlayMode.Backward)
    {
      if ((bool) (TrackedReference) animationState)
      {
        animationState.weight = 1f;
        animationState.speed = -1f;
        animationState.normalizedTime = 1f;
        animationState.enabled = true;
      }
      else
        UnityEngine.Debug.LogError((object) "Failed to get door animation state!");
      this.transform.rotation = Quaternion.Euler(0.0f, 180f + this.transform.rotation.eulerAngles.y, 0.0f);
      SfxManager.Play2dAudioClip(LevelTutorial.Instance.BigDoorClose);
    }
    else
    {
      animationState.enabled = false;
      animationState.weight = 0.0f;
      animationState.speed = 1f;
      animationState.normalizedTime = 0.0f;
      LevelTutorial.Instance.AirlockDoorAnim.Play();
      this.StartCoroutine(this.StartHideMe(animationState.length));
    }
  }

  [DebuggerHidden]
  private IEnumerator StartHideMe(float time) => (IEnumerator) new TutorialAirlockDoor.\u003CStartHideMe\u003Ec__Iterator3B()
  {
    time = time,
    \u003C\u0024\u003Etime = time,
    \u003C\u003Ef__this = this
  };

  public enum AnimPlayMode
  {
    Forward,
    Backward,
  }
}
