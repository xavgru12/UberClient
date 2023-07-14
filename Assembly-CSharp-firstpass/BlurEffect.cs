// Decompiled with JetBrains decompiler
// Type: BlurEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21AF7BBC-70B8-4BE8-9CDE-C2EC2144EAE4
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Blur")]
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
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
  }

  private void DownSample4x(RenderTexture source, RenderTexture dest)
  {
    float num = 1f;
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
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
