using System;
using System.Collections;
using UberStrike.Core.Models;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class LocalPlayer : MonoBehaviour
{
	private const float RundownThreshold = -300f;

	private const float GroundedThreshold = 0.5f;

	[SerializeField]
	private Transform _cameraTarget;

	[SerializeField]
	private Transform _weaponAttachPoint;

	[SerializeField]
	private WeaponCamera _weaponCamera;

	private bool _didPlayTargetSound;

	private float _damageFactor;

	private float _damageFactorDuration;

	private float _lastGrounded;

	public CharacterConfig Character
	{
		get;
		private set;
	}

	public float DamageFactor
	{
		get
		{
			return _damageFactor;
		}
		set
		{
			_damageFactor = Mathf.Clamp01(value);
			_damageFactorDuration = _damageFactor * 15f;
		}
	}

	public bool IsWalkingEnabled
	{
		get;
		set;
	}

	public bool EnableWeaponControl
	{
		get;
		set;
	}

	public CharacterMoveController MoveController
	{
		get;
		private set;
	}

	public WeaponCamera WeaponCamera => _weaponCamera;

	public Transform WeaponAttachPoint => _weaponAttachPoint;

	public Transform CameraTarget => _cameraTarget;

	public Vector3 EyePosition => new Vector3(0f, -0.2f, 0f);

	private void Awake()
	{
		MoveController = new CharacterMoveController(GetComponent<CharacterController>());
		MoveController.CharacterLanded += OnCharacterGrounded;
		IsWalkingEnabled = true;
	}

	private void OnEnable()
	{
		MoveController.Init();
		StartCoroutine(StartPlayerIdentification());
		StartCoroutine(StartUpdatePlayerPingTime(5));
	}

	private void OnDisable()
	{
		Screen.lockCursor = false;
		AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
	}

	private void Update()
	{
		_cameraTarget.localPosition = Vector3.Lerp(_cameraTarget.localPosition, GameState.Current.PlayerData.CurrentOffset, 10f * Time.deltaTime);
		if (_damageFactor != 0f)
		{
			if (_damageFactorDuration > 0f)
			{
				_damageFactorDuration -= Time.deltaTime;
			}
			if (_damageFactorDuration <= 0f || !GameState.Current.PlayerData.IsAlive)
			{
				_damageFactor = 0f;
				_damageFactorDuration = 0f;
			}
		}
		UpdateCameraBob();
		if (AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled)
		{
			if (Screen.lockCursor)
			{
				UserInputt.UpdateMouse();
			}
			UserInputt.UpdateDirections();
			if (Screen.lockCursor)
			{
				UpdateRotation();
			}
		}
		else
		{
			UserInputt.ResetDirection();
		}
	}

	private void LateUpdate()
	{
		Singleton<WeaponController>.Instance.LateUpdate();
	}

	private void UpdateRotation()
	{
		_cameraTarget.localRotation = UserInputt.Rotation;
	}

	private IEnumerator StartPlayerIdentification()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.3f);
			if (GameState.Current.IsPlayerPaused)
			{
				continue;
			}
			Vector3 vector = GameState.Current.PlayerData.ShootingPoint + GameState.Current.Player.EyePosition;
			Vector3 end = vector + GameState.Current.PlayerData.ShootingDirection * 1000f;
			if (Physics.Linecast(vector, end, out RaycastHit hitInfo, UberstrikeLayerMasks.IdentificationMask))
			{
				CharacterHitArea component = hitInfo.collider.GetComponent<CharacterHitArea>();
				if ((bool)component && component.Shootable != null && !component.Shootable.IsLocal)
				{
					CharacterConfig characterConfig = (CharacterConfig)component.Shootable;
					characterConfig.AimTrigger.HudInfo.Show();
					GameState.Current.PlayerData.FocusedPlayerTeam.Value = characterConfig.Team;
					if (!_didPlayTargetSound)
					{
						AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.FocusEnemy, 0uL);
						_didPlayTargetSound = true;
					}
				}
				else
				{
					GameState.Current.PlayerData.FocusedPlayerTeam.Value = TeamID.NONE;
					_didPlayTargetSound = false;
				}
			}
			else
			{
				GameState.Current.PlayerData.FocusedPlayerTeam.Value = TeamID.NONE;
				_didPlayTargetSound = false;
			}
		}
	}

	private IEnumerator StartUpdatePlayerPingTime(int sec)
	{
		while (true)
		{
			GameState.Current.PlayerData.SetPing(Singleton<GameStateController>.Instance.Client.Ping);
			yield return new WaitForSeconds(sec);
		}
	}

	private void OnCharacterGrounded(float velocity)
	{
		if (!GameState.Current.HasJoinedGame || !GameState.Current.IsInGame || WeaponFeedbackManager.IsBobbing || !(_lastGrounded + 0.5f < Time.time) || GameState.Current.PlayerData.Is(MoveStates.Diving))
		{
			return;
		}
		_lastGrounded = Time.time;
		if (Character != null && Character.Avatar != null && Character.Avatar.Decorator != null)
		{
			Character.Avatar.Decorator.PlayFootSound(Character.WalkingSoundSpeed);
			if (velocity < -20f)
			{
				LevelCamera.DoLandFeedback(shake: true);
			}
			else
			{
				LevelCamera.DoLandFeedback(shake: false);
			}
		}
	}

	private void UpdateCameraBob()
	{
		switch (GameState.Current.PlayerData.MovementState)
		{
		case MoveStates.Swimming:
			WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.Swim);
			return;
		case MoveStates.Grounded | MoveStates.Ducked:
			if (UserInputt.IsWalking)
			{
				if (Singleton<WeaponController>.Instance.IsSecondaryAction)
				{
					WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.None);
				}
				else
				{
					WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.Crouch);
				}
			}
			else if (Singleton<WeaponController>.Instance.IsSecondaryAction)
			{
				WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.None);
			}
			else
			{
				WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.Idle);
			}
			return;
		case MoveStates.Flying:
			WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.Fly);
			return;
		case MoveStates.Grounded:
			if (UserInputt.IsWalking)
			{
				if (Singleton<WeaponController>.Instance.IsSecondaryAction)
				{
					WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.None);
				}
				else
				{
					WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.Run);
				}
				return;
			}
			if (Singleton<WeaponController>.Instance.IsSecondaryAction)
			{
				WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.None);
				return;
			}
			if (UserInputt.IsWalking)
			{
				Vector3 velocity = MoveController.Velocity;
				if (!(velocity.y < -300f))
				{
					return;
				}
			}
			WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.Idle);
			return;
		}
		if (UserInputt.IsWalking)
		{
			Vector3 velocity2 = MoveController.Velocity;
			if (!(velocity2.y < -300f))
			{
				return;
			}
		}
		WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.None);
	}

	public void InitializeWeapons()
	{
		Singleton<WeaponController>.Instance.InitializeAllWeapons(_weaponAttachPoint);
	}

	public void InitializePlayer()
	{
		try
		{
			InitializeWeapons();
			WeaponFeedbackManager.SetBobMode(LevelCamera.BobMode.None);
			LevelCamera.EnableLowPassFilter(enabled: false);
			UpdateRotation();
			MoveController.Start();
			MoveController.ResetDuckMode();
			Singleton<QuickItemController>.Instance.Reset();
			GameState.Current.PlayerData.InitializePlayer();
			DamageFactor = 0f;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	public void SpawnPlayerAt(Vector3 pos, Quaternion rot)
	{
		try
		{
			base.transform.position = pos + Vector3.up;
			_cameraTarget.localRotation = rot;
			Vector3 eulerAngles = rot.eulerAngles;
			UserInputt.SetRotation(eulerAngles.y);
			LevelCamera.ResetFeedback();
			MoveController.ResetEnviroment();
			MoveController.Platform = null;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	public void ResetShopCharPos(Quaternion rot)
	{
		_cameraTarget.localRotation = rot;
		Vector3 eulerAngles = rot.eulerAngles;
		UserInputt.SetRotation(rot.eulerAngles.y);
	}

	public void SetCurrentCharacterConfig(CharacterConfig character)
	{
		Character = character;
	}

	public void SetEnabled(bool enabled)
	{
		base.gameObject.SetActive(enabled);
	}
}
