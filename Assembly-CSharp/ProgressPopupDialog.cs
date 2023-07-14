// Decompiled with JetBrains decompiler
// Type: ProgressPopupDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ProgressPopupDialog : GeneralPopupDialog
{
  private ProgressPopupDialog.ProgressFunction _progress;

  public ProgressPopupDialog(
    string title,
    string text,
    ProgressPopupDialog.ProgressFunction progress = null)
    : base(title, text)
  {
    this._progress = progress;
  }

  public float Progress { get; set; }

  protected override void DrawPopupWindow()
  {
    GUI.Label(new Rect(17f, 95f, this._size.x - 34f, 32f), this.Text, BlueStonez.label_interparkbold_11pt);
    if (this._progress != null)
      this.DrawLevelBar(new Rect(17f, 125f, this._size.x - 34f, 16f), this._progress(), ColorScheme.ProgressBar);
    else
      this.DrawLevelBar(new Rect(17f, 125f, this._size.x - 34f, 16f), this.Progress, ColorScheme.ProgressBar);
  }

  private void DrawLevelBar(Rect position, float amount, Color barColor)
  {
    GUI.BeginGroup(position);
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 12f), GUIContent.none, BlueStonez.progressbar_background);
    GUI.color = barColor;
    GUI.Label(new Rect(2f, 2f, (position.width - 4f) * Mathf.Clamp01(amount), 8f), string.Empty, BlueStonez.progressbar_thumb);
    GUI.color = Color.white;
    GUI.EndGroup();
  }

  public void SetCancelable(Action action)
  {
    this._callbackCancel = action;
    this._cancelCaption = LocalizedStrings.Cancel;
    this._alertType = action == null ? PopupSystem.AlertType.None : PopupSystem.AlertType.Cancel;
    this._actionType = PopupSystem.ActionType.None;
  }

  public delegate float ProgressFunction();
}
