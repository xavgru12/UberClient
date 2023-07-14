// Decompiled with JetBrains decompiler
// Type: ProjectileDetonator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Types;
using UnityEngine;

public class ProjectileDetonator
{
  public ProjectileDetonator(
    float radius,
    float damage,
    int force,
    Vector3 direction,
    int projectileId,
    int weaponId,
    UberstrikeItemClass weaponClass,
    DamageEffectType damageEffectFlag,
    float damageEffectValue)
  {
    this.Radius = radius;
    this.Damage = damage;
    this.Force = force;
    this.Direction = direction;
    this.ProjectileID = projectileId;
    this.WeaponID = weaponId;
    this.WeaponClass = weaponClass;
    this.DamageEffectFlag = damageEffectFlag;
    this.DamageEffectValue = damageEffectValue;
  }

  public float Radius { get; private set; }

  public float Damage { get; private set; }

  public int Force { get; private set; }

  public Vector3 Direction { get; set; }

  public int WeaponID { get; private set; }

  public UberstrikeItemClass WeaponClass { get; private set; }

  public int ProjectileID { get; private set; }

  public DamageEffectType DamageEffectFlag { get; private set; }

  public float DamageEffectValue { get; private set; }

  public void Explode(Vector3 position) => ProjectileDetonator.Explode(position, this.ProjectileID, this.Damage, this.Direction, this.Radius, this.Force, this.WeaponID, this.WeaponClass, this.DamageEffectFlag, this.DamageEffectValue);

  public static void Explode(
    Vector3 position,
    int projectileId,
    float damage,
    Vector3 dir,
    float radius,
    int force,
    int weaponId,
    UberstrikeItemClass weaponClass,
    DamageEffectType damageEffectFlag = DamageEffectType.None,
    float damageEffectValue = 0)
  {
    Collider[] colliderArray = Physics.OverlapSphere(position, radius, UberstrikeLayerMasks.ExplosionMask);
    Vector3 start = position;
    int num1 = 1;
    foreach (Collider collider in colliderArray)
    {
      BaseGameProp component = collider.transform.GetComponent<BaseGameProp>();
      RaycastHit hitInfo;
      if ((Object) component != (Object) null && component.RecieveProjectileDamage && (!Physics.Linecast(start, collider.bounds.center, out hitInfo, UberstrikeLayerMasks.ProtectionMask) || (Object) hitInfo.transform == (Object) component.transform || (Object) hitInfo.transform.GetComponent<BaseGameProp>() != (Object) null))
      {
        Vector3 vector3_1 = collider.ClosestPointOnBounds(position);
        float num2 = 1f;
        Vector3 vector3_2 = collider.transform.position - position;
        if ((double) vector3_2.sqrMagnitude < 0.0099999997764825821)
        {
          vector3_2 = dir;
        }
        else
        {
          if ((double) radius > 1.0)
            num2 = Mathf.Clamp((radius - Mathf.Clamp(vector3_2.magnitude, 0.0f, radius)) / radius, 0.0f, 0.6f) + 0.4f;
          vector3_2 = vector3_2.normalized;
        }
        short damage1 = (short) Mathf.CeilToInt(damage * num2);
        if (component.IsLocal)
          damage1 /= (short) 2;
        if (damage1 > (short) 0)
          component.ApplyDamage(new DamageInfo(damage1)
          {
            Force = vector3_2 * (float) force,
            Hitpoint = vector3_1,
            ProjectileID = projectileId,
            WeaponID = weaponId,
            WeaponClass = weaponClass,
            DamageEffectFlag = damageEffectFlag,
            DamageEffectValue = damageEffectValue
          });
        else
          continue;
      }
      ++num1;
    }
  }
}
