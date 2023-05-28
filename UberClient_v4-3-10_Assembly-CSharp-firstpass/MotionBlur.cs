// Decompiled with JetBrains decompiler
// Type: MotionBlur
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Motion Blur (Color Accumulation)")]
public class MotionBlur : ImageEffectBase
{
  public float blurAmount = 0.8f;
  public bool extraBlur;
  private RenderTexture accumTexture;

  protected override void Start()
  {
    if (!SystemInfo.supportsRenderTextures)
      this.enabled = false;
    else
      base.Start();
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    Object.DestroyImmediate((Object) this.accumTexture);
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((Object) this.accumTexture == (Object) null || this.accumTexture.width != source.width || this.accumTexture.height != source.height)
    {
      Object.DestroyImmediate((Object) this.accumTexture);
      this.accumTexture = new RenderTexture(source.width, source.height, 0);
      this.accumTexture.hideFlags = HideFlags.HideAndDontSave;
      Graphics.Blit((Texture) source, this.accumTexture);
    }
    if (this.extraBlur)
    {
      RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
      Graphics.Blit((Texture) this.accumTexture, temporary);
      Graphics.Blit((Texture) temporary, this.accumTexture);
      RenderTexture.ReleaseTemporary(temporary);
    }
    this.blurAmount = Mathf.Clamp(this.blurAmount, 0.0f, 0.92f);
    this.material.SetTexture("_MainTex", (Texture) this.accumTexture);
    this.material.SetFloat("_AccumOrig", 1f - this.blurAmount);
    Graphics.Blit((Texture) source, this.accumTexture, this.material);
    Graphics.Blit((Texture) this.accumTexture, destination);
  }
}
