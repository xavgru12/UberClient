// Decompiled with JetBrains decompiler
// Type: AudioEffectArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Collider))]
public class AudioEffectArea : MonoBehaviour
{
  [SerializeField]
  private GameObject outdoorEnvironment;
  [SerializeField]
  private GameObject indoorEnvironment;

  private void Awake()
  {
    this.collider.isTrigger = true;
    if ((Object) this.indoorEnvironment != (Object) null)
      this.indoorEnvironment.SetActive(true);
    if (!((Object) this.outdoorEnvironment != (Object) null))
      return;
    this.outdoorEnvironment.SetActive(false);
  }

  private void OnTriggerEnter(Collider collider)
  {
    if (!(collider.tag == "Player"))
      return;
    if ((Object) this.outdoorEnvironment != (Object) null)
      this.outdoorEnvironment.SetActive(false);
    if (!((Object) this.indoorEnvironment != (Object) null))
      return;
    this.indoorEnvironment.SetActive(true);
  }

  private void OnTriggerExit(Collider collider)
  {
    if (!(collider.tag == "Player"))
      return;
    if ((Object) this.outdoorEnvironment != (Object) null)
      this.outdoorEnvironment.SetActive(true);
    if (!((Object) this.indoorEnvironment != (Object) null))
      return;
    this.indoorEnvironment.SetActive(false);
  }
}
