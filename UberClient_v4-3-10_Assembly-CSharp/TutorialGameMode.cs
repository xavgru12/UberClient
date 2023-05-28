// Decompiled with JetBrains decompiler
// Type: TutorialGameMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

[NetworkClass(-1)]
public class TutorialGameMode : FpsGameMode, ITutorialCinematicSequenceListener
{
  private AvatarDecorator _airlockNPC;
  private TutorialCinematicSequence _sequence;
  private TutorialShootingTargetController _shootingRangeController;
  private float _fadeInAlpha = 1f;
  private string _subtitle = string.Empty;
  private Vector2 _scale = Vector2.one;
  private MeshGUIText _txtObjectives;
  private MeshGUIText _txtObjUnderscore;
  private MeshGUIText _txtMouseLook;
  private MeshGUIText _txtWasdWalk;
  private MeshGUIText _txtToArmory;
  private MeshGUIText _txtPickupMG;
  private MeshGUIText _txtShoot3;
  private MeshGUIText _txtShoot6;
  private MeshGUIText _txtComplete;
  private ObjectiveTick _objMouseMove;
  private ObjectiveTick _objWasdWalk;
  private ObjectiveTick _objGotoArmory;
  private ObjectiveTick _objPickupWeapon;
  private ObjectiveTick _objShootTarget3;
  private ObjectiveTick _objShootTarget6;
  private bool _showObjective;
  private bool _showObjMouse;
  private bool _showObjWasdWalk;
  private bool _showGotoArmory;
  private float _blackBarHeight;
  private Rect _mousePos;
  private float _mouseXOffset;
  private KeyState[] _keys = new KeyState[4]
  {
    KeyState.Forward,
    KeyState.Left,
    KeyState.Backward,
    KeyState.Right
  };

  public TutorialGameMode(RemoteMethodInterface rmi)
    : base(rmi, new GameMetaData(0, string.Empty, 120, 0, (short) 108))
  {
    this._sequence = new TutorialCinematicSequence((ITutorialCinematicSequenceListener) this);
    Singleton<LoadoutManager>.Instance.SetLoadoutWeapons(new int[4]);
    MonoRoutine.Start(this.StartTutorialMode());
  }

  public TutorialCinematicSequence Sequence => this._sequence;

  public ObjectiveTick ObjShootTarget3 => this._objShootTarget3;

  public ObjectiveTick ObjShootTarget6 => this._objShootTarget6;

  public void DrawGui()
  {
    GUI.depth = 100;
    this.DrawFadeInRect();
    this.DrawSubtitle();
    if (LevelTutorial.Instance.ShowObjectives)
      this.DrawObjectives();
    if (LevelTutorial.Instance.ShowObjComplete)
      this._txtComplete.Draw((float) (((double) Screen.width - (double) this._txtComplete.Size.x) / 2.0), (float) (((double) Screen.height - (double) this._txtComplete.Size.y) / 2.0));
    this.PlayAvatarAnimation();
    if (!LevelTutorial.Instance.IsCinematic)
      return;
    this.DrawBlackBars();
  }

