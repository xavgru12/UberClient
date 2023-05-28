// Decompiled with JetBrains decompiler
// Type: MeshGUIText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MeshGUIText : MeshGUIBase
{
  private string _namePrefix;
  private ColorAnim _shadowColorAnim;

  public MeshGUIText(string text, BitmapFont font, TextAnchor textAnchor = TextAnchor.UpperLeft, GameObject parentObject = null)
    : base(parentObject)
  {
    BitmapMeshText bitmapMeshText = this.BitmapMeshText;
    if ((Object) bitmapMeshText != (Object) null)
    {
      bitmapMeshText.Font = font;
      bitmapMeshText.Anchor = textAnchor;
    }
    this.Text = text;
    this.ShadowColorAnim.Color = this.BitmapMeshText.ShadowColor;
  }

  public override void FreeObject() => MeshGUIManager.Instance.FreeMeshText(this._meshObject);

  public override Vector2 GetOriginalBounds() => this.TextBounds;

  protected override Vector2 GetAdjustScale() => Vector2.one;

  protected override CustomMesh GetCustomMesh() => (Object) this._meshObject != (Object) null ? (CustomMesh) this._meshObject.GetComponent<BitmapMeshText>() : (CustomMesh) null;

  protected override GameObject AllocObject(GameObject parentObject) => MeshGUIManager.Instance.CreateMeshText(parentObject);

  protected override void UpdateRect()
  {
    Vector2 screen = MeshGUIManager.Instance.TransformSizeFromWorldToScreen((Vector3) this.BitmapMeshText.OffsetToUpperLeft);
    this._rect.x = this.Position.x - screen.x * this.Scale.x;
    this._rect.y = this.Position.y - screen.y * this.Scale.y;
    this._rect.width = this.Size.x;
    this._rect.height = this.Size.y;
  }

  public string NamePrefix
  {
    get => this._namePrefix;
    set
    {
      this._namePrefix = value;
      this.UpdateName();
    }
  }

  public string Text
  {
    get => this.BitmapMeshText.Text;
    set
    {
      if (!(this.BitmapMeshText.Text != value))
        return;
      this.BitmapMeshText.Text = value;
      this.UpdateName();
    }
  }

  public ColorAnim ShadowColorAnim
  {
    get
    {
      if (this._shadowColorAnim == null)
        this._shadowColorAnim = new ColorAnim(new ColorAnim.OnValueChange(this.OnShadowColorChange));
      return this._shadowColorAnim;
    }
  }

  public BitmapMeshText BitmapMeshText => this._customeMesh as BitmapMeshText;

  public Vector2 TextBounds => MeshGUIManager.Instance.TransformSizeFromWorldToScreen((Vector3) this.BitmapMeshText.Bounds);

  private void OnShadowColorChange(Color oldColor, Color newColor) => this.BitmapMeshText.ShadowColor = newColor;

  private void UpdateName()
  {
    if (string.IsNullOrEmpty(this._namePrefix))
      this._meshObject.name = this.Text;
    else
      this._meshObject.name = this._namePrefix + "_" + this.Text;
  }
}
