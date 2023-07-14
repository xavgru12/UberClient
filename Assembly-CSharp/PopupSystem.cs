// Decompiled with JetBrains decompiler
// Type: PopupSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupSystem : AutoMonoBehaviour<PopupSystem>
{
  private GuiDepth _lastLockDepth;
  private readonly PopupStack<IPopupDialog> _popups = new PopupStack<IPopupDialog>();

  private void OnGUI()
  {
    this.ReleaseOldLock();
    if (this._popups.Count > 0)
    {
      IPopupDialog popupDialog = this._popups.Peek();
      this._lastLockDepth = popupDialog.Depth;
      GUI.depth = (int) this._lastLockDepth;
      popupDialog.OnGUI();
      if (Event.current.type == UnityEngine.EventType.Layout)
        GuiLockController.EnableLock(this._lastLockDepth);
    }
    GuiManager.DrawTooltip();
  }

  private void ReleaseOldLock()
  {
    if (Event.current.type != UnityEngine.EventType.Layout)
      return;
    if (this._popups.Count > 0)
    {
      if (this._lastLockDepth == this._popups.Peek().Depth)
        return;
      GuiLockController.ReleaseLock(this._lastLockDepth);
    }
    else
    {
      GuiLockController.ReleaseLock(this._lastLockDepth);
      this.enabled = false;
    }
  }

  public static void Show(IPopupDialog popup)
  {
    AutoMonoBehaviour<PopupSystem>.Instance._popups.Push(popup);
    AutoMonoBehaviour<PopupSystem>.Instance.enabled = true;
  }

  public static void ShowMessage(string title, string text, PopupSystem.AlertType flag, Action ok) => PopupSystem.ShowMessage(title, text, flag, ok, (Action) null);

  public static void ShowError(string title, string text, PopupSystem.AlertType flag, Action ok) => PopupSystem.ShowError(title, text, flag, ok, (Action) null);

  public static void ShowMessage(
    string title,
    string text,
    PopupSystem.AlertType flag,
    Action ok,
    Action cancel)
  {
    PopupSystem.Show((IPopupDialog) new GeneralPopupDialog(title, text, flag, ok, cancel));
  }

  public static void ShowError(
    string title,
    string text,
    PopupSystem.AlertType flag,
    Action ok,
    Action cancel)
  {
    PopupSystem.Show((IPopupDialog) new GeneralPopupDialog(title, text, flag, ok, cancel, false));
  }

  public static IPopupDialog ShowMessage(
    string title,
    string text,
    PopupSystem.AlertType flag,
    Action ok,
    string okCaption,
    Action cancel,
    string cancelCaption,
    PopupSystem.ActionType type)
  {
    IPopupDialog popup = (IPopupDialog) new GeneralPopupDialog(title, text, flag, ok, okCaption, cancel, cancelCaption, type);
    PopupSystem.Show(popup);
    return popup;
  }

  public static IPopupDialog ShowMessage(
    string title,
    string text,
    PopupSystem.AlertType flag,
    Action ok,
    string okCaption,
    Action cancel,
    string cancelCaption)
  {
    IPopupDialog popup = (IPopupDialog) new GeneralPopupDialog(title, text, flag, ok, okCaption, cancel, cancelCaption, PopupSystem.ActionType.None);
    PopupSystem.Show(popup);
    return popup;
  }

  public static IPopupDialog ShowMessage(
    string title,
    string text,
    PopupSystem.AlertType flag,
    string okCaption,
    Action ok)
  {
    IPopupDialog popup = (IPopupDialog) new GeneralPopupDialog(title, text, flag, ok, okCaption);
    PopupSystem.Show(popup);
    return popup;
  }

  public static ProgressPopupDialog ShowProgress(
    string title,
    string text,
    ProgressPopupDialog.ProgressFunction progress = null)
  {
    ProgressPopupDialog popup = new ProgressPopupDialog(title, text, progress);
    PopupSystem.Show((IPopupDialog) popup);
    return popup;
  }

  public static IPopupDialog ShowItems(
    string title,
    string text,
    List<IUnityItem> items,
    ShopArea area)
  {
    IPopupDialog popup = (IPopupDialog) new ItemListPopupDialog(title, text, items, area);
    PopupSystem.Show(popup);
    return popup;
  }

  public static IPopupDialog ShowItem(IUnityItem item)
  {
    IPopupDialog popup = (IPopupDialog) new ItemListPopupDialog(item);
    PopupSystem.Show(popup);
    return popup;
  }

  public static IPopupDialog ShowMessage(string title, string text)
  {
    IPopupDialog popup = (IPopupDialog) new GeneralPopupDialog(title, text, PopupSystem.AlertType.OK);
    PopupSystem.Show(popup);
    return popup;
  }

  public static IPopupDialog ShowMessage(string title, string text, PopupSystem.AlertType flag)
  {
    IPopupDialog popup = (IPopupDialog) new GeneralPopupDialog(title, text, flag);
    PopupSystem.Show(popup);
    return popup;
  }

  public static IPopupDialog ShowError(string title, string text, PopupSystem.AlertType flag)
  {
    IPopupDialog popup = (IPopupDialog) new GeneralPopupDialog(title, text, flag);
    PopupSystem.Show(popup);
    return popup;
  }

  public static void HideMessage(IPopupDialog dialog)
  {
    if (dialog == null)
      return;
    AutoMonoBehaviour<PopupSystem>.Instance._popups.Remove(dialog);
    dialog.OnHide();
  }

  public static bool IsAnyPopupOpen => AutoMonoBehaviour<PopupSystem>.Instance._popups.Count > 0;

  public static void ClearAll() => AutoMonoBehaviour<PopupSystem>.Instance._popups.Clear();

  private static bool IsCurrentPopup(IPopupDialog dialog) => AutoMonoBehaviour<PopupSystem>.Instance._popups.Count > 0 && AutoMonoBehaviour<PopupSystem>.Instance._popups.Peek() == dialog;

  public static string CurrentPopupName => AutoMonoBehaviour<PopupSystem>.Instance._popups.Count > 0 ? AutoMonoBehaviour<PopupSystem>.Instance._popups.Peek().ToString() : string.Empty;

  public enum AlertType
  {
    OK,
    OKCancel,
    Cancel,
    None,
  }

  public enum ActionType
  {
    None,
    Negative,
    Positive,
  }
}
