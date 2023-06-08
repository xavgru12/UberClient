using UnityEngine;

public static class GuiManager
{
	public static void DrawTooltip()
	{
		if (!string.IsNullOrEmpty(GUI.tooltip))
		{
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			Vector2 vector = BlueStonez.tooltip.CalcSize(new GUIContent(GUI.tooltip));
			Vector2 mousePosition = Event.current.mousePosition;
			float left = Mathf.Clamp(mousePosition.x, 14f, (float)Screen.width - (vector.x + 14f));
			Vector2 mousePosition2 = Event.current.mousePosition;
			Rect position = new Rect(left, mousePosition2.y + 24f, vector.x, vector.y + 16f);
			if (position.yMax > (float)Screen.height)
			{
				position.x += 30f;
				position.y += (float)Screen.height - position.yMax;
			}
			if (position.xMax > (float)Screen.width)
			{
				position.x += (float)Screen.width - position.xMax;
			}
			GUI.Label(position, GUI.tooltip, BlueStonez.tooltip);
			GUI.matrix = matrix;
		}
	}
}
