using UnityEngine;

public abstract class PageGUI : MonoBehaviour
{
	[SerializeField]
	private string _title;

	public bool IsOnGUIEnabled
	{
		get;
		set;
	}

	public string Title => _title;

	public abstract void DrawGUI(Rect rect);
}
