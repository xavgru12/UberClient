// Decompiled with JetBrains decompiler
// Type: BasePopupDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public abstract class BasePopupDialog : IPopupDialog
{
  protected Vector2 _size = new Vector2(320f, 240f);
  protected PopupSystem.ActionType _actionType;
  protected PopupSystem.AlertType _alertType;
  protected string _okCaption = string.Empty;
  protected string _cancelCaption = string.Empty;
  protected bool _allowAudio = true;
  protected Action _callbackOk;
  protected Action _callbackCancel;
  protected Action _onGUIAction;

  public string Text { get; set; }

  public string Title { get; set; }

  public virtual void OnHide()
  {
  }

  public void SetAlertType(PopupSystem.AlertType type) => this._alertType = type;

  public void OnGUI()
  {
    GUI.BeginGroup(new Rect((float) (((double) Screen.width - (double) this._size.x) * 0.5), (float) (((double) Screen.height - (double) this._size.y - 56.0) * 0.5), this._size.x, this._size.y), GUIContent.none, PopupSkin.window);
    GUI.Label(new Rect(0.0f, 0.0f, this._size.x, 56f), this.Title, PopupSkin.title);
    this.DrawPopupWindow();
    switch (this._alertType)
    {
      case PopupSystem.AlertType.OK:
        this.DoOKButton();
        break;
      case PopupSystem.AlertType.OKCancel:
        this.DoOKCancelButtons();
        break;
      case PopupSystem.AlertType.Cancel:
        this.DoCancelButton();
        break;
    }
    GUI.EndGroup();
  }

  protected abstract void DrawPopupWindow();

  public GuiDepth Depth => GuiDepth.Popup;

  private void DoOKButton()
  {
    GUIStyle style = PopupSkin.button;
    switch (this._actionType)
    {
      case PopupSystem.ActionType.Negative:
        style = PopupSkin.button_red;
        break;
      case PopupSystem.ActionType.Positive:
        style = PopupSkin.button_green;
        break;
    }
    Rect rect = new Rect((float) (((double) this._size.x - 120.0) * 0.5), this._size.y - 40f, 120f, 32f);
    GUIContent content = new GUIContent(!string.IsNullOrEmpty(this._okCaption) ? this._okCaption : LocalizedStrings.OkCaps);
    if (!(!this._allowAudio ? GUI.Button(rect, content, style) : GUITools.Button(rect, content, style)))
      return;
    PopupSystem.HideMessage((IPopupDialog) this);
    if (this._callbackOk == null)
      return;
    this._callbackOk();
  }

  private void DoOKCancelButtons()
  {
    Rect rect = new Rect((float) ((double) this._size.x * 0.5 + 5.0), this._size.y - 40f, 120f, 32f);
    GUIContent content1 = new GUIContent(!string.IsNullOrEmpty(this._cancelCaption) ? this._cancelCaption : LocalizedStrings.CancelCaps);
    GUI.color = Color.white;
    if (!this._allowAudio ? GUI.Button(rect, content1, PopupSkin.button) : GUITools.Button(rect, content1, PopupSkin.button))
    {
      PopupSystem.HideMessage((IPopupDialog) this);
      if (this._callbackCancel != null)
        this._callbackCancel();
    }
    GUIStyle style = PopupSkin.button;
    switch (this._actionType)
    {
      case PopupSystem.ActionType.Negative:
        style = PopupSkin.button_red;
        break;
      case PopupSystem.ActionType.Positive:
        style = PopupSkin.button_green;
        break;
    }
    rect = new Rect((float) ((double) this._size.x * 0.5 - 125.0), this._size.y - 40f, 120f, 32f);
    GUIContent content2 = new GUIContent(!string.IsNullOrEmpty(this._okCaption) ? this._okCaption : LocalizedStrings.OkCaps);
    if (!(!this._allowAudio ? GUI.Button(rect, content2, style) : GUITools.Button(rect, content2, style)))
      return;
    PopupSystem.HideMessage((IPopupDialog) this);
    if (this._callbackOk == null)
      return;
    this._callbackOk();
  }

  private void DoCancelButton()
  {
    GUIStyle guiStyle = PopupSkin.button;
    switch (this._actionType)
    {
      case PopupSystem.ActionType.Negative:
        guiStyle = PopupSkin.button_red;
        break;
      case PopupSystem.ActionType.Positive:
        guiStyle = PopupSkin.button_green;
        break;
    }
    Rect rect = new Rect((float) (((double) this._size.x - 120.0) * 0.5), this._size.y - 40f, 120f, 32f);
    GUIContent content = new GUIContent(!string.IsNullOrEmpty(this._cancelCaption) ? this._cancelCaption : LocalizedStrings.CancelCaps);
    if (!(!this._allowAudio ? GUI.Button(rect, content, PopupSkin.button) : GUITools.Button(rect, content, PopupSkin.button)))
      return;
    PopupSystem.HideMessage((IPopupDialog) this);
    if (this._callbackCancel == null)
      return;
    this._callbackCancel();
  }
}
