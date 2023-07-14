// Decompiled with JetBrains decompiler
// Type: CustomMesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class CustomMesh : MonoBehaviour
{
  protected bool _isMaterialInitialized;
  protected Vector2 _bounds;
  protected MeshFilter _meshFilter;
  protected MeshRenderer _meshRenderer;
  protected Color _color;
  protected Texture _texture;
  [SerializeField]
  protected Shader _shader;

  public Vector2 Bounds => this._bounds;

  public bool IsVisible
  {
    get => (Object) this._meshRenderer != (Object) null && this._meshRenderer.enabled;
    set
    {
      if (!((Object) this._meshRenderer != (Object) null))
        return;
      this._meshRenderer.enabled = value;
    }
  }

  public virtual Color Color
  {
    get => this._color;
    set
    {
      this._color = value;
      if (!((Object) this._meshRenderer != (Object) null))
        return;
      this._meshRenderer.material.SetColor("_Color", value);
    }
  }

  public virtual float Alpha
  {
    get => this._color.a;
    set
    {
      this._color.a = value;
      if (!((Object) this._meshRenderer != (Object) null))
        return;
      this._meshRenderer.material.SetColor("_Color", this._color);
    }
  }

  public Texture Texture
  {
    get => this._texture;
    set
    {
      this._texture = value;
      if (!((Object) this._meshRenderer != (Object) null))
        return;
      this._meshRenderer.material.SetTexture("_MainTex", value);
    }
  }

  protected virtual void Reset()
  {
    if ((Object) this._meshRenderer == (Object) null)
      this._meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
    if ((Object) this._meshFilter == (Object) null)
      this._meshFilter = this.gameObject.AddComponent<MeshFilter>();
    Mesh mesh = this.GenerateMesh();
    if ((Object) this._meshFilter.sharedMesh != (Object) null)
    {
      if (Application.isPlaying)
        Object.Destroy((Object) this._meshFilter.sharedMesh);
      else
        Object.DestroyImmediate((Object) this._meshFilter.sharedMesh);
    }
    this._meshFilter.mesh = mesh;
    if (this._isMaterialInitialized)
      return;
    this._meshRenderer.material = new Material(this._shader);
    this._meshRenderer.enabled = this.IsVisible;
    this._meshRenderer.material.SetColor("_Color", this._color);
    this._meshRenderer.material.SetTexture("_MainTex", this._texture);
    this._isMaterialInitialized = true;
  }

  protected abstract Mesh GenerateMesh();
}
