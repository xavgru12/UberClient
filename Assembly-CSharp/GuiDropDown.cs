using System;
using System.Collections.Generic;
using UnityEngine;

public class GuiDropDown
{
	private class Button
	{
		private GUIContent onContent;

		private GUIContent offContent;

		private Func<bool> isOn;

		public Action Action;

		public Func<bool> IsEnabled;

		public GUIContent Content
		{
			get
			{
				if (isOn == null || isOn())
				{
					return onContent;
				}
				return offContent;
			}
		}

		public Button(GUIContent onContent)
			: this(onContent, onContent, () => true)
		{
		}

		public Button(GUIContent onContent, GUIContent offContent, Func<bool> isOn)
		{
			this.onContent = onContent;
			this.offContent = offContent;
			this.isOn = isOn;
			IsEnabled = (() => true);
			Action = delegate
			{
			};
		}
	}

	private List<Button> _data = new List<Button>();

	private bool _isDown;

	private Rect _rect;

	public bool ShowRightAligned = true;

	public float ButtonWidth = 100f;

	public float ButtonHeight = 20f;

	public GUIContent Caption
	{
		get;
		set;
	}

	public void Add(GUIContent content, Action onClick)
	{
		_data.Add(new Button(content)
		{
			Action = onClick
		});
	}

	public void Add(GUIContent onContent, GUIContent offContent, Func<bool> isOn, Action onClick)
	{
		_data.Add(new Button(onContent, offContent, isOn)
		{
			Action = onClick
		});
	}

	public void SetRect(Rect rect)
	{
		_rect = GUITools.ToGlobal(rect);
	}

	public void Draw()
	{
		bool isDown = _isDown;
		_isDown = GUI.Toggle(_rect, _isDown, Caption, BlueStonez.buttondark_medium);
		if (isDown != _isDown)
		{
			MouseOrbit.Disable = false;
		}
		if (!_isDown)
		{
			return;
		}
		MouseOrbit.Disable = true;
		Rect position = ShowRightAligned ? new Rect(_rect.xMax - ButtonWidth, _rect.yMax, ButtonWidth, ButtonHeight * (float)_data.Count) : new Rect(_rect.x, _rect.yMax, ButtonWidth, ButtonHeight * (float)_data.Count);
		if (!position.Contains(Event.current.mousePosition) && !_rect.Contains(Event.current.mousePosition) && (Event.current.type == EventType.MouseUp || Event.current.type == EventType.Used))
		{
			_isDown = false;
			MouseOrbit.Disable = false;
		}
		GUI.BeginGroup(position);
		for (int i = 0; i < _data.Count; i++)
		{
			if (_data[i].IsEnabled())
			{
				GUIStyle style = BlueStonez.dropdown;
				if (ApplicationDataManager.IsMobile)
				{
					style = BlueStonez.dropdown_large;
				}
				if (GUI.Button(new Rect(0f, (float)i * ButtonHeight, ButtonWidth, ButtonHeight), _data[i].Content, style))
				{
					_isDown = false;
					MouseOrbit.Disable = false;
					_data[i].Action();
				}
			}
		}
		GUI.EndGroup();
	}
}
