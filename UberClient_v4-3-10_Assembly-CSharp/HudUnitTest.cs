// Decompiled with JetBrains decompiler
// Type: HudUnitTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class HudUnitTest : MonoBehaviour
{
  public GUISkin skin;
  public TeamID teamID;
  public bool enableMultiKill;
  public bool enableHpAp;
  public bool enableAmmo;
  public bool enableXpPtsBar;
  public bool enableEventStream;
  public bool enableTeamScoreAndClock;
  public bool enableHelpInfo;
  public bool enablePermanentStateMsg;
  public bool enableTemporaryWeapon;
  private TeamID lastTeamId;
  private MeshGUIList weaponHud;
  private QuickItemHud armorQuickItem = new QuickItemHud("ArmorQuickItem");
  private QuickItemHud healthQuickItem = new QuickItemHud("HealthQuickItem");
  private QuickItemHud springQuickItem = new QuickItemHud("SpringQuickItem");
  private Dictionary<LoadoutSlotType, QuickItemHud> _quickItemsHud;
  private Animatable2DGroup _quickItemsGroup;

  private void Start()
  {
    Singleton<HudStyleUtility>.Instance.OnTeamChange(new OnSetPlayerTeamEvent());
    LocalizedStrings.KillsRemain = "Kills Remaining";
    LocalizedStrings.WaitingForOtherPlayers = "Waiting For Other Players";
    LocalizedStrings.SpectatorMode = "Spectator Mode";
    this.lastTeamId = TeamID.NONE;
    this.teamID = TeamID.BLUE;
    Singleton<MatchStatusHud>.Instance.RemainingSeconds = 31;
    Singleton<MatchStatusHud>.Instance.RemainingKills = 6;
    Singleton<InGameHelpHud>.Instance.EnableChangeTeamHelp = true;
    Singleton<TemporaryWeaponHud>.Instance.StartCounting(30);
    this.weaponHud = new MeshGUIList();
    this.weaponHud.Enabled = true;
    this.weaponHud.AddItem("Melee");
    this.weaponHud.AddItem("Machine Gun");
    this.weaponHud.AddItem("Sniper");
    this._quickItemsHud = new Dictionary<LoadoutSlotType, QuickItemHud>();
    this.armorQuickItem = new QuickItemHud("ArmorQuickItem");
    this.healthQuickItem = new QuickItemHud("HealthQuickItem");
    this.springQuickItem = new QuickItemHud("SpringQuickItem");
    this._quickItemsGroup = new Animatable2DGroup();
    this._quickItemsHud.Add(LoadoutSlotType.QuickUseItem1, this.springQuickItem);
    this._quickItemsHud.Add(LoadoutSlotType.QuickUseItem2, this.healthQuickItem);
    this._quickItemsHud.Add(LoadoutSlotType.QuickUseItem3, this.armorQuickItem);
    foreach (QuickItemHud quickItemHud in this._quickItemsHud.Values)
      this._quickItemsGroup.Group.Add((IAnimatable2D) quickItemHud.Group);
    this.springQuickItem.Amount = 3;
    this.ResetQuickItemsTransform();
  }

  private void Update()
  {
    Singleton<HpApHud>.Instance.Enabled = this.enableHpAp;
    Singleton<AmmoHud>.Instance.Enabled = this.enableAmmo;
    HudController.Instance.XpPtsHud.Enabled = this.enableXpPtsBar;
    Singleton<EventStreamHud>.Instance.Enabled = this.enableEventStream;
    Singleton<MatchStatusHud>.Instance.Enabled = this.enableTeamScoreAndClock;
    Singleton<InGameHelpHud>.Instance.Enabled = this.enableHelpInfo;
    Singleton<PlayerStateMsgHud>.Instance.PermanentMsgEnabled = this.enablePermanentStateMsg;
    Singleton<TemporaryWeaponHud>.Instance.Enabled = this.enableTemporaryWeapon;
    Singleton<HpApHud>.Instance.Update();
    HudController.Instance.XpPtsHud.Update();
    Singleton<LocalShotFeedbackHud>.Instance.Update();
    Singleton<PickupNameHud>.Instance.Update();
    Singleton<InGameFeatHud>.Instance.Update();
    Singleton<EventStreamHud>.Instance.Update();
    Singleton<GameModeObjectiveHud>.Instance.Update();
    if ((double) Input.GetAxis("Mouse ScrollWheel") < 0.0)
      this.weaponHud.AnimDownward();
    if ((double) Input.GetAxis("Mouse ScrollWheel") > 0.0)
      this.weaponHud.AnimUpward();
    if (this.teamID != this.lastTeamId)
    {
      Singleton<HudUtil>.Instance.SetPlayerTeam(this.teamID);
      this.lastTeamId = this.teamID;
    }
    Singleton<HudUtil>.Instance.Update();
  }

  private void OnGUI()
  {
    if (this.enableMultiKill)
    {
      if (GUI.Button(new Rect(0.0f, 0.0f, 100f, 30f), "Popup Double"))
        Singleton<PopupHud>.Instance.PopupMultiKill(2);
      if (GUI.Button(new Rect(0.0f, 40f, 100f, 30f), "Popup Triple"))
        Singleton<PopupHud>.Instance.PopupMultiKill(3);
      if (GUI.Button(new Rect(0.0f, 80f, 100f, 30f), "Popup Quad"))
        Singleton<PopupHud>.Instance.PopupMultiKill(4);
      if (GUI.Button(new Rect(0.0f, 120f, 100f, 30f), "Popup Mega"))
        Singleton<PopupHud>.Instance.PopupMultiKill(5);
      if (GUI.Button(new Rect(0.0f, 160f, 100f, 30f), "Popup Uber"))
        Singleton<PopupHud>.Instance.PopupMultiKill(6);
      if (GUI.Button(new Rect(0.0f, 200f, 100f, 30f), "Round Start"))
        Singleton<PopupHud>.Instance.PopupRoundStart();
      Singleton<PopupHud>.Instance.Draw();
    }
    if (this.enableHpAp)
    {
      if (GUI.Button(new Rect((float) Screen.width - 100f, 200f, 100f, 30f), "Increase HP"))
        ++Singleton<HpApHud>.Instance.HP;
      if (GUI.Button(new Rect((float) Screen.width - 100f, 240f, 100f, 30f), "Decrease HP"))
        --Singleton<HpApHud>.Instance.HP;
      if (GUI.Button(new Rect((float) Screen.width - 150f, 280f, 150f, 30f), "Decrease Armor"))
        --Singleton<HpApHud>.Instance.AP;
      Singleton<HpApHud>.Instance.Draw();
    }
    if (this.enableXpPtsBar)
    {
      if (GUI.Button(new Rect((float) Screen.width - 100f, 320f, 100f, 30f), "Generate XP"))
        HudController.Instance.XpPtsHud.GainXp(3);
      if (GUI.Button(new Rect((float) Screen.width - 100f, 360f, 100f, 30f), "Generate Pts"))
        HudController.Instance.XpPtsHud.GainPoints(3);
      if (GUI.Button(new Rect((float) Screen.width - 100f, 120f, 100f, 30f), "Popup Xp bar"))
        HudController.Instance.XpPtsHud.PopupTemporarily();
      HudController.Instance.XpPtsHud.Draw();
    }
    if (this.enableAmmo)
    {
      if (GUI.Button(new Rect((float) Screen.width - 150f, 400f, 150f, 30f), "Increase Ammo"))
        ++Singleton<AmmoHud>.Instance.Ammo;
      Singleton<AmmoHud>.Instance.Draw();
    }
    if (this.enableEventStream)
    {
      if (GUI.Button(new Rect((float) Screen.width - 100f, 440f, 100f, 30f), "Add Event"))
        Singleton<EventStreamHud>.Instance.AddEventText("UberPlayerUberPlayerUberPlayer", TeamID.BLUE, "smackdown", "Bulletsponge", TeamID.RED);
      Singleton<EventStreamHud>.Instance.Draw();
    }
    if (GUI.Button(new Rect((float) Screen.width - 200f, 480f, 200f, 30f), "Set Deathmatch Mode"))
      Singleton<GameModeObjectiveHud>.Instance.DisplayGameMode(GameMode.DeathMatch);
    Singleton<GameModeObjectiveHud>.Instance.Draw();
    if (this.enableTeamScoreAndClock)
    {
      if (GUI.Button(new Rect(0.0f, 520f, 150f, 30f), "Decrement Clock"))
        --Singleton<MatchStatusHud>.Instance.RemainingSeconds;
      if (GUI.Button(new Rect(0.0f, 560f, 200f, 30f), "Decrement Remaining Kill"))
        Singleton<MatchStatusHud>.Instance.RemainingRoundsText = "Final Round RED";
      Singleton<MatchStatusHud>.Instance.Draw();
    }
    if (this.enableHelpInfo)
      Singleton<InGameHelpHud>.Instance.Draw();
    if (GUI.Button(new Rect(0.0f, 480f, 150f, 30f), "Add Event Feedback"))
      Singleton<EventFeedbackHud>.Instance.EnqueueFeedback(InGameEventFeedbackType.CustomMessage, "Headshot from Benny");
    Singleton<EventFeedbackHud>.Instance.Draw();
    Singleton<PickupNameHud>.Instance.Draw();
    if (GUI.Button(new Rect(0.0f, 220f, 150f, 30f), "Anim Pickup Item"))
      Singleton<PickupNameHud>.Instance.DisplayPickupName("Ammo Small", PickUpMessageType.Armor5);
    if (GUI.Button(new Rect(0.0f, 360f, 150f, 30f), "Nut Shot"))
      Singleton<LocalShotFeedbackHud>.Instance.DisplayLocalShotFeedback(InGameEventFeedbackType.NutShot);
    if (GUI.Button(new Rect(0.0f, 400f, 150f, 30f), "Head Shot"))
      Singleton<LocalShotFeedbackHud>.Instance.DisplayLocalShotFeedback(InGameEventFeedbackType.HeadShot);
    if (GUI.Button(new Rect(0.0f, 440f, 150f, 30f), "Smackdown"))
      Singleton<LocalShotFeedbackHud>.Instance.DisplayLocalShotFeedback(InGameEventFeedbackType.Humiliation);
    if (GUI.Button(new Rect(0.0f, 260f, 150f, 30f), "Decrement Spring"))
      --this.springQuickItem.Amount;
    this.weaponHud.Draw();
    this._quickItemsGroup.Draw(0.0f, 0.0f);
    Singleton<PlayerStateMsgHud>.Instance.Draw();
    if (this.enablePermanentStateMsg)
    {
      if (GUI.Button(new Rect((float) Screen.width - 200f, 0.0f, 200f, 30f), "WaitingForPlayer"))
        Singleton<PlayerStateMsgHud>.Instance.DisplayWaitingForOtherPlayerMsg();
      if (GUI.Button(new Rect((float) Screen.width - 100f, 40f, 100f, 30f), "SpectatorMsg"))
        Singleton<PlayerStateMsgHud>.Instance.DisplaySpectatorModeMsg();
    }
    if (GUI.Button(new Rect(300f, 0.0f, 150f, 30f), "Set temporary remaining"))
      --Singleton<TemporaryWeaponHud>.Instance.RemainingSeconds;
    Singleton<TemporaryWeaponHud>.Instance.Draw();
  }

  private void ResetQuickItemsTransform()
  {
    foreach (QuickItemHud quickItemHud in this._quickItemsHud.Values)
      quickItemHud.ResetHud();
    Animatable2DGroup group1 = this._quickItemsHud[LoadoutSlotType.QuickUseItem1].Group;
    Animatable2DGroup group2 = this._quickItemsHud[LoadoutSlotType.QuickUseItem2].Group;
    Animatable2DGroup group3 = this._quickItemsHud[LoadoutSlotType.QuickUseItem3].Group;
    group3.Position = Vector2.zero;
    group2.Position = group3.Position - new Vector2(0.0f, group3.Rect.height * 0.75f);
    group1.Position = group2.Position - new Vector2(0.0f, group2.Rect.height * 0.75f);
    this._quickItemsGroup.Position = new Vector2((float) Screen.width - 50f, (float) Screen.height - this._quickItemsGroup.Rect.height / 2f);
  }
}
