using System;
using UnityEngine;

public class NGUITouchJoystick : MonoBehaviour
{
	[SerializeField]
	private GameObject backgroundContainer;

	[SerializeField]
	private UISprite movingStick;

	[SerializeField]
	private Vector2 joystickLimits = new Vector2(128f, 128f);

	[SerializeField]
	private Rect touchBoundary = new Rect(0f, 0f, Screen.width, Screen.height);

	public Action<Vector2> OnJoystickMoved;

	public Action OnJoystickStopped;

	private Rect boundary;

	private Rect joystickBoundary;

	private TouchFinger finger = new TouchFinger();

	private Vector2 joystickPosition = Vector2.zero;

	public Rect TouchBoundary
	{
		set
		{
			touchBoundary = value;
			boundary = touchBoundary;
		}
	}

	private void Awake()
	{
		boundary = touchBoundary;
	}

	private void Update()
	{
		Touch[] touches = Input.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			Touch touch = touches[i];
			if (touch.phase == TouchPhase.Began && boundary.ContainsTouch(touch.position) && finger.FingerId == -1)
			{
				TouchFinger touchFinger = new TouchFinger();
				Vector2 position = touch.position;
				float x = position.x;
				Vector2 position2 = touch.position;
				touchFinger.StartPos = new Vector2(x, position2.y);
				touchFinger.StartTouchTime = Time.time;
				touchFinger.FingerId = touch.fingerId;
				finger = touchFinger;
				Vector2 position3 = touch.position;
				float left = position3.x - joystickLimits.x / 2f;
				Vector2 position4 = touch.position;
				joystickBoundary = new Rect(left, position4.y - joystickLimits.y / 2f, joystickLimits.x, joystickLimits.y);
				Camera currentCamera = UICamera.currentCamera;
				Vector2 position5 = touch.position;
				float x2 = position5.x;
				Vector2 position6 = touch.position;
				Vector3 vector = currentCamera.ScreenToWorldPoint(new Vector3(x2, position6.y, UICamera.currentCamera.nearClipPlane));
				vector = backgroundContainer.transform.parent.InverseTransformPoint(new Vector3(vector.x, vector.y, 0f));
				backgroundContainer.transform.localPosition = vector;
				movingStick.transform.localPosition = vector;
				ShowJoystick(show: true);
			}
			else
			{
				if (finger.FingerId != touch.fingerId)
				{
					continue;
				}
				if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
				{
					ref Vector2 reference = ref joystickPosition;
					Vector2 position7 = touch.position;
					reference.x = Mathf.Clamp(position7.x, joystickBoundary.x, joystickBoundary.x + joystickBoundary.width);
					ref Vector2 reference2 = ref joystickPosition;
					Vector2 position8 = touch.position;
					reference2.y = Mathf.Clamp(position8.y, joystickBoundary.y, joystickBoundary.y + joystickBoundary.height);
					Vector3 position9 = new Vector3(joystickPosition.x, joystickPosition.y, 0f);
					position9 = UICamera.currentCamera.ScreenToWorldPoint(position9);
					movingStick.transform.localPosition = backgroundContainer.transform.parent.InverseTransformPoint(position9);
					Vector2 zero = Vector2.zero;
					zero.x = (joystickPosition.x - finger.StartPos.x) * 2f / joystickBoundary.width;
					zero.y = (joystickPosition.y - finger.StartPos.y) * 2f / joystickBoundary.height;
					zero *= ApplicationDataManager.ApplicationOptions.TouchMoveSensitivity;
					if (touch.phase == TouchPhase.Moved && OnJoystickMoved != null)
					{
						OnJoystickMoved(zero);
					}
				}
				else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					ShowJoystick(show: false);
					boundary = touchBoundary;
					if (OnJoystickStopped != null)
					{
						OnJoystickStopped();
					}
					finger.Reset();
				}
			}
		}
	}

	private void ShowJoystick(bool show)
	{
		movingStick.enabled = show;
		NGUITools.SetActiveChildren(backgroundContainer, show);
		NGUITools.SetActiveChildren(movingStick.gameObject, show);
	}
}
