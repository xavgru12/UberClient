// Decompiled with JetBrains decompiler
// Type: Displace
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (WaterBase))]
public class Displace : MonoBehaviour
{
  public void Awake()
  {
    if (this.enabled)
      this.OnEnable();
    else
      this.OnDisable();
  }

  public void OnEnable()
  {
    Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
    Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
  }

  public void OnDisable()
  {
    Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
    Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
  }
}
