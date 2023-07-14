// Decompiled with JetBrains decompiler
// Type: MenuPageManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MenuPageManager : MonoBehaviour
{
  private IDictionary<PageType, PageScene> _pageByPageType;
  private static PageType _currentPageType;
  private EaseType _transitionType = EaseType.InOut;
  private int _lastScreenWidth;
  private int _lastScreenHeight;

  public static MenuPageManager Instance { get; private set; }

  private void Awake()
  {
    MenuPageManager.Instance = this;
    GlobalUIRibbon.IsVisible = true;
    GlobalUIRibbon.Instance.Show();
    this._pageByPageType = (IDictionary<PageType, PageScene>) new Dictionary<PageType, PageScene>();
  }

  private void OnDisabe() => MenuPageManager.Instance = (MenuPageManager) null;

  private void Start()
  {
    foreach (PageScene componentsInChild in this.GetComponentsInChildren<PageScene>(true))
      this._pageByPageType.Add(componentsInChild.PageType, componentsInChild);
    if (MenuPageManager._currentPageType != PageType.None)
      this.LoadPage(MenuPageManager._currentPageType, true);
    else
      this.LoadPage(PageType.Home);
  }

  private void OnGUI()
  {
    int pagePanelWidth = this.GetPagePanelWidth(MenuPageManager._currentPageType);
    if (!GameState.HasCurrentSpace || !this.IsScreenResolutionChanged())
      return;
    GameState.CurrentSpace.Camera.pixelRect = new Rect(0.0f, 0.0f, (float) (Screen.width - pagePanelWidth), (float) Screen.height);
  }

  [DebuggerHidden]
  private IEnumerator StartPageTransition(PageScene newPage, float time) => (IEnumerator) new MenuPageManager.\u003CStartPageTransition\u003Ec__Iterator73()
  {
    newPage = newPage,
    time = time,
    \u003C\u0024\u003EnewPage = newPage,
    \u003C\u0024\u003Etime = time,
    \u003C\u003Ef__this = this
  };

  private int GetPagePanelWidth(PageType type)
  {
    PageScene pageScene;
    return this._pageByPageType.TryGetValue(type, out pageScene) ? pageScene.GuiWidth : 0;
  }

  [DebuggerHidden]
  private IEnumerator AnimateCameraPixelRect(PageType type, float time) => (IEnumerator) new MenuPageManager.\u003CAnimateCameraPixelRect\u003Ec__Iterator74()
  {
    type = type,
    time = time,
    \u003C\u0024\u003Etype = type,
    \u003C\u0024\u003Etime = time,
    \u003C\u003Ef__this = this
  };

  public bool IsCurrentPage(PageType type) => MenuPageManager._currentPageType == type;

  public PageType GetCurrentPage() => MenuPageManager._currentPageType;

  public void UnloadCurrentPage()
  {
    PageScene pageScene;
    if (!this._pageByPageType.TryGetValue(MenuPageManager._currentPageType, out pageScene) || !(bool) (UnityEngine.Object) pageScene)
      return;
    pageScene.Unload();
    MenuPageManager._currentPageType = PageType.None;
    MouseOrbit.Instance.enabled = false;
  }

  public void LoadPage(PageType pageType, bool forceReload = false)
  {
    if (GameState.HasCurrentGame && GameState.CurrentGame.IsGameStarted)
    {
      PopupSystem.ShowMessage(LocalizedStrings.LeavingGame, LocalizedStrings.LeaveGameWarningMsg, PopupSystem.AlertType.OKCancel, (Action) (() => Singleton<GameStateController>.Instance.LeaveGame()), LocalizedStrings.LeaveCaps, (Action) null, LocalizedStrings.CancelCaps, PopupSystem.ActionType.Negative);
    }
    else
    {
      if (GameState.HasCurrentGame)
        Singleton<GameStateController>.Instance.UnloadGameMode();
      PanelManager.Instance.CloseAllPanels();
      if (pageType == MenuPageManager._currentPageType && !forceReload)
        return;
      PageScene newPage = (PageScene) null;
      if (!this._pageByPageType.TryGetValue(pageType, out newPage))
        return;
      PageScene pageScene = (PageScene) null;
      this._pageByPageType.TryGetValue(MenuPageManager._currentPageType, out pageScene);
      if ((bool) (UnityEngine.Object) pageScene && !forceReload)
        pageScene.Unload();
      MenuPageManager._currentPageType = pageType;
      this.StartCoroutine(this.AnimateCameraPixelRect(newPage.PageType, 0.25f));
      MouseOrbit.Instance.enabled = false;
      MenuPageManager.Instance.StartCoroutine(this.StartPageTransition(newPage, 1f));
    }
  }

  private bool IsScreenResolutionChanged()
  {
    if (Screen.width == this._lastScreenWidth && Screen.height == this._lastScreenHeight)
      return false;
    this._lastScreenWidth = Screen.width;
    this._lastScreenHeight = Screen.height;
    return true;
  }
}
