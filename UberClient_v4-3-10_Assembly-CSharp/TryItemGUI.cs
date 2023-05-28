// Decompiled with JetBrains decompiler
// Type: TryItemGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class TryItemGUI : MonoBehaviour
{
  private LoadoutArea _currentLoadoutArea;

  private void OnGUI()
  {
    if (PopupSystem.IsAnyPopupOpen || PanelManager.IsAnyPanelOpen)
      return;
    switch (this._currentLoadoutArea)
    {
      case LoadoutArea.Weapons:
      case LoadoutArea.QuickItems:
        this.DrawTryWeapon();
        break;
      case LoadoutArea.Gear:
        this.DrawResetGear();
        break;
    }
  }

  private void OnEnable() => CmuneEventHandler.AddListener<LoadoutAreaChangedEvent>(new Action<LoadoutAreaChangedEvent>(this.OnLoadoutAreaChanged));

  private void OnDisable() => CmuneEventHandler.RemoveListener<LoadoutAreaChangedEvent>(new Action<LoadoutAreaChangedEvent>(this.OnLoadoutAreaChanged));

  private void DrawTryWeapon()
  {
    float width = Mathf.Max((float) (Screen.width - 584) * 0.5f, 170f);
    if (!GUITools.Button(new Rect(2f + (float) (((double) (Screen.width - 584) - (double) width) * 0.5), (float) (Screen.height - 60), width, 32f), new GUIContent(LocalizedStrings.TryYourWeapons), BlueStonez.button_white))
      return;
    Singleton<GameStateController>.Instance.LoadTryWeaponMode();
  }

  private void DrawResetGear()
  {
    float width = Mathf.Max((float) (Screen.width - 584) * 0.5f, 170f);
    float num = (float) (((double) (Screen.width - 584) - (double) width) * 0.5);
    if (!Singleton<TemporaryLoadoutManager>.Instance.IsGearLoadoutModified || !GUITools.Button(new Rect(2f + num, (float) (Screen.height - 60), width, 32f), new GUIContent("Reset Avatar"), BlueStonez.button_white))
      return;
    Singleton<TemporaryLoadoutManager>.Instance.ResetGearLoadout();
  }

  public void OnLoadoutAreaChanged(LoadoutAreaChangedEvent ev) => this._currentLoadoutArea = ev.Area;
}
