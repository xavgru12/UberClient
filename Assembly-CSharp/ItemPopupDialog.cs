// Decompiled with JetBrains decompiler
// Type: ItemPopupDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ItemPopupDialog : IPopupDialog
{
  private const int Width = 400;
  private const int Height = 300;
  private IUnityItem _item;
  private string _caption = "OK";
  private Action _action;
  private int _frameCount;
  private float _alpha;

  public ItemPopupDialog(
    string title,
    string text,
    IUnityItem items,
    string caption,
    Action action)
  {
    this.Title = title;
    this.Text = text;
    this._item = items;
    this._caption = caption;
    this._action = action;
    Singleton<InventoryManager>.Instance.HighlightItem(this._item.ItemId, true);
  }

  public ItemPopupDialog(string title, string text, IUnityItem items)
    : this(title, text, items, LocalizedStrings.Inventory, new Action(ItemPopupDialog.OpenInventory))
  {
  }

  public string Text { get; set; }

  public string Title { get; set; }

  public GuiDepth Depth => GuiDepth.Popup;

  private static void OpenInventory()
  {
    MenuPageManager.Instance.LoadPage(PageType.Shop);
    CmuneEventHandler.Route((object) new SelectShopAreaEvent()
    {
      ShopArea = ShopArea.Inventory
    });
  }

  public void OnGUI()
  {
    this.UpdateAlpha();
    GUI.color = GUI.color.SetAlpha(this._alpha);
    GUI.BeginGroup(new Rect((float) (Screen.width - 400) * 0.5f, (float) GlobalUIRibbon.Instance.Height() + (float) (Screen.height - GlobalUIRibbon.Instance.Height() - 300) * 0.5f, 400f, 300f), BlueStonez.window);
    GUITools.OutlineLabel(new Rect(0.0f, 10f, 400f, 40f), this.Title, BlueStonez.label_interparkbold_32pt, 1, Color.white, ColorScheme.GuiTeamBlue);
    GUI.Label(new Rect(0.0f, 50f, 400f, 20f), this.Text, BlueStonez.label_interparkbold_16pt);
    GUI.DrawTexture(new Rect(176f, 80f, 48f, 48f), (Texture) this._item.Icon);
    GUI.Label(new Rect(17f, 150f, 366f, 20f), this._item.Name, BlueStonez.label_interparkbold_16pt);
    if (this._item.ItemView != null)
    {
      string text = this._item.ItemView.Description;
      if (string.IsNullOrEmpty(text))
        text = "No description available.";
      GUI.Label(new Rect(17f, 170f, 366f, 80f), text, BlueStonez.label_interparkmed_11pt);
    }
    if (GUI.Button(new Rect(17f, 246f, 366f, 32f), this._caption, BlueStonez.button_green))
    {
      PopupSystem.HideMessage((IPopupDialog) this);
      if (this._action != null)
        this._action();
    }
    GUI.EndGroup();
    GUI.color = Color.white;
  }

  public void OnHide()
  {
  }

  private void UpdateAlpha()
  {
    if (this._frameCount == Time.frameCount)
      return;
    this._frameCount = Time.frameCount;
    this._alpha = Mathf.Clamp01(this._alpha + Time.deltaTime * 3f);
  }
}
