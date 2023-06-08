using UnityEngine;

public static class StormFront
{
	public static GUIStyle box = GUIStyle.none;

	public static GUIStyle label = GUIStyle.none;

	public static GUIStyle textField = GUIStyle.none;

	public static GUIStyle textArea = GUIStyle.none;

	public static GUIStyle button = GUIStyle.none;

	public static GUIStyle toggle = GUIStyle.none;

	public static GUIStyle window = GUIStyle.none;

	public static GUIStyle horizontalSlider = GUIStyle.none;

	public static GUIStyle horizontalSliderThumb = GUIStyle.none;

	public static GUIStyle verticalSlider = GUIStyle.none;

	public static GUIStyle verticalSliderThumb = GUIStyle.none;

	public static GUIStyle horizontalScrollbar = GUIStyle.none;

	public static GUIStyle horizontalScrollbarThumb = GUIStyle.none;

	public static GUIStyle horizontalScrollbarLeftButton = GUIStyle.none;

	public static GUIStyle horizontalScrollbarRightButton = GUIStyle.none;

	public static GUIStyle verticalScrollbar = GUIStyle.none;

	public static GUIStyle verticalScrollbarThumb = GUIStyle.none;

	public static GUIStyle verticalScrollbarUpButton = GUIStyle.none;

	public static GUIStyle verticalScrollbarDownButton = GUIStyle.none;

	public static GUIStyle scrollView = GUIStyle.none;

	public static GUIStyle BlueBox = GUIStyle.none;

	public static GUIStyle RedBox = GUIStyle.none;

	public static GUIStyle BluePanelBox = GUIStyle.none;

	public static GUIStyle GrayPanelBox = GUIStyle.none;

	public static GUIStyle GrayPanelBlankBox = GUIStyle.none;

	public static GUIStyle RedPanelBox = GUIStyle.none;

	public static GUIStyle ButtonRed = GUIStyle.none;

	public static GUIStyle ButtonBlue = GUIStyle.none;

	public static GUIStyle ButtonGray = GUIStyle.none;

	public static GUIStyle ButtonJoinBlue = GUIStyle.none;

	public static GUIStyle ButtonJoinRed = GUIStyle.none;

	public static GUIStyle ButtonJoinGray = GUIStyle.none;

	public static GUIStyle ButtonSpectator = GUIStyle.none;

	public static GUIStyle ButtonLoadout = GUIStyle.none;

	public static GUIStyle ButtonLoadoutRed = GUIStyle.none;

	public static GUIStyle InGameChatBlue = GUIStyle.none;

	public static GUIStyle InGameChatRed = GUIStyle.none;

	public static GUIStyle DotBlue = GUIStyle.none;

	public static GUIStyle DotRed = GUIStyle.none;

	public static GUIStyle DotGray = GUIStyle.none;

	public static GUIStyle ButtonCam = GUIStyle.none;

	public static GUIStyle Interpark32Center = GUIStyle.none;

	public static GUIStyle Interpark16Left = GUIStyle.none;

	public static GUIStyle ProgressBackground = GUIStyle.none;

	public static GUIStyle ProgressForeground = GUIStyle.none;

	public static GUIStyle ProgressThumb = GUIStyle.none;

	public static GUIStyle MenuTile = GUIStyle.none;

	public static GUISkin Skin
	{
		get;
		private set;
	}

	public static void Initialize(GUISkin skin)
	{
		Skin = skin;
		box = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("box"));
		label = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("label"));
		textField = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("textField"));
		textArea = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("textArea"));
		button = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("button"));
		toggle = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("toggle"));
		window = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("window"));
		horizontalSlider = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("horizontalSlider"));
		horizontalSliderThumb = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("horizontalSliderThumb"));
		verticalSlider = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("verticalSlider"));
		verticalSliderThumb = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("verticalSliderThumb"));
		horizontalScrollbar = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("horizontalScrollbar"));
		horizontalScrollbarThumb = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("horizontalScrollbarThumb"));
		horizontalScrollbarLeftButton = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("horizontalScrollbarLeftButton"));
		horizontalScrollbarRightButton = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("horizontalScrollbarRightButton"));
		verticalScrollbar = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("verticalScrollbar"));
		verticalScrollbarThumb = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("verticalScrollbarThumb"));
		verticalScrollbarUpButton = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("verticalScrollbarUpButton"));
		verticalScrollbarDownButton = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("verticalScrollbarDownButton"));
		scrollView = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("scrollView"));
		BlueBox = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("BlueBox"));
		RedBox = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("RedBox"));
		BluePanelBox = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("BluePanelBox"));
		GrayPanelBox = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("GrayPanelBox"));
		GrayPanelBlankBox = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("GrayPanelBlankBox"));
		RedPanelBox = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("RedPanelBox"));
		ButtonRed = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ButtonRed"));
		ButtonBlue = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ButtonBlue"));
		ButtonGray = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ButtonGray"));
		ButtonJoinBlue = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ButtonJoinBlue"));
		ButtonJoinRed = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ButtonJoinRed"));
		ButtonJoinGray = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ButtonJoinGray"));
		ButtonSpectator = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ButtonSpectator"));
		ButtonLoadout = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ButtonLoadout"));
		ButtonLoadoutRed = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ButtonLoadoutRed"));
		InGameChatBlue = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("InGameChatBlue"));
		InGameChatRed = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("InGameChatRed"));
		DotBlue = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("DotBlue"));
		DotRed = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("DotRed"));
		DotGray = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("DotGray"));
		ButtonCam = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ButtonCam"));
		Interpark32Center = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("Interpark32Center"));
		Interpark16Left = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("Interpark16Left"));
		ProgressBackground = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ProgressBackground"));
		ProgressForeground = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ProgressForeground"));
		ProgressThumb = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("ProgressThumb"));
		MenuTile = LocalizationHelper.GetLocalizedStyle(Skin.GetStyle("MenuTile"));
	}
}
