using UnityEngine;

internal class SearchBarGUI
{
	private string _guiName;

	public string FilterText
	{
		get;
		private set;
	}

	public bool IsSearching => !string.IsNullOrEmpty(FilterText);

	public SearchBarGUI(string name)
	{
		_guiName = name;
		FilterText = string.Empty;
	}

	public void Draw(Rect rect)
	{
		int num = 20;
		if (ApplicationDataManager.IsMobile)
		{
			num = 30;
		}
		if (!TabScreenPanelGUI.Enabled)
		{
			GUI.SetNextControlName(_guiName);
			FilterText = GUI.TextField(new Rect(rect.x, rect.y, (!IsSearching) ? rect.width : (rect.width - (float)num - 2f), rect.height), FilterText, BlueStonez.textField);
		}
		if (string.IsNullOrEmpty(FilterText) && GUI.GetNameOfFocusedControl() != _guiName)
		{
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			GUI.Label(rect, " " + LocalizedStrings.Search, BlueStonez.label_interparkbold_11pt_left);
			GUI.color = Color.white;
		}
		if (IsSearching && GUITools.Button(new Rect(rect.x + rect.width - (float)num, 8f, num, num), new GUIContent("x"), BlueStonez.buttondark_medium))
		{
			ClearFilter();
			GUIUtility.hotControl = 1;
		}
	}

	public void ClearFilter()
	{
		FilterText = string.Empty;
	}

	public bool CheckIfPassFilter(string text)
	{
		if (IsSearching)
		{
			return text.ToLower().Contains(FilterText.ToLower());
		}
		return true;
	}
}
