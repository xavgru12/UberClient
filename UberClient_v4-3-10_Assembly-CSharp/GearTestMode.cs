// Decompiled with JetBrains decompiler
// Type: GearTestMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

[NetworkClass(-1)]
public class GearTestMode : FpsGameMode
{
  public GearTestMode(RemoteMethodInterface rmi)
    : base(rmi, new GameMetaData(0, string.Empty, 120, 0, (short) 0))
  {
  }

  protected override void OnInitialized()
  {
    base.OnInitialized();
    MonoRoutine.Run(new MonoRoutine.FunctionVoid(this.StartDecreasingHealthAndArmor));
    MonoRoutine.Run(new MonoRoutine.FunctionVoid(this.SimulateGameFrameUpdate));
    Singleton<ArmorHud>.Instance.Enabled = false;
  }

  protected override void OnCharacterLoaded()
  {
    Vector3 position;
    Quaternion rotation;
    Singleton<SpawnPointManager>.Instance.GetSpawnPointAt(0, GameMode.TryWeapon, TeamID.NONE, out position, out rotation);
    this.SpawnPlayerAt(position, rotation);
    if (!((Object) LevelCamera.Instance != (Object) null))
      return;
    LevelCamera.Instance.MainCamera.rect = new Rect(0.0f, 0.0f, 1f, 1f);
  }

  protected override void OnModeInitialized()
  {
    this.OnPlayerJoined(SyncObjectBuilder.GetSyncData((CmuneDeltaSync) GameState.LocalCharacter, true), Vector3.zero);
    this.IsMatchRunning = true;
    GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Playing);
  }

  public override void PlayerHit(
    int targetPlayer,
    short damage,
    BodyPart part,
    Vector3 force,
    int shotCount,
    int weaponID,
    UberstrikeItemClass weaponClass,
    DamageEffectType damageEffectFlag,
    float damageEffectValue)
  {
    GameState.LocalPlayer.MoveController.ApplyForce(force, CharacterMoveController.ForceType.Additive);
  }

  protected override void ApplyCurrentGameFrameUpdates(SyncObject delta)
  {
    base.ApplyCurrentGameFrameUpdates(delta);
    if (!delta.Contains(2097152) || GameState.LocalCharacter.IsAlive)
      return;
    this.OnSetNextSpawnPoint(Random.Range(0, Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(GameMode.TryWeapon, TeamID.NONE)), 3);
  }

  public override void RequestRespawn() => this.OnSetNextSpawnPoint(Random.Range(0, Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(GameMode.TryWeapon, TeamID.NONE)), 3);

  public override void IncreaseHealthAndArmor(int health, int armor)
  {
    UberStrike.Realtime.UnitySdk.CharacterInfo localCharacter = GameState.LocalCharacter;
    if (health > 0 && localCharacter.Health < (short) 200)
      localCharacter.Health = (short) Mathf.Clamp((int) localCharacter.Health + health, 0, 200);
    if (armor <= 0 || localCharacter.Armor.ArmorPoints >= 200)
      return;
    localCharacter.Armor.ArmorPoints = Mathf.Clamp(localCharacter.Armor.ArmorPoints + armor, 0, 200);
  }

  public override void PickupPowerup(int pickupID, PickupItemType type, int value)
  {
    switch (type)
    {
      case PickupItemType.Armor:
        GameState.LocalCharacter.Armor.ArmorPoints += value;
        break;
      case PickupItemType.Health:
        GameState.LocalCharacter.Health += (short) value;
        break;
    }
  }

  [DebuggerHidden]
  private IEnumerator StartDecreasingHealthAndArmor() => (IEnumerator) new GearTestMode.\u003CStartDecreasingHealthAndArmor\u003Ec__Iterator20()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator SimulateGameFrameUpdate() => (IEnumerator) new GearTestMode.\u003CSimulateGameFrameUpdate\u003Ec__Iterator21()
  {
    \u003C\u003Ef__this = this
  };
}
