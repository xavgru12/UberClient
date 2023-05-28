// Decompiled with JetBrains decompiler
// Type: RocketProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RocketProjectile : Projectile
{
  [SerializeField]
  private ParticleRenderer _smokeRenderer;
  [SerializeField]
  private ParticleEmitter _smokeEmitter;
  [SerializeField]
  private Color _smokeColor = Color.white;
  [SerializeField]
  private float _smokeAmount = 1f;
  [SerializeField]
  private Light _light;

  public Color SmokeColor
  {
    get => this._smokeColor;
    set
    {
      this._smokeColor = value;
      if (!(bool) (Object) this._smokeRenderer)
        return;
      this._smokeRenderer.material.SetColor("_TintColor", this._smokeColor);
    }
  }

  public float SmokeAmount
  {
    get => this._smokeAmount;
    set
    {
      this._smokeAmount = value;
      if (!(bool) (Object) this._smokeEmitter)
        return;
      this._smokeEmitter.minEmission = this._smokeAmount * 10f;
      this._smokeEmitter.maxEmission = this._smokeAmount * 20f;
    }
  }

  protected override void Awake()
  {
    base.Awake();
    this.SmokeColor = this._smokeColor;
    this.SmokeAmount = this._smokeAmount;
    if (!((Object) this._light != (Object) null))
      return;
    this._light.enabled = ApplicationDataManager.ApplicationOptions.VideoQualityLevel == 2;
  }

  protected override void OnTriggerEnter(Collider c)
  {
    if (this.IsProjectileExploded || !LayerUtil.IsLayerInMask(this.CollisionMask, c.gameObject.layer))
      return;
    Singleton<ProjectileManager>.Instance.RemoveProjectile(this.ID);
    GameState.CurrentGame.RemoveProjectile(this.ID, true);
  }

  protected override void OnCollisionEnter(Collision c)
  {
    if (this.IsProjectileExploded || !(bool) (Object) c.gameObject || !LayerUtil.IsLayerInMask(this.CollisionMask, c.gameObject.layer))
      return;
    Singleton<ProjectileManager>.Instance.RemoveProjectile(this.ID);
    GameState.CurrentGame.RemoveProjectile(this.ID, true);
  }
}
