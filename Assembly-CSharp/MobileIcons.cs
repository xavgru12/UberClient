using UnityEngine;

public static class MobileIcons
{
	public static Material TextureAtlas
	{
		get;
		private set;
	}

	public static Rect TouchArrowLeft
	{
		get;
		private set;
	}

	public static Rect TouchArrowRight
	{
		get;
		private set;
	}

	public static Rect TouchChatButton
	{
		get;
		private set;
	}

	public static Rect TouchCrouchButton
	{
		get;
		private set;
	}

	public static Rect TouchCrouchButtonActive
	{
		get;
		private set;
	}

	public static Rect TouchFireButton
	{
		get;
		private set;
	}

	public static Rect TouchJumpButton
	{
		get;
		private set;
	}

	public static Rect TouchKeyboardDpad
	{
		get;
		private set;
	}

	public static Rect TouchMenuButton
	{
		get;
		private set;
	}

	public static Rect TouchMoveInner
	{
		get;
		private set;
	}

	public static Rect TouchMoveOuter
	{
		get;
		private set;
	}

	public static Rect TouchScoreboardButton
	{
		get;
		private set;
	}

	public static Rect TouchSecondFireButton
	{
		get;
		private set;
	}

	public static Rect TouchWeaponCannon
	{
		get;
		private set;
	}

	public static Rect TouchWeaponHandgun
	{
		get;
		private set;
	}

	public static Rect TouchWeaponLauncher
	{
		get;
		private set;
	}

	public static Rect TouchWeaponMachinegun
	{
		get;
		private set;
	}

	public static Rect TouchWeaponMelee
	{
		get;
		private set;
	}

	public static Rect TouchWeaponShotgun
	{
		get;
		private set;
	}

	public static Rect TouchWeaponSniperrifle
	{
		get;
		private set;
	}

	public static Rect TouchWeaponSplattergun
	{
		get;
		private set;
	}

	public static Rect TouchZoomScrollbar
	{
		get;
		private set;
	}

	static MobileIcons()
	{
		Texture2DAtlasHolder component;
		try
		{
			component = GameObject.Find("MobileIcons").GetComponent<Texture2DAtlasHolder>();
		}
		catch
		{
			Debug.LogError("Missing instance of the prefab with name: MobileIcons!");
			return;
		}
		TextureAtlas = component.Atlas;
		TouchArrowLeft = new Rect(0.1582031f, 113f / 128f, 0.01074219f, 0.015625f);
		TouchArrowRight = new Rect(0.1582031f, 0.899414063f, 0.01074219f, 0.015625f);
		TouchChatButton = new Rect(0.1132813f, 113f / 128f, 0.04394531f, 0.0439453162f);
		TouchCrouchButton = new Rect(0.6962891f, 0.5f, 19f / 256f, 0.0732422f);
		TouchCrouchButtonActive = new Rect(395f / 512f, 0.5f, 19f / 256f, 0.0732422f);
		TouchFireButton = new Rect(0f, 113f / 128f, 0.1123047f, 0.111328147f);
		TouchJumpButton = new Rect(0.5439453f, 0.5f, 0.07617188f, 19f / 256f);
		TouchKeyboardDpad = new Rect(0f, 0.5f, 25f / 64f, 0.204101548f);
		TouchMenuButton = new Rect(0.1132813f, 475f / 512f, 0.04394531f, 0.0439453162f);
		TouchMoveInner = new Rect(0.3916016f, 451f / 512f, 0.09082031f, 0.0908203f);
		TouchMoveOuter = new Rect(0f, 361f / 512f, 0.1767578f, 0.1767578f);
		TouchScoreboardButton = new Rect(0.3300781f, 361f / 512f, 0.04394531f, 0.0439453162f);
		TouchSecondFireButton = new Rect(0.6210938f, 0.5f, 19f / 256f, 19f / 256f);
		TouchWeaponCannon = new Rect(0.1777344f, 361f / 512f, 0.1513672f, 0.0751953f);
		TouchWeaponHandgun = new Rect(0.1777344f, 25f / 32f, 0.1513672f, 0.0751953f);
		TouchWeaponLauncher = new Rect(0.1777344f, 439f / 512f, 0.1513672f, 0.0751953f);
		TouchWeaponMachinegun = new Rect(0.3916016f, 0.5f, 0.1513672f, 0.0751953f);
		TouchWeaponMelee = new Rect(0.3916016f, 295f / 512f, 0.1513672f, 0.0751953f);
		TouchWeaponShotgun = new Rect(0.3916016f, 167f / 256f, 0.1513672f, 0.0751953f);
		TouchWeaponSniperrifle = new Rect(0.3916016f, 373f / 512f, 0.1513672f, 0.0751953f);
		TouchWeaponSplattergun = new Rect(0.3916016f, 103f / 128f, 0.1513672f, 0.0751953f);
		TouchZoomScrollbar = new Rect(0.5439453f, 0.5751953f, 0.02636719f, 0.186523452f);
	}
}
