// Decompiled with JetBrains decompiler
// Type: DeathArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Collider))]
public class DeathArea : MonoBehaviour
{
  private void Awake()
  {
    if (!(bool) (Object) this.collider)
      return;
    this.collider.isTrigger = true;
  }

  private void OnTriggerEnter(Collider c)
  {
    if (!(c.tag == "Player") || !GameState.HasCurrentPlayer)
      return;
    LevelBoundary.KillPlayer();
  }
}
