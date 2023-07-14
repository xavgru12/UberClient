// Decompiled with JetBrains decompiler
// Type: EndOfRoundPageGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class EndOfRoundPageGUI : PageGUI
{
  private const float WeaponRecommendHeight = 265f;
  private WeaponDetailGUI _weaponDetailGui;
  private ValuablePlayerDetailGUI _playerDetailGui;
  private ValuablePlayerListGUI _playerListGui;
  private WeaponRecommendListGUI _weaponRecomGui;

  public override void DrawGUI(Rect rect)
  {
    float height = Mathf.Min(this._playerListGui.Height, rect.height - 265f) - 2f;
    float num = Mathf.Min(this._playerListGui.Height, rect.height - 265f);
    GUI.BeginGroup(rect, GUIContent.none, BlueStonez.window_standard_grey38);
    this._playerListGui.Draw(new Rect(2f, 2f, rect.width - 4f, height));
    this.DrawWeaponRecommend(new Rect(2f, 2f + num, rect.width - 4f, 265f));
    GUI.EndGroup();
  }

  private void Awake()
  {
    this._weaponRecomGui = new WeaponRecommendListGUI(BuyingLocationType.EndOfRound);
    this._weaponDetailGui = new WeaponDetailGUI();
    this._playerListGui = new ValuablePlayerListGUI();
    this._playerDetailGui = new ValuablePlayerDetailGUI();
    this._playerListGui.OnSelectionChange = new Action<StatsSummary>(this.OnValuablePlayerListSelectionChange);
    this._weaponRecomGui.OnSelectionChange = new Action<IUnityItem, RecommendType>(this.OnRecomListSelectionChange);
  }

  private void OnEnable()
  {
    this.OnUpdateRecommendationEvent((UpdateRecommendationEvent) null);
    CmuneEventHandler.AddListener<UpdateRecommendationEvent>(new Action<UpdateRecommendationEvent>(this.OnUpdateRecommendationEvent));
    if (Singleton<EndOfMatchStats>.Instance.Data.MostValuablePlayers.Count > 0)
      this._playerListGui.SetSelection(0);
    else
      this._playerDetailGui.SetValuablePlayer((StatsSummary) null);
    this._weaponRecomGui.Enabled = true;
    this._playerListGui.Enabled = true;
  }

  private void OnDisabled()
  {
    this._weaponRecomGui.Enabled = false;
    this._playerListGui.Enabled = false;
    this._playerListGui.ClearSelection();
    this._playerDetailGui.StopBadgeShow();
    CmuneEventHandler.RemoveListener<UpdateRecommendationEvent>(new Action<UpdateRecommendationEvent>(this.OnUpdateRecommendationEvent));
  }

  private void OnUpdateRecommendationEvent(UpdateRecommendationEvent ev)
  {
    List<KeyValuePair<RecommendType, IUnityItem>> recomendations = new List<KeyValuePair<RecommendType, IUnityItem>>(3);
    recomendations.Add(new KeyValuePair<RecommendType, IUnityItem>(RecommendType.StaffPick, Singleton<MapManager>.Instance.GetRecommendedItem(GameState.CurrentSpace.SceneName)));
    recomendations.Add(new KeyValuePair<RecommendType, IUnityItem>(RecommendType.RecommendedArmor, (IUnityItem) ShopUtils.GetRecommendedArmor(PlayerDataManager.PlayerLevelSecure, Singleton<LoadoutManager>.Instance.GetItemOnSlot<HoloGearItem>(LoadoutSlotType.GearHolo), Singleton<LoadoutManager>.Instance.GetItemOnSlot<GearItem>(LoadoutSlotType.GearUpperBody), Singleton<LoadoutManager>.Instance.GetItemOnSlot<GearItem>(LoadoutSlotType.GearLowerBody))));
    IUnityItem itemInShop = Singleton<ItemManager>.Instance.GetItemInShop(Singleton<EndOfMatchStats>.Instance.Data.MostEffecientWeaponId);
    if (itemInShop == null)
    {
      IUnityItem unityItem = (IUnityItem) (RecommendationUtils.GetRecommendedWeapon(PlayerDataManager.PlayerLevelSecure, GameState.CurrentSpace.CombatRangeTiers).ItemWeapon ?? RecommendationUtils.FallBackWeapon);
      recomendations.Add(new KeyValuePair<RecommendType, IUnityItem>(RecommendType.RecommendedWeapon, unityItem));
    }
    else
      recomendations.Add(new KeyValuePair<RecommendType, IUnityItem>(RecommendType.MostEfficient, itemInShop));
    this._weaponRecomGui.UpdateRecommendedList((IEnumerable<KeyValuePair<RecommendType, IUnityItem>>) recomendations);
  }

  private void DrawWeaponRecommend(Rect rect)
  {
    GUI.BeginGroup(rect);
    GUI.Label(new Rect(0.0f, 2f, rect.width, 20f), LocalizedStrings.RecommendedLoadoutCaps, BlueStonez.label_interparkbold_18pt);
    this.DrawRecommendContent(new Rect(0.0f, 25f, rect.width, rect.height - 25f));
    GUI.EndGroup();
  }

  private void DrawRecommendContent(Rect rect)
  {
    GUI.BeginGroup(rect);
    if (this._weaponRecomGui.SelectedItem != null)
      this._weaponDetailGui.Draw(new Rect(0.0f, 0.0f, 200f, rect.height));
    else
      this._playerDetailGui.Draw(new Rect(0.0f, 0.0f, 200f, rect.height));
    this._weaponRecomGui.Draw(new Rect(199f, 0.0f, (float) ((double) rect.width - 200.0 + 1.0), rect.height));
    GUI.EndGroup();
  }

  private void OnRecomListSelectionChange(IUnityItem item, RecommendType type)
  {
    this._playerListGui.ClearSelection();
    this._playerDetailGui.StopBadgeShow();
    this._weaponDetailGui.SetWeaponItem(item, type);
  }

  private void OnValuablePlayerListSelectionChange(StatsSummary playerStats)
  {
    this._weaponRecomGui.ClearSelection();
    this._playerDetailGui.SetValuablePlayer(playerStats);
  }
}
