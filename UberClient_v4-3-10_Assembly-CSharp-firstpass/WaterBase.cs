// Decompiled with JetBrains decompiler
// Type: WaterBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
public class WaterBase : MonoBehaviour
{
  public Material sharedMaterial;
  public WaterQuality waterQuality = WaterQuality.High;
  public bool edgeBlend = true;

  public void UpdateShader()
  {
    this.sharedMaterial.shader.maximumLOD = this.waterQuality <= WaterQuality.Medium ? (this.waterQuality <= WaterQuality.Low ? 201 : 301) : 501;
    if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
      this.edgeBlend = false;
    if (this.edgeBlend)
    {
      Shader.EnableKeyword("WATER_EDGEBLEND_ON");
      Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
      if (!(bool) (Object) Camera.main)
        return;
      Camera.main.depthTextureMode |= DepthTextureMode.Depth;
    }
    else
    {
      Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
      Shader.DisableKeyword("WATER_EDGEBLEND_ON");
    }
  }

  public void WaterTileBeingRendered(Transform tr, Camera currentCam)
  {
    if (!(bool) (Object) currentCam || !this.edgeBlend)
      return;
    currentCam.depthTextureMode |= DepthTextureMode.Depth;
  }

  public void Update()
  {
    if (!(bool) (Object) this.sharedMaterial)
      return;
    this.UpdateShader();
  }
}
