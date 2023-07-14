// Decompiled with JetBrains decompiler
// Type: BaseEventPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public abstract class BaseEventPopup : IPopupDialog
{
  private const float BerpSpeed = 2.5f;
  protected int Width = 650;
  protected int Height = 330;
  protected bool ClickAnywhereToExit = true;
  private float _startTime;
  protected Action _onCloseButtonClicked;

  public string Text { get; set; }

  public string Title { get; set; }

  public GuiDepth Depth => GuiDepth.Event;

  public float Scale => (double) this._startTime > (double) Time.time - 1.0 ? Mathfx.Berp(0.01f, 1f, (float) (((double) Time.time - (double) this._startTime) * 2.5)) : 1f;

  protected abstract void DrawGUI(Rect rect);

  public virtual void OnHide()
  {
  }

  public void OnGUI()
  {
    if ((double) this._startTime == 0.0)
      this._startTime = Time.time;
    GUI.color = Color.white.SetAlpha(this.Scale);
    float left = (float) (Screen.width - this.Width) * 0.5f;
    float top = (float) GlobalUIRibbon.Instance.Height() + (float) (Screen.height - GlobalUIRibbon.Instance.Height() - this.Height) * 0.5f;
    Rect rect = new Rect(left, top, (float) this.Width, (float) (64 + this.Height) - 64f * this.Scale);
    GUI.Box(new Rect(left - 1f, top - 1f, rect.width + 2f, rect.height + 2f), GUIContent.none, BlueStonez.window);
    GUI.BeginGroup(rect);
    if (GUI.Button(new Rect(rect.width - 20f, 0.0f, 20f, 20f), "X", BlueStonez.friends_hidden_button))
      this.Close();
    this.DrawGUI(rect);
    GUI.EndGroup();
    GUI.color = Color.white;
    if (this.ClickAnywhereToExit && Event.current.type == UnityEngine.EventType.MouseDown && !rect.Contains(Event.current.mousePosition))
    {
      Event.current.Use();
      this.Close();
    }
    this.OnAfterGUI();
  }

  public virtual void OnAfterGUI()
  {
  }

  private void Close()
  {
    PopupSystem.HideMessage((IPopupDialog) this);
    if (this._onCloseButtonClicked == null)
      return;
    this._onCloseButtonClicked();
  }
}