  private void DrawFadeInRect()
  {
    if ((double) this._fadeInAlpha <= 0.0)
      return;
    this._fadeInAlpha = Mathf.Lerp(this._fadeInAlpha, 0.0f, Time.deltaTime);
    if (Mathf.Approximately(0.0f, this._fadeInAlpha))
      this._fadeInAlpha = 0.0f;
    GUI.color = new Color(1f, 1f, 1f, this._fadeInAlpha);
    GUI.Label(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), GUIContent.none, BlueStonez.box_black);
    GUI.color = Color.white;
  }

  private void DrawSubtitle()
  {
    GUI.color = Color.black;
    GUI.Label(new Rect(1f, (float) (Screen.height - 149), (float) Screen.width, 80f), this._subtitle, BlueStonez.label_interparkbold_18pt);
    GUI.color = Color.white;
    GUI.Label(new Rect(0.0f, (float) (Screen.height - 150), (float) Screen.width, 80f), this._subtitle, BlueStonez.label_interparkbold_18pt);
  }

  private void PlayAvatarAnimation()
  {
    switch (this._sequence.State)
    {
      case TutorialCinematicSequence.TutorialState.AirlockCameraZoomIn:
      case TutorialCinematicSequence.TutorialState.AirlockCameraReady:
      case TutorialCinematicSequence.TutorialState.AirlockWelcome:
      case TutorialCinematicSequence.TutorialState.AirlockMouseLookSubtitle:
      case TutorialCinematicSequence.TutorialState.AirlockMouseLook:
      case TutorialCinematicSequence.TutorialState.AirlockWasdSubtitle:
      case TutorialCinematicSequence.TutorialState.AirlockWasdWalk:
        GameState.LocalDecorator.AnimationController.PlayAnimation(AnimationIndex.HomeNoWeaponIdle);
        break;
      case TutorialCinematicSequence.TutorialState.ArmoryPickupMG:
      case TutorialCinematicSequence.TutorialState.TutorialEnd:
        GameState.LocalDecorator.AnimationController.PlayAnimation(AnimationIndex.HomeNoWeaponnLookAround);
        break;
    }
  }

  private void DrawObjectives()
  {
    float num1 = Mathf.Clamp((float) ((double) Screen.height * 0.5 / 1000.0), 0.35f, 0.4f);
    float scale = (float) Screen.height / 1000f;
    float a = (float) Screen.width / 4f;
    float b = (float) Screen.height / 4f;
    float num2 = Mathf.Clamp(Mathf.Min(a, b) / 2f, 20f, 68f);
    float num3 = num2 / 5f;
    Rect position1 = new Rect(Mathf.Clamp((float) (Screen.width / 6 - 70), 0.0f, float.PositiveInfinity), (float) (((double) Screen.height - (double) b) / 2.0 - 40.0), (float) ((double) num2 * 3.0 + (double) num3 * 2.0), num2 * 2f + num3);
    Rect position2 = position1;
    position2.x = (float) ((double) Screen.width - (double) position2.x - (double) position2.width - 140.0);
    position2.height = position2.width * 0.8f;
    this._scale.x = num1;
    this._scale.y = num1;
    GUI.BeginGroup(new Rect(70f, 40f, (float) (Screen.width - 70), (float) (Screen.height - 40)));
    if (this._showObjective)
    {
      this._txtObjectives.Scale = this._scale * 1.5f;
      this._txtObjectives.Position = new Vector2(70f, 36f);
      this._txtObjUnderscore.Scale = this._scale * 1.5f;
      this._txtObjUnderscore.Position = new Vector2(73f + this._txtObjectives.Size.x, 36f);
      this._txtObjUnderscore.Alpha = (double) Mathf.Sin(Time.time * 4f) <= 0.0 ? 0.0f : 1f;
      this._txtObjectives.Draw(0.0f, 0.0f);
    }
    if (this._showObjMouse)
    {
      this._txtMouseLook.Scale = this._scale;
      this._txtMouseLook.Position = new Vector2(70f, 40f);
      this._txtMouseLook.Draw(0.0f, 58f);
      this._objMouseMove.Draw(new Vector2(this._txtMouseLook.Size.x + 5f, 38f), scale);
      GUI.BeginGroup(position2);
      float width = position2.height * (float) LevelTutorial.Instance.ImgMouse.width / (float) LevelTutorial.Instance.ImgMouse.height;
      Rect rect = new Rect((float) (((double) position2.width - (double) width) / 2.0), width / 4f, width, position2.height);
      GUIUtility.RotateAroundPivot(-17f, new Vector2(rect.x, rect.y));
      GUI.Label(new Rect(rect.x + this._mouseXOffset, rect.y, rect.width, rect.height), LevelTutorial.Instance.ImgMouse);
      GUI.matrix = Matrix4x4.identity;
      GUI.EndGroup();
    }
    if (this._showObjWasdWalk && GameState.HasCurrentPlayer)
    {
      this._txtWasdWalk.Scale = this._scale;
      this._txtWasdWalk.Position = new Vector2(70f, 40f);
      this._txtWasdWalk.Draw(0.0f, 64f);
      this._objWasdWalk.Draw(new Vector2(this._txtWasdWalk.Size.x + 5f, 38f), scale);
      GUI.BeginGroup(position1);
      GUI.Label(new Rect(num2 + num3, 0.0f, num2, num2), !UserInput.IsPressed(KeyState.Forward) ? LevelTutorial.Instance.ImgWasdWalkBlack[0] : LevelTutorial.Instance.ImgWasdWalkBlue[0]);
      for (int index = 1; index < 4; ++index)
        GUI.Label(new Rect((float) (index - 1) * (num2 + num3), num2 + num3, num2, num2), !UserInput.IsPressed(this._keys[index]) ? LevelTutorial.Instance.ImgWasdWalkBlack[index] : LevelTutorial.Instance.ImgWasdWalkBlue[index]);
      GUI.EndGroup();
    }
    if (this._showGotoArmory)
    {
      this._txtToArmory.Scale = this._scale;
      this._txtToArmory.Position = new Vector2(70f, 40f);
      this._txtToArmory.Draw(0.0f, 58f);
      this._objGotoArmory.Draw(new Vector2(this._txtToArmory.Size.x + 5f, 38f), scale);
    }
    if (LevelTutorial.Instance.ShowObjPickupMG)
    {
      this._txtPickupMG.Scale = this._scale;
      this._txtPickupMG.Position = new Vector2(70f, 40f);
      this._txtPickupMG.Draw(0.0f, 57f);
      this._objPickupWeapon.Draw(new Vector2(this._txtPickupMG.Size.x + 5f, 38f), scale);
    }
    if (LevelTutorial.Instance.ShowObjShoot3)
    {
      this._txtShoot3.Scale = this._scale;
      this._txtShoot3.Position = new Vector2(70f, 40f);
      this._txtShoot3.Draw(0.0f, 57f);
      this._objShootTarget3.Draw(new Vector2(this._txtShoot3.Size.x + 5f, 38f), scale);
    }
    if (LevelTutorial.Instance.ShowObjShoot6)
    {
      this._txtShoot6.Scale = this._scale;
      this._txtShoot6.Position = new Vector2(70f, 40f);
      this._txtShoot6.Draw(0.0f, 57f);
      this._objShootTarget6.Draw(new Vector2(this._txtShoot6.Size.x + 5f, 38f), scale);
    }
    GUI.EndGroup();
  }

  private void DrawWaypoints()
  {
    if (this._sequence.State <= TutorialCinematicSequence.TutorialState.AirlockWasdWalk)
      ;
  }

  private void DrawBlackBars()
  {
    if (Event.current.type == UnityEngine.EventType.Repaint)
      this._blackBarHeight = Mathf.Lerp(this._blackBarHeight, (float) (Screen.height * 1) / 8f, Time.deltaTime * 3f);
    GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) Screen.width, this._blackBarHeight), (Texture) BlueStonez.box_black.normal.background);
    GUI.DrawTexture(new Rect(0.0f, (float) Screen.height - this._blackBarHeight, (float) Screen.width, this._blackBarHeight), (Texture) BlueStonez.box_black.normal.background);
  }

  protected override void OnInitialized()
  {
    base.OnInitialized();
    MonoRoutine.Run(new MonoRoutine.FunctionVoid(this.StartDecreasingHealthAndArmor));
    MonoRoutine.Run(new MonoRoutine.FunctionVoid(this.SimulateGameFrameUpdate));
  }

  protected override void OnCharacterLoaded()
  {
    this.CreateAirlockNPC();
    this.PlaceAvatarInAirlock();
    GameState.LocalDecorator.HideWeapons();
    MonoRoutine.Start(this._sequence.StartSequences());
  }

  protected override void OnModeInitialized()
  {
    if (Application.isEditor)
      GlobalUIRibbon.Instance.Hide();
    GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.None);
    GameState.LocalPlayer.SetEnabled(true);
    this.OnPlayerJoined(SyncObjectBuilder.GetSyncData((CmuneDeltaSync) GameState.LocalCharacter, true), Vector3.zero);
    switch (this._sequence.State)
    {
      case TutorialCinematicSequence.TutorialState.None:
      case TutorialCinematicSequence.TutorialState.AirlockCameraZoomIn:
        AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
        break;
    }
    this.IsMatchRunning = true;
    Screen.lockCursor = true;
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag = HudDrawFlags.XpPoints;
    ApplicationWebServiceClient.RecordTutorialStep(PlayerDataManager.CmidSecure, TutorialStepType.TutorialStart, (Action) (() => UnityEngine.Debug.Log((object) "TutorialStart recorded")), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
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
    this.OnSetNextSpawnPoint(UnityEngine.Random.Range(0, Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(GameMode.Tutorial, TeamID.NONE)), 3);
  }

  public override void RequestRespawn() => this.OnSetNextSpawnPoint(UnityEngine.Random.Range(0, Singleton<SpawnPointManager>.Instance.GetSpawnPointCount(GameMode.Tutorial, TeamID.NONE)), 3);

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

  public void ResetBlackBar() => this._blackBarHeight = 0.0f;

  private void PlaceAvatarInAirlock()
  {
    Vector3 vector3 = new Vector3(58f, -6.437f, 64.4f);
    Quaternion quaternion = Quaternion.Euler(0.0f, 224f, 0.0f);
    GameState.LocalCharacter.Position = vector3;
    GameState.LocalCharacter.HorizontalRotation = quaternion;
    GameState.LocalDecorator.HideWeapons();
    GameState.LocalPlayer.transform.position = vector3;
    if (!(bool) (UnityEngine.Object) GameState.LocalPlayer.Character || !(bool) (UnityEngine.Object) GameState.LocalPlayer.Decorator || GameState.LocalPlayer.Decorator.AnimationController == null)
      return;
    GameState.LocalPlayer.Character.StateController.IsCinematic = true;
    GameState.LocalPlayer.Decorator.AnimationController.ResetAllAnimations();
    GameState.LocalPlayer.Decorator.AnimationController.PlayAnimation(AnimationIndex.HomeNoWeaponIdle);
  }

  private void CreateAirlockNPC()
  {
    Vector3 pos = new Vector3(56.97f, -7.4f, 63.18f);
    if (!GameState.HasCurrentSpace)
      throw new Exception("GameState doesn't have current space!");
    if (!LevelTutorial.Exists)
      throw new Exception("LevelTutorial is not initialized!");
    LevelTutorial instance = LevelTutorial.Instance;
    AvatarDecorator avatarDecorator = UnityEngine.Object.Instantiate((UnityEngine.Object) PrefabManager.Instance.DefaultAvatar) as AvatarDecorator;
    if (!(bool) (UnityEngine.Object) avatarDecorator)
      return;
    List<GameObject> objects = new List<GameObject>()
    {
      Singleton<ItemManager>.Instance.GetPrefab(instance.GearHead),
      Singleton<ItemManager>.Instance.GetPrefab(instance.GearFace),
      Singleton<ItemManager>.Instance.GetPrefab(instance.GearGloves),
      Singleton<ItemManager>.Instance.GetPrefab(instance.GearHead),
      Singleton<ItemManager>.Instance.GetPrefab(instance.GearLB),
      Singleton<ItemManager>.Instance.GetPrefab(instance.GearUB),
      Singleton<ItemManager>.Instance.GetPrefab(instance.GearBoots)
    };
    SkinnedMeshCombiner.Combine(avatarDecorator.gameObject, objects);
    avatarDecorator.gameObject.layer = 21;
    avatarDecorator.transform.position = LevelTutorial.Instance.NpcStartPos.position;
    avatarDecorator.transform.rotation = LevelTutorial.Instance.NpcStartPos.rotation;
    this._airlockNPC = avatarDecorator.GetComponent<AvatarDecorator>();
    if ((bool) (UnityEngine.Object) this._airlockNPC)
    {
      if ((bool) (UnityEngine.Object) this._airlockNPC.Animation)
        this._airlockNPC.Animation.enabled = false;
      BaseWeaponDecorator baseWeaponDecorator = LevelTutorial.Instance.Weapon.Clone();
      if ((bool) (UnityEngine.Object) baseWeaponDecorator)
      {
        baseWeaponDecorator.transform.parent = this._airlockNPC.WeaponAttachPoint;
        baseWeaponDecorator.transform.localPosition = Vector3.zero;
        baseWeaponDecorator.transform.localRotation = Quaternion.identity;
        LayerUtil.SetLayerRecursively(baseWeaponDecorator.transform, UberstrikeLayer.Props);
      }
      UnityEngine.Object.Destroy((UnityEngine.Object) this._airlockNPC);
    }
    avatarDecorator.animation.enabled = true;
    avatarDecorator.animation.Play(AnimationIndex.HomeSmallGunIdle.ToString());
    avatarDecorator.animation.Stop();
    CapsuleCollider capsuleCollider = avatarDecorator.gameObject.AddComponent<CapsuleCollider>();
    if ((bool) (UnityEngine.Object) capsuleCollider)
      capsuleCollider.radius = 0.4f;
    TutorialAirlockNPC tutorialAirlockNpc = avatarDecorator.gameObject.AddComponent<TutorialAirlockNPC>();
    if ((bool) (UnityEngine.Object) tutorialAirlockNpc)
    {
      MonoRoutine.Start(this.StartNPCAirlockVoiceOver());
      tutorialAirlockNpc.SetFinalPosition(pos);
    }
    LevelTutorial.Instance.NPC = avatarDecorator.transform;
  }

  [DebuggerHidden]
  private IEnumerator StartNPCAirlockVoiceOver() => (IEnumerator) new TutorialGameMode.\u003CStartNPCAirlockVoiceOver\u003Ec__Iterator27()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartTutorialMode() => (IEnumerator) new TutorialGameMode.\u003CStartTutorialMode\u003Ec__Iterator28()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartDecreasingHealthAndArmor() => (IEnumerator) new TutorialGameMode.\u003CStartDecreasingHealthAndArmor\u003Ec__Iterator29()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator SimulateGameFrameUpdate() => (IEnumerator) new TutorialGameMode.\u003CSimulateGameFrameUpdate\u003Ec__Iterator2A()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartAirlockCameraSmoothIn() => (IEnumerator) new TutorialGameMode.\u003CStartAirlockCameraSmoothIn\u003Ec__Iterator2B()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartShowObjective() => (IEnumerator) new TutorialGameMode.\u003CStartShowObjective\u003Ec__Iterator2C()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartAirlockMouseLook() => (IEnumerator) new TutorialGameMode.\u003CStartAirlockMouseLook\u003Ec__Iterator2D()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartAirlockWasdWalk() => (IEnumerator) new TutorialGameMode.\u003CStartAirlockWasdWalk\u003Ec__Iterator2E()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartNavigateToArmory() => (IEnumerator) new TutorialGameMode.\u003CStartNavigateToArmory\u003Ec__Iterator2F()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartHideObjectives() => (IEnumerator) new TutorialGameMode.\u003CStartHideObjectives\u003Ec__Iterator30()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartArmoryObjective() => (IEnumerator) new TutorialGameMode.\u003CStartArmoryObjective\u003Ec__Iterator31()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartPickupWeapon() => (IEnumerator) new TutorialGameMode.\u003CStartPickupWeapon\u003Ec__Iterator32()
  {
    \u003C\u003Ef__this = this
  };

  private void ShowDirective(MeshGUIText txt, bool showObjective = true)
  {
    this.EnableObjectives(showObjective);
    txt.Alpha = 0.0f;
    txt.Flicker(0.5f, 0.02f);
    txt.FadeAlphaTo(1f, 0.5f);
    txt.Show();
  }

  [DebuggerHidden]
  private IEnumerator StartHideDirective(MeshGUIText txt, bool delete = true) => (IEnumerator) new TutorialGameMode.\u003CStartHideDirective\u003Ec__Iterator33()
  {
    txt = txt,
    delete = delete,
    \u003C\u0024\u003Etxt = txt,
    \u003C\u0024\u003Edelete = delete,
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator StartTerminateTutorial() => (IEnumerator) new TutorialGameMode.\u003CStartTerminateTutorial\u003Ec__Iterator34()
  {
    \u003C\u003Ef__this = this
  };

  public void ShowShoot3() => this.ShowDirective(this._txtShoot3);

  public void ShowShoot6()
  {
    this._subtitle = string.Empty;
    this.ShowDirective(this._txtShoot6);
  }

  public void HideShoot3()
  {
    ApplicationWebServiceClient.RecordTutorialStep(PlayerDataManager.CmidSecure, TutorialStepType.ShootFirstGroup, (Action) (() => UnityEngine.Debug.Log((object) "ShootFirstGroup recorded")), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
    SfxManager.Play2dAudioClip(LevelTutorial.Instance.VoiceShootMore);
    this._subtitle = "Hah, not bad at all! Try some more!";
    MonoRoutine.Start(this.StartHideDirective(this._txtShoot3));
  }

  public void HideShoot6()
  {
    ApplicationWebServiceClient.RecordTutorialStep(PlayerDataManager.CmidSecure, TutorialStepType.ShootSecondGroup, (Action) (() => UnityEngine.Debug.Log((object) "ShootSecondGroup recorded")), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
    this._subtitle = "Who would have thought you had it in you.\nNow let's see how you do in some real combat.";
    MonoRoutine.Start(this.StartHideDirective(this._txtShoot6));
  }

  public void DestroyObjectives()
  {
    MonoRoutine.Start(this.StartHideDirective(this._txtObjectives));
    MonoRoutine.Start(this.StartHideDirective(this._txtObjUnderscore));
  }

  public void ShowObjComplete() => this.ShowDirective(this._txtComplete, false);

  public void HideObjComplete(bool destroyAfter = true)
  {
    this._subtitle = string.Empty;
    MonoRoutine.Start(this.StartHideDirective(this._txtComplete, destroyAfter));
  }

  public void ShowTutorialComplete()
  {
    this._txtComplete.Text = "TUTORIAL COMPLETE";
    this._txtComplete.BitmapMeshText.ShadowColor = new Color(0.7529412f, 0.549019635f, 0.0f);
    this.ShowDirective(this._txtComplete, false);
  }

  public void OnAirlockCameraZoomIn()
  {
    GameState.LocalPlayer.Character.Decorator.MeshRenderer.enabled = true;
    GameState.LocalPlayer.Character.Decorator.HudInformation.enabled = true;
    MonoRoutine.Start(this.StartAirlockCameraSmoothIn());
  }

  public void OnAirlockWelcome() => MonoRoutine.Start(this.StartShowObjective());

  public void OnAirlockMouseLookSubtitle()
  {
    this._subtitle = string.Empty;
    MonoRoutine.Start(this.StartAirlockMouseLook());
  }

  public void OnAirlockWasdSubtitle() => MonoRoutine.Start(this.StartAirlockWasdWalk());

  public void OnAirlockDoorOpen()
  {
    this._subtitle = string.Empty;
    MonoRoutine.Start(this.StartNavigateToArmory());
  }

  public void ReachArmoryWaypoint()
  {
    this._objGotoArmory.Complete();
    SfxManager.Play2dAudioClip(GameAudio.ObjectiveTick);
    GameState.LocalCharacter.Keys = KeyState.Still;
    HudController.Instance.XpPtsHud.GainXp(5);
  }

  public void HideObjectives() => MonoRoutine.Start(this.StartHideObjectives());

  public void EnterArmory(SplineController splineController)
  {
    this._sequence.OnArmoryEnter();
    this._shootingRangeController = new TutorialShootingTargetController(this);
  }

  public void OnArmoryEnterSubtitle()
  {
    this._subtitle = "Right, go to the counter, pick up your weapon and lets see what you're made of.";
    SfxManager.Play2dAudioClip(LevelTutorial.Instance.VoicePickupWeapon);
    LevelTutorial.Instance.ArmoryDoor.Close();
    LevelTutorial.Instance.IsCinematic = false;
    LevelTutorial.Instance.ShowObjectives = true;
    SfxManager.Play2dAudioClip(GameAudio.SubObjective);
    MonoRoutine.Start(this.StartArmoryObjective());
  }

  public void OnArmoryPickupMG()
  {
    ApplicationWebServiceClient.RecordTutorialStep(PlayerDataManager.CmidSecure, TutorialStepType.PickUpWeapon, (Action) (() => UnityEngine.Debug.Log((object) "PickUpWeapon recorded")), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
    this._subtitle = "Alright my soldier. Let's see if you can shoot straight.\nFeed these targets some lead.";
    this._sequence.OnArmoryPickupMG();
    GameState.LocalPlayer.SetWeaponControlState(PlayerHudState.Playing);
    GameState.LocalPlayer.UnPausePlayer();
    Singleton<HudDrawFlagGroup>.Instance.BaseDrawFlag = HudDrawFlags.Ammo | HudDrawFlags.Reticle | HudDrawFlags.XpPoints;
    this._objPickupWeapon.Complete();
    SfxManager.Play2dAudioClip(GameAudio.ObjectiveTick);
    MonoRoutine.Start(this.StartPickupWeapon());
    LevelTutorial.Instance.WeaponWaypoint.CanShow = false;
    MonoRoutine.Start(this._shootingRangeController.StartShootingRange());
  }

  public void OnTutorialEnd()
  {
    if (!LevelTutorial.Exists)
      return;
    LevelTutorial.Instance.StartCoroutine(this.StartTerminateTutorial());
  }

  private void EnableObjectives(bool enabled)
  {
    if (enabled)
    {
      this._txtObjectives.Show();
      this._txtObjUnderscore.Show();
    }
    else
    {
      this._txtObjectives.Hide();
      this._txtObjUnderscore.Hide();
    }
  }
}
