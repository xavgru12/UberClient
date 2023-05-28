// Decompiled with JetBrains decompiler
// Type: CameraComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Camera))]
public class CameraComponents : MonoBehaviour
{
  private void Awake()
  {
    this.Camera = this.GetComponent<Camera>();
    this.MouseOrbit = this.GetComponent<MouseOrbit>();
  }

  public Camera Camera { get; private set; }

  public MouseOrbit MouseOrbit { get; private set; }
}
