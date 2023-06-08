using UnityEngine;

public class TouchInput : AutoMonoBehaviour<TouchInput>
{
	public static Property<bool> ShowTouchControls = new Property<bool>(defaultValue: false);

	public static Property<bool> OnSecondaryFire = new Property<bool>(defaultValue: false);

	public static Property<bool> UseMultiTouch = new Property<bool>(defaultValue: true);

	public static Vector2 WishLook;

	public static Vector2 WishDirection;

	public static bool WishJump;

	public static bool WishCrouch;

	public static bool IsFiring;

	public static bool DisableIdleTimer = false;

	public TouchShooter Shooter;

	public void EnablePerformanceChecker()
	{
	}

	public void DisablePerformanceChecker()
	{
	}
}
