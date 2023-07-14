// Decompiled with JetBrains decompiler
// Type: MobileBloom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Mobile Bloom")]
[RequireComponent(typeof (Camera))]
public class MobileBloom : MonoBehaviour
{
  public float intensity = 0.5f;
  public Color colorMix = Color.white;
  public float colorMixBlend = 0.25f;
  public float agonyTint;
  public float intensityBoost;
  private Shader bloomShader = new Shader();
  private Material apply;
  private RenderTextureFormat rtFormat = RenderTextureFormat.Default;

  private void Start()
  {
    this.FindShaders();
    this.CheckSupport();
    this.CreateMaterials();
  }

  private void FindShaders()
  {
    if ((bool) (Object) this.bloomShader)
      return;
    this.bloomShader = Shader.Find("Cross Platform Shaders/Mobile Bloom");
  }

  private void CreateMaterials()
  {
    if ((bool) (Object) this.apply)
      return;
    this.apply = new Material(this.bloomShader);
    this.apply.hideFlags = HideFlags.DontSave;
  }

  private bool CheckSupport()
  {
    if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures || !this.bloomShader.isSupported)
    {
      this.enabled = false;
      return false;
    }
    this.rtFormat = !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGB565) ? RenderTextureFormat.Default : RenderTextureFormat.RGB565;
    return true;
  }

  private void OnDisable()
  {
    if (!(bool) (Object) this.apply)
      return;
    Object.DestroyImmediate((Object) this.apply);
    this.apply = (Material) null;
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.agonyTint = Mathf.Clamp01(this.agonyTint - Time.deltaTime * 1.25f);
    this.intensityBoost = Mathf.Clamp01(this.intensityBoost - Time.deltaTime * 0.75f);
    RenderTexture temporary1 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, this.rtFormat);
    RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, this.rtFormat);
    this.apply.SetColor("_ColorMix", this.colorMix);
    this.apply.SetVector("_Parameter", new Vector4(this.colorMixBlend * 0.25f, 0.0f, 0.0f, (float) (1.0 - (double) this.intensity - ((double) this.agonyTint + (double) this.intensityBoost))));
    Graphics.Blit((Texture) source, temporary1, this.apply, (double) this.agonyTint >= 0.5 ? 5 : 1);
    Graphics.Blit((Texture) temporary1, temporary2, this.apply, 2);
    Graphics.Blit((Texture) temporary2, temporary1, this.apply, 3);
    this.apply.SetTexture("_Bloom", (Texture) temporary1);
    Graphics.Blit((Texture) source, destination, this.apply, 4);
    RenderTexture.ReleaseTemporary(temporary1);
    RenderTexture.ReleaseTemporary(temporary2);
  }
}
