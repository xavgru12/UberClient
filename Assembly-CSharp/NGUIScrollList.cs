using System.Collections.Generic;
using UnityEngine;

public class NGUIScrollList : MonoBehaviour
{
	public enum ScrollType
	{
		NotCircular,
		Visible1,
		Visible3
	}

	public ScrollType scrollType;

	[SerializeField]
	private UIDraggablePanel dragPanel;

	[SerializeField]
	private UIGrid grid;

	public UICenterOnChild centeringTool;

	[SerializeField]
	private List<GameObject> elements = new List<GameObject>();

	public Property<GameObject> SelectedElement = new Property<GameObject>();

	public IntegerProperty SelectedIndex = new IntegerProperty(0, 0);

	private List<GameObject> activeElements;

	private GameObject helperElement;

	private GameObject helperElementReference;

	public UIDraggablePanel Panel => dragPanel;

	public List<GameObject> ActiveElements
	{
		get
		{
			if (activeElements == null)
			{
				return elements;
			}
			return activeElements;
		}
	}

	private void OnEnable()
	{
		centeringTool.onFinished = delegate
		{
			UpdateSelection(centeringTool.centeredObject);
		};
	}

	public void UpdateCircularList()
	{
		if (scrollType == ScrollType.NotCircular || ActiveElements.Count <= 1)
		{
			return;
		}
		NGUITools.SetActiveChildren(grid.gameObject, state: false);
		GameObject gameObject = ActiveElements[GetPreviousIndex(SelectedElement.Value)];
		GameObject gameObject2 = ActiveElements[GetNextIndex(SelectedElement.Value)];
		GameObject gameObject3 = null;
		GameObject gameObject4 = null;
		if (scrollType == ScrollType.Visible3)
		{
			gameObject3 = ActiveElements[GetPreviousIndex(gameObject)];
			gameObject4 = ActiveElements[GetNextIndex(gameObject2)];
		}
		if (gameObject == gameObject2)
		{
			if (helperElement != null)
			{
				Object.DestroyImmediate(helperElement);
			}
			helperElement = NGUITools.AddChild(grid.gameObject, gameObject2);
			gameObject = helperElement;
			helperElementReference = gameObject2;
		}
		if (gameObject3 == null || gameObject4 == null)
		{
			gameObject.name = "0";
			SelectedElement.Value.name = "1";
			gameObject2.name = "2";
		}
		else
		{
			gameObject3.name = "0";
			gameObject.name = "1";
			SelectedElement.Value.name = "2";
			gameObject2.name = "3";
			gameObject4.name = ((!(gameObject == gameObject4)) ? "4" : "0");
			gameObject3.SetActive(value: true);
			gameObject4.SetActive(value: true);
		}
		gameObject.SetActive(value: true);
		SelectedElement.Value.SetActive(value: true);
		gameObject2.SetActive(value: true);
		grid.sorted = true;
		grid.Reposition();
		SpringToElement(SelectedElement.Value);
	}

	private int GetPreviousIndex(GameObject element)
	{
		int num = Mathf.Clamp(ActiveElements.IndexOf(element), 0, ActiveElements.Count - 1);
		if (num == 0)
		{
			return ActiveElements.Count - 1;
		}
		return num - 1;
	}

	private int GetNextIndex(GameObject element)
	{
		int num = Mathf.Clamp(ActiveElements.IndexOf(element), 0, ActiveElements.Count - 1);
		if (num == ActiveElements.Count - 1)
		{
			return 0;
		}
		return num + 1;
	}

	public void Reposition()
	{
		grid.Reposition();
	}

	public void SpringToElement(GameObject element, float springDuration = 100f)
	{
		if (element != null)
		{
			dragPanel.SpringToSelection(element, springDuration);
		}
	}

	public void SelectElement(GameObject element, float springDuration)
	{
		if (element != null)
		{
			SpringToElement(element, springDuration);
			UpdateSelection(element);
		}
	}

	private void UpdateSelection(GameObject newSelection)
	{
		if (SelectedElement.Value != newSelection)
		{
			SelectedElement.Value = ((!(newSelection == helperElement)) ? newSelection : helperElementReference);
			if (ActiveElements.Contains(SelectedElement.Value))
			{
				SelectedIndex.Value = ActiveElements.IndexOf(SelectedElement.Value);
			}
			UpdateCircularList();
		}
	}

	public void SetActiveElements(List<GameObject> newElements)
	{
		elements.ForEach(delegate(GameObject el)
		{
			if (el != null)
			{
				NGUITools.SetActive(el, newElements.Contains(el));
			}
		});
		activeElements = newElements;
		if (helperElement != null)
		{
			Object.DestroyImmediate(helperElement);
		}
		grid.Reposition();
	}
}
