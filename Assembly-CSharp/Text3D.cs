// Decompiled with JetBrains decompiler
// Type: Text3D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Text3D : BitmapMeshText
{
  [SerializeField]
  private string _textContent = "Cmune";
  [SerializeField]
  private BitmapFont _font;
  [SerializeField]
  private Color _innerColor = Color.black;
  [SerializeField]
  private Color _borderColor = Color.white;
  [SerializeField]
  private Text3D.StyleType _textStyle;

  private void Awake()
  {
    this.Font = this._font;
    this.Anchor = TextAnchor.MiddleCenter;
    this.Text = this._textContent;
    switch (this._textStyle)
    {
      case Text3D.StyleType.Default:
        this.SetDefaultStyle();
        break;
      case Text3D.StyleType.Custom:
        this.SetCustomStyle();
        break;
      case Text3D.StyleType.NoBorder:
        this.SetNoShadowStyle();
        break;
    }
  }

  private void SetCustomStyle()
  {
    this.Color = this._innerColor;
    this.ShadowColor = this._borderColor;
    this.AlphaMin = 0.45f;
    this.AlphaMax = 0.62f;
    this.ShadowAlphaMin = 0.2f;
    this.ShadowAlphaMax = 0.45f;
    this.ShadowOffsetU = 0.0f;
    this.ShadowOffsetV = 0.0f;
  }

  private void SetDefaultStyle()
  {
    this.Color = Color.white;
    this.ShadowColor = HudStyleUtility.DEFAULT_BLUE_COLOR;
    this.AlphaMin = 0.45f;
    this.AlphaMax = 0.62f;
    this.ShadowAlphaMin = 0.2f;
    this.ShadowAlphaMax = 0.45f;
    this.ShadowOffsetU = 0.0f;
    this.ShadowOffsetV = 0.0f;
  }

  private void SetNoShadowStyle()
  {
    this.Color = this._innerColor;
    this.ShadowColor = new Color(1f, 1f, 1f, 0.0f);
    this.AlphaMin = 0.18f;
    this.AlphaMax = 0.62f;
    this.ShadowAlphaMin = 0.18f;
    this.ShadowAlphaMax = 0.39f;
    this.ShadowOffsetU = 0.0f;
    this.ShadowOffsetV = 0.0f;
  }

  public enum StyleType
  {
    Default,
    Custom,
    NoBorder,
  }
}
