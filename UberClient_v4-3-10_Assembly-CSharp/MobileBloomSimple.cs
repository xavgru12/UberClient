// Decompiled with JetBrains decompiler
// Type: MobileBloomSimple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("Image Effects/Mobile Bloom (Simple)")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class MobileBloomSimple : MonoBehaviour
{
  [SerializeField]
  private float _intensity = 0.7f;
  [SerializeField]
  private float _threshhold = 0.75f;
  [SerializeField]
  private float _blurWidth = 1f;
  [SerializeField]
  private bool _extraBlurry;
  [SerializeField]
  private Shader _bloomShader = new Shader();
  [SerializeField]
  private Material _bloomMaterial;
  [SerializeField]
  private bool _supported;
  private RenderTexture _tempRtA;
  private RenderTexture _tempRtB;

  private void Start()
  {
    this.CreateMaterials();
    this.CheckSupport();
  }

  private void CreateMaterials()
  {
    if (!(bool) (Object) this._bloomShader)
      this._bloomShader = Shader.Find("Cross Platform Shaders/Mobile Bloom Simple");
    if ((bool) (Object) this._bloomMaterial)
      return;
    this._bloomMaterial = new Material(this._bloomShader);
    this._bloomMaterial.hideFlags = HideFlags.DontSave;
  }

  private bool CheckSupport()
  {
    if (this._supported)
      return true;
    this._supported = SystemInfo.supportsImageEffects && SystemInfo.supportsRenderTextures && this._bloomMaterial.shader.isSupported;
    return this._supported;
  }

  private void CreateBuffers()
  {
    if (!(bool) (Object) this._tempRtA)
    {
      this._tempRtA = new RenderTexture(Screen.width / 4, Screen.height / 4, 0);
      this._tempRtA.hideFlags = HideFlags.DontSave;
    }
    if ((bool) (Object) this._tempRtB)
      return;
    this._tempRtB = new RenderTexture(Screen.width / 4, Screen.height / 4, 0);
    this._tempRtB.hideFlags = HideFlags.DontSave;
  }

  private void OnDisable()
  {
    if ((bool) (Object) this._tempRtA)
    {
      Object.DestroyImmediate((Object) this._tempRtA);
      this._tempRtA = (RenderTexture) null;
    }
    if ((bool) (Object) this._tempRtB)
    {
      Object.DestroyImmediate((Object) this._tempRtB);
      this._tempRtB = (RenderTexture) null;
    }
    if (!(bool) (Object) this._bloomMaterial)
      return;
    Object.DestroyImmediate((Object) this._bloomMaterial);
    this._bloomMaterial = (Material) null;
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.CreateBuffers();
    this._bloomMaterial.SetVector("_Parameter", new Vector4(0.0f, 0.0f, this._threshhold, this._intensity / (1f - this._threshhold)));
    float num1 = (float) (1.0 / ((double) source.width * 1.0));
    float num2 = (float) (1.0 / ((double) source.height * 1.0));
    this._bloomMaterial.SetVector("_OffsetsA", new Vector4(1.5f * num1, 1.5f * num2, -1.5f * num1, 1.5f * num2));
    this._bloomMaterial.SetVector("_OffsetsB", new Vector4(-1.5f * num1, -1.5f * num2, 1.5f * num1, -1.5f * num2));
    Graphics.Blit((Texture) source, this._tempRtB, this._bloomMaterial, 1);
    float num3 = num1 * (4f * this._blurWidth);
    float num4 = num2 * (4f * this._blurWidth);
    this._bloomMaterial.SetVector("_OffsetsA", new Vector4(1.5f * num3, 0.0f, -1.5f * num3, 0.0f));
    this._bloomMaterial.SetVector("_OffsetsB", new Vector4(0.5f * num3, 0.0f, -0.5f * num3, 0.0f));
    Graphics.Blit((Texture) this._tempRtB, this._tempRtA, this._bloomMaterial, 2);
    this._bloomMaterial.SetVector("_OffsetsA", new Vector4(0.0f, 1.5f * num4, 0.0f, -1.5f * num4));
    this._bloomMaterial.SetVector("_OffsetsB", new Vector4(0.0f, 0.5f * num4, 0.0f, -0.5f * num4));
    Graphics.Blit((Texture) this._tempRtA, this._tempRtB, this._bloomMaterial, 2);
    if (this._extraBlurry)
    {
      this._bloomMaterial.SetVector("_OffsetsA", new Vector4(1.5f * num3, 0.0f, -1.5f * num3, 0.0f));
      this._bloomMaterial.SetVector("_OffsetsB", new Vector4(0.5f * num3, 0.0f, -0.5f * num3, 0.0f));
      Graphics.Blit((Texture) this._tempRtB, this._tempRtA, this._bloomMaterial, 2);
      this._bloomMaterial.SetVector("_OffsetsA", new Vector4(0.0f, 1.5f * num4, 0.0f, -1.5f * num4));
      this._bloomMaterial.SetVector("_OffsetsB", new Vector4(0.0f, 0.5f * num4, 0.0f, -0.5f * num4));
      Graphics.Blit((Texture) this._tempRtA, this._tempRtB, this._bloomMaterial, 2);
    }
    this._bloomMaterial.SetTexture("_Bloom", (Texture) this._tempRtB);
    Graphics.Blit((Texture) source, destination, this._bloomMaterial, 0);
  }
}
