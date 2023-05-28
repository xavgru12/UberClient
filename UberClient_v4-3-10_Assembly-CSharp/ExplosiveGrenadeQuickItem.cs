// Decompiled with JetBrains decompiler
// Type: ExplosiveGrenadeQuickItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Types;
using UnityEngine;

public class ExplosiveGrenadeQuickItem : QuickItem, IProjectile, IGrenadeProjectile
{
  [SerializeField]
  private Renderer _renderer;
  [SerializeField]
  private ParticleEmitter _smoke;
  [SerializeField]
  private ParticleEmitter _deployedEffect;
  [SerializeField]
  private AudioClip _explosionSound;
  [SerializeField]
  private GameObject _explosionSfx;
  [SerializeField]
  private ExplosiveGrenadeConfiguration _config;
  private StateMachine machine = new StateMachine();
  private bool _isDestroyed;

  private event Action<Collider> OnTriggerEnterEvent;

  private event Action<Collision> OnCollisionEnterEvent;

  public event Action<IGrenadeProjectile> OnProjectileExploded;

  public event Action<IGrenadeProjectile> OnProjectileEmitted;

  public ParticleEmitter Smoke => this._smoke;

  public ParticleEmitter DeployedEffect => this._deployedEffect;

  public Renderer Renderer => this._renderer;

  public override QuickItemConfiguration Configuration
  {
    get => (QuickItemConfiguration) this._config;
    set => this._config = (ExplosiveGrenadeConfiguration) value;
  }

  protected override void OnActivated()
  {
    Vector3 origin = GameState.LocalCharacter.ShootingPoint + LocalPlayer.EyePosition;
    Vector3 position = origin + GameState.LocalCharacter.ShootingDirection * 2f;
    Vector3 velocity = GameState.LocalCharacter.ShootingDirection * (float) this._config.Speed;
    float distance = 2f;
    RaycastHit hitInfo;
    if (Physics.Raycast(origin, GameState.LocalCharacter.ShootingDirection * 2f, out hitInfo, distance, UberstrikeLayerMasks.LocalRocketMask))
    {
      ExplosiveGrenadeQuickItem grenadeQuickItem = this.Throw(hitInfo.point, Vector3.zero) as ExplosiveGrenadeQuickItem;
      grenadeQuickItem.machine.PopAllStates();
      grenadeQuickItem.OnProjectileExploded += (Action<IGrenadeProjectile>) (p => ProjectileDetonator.Explode(p.Position, p.ID, (float) this._config.Damage, Vector3.up, (float) this._config.SplashRadius, 5, this.Configuration.ID, UberstrikeItemClass.WeaponLauncher, damageEffectValue: 0.0f));
      Singleton<ProjectileManager>.Instance.RemoveProjectile(grenadeQuickItem.ID);
      GameState.CurrentGame.RemoveProjectile(grenadeQuickItem.ID, true);
    }
    else
      this.Throw(position, velocity).OnProjectileExploded += (Action<IGrenadeProjectile>) (p => ProjectileDetonator.Explode(p.Position, p.ID, (float) this._config.Damage, Vector3.up, (float) this._config.SplashRadius, 5, this.Configuration.ID, UberstrikeItemClass.WeaponLauncher, damageEffectValue: 0.0f));
  }

  public IGrenadeProjectile Throw(Vector3 position, Vector3 velocity)
  {
    ExplosiveGrenadeQuickItem behaviour = UnityEngine.Object.Instantiate((UnityEngine.Object) this) as ExplosiveGrenadeQuickItem;
    if ((bool) (UnityEngine.Object) behaviour)
    {
      for (int index = 0; index < behaviour.transform.childCount; ++index)
        behaviour.transform.GetChild(index).gameObject.SetActive(true);
      behaviour.Position = position;
      behaviour.Velocity = velocity;
      behaviour.collider.material.bounciness = this._config.Bounciness;
      behaviour.machine.RegisterState(1, (IState) new ExplosiveGrenadeQuickItem.FlyingState(behaviour));
      behaviour.machine.RegisterState(2, (IState) new ExplosiveGrenadeQuickItem.DeployedState(behaviour));
      behaviour.machine.PushState(1);
    }
    if (this.OnProjectileEmitted != null)
      this.OnProjectileEmitted((IGrenadeProjectile) behaviour);
    return (IGrenadeProjectile) behaviour;
  }

