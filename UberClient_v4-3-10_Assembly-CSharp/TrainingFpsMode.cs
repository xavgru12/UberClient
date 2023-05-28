// Decompiled with JetBrains decompiler
// Type: TrainingFpsMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

[NetworkClass(-1)]
public class TrainingFpsMode : FpsGameMode
{
  public TrainingFpsMode(RemoteMethodInterface rmi, int mapId)
    : base(rmi, new GameMetaData(mapId, string.Empty, 120, 0, (short) 109))
  {
  }

  protected override void OnInitialized()
  {
    base.OnInitialized();
    MonoRoutine.Run(new MonoRoutine.FunctionVoid(this.StartDecreasingHealthAndArmor));
    MonoRoutine.Run(new MonoRoutine.FunctionVoid(this.SimulateGameFrameUpdate));
  }

  protected override void OnCharacterLoaded() => this.OnSetNextSpawnPoint(Random.Range(0, Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(GameMode.Training, TeamID.NONE)), 0);

  protected override void OnModeInitialized()
  {
    this.OnPlayerJoined(SyncObjectBuilder.GetSyncData((CmuneDeltaSync) GameState.LocalCharacter, true), Vector3.zero);
    this.IsMatchRunning = true;
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
    if (!this.MyCharacterState.Info.IsAlive)
      return;
    byte angle = Conversion.Angle2Byte(Vector3.Angle(Vector3.forward, force));
    this.MyCharacterState.Info.Health -= this.MyCharacterState.Info.Armor.AbsorbDamage(damage, part);
    Singleton<DamageFeedbackHud>.Instance.AddDamageMark(Mathf.Clamp01((float) damage / 50f), Conversion.Byte2Angle(angle));
    if (ApplicationDataManager.ApplicationOptions.VideoBloomHitEffect)
      RenderSettingsController.Instance.ShowAgonyTint((float) damage / 50f);
    Singleton<HpApHud>.Instance.HP = (int) GameState.LocalCharacter.Health;
    Singleton<HpApHud>.Instance.AP = GameState.LocalCharacter.Armor.ArmorPoints;
    GameState.LocalPlayer.MoveController.ApplyForce(force, CharacterMoveController.ForceType.Additive);
  }

  protected override void ApplyCurrentGameFrameUpdates(SyncObject delta)
  {
    base.ApplyCurrentGameFrameUpdates(delta);
    if (!delta.Contains(2097152) || GameState.LocalCharacter.IsAlive)
      return;
    this.OnSetNextSpawnPoint(Random.Range(0, Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(GameMode.Training, TeamID.NONE)), 3);
  }

  public override void RequestRespawn() => this.OnSetNextSpawnPoint(Random.Range(0, Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(GameMode.Training, TeamID.NONE)), 3);

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
        switch (value)
        {
          case 5:
          case 100:
            if (GameState.LocalCharacter.Health >= (short) 200)
              return;
            GameState.LocalCharacter.Health = (short) Mathf.Clamp((int) GameState.LocalCharacter.Health + value, 0, 200);
            return;
          case 25:
          case 50:
            if (GameState.LocalCharacter.Health >= (short) 100)
              return;
            GameState.LocalCharacter.Health = (short) Mathf.Clamp((int) GameState.LocalCharacter.Health + value, 0, 100);
            return;
          default:
            return;
        }
    }
  }

  [DebuggerHidden]
  private IEnumerator StartDecreasingHealthAndArmor() => (IEnumerator) new TrainingFpsMode.\u003CStartDecreasingHealthAndArmor\u003Ec__Iterator24()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator SimulateGameFrameUpdate() => (IEnumerator) new TrainingFpsMode.\u003CSimulateGameFrameUpdate\u003Ec__Iterator25()
  {
    \u003C\u003Ef__this = this
  };

  public override float GameTime => Time.realtimeSinceStartup;
}
