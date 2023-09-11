// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.CharacterInfo
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Types.Attributes;
using Cmune.DataCenter.Common.Entities;
using Cmune.Realtime.Common;
using Cmune.Realtime.Common.Utils;
using Cmune.Util;
using System.Collections.Generic;
using System.Text;
using UberStrike.Core.Types;
using UnityEngine;

namespace UberStrike.Realtime.Common
{
  public class CharacterInfo : ActorInfo
  {
    [CMUNESYNC(1048576)]
    private byte _keys;
    [CMUNESYNC(256)]
    private ushort _moveState;
    [CMUNESYNC(512)]
    private byte _horizontalRotation;
    [CMUNESYNC(1024)]
    private byte _verticalRotation;
    [CMUNESYNC(1073741824)]
    private byte _playerNumber;
    [CMUNESYNC(2097152)]
    private short _health;
    [CMUNESYNC(16384)]
    private StatsInfo _stats = new StatsInfo();
    [CMUNESYNC(67108864)]
    private ArmorInfo _armor = new ArmorInfo();
    [CMUNESYNC(131072)]
    private byte _level;
    [CMUNESYNC(262144)]
    private byte _teamID;
    [CMUNESYNC(16777216)]
    private byte _currentWeaponSlot = 1;
    [CMUNESYNC(33554432)]
    private byte _currentFiringMode;
    [CMUNESYNC(128)]
    private bool _isFiring = false;
    [CMUNESYNC(4194304)]
    private Color _skinColor = Color.white;
    [CMUNESYNC(8388608)]
    private WeaponInfo _weapons = new WeaponInfo();
    [CMUNESYNC(134217728)]
    private List<int> _gear = new List<int>(6);
    [CMUNESYNC(65536)]
    private List<int> _functionalItems = new List<int>(3);
    [CMUNESYNC(4096)]
    private List<int> _quickItems = new List<int>(3);
    [CMUNESYNC(268435456)]
    private bool _isReadyForGame = false;
    [CMUNESYNC(524288)]
    private byte _surfaceSound;
    private Quaternion _realRotation;
    private bool _hasRealRotation = false;
    private Vector3 _standingOffset = new Vector3(0.0f, 0.65f, 0.0f);
    private Vector3 _crouchingOffset = new Vector3(0.0f, 0.1f, 0.0f);
    public Vector3 Position;
    public float TimeStamp;

    public CharacterInfo(SyncObject data)
      : this(string.Empty, 0, ChannelType.WebPortal)
    {
      this.ReadSyncData(data);
    }

    public CharacterInfo()
      : this(string.Empty, 0, ChannelType.WebPortal)
    {
    }

    public CharacterInfo(string name, int actorId, ChannelType channel)
      : base(name, actorId, channel)
    {
      this._gear.AddRange((IEnumerable<int>) new int[7]);
      this._functionalItems.AddRange((IEnumerable<int>) new int[3]);
      this._quickItems.AddRange((IEnumerable<int>) new int[3]);
    }

    public void ResetScore()
    {
      this._stats.Kills = (short) 0;
      this._stats.Deaths = (short) 0;
      this._stats.XP = (ushort) 0;
      this._stats.Points = (ushort) 0;
    }

    public void ResetState()
    {
      this.Cache.Clear();
      this._keys = (byte) 0;
      this._moveState = (ushort) 0;
      this._horizontalRotation = (byte) 0;
      this._verticalRotation = (byte) 0;
      this._health = (short) 100;
      this._currentWeaponSlot = (byte) 1;
      this._currentFiringMode = (byte) 0;
      this._isFiring = false;
      this._isReadyForGame = false;
      this.Armor.Reset();
      this.Weapons.ResetWeaponSlot(WeaponInfo.SlotType.Pickup);
    }

    public bool Is(PlayerStates state) => (this.PlayerState & state) == state;

    public void Set(PlayerStates state) => this.PlayerState |= state;

    public void Set(PlayerStates state, bool b)
    {
      if (b)
        this.PlayerState |= state;
      else
        this.PlayerState &= ~state;
    }

    public void Unset(PlayerStates state) => this.PlayerState &= ~state;

    public float KDR => (float) ((this.Kills > (short) 0 ? (double) this.Kills : 1.0) / (this.Deaths > (short) 0 ? (double) this.Deaths : 1.0));

    public StatsInfo Stats => this._stats;

    public int GetPointBonus() => 10;

    public byte PlayerNumber
    {
      get => this._playerNumber;
      set => this._playerNumber = value;
    }

    public Vector3 CurrentOffset => this.Is(PlayerStates.DUCKED) ? this._crouchingOffset : this._standingOffset;

    public Vector3 ShootingPoint => this.Position + this.CurrentOffset;

    public Vector3 ShootingDirection => this.Rotation * Vector3.forward;

    public KeyState Keys
    {
      get => (KeyState) this._keys;
      set => this._keys = (byte) value;
    }

    public PlayerStates PlayerState
    {
      get => (PlayerStates) this._moveState;
      set => this._moveState = (ushort) value;
    }

    public Quaternion HorizontalRotation
    {
      get => Quaternion.Euler(0.0f, Conversion.Byte2Angle(this._horizontalRotation), 0.0f);
      set
      {
        this.Rotation = value;
        this._horizontalRotation = Conversion.Angle2Byte(value.eulerAngles.y);
      }
    }

