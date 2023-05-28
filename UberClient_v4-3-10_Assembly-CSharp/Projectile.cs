// Decompiled with JetBrains decompiler
// Type: Projectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
[RequireComponent(typeof (Rigidbody))]
public abstract class Projectile : MonoBehaviour, IProjectile
{
  public const int DefaultTimeout = 30;
  [SerializeField]
  private Collider _trigger;
  [SerializeField]
  private Collider _collider;
  [SerializeField]
  private bool _showHeatwave;
  [SerializeField]
  private GameObject _explosionEffect;
  private Rigidbody _rigidbody;
  protected AudioSource _source;
  private float _positionSign;
  private Transform _transform;
  protected AudioClip _explosionSound;

  public ParticleConfigurationType ExplosionEffect { get; set; }

  public Rigidbody Rigidbody => this._rigidbody;

  public ProjectileDetonator Detonator { get; set; }

  public bool IsProjectileExploded { get; protected set; }

  public float TimeOut { get; set; }

  public int ID { get; set; }

  protected virtual void Awake()
  {
    this._rigidbody = this.GetComponent<Rigidbody>();
    this._source = this.GetComponent<AudioSource>();
    if ((Object) this._collider == (Object) null && (Object) this._trigger == (Object) null)
      UnityEngine.Debug.LogError((object) ("The Projectile " + this.gameObject.name + " has not assigned Collider or Trigger! Check your Inspector settings."));
    if ((bool) (Object) this._collider && this._collider.isTrigger)
      UnityEngine.Debug.LogError((object) ("The Projectile " + this.gameObject.name + " has a Collider attached that is configured as Trigger! Check your Inspector settings."));
    if ((bool) (Object) this._trigger && !this._trigger.isTrigger)
      UnityEngine.Debug.LogError((object) ("The Projectile " + this.gameObject.name + " has a Trigger attached that is configured as Collider! Check your Inspector settings."));
    this._transform = this.transform;
    this._positionSign = Mathf.Sign(this._transform.position.y);
  }

  protected virtual void Start()
  {
    if (GameState.HasCurrentSpace && GameState.CurrentSpace.HasWaterPlane)
      this._positionSign = Mathf.Sign(this._transform.position.y - GameState.CurrentSpace.WaterPlaneHeight);
    this.StartCoroutine(this.StartTimeout());
  }

  public void MoveInDirection(Vector3 direction)
  {
    this.Rigidbody.isKinematic = false;
    this.Rigidbody.velocity = direction;
  }

  [DebuggerHidden]
  protected virtual IEnumerator StartTimeout() => (IEnumerator) new Projectile.\u003CStartTimeout\u003Ec__Iterator8A()
  {
    \u003C\u003Ef__this = this
  };

  protected abstract void OnTriggerEnter(Collider c);

  protected abstract void OnCollisionEnter(Collision c);

  protected virtual void Update()
  {
    if (!GameState.HasCurrentSpace || !GameState.CurrentSpace.HasWaterPlane || (double) this._positionSign == (double) Mathf.Sign(this._transform.position.y - GameState.CurrentSpace.WaterPlaneHeight))
      return;
    this._positionSign = Mathf.Sign(this._transform.position.y - GameState.CurrentSpace.WaterPlaneHeight);
    ParticleEffectController.ProjectileWaterRipplesEffect(this.ExplosionEffect, this._transform.position);
  }

  protected void Explode(Vector3 point, Vector3 normal, string tag)
  {
    this.Destroy();
    if (this.Detonator != null)
      this.Detonator.Explode(point);
    Singleton<ExplosionManager>.Instance.PlayExplosionSound(point, this._explosionSound);
    Singleton<ExplosionManager>.Instance.ShowExplosionEffect(point, normal, tag, this.ExplosionEffect);
    if (this._showHeatwave)
      ParticleEffectController.ShowHeatwaveEffect(this.transform.position);
    if (!(bool) (Object) this._explosionEffect)
      return;
    Object.Instantiate((Object) this._explosionEffect, point, Quaternion.LookRotation(normal));
  }

  public void Destroy()
  {
    if (this.IsProjectileExploded)
      return;
    this.IsProjectileExploded = true;
    this.gameObject.SetActive(false);
    Object.Destroy((Object) this.gameObject);
  }

  protected int CollisionMask => (bool) (Object) this.gameObject && this.gameObject.layer == 24 ? UberstrikeLayerMasks.RemoteRocketMask : UberstrikeLayerMasks.LocalRocketMask;

  public void SetExplosionSound(AudioClip clip) => this._explosionSound = clip;

  protected void PlayBounceSound(Vector3 position)
  {
    AudioClip audioClip = GameAudio.LauncherBounce1;
    if (Random.Range(0, 2) > 0)
      audioClip = GameAudio.LauncherBounce2;
    SfxManager.Play3dAudioClip(audioClip, position);
  }

  public Vector3 Explode()
  {
    Vector3 point = Vector3.zero;
    try
    {
      RaycastHit hitInfo;
      if (Physics.Raycast(this.transform.position - this.transform.forward, this.transform.forward, out hitInfo, 2f, this.CollisionMask))
      {
        point = hitInfo.point - this.transform.forward * 0.01f;
        this.Explode(point, hitInfo.normal, TagUtil.GetTag(hitInfo.collider));
      }
      else
      {
        point = this.transform.position;
        this.Explode(point, -this.transform.forward, string.Empty);
      }
    }
    catch
    {
      UnityEngine.Debug.LogWarning((object) "Grenade not exploded because it was already destroyed.");
    }
    return point;
  }
}
