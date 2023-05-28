// Decompiled with JetBrains decompiler
// Type: ArmorHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ArmorHud : Singleton<ArmorHud>
{
  private MeshGUIText _defenseBonusText;
  private MeshGUIText _defenseBonusSymbol;
  private MeshGUIText _defenseBonusValue;
  private MeshGUIText _armorCarriedText;
  private MeshGUIText _armorCarriedSymbol;
  private MeshGUIText _armorCarriedValue;
  private Animatable2DGroup _meshGUITexts;

  private ArmorHud()
  {
    this._armorCarriedValue = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleRight);
    this._armorCarriedSymbol = new MeshGUIText("AP", HudAssets.Instance.HelveticaBitmapFont, TextAnchor.MiddleRight);
    this._armorCarriedText = new MeshGUIText(LocalizedStrings.ArmorCarried, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleRight);
    this._defenseBonusValue = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleRight);
    this._defenseBonusSymbol = new MeshGUIText("%", HudAssets.Instance.HelveticaBitmapFont, TextAnchor.MiddleRight);
    this._defenseBonusText = new MeshGUIText(LocalizedStrings.DefenseBonus, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleRight);
    Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._defenseBonusText);
    Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._defenseBonusSymbol);
    Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._defenseBonusValue);
    Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._armorCarriedText);
    Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._armorCarriedSymbol);
    Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._armorCarriedValue);
    this._meshGUITexts = new Animatable2DGroup();
    this._meshGUITexts.Group.Add((IAnimatable2D) this._armorCarriedValue);
    this._meshGUITexts.Group.Add((IAnimatable2D) this._armorCarriedSymbol);
    this._meshGUITexts.Group.Add((IAnimatable2D) this._armorCarriedText);
    this._meshGUITexts.Group.Add((IAnimatable2D) this._defenseBonusValue);
    this._meshGUITexts.Group.Add((IAnimatable2D) this._defenseBonusSymbol);
    this._meshGUITexts.Group.Add((IAnimatable2D) this._defenseBonusText);
  }

  public int ArmorCarried
  {
    set => this._armorCarriedValue.Text = value.ToString();
  }

  public int DefenseBonus
  {
    set => this._defenseBonusValue.Text = value.ToString();
  }

  public bool Enabled
  {
    set => this._meshGUITexts.IsEnabled = value;
  }

  public void Update()
  {
    float num = (float) (Screen.height * 20 / 660 - 20);
    this._armorCarriedValue.Position = new Vector2((float) (Screen.width - 625) - num, 327f);
    this._armorCarriedValue.Scale = Vector2.one * 0.4f;
    this._armorCarriedSymbol.Position = new Vector2((float) (Screen.width - 605), 331.2f);
    this._armorCarriedSymbol.Scale = Vector2.one * 0.25f;
    this._armorCarriedText.Position = new Vector2((float) (Screen.width - 602), 352f);
    this._armorCarriedText.Scale = Vector2.one * 0.25f;
    this._defenseBonusValue.Position = new Vector2((float) (Screen.width - 620) - num, 417f);
    this._defenseBonusValue.Scale = Vector2.one * 0.4f;
    this._defenseBonusSymbol.Position = new Vector2((float) (Screen.width - 602), 421f);
    this._defenseBonusSymbol.Scale = Vector2.one * 0.3f;
    this._defenseBonusText.Position = new Vector2((float) (Screen.width - 602), 442f);
    this._defenseBonusText.Scale = Vector2.one * 0.25f;
  }
}
