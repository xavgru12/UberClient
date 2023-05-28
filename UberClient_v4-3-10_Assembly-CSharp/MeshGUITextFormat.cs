// Decompiled with JetBrains decompiler
// Type: MeshGUITextFormat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MeshGUITextFormat : Animatable2DGroup
{
  private string _formatText;
  private BitmapFont _bitmapFont;
  private TextAnchor _textAnchor;
  private Vector2Anim _scaleAnim;
  private ColorAnim _shadowColorAnim;
  private float _verticalGapBetweenLines;
  private MeshGUITextFormat.SetStyle _setStyleFunc;

  public MeshGUITextFormat(
    string formatText,
    BitmapFont bitmapFont,
    TextAlignment textAlignment = TextAlignment.Left,
    MeshGUITextFormat.SetStyle setTyleFunc = null)
  {
    this._bitmapFont = bitmapFont;
    this._setStyleFunc = setTyleFunc;
    this.SetTextAlignment(textAlignment);
    this._scaleAnim = new Vector2Anim(new Vector2Anim.OnValueChange(this.OnScaleChange));
    this._scaleAnim.Vec2 = Vector2.one;
    this._setStyleFunc = setTyleFunc;
    this.FormatText = formatText;
  }

  public string FormatText
  {
    get => this._formatText;
    set
    {
      this._formatText = value;
      this.UpdateMeshTextGroup();
    }
  }

  public Vector2 Scale
  {
    get => this._scaleAnim.Vec2;
    set => this._scaleAnim.Vec2 = value;
  }

  public float LineGap
  {
    get => this._verticalGapBetweenLines;
    set
    {
      this._verticalGapBetweenLines = value;
      this.ResetTransform();
    }
  }

  public float LineHeight => this.Group.Count > 0 ? (this.Group[0] as MeshGUIText).Size.y : 0.0f;

  public ColorAnim ShadowColorAnim
  {
    get
    {
      if (this._shadowColorAnim == null)
        this._shadowColorAnim = new ColorAnim(new ColorAnim.OnValueChange(this.OnShadowColorChange));
      return this._shadowColorAnim;
    }
  }

  public MeshGUITextFormat.SetStyle SetStyleFunc
  {
    set => this._setStyleFunc = value;
  }

  public void UpdateStyle()
  {
    for (int index = 0; index < this.Group.Count; ++index)
    {
      if (this.Group[index] is MeshGUIText meshText && this._setStyleFunc != null)
        this._setStyleFunc(meshText);
    }
  }

  private void SetTextAlignment(TextAlignment textAlignment)
  {
    switch (textAlignment)
    {
      case TextAlignment.Left:
        this._textAnchor = TextAnchor.UpperLeft;
        break;
      case TextAlignment.Center:
        this._textAnchor = TextAnchor.UpperCenter;
        break;
      case TextAlignment.Right:
        this._textAnchor = TextAnchor.UpperRight;
        break;
    }
  }

  private void UpdateMeshTextGroup()
  {
    this.ClearAndFree();
    foreach (string text in this.GetStringArray(this._formatText))
    {
      MeshGUIText meshText = new MeshGUIText(text, this._bitmapFont, this._textAnchor);
      if (this._setStyleFunc != null)
        this._setStyleFunc(meshText);
      this.Group.Add((IAnimatable2D) meshText);
    }
    this.ResetTransform();
  }

  private List<string> GetStringArray(string formatStr)
  {
    List<string> stringArray = new List<string>();
    string str = formatStr;
    while (true)
    {
      int length = str.IndexOf('\n');
      if (length != -1)
      {
        stringArray.Add(str.Substring(0, length));
        str = str.Substring(length + 1);
      }
      else
        break;
    }
    if (!string.IsNullOrEmpty(str))
      stringArray.Add(str);
    return stringArray;
  }

  private void OnShadowColorChange(Color oldColor, Color newColor)
  {
    foreach (IAnimatable2D animatable2D in this.Group)
      (animatable2D as MeshGUIText).ShadowColorAnim.Color = newColor;
  }

  private void OnScaleChange(Vector2 oldScale, Vector2 newScale) => this.ResetTransform();

  private void ResetTransform()
  {
    Vector2 zero = Vector2.zero;
    for (int index = 0; index < this.Group.Count; ++index)
    {
      if (this.Group[index] is MeshGUIText meshGuiText)
      {
        meshGuiText.Scale = this._scaleAnim.Vec2;
        meshGuiText.Position = zero;
        zero += new Vector2(0.0f, meshGuiText.Size.y + this._verticalGapBetweenLines);
      }
    }
  }

  public delegate void SetStyle(MeshGUIText meshText);
}