  public void SetLayer(UberstrikeLayer layer) => LayerUtil.SetLayerRecursively(this.transform, layer);

  private void Update() => this.machine.Update();

  private void OnGUI()
  {
    if (!this.Behaviour.IsBusy || (double) this.Behaviour.FocusTimeRemaining <= 0.0)
      return;
    float height = Mathf.Clamp((float) Screen.height * 0.03f, 10f, 40f);
    float width = height * 10f;
    GUI.Label(new Rect((float) (((double) Screen.width - (double) width) * 0.5), (float) (Screen.height / 2 + 20), width, height), "Charging Grenade", BlueStonez.label_interparkbold_16pt);
    GUITools.DrawWarmupBar(new Rect((float) (((double) Screen.width - (double) width) * 0.5), (float) (Screen.height / 2 + 50), width, height), this.Behaviour.FocusTimeTotal - this.Behaviour.FocusTimeRemaining, this.Behaviour.FocusTimeTotal);
  }

  private void OnTriggerEnter(Collider c)
  {
    if (this.OnTriggerEnterEvent == null)
      return;
    this.OnTriggerEnterEvent(c);
  }

  private void OnCollisionEnter(Collision c)
  {
    if (this.OnCollisionEnterEvent == null)
      return;
    this.OnCollisionEnterEvent(c);
  }

  public Vector3 Explode()
  {
    Vector3 vector3 = Vector3.zero;
    try
    {
      if ((UnityEngine.Object) this._explosionSound != (UnityEngine.Object) null)
        SfxManager.Play3dAudioClip(this._explosionSound, this.transform.position);
      if ((bool) (UnityEngine.Object) this._explosionSfx)
      {
        GameObject gameObject = UnityEngine.Object.Instantiate((UnityEngine.Object) this._explosionSfx) as GameObject;
        if ((bool) (UnityEngine.Object) gameObject)
        {
          gameObject.transform.position = this.transform.position;
          SelfDestroy selfDestroy = gameObject.AddComponent<SelfDestroy>();
          if ((bool) (UnityEngine.Object) selfDestroy)
            selfDestroy.SetDelay(2f);
        }
      }
      else
        ParticleEffectController.ShowExplosionEffect(ParticleConfigurationType.LauncherDefault, SurfaceEffectType.None, this.transform.position, Vector3.up);
      if (this.OnProjectileExploded != null)
        this.OnProjectileExploded((IGrenadeProjectile) this);
      vector3 = this.transform.position;
      this.Destroy();
    }
    catch
    {
      Debug.LogWarning((object) "ExplosiveGrenade not exploded because it was already destroyed.");
    }
    return vector3;
  }

  public int ID { get; set; }

