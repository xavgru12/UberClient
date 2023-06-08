using System;
using System.Collections;
using UnityEngine;

public class SpringGrenadeQuickItem : QuickItem, IProjectile, IGrenadeProjectile
{
	private enum SpringGrenadeState
	{
		Flying = 1,
		Deployed
	}

	private class FlyingState : IState
	{
		private SpringGrenadeQuickItem behaviour;

		private float _timeOut;

		public FlyingState(SpringGrenadeQuickItem behaviour)
		{
			this.behaviour = behaviour;
		}

		public void OnEnter()
		{
			_timeOut = Time.time + (float)behaviour._config.LifeTime;
			SpringGrenadeQuickItem springGrenadeQuickItem = behaviour;
			springGrenadeQuickItem.OnCollisionEnterEvent = (Action<Collision>)Delegate.Combine(springGrenadeQuickItem.OnCollisionEnterEvent, new Action<Collision>(OnCollisionEnterEvent));
			GameObject gameObject = behaviour.gameObject;
			if (!gameObject || !GameState.Current.Avatar.Decorator || !gameObject.collider)
			{
				return;
			}
			Collider collider = gameObject.collider;
			CharacterHitArea[] hitAreas = GameState.Current.Avatar.Decorator.HitAreas;
			CharacterHitArea[] array = hitAreas;
			foreach (CharacterHitArea characterHitArea in array)
			{
				if (gameObject.activeInHierarchy && characterHitArea.gameObject.activeInHierarchy)
				{
					Physics.IgnoreCollision(collider, characterHitArea.collider);
				}
			}
		}

		public void OnResume()
		{
		}

		public void OnExit()
		{
			SpringGrenadeQuickItem springGrenadeQuickItem = behaviour;
			springGrenadeQuickItem.OnCollisionEnterEvent = (Action<Collision>)Delegate.Remove(springGrenadeQuickItem.OnCollisionEnterEvent, new Action<Collision>(OnCollisionEnterEvent));
		}

		public void OnUpdate()
		{
			if (_timeOut < Time.time)
			{
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
			}
		}

