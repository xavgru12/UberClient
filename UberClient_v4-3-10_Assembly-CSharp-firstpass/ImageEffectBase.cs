// Decompiled with JetBrains decompiler
// Type: ImageEffectBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[AddComponentMenu("")]
[RequireComponent(typeof (Camera))]
public class ImageEffectBase : MonoBehaviour
{
  public Shader shader;
  private Material m_Material;

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

  protected virtual void OnDisable()
  {
    if (!(bool) (Object) this.m_Material)
      return;
    Object.DestroyImmediate((Object) this.m_Material);
  }
}
