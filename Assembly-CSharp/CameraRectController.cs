using UnityEngine;

public class CameraRectController : AutoMonoBehaviour<CameraRectController>
{
	private float lastWidth = 1f;

	private Vector2 screenSize;

	public float PixelWidth
	{
		get
		{
			if (GameState.Current.Map != null && GameState.Current.Map.Camera != null)
			{
				return GameState.Current.Map.Camera.pixelWidth;
			}
			return Screen.width;
		}
	}

	public float NormalizedWidth => PixelWidth / (float)Screen.width;

	private void LateUpdate()
	{
		if ((float)Screen.width != screenSize.x || (float)Screen.height != screenSize.y)
		{
			screenSize.x = Screen.width;
			screenSize.y = Screen.height;
			EventHandler.Global.Fire(new GlobalEvents.ScreenResolutionChanged());
		}
		if (GameState.Current.Map != null && GameState.Current.Map.Camera != null && GameState.Current.Map.Camera.pixelWidth != lastWidth)
		{
			lastWidth = GameState.Current.Map.Camera.pixelWidth;
			EventHandler.Global.Fire(new GlobalEvents.CameraWidthChanged());
		}
	}

	public void SetAbsoluteWidth(float width)
	{
		SetNormalizedWidth(width / (float)Screen.width);
	}

	public void SetNormalizedWidth(float width)
	{
		width = Mathf.Clamp(width, 0f, 1f);
		if (GameState.Current.Map != null && GameState.Current.Map.Camera != null && lastWidth != width)
		{
			GameState.Current.Map.Camera.rect = new Rect(0f, 0f, width, 1f);
		}
	}
}
