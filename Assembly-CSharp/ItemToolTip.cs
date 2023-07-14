// Decompiled with JetBrains decompiler
// Type: ItemToolTip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ItemToolTip : AutoMonoBehaviour<ItemToolTip>
{
  private const int TextWidth = 80;
  private readonly Vector2 Size = new Vector2(260f, 240f);
  private ItemToolTip.FloatPropertyBar _ammo = new ItemToolTip.FloatPropertyBar(LocalizedStrings.Ammo);
  private ItemToolTip.FloatPropertyBar _damage = new ItemToolTip.FloatPropertyBar(LocalizedStrings.Damage);
  private ItemToolTip.FloatPropertyBar _fireRate = new ItemToolTip.FloatPropertyBar(LocalizedStrings.RateOfFire);
  private ItemToolTip.FloatPropertyBar _accuracy = new ItemToolTip.FloatPropertyBar(LocalizedStrings.Accuracy);
  private ItemToolTip.FloatPropertyBar _velocity = new ItemToolTip.FloatPropertyBar(LocalizedStrings.Velocity);
  private ItemToolTip.FloatPropertyBar _damageRadius = new ItemToolTip.FloatPropertyBar(LocalizedStrings.Radius);
  private ItemToolTip.FloatPropertyBar _defenseBonus = new ItemToolTip.FloatPropertyBar(LocalizedStrings.DefenseBonus);
  private ItemToolTip.FloatPropertyBar _armorCarried = new ItemToolTip.FloatPropertyBar(LocalizedStrings.ArmorCarried);
  private Rect _finalRect = new Rect(0.0f, 0.0f, 260f, 230f);
  private Rect _rect = new Rect(0.0f, 0.0f, 260f, 230f);
  private Texture2D _icon;
  private string _name;
  private int _level;
  private int _daysLeft;
  private int _criticalHit;
  private string _description;
  private IUnityItem _item;
  private Rect _cacheRect;
  private float _alpha;
  private BuyingDurationType _duration;
  private int _startAmmo;
  private Action OnDrawItemDetails = (Action) (() => { });
  private Action OnDrawTip = (Action) (() => { });

  public bool IsEnabled { get; private set; }

  private float Alpha => Mathf.Clamp01(this._alpha - Time.time);

  private void OnGUI()
  {
    this._rect = this._rect.Lerp(this._finalRect, Time.deltaTime * 5f);
    if (!this.IsEnabled || PanelManager.IsAnyPanelOpen)
      return;
    GUI.color = new Color(1f, 1f, 1f, this.Alpha);
    GUI.BeginGroup(this._rect, BlueStonez.box_grey_outlined);
    if ((bool) (UnityEngine.Object) this._icon)
      GUI.DrawTexture(new Rect(20f, 10f, 48f, 48f), (Texture) this._icon);
    GUI.Label(new Rect(75f, 15f, 200f, 30f), this._name, BlueStonez.label_interparkbold_13pt_left);
    GUI.Label(new Rect(20f, 70f, 220f, 50f), this._description, BlueStonez.label_interparkmed_11pt_left);
    if (this._duration != BuyingDurationType.None)
      GUI.Label(new Rect(75f, 40f, 200f, 20f), new GUIContent(ShopUtils.PrintDuration(this._duration), (Texture) ShopIcons.ItemexpirationIcon), BlueStonez.label_interparkbold_11pt_left);
    else if (this._daysLeft == 0)
      GUI.Label(new Rect(75f, 40f, 200f, 20f), new GUIContent(ShopUtils.PrintDuration(BuyingDurationType.Permanent), (Texture) ShopIcons.ItemexpirationIcon), BlueStonez.label_interparkmed_11pt_left);
    else if (this._daysLeft > 0)
      GUI.Label(new Rect(75f, 40f, 200f, 20f), new GUIContent(string.Format(LocalizedStrings.NDaysLeft, (object) this._daysLeft), (Texture) ShopIcons.ItemexpirationIcon), BlueStonez.label_interparkbold_11pt_left);
    this.OnDrawItemDetails();
    if (this._level > 1)
      GUI.Label(new Rect(20f, 200f, 210f, 20f), LocalizedStrings.LevelRequired + (object) this._level, BlueStonez.label_interparkbold_11pt_left);
    if (this._criticalHit > 0)
      GUI.Label(new Rect(20f, 218f, 210f, 20f), LocalizedStrings.CriticalHitBonus + (object) this._criticalHit + "%", BlueStonez.label_interparkmed_11pt_left);
    GUI.EndGroup();
    this.OnDrawTip();
    GUI.color = Color.white;
    if ((double) this._alpha - (double) Time.time >= 0.0)
      return;
    this.IsEnabled = false;
  }

  public void SetItem(
    IUnityItem item,
    Rect bounds,
    PopupViewSide side,
    int daysLeft = -1,
    BuyingDurationType duration = BuyingDurationType.None)
  {
    if (Event.current.type != UnityEngine.EventType.Repaint || item == null || Singleton<ItemManager>.Instance.IsDefaultGearItem(item.ItemView.PrefabName))
      return;
    bool flag = (double) this._alpha < (double) Time.time + 0.10000000149011612;
    this._alpha = Mathf.Lerp(this._alpha, Time.time + 1.1f, Time.deltaTime * 12f);
    if (this._item == item && !(this._cacheRect != bounds) && this.IsEnabled)
      return;
    this._cacheRect = bounds;
    bounds = GUITools.ToGlobal(bounds);
    this.IsEnabled = true;
    this._item = item;
    this._icon = item.Icon;
    this._name = item.Name;
    this._level = item.ItemView == null ? 0 : item.ItemView.LevelLock;
    this._description = item.ItemView == null ? string.Empty : item.ItemView.Description;
    this._daysLeft = daysLeft;
    this._criticalHit = 0;
    this._duration = duration;
    switch (side)
    {
      case PopupViewSide.Left:
        float tipPosition1 = (float) ((double) bounds.y - 10.0 + (double) bounds.height * 0.5);
        Rect rect1 = new Rect((float) ((double) bounds.x - (double) this.Size.x - 9.0), bounds.y - this.Size.y * 0.5f, this.Size.x, this.Size.y);
        Rect rect2 = new Rect(rect1.xMax - 1f, tipPosition1, 12f, 21f);
        if ((double) rect1.y <= (double) GlobalUIRibbon.Instance.Height())
          rect1.y += (float) GlobalUIRibbon.Instance.Height() - rect1.y;
        if ((double) rect1.yMax >= (double) Screen.height)
          rect1.y -= rect1.yMax - (float) Screen.height;
        if ((double) rect2.y < (double) this._finalRect.y || (double) rect2.yMax > (double) this._finalRect.yMax || (double) this._finalRect.x != (double) rect1.x)
        {
          this._finalRect = rect1;
          if (flag)
            this._rect = rect1;
        }
        this.OnDrawTip = (Action) (() => GUI.DrawTexture(new Rect(this._rect.xMax - 1f, tipPosition1, 12f, 21f), (Texture) ConsumableHudTextures.TooltipRight));
        break;
      case PopupViewSide.Top:
        float tipPosition2 = (float) ((double) bounds.x - 10.0 + (double) bounds.width * 0.5);
        Rect rect3 = new Rect(bounds.x + (float) (((double) bounds.width - (double) this.Size.x) * 0.5), (float) ((double) bounds.y - (double) this.Size.y - 9.0), this.Size.x, this.Size.y);
        Rect rect4 = new Rect(tipPosition2, rect3.yMax - 1f, 21f, 12f);
        if ((double) rect3.xMin <= 10.0)
          rect3.x = 10f;
        if ((double) rect3.xMax >= (double) (Screen.width - 10))
          rect3.x -= (float) ((double) rect3.xMax - (double) Screen.width + 10.0);
        if ((double) rect4.x < (double) this._finalRect.x || (double) rect4.xMax > (double) this._finalRect.xMax || (double) this._finalRect.y != (double) rect3.y)
        {
          this._finalRect = rect3;
          if (flag)
            this._rect = rect3;
        }
        this.OnDrawTip = (Action) (() => GUI.DrawTexture(new Rect(tipPosition2, this._rect.yMax - 1f, 21f, 12f), (Texture) ConsumableHudTextures.TooltipDown));
        break;
    }
    switch (item.ItemClass)
    {
      case UberstrikeItemClass.WeaponMelee:
        this.OnDrawItemDetails = new Action(this.DrawMeleeWeapon);
        if (!(item.ItemView is UberStrikeItemWeaponView itemView1))
          break;
        this._damage.Value = WeaponConfigurationHelper.GetDamage(itemView1);
        this._damage.Max = WeaponConfigurationHelper.MaxDamage;
        this._fireRate.Value = WeaponConfigurationHelper.GetRateOfFire(itemView1);
        this._fireRate.Max = WeaponConfigurationHelper.MaxRateOfFire;
        break;
      case UberstrikeItemClass.WeaponHandgun:
      case UberstrikeItemClass.WeaponMachinegun:
      case UberstrikeItemClass.WeaponShotgun:
      case UberstrikeItemClass.WeaponSniperRifle:
        this.OnDrawItemDetails = new Action(this.DrawInstantHitWeapon);
        if (item.ItemView is WeaponItemConfiguration itemView2)
        {
          this._startAmmo = itemView2.StartAmmo;
          this._ammo.Value = WeaponConfigurationHelper.GetAmmo((UberStrikeItemWeaponView) itemView2);
          this._ammo.Max = WeaponConfigurationHelper.MaxAmmo;
          this._damage.Value = WeaponConfigurationHelper.GetDamage((UberStrikeItemWeaponView) itemView2);
          this._damage.Max = WeaponConfigurationHelper.MaxDamage;
          this._fireRate.Value = WeaponConfigurationHelper.GetRateOfFire((UberStrikeItemWeaponView) itemView2);
          this._fireRate.Max = WeaponConfigurationHelper.MaxRateOfFire;
          this._accuracy.Value = WeaponConfigurationHelper.MaxAccuracySpread - WeaponConfigurationHelper.GetAccuracySpread((UberStrikeItemWeaponView) itemView2);
          this._accuracy.Max = WeaponConfigurationHelper.MaxAccuracySpread;
          this._criticalHit = itemView2.CriticalStrikeBonus;
        }
        if (this._criticalHit != 0 || item.ItemClass != UberstrikeItemClass.WeaponHandgun && item.ItemClass != UberstrikeItemClass.WeaponSniperRifle)
          break;
        this._criticalHit = 50;
        break;
      case UberstrikeItemClass.WeaponCannon:
      case UberstrikeItemClass.WeaponSplattergun:
      case UberstrikeItemClass.WeaponLauncher:
        this.OnDrawItemDetails = new Action(this.DrawProjectileWeapon);
        if (!(item.ItemView is UberStrikeItemWeaponView itemView3))
          break;
        this._startAmmo = itemView3.StartAmmo;
        this._ammo.Value = WeaponConfigurationHelper.GetAmmo(itemView3);
        this._ammo.Max = WeaponConfigurationHelper.MaxAmmo;
        this._damage.Value = WeaponConfigurationHelper.GetDamage(itemView3);
        this._damage.Max = WeaponConfigurationHelper.MaxDamage;
        this._fireRate.Value = WeaponConfigurationHelper.GetRateOfFire(itemView3);
        this._fireRate.Max = WeaponConfigurationHelper.MaxRateOfFire;
        this._velocity.Value = WeaponConfigurationHelper.GetProjectileSpeed(itemView3);
        this._velocity.Max = WeaponConfigurationHelper.MaxProjectileSpeed;
        this._damageRadius.Value = WeaponConfigurationHelper.GetSplashRadius(itemView3);
        this._damageRadius.Max = WeaponConfigurationHelper.MaxSplashRadius;
        break;
      case UberstrikeItemClass.GearBoots:
      case UberstrikeItemClass.GearHead:
      case UberstrikeItemClass.GearFace:
      case UberstrikeItemClass.GearUpperBody:
      case UberstrikeItemClass.GearLowerBody:
      case UberstrikeItemClass.GearGloves:
      case UberstrikeItemClass.GearHolo:
        this.OnDrawItemDetails = new Action(this.DrawGear);
        this._defenseBonus.Value = (float) ((UberStrikeItemGearView) item.ItemView).ArmorAbsorptionPercent;
        this._defenseBonus.Max = 25f;
        this._armorCarried.Value = (float) ((UberStrikeItemGearView) item.ItemView).ArmorPoints;
        this._armorCarried.Max = 200f;
        break;
      case UberstrikeItemClass.QuickUseGeneral:
      case UberstrikeItemClass.QuickUseGrenade:
      case UberstrikeItemClass.QuickUseMine:
        this.OnDrawItemDetails = new Action(this.DrawQuickItem);
        break;
      default:
        this.OnDrawItemDetails = (Action) (() => { });
        break;
    }
  }

  public void ComparisonOverlay(Rect position, float value, float otherValue)
  {
    float num1 = (float) ((double) position.width - 80.0 - 50.0);
    float num2 = (num1 - 4f) * Mathf.Clamp01(value);
    float width = (num1 - 4f) * Mathf.Clamp01(Mathf.Abs(value - otherValue));
    GUI.BeginGroup(position);
    if ((double) value < (double) otherValue)
    {
      GUI.color = Color.green.SetAlpha(this.Alpha * 0.9f);
      GUI.Label(new Rect(82f + num2, 3f, width, 8f), string.Empty, BlueStonez.progressbar_thumb);
    }
    else
    {
      GUI.color = Color.red.SetAlpha(this.Alpha * 0.9f);
      GUI.Label(new Rect(82f + num2 - width, 3f, width, 8f), string.Empty, BlueStonez.progressbar_thumb);
    }
    GUI.color = new Color(1f, 1f, 1f, this.Alpha);
    GUI.EndGroup();
  }

  public void ProgressBar(
    Rect position,
    string text,
    float percentage,
    Color barColor,
    string value)
  {
    float width = (float) ((double) position.width - 80.0 - 50.0);
    GUI.BeginGroup(position);
    GUI.Label(new Rect(0.0f, 0.0f, 80f, 14f), text, BlueStonez.label_interparkbold_11pt_left);
    GUI.Label(new Rect(80f, 1f, width, 12f), GUIContent.none, BlueStonez.progressbar_background);
    GUI.color = barColor.SetAlpha(this.Alpha);
    GUI.Label(new Rect(82f, 3f, (width - 4f) * Mathf.Clamp01(percentage), 8f), string.Empty, BlueStonez.progressbar_thumb);
    GUI.color = new Color(1f, 1f, 1f, this.Alpha);
    if (!string.IsNullOrEmpty(value))
      GUI.Label(new Rect((float) (80.0 + (double) width + 10.0), 0.0f, 40f, 14f), value, BlueStonez.label_interparkmed_10pt_left);
    GUI.EndGroup();
  }

  private void DrawGear()
  {
    this.ProgressBar(new Rect(20f, 120f, 200f, 12f), this._defenseBonus.Title, this._defenseBonus.Percent, ColorScheme.ProgressBar, "+" + CmunePrint.Percent(this._defenseBonus.Value / 100f));
    this.ProgressBar(new Rect(20f, 135f, 200f, 12f), this._armorCarried.Title, this._armorCarried.Percent, ColorScheme.ProgressBar, this._armorCarried.Value.ToString("F0") + "AP");
  }

  private void DrawProjectileWeapon()
  {
    bool flag = Singleton<DragAndDrop>.Instance.IsDragging && ShopUtils.IsProjectileWeapon(Singleton<DragAndDrop>.Instance.DraggedItem.Item) && Singleton<DragAndDrop>.Instance.DraggedItem.Item.ItemClass == this._item.ItemClass;
    this.ProgressBar(new Rect(20f, 120f, 200f, 12f), this._damage.Title, this._damage.Percent, ColorScheme.ProgressBar, this._damage.Value.ToString("F0") + "HP");
    this.ProgressBar(new Rect(20f, 135f, 200f, 12f), this._fireRate.Title, 1f - this._fireRate.Percent, ColorScheme.ProgressBar, (1f / this._fireRate.Value).ToString("F1") + "/s");
    this.ProgressBar(new Rect(20f, 150f, 200f, 12f), this._velocity.Title, this._velocity.Percent, ColorScheme.ProgressBar, this._velocity.Value.ToString("F0") + "m/s");
    this.ProgressBar(new Rect(20f, 165f, 200f, 12f), this._damageRadius.Title, this._damageRadius.Percent, ColorScheme.ProgressBar, this._damageRadius.Value.ToString("F1") + "m");
    this.ProgressBar(new Rect(20f, 180f, 200f, 12f), this._ammo.Title, this._ammo.Percent, ColorScheme.ProgressBar, this._startAmmo.ToString() + "/" + this._ammo.Value.ToString("F0"));
    if (!flag)
      return;
    UberStrikeItemWeaponView itemView = Singleton<DragAndDrop>.Instance.DraggedItem.Item.ItemView as UberStrikeItemWeaponView;
    this.ComparisonOverlay(new Rect(20f, 120f, 200f, 12f), this._damage.Percent, WeaponConfigurationHelper.GetDamageNormalized(itemView));
    this.ComparisonOverlay(new Rect(20f, 135f, 200f, 12f), 1f - this._fireRate.Percent, 1f - WeaponConfigurationHelper.GetRateOfFireNormalized(itemView));
    this.ComparisonOverlay(new Rect(20f, 150f, 200f, 12f), this._velocity.Percent, WeaponConfigurationHelper.GetProjectileSpeedNormalized(itemView));
    this.ComparisonOverlay(new Rect(20f, 165f, 200f, 12f), this._damageRadius.Percent, WeaponConfigurationHelper.GetSplashRadiusNormalized(itemView));
  }

  private void DrawInstantHitWeapon()
  {
    bool flag = Singleton<DragAndDrop>.Instance.IsDragging && ShopUtils.IsInstantHitWeapon(Singleton<DragAndDrop>.Instance.DraggedItem.Item) && Singleton<DragAndDrop>.Instance.DraggedItem.Item.ItemClass == this._item.ItemClass;
    this.ProgressBar(new Rect(20f, 120f, 200f, 12f), this._damage.Title, this._damage.Percent, ColorScheme.ProgressBar, this._damage.Value.ToString("F0") + "HP");
    this.ProgressBar(new Rect(20f, 135f, 200f, 12f), this._fireRate.Title, 1f - this._fireRate.Percent, ColorScheme.ProgressBar, (1f / this._fireRate.Value).ToString("F1") + "/s");
    this.ProgressBar(new Rect(20f, 150f, 200f, 12f), this._accuracy.Title, this._accuracy.Percent, ColorScheme.ProgressBar, CmunePrint.Percent(this._accuracy.Value / this._accuracy.Max));
    this.ProgressBar(new Rect(20f, 165f, 200f, 12f), this._ammo.Title, this._ammo.Percent, ColorScheme.ProgressBar, this._startAmmo.ToString() + "/" + this._ammo.Value.ToString("F0"));
    if (!flag)
      return;
    UberStrikeItemWeaponView itemView = Singleton<DragAndDrop>.Instance.DraggedItem.Item.ItemView as UberStrikeItemWeaponView;
    this.ComparisonOverlay(new Rect(20f, 120f, 200f, 12f), this._damage.Percent, WeaponConfigurationHelper.GetDamageNormalized(itemView));
    this.ComparisonOverlay(new Rect(20f, 135f, 200f, 12f), 1f - this._fireRate.Percent, 1f - WeaponConfigurationHelper.GetRateOfFireNormalized(itemView));
    this.ComparisonOverlay(new Rect(20f, 150f, 200f, 12f), this._accuracy.Percent, 1f - WeaponConfigurationHelper.GetAccuracySpreadNormalized(itemView));
  }

  private void DrawMeleeWeapon()
  {
    this.ProgressBar(new Rect(20f, 120f, 200f, 12f), this._damage.Title, this._damage.Percent, ColorScheme.ProgressBar, this._damage.Value.ToString("F0") + "HP");
    this.ProgressBar(new Rect(20f, 135f, 200f, 12f), this._fireRate.Title, 1f - this._fireRate.Percent, ColorScheme.ProgressBar, (1f / this._fireRate.Value).ToString("F1") + "/s");
    if (!Singleton<DragAndDrop>.Instance.IsDragging || !ShopUtils.IsMeleeWeapon(Singleton<DragAndDrop>.Instance.DraggedItem.Item))
      return;
    UberStrikeItemWeaponView itemView = Singleton<DragAndDrop>.Instance.DraggedItem.Item.ItemView as UberStrikeItemWeaponView;
    this.ComparisonOverlay(new Rect(20f, 120f, 200f, 12f), this._damage.Percent, WeaponConfigurationHelper.GetDamageNormalized(itemView));
    this.ComparisonOverlay(new Rect(20f, 135f, 200f, 12f), 1f - this._fireRate.Percent, 1f - WeaponConfigurationHelper.GetRateOfFireNormalized(itemView));
  }

  private void DrawQuickItem()
  {
    if (this._item == null)
      return;
    QuickItemConfiguration itemView1 = this._item.ItemView as QuickItemConfiguration;
    if (this._item.ItemView is HealthBuffConfiguration)
    {
      HealthBuffConfiguration itemView2 = this._item.ItemView as HealthBuffConfiguration;
      GUI.Label(new Rect(20f, 102f, 200f, 20f), LocalizedStrings.HealthColon + itemView2.GetHealthBonusDescription(), BlueStonez.label_interparkbold_11pt_left);
      GUI.Label(new Rect(20f, 117f, 200f, 20f), LocalizedStrings.TimeColon + (itemView2.IncreaseTimes <= 0 ? LocalizedStrings.Instant : ((float) (itemView2.IncreaseFrequency * itemView2.IncreaseTimes) / 1000f).ToString("f1") + "s"), BlueStonez.label_interparkbold_11pt_left);
    }
    else if (this._item.ItemView is AmmoBuffConfiguration)
    {
      AmmoBuffConfiguration itemView3 = this._item.ItemView as AmmoBuffConfiguration;
      GUI.Label(new Rect(20f, 102f, 200f, 20f), LocalizedStrings.AmmoColon + itemView3.GetAmmoBonusDescription(), BlueStonez.label_interparkbold_11pt_left);
      GUI.Label(new Rect(20f, 117f, 200f, 20f), LocalizedStrings.TimeColon + (itemView3.IncreaseTimes <= 0 ? LocalizedStrings.Instant : ((float) (itemView3.IncreaseFrequency * itemView3.IncreaseTimes) / 1000f).ToString("f1") + "s"), BlueStonez.label_interparkbold_11pt_left);
    }
    else if (this._item.ItemView is ArmorBuffConfiguration)
    {
      ArmorBuffConfiguration itemView4 = this._item.ItemView as ArmorBuffConfiguration;
      GUI.Label(new Rect(20f, 102f, 200f, 20f), LocalizedStrings.ArmorColon + itemView4.GetArmorBonusDescription(), BlueStonez.label_interparkbold_11pt_left);
      GUI.Label(new Rect(20f, 117f, 200f, 20f), LocalizedStrings.TimeColon + (itemView4.IncreaseTimes <= 0 ? LocalizedStrings.Instant : ((float) (itemView4.IncreaseFrequency * itemView4.IncreaseTimes) / 1000f).ToString("f1") + "s"), BlueStonez.label_interparkbold_11pt_left);
    }
    else if (this._item.ItemView is ExplosiveGrenadeConfiguration)
    {
      ExplosiveGrenadeConfiguration itemView5 = this._item.ItemView as ExplosiveGrenadeConfiguration;
      GUI.Label(new Rect(20f, 102f, 200f, 20f), LocalizedStrings.DamageColon + (object) itemView5.Damage + "HP", BlueStonez.label_interparkbold_11pt_left);
      GUI.Label(new Rect(20f, 117f, 200f, 20f), LocalizedStrings.RadiusColon + (object) itemView5.SplashRadius + "m", BlueStonez.label_interparkbold_11pt_left);
    }
    else if (this._item.ItemView is SpringGrenadeConfiguration)
    {
      SpringGrenadeConfiguration itemView6 = this._item.ItemView as SpringGrenadeConfiguration;
      GUI.Label(new Rect(20f, 102f, 200f, 20f), LocalizedStrings.ForceColon + (object) itemView6.Force, BlueStonez.label_interparkbold_11pt_left);
      GUI.Label(new Rect(20f, 117f, 200f, 20f), LocalizedStrings.LifetimeColon + (object) itemView6.LifeTime + "s", BlueStonez.label_interparkbold_11pt_left);
    }
    GUI.Label(new Rect(20f, 132f, 200f, 20f), LocalizedStrings.WarmupColon + (itemView1.WarmUpTime <= 0 ? LocalizedStrings.Instant : ((float) itemView1.WarmUpTime / 1000f).ToString("f1") + "s"), BlueStonez.label_interparkbold_11pt_left);
    GUI.Label(new Rect(20f, 147f, 200f, 20f), LocalizedStrings.CooldownColon + ((float) itemView1.CoolDownTime / 1000f).ToString("f1") + "s", BlueStonez.label_interparkbold_11pt_left);
    GUI.Label(new Rect(20f, 162f, 200f, 20f), LocalizedStrings.UsesPerLifeColon + (itemView1.UsesPerLife <= 0 ? LocalizedStrings.Unlimited : itemView1.UsesPerLife.ToString()), BlueStonez.label_interparkbold_11pt_left);
    GUI.Label(new Rect(20f, 177f, 200f, 20f), LocalizedStrings.UsesPerGameColon + (itemView1.UsesPerGame <= 0 ? LocalizedStrings.Unlimited : itemView1.UsesPerGame.ToString()), BlueStonez.label_interparkbold_11pt_left);
  }

  private class FloatPropertyBar
  {
    private float _value;
    private float _lastValue;
    private float _max = 1f;
    private float _time;

    public FloatPropertyBar(string title) => this.Title = title;

    public string Title { get; private set; }

    public float SmoothValue => Mathf.Lerp(this._lastValue, this.Value, (float) (((double) Time.time - (double) this._time) * 5.0));

    public float Value
    {
      get => this._value;
      set
      {
        this._lastValue = this._value;
        this._time = Time.time;
        this._value = value;
      }
    }

    public float Percent => this.SmoothValue / this.Max;

    public float Max
    {
      get => this._max;
      set => this._max = Mathf.Max(value, 1f);
    }
  }
}
