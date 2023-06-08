using System;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PlayerData : ICharacterState
{
	public readonly PlayerActions Actions = new PlayerActions();

	private float posSyncFrame;

	private float shotSyncFrame;

	public IntegerProperty Health = new IntegerProperty(0, 0, 200);

	public IntegerProperty ArmorPoints = new IntegerProperty(0, 0, 200);

	public IntegerProperty ArmorCarried = new IntegerProperty(0, 0, 200);

	public IntegerProperty RemainingTime = new IntegerProperty(0, 0);

	public IntegerProperty RemainingKills = new IntegerProperty(0, 0);

	public Property<TeamID> Team = new Property<TeamID>(TeamID.NONE);

	public IntegerProperty BlueTeamScore = new IntegerProperty(0, 0);

	public IntegerProperty RedTeamScore = new IntegerProperty(0, 0);

	public Property<Dictionary<LoadoutSlotType, IUnityItem>> LoadoutWeapons = new Property<Dictionary<LoadoutSlotType, IUnityItem>>();

	public Property<WeaponSlot> ActiveWeapon = new Property<WeaponSlot>();

	public Property<WeaponSlot> NextActiveWeapon = new Property<WeaponSlot>();

	public IntegerProperty Ammo = new IntegerProperty(0, 0);

	public Property<WeaponSlot> WeaponFired = new Property<WeaponSlot>();

	public Property<TeamID> FocusedPlayerTeam = new Property<TeamID>(TeamID.NONE);

	public Property<DamageInfo> AppliedDamage = new Property<DamageInfo>();

	public Property<bool> IsIronSighted = new Property<bool>(defaultValue: false);

	public Property<bool> IsZoomedIn = new Property<bool>(defaultValue: false);

	public static readonly Vector3 StandingOffset = new Vector3(0f, 0.65f, 0f);

	public static readonly Vector3 CrouchingOffset = new Vector3(0f, 0.1f, 0f);

	private MovementUpdateCache cache = new MovementUpdateCache();

	public int ArmorPointCapacity => Player.ArmorPointCapacity;

	public PlayerStates PlayerState => Player.PlayerState;

	public MoveStates MovementState
	{
		get;
		set;
	}

	public KeyState KeyState
	{
		get;
		private set;
	}

	public Vector3 Velocity
	{
		get;
		set;
	}

	public Vector3 Position
	{
		get
		{
			if (GameState.Current.Player != null)
			{
				return GameState.Current.Player.transform.position;
			}
			return Vector3.zero;
		}
	}

	public Quaternion HorizontalRotation => UserInputt.HorizontalRotation;

	public float VerticalRotation
	{
		get
		{
			Vector3 eulerAngles = UserInputt.VerticalRotation.eulerAngles;
			return eulerAngles.x;
		}
	}

	public bool IsOnline => !Is(PlayerStates.Offline);

	public bool IsAlive => (PlayerState & PlayerStates.Dead) == 0;

	public bool IsSpectator => (PlayerState & PlayerStates.Spectator) != 0;

	public bool IsUnderWater => (MovementState & (MoveStates.Swimming | MoveStates.Diving)) != 0;

	public Vector3 CurrentOffset
	{
		get
		{
			if ((MovementState & MoveStates.Ducked) == 0)
			{
				return StandingOffset;
			}
			return CrouchingOffset;
		}
	}

	public Vector3 ShootingPoint => GameState.Current.Player.transform.position + CurrentOffset;

	public Vector3 ShootingDirection => UserInputt.Rotation * Vector3.forward;

	public GameActorInfo Player
	{
		get;
		set;
	}

	public event Action<GameActorInfoDelta> OnDeltaUpdate = delegate
	{
	};

	public event Action<PlayerMovement> OnPositionUpdate = delegate
	{
	};

	public event Action<Vector3> SendJumpUpdate = delegate
	{
	};

	public event Action<ShortVector3, ShortVector3, byte, byte, byte> SendMovementUpdate = delegate
	{
	};

	public PlayerData()
	{
		Player = new GameActorInfo();
		Health.AddEvent(delegate(int el)
		{
			Player.Health = (short)el;
		}, null);
		ArmorPoints.AddEvent(delegate(int el)
		{
			Player.ArmorPoints = (byte)el;
		}, null);
	}

	public void Reset()
	{
		int armorPoints = 0;
		Singleton<LoadoutManager>.Instance.GetArmorValues(out armorPoints);
		Player.ArmorPointCapacity = (byte)armorPoints;
		Player.PlayerState = PlayerStates.None;
		KeyState = KeyState.Still;
		MovementState = MoveStates.None;
		Velocity = Vector3.zero;
		Health.Value = 100;
		ArmorPoints.Value = armorPoints;
	}

	public void InitializePlayer()
	{
		Reset();
	}

	public void SwitchWeaponSlot(int slot)
	{
		Actions.SwitchWeapon((byte)slot);
		Player.CurrentWeaponSlot = (byte)slot;
	}

	public void SetPing(int ping)
	{
		if (Player.Ping != ping)
		{
			Player.Ping = (ushort)ping;
			Actions.UpdatePing(Player.Ping);
		}
	}

	public float GetAbsorptionRate()
	{
		return 0.66f;
	}

	public void GetArmorDamage(short damage, BodyPart part, out short healthDamage, out byte armorDamage)
	{
		if ((int)ArmorPoints > 0)
		{
			int value = Mathf.CeilToInt(GetAbsorptionRate() * (float)damage);
			armorDamage = (byte)Mathf.Clamp(value, 0, ArmorPoints);
			healthDamage = (short)(damage - armorDamage);
		}
		else
		{
			armorDamage = 0;
			healthDamage = damage;
		}
	}

	public void Set(MoveStates state, bool on)
	{
		if (on)
		{
			MovementState |= state;
		}
		else
		{
			MovementState &= (MoveStates)(byte)(~(uint)state);
		}
	}

	public void Set(PlayerStates state, bool on)
	{
		if (on)
		{
			Player.PlayerState |= state;
			switch (state)
			{
			case PlayerStates.Paused:
				Actions.PausePlayer(obj: true);
				break;
			case PlayerStates.Sniping:
				Actions.SniperMode(obj: true);
				break;
			case PlayerStates.Shooting:
				Actions.AutomaticFire(obj: true);
				break;
			}
		}
		else
		{
			Player.PlayerState &= (PlayerStates)(byte)(~(uint)state);
			switch (state)
			{
			case PlayerStates.Paused:
				Actions.PausePlayer(obj: false);
				break;
			case PlayerStates.Sniping:
				Actions.SniperMode(obj: false);
				break;
			case PlayerStates.Shooting:
				Actions.AutomaticFire(obj: false);
				break;
			}
		}
	}

	public void Set(KeyState state, bool on)
	{
		if (on && !Is(state))
		{
			KeyState |= state;
			Actions.UpdateKeyState((byte)KeyState);
		}
		else if (!on && Is(state))
		{
			KeyState &= (KeyState)(byte)(~(uint)state);
			Actions.UpdateKeyState((byte)KeyState);
		}
	}

	public void ResetKeys()
	{
		if (KeyState != 0)
		{
			KeyState = KeyState.Still;
			Actions.UpdateKeyState((byte)KeyState);
		}
	}

	public bool Is(MoveStates state)
	{
		return (MovementState & state) != 0;
	}

	public bool Is(PlayerStates state)
	{
		return (PlayerState & state) != 0;
	}

	public bool Is(KeyState state)
	{
		return (KeyState & state) != 0;
	}

	public void DeltaUpdate(GameActorInfoDelta delta)
	{
		foreach (KeyValuePair<GameActorInfoDelta.Keys, object> change in delta.Changes)
		{
			switch (change.Key)
			{
			case GameActorInfoDelta.Keys.Health:
				Health.Value = (short)change.Value;
				break;
			case GameActorInfoDelta.Keys.PlayerState:
				Player.PlayerState = (PlayerStates)(byte)change.Value;
				break;
			case GameActorInfoDelta.Keys.ArmorPoints:
				ArmorPoints.Value = (byte)change.Value;
				break;
			}
		}
		delta.Apply(Player);
		this.OnDeltaUpdate(delta);
	}

	public void PositionUpdate(PlayerMovement update, ushort gameFrame)
	{
	}

	public void LandingUpdate()
	{
		posSyncFrame = Time.realtimeSinceStartup + 0.05f;
		byte arg = (byte)(MovementState | MoveStates.Landed | MoveStates.Grounded);
		Action<ShortVector3, ShortVector3, byte, byte, byte> sendMovementUpdate = this.SendMovementUpdate;
		ShortVector3 arg2 = GameState.Current.Player.transform.position;
		ShortVector3 arg3 = GameState.Current.Player.MoveController.Velocity;
		Vector3 eulerAngles = HorizontalRotation.eulerAngles;
		sendMovementUpdate(arg2, arg3, Conversion.Angle2Byte(eulerAngles.y), Conversion.Angle2Byte(VerticalRotation), arg);
	}

	public void JumpingUpdate()
	{
		Set(MoveStates.Jumping, on: true);
		if (GameState.Current.Player != null)
		{
			this.SendJumpUpdate(GameState.Current.Player.transform.position);
		}
	}

	public void SendUpdates()
	{
		if (GameState.Current.IsInGame && IsAlive && posSyncFrame <= Time.realtimeSinceStartup && this.SendMovementUpdate != null)
		{
			posSyncFrame = Time.realtimeSinceStartup + 0.05f;
			MovementUpdateCache movementUpdateCache = cache;
			Vector3 position = GameState.Current.Player.transform.position;
			Vector3 eulerAngles = HorizontalRotation.eulerAngles;
			if (movementUpdateCache.Update(position, Conversion.Angle2Byte(eulerAngles.y), Conversion.Angle2Byte(VerticalRotation), (byte)MovementState))
			{
				this.SendMovementUpdate(cache.Position, GameState.Current.Player.MoveController.Velocity, cache.HRotation, cache.VRotation, cache.MovementState);
				Singleton<GameStateController>.Instance.Client.Peer.SendOutgoingCommands();
			}
		}
	}
}
