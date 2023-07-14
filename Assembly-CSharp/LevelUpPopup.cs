// Decompiled with JetBrains decompiler
// Type: LevelUpPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPopup : IPopupDialog
{
  protected int Width = 650;
  protected int Height = 330;
  private ShopItemGrid _itemGrid;
  private Action _action;
  private int _level;

  public LevelUpPopup(int level, Action action = null)
    : this(level, level - 1, action)
  {
  }

  public LevelUpPopup(int newLevel, int previousLevel, Action action = null)
  {
    this._action = action;
    this._level = newLevel;
    this.Title = "Level Up";
    this.Text = "Congratulations, your reached level " + (object) this._level + "!";
    this.Width = 388;
    this.Height = 560 - GlobalUIRibbon.Instance.Height() - 10;
    List<ShopItemView> items = new List<ShopItemView>();
    for (int level = newLevel; level > previousLevel; --level)
      items.AddRange((IEnumerable<ShopItemView>) this.GetItemsUnlocked(level));
    this._itemGrid = new ShopItemGrid(items);
    this._itemGrid.Show = true;
  }

  public string Text { get; set; }

  public string Title { get; set; }

  public bool IsWaiting { get; set; }

  public void OnGUI()
  {
    Rect position = this.GetPosition();
    GUI.Box(position, GUIContent.none, BlueStonez.window);
    GUITools.PushGUIState();
    GUI.BeginGroup(position);
    this.DrawPlayGUI(position);
    GUI.EndGroup();
    GUITools.PopGUIState();
    if (!this.IsWaiting)
      return;
    WaitingTexture.Draw(position.center);
  }

  private Rect GetPosition() => new Rect((float) (Screen.width - this.Width) * 0.5f, (float) GlobalUIRibbon.Instance.Height() + (float) (Screen.height - GlobalUIRibbon.Instance.Height() - this.Height) * 0.5f, (float) this.Width, (float) this.Height);

  private List<ShopItemView> GetItemsUnlocked(int level)
  {
    List<ShopItemView> itemsUnlocked = new List<ShopItemView>();
    if (level > 1)
    {
      foreach (IUnityItem shopItem in Singleton<ItemManager>.Instance.ShopItems)
      {
        if (shopItem.ItemView.LevelLock == level)
          itemsUnlocked.Add(new ShopItemView(shopItem.ItemId));
      }
    }
    return itemsUnlocked;
  }

  private void DrawPlayGUI(Rect rect)
  {
    GUI.color = ColorScheme.HudTeamBlue;
    float width1 = BlueStonez.label_interparkbold_18pt.CalcSize(new GUIContent(this.Title)).x * 2.5f;
    GUI.DrawTexture(new Rect((float) (((double) rect.width - (double) width1) * 0.5), -29f, width1, 100f), (Texture) HudTextures.WhiteBlur128);
    GUI.color = Color.white;
    GUITools.OutlineLabel(new Rect(0.0f, 10f, rect.width, 30f), this.Title, BlueStonez.label_interparkbold_18pt, 1, Color.white, ColorScheme.GuiTeamBlue.SetAlpha(0.5f));
    GUI.Label(new Rect(30f, 35f, rect.width - 60f, 40f), this.Text, BlueStonez.label_interparkbold_16pt);
    int width2 = 288;
    int left = (this.Width - width2 - 6) / 2;
    int height = 323;
    int count = this._itemGrid.Items.Count;
    GUI.BeginGroup(new Rect((float) left, 75f, (float) width2, (float) height), BlueStonez.item_slot_large);
    GUI.DrawTexture(new Rect((float) ((width2 - 282) / 2), (float) ((height - 317) / 2), 282f, 317f), (Texture) UberstrikeIcons.LevelUpPopup);
    if (count > 0)
      this._itemGrid.Draw(new Rect(0.0f, 0.0f, (float) width2, (float) height));
    GUI.EndGroup();
    if (count > 0)
      GUI.Label(new Rect(30f, rect.height - 107f, rect.width - 60f, 40f), string.Format("You unlocked {0} new item{1}.", (object) count, count != 1 ? (object) "s" : (object) string.Empty), BlueStonez.label_interparkbold_16pt);
    if (!GUI.Button(new Rect((float) ((double) rect.width * 0.5 - 70.0), rect.height - 47f, 140f, 30f), "OK", BlueStonez.buttongold_large_price))
      return;
    if (ApplicationDataManager.Channel == ChannelType.WebFacebook)
      AutoMonoBehaviour<FacebookInterface>.Instance.PublishFbLevelUp(this._level);
    PopupSystem.HideMessage((IPopupDialog) this);
    if (this._action == null)
      return;
    this._action();
  }

  public GuiDepth Depth => GuiDepth.Event;

  public void OnHide()
  {
  }
}