  public void Destroy()
  {
    if (this._isDestroyed)
      return;
    this._isDestroyed = true;
    this.gameObject.SetActive(false);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public Vector3 Position
  {
    get => (bool) (UnityEngine.Object) this.transform ? this.transform.position : Vector3.zero;
    private set
    {
      if (!(bool) (UnityEngine.Object) this.transform)
        return;
      this.transform.position = value;
    }
  }

  public Vector3 Velocity
  {
    get => (bool) (UnityEngine.Object) this.rigidbody ? this.rigidbody.velocity : Vector3.zero;
    private set
    {
      if (!(bool) (UnityEngine.Object) this.rigidbody)
        return;
      this.rigidbody.velocity = value;
    }
  }

  private class FlyingState : IState
  {
    private ExplosiveGrenadeQuickItem behaviour;
    private float _timeOut;

    public FlyingState(ExplosiveGrenadeQuickItem behaviour) => this.behaviour = behaviour;

    public void OnEnter()
    {
      this._timeOut = Time.time + (float) this.behaviour._config.LifeTime;
      this.behaviour.OnCollisionEnterEvent += new Action<Collision>(this.OnCollisionEnterEvent);
      if (!this.behaviour._config.IsSticky)
        this.behaviour.OnTriggerEnterEvent += new Action<Collider>(this.OnTriggerEnterEvent);
      GameObject gameObject = this.behaviour.gameObject;
      if (!(bool) (UnityEngine.Object) gameObject || !(bool) (UnityEngine.Object) GameState.LocalDecorator || !(bool) (UnityEngine.Object) gameObject.collider)
        return;
      Collider collider = gameObject.collider;
      foreach (CharacterHitArea hitArea in GameState.LocalDecorator.HitAreas)
      {
        if (gameObject.activeSelf && hitArea.gameObject.activeSelf)
          Physics.IgnoreCollision(collider, hitArea.collider);
      }
    }

    public void OnExit()
    {
      this.behaviour.OnCollisionEnterEvent -= new Action<Collision>(this.OnCollisionEnterEvent);
      if (this.behaviour._config.IsSticky)
        return;
      this.behaviour.OnTriggerEnterEvent -= new Action<Collider>(this.OnTriggerEnterEvent);
    }

    public void OnUpdate()
    {
      if ((double) this._timeOut >= (double) Time.time)
        return;
      this.behaviour.machine.PopState();
      Singleton<ProjectileManager>.Instance.RemoveProjectile(this.behaviour.ID);
    }

    public void OnGUI()
    {
    }

    private void OnCollisionEnterEvent(Collision c)
    {
      if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer))
      {
        this.behaviour.machine.PopState();
        Singleton<ProjectileManager>.Instance.RemoveProjectile(this.behaviour.ID);
        GameState.CurrentGame.RemoveProjectile(this.behaviour.ID, true);
      }
      else if (this.behaviour._config.IsSticky)
      {
        if (c.contacts.Length > 0)
          this.behaviour.transform.position = c.contacts[0].point + c.contacts[0].normal * this.behaviour.collider.bounds.extents.sqrMagnitude;
        this.behaviour.machine.PopState();
        this.behaviour.machine.PushState(2);
      }
      this.PlayBounceSound(c.transform.position);
    }

    private void OnTriggerEnterEvent(Collider c)
    {
      if (!LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer))
        return;
      this.behaviour.machine.PopState();
      Singleton<ProjectileManager>.Instance.RemoveProjectile(this.behaviour.ID);
      GameState.CurrentGame.RemoveProjectile(this.behaviour.ID, true);
    }

    protected void PlayBounceSound(Vector3 position)
    {
      AudioClip audioClip = GameAudio.LauncherBounce1;
      if (UnityEngine.Random.Range(0, 2) > 0)
        audioClip = GameAudio.LauncherBounce2;
      SfxManager.Play3dAudioClip(audioClip, position);
    }
  }

  private class DeployedState : IState
  {
    private ExplosiveGrenadeQuickItem behaviour;
    private float _timeOut;

    public DeployedState(ExplosiveGrenadeQuickItem behaviour) => this.behaviour = behaviour;

    public void OnEnter()
    {
      this._timeOut = Time.time + (float) this.behaviour._config.LifeTime;
      this.behaviour.OnTriggerEnterEvent += new Action<Collider>(this.OnTriggerEnterEvent);
      if ((bool) (UnityEngine.Object) this.behaviour.rigidbody)
        this.behaviour.rigidbody.isKinematic = true;
      if ((bool) (UnityEngine.Object) this.behaviour.collider)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.behaviour.collider);
      this.behaviour.gameObject.layer = 2;
      if (!(bool) (UnityEngine.Object) this.behaviour.DeployedEffect)
        return;
      this.behaviour.DeployedEffect.emit = true;
    }

    public void OnExit() => this.behaviour.OnTriggerEnterEvent -= new Action<Collider>(this.OnTriggerEnterEvent);

    private void OnTriggerEnterEvent(Collider c)
    {
      if (!LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer))
        return;
      this.behaviour.machine.PopState();
      Singleton<ProjectileManager>.Instance.RemoveProjectile(this.behaviour.ID);
      GameState.CurrentGame.RemoveProjectile(this.behaviour.ID, true);
    }

    public void OnUpdate()
    {
      if ((double) this._timeOut >= (double) Time.time)
        return;
      this.behaviour.machine.PopState();
      Singleton<ProjectileManager>.Instance.RemoveProjectile(this.behaviour.ID);
    }

    public void OnGUI()
    {
    }
  }
}
