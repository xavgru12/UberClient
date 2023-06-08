using System;
using UberStrike.Core.Types;
using UnityEngine;

public class ExplosiveGrenadeQuickItem : QuickItem, IProjectile, IGrenadeProjectile
{
	private class FlyingState : IState
	{
		private ExplosiveGrenadeQuickItem behaviour;

		private float _timeOut;

		public FlyingState(ExplosiveGrenadeQuickItem behaviour)
		{
			this.behaviour = behaviour;
		}

		public void OnEnter()
		{
			_timeOut = Time.time + (float)behaviour._config.LifeTime;
			ExplosiveGrenadeQuickItem explosiveGrenadeQuickItem = behaviour;
			explosiveGrenadeQuickItem.OnCollisionEnterEvent = (Action<Collision>)Delegate.Combine(explosiveGrenadeQuickItem.OnCollisionEnterEvent, new Action<Collision>(OnCollisionEnterEvent));
			if (!behaviour._config.IsSticky)
			{
				ExplosiveGrenadeQuickItem explosiveGrenadeQuickItem2 = behaviour;
				explosiveGrenadeQuickItem2.OnTriggerEnterEvent = (Action<Collider>)Delegate.Combine(explosiveGrenadeQuickItem2.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));
			}
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
				if (gameObject.activeSelf && characterHitArea.gameObject.activeSelf)
				{
					Physics.IgnoreCollision(collider, characterHitArea.collider);
				}
			}
		}

		public void OnExit()
		{
			ExplosiveGrenadeQuickItem explosiveGrenadeQuickItem = behaviour;
			explosiveGrenadeQuickItem.OnCollisionEnterEvent = (Action<Collision>)Delegate.Remove(explosiveGrenadeQuickItem.OnCollisionEnterEvent, new Action<Collision>(OnCollisionEnterEvent));
			if (!behaviour._config.IsSticky)
			{
				ExplosiveGrenadeQuickItem explosiveGrenadeQuickItem2 = behaviour;
				explosiveGrenadeQuickItem2.OnTriggerEnterEvent = (Action<Collider>)Delegate.Remove(explosiveGrenadeQuickItem2.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));
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

		public void OnResume()
		{
		}

		private void OnCollisionEnterEvent(Collision c)
		{
			if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer))
			{
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
				GameState.Current.Actions.RemoveProjectile(behaviour.ID, arg2: true);
			}
			else if (behaviour._config.IsSticky)
			{
				if (c.contacts.Length != 0)
				{
					behaviour.transform.position = c.contacts[0].point + c.contacts[0].normal * behaviour.collider.bounds.extents.sqrMagnitude;
				}
				behaviour.machine.PopState();
				behaviour.machine.PushState(2);
			}
			PlayBounceSound(c.transform.position);
		}

		private void OnTriggerEnterEvent(Collider c)
		{
			if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer))
			{
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
				GameState.Current.Actions.RemoveProjectile(behaviour.ID, arg2: true);
			}
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
		private ExplosiveGrenadeQuickItem behaviour;

		private float _timeOut;

		public DeployedState(ExplosiveGrenadeQuickItem behaviour)
		{
			this.behaviour = behaviour;
		}

		public void OnEnter()
		{
			_timeOut = Time.time + (float)behaviour._config.LifeTime;
			ExplosiveGrenadeQuickItem explosiveGrenadeQuickItem = behaviour;
			explosiveGrenadeQuickItem.OnTriggerEnterEvent = (Action<Collider>)Delegate.Combine(explosiveGrenadeQuickItem.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));
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
			ExplosiveGrenadeQuickItem explosiveGrenadeQuickItem = behaviour;
			explosiveGrenadeQuickItem.OnTriggerEnterEvent = (Action<Collider>)Delegate.Remove(explosiveGrenadeQuickItem.OnTriggerEnterEvent, new Action<Collider>(OnTriggerEnterEvent));
		}

		private void OnTriggerEnterEvent(Collider c)
		{
			if (LayerUtil.IsLayerInMask(UberstrikeLayerMasks.GrenadeCollisionMask, c.gameObject.layer))
			{
				behaviour.machine.PopState();
				Singleton<ProjectileManager>.Instance.RemoveProjectile(behaviour.ID);
				GameState.Current.Actions.RemoveProjectile(behaviour.ID, arg2: true);
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
			_config = (ExplosiveGrenadeConfiguration)value;
		}
	}

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

	protected override void OnActivated()
	{
		Vector3 vector = GameState.Current.PlayerData.ShootingPoint + GameState.Current.Player.EyePosition;
		Vector3 position = vector + GameState.Current.PlayerData.ShootingDirection * 2f;
		Vector3 velocity = GameState.Current.PlayerData.ShootingDirection * _config.Speed;
		float distance = 2f;
		if (Physics.Raycast(vector, GameState.Current.PlayerData.ShootingDirection * 2f, out RaycastHit hitInfo, distance, UberstrikeLayerMasks.LocalRocketMask))
		{
			ExplosiveGrenadeQuickItem explosiveGrenadeQuickItem = Throw(hitInfo.point, Vector3.zero) as ExplosiveGrenadeQuickItem;
			explosiveGrenadeQuickItem.machine.PopAllStates();
			explosiveGrenadeQuickItem.OnProjectileExploded = (Action<IGrenadeProjectile>)Delegate.Combine(explosiveGrenadeQuickItem.OnProjectileExploded, (Action<IGrenadeProjectile>)delegate(IGrenadeProjectile p)
			{
				ProjectileDetonator.Explode(p.Position, p.ID, _config.Damage, Vector3.up, _config.SplashRadius, 5, 5, Configuration.ID, UberstrikeItemClass.WeaponLauncher);
			});
			Singleton<ProjectileManager>.Instance.RemoveProjectile(explosiveGrenadeQuickItem.ID);
			GameState.Current.Actions.RemoveProjectile(explosiveGrenadeQuickItem.ID, arg2: true);
		}
		else
		{
			IGrenadeProjectile grenadeProjectile = Throw(position, velocity);
			grenadeProjectile.OnProjectileExploded += delegate(IGrenadeProjectile p)
			{
				ProjectileDetonator.Explode(p.Position, p.ID, _config.Damage, Vector3.up, _config.SplashRadius, 5, 5, Configuration.ID, UberstrikeItemClass.WeaponLauncher);
			};
		}
	}

	public IGrenadeProjectile Throw(Vector3 position, Vector3 velocity)
	{
		ExplosiveGrenadeQuickItem explosiveGrenadeQuickItem = UnityEngine.Object.Instantiate(this) as ExplosiveGrenadeQuickItem;
		if ((bool)explosiveGrenadeQuickItem)
		{
			explosiveGrenadeQuickItem.gameObject.SetActive(value: true);
			for (int i = 0; i < explosiveGrenadeQuickItem.transform.childCount; i++)
			{
				explosiveGrenadeQuickItem.transform.GetChild(i).gameObject.SetActive(value: true);
			}
			explosiveGrenadeQuickItem.Position = position;
			explosiveGrenadeQuickItem.Velocity = velocity;
			explosiveGrenadeQuickItem.collider.material.bounciness = _config.Bounciness;
			explosiveGrenadeQuickItem.machine.RegisterState(1, new FlyingState(explosiveGrenadeQuickItem));
			explosiveGrenadeQuickItem.machine.RegisterState(2, new DeployedState(explosiveGrenadeQuickItem));
			explosiveGrenadeQuickItem.machine.PushState(1);
		}
		if (this.OnProjectileEmitted != null)
		{
			this.OnProjectileEmitted(explosiveGrenadeQuickItem);
		}
		return explosiveGrenadeQuickItem;
	}

	public void SetLayer(UberstrikeLayer layer)
	{
		LayerUtil.SetLayerRecursively(base.transform, layer);
	}

	private void Update()
	{
		machine.Update();
	}

	private void OnGUI()
	{
		if (base.Behaviour.IsCoolingDown && base.Behaviour.FocusTimeRemaining > 0f)
		{
			float num = Mathf.Clamp((float)Screen.height * 0.03f, 10f, 40f);
			float num2 = num * 10f;
			GUI.Label(new Rect(((float)Screen.width - num2) * 0.5f, Screen.height / 2 + 20, num2, num), "Charging Grenade", BlueStonez.label_interparkbold_16pt);
			GUITools.DrawWarmupBar(new Rect(((float)Screen.width - num2) * 0.5f, Screen.height / 2 + 50, num2, num), base.Behaviour.FocusTimeTotal - base.Behaviour.FocusTimeRemaining, base.Behaviour.FocusTimeTotal);
		}
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
			if (_explosionSound != null)
			{
				AutoMonoBehaviour<SfxManager>.Instance.Play3dAudioClip(_explosionSound, base.transform.position);
			}
			if ((bool)_explosionSfx)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(_explosionSfx) as GameObject;
				if ((bool)gameObject)
				{
					gameObject.transform.position = base.transform.position;
					SelfDestroy selfDestroy = gameObject.AddComponent<SelfDestroy>();
					if ((bool)selfDestroy)
					{
						selfDestroy.SetDelay(2f);
					}
				}
			}
			else
			{
				ParticleEffectController.ShowExplosionEffect(ParticleConfigurationType.LauncherDefault, SurfaceEffectType.None, base.transform.position, Vector3.up);
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
