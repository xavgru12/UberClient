// Decompiled with JetBrains decompiler
// Type: ImageEffectBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 21AF7BBC-70B8-4BE8-9CDE-C2EC2144EAE4
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("")]
public class ImageEffectBase : MonoBehaviour
{
  public Shader shader;
  private Material m_Material;

  protected virtual void Start()
  {
    if (!SystemInfo.supportsImageEffects)
    {
      this.enabled = false;
    }
    else
    {
      if ((bool) (Object) this.shader && this.shader.isSupported)
        return;
      this.enabled = false;
    }
  }

  protected Material material
  {
    get
    {
      if ((Object) this.m_Material == (Object) null)
      {
        this.m_Material = new Material(this.shader);
        this.m_Material.hideFlags = HideFlags.HideAndDontSave;
      }
      return this.m_Material;
    }
  }

  protected virtual void OnDisable()
  {
    if (!(bool) (Object) this.m_Material)
      return;
    Object.DestroyImmediate((Object) this.m_Material);
  }
}
