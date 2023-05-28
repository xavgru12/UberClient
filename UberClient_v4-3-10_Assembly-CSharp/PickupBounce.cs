// Decompiled with JetBrains decompiler
// Type: PickupBounce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PickupBounce : MonoBehaviour
{
  private float origPosY;
  private float startOffset;

  private void Awake()
  {
    this.origPosY = this.transform.position.y;
    this.startOffset = Random.value * 3f;
  }

  private void FixedUpdate()
  {
    this.transform.Rotate(new Vector3(0.0f, 2f, 0.0f));
    this.transform.position = new Vector3(this.transform.position.x, this.origPosY + Mathf.Sin((float) (((double) this.startOffset + (double) Time.realtimeSinceStartup) * 4.0)) * 0.08f, this.transform.position.z);
  }
}
