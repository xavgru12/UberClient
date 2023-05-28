// Decompiled with JetBrains decompiler
// Type: SoundArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SoundArea : MonoBehaviour
{
  [SerializeField]
  private FootStepSoundType _footStep;

  private void OnTriggerEnter(Collider other) => this.SetFootStep(other);

  private void OnTriggerStay(Collider other) => this.SetFootStep(other);

  private void OnTriggerExit(Collider other)
  {
    if (!(other.tag == "Avatar"))
      return;
    CharacterTrigger component = other.GetComponent<CharacterTrigger>();
    if (!(bool) (Object) component || !(bool) (Object) component.Avatar || !(bool) (Object) component.Avatar.Decorator || !GameState.HasCurrentSpace)
      return;
    component.Avatar.Decorator.SetFootStep(GameState.CurrentSpace.DefaultFootStep);
  }

  private void SetFootStep(Collider other)
  {
    if (!(other.tag == "Avatar"))
      return;
    CharacterTrigger component = other.GetComponent<CharacterTrigger>();
    if (!(bool) (Object) component || !(bool) (Object) component.Avatar || !(bool) (Object) component.Avatar.Decorator)
      return;
    component.Avatar.Decorator.SetFootStep(this._footStep);
  }
}
