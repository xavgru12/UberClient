// Decompiled with JetBrains decompiler
// Type: QuickItemGroupHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class QuickItemGroupHud
{
  private Animatable2DGroup _quickItemsGroup;
  private List<QuickItemHud> _quickItemSlots;

  public QuickItemGroupHud()
  {
    if (!HudAssets.Exists)
      return;
    this._quickItemSlots = new List<QuickItemHud>(3);
    this._quickItemsGroup = new Animatable2DGroup();
    this._quickItemSlots.Add(new QuickItemHud("Slot A-"));
    this._quickItemSlots.Add(new QuickItemHud("Slot B-"));
    this._quickItemSlots.Add(new QuickItemHud("Slot C-"));
    foreach (QuickItemHud quickItemSlot in this._quickItemSlots)
      this._quickItemsGroup.Group.Add((IAnimatable2D) quickItemSlot.Group);
    this.ResetQuickItemsTransform();
    this.ResetQuickItemVisibility();
    CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
    CmuneEventHandler.AddListener<InputAssignmentEvent>(new Action<InputAssignmentEvent>(this.OnInputAssignmentChange));
  }

  public Animatable2DGroup Group => this._quickItemsGroup;

  public bool Enabled
  {
    get => this._quickItemsGroup.IsVisible;
    set
    {
      if (value)
      {
        this._quickItemsGroup.Show();
        this.ResetQuickItemVisibility();
      }
      else
        this._quickItemsGroup.Hide();
    }
  }

  public void SetSelected(int slotIndex, bool moveNext = true)
  {
    for (int index = 0; index < this._quickItemSlots.Count; ++index)
      this._quickItemSlots[index].SetSelected(slotIndex == index, moveNext);
  }

  public void Draw() => this._quickItemsGroup.Draw(0.0f, 0.0f);

  public void ConfigureQuickItem(int slot, QuickItem quickItem, TeamID team = TeamID.NONE)
  {
    if (this._quickItemSlots.Count > slot && slot >= 0)
    {
      QuickItemHud quickItemSlot = this._quickItemSlots[slot];
      if ((UnityEngine.Object) quickItem != (UnityEngine.Object) null)
      {
        quickItemSlot.SetRechargeBarVisible(quickItem.Configuration.RechargeTime > 0);
        quickItemSlot.SetKeyBinding(AutoMonoBehaviour<InputManager>.Instance.GetKeyAssignmentString((GameInputKey) (16 + slot)));
        if (team == TeamID.RED)
          quickItemSlot.ConfigureSlot(HudStyleUtility.DEFAULT_RED_COLOR, (Texture) ConsumableHudTextures.CircleRed, (Texture) ConsumableHudTextures.CircleWhite, (Texture) ConsumableHudTextures.CircleRed, (Texture) ConsumableHudTextures.CircleRed, this.GetIconRed(quickItem));
        else
          quickItemSlot.ConfigureSlot(HudStyleUtility.DEFAULT_BLUE_COLOR, (Texture) ConsumableHudTextures.CircleBlue, (Texture) ConsumableHudTextures.CircleWhite, (Texture) ConsumableHudTextures.CircleBlue, (Texture) ConsumableHudTextures.CircleBlue, this.GetIconBlue(quickItem));
      }
      else
        quickItemSlot.ConfigureEmptySlot();
    }
    this.ResetQuickItemsTransform();
  }

  public QuickItemHud GetLoadoutQuickItemHud(int slot) => this._quickItemSlots.Count > slot && slot >= 0 ? this._quickItemSlots[slot] : (QuickItemHud) null;

  public void Expand()
  {
    if (ApplicationDataManager.IsMobile)
      return;
    int num = 0;
    for (int index = 0; index < this._quickItemSlots.Count; ++index)
    {
      if (!this._quickItemSlots[index].IsEmpty)
      {
        this._quickItemSlots[index].Expand(new Vector2(0.0f, this._quickItemSlots[index].ExpandedHeight * (float) (num - 3)), (float) num * 0.1f);
        ++num;
      }
    }
  }

  public void Collapse()
  {
    if (ApplicationDataManager.IsMobile)
      return;
    int num = 0;
    for (int index = 0; index < this._quickItemSlots.Count; ++index)
    {
      if (!this._quickItemSlots[index].IsEmpty)
      {
        this._quickItemSlots[index].Collapse(new Vector2(0.0f, this._quickItemSlots[index].CollapsedHeight * (float) (num - 3)), (float) num * 0.1f);
        ++num;
      }
    }
  }

  private Texture2D GetIconBlue(QuickItem config)
  {
    switch (config.Logic)
    {
      case QuickItemLogic.SpringGrenade:
        return ConsumableHudTextures.SpringGrenadeBlue;
      case QuickItemLogic.HealthPack:
        return ConsumableHudTextures.HealthBlue;
      case QuickItemLogic.ArmorPack:
        return ConsumableHudTextures.ArmorBlue;
      case QuickItemLogic.AmmoPack:
        return ConsumableHudTextures.AmmoBlue;
      case QuickItemLogic.ExplosiveGrenade:
        return ConsumableHudTextures.OffensiveGrenadeBlue;
      default:
        return ConsumableHudTextures.AmmoBlue;
    }
  }

  private Texture2D GetIconRed(QuickItem config)
  {
    switch (config.Logic)
    {
      case QuickItemLogic.SpringGrenade:
        return ConsumableHudTextures.SpringGrenadeRed;
      case QuickItemLogic.HealthPack:
        return ConsumableHudTextures.HealthRed;
      case QuickItemLogic.ArmorPack:
        return ConsumableHudTextures.ArmorRed;
      case QuickItemLogic.AmmoPack:
        return ConsumableHudTextures.AmmoRed;
      case QuickItemLogic.ExplosiveGrenade:
        return ConsumableHudTextures.OffensiveGrenadeRed;
      default:
        return ConsumableHudTextures.AmmoRed;
    }
  }

  private void ResetQuickItemVisibility()
  {
    if (this._quickItemSlots.Count == 0)
    {
      this._quickItemsGroup.Hide();
    }
    else
    {
      this._quickItemsGroup.Show();
      foreach (QuickItemHud quickItemSlot in this._quickItemSlots)
      {
        if (quickItemSlot.IsEmpty)
          quickItemSlot.ConfigureEmptySlot();
      }
    }
  }

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => this.ResetQuickItemsTransform();

  private void OnInputAssignmentChange(InputAssignmentEvent ev)
  {
    for (int index = 0; index < this._quickItemSlots.Count; ++index)
    {
      QuickItemHud quickItemSlot = this._quickItemSlots[index];
      if (!quickItemSlot.IsEmpty)
        quickItemSlot.SetKeyBinding(AutoMonoBehaviour<InputManager>.Instance.GetKeyAssignmentString((GameInputKey) (16 + index)));
    }
  }

  private void ResetQuickItemsTransform()
  {
    QuickItemHud quickItemSlot1 = this._quickItemSlots[0];
    foreach (QuickItemHud quickItemSlot2 in this._quickItemSlots)
      quickItemSlot2.ResetHud();
    if (quickItemSlot1.IsExpanded)
      this.Expand();
    else
      this.Collapse();
    float y = (float) ((double) Screen.height * 0.89999997615814209 - 10.0);
    if (ApplicationDataManager.IsMobile)
      y = 160f;
    this._quickItemsGroup.Position = new Vector2((float) ((double) Screen.width * 0.949999988079071 - (double) quickItemSlot1.Group.Rect.width / 2.0), y);
  }
}
