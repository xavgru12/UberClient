// Decompiled with JetBrains decompiler
// Type: SilhouetteOutlined
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("Image Effects/Silhouette Outlined")]
[ExecuteInEditMode]
public class SilhouetteOutlined : ImageEffectBase
{
  [SerializeField]
  private Shader generateGlowTextureShader;
  [SerializeField]
  private Shader gaussianBlurShader;
  [SerializeField]
  private Shader objectMaskShader;
  [SerializeField]
  private Color globalOutlineColor;
  [SerializeField]
  private bool isUseGlobalColor;
  private Material _gaussianBlurMaterial;
  private RenderTexture _glowTexture;
  private RenderTexture _maskTexture;
  private GameObject _shaderCamera;
  private int _glowTexWidth;
  private int _glowTexHeight;

  protected Material GaussianBlurMaterial
  {
    get
    {
      if ((UnityEngine.Object) this._gaussianBlurMaterial == (UnityEngine.Object) null)
      {
        this._gaussianBlurMaterial = new Material(this.gaussianBlurShader);
        this._gaussianBlurMaterial.hideFlags = HideFlags.HideAndDontSave;
      }
      return this._gaussianBlurMaterial;
    }
  }

  protected GameObject ShaderCamera
  {
    get
    {
      if (!(bool) (UnityEngine.Object) this._shaderCamera)
      {
        this._shaderCamera = new GameObject(nameof (ShaderCamera), new System.Type[1]
        {
          typeof (Camera)
        });
        this._shaderCamera.camera.enabled = false;
        this._shaderCamera.hideFlags = HideFlags.HideAndDontSave;
      }
      return this._shaderCamera;
    }
  }

  private new void OnDisable()
  {
    base.OnDisable();
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this._shaderCamera);
    if (!((UnityEngine.Object) this._glowTexture != (UnityEngine.Object) null))
      return;
    RenderTexture.ReleaseTemporary(this._glowTexture);
    this._glowTexture = (RenderTexture) null;
  }

  private void OnPreRender()
  {
    if (!this.enabled || !this.gameObject.activeSelf)
      return;
    this.CleanRenderTextures();
    Camera camera = this.ShaderCamera.camera;
    camera.CopyFrom(this.camera);
    camera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    camera.clearFlags = CameraClearFlags.Color;
    this._maskTexture = RenderTexture.GetTemporary((int) this.camera.pixelWidth, (int) this.camera.pixelHeight, 16);
    camera.targetTexture = this._maskTexture;
    camera.RenderWithShader(this.objectMaskShader, "Outline");
    this.UpdateGlowTextureSize(this.camera.pixelWidth, this.camera.pixelHeight);
    this._glowTexture = RenderTexture.GetTemporary(this._glowTexWidth, this._glowTexHeight, 16);
    camera.targetTexture = this._glowTexture;
    camera.RenderWithShader(this.generateGlowTextureShader, "Outline");
  }

  private void UpdateGlowTextureSize(float cameraWidth, float cameraHeight)
  {
    this._glowTexWidth = (int) cameraWidth;
    this._glowTexHeight = (int) cameraHeight;
    float num = cameraWidth / cameraHeight;
    if ((double) cameraWidth > 256.0 && (double) cameraWidth < 512.0)
    {
      this._glowTexWidth = 256;
      this._glowTexHeight = (int) ((double) this._glowTexWidth / (double) num);
    }
    if ((double) cameraWidth <= 512.0)
      return;
    this._glowTexWidth = 512;
    this._glowTexHeight = (int) ((double) this._glowTexWidth / (double) num);
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.GaussianBlur(source, destination);
    this.CleanRenderTextures();
  }

  private void GaussianBlur(RenderTexture source, RenderTexture dest)
  {
    RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
    this.GaussianBlurMaterial.SetFloat("_TexWidth", (float) this._glowTexWidth);
    this.GaussianBlurMaterial.SetFloat("_TexHeight", (float) this._glowTexHeight);
    Graphics.Blit((Texture) this._glowTexture, temporary, this.GaussianBlurMaterial, 0);
    Graphics.Blit((Texture) temporary, this._glowTexture, this.GaussianBlurMaterial, 1);
    RenderTexture.ReleaseTemporary(temporary);
    this.material.SetTexture("_GlowTex", (Texture) this._glowTexture);
    this.material.SetTexture("_MaskTex", (Texture) this._maskTexture);
    this.material.SetFloat("_IsUseGlobalColor", !this.isUseGlobalColor ? 0.0f : 1f);
    this.material.SetColor("_GlobalOutlineColor", this.globalOutlineColor);
    Graphics.Blit((Texture) source, dest, this.material);
  }

  private void NoBlur(RenderTexture source, RenderTexture dest)
  {
    this.material.SetTexture("_GlowTex", (Texture) this._glowTexture);
    this.material.SetTexture("_MaskTex", (Texture) this._maskTexture);
    this.material.SetFloat("_IsUseGlobalColor", !this.isUseGlobalColor ? 0.0f : 1f);
    this.material.SetColor("_GlobalOutlineColor", this.globalOutlineColor);
    Graphics.Blit((Texture) source, dest, this.material);
  }

  private void CleanRenderTextures()
  {
    if ((UnityEngine.Object) this._glowTexture != (UnityEngine.Object) null)
    {
      RenderTexture.ReleaseTemporary(this._glowTexture);
      this._glowTexture = (RenderTexture) null;
    }
    if (!((UnityEngine.Object) this._maskTexture != (UnityEngine.Object) null))
      return;
    RenderTexture.ReleaseTemporary(this._maskTexture);
    this._maskTexture = (RenderTexture) null;
  }
}
