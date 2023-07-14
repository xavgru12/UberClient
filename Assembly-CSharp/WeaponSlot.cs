// Decompiled with JetBrains decompiler
// Type: WeaponSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Types;
using UnityEngine;

public class WeaponSlot
{
  public WeaponSlot(
    LoadoutSlotType slot,
    WeaponItem item,
    Transform attachPoint,
    IWeaponController controller)
  {
    this.Slot = slot;
    this.Item = item;
    this.CreateWeaponLogic(item, controller);
    this.CreateWeaponInputHandler(item, (IWeaponLogic) this.Logic, this.Decorator, controller.IsLocal);
    this.ConfigureWeaponDecorator(attachPoint);
  }

  public LoadoutSlotType Slot { get; private set; }

  public BaseWeaponLogic Logic { get; private set; }

  public BaseWeaponDecorator Decorator { get; private set; }

  public WeaponItem Item { get; private set; }

  public WeaponInputHandler InputHandler { get; private set; }

  public float NextShootTime { get; set; }

  public bool HasWeapon => (UnityEngine.Object) this.Item != (UnityEngine.Object) null;

  private void CreateWeaponLogic(WeaponItem weapon, IWeaponController controller)
  {
    switch (weapon.ItemClass)
    {
      case UberstrikeItemClass.WeaponMelee:
        this.Decorator = this.InstantiateWeaponDecorator(weapon);
        this.Logic = (BaseWeaponLogic) new MeleeWeapon(weapon, this.Decorator as MeleeWeaponDecorator, controller);
        break;
      case UberstrikeItemClass.WeaponHandgun:
      case UberstrikeItemClass.WeaponSniperRifle:
        this.Decorator = this.InstantiateWeaponDecorator(weapon);
        this.Logic = (BaseWeaponLogic) new InstantHitWeapon(weapon, this.Decorator, controller);
        break;
      case UberstrikeItemClass.WeaponMachinegun:
        this.Decorator = this.InstantiateWeaponDecorator(weapon);
        if (weapon.Configuration.ProjectilesPerShot > 1)
        {
          this.Logic = (BaseWeaponLogic) new InstantMultiHitWeapon(weapon, this.Decorator, weapon.Configuration.ProjectilesPerShot, controller);
          break;
        }
        this.Logic = (BaseWeaponLogic) new InstantHitWeapon(weapon, this.Decorator, controller);
        break;
      case UberstrikeItemClass.WeaponShotgun:
        this.Decorator = this.InstantiateWeaponDecorator(weapon);
        this.Logic = (BaseWeaponLogic) new InstantMultiHitWeapon(weapon, this.Decorator, weapon.Configuration.ProjectilesPerShot, controller);
        break;
      case UberstrikeItemClass.WeaponCannon:
      case UberstrikeItemClass.WeaponSplattergun:
      case UberstrikeItemClass.WeaponLauncher:
        ProjectileWeaponDecorator projectileWeaponDecorator = this.CreateProjectileWeaponDecorator(weapon);
        this.Logic = (BaseWeaponLogic) new ProjectileWeapon(weapon, projectileWeaponDecorator, controller);
        this.Decorator = (BaseWeaponDecorator) projectileWeaponDecorator;
        break;
      default:
        throw new Exception("Failed to create weapon logic!");
    }
  }

  private ProjectileWeaponDecorator CreateProjectileWeaponDecorator(WeaponItem item)
  {
    ProjectileWeaponDecorator component = Singleton<ItemManager>.Instance.Instantiate(item.ItemId).GetComponent<ProjectileWeaponDecorator>();
    if ((bool) (UnityEngine.Object) component)
      component.SetMissileTimeOut((float) item.Configuration.MissileTimeToDetonate / 1000f);
    return component;
  }

  private BaseWeaponDecorator InstantiateWeaponDecorator(WeaponItem item) => Singleton<ItemManager>.Instance.Instantiate(item.ItemId).GetComponent<BaseWeaponDecorator>();

  private void ConfigureWeaponDecorator(Transform parent)
  {
    if ((bool) (UnityEngine.Object) this.Decorator)
    {
      this.Decorator.IsEnabled = false;
      this.Decorator.transform.parent = parent;
      this.Decorator.DefaultPosition = this.Item.Configuration.Position;
      this.Decorator.DefaultAngles = this.Item.Configuration.Rotation;
      this.Decorator.CurrentPosition = this.Item.Configuration.Position;
      this.Decorator.gameObject.name = this.Slot.ToString() + " " + (object) this.Item.ItemClass;
      this.Decorator.SetSurfaceEffect(this.Item.Configuration.ParticleEffect);
      LayerUtil.SetLayerRecursively(this.Decorator.transform, UberstrikeLayer.Weapons);
    }
    else
      Debug.LogError((object) "Failed to configure WeaponDecorator!");
  }

  private void CreateWeaponInputHandler(
    WeaponItem item,
    IWeaponLogic logic,
    BaseWeaponDecorator decorator,
    bool isLocal)
  {
    switch (item.Configuration.SecondaryAction)
    {
      case WeaponSecondaryAction.SniperRifle:
        this.InputHandler = (WeaponInputHandler) new SniperRifleInputHandler(logic, isLocal, item.Configuration.ZoomInformation, item.Configuration.HasAutomaticFire);
        break;
      case WeaponSecondaryAction.IronSight:
        this.InputHandler = (WeaponInputHandler) new IronsightInputHandler(logic, isLocal, item.Configuration.ZoomInformation, item.Configuration.HasAutomaticFire);
        break;
      case WeaponSecondaryAction.ExplosionTrigger:
        this.InputHandler = (WeaponInputHandler) new DefaultWeaponInputHandler(logic, isLocal, item.Configuration.HasAutomaticFire, (IWeaponFireHandler) new GrenadeExplosionHander());
        break;
      case WeaponSecondaryAction.Minigun:
        this.InputHandler = (WeaponInputHandler) new MinigunInputHandler(logic, isLocal, decorator as MinigunWeaponDecorator);
        break;
      default:
        this.InputHandler = (WeaponInputHandler) new DefaultWeaponInputHandler(logic, isLocal, item.Configuration.HasAutomaticFire);
        break;
    }
  }
}
