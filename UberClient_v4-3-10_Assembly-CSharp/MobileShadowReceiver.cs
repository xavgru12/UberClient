// Decompiled with JetBrains decompiler
// Type: MobileShadowReceiver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MobileShadowReceiver : MonoBehaviour
{
  public void OnWillRenderObject()
  {
    Camera camera = (Camera) null;
    for (int index = 0; index < Camera.allCameras.Length; ++index)
    {
      if (Camera.allCameras[index].name == "Shadow Camera")
      {
        camera = Camera.allCameras[index];
        break;
      }
    }
    if (!((Object) camera != (Object) null))
      return;
    this.renderer.material.SetMatrix("_LocalToShadowMatrix", camera.projectionMatrix * camera.worldToCameraMatrix * this.renderer.localToWorldMatrix);
  }
}