    public float VerticalRotation
    {
      get => (float) this._verticalRotation / (float) byte.MaxValue;
      set => this._verticalRotation = (byte) ((double) Mathf.Clamp01(value) * (double) byte.MaxValue);
    }

    public Quaternion Rotation
    {
      private set
      {
        this._hasRealRotation = true;
        this._realRotation = value;
      }
      get => this._hasRealRotation ? this._realRotation : this.HorizontalRotation * Quaternion.AngleAxis((float) ((double) this.VerticalRotation * 180.0 - 90.0), Vector3.left);
    }

    public TeamID TeamID
    {
      get => (TeamID) this._teamID;
      set => this._teamID = (byte) value;
    }

    public ushort XP
    {
      get => this._stats.XP;
      set => this._stats.XP = value;
    }

    public ushort Points
    {
      get => this._stats.Points;
      set => this._stats.Points = value;
    }

    public short Kills
    {
      get => this._stats.Kills;
      set => this._stats.Kills = value;
    }

    public short Deaths
    {
      get => this._stats.Deaths;
      set => this._stats.Deaths = value;
    }

    public int Level
    {
      get => (int) this._level;
      set => this._level = (byte) Mathf.Clamp(value, 0, (int) byte.MaxValue);
    }

    public short Health
    {
      get => this._health;
      set
      {
        if (value < (short) -100)
          this._health = (short) -100;
        else if (value > (short) 200)
          this._health = (short) 200;
        else
          this._health = value;
      }
    }

    public bool IsFiring
    {
      get => this._isFiring;
      set => this._isFiring = value;
    }

    public bool IsUnderWater => this.PlayerState == PlayerStates.SWIMMING || this.PlayerState == PlayerStates.DIVING;

    public byte CurrentWeaponSlot
    {
      get => this._currentWeaponSlot;
      set => this._currentWeaponSlot = value;
    }

    public FireMode CurrentFiringMode
    {
      get => (FireMode) this._currentFiringMode;
      set => this._currentFiringMode = (byte) value;
    }

    public WeaponInfo Weapons
    {
      get => this._weapons;
      set => this._weapons = value;
    }

    public ArmorInfo Armor
    {
      get => this._armor;
      set => this._armor = value;
    }

    public Color SkinColor
    {
      get => this._skinColor;
      set => this._skinColor = value;
    }

    public List<int> Gear
    {
      get => this._gear;
      set => this._gear = value;
    }

    public List<int> FunctionalItems
    {
      get => this._functionalItems;
      set => this._functionalItems = value;
    }

    public List<int> QuickItems
    {
      get => this._quickItems;
      set => this._quickItems = value;
    }

    public bool IsReadyForGame
    {
      get => this._isReadyForGame;
      set => this._isReadyForGame = value;
    }

    public SurfaceType SurfaceSound
    {
      get => (SurfaceType) this._surfaceSound;
      set => this._surfaceSound = (byte) value;
    }

    public int CurrentWeaponID => this._weapons.ItemIDs[(int) this._currentWeaponSlot];

    public UberstrikeItemClass CurrentWeaponCategory => (UberstrikeItemClass) this._weapons.Categories[(int) this._currentWeaponSlot];

    public bool IsAlive => this._health > (short) 0;

    public float Velocity { get; set; }

    public float Distance { get; set; }

    public bool IsSpectator => this.Is(PlayerStates.SPECTATOR);

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(base.ToString());
      stringBuilder.AppendLine("Number: " + (object) this._playerNumber);
      stringBuilder.AppendLine("Keys: " + CmunePrint.Flag(this._keys));
      stringBuilder.AppendLine("State: " + CmunePrint.Flag<PlayerStates>(this._moveState));
      stringBuilder.AppendLine("Health: " + (object) this._health);
      stringBuilder.AppendLine("Armor: " + (object) this._armor);
      stringBuilder.AppendLine("IsFiring: " + (object) this._isFiring);
      stringBuilder.AppendLine("TeamID: " + (object) this._teamID);
      stringBuilder.AppendLine("Weapon: " + (object) this._currentWeaponSlot);
      stringBuilder.AppendLine("Mode: " + (object) this._currentFiringMode);
      stringBuilder.AppendLine("Rotation: " + string.Format("{0}/{1}", (object) this._horizontalRotation, (object) this._verticalRotation));
      stringBuilder.AppendLine("Weapons: " + this._weapons.CategoriesToString());
      stringBuilder.AppendLine("Gear: " + CmunePrint.Values((object) this._gear));
      stringBuilder.AppendLine("Funcs: " + CmunePrint.Values((object) this._functionalItems));
      return stringBuilder.ToString();
    }

    [ExtendableEnumBounds(128, 1073741824)]
    public new class FieldTag : ActorInfo.FieldTag
    {
      public const int IsFiring = 128;
      public const int MoveState = 256;
      public const int HorizontalRotation = 512;
      public const int VerticalRotation = 1024;
      public const int QuickItems = 4096;
      public const int Stats = 16384;
      public const int FunctionalItems = 65536;
      public const int Level = 131072;
      public const int TeamID = 262144;
      public const int SurfaceSound = 524288;
      public const int Keys = 1048576;
      public const int Health = 2097152;
      public const int SkinColor = 4194304;
      public const int Weapons = 8388608;
      public const int CurrentWeaponSlot = 16777216;
      public const int CurrentFiringMode = 33554432;
      public const int Armor = 67108864;
      public const int Gear = 134217728;
      public const int ReadyForGame = 268435456;
      public const int PlayerNumber = 1073741824;
    }
  }
}
