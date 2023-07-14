// Decompiled with JetBrains decompiler
// Type: GeneralPopupDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class GeneralPopupDialog : BasePopupDialog
{
  public GeneralPopupDialog(
    string title,
    string text,
    PopupSystem.AlertType flag,
    Action ok,
    string okCaption,
    Action cancel,
    string cancelCaption,
    PopupSystem.ActionType actionType,
    bool allowAudio = true)
  {
    this.Text = text;
    this.Title = title;
    this._alertType = flag;
    this._actionType = actionType;
    this._callbackOk = ok;
    this._callbackCancel = cancel;
    this._okCaption = okCaption;
    this._cancelCaption = cancelCaption;
    this._allowAudio = allowAudio;
  }

  public GeneralPopupDialog(string title, string text)
    : this(title, text, PopupSystem.AlertType.None, (Action) null, string.Empty, (Action) null, string.Empty, PopupSystem.ActionType.None)
  {
  }

  public GeneralPopupDialog(
    string title,
    string text,
    PopupSystem.AlertType flag,
    bool allowAudio = true)
    : this(title, text, flag, (Action) null, string.Empty, (Action) null, string.Empty, PopupSystem.ActionType.None, allowAudio)
  {
  }

  public GeneralPopupDialog(
    string title,
    string text,
    PopupSystem.AlertType flag,
    Action ok,
    string okCaption,
    bool allowAudio = true)
    : this(title, text, flag, ok, okCaption, (Action) null, string.Empty, PopupSystem.ActionType.None, allowAudio)
  {
  }

  public GeneralPopupDialog(
    string title,
    string text,
    PopupSystem.AlertType flag,
    Action action,
    bool allowAudio = true)
    : this(title, text, flag, action, string.Empty, (Action) null, string.Empty, PopupSystem.ActionType.None, allowAudio)
  {
  }

  public GeneralPopupDialog(
    string title,
    string text,
    PopupSystem.AlertType flag,
    Action ok,
    Action cancel,
    bool allowAudio = true)
    : this(title, text, flag, ok, string.Empty, cancel, string.Empty, PopupSystem.ActionType.None, allowAudio)
  {
  }

  protected override void DrawPopupWindow() => GUI.Label(new Rect(17f, 55f, this._size.x - 34f, this._size.y - 100f), this.Text, PopupSkin.label);
}
