// Decompiled with JetBrains decompiler
// Type: Displace
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21AF7BBC-70B8-4BE8-9CDE-C2EC2144EAE4
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[RequireComponent(typeof (WaterBase))]
[ExecuteInEditMode]
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
