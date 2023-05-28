// Decompiled with JetBrains decompiler
// Type: WeaponDetailGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class WeaponDetailGUI
{
  private IUnityItem _selectedItem;
  private Texture2D _curBadge;
  private RecommendType _curRecomType;

  public void SetWeaponItem(IUnityItem item, RecommendType type)
  {
    this._selectedItem = item;
    this._curRecomType = type;
    this._curBadge = UberstrikeIconsHelper.GetRecommendBadgeTexture(type);
  }

  public void Draw(Rect rect)
  {
    GUI.BeginGroup(new Rect(rect.x, rect.y, rect.width, rect.height - 2f), GUIContent.none, StormFront.GrayPanelBox);
    this.DrawWeaponBadge(new Rect((float) (((double) rect.width - 180.0) / 2.0), 15f, 180f, 125f));
    this.DrawWeaponCaption(new Rect(0.0f, 2f, rect.width, 20f));
    this.DrawWeaponIcons(new Rect(25f, 140f, rect.width - 50f, 30f));
    this.DrawWeaponPropertyBars(new Rect(-25f, 175f, rect.width, rect.height - 60f));
    GUI.EndGroup();
  }

  private void DrawWeaponCaption(Rect rect)
  {
    GUI.color = ColorConverter.HexToColor("ffc41b");
    GUI.Label(rect, ShopUtils.GetRecommendationString(this._curRecomType), BlueStonez.label_interparkbold_16pt);
    GUI.color = Color.white;
  }

  private void DrawWeaponBadge(Rect rect)
  {
    if (!((Object) this._curBadge != (Object) null))
      return;
    GUI.DrawTexture(rect, (Texture) this._curBadge);
  }

  private void DrawWeaponIcons(Rect rect)
  {
    GUI.BeginGroup(rect);
    if (this._selectedItem is WeaponItem)
    {
      WeaponItem selectedItem = this._selectedItem as WeaponItem;
      this.DrawWeaponIcon(new Rect(0.0f, 0.0f, 50f, rect.height), (Texture) UberstrikeIconsHelper.GetIconForItemClass(selectedItem.ItemClass));
      GUI.Label(new Rect(49f, 0.0f, 2f, rect.height), GUIContent.none, BlueStonez.vertical_line_grey95);
      GUI.DrawTexture(new Rect(60f, (float) ((double) rect.height / 2.0 - 16.0), 32f, 32f), (Texture) DrawCombatRangeIconUtil.GetIconByRange(selectedItem.Configuration.CombatRange));
      GUI.Label(new Rect(99f, 0.0f, 2f, rect.height), GUIContent.none, BlueStonez.vertical_line_grey95);
      this.DrawWeaponIcon(new Rect(100f, 0.0f, 50f, rect.height), (Texture) ShopIcons.BlankItemFrame);
      if (selectedItem.ItemView != null)
        GUI.Label(new Rect(98f, 12f, 50f, 16f), selectedItem.ItemView.LevelLock.ToString(), BlueStonez.label_interparkbold_13pt);
    }
    else if (this._selectedItem is GearItem)
    {
      GearItem selectedItem = this._selectedItem as GearItem;
      this.DrawWeaponIcon(new Rect(25f, 0.0f, 50f, rect.height), (Texture) UberstrikeIconsHelper.GetIconForItemClass(selectedItem.ItemClass));
      GUI.Label(new Rect(74f, 0.0f, 2f, rect.height), GUIContent.none, BlueStonez.vertical_line_grey95);
      this.DrawWeaponIcon(new Rect(75f, 0.0f, 50f, rect.height), (Texture) ShopIcons.BlankItemFrame);
      if (selectedItem.ItemView != null)
        GUI.Label(new Rect(73f, 12f, 50f, 16f), selectedItem.ItemView.LevelLock.ToString(), BlueStonez.label_interparkbold_13pt);
    }
    GUI.EndGroup();
  }

  private void DrawWeaponIcon(Rect rect, Texture iconTexture) => GUI.Label(new Rect((float) ((double) rect.x + (double) rect.width / 2.0 - 13.0), (float) ((double) rect.y + (double) rect.height / 2.0 - 14.0), 35f, rect.height), iconTexture);

  private void DrawWeaponPropertyBars(Rect rect)
  {
    int barWidth = 60;
    float height = 12f;
    float num1 = 2f;
    GUI.BeginGroup(rect);
    if (this._selectedItem is WeaponItem)
    {
      WeaponItem selectedItem = this._selectedItem as WeaponItem;
      GUITools.ProgressBar(new Rect(0.0f, (float) (((double) height + (double) num1) * 2.0), rect.width, height), LocalizedStrings.Damage, WeaponConfigurationHelper.GetDamageNormalized((UberStrikeItemWeaponView) selectedItem.Configuration), ColorScheme.ProgressBar, barWidth, WeaponConfigurationHelper.GetDamage((UberStrikeItemWeaponView) selectedItem.Configuration).ToString("F0"));
      GUITools.ProgressBar(new Rect(0.0f, (float) (((double) height + (double) num1) * 3.0), rect.width, height), LocalizedStrings.RateOfFire, WeaponConfigurationHelper.GetRateOfFireNormalized((UberStrikeItemWeaponView) selectedItem.Configuration), ColorScheme.ProgressBar, barWidth, WeaponConfigurationHelper.GetRateOfFire((UberStrikeItemWeaponView) selectedItem.Configuration).ToString("F0"));
      if (selectedItem.ItemClass == UberstrikeItemClass.WeaponCannon || selectedItem.ItemClass == UberstrikeItemClass.WeaponLauncher || selectedItem.ItemClass == UberstrikeItemClass.WeaponSplattergun)
      {
        GUITools.ProgressBar(new Rect(0.0f, 0.0f, rect.width, height), LocalizedStrings.Velocity, WeaponConfigurationHelper.GetProjectileSpeedNormalized((UberStrikeItemWeaponView) selectedItem.Configuration), ColorScheme.ProgressBar, barWidth, WeaponConfigurationHelper.GetProjectileSpeed((UberStrikeItemWeaponView) selectedItem.Configuration).ToString("F0"));
        GUITools.ProgressBar(new Rect(0.0f, height + num1, rect.width, height), LocalizedStrings.Impact, WeaponConfigurationHelper.GetSplashRadiusNormalized((UberStrikeItemWeaponView) selectedItem.Configuration), ColorScheme.ProgressBar, barWidth, WeaponConfigurationHelper.GetSplashRadius((UberStrikeItemWeaponView) selectedItem.Configuration).ToString("F1"));
      }
      else if (selectedItem.ItemClass == UberstrikeItemClass.WeaponMelee)
      {
        bool enabled = GUI.enabled;
        GUI.enabled = false;
        GUITools.ProgressBar(new Rect(0.0f, 0.0f, rect.width, height), LocalizedStrings.Accuracy, 0.0f, ColorScheme.ProgressBar, barWidth, string.Empty);
        GUITools.ProgressBar(new Rect(0.0f, height + num1, rect.width, height), LocalizedStrings.Recoil, 0.0f, ColorScheme.ProgressBar, barWidth, string.Empty);
        GUI.enabled = enabled;
      }
      else
        GUITools.ProgressBar(new Rect(0.0f, 0.0f, rect.width, height), LocalizedStrings.Accuracy, WeaponConfigurationHelper.GetAccuracySpreadNormalized((UberStrikeItemWeaponView) selectedItem.Configuration), ColorScheme.ProgressBar, barWidth, (WeaponConfigurationHelper.GetAccuracySpread((UberStrikeItemWeaponView) selectedItem.Configuration) * 100f).ToString("F0"));
    }
    else if (this._selectedItem is GearItem)
    {
      GearItem selectedItem = this._selectedItem as GearItem;
      float num2 = (float) selectedItem.Configuration.ArmorAbsorptionPercent / 100f;
      GUI.DrawTexture(new Rect(50f, 0.0f, 32f, 32f), (Texture) ShopIcons.ItemarmorpointsIcon);
      GUI.contentColor = Color.black;
      GUI.Label(new Rect(50f, 0.0f, 32f, 32f), selectedItem.Configuration.ArmorPoints.ToString(), BlueStonez.label_interparkbold_16pt);
      GUI.contentColor = Color.white;
      GUI.Label(new Rect(87f, 0.0f, 32f, 32f), "AP", BlueStonez.label_interparkbold_18pt_left);
      GUITools.ProgressBar(new Rect(0.0f, 37f, rect.width, 15f), LocalizedStrings.Absorption, num2, ColorScheme.ProgressBar, barWidth, string.Empty);
      GUI.Label(new Rect(rect.width - 25f, 37f, 25f, 15f), CmunePrint.Percent(num2), BlueStonez.label_interparkmed_10pt_left);
    }
    GUI.EndGroup();
  }

  private void OnSelectionChange(IUnityItem item) => this._curBadge = UberstrikeIconsHelper.GetAchievementBadgeTexture(AchievementType.CostEffective);
}
