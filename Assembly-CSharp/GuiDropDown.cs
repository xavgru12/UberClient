// Decompiled with JetBrains decompiler
// Type: GuiDropDown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GuiDropDown
{
  private List<GuiDropDown.Button> _data = new List<GuiDropDown.Button>();
  private bool _isDown;
  private Rect _rect;
  public bool ShowRightAligned = true;
  public float ButtonWidth = 100f;
  public float ButtonHeight = 20f;

  public GUIContent Caption { get; set; }

  public void Add(GUIContent content, Action onClick) => this._data.Add(new GuiDropDown.Button(content)
  {
    Action = onClick
  });

  public void Add(GUIContent onContent, GUIContent offContent, Func<bool> isOn, Action onClick) => this._data.Add(new GuiDropDown.Button(onContent, offContent, isOn)
  {
    Action = onClick
  });

  public void SetRect(Rect rect) => this._rect = GUITools.ToGlobal(rect);

  public void Draw()
  {
    bool isDown = this._isDown;
    this._isDown = GUI.Toggle(this._rect, this._isDown, this.Caption, BlueStonez.buttondark_medium);
    if (isDown != this._isDown)
      MouseOrbit.Disable = false;
    if (!this._isDown)
      return;
    MouseOrbit.Disable = true;
    Rect position = !this.ShowRightAligned ? new Rect(this._rect.x, this._rect.yMax, this.ButtonWidth, this.ButtonHeight * (float) this._data.Count) : new Rect(this._rect.xMax - this.ButtonWidth, this._rect.yMax, this.ButtonWidth, this.ButtonHeight * (float) this._data.Count);
    if (!position.Contains(Event.current.mousePosition) && !this._rect.Contains(Event.current.mousePosition) && (Event.current.type == UnityEngine.EventType.MouseUp || Event.current.type == UnityEngine.EventType.Used))
    {
      this._isDown = false;
      MouseOrbit.Disable = false;
    }
    GUI.BeginGroup(position);
    for (int index = 0; index < this._data.Count; ++index)
    {
      if (this._data[index].IsEnabled())
      {
        GUIStyle style = BlueStonez.dropdown;
        if (ApplicationDataManager.IsMobile)
          style = BlueStonez.dropdown_large;
        if (GUI.Button(new Rect(0.0f, (float) index * this.ButtonHeight, this.ButtonWidth, this.ButtonHeight), this._data[index].Content, style))
        {
          this._isDown = false;
          MouseOrbit.Disable = false;
          this._data[index].Action();
        }
      }
    }
    GUI.EndGroup();
  }

  private class Button
  {
    private GUIContent onContent;
    private GUIContent offContent;
    private Func<bool> isOn;
    public Action Action;
    public Func<bool> IsEnabled;

    public Button(GUIContent onContent)
      : this(onContent, onContent, (Func<bool>) (() => true))
    {
    }

    public Button(GUIContent onContent, GUIContent offContent, Func<bool> isOn)
    {
      this.onContent = onContent;
      this.offContent = offContent;
      this.isOn = isOn;
      this.IsEnabled = (Func<bool>) (() => true);
      this.Action = (Action) (() => { });
    }

    public GUIContent Content => this.isOn == null || this.isOn() ? this.onContent : this.offContent;
  }
}
