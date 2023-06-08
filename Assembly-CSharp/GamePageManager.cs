using System.Collections.Generic;
using UnityEngine;

public class GamePageManager : MonoBehaviour
{
	private static IDictionary<IngamePageType, SceneGuiController> _pageByPageType;

	private static IngamePageType _currentPageType;

	public static GamePageManager Instance
	{
		get;
		private set;
	}

	public static bool Exists => Instance != null;

	public static bool HasPage => _currentPageType != IngamePageType.None;

	private void Awake()
	{
		Instance = this;
		_pageByPageType = new Dictionary<IngamePageType, SceneGuiController>();
	}

	private void Start()
	{
		SceneGuiController[] componentsInChildren = GetComponentsInChildren<SceneGuiController>(includeInactive: true);
		SceneGuiController[] array = componentsInChildren;
		foreach (SceneGuiController sceneGuiController in array)
		{
			_pageByPageType[sceneGuiController.PageType] = sceneGuiController;
		}
	}

	public static bool IsCurrentPage(IngamePageType type)
	{
		return _currentPageType == type;
	}

	public SceneGuiController GetCurrentPage()
	{
		_pageByPageType.TryGetValue(_currentPageType, out SceneGuiController value);
		return value;
	}

	public void UnloadCurrentPage()
	{
		SceneGuiController currentPage = GetCurrentPage();
		if ((bool)currentPage)
		{
			currentPage.gameObject.SetActive(value: false);
			_currentPageType = IngamePageType.None;
		}
		EventHandler.Global.Fire(new GlobalEvents.GamePageChanged());
	}

	public void LoadPage(IngamePageType pageType)
	{
		if (pageType == _currentPageType)
		{
			return;
		}
		SceneGuiController value = null;
		if (_pageByPageType.TryGetValue(pageType, out value))
		{
			SceneGuiController value2 = null;
			_pageByPageType.TryGetValue(_currentPageType, out value2);
			if ((bool)value2)
			{
				value2.gameObject.SetActive(value: false);
			}
			_currentPageType = pageType;
			value.gameObject.SetActive(value: true);
			EventHandler.Global.Fire(new GlobalEvents.GamePageChanged());
		}
	}
}
