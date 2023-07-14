// Decompiled with JetBrains decompiler
// Type: SpecularLighting
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21AF7BBC-70B8-4BE8-9CDE-C2EC2144EAE4
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[RequireComponent(typeof (WaterBase))]
[ExecuteInEditMode]
public class SpecularLighting : MonoBehaviour
{
  public Transform specularLight;
  private WaterBase waterBase;

  public void Start() => this.waterBase = (WaterBase) this.gameObject.GetComponent(typeof (WaterBase));

  public void Update()
  {
    if (!(bool) (Object) this.waterBase)
      this.waterBase = (WaterBase) this.gameObject.GetComponent(typeof (WaterBase));
    if (!(bool) (Object) this.specularLight || !(bool) (Object) this.waterBase.sharedMaterial)
      return;
    this.waterBase.sharedMaterial.SetVector("_WorldLightDir", (Vector4) this.specularLight.transform.forward);
  }
}