		private void OnCollisionEnterEvent(Collision c)
		{
			if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer))
			{
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
				GameState.Current.Actions.RemoveProjectile(behaviour.ID, arg2: true);
			}
			else if (!(c.transform.tag == "MovableObject") && behaviour._config.IsSticky)
			{
				if (c.contacts.Length != 0)
				{
					behaviour.transform.position = c.contacts[0].point + c.contacts[0].normal * behaviour.collider.bounds.extents.sqrMagnitude;
				}
				behaviour.machine.PopState();
				behaviour.machine.PushState(SpringGrenadeState.Deployed);
			}
			PlayBounceSound(c.transform.position);
		}

		protected void PlayBounceSound(Vector3 position)
		{
			AudioClip clip = GameAudio.LauncherBounce1;
			int num = UnityEngine.Random.Range(0, 2);
			if (num > 0)
			{
				clip = GameAudio.LauncherBounce2;
			}
			AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(clip, position);
		}
	}

	private class DeployedState : IState
	{
		private SpringGrenadeQuickItem behaviour;

		private float _timeOut;

		public DeployedState(SpringGrenadeQuickItem behaviour)
		{
			this.behaviour = behaviour;
			behaviour.OnProjectileExploded = null;
		}

		public void OnEnter()
		{
			_timeOut = Time.time + (float)behaviour._config.LifeTime;
			SpringGrenadeQuickItem springGrenadeQuickItem = behaviour;
			springGrenadeQuickItem.OnTriggerEnterEvent = (Action<Collider>)Delegate.Combine(springGrenadeQuickItem.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));
			if ((bool)behaviour.rigidbody)
			{
				behaviour.rigidbody.isKinematic = true;
			}
			if ((bool)behaviour.collider)
			{
				UnityEngine.Object.Destroy(behaviour.collider);
			}
			behaviour.gameObject.layer = 2;
			if ((bool)behaviour.DeployedEffect)
			{
				behaviour.DeployedEffect.emit = true;
			}
		}

		public void OnResume()
		{
		}

		public void OnExit()
		{
			SpringGrenadeQuickItem springGrenadeQuickItem = behaviour;
			springGrenadeQuickItem.OnTriggerEnterEvent = (Action<Collider>)Delegate.Remove(springGrenadeQuickItem.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));
		}

		public void OnTriggerEnterEvent(Collider c)
		{
			if (TagUtil.GetTag(c) == "Player")
			{
				behaviour.machine.PopState();
				GameState.Current.Player.MoveController.ApplyForce(behaviour._config.JumpDirection.normalized * behaviour._config.Force, CharacterMoveController.ForceType.Additive);
				AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(behaviour.JumpSound, 0uL);
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
				GameState.Current.Actions.RemoveProjectile(behaviour.ID, arg2: true);
			}
			else if (behaviour.collider.gameObject.layer == 20)
			{
				AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(GameAudio.JumpPad, behaviour.transform.position);
			}
		}

		public void OnUpdate()
		{
			if (_timeOut < Time.time)
			{
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
			}
		}
	}

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

	private StateMachine<SpringGrenadeState> machine = new StateMachine<SpringGrenadeState>();

	private bool _isDestroyed;

	public ParticleEmitter Smoke => _smoke;

	public ParticleEmitter DeployedEffect => _deployedEffect;

	public Renderer Renderer => _renderer;

	public override QuickItemConfiguration Configuration
	{
		get
		{
			return _config;
		}
		set
		{
			_config = (SpringGrenadeConfiguration)value;
		}
	}

	public AudioClip ExplosionSound
	{
		get;
		set;
	}

	public AudioClip JumpSound => _sound;

	public int ID
	{
		get;
		set;
	}

	public Vector3 Position
	{
		get
		{
			if ((bool)base.transform)
			{
				return base.transform.position;
			}
			return Vector3.zero;
		}
		private set
		{
			if ((bool)base.transform)
			{
				base.transform.position = value;
			}
		}
	}

	public Vector3 Velocity
	{
		get
		{
			if ((bool)base.rigidbody)
			{
				return base.rigidbody.velocity;
			}
			return Vector3.zero;
		}
		private set
		{
			if ((bool)base.rigidbody)
			{
				base.rigidbody.velocity = value;
			}
		}
	}

	private event Action<Collider> OnTriggerEnterEvent;

	private event Action<Collision> OnCollisionEnterEvent;

	public event Action<IGrenadeProjectile> OnProjectileExploded;

	public event Action<IGrenadeProjectile> OnProjectileEmitted;

	public event Action<int, Vector3> OnExploded;

	protected override void OnActivated()
	{
		Vector3 vector = GameState.Current.PlayerData.ShootingPoint + GameState.Current.Player.EyePosition;
		Vector3 position = vector + GameState.Current.PlayerData.ShootingDirection * 2f;
		Vector3 velocity = GameState.Current.PlayerData.ShootingDirection * _config.Speed;
		float distance = 2f;
		if (Physics.Raycast(vector, GameState.Current.PlayerData.ShootingDirection * 2f, out RaycastHit hitInfo, distance, UberstrikeLayerMasks.LocalRocketMask))
		{
			SpringGrenadeQuickItem springGrenadeQuickItem = Throw(hitInfo.point, Vector3.zero) as SpringGrenadeQuickItem;
			springGrenadeQuickItem.machine.PopAllStates();
			GameState.Current.Player.MoveController.ApplyForce(_config.JumpDirection.normalized * _config.Force, CharacterMoveController.ForceType.Additive);
			AutoMonoBehaviour<SfxManager>.Instance.PlayInGameAudioClip(JumpSound, 0uL);
			StartCoroutine(DestroyDelayed(springGrenadeQuickItem.ID));
		}
		else
		{
			IGrenadeProjectile grenadeProjectile = Throw(position, velocity);
			grenadeProjectile.OnProjectileExploded += delegate(IGrenadeProjectile p)
			{
				Collider[] array = Physics.OverlapSphere(p.Position, 2f, UberstrikeLayerMasks.ExplosionMask);
				Collider[] array2 = array;
				Collider[] array3 = array2;
				foreach (Collider collider in array3)
				{
					CharacterHitArea component = collider.gameObject.GetComponent<CharacterHitArea>();
					if (component != null && component.RecieveProjectileDamage)
					{
						component.Shootable.ApplyForce(component.transform.position, _config.JumpDirection.normalized * _config.Force);
					}
				}
			};
		}
	}

	private IEnumerator DestroyDelayed(int projectileId)
	{
		yield return new WaitForSeconds(0.2f);
		Singleton<ProjectileManager>.Instance.RemoveProjectile(projectileId);
		GameState.Current.Actions.RemoveProjectile(projectileId, arg2: true);
	}

	public IGrenadeProjectile Throw(Vector3 position, Vector3 velocity)
	{
		SpringGrenadeQuickItem springGrenadeQuickItem = UnityEngine.Object.Instantiate(this) as SpringGrenadeQuickItem;
		springGrenadeQuickItem.gameObject.SetActive(value: true);
		for (int i = 0; i < springGrenadeQuickItem.gameObject.transform.childCount; i++)
		{
			springGrenadeQuickItem.gameObject.transform.GetChild(i).gameObject.SetActive(value: true);
		}
		if ((bool)springGrenadeQuickItem.rigidbody)
		{
			springGrenadeQuickItem.rigidbody.isKinematic = false;
		}
		springGrenadeQuickItem.Position = position;
		springGrenadeQuickItem.Velocity = velocity;
		springGrenadeQuickItem.machine.RegisterState(SpringGrenadeState.Flying, new FlyingState(springGrenadeQuickItem));
		springGrenadeQuickItem.machine.RegisterState(SpringGrenadeState.Deployed, new DeployedState(springGrenadeQuickItem));
		springGrenadeQuickItem.machine.PushState(SpringGrenadeState.Flying);
		if (this.OnProjectileEmitted != null)
		{
			this.OnProjectileEmitted(springGrenadeQuickItem);
		}
		return springGrenadeQuickItem;
	}

	public void SetLayer(UberstrikeLayer layer)
	{
		LayerUtil.SetLayerRecursively(base.transform, layer);
	}

	private void Update()
	{
		machine.Update();
	}

	private void OnTriggerEnter(Collider c)
	{
		if (this.OnTriggerEnterEvent != null)
		{
			this.OnTriggerEnterEvent(c);
		}
	}

	private void OnCollisionEnter(Collision c)
	{
		if (this.OnCollisionEnterEvent != null)
		{
			this.OnCollisionEnterEvent(c);
		}
	}

	public Vector3 Explode()
	{
		Vector3 result = Vector3.zero;
		try
		{
			if (this.OnExploded != null)
			{
				this.OnExploded(ID, base.transform.position);
			}
			if (this.OnProjectileExploded != null)
			{
				this.OnProjectileExploded(this);
			}
			result = base.transform.position;
			Destroy();
			return result;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			return result;
		}
	}

	public void Destroy()
	{
		if (!_isDestroyed)
		{
			_isDestroyed = true;
			base.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
