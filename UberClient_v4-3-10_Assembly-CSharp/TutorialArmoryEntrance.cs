// Decompiled with JetBrains decompiler
// Type: TutorialArmoryEntrance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TutorialArmoryEntrance : MonoBehaviour
{
  public Collider Block;
  private bool _entered;

  private void OnTriggerEnter(Collider c)
  {
    if (this._entered || !(c.tag == "Player"))
      return;
    this._entered = true;
    if (!GameState.HasCurrentGame || !(GameState.CurrentGame is TutorialGameMode))
      return;
    (GameState.CurrentGame as TutorialGameMode).Sequence.OnArmoryEnter();
    this.Block.isTrigger = false;
    this.StartCoroutine(this.StartDeleteMe());
  }

  [DebuggerHidden]
  private IEnumerator StartDeleteMe() => (IEnumerator) new TutorialArmoryEntrance.\u003CStartDeleteMe\u003Ec__Iterator3E()
  {
    \u003C\u003Ef__this = this
  };
}
