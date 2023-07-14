// Decompiled with JetBrains decompiler
// Type: GamePageManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GamePageManager : MonoBehaviour
{
  private static IDictionary<PageType, PageScene> _pageByPageType;
  private static PageType _currentPageType;

  public static GamePageManager Instance { get; private set; }

  public static bool Exists => (Object) GamePageManager.Instance != (Object) null;

  private void Awake()
  {
    GamePageManager.Instance = this;
    GamePageManager._pageByPageType = (IDictionary<PageType, PageScene>) new Dictionary<PageType, PageScene>();
  }

  private void Start()
  {
    foreach (PageScene componentsInChild in this.GetComponentsInChildren<PageScene>(true))
      GamePageManager._pageByPageType.Add(componentsInChild.PageType, componentsInChild);
  }

  public static bool IsCurrentPage(PageType type) => GamePageManager._currentPageType == type;

  public PageScene GetCurrentPage()
  {
    PageScene currentPage;
    GamePageManager._pageByPageType.TryGetValue(GamePageManager._currentPageType, out currentPage);
    return currentPage;
  }

  public bool HasPage => GamePageManager._currentPageType != PageType.None;

  public void UnloadCurrentPage()
  {
    PageScene currentPage = this.GetCurrentPage();
    if (!(bool) (Object) currentPage)
      return;
    currentPage.Unload();
    GamePageManager._currentPageType = PageType.None;
  }

  public void LoadPage(PageType pageType)
  {
    if (pageType == GamePageManager._currentPageType)
      return;
    PageScene pageScene1 = (PageScene) null;
    if (!GamePageManager._pageByPageType.TryGetValue(pageType, out pageScene1))
      return;
    PageScene pageScene2 = (PageScene) null;
    GamePageManager._pageByPageType.TryGetValue(GamePageManager._currentPageType, out pageScene2);
    if ((bool) (Object) pageScene2)
      pageScene2.Unload();
    GamePageManager._currentPageType = pageType;
    pageScene1.Load();
  }
}
