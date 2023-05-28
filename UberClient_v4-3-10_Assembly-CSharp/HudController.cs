// Decompiled with JetBrains decompiler
// Type: HudController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class HudController : MonoBehaviour
{
  private XpPtsHud _xpPtsHud;
  private HudDrawFlags _finalDrawFlag;

  public static HudController Instance { get; private set; }

  public static bool Exists => (UnityEngine.Object) HudController.Instance != (UnityEngine.Object) null;

  public XpPtsHud XpPtsHud
  {
    get
    {
      if (this._xpPtsHud == null)
      {
        this._xpPtsHud = new XpPtsHud();
        CmuneEventHandler.AddListener<OnSetPlayerTeamEvent>(new Action<OnSetPlayerTeamEvent>(this.XpPtsHud.OnTeamChange));
        CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.XpPtsHud.OnScreenResolutionChange));
      }
      return this._xpPtsHud;
    }
  }

  public HudDrawFlags DrawFlags
  {
    get => this._finalDrawFlag;
    set
    {
      this._finalDrawFlag = value;
      this.UpdateHudVisibilityByFlag();
    }
  }

  public string DrawFlagString => CmunePrint.Flag<HudDrawFlags>((ushort) this.DrawFlags);

  private void Awake() => HudController.Instance = this;

  private void OnEnable() => this.UpdateHudVisibilityByFlag();

  private void OnDisable()
  {
    if (GameState.IsShuttingDown)
      return;
    this.UpdateHudVisibilityByFlag();
  }

  private void OnGUI()
  {
    GUI.depth = 100;
    if (this.IsDrawFlagEnabled(HudDrawFlags.InGameChat))
      Singleton<InGameChatHud>.Instance.Draw();
    if (Input.GetKeyDown(KeyCode.B))
      this.XpPtsHud.PopupTemporarily();
    if (this.IsDrawFlagEnabled(HudDrawFlags.HealthArmor))
      Singleton<HpApHud>.Instance.Draw();
    if (this.IsDrawFlagEnabled(HudDrawFlags.Ammo))
    {
      Singleton<AmmoHud>.Instance.Draw();
      Singleton<TemporaryWeaponHud>.Instance.Draw();
    }
    if (this.IsDrawFlagEnabled(HudDrawFlags.XpPoints))
      this.XpPtsHud.Draw();
    if (this.IsDrawFlagEnabled(HudDrawFlags.Weapons))
      Singleton<WeaponsHud>.Instance.Draw();
    if (this.IsDrawFlagEnabled(HudDrawFlags.EventStream))
      Singleton<EventStreamHud>.Instance.Draw();
    if (this.IsDrawFlagEnabled(HudDrawFlags.Score | HudDrawFlags.RoundTime | HudDrawFlags.RemainingKill))
      Singleton<MatchStatusHud>.Instance.Draw();
    if (this.IsDrawFlagEnabled(HudDrawFlags.Reticle))
      Singleton<ReticleHud>.Instance.Draw();
    if (this.IsDrawFlagEnabled(HudDrawFlags.InGameHelp))
      Singleton<InGameHelpHud>.Instance.Draw();
    Singleton<EventFeedbackHud>.Instance.Draw();
    Singleton<LevelUpHud>.Instance.Draw();
    Singleton<PopupHud>.Instance.Draw();
    Singleton<GameModeObjectiveHud>.Instance.Draw();
    Singleton<TeamChangeWarningHud>.Instance.Draw();
    Singleton<DamageFeedbackHud>.Instance.Draw();
    Singleton<PlayerStateMsgHud>.Instance.Draw();
    Singleton<ScreenshotHud>.Instance.Draw();
    Singleton<FrameRateHud>.Instance.Draw();
  }

  private void Update()
  {
    Singleton<HpApHud>.Instance.Update();
    this.XpPtsHud.Update();
    Singleton<PickupNameHud>.Instance.Update();
    Singleton<ReticleHud>.Instance.Update();
    Singleton<LocalShotFeedbackHud>.Instance.Update();
    Singleton<DamageFeedbackHud>.Instance.Update();
    Singleton<InGameFeatHud>.Instance.Update();
    Singleton<WeaponsHud>.Instance.Update();
    Singleton<EventStreamHud>.Instance.Update();
    Singleton<GameModeObjectiveHud>.Instance.Update();
    if (this.IsDrawFlagEnabled(HudDrawFlags.InGameHelp))
      Singleton<InGameHelpHud>.Instance.Update();
    if (this.IsDrawFlagEnabled(HudDrawFlags.InGameChat))
      Singleton<InGameChatHud>.Instance.Update();
    Singleton<HudUtil>.Instance.Update();
  }

  private void UpdateHudVisibilityByFlag()
  {
    Singleton<HpApHud>.Instance.Enabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.HealthArmor);
    Singleton<AmmoHud>.Instance.Enabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.Ammo);
    this.XpPtsHud.Enabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.XpPoints);
    Singleton<WeaponsHud>.Instance.SetEnabled(this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.Weapons));
    Singleton<EventStreamHud>.Instance.Enabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.EventStream);
    Singleton<MatchStatusHud>.Instance.IsScoreEnabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.Score);
    Singleton<MatchStatusHud>.Instance.IsClockEnabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.RoundTime);
    Singleton<MatchStatusHud>.Instance.IsRemainingKillEnabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.RemainingKill);
    Singleton<InGameHelpHud>.Instance.Enabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.InGameHelp);
    Singleton<ReticleHud>.Instance.Enabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.Reticle);
    Singleton<InGameChatHud>.Instance.Enabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.InGameChat);
    Singleton<EventFeedbackHud>.Instance.Enabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.StateMsg);
    Singleton<GameModeObjectiveHud>.Instance.Enabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.StateMsg);
    Singleton<PickupNameHud>.Instance.Enabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.StateMsg);
    Singleton<PlayerStateMsgHud>.Instance.TemporaryMsgEnabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.StateMsg);
    Singleton<PlayerStateMsgHud>.Instance.PermanentMsgEnabled = this.enabled && this.IsDrawFlagEnabled(HudDrawFlags.StateMsg);
  }

  private bool IsDrawFlagEnabled(HudDrawFlags drawFlag) => (this.DrawFlags & drawFlag) != HudDrawFlags.None;
}
