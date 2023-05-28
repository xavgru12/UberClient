// Decompiled with JetBrains decompiler
// Type: EnergyProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class EnergyProjectile : Projectile
{
  [SerializeField]
  private MeshRenderer _trailRenderer;
  [SerializeField]
  private MeshRenderer _headRenderer;
  [SerializeField]
  private Light _light;
  [SerializeField]
  private Color _energyColor = Color.white;
  [SerializeField]
  private float _afterGlowDuration = 2f;

  public Color EnergyColor
  {
    get => this._energyColor;
    set
    {
      this._energyColor = value;
      this._headRenderer.material.SetColor("_TintColor", this._energyColor);
      this._trailRenderer.material.SetColor("_TintColor", this._energyColor);
    }
  }

  public float AfterGlowDuration
  {
    get => this._afterGlowDuration;
    set => this._afterGlowDuration = value;
  }

  protected override void Awake()
  {
    base.Awake();
    if (!((Object) this._light != (Object) null))
      return;
    this._light.enabled = ApplicationDataManager.ApplicationOptions.VideoQualityLevel == 2;
  }

  protected override void OnTriggerEnter(Collider c)
  {
    if (this.IsProjectileExploded || !LayerUtil.IsLayerInMask(this.CollisionMask, c.gameObject.layer))
      return;
    this.Explode();
  }

  protected override void OnCollisionEnter(Collision c)
  {
    if (this.IsProjectileExploded || !LayerUtil.IsLayerInMask(this.CollisionMask, c.gameObject.layer))
      return;
    if (c.contacts.Length > 0)
      this.Explode(c.contacts[0].point, c.contacts[0].normal, TagUtil.GetTag(c.collider));
    else
      this.Explode();
  }
}
