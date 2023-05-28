// Decompiled with JetBrains decompiler
// Type: BlurEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[AddComponentMenu("Image Effects/Blur")]
[ExecuteInEditMode]
public class BlurEffect : MonoBehaviour
{
  public int iterations = 3;
  public float blurSpread = 0.6f;
  public Shader blurShader;
  private static Material m_Material;

  protected Material material
  {
    get
    {
      if ((Object) BlurEffect.m_Material == (Object) null)
      {
        BlurEffect.m_Material = new Material(this.blurShader);
        BlurEffect.m_Material.hideFlags = HideFlags.DontSave;
      }
      return BlurEffect.m_Material;
    }
  }

  protected void OnDisable()
  {
    if (!(bool) (Object) BlurEffect.m_Material)
      return;
    Object.DestroyImmediate((Object) BlurEffect.m_Material);
  }

  protected void Start()
  {
    if (!SystemInfo.supportsImageEffects)
    {
      this.enabled = false;
    }
    else
    {
      if ((bool) (Object) this.blurShader && this.material.shader.isSupported)
        return;
      this.enabled = false;
    }
  }

  public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
  {
    float num = (float) (0.5 + (double) iteration * (double) this.blurSpread);
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(0.0f - num, 0.0f - num), new Vector2(0.0f - num, num), new Vector2(num, num), new Vector2(num, 0.0f - num));
  }

  private void DownSample4x(RenderTexture source, RenderTexture dest)
  {
    float num = 1f;
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(0.0f - num, 0.0f - num), new Vector2(0.0f - num, num), new Vector2(num, num), new Vector2(num, 0.0f - num));
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    RenderTexture temporary1 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
    RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
    this.DownSample4x(source, temporary1);
    bool flag = true;
    for (int iteration = 0; iteration < this.iterations; ++iteration)
    {
      if (flag)
        this.FourTapCone(temporary1, temporary2, iteration);
      else
        this.FourTapCone(temporary2, temporary1, iteration);
      flag = !flag;
    }
    if (flag)
      Graphics.Blit((Texture) temporary1, destination);
    else
      Graphics.Blit((Texture) temporary2, destination);
    RenderTexture.ReleaseTemporary(temporary1);
    RenderTexture.ReleaseTemporary(temporary2);
  }
}
