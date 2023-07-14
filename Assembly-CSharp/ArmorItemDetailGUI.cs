// Decompiled with JetBrains decompiler
// Type: ArmorItemDetailGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ArmorItemDetailGUI : IBaseItemDetailGUI
{
  private GearItem _item;
  private Texture2D _armorPointsIcon;

  public ArmorItemDetailGUI(GearItem item, Texture2D armorPointsIcon)
  {
    this._item = item;
    this._armorPointsIcon = armorPointsIcon;
  }

  public void Draw()
  {
    float num = (float) this._item.Configuration.ArmorAbsorptionPercent / 100f;
    GUI.DrawTexture(new Rect(48f, 89f, 32f, 32f), (Texture) this._armorPointsIcon);
    GUI.contentColor = Color.black;
    GUI.Label(new Rect(48f, 89f, 32f, 32f), this._item.Configuration.ArmorPoints.ToString(), BlueStonez.label_interparkbold_16pt);
    GUI.contentColor = Color.white;
    GUI.Label(new Rect(80f, 89f, 32f, 32f), "AP", BlueStonez.label_interparkbold_18pt_left);
    GUITools.ProgressBar(new Rect(120f, 95f, 160f, 12f), LocalizedStrings.Absorption, num, ColorScheme.ProgressBar, 64, CmunePrint.Percent(num));
  }
}
