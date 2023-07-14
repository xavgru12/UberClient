// Decompiled with JetBrains decompiler
// Type: SoundOnTouchArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SoundOnTouchArea : MonoBehaviour
{
  [SerializeField]
  private Transform source;

  private void OnTriggerStay(Collider other)
  {
    if (!(other.tag == "Avatar"))
      return;
    CharacterTrigger component = other.GetComponent<CharacterTrigger>();
    if (!(bool) (Object) component || !component.Avatar.IsLocal)
      return;
    this.source.position = GameState.LocalCharacter.Position + Vector3.down;
  }
}
