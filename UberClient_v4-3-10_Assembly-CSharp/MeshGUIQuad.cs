// Decompiled with JetBrains decompiler
// Type: MeshGUIQuad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MeshGUIQuad : MeshGUIBase
{
  public MeshGUIQuad(Texture tex, TextAnchor anchor = TextAnchor.UpperLeft, GameObject parentObject = null)
    : base(parentObject)
  {
    this.Texture = tex;
    QuadMesh quadMesh = this.QuadMesh;
    if (!((Object) quadMesh != (Object) null))
      return;
    quadMesh.Anchor = anchor;
  }

  public override void FreeObject() => MeshGUIManager.Instance.FreeQuadMesh(this._meshObject);

  public override Vector2 GetOriginalBounds() => (Object) this.Texture != (Object) null ? new Vector2((float) this.Texture.width, (float) this.Texture.height) : Vector2.zero;

  protected override Vector2 GetAdjustScale() => (Object) this.Texture != (Object) null ? (Vector2) MeshGUIManager.Instance.TransformSizeFromScreenToWorld(new Vector2((float) this.Texture.width, (float) this.Texture.height)) : Vector2.zero;

  protected override CustomMesh GetCustomMesh() => (Object) this._meshObject != (Object) null ? (CustomMesh) this._meshObject.GetComponent<QuadMesh>() : (CustomMesh) null;

  protected override GameObject AllocObject(GameObject parentObject) => MeshGUIManager.Instance.CreateQuadMesh(parentObject);

  protected override void UpdateRect()
  {
    Vector2 screen = MeshGUIManager.Instance.TransformSizeFromWorldToScreen((Vector3) this.QuadMesh.OffsetToUpperLeft);
    this._rect.x = this.Position.x - screen.x * this.Scale.x;
    this._rect.y = this.Position.y - screen.y * this.Scale.y;
    this._rect.width = this.Size.x;
    this._rect.height = this.Size.y;
  }

  public Texture Texture
  {
    get => this.QuadMesh.Texture;
    set => this.QuadMesh.Texture = value;
  }

  public QuadMesh QuadMesh => this._customeMesh as QuadMesh;
}
