// Decompiled with JetBrains decompiler
// Type: TouchButtonCircle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TouchButtonCircle : TouchButton
{
  private Vector2 _centerPosition;
  public bool ShowEffect = true;
  public float EffectTime = 0.25f;
  private float sqrRadius;
  private float initialScale;
  private MeshGUIQuad _quad;
  private float _timer;

  public TouchButtonCircle(Texture texture)
  {
    this.Content = new GUIContent(texture);
    this.initialScale = (float) texture.width / (float) ConsumableHudTextures.CircleWhite.width;
  }

  public Vector2 CenterPosition
  {
    get => this._centerPosition;
    set
    {
      if (!(value != this._centerPosition))
        return;
      if (this.Content != null)
      {
        float num1 = (float) (this.Content.image.width / 2);
        this.Boundary = new Rect(value.x - num1, value.y - num1, (float) this.Content.image.width, (float) this.Content.image.height);
        float num2 = num1 + 5f;
        this.sqrRadius = num2 * num2;
      }
      this._centerPosition = value;
    }
  }

  public override bool Enabled
  {
    get => base.Enabled;
    set
    {
      base.Enabled = value;
      if (base.Enabled || this._quad == null)
        return;
      this._quad.FreeObject();
      this._quad = (MeshGUIQuad) null;
    }
  }

  public override void FinalUpdate()
  {
    base.FinalUpdate();
    if (!this.ShowEffect)
      return;
    if (this._quad == null && this.finger.FingerId != -1)
    {
      this._quad = new MeshGUIQuad((Texture) ConsumableHudTextures.CircleWhite, TextAnchor.MiddleCenter);
      this._quad.Position = this.CenterPosition - new Vector2(3f, 0.0f);
      this._quad.Scale = new Vector2(this.initialScale, this.initialScale);
      this._timer = 0.0f;
    }
    if (this._quad != null)
    {
      this._quad.Scale = new Vector2((float) ((double) this._timer / (double) this.EffectTime + 1.0) * this.initialScale, (float) ((double) this._timer / (double) this.EffectTime + 1.0) * this.initialScale);
      this._quad.Alpha = (float) (1.0 - (double) this._timer / (double) this.EffectTime);
      this._timer += Time.deltaTime;
    }
    if ((double) this._timer <= (double) this.EffectTime)
      return;
    this._timer = 0.0f;
    if (this.finger.FingerId != -1)
      return;
    this._quad.FreeObject();
    this._quad = (MeshGUIQuad) null;
  }

  public override void Draw()
  {
    GUI.color = new Color(1f, 1f, 1f, Mathf.Clamp(Singleton<TouchController>.Instance.GUIAlpha, this.MinGUIAlpha, 1f));
    if (this.Content != null)
      GUI.Label(this.Boundary, this.Content);
    else
      Debug.LogWarning((object) "You need to set a CenterPosition and Texture for the TouchButtonCircle to draw!");
    GUI.color = Color.white;
  }

  protected override bool TouchInside(Vector2 position)
  {
    Vector2 vector2 = new Vector2(this.Boundary.x + this.Boundary.width / 2f, this.Boundary.y + this.Boundary.height / 2f);
    vector2.y = (float) Screen.height - vector2.y;
    return (double) (vector2 - position).sqrMagnitude < (double) this.sqrRadius;
  }
}
