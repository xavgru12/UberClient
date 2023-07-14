// Decompiled with JetBrains decompiler
// Type: PopupMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenu
{
  private const int Height = 24;
  private const int Width = 105;
  private Rect _position;
  private List<PopupMenu.MenuItem> _items;
  private CommUser _selectedUser;

  public PopupMenu() => this._items = new List<PopupMenu.MenuItem>();

  public void AddMenuItem(
    string item,
    Action<CommUser> action,
    PopupMenu.IsEnabledForUser isEnabledForUser)
  {
    this._items.Add(new PopupMenu.MenuItem()
    {
      Item = item,
      Callback = action,
      CheckItem = isEnabledForUser,
      Enabled = false
    });
  }

  private void Configure()
  {
    for (int index = 0; index < this._items.Count; ++index)
      this._items[index].Enabled = this._items[index].CheckItem(this._selectedUser);
  }

  public static void Hide() => PopupMenu.Current = (PopupMenu) null;

  public void Show(Vector2 screenPos, CommUser user) => PopupMenu.Show(screenPos, user, this);

  public static void Show(Vector2 screenPos, CommUser user, PopupMenu menu)
  {
    if (menu == null)
      return;
    menu._selectedUser = user;
    menu.Configure();
    menu._position.height = (float) (24 * menu._items.Count);
    menu._position.width = 105f;
    menu._position.x = screenPos.x - 1f;
    menu._position.y = (double) screenPos.y + (double) menu._position.height <= (double) Screen.height ? screenPos.y - 1f : (float) ((double) screenPos.y - (double) menu._position.height + 1.0);
    PopupMenu.Current = menu;
  }

  public void Draw()
  {
    GUI.BeginGroup(new Rect(this._position.x, this._position.y, this._position.width, this._position.height + 6f), BlueStonez.window);
    GUI.Label(new Rect(1f, 1f, this._position.width - 2f, this._position.height + 4f), GUIContent.none, BlueStonez.box_grey50);
    GUI.Label(new Rect(0.0f, 0.0f, this._position.width, this._position.height + 6f), GUIContent.none, BlueStonez.box_grey50);
    for (int index = 0; index < this._items.Count; ++index)
    {
      GUITools.PushGUIState();
      GUI.enabled = this._items[index].Enabled;
      GUI.Label(new Rect(8f, (float) (8 + index * 24), this._position.width - 8f, 24f), this._items[index].Item, BlueStonez.label_interparkmed_11pt_left);
      if (GUI.Button(new Rect(2f, (float) (3 + index * 24), this._position.width - 4f, 24f), GUIContent.none, BlueStonez.dropdown_list))
      {
        PopupMenu.Current = (PopupMenu) null;
        this._items[index].Callback(this._selectedUser);
      }
      GUITools.PopGUIState();
    }
    GUI.EndGroup();
    if (Event.current.type != UnityEngine.EventType.MouseUp || this._position.Contains(Event.current.mousePosition))
      return;
    PopupMenu.Current = (PopupMenu) null;
  }

  public CommUser SelectedUser => this._selectedUser;

  public static PopupMenu Current { get; private set; }

  public static bool IsEnabled => PopupMenu.Current != null;

  private class MenuItem
  {
    public string Item;
    public Action<CommUser> Callback;
    public PopupMenu.IsEnabledForUser CheckItem;
    public bool Enabled;
  }

  public delegate bool IsEnabledForUser(CommUser user);
}
