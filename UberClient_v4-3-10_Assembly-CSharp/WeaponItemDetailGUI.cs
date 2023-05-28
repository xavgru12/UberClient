// Decompiled with JetBrains decompiler
// Type: WeaponItemDetailGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public class WeaponItemDetailGUI : IBaseItemDetailGUI
{
  private WeaponItem _item;

  public WeaponItemDetailGUI(WeaponItem item) => this._item = item;

  public void Draw()
  {
    GUITools.ProgressBar(new Rect(14f, 95f, 165f, 12f), LocalizedStrings.Damage, WeaponConfigurationHelper.GetDamageNormalized((UberStrikeItemWeaponView) this._item.Configuration), ColorScheme.ProgressBar, 64, WeaponConfigurationHelper.GetDamage((UberStrikeItemWeaponView) this._item.Configuration).ToString("F0"));
    GUITools.ProgressBar(new Rect(14f, 111f, 165f, 12f), LocalizedStrings.RateOfFire, WeaponConfigurationHelper.GetRateOfFireNormalized((UberStrikeItemWeaponView) this._item.Configuration), ColorScheme.ProgressBar, 64, WeaponConfigurationHelper.GetRateOfFire((UberStrikeItemWeaponView) this._item.Configuration).ToString("F0"));
    if (this._item.ItemClass == UberstrikeItemClass.WeaponCannon || this._item.ItemClass == UberstrikeItemClass.WeaponLauncher || this._item.ItemClass == UberstrikeItemClass.WeaponSplattergun)
    {
      GUITools.ProgressBar(new Rect(175f, 95f, 165f, 12f), LocalizedStrings.Velocity, WeaponConfigurationHelper.GetProjectileSpeedNormalized((UberStrikeItemWeaponView) this._item.Configuration), ColorScheme.ProgressBar, 64, WeaponConfigurationHelper.GetProjectileSpeed((UberStrikeItemWeaponView) this._item.Configuration).ToString("F0"));
      GUITools.ProgressBar(new Rect(175f, 111f, 165f, 12f), LocalizedStrings.Impact, WeaponConfigurationHelper.GetSplashRadiusNormalized((UberStrikeItemWeaponView) this._item.Configuration), ColorScheme.ProgressBar, 64, WeaponConfigurationHelper.GetSplashRadius((UberStrikeItemWeaponView) this._item.Configuration).ToString("F1"));
    }
    else if (this._item.ItemClass == UberstrikeItemClass.WeaponMelee)
    {
      bool enabled = GUI.enabled;
      GUI.enabled = false;
      GUITools.ProgressBar(new Rect(175f, 95f, 165f, 12f), LocalizedStrings.Accuracy, 0.0f, ColorScheme.ProgressBar, 64, string.Empty);
      GUITools.ProgressBar(new Rect(175f, 111f, 165f, 12f), LocalizedStrings.Recoil, 0.0f, ColorScheme.ProgressBar, 64, string.Empty);
      GUI.enabled = enabled;
    }
    else
    {
      GUITools.ProgressBar(new Rect(175f, 95f, 165f, 12f), LocalizedStrings.Accuracy, WeaponConfigurationHelper.GetAccuracySpreadNormalized((UberStrikeItemWeaponView) this._item.Configuration), ColorScheme.ProgressBar, 64, (WeaponConfigurationHelper.GetAccuracySpread((UberStrikeItemWeaponView) this._item.Configuration) * 100f).ToString("F0") + "%");
      GUITools.ProgressBar(new Rect(175f, 111f, 165f, 12f), LocalizedStrings.Recoil, WeaponConfigurationHelper.GetRecoilKickbackNormalized((UberStrikeItemWeaponView) this._item.Configuration), ColorScheme.ProgressBar, 64, WeaponConfigurationHelper.GetRecoilKickback((UberStrikeItemWeaponView) this._item.Configuration).ToString());
    }
  }
}
