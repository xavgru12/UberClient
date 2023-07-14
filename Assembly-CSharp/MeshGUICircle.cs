// Decompiled with JetBrains decompiler
// Type: MeshGUICircle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MeshGUICircle : MeshGUIBase
{
  private FloatAnim _angleAnim;

  public MeshGUICircle(Texture tex, GameObject parentObject = null)
    : base(parentObject)
  {
    this.Texture = tex;
    this._angleAnim = new FloatAnim(new FloatAnim.OnValueChange(this.OnAngleChange));
  }

  public override void FreeObject() => MeshGUIManager.Instance.FreeCircleMesh(this._meshObject);

  public override void Draw(float offsetX = 0.0f, float offsetY = 0.0f)
  {
    base.Draw(offsetX, offsetY);
    this._angleAnim.Update();
  }

  public override Vector2 GetOriginalBounds() => (Object) this.Texture != (Object) null ? new Vector2((float) this.Texture.width, (float) this.Texture.height) : Vector2.zero;

  public void AnimAngleTo(float destAngle, float time = 0.0f, EaseType easeType = EaseType.None) => this._angleAnim.AnimTo(destAngle, time, easeType);

  public void AnimAngleDelta(float deltaAngle, float time = 0.0f, EaseType easeType = EaseType.None) => this._angleAnim.AnimBy(deltaAngle, time, easeType);

  protected override Vector2 GetAdjustScale() => (Object) this.Texture != (Object) null ? (Vector2) MeshGUIManager.Instance.TransformSizeFromScreenToWorld(new Vector2((float) this.Texture.width, (float) this.Texture.height)) : Vector2.zero;

  protected override CustomMesh GetCustomMesh() => (Object) this._meshObject != (Object) null ? (CustomMesh) this._meshObject.GetComponent<CircleMesh>() : (CustomMesh) null;

  protected override GameObject AllocObject(GameObject parentObject) => MeshGUIManager.Instance.CreateCircleMesh(parentObject);

  protected override void UpdateRect()
  {
    this._rect.x = this.Position.x - this.Size.x / 2f;
    this._rect.y = this.Position.y - this.Size.x / 2f;
    this._rect.width = this.Size.x;
    this._rect.height = this.Size.y;
  }

  public Texture Texture
  {
    get => this.CircleMesh.Texture;
    set => this.CircleMesh.Texture = value;
  }

  public CircleMesh CircleMesh => this._customeMesh as CircleMesh;

  public float Angle
  {
    get => this.CircleMesh.FillAngle;
    set => this.CircleMesh.FillAngle = value;
  }

  private void OnAngleChange(float oldAngle, float newAngle) => this.CircleMesh.FillAngle = newAngle;
}
