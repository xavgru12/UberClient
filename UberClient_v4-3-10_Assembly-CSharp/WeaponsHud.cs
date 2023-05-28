// Decompiled with JetBrains decompiler
// Type: WeaponsHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class WeaponsHud : Singleton<WeaponsHud>
{
  private WeaponsHud()
  {
    this.Weapons = new WeaponSelectorHud();
    this.QuickItems = new QuickItemGroupHud();
    this.SetEnabled(false);
  }

  public QuickItemGroupHud QuickItems { get; private set; }

  public WeaponSelectorHud Weapons { get; private set; }

  public void SetEnabled(bool enabled)
  {
    this.Weapons.Enabled = enabled;
    this.QuickItems.Enabled = enabled;
  }

  public void Draw()
  {
    this.Weapons.Draw();
    this.QuickItems.Draw();
  }

  public void Update() => this.Weapons.Update();

  public void SetActiveLoadout(LoadoutSlotType loadoutSlotType)
  {
    switch (loadoutSlotType)
    {
      case LoadoutSlotType.WeaponMelee:
      case LoadoutSlotType.WeaponPrimary:
      case LoadoutSlotType.WeaponSecondary:
      case LoadoutSlotType.WeaponTertiary:
      case LoadoutSlotType.WeaponPickup:
        this.SetActiveWeaponLoadout(loadoutSlotType);
        break;
      default:
        Debug.LogError((object) "You passed in an invalid LoadoutSlotType!");
        break;
    }
  }

  public void ResetActiveWeapon() => this.SetActiveLoadout(LoadoutSlotType.WeaponPrimary);

  public void SetQuickItemCurrentAmount(int slot, int amount)
  {
    QuickItemHud loadoutQuickItemHud = this.QuickItems.GetLoadoutQuickItemHud(slot);
    if (loadoutQuickItemHud == null)
      return;
    loadoutQuickItemHud.Amount = amount;
  }

  public void SetQuickItemCooldown(int slot, float cooldown)
  {
    QuickItemHud loadoutQuickItemHud = this.QuickItems.GetLoadoutQuickItemHud(slot);
    if (loadoutQuickItemHud == null)
      return;
    loadoutQuickItemHud.Cooldown = cooldown;
  }

  public void SetQuickItemCooldownMax(int slot, float cooldownMax)
  {
    QuickItemHud loadoutQuickItemHud = this.QuickItems.GetLoadoutQuickItemHud(slot);
    if (loadoutQuickItemHud == null)
      return;
    loadoutQuickItemHud.CooldownMax = cooldownMax;
  }

  public void SetQuickItemRecharging(int slot, float recharging)
  {
    QuickItemHud loadoutQuickItemHud = this.QuickItems.GetLoadoutQuickItemHud(slot);
    if (loadoutQuickItemHud == null)
      return;
    loadoutQuickItemHud.Recharging = recharging;
  }

  public void SetQuickItemRechargingMax(int slot, float rechargingMax)
  {
    QuickItemHud loadoutQuickItemHud = this.QuickItems.GetLoadoutQuickItemHud(slot);
    if (loadoutQuickItemHud == null)
      return;
    loadoutQuickItemHud.RechargingMax = rechargingMax;
  }

  private void SetActiveWeaponLoadout(LoadoutSlotType loadoutSlotType)
  {
    if (!HudAssets.Exists)
      return;
    WeaponItem loadoutWeapon = this.Weapons.GetLoadoutWeapon(loadoutSlotType);
    if (!((Object) loadoutWeapon != (Object) null))
      return;
    this.Weapons.SetActiveWeaponLoadout(loadoutSlotType);
    Singleton<ReticleHud>.Instance.ConfigureReticle(loadoutWeapon.Configuration);
  }
}
