// Decompiled with JetBrains decompiler
// Type: SpringGrenadeQuickItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SpringGrenadeQuickItem : QuickItem, IProjectile, IGrenadeProjectile
{
  [SerializeField]
  private AudioClip _sound;
  [SerializeField]
  private Renderer _renderer;
  [SerializeField]
  private ParticleEmitter _smoke;
  [SerializeField]
  private ParticleEmitter _deployedEffect;
  [SerializeField]
  private SpringGrenadeConfiguration _config;
  private StateMachine machine = new StateMachine();
  private bool _isDestroyed;

  private event Action<Collider> OnTriggerEnterEvent;

  private event Action<Collision> OnCollisionEnterEvent;

  public event Action<IGrenadeProjectile> OnProjectileExploded;

  public event Action<IGrenadeProjectile> OnProjectileEmitted;

  public event Action<int, Vector3> OnExploded;

  public ParticleEmitter Smoke => this._smoke;

  public ParticleEmitter DeployedEffect => this._deployedEffect;

  public Renderer Renderer => this._renderer;

  public override QuickItemConfiguration Configuration
  {
    get => (QuickItemConfiguration) this._config;
    set => this._config = (SpringGrenadeConfiguration) value;
  }

  public AudioClip ExplosionSound { get; set; }

  public AudioClip JumpSound => this._sound;

  protected override void OnActivated()
  {
    Vector3 origin = GameState.LocalCharacter.ShootingPoint + LocalPlayer.EyePosition;
    Vector3 position = origin + GameState.LocalCharacter.ShootingDirection * 2f;
    Vector3 velocity = GameState.LocalCharacter.ShootingDirection * (float) this._config.Speed;
    float distance = 2f;
    RaycastHit hitInfo;
    if (Physics.Raycast(origin, GameState.LocalCharacter.ShootingDirection * 2f, out hitInfo, distance, UberstrikeLayerMasks.LocalRocketMask))
    {
      SpringGrenadeQuickItem grenadeQuickItem = this.Throw(hitInfo.point, Vector3.zero) as SpringGrenadeQuickItem;
      grenadeQuickItem.machine.PopAllStates();
      GameState.LocalPlayer.MoveController.ApplyForce(this._config.JumpDirection.normalized * (float) this._config.Force, CharacterMoveController.ForceType.Additive);
      SfxManager.Play2dAudioClip(this.JumpSound);
      this.StartCoroutine(this.DestroyDelayed(grenadeQuickItem.ID));
    }
    else
      this.Throw(position, velocity).OnProjectileExploded += (Action<IGrenadeProjectile>) (p =>
      {
        foreach (Component component1 in Physics.OverlapSphere(p.Position, 2f, UberstrikeLayerMasks.ExplosionMask))
        {
          CharacterHitArea component2 = component1.gameObject.GetComponent<CharacterHitArea>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.RecieveProjectileDamage)
            component2.Shootable.ApplyForce(component2.transform.position, this._config.JumpDirection.normalized * (float) this._config.Force);
        }
      });
  }

  [DebuggerHidden]
  private IEnumerator DestroyDelayed(int projectileId) => (IEnumerator) new SpringGrenadeQuickItem.\u003CDestroyDelayed\u003Ec__Iterator58()
  {
    projectileId = projectileId,
    \u003C\u0024\u003EprojectileId = projectileId
  };

  public IGrenadeProjectile Throw(Vector3 position, Vector3 velocity)
  {
    SpringGrenadeQuickItem behaviour = UnityEngine.Object.Instantiate((UnityEngine.Object) this) as SpringGrenadeQuickItem;
    behaviour.gameObject.SetActive(true);
    for (int index = 0; index < behaviour.gameObject.transform.childCount; ++index)
      behaviour.gameObject.transform.GetChild(index).gameObject.SetActive(true);
    behaviour.Position = position;
    behaviour.Velocity = velocity;
    behaviour.machine.RegisterState(1, (IState) new SpringGrenadeQuickItem.FlyingState(behaviour));
    behaviour.machine.RegisterState(2, (IState) new SpringGrenadeQuickItem.DeployedState(behaviour));
    behaviour.machine.PushState(1);
    if (this.OnProjectileEmitted != null)
      this.OnProjectileEmitted((IGrenadeProjectile) behaviour);
    return (IGrenadeProjectile) behaviour;
  }

  public void SetLayer(UberstrikeLayer layer) => LayerUtil.SetLayerRecursively(this.transform, layer);

  private void Update() => this.machine.Update();

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
      if (this.OnExploded != null)
        this.OnExploded(this.ID, this.transform.position);
      if (this.OnProjectileExploded != null)
        this.OnProjectileExploded((IGrenadeProjectile) this);
      vector3 = this.transform.position;
      this.Destroy();
    }
    catch
    {
      UnityEngine.Debug.LogWarning((object) "SpringGrenade not exploded because it was already destroyed.");
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

  private enum SpringGrenadeState
  {
    Flying = 1,
    Deployed = 2,
  }

  private class FlyingState : IState
  {
    private SpringGrenadeQuickItem behaviour;
    private float _timeOut;

    public FlyingState(SpringGrenadeQuickItem behaviour) => this.behaviour = behaviour;

    public void OnEnter()
    {
      this._timeOut = Time.time + (float) this.behaviour._config.LifeTime;
      this.behaviour.OnCollisionEnterEvent += new Action<Collision>(this.OnCollisionEnterEvent);
      GameObject gameObject = this.behaviour.gameObject;
      if (!(bool) (UnityEngine.Object) gameObject || !(bool) (UnityEngine.Object) GameState.LocalDecorator || !(bool) (UnityEngine.Object) gameObject.collider)
        return;
      Collider collider = gameObject.collider;
      foreach (CharacterHitArea hitArea in GameState.LocalDecorator.HitAreas)
      {
        if (gameObject.activeInHierarchy && hitArea.gameObject.activeInHierarchy)
          Physics.IgnoreCollision(collider, hitArea.collider);
      }
    }

    public void OnExit() => this.behaviour.OnCollisionEnterEvent -= new Action<Collision>(this.OnCollisionEnterEvent);

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
    private SpringGrenadeQuickItem behaviour;
    private float _timeOut;

    public DeployedState(SpringGrenadeQuickItem behaviour)
    {
      this.behaviour = behaviour;
      behaviour.OnProjectileExploded = (Action<IGrenadeProjectile>) null;
    }

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

    public void OnTriggerEnterEvent(Collider c)
    {
      if (TagUtil.GetTag(c) == "Player")
      {
        this.behaviour.machine.PopState();
        GameState.LocalPlayer.MoveController.ApplyForce(this.behaviour._config.JumpDirection.normalized * (float) this.behaviour._config.Force, CharacterMoveController.ForceType.Additive);
        SfxManager.Play2dAudioClip(this.behaviour.JumpSound);
        Singleton<ProjectileManager>.Instance.RemoveProjectile(this.behaviour.ID);
        GameState.CurrentGame.RemoveProjectile(this.behaviour.ID, true);
      }
      else
      {
        if (this.behaviour.collider.gameObject.layer != 20)
          return;
        SfxManager.Play3dAudioClip(GameAudio.JumpPad, 1f, 0.1f, 10f, AudioRolloffMode.Linear, this.behaviour.transform.position);
      }
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
