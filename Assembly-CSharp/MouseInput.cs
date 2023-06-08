using UnityEngine;

public static class MouseInput
{
	private struct Click
	{
		public float Time;

		public Vector2 Point;

		public int Button;
	}

	public const float DoubleClickInterval = 0.3f;

	private static Click Current;

	private static Click Previous;

	static MouseInput()
	{
		AutoMonoBehaviour<UnityRuntime>.Instance.OnGui += OnGUI;
	}

	public static bool IsDoubleClick()
	{
		if (Time.time - Previous.Time < 0.3f)
		{
			return Current.Point == Previous.Point;
		}
		return false;
	}

	public static bool IsMouseClickIn(Rect rect, int mouse = 0)
	{
		if (Event.current.type == EventType.MouseDown && Event.current.button == mouse)
		{
			return rect.Contains(Event.current.mousePosition);
		}
		return false;
	}

	private static void OnGUI()
	{
		if (Event.current.type == EventType.MouseDown)
		{
			Previous = Current;
			Current.Time = Time.time;
			Current.Point = Event.current.mousePosition;
			Current.Button = Event.current.button;
		}
	}
}
