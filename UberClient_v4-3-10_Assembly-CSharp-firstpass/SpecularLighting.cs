// Decompiled with JetBrains decompiler
// Type: SpecularLighting
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
