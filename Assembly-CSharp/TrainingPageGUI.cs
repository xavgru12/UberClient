using System;
using System.Text.RegularExpressions;
using UberStrike.Core.Models;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class TrainingPageGUI : MonoBehaviour
{
	private const int PageWidth = 700;

	private const int PageHeight = 480;

	private const int MapsPerRow = 4;

	private Vector2 _mapScroll;

	private static GameFlags _gameFlags = new GameFlags();

	public static bool isPopup = false;

	private static UberstrikeMap _mapSelected;

	private static bool isBlueBoxMap
	{
		get
		{
			if (CreateGamePanelGUI.BlueBoxSupport(_mapSelected.Id) || _mapSelected.Id == 1)
			{
				return true;
			}
			return false;
		}
	}

	private void OnGUI()
	{
		isPopup = false;
		GUI.depth = 11;
		GUI.skin = BlueStonez.Skin;
		GUI.BeginGroup(new Rect((float)(Screen.width - 700) * 0.5f, (float)(Screen.height - GlobalUIRibbon.Instance.Height() - 480) * 0.5f, 700f, 480f), string.Empty, BlueStonez.window);
		GUI.Label(new Rect(10f, 20f, 670f, 48f), LocalizedStrings.ExploreMaps, BlueStonez.label_interparkbold_48pt);
		GUI.Label(new Rect(30f, 50f, 640f, 120f), LocalizedStrings.TrainingModeDesc, BlueStonez.label_interparkbold_13pt);
		GUI.Box(new Rect(12f, 160f, 670f, 20f), string.Empty, BlueStonez.box_grey50);
		GUI.Label(new Rect(16f, 160f, 120f, 20f), LocalizedStrings.ChooseAMap, BlueStonez.label_interparkbold_18pt_left);
		int num = 280;
		GUI.Box(new Rect(12f, 179f, 670f, num), string.Empty, BlueStonez.window);
		int num2 = 0;
		if (Singleton<MapManager>.Instance.Count > 0)
		{
			num2 = (Singleton<MapManager>.Instance.Count - 1) / 4 + 1;
		}
		_mapScroll = GUITools.BeginScrollView(new Rect(0f, 179f, 682f, num), _mapScroll, new Rect(0f, 0f, 655f, 10 + 80 * num2));
		Vector2 v = new Vector2(163f, 80f);
		int num3 = 0;
		foreach (UberstrikeMap allMap in Singleton<MapManager>.Instance.AllMaps)
		{
			if (allMap.IsVisible)
			{
				Color white = Color.white;
				int num4 = num3 / 4;
				int num5 = num3 % 4;
				Rect rect = new Rect(13f + (float)num5 * v.Width(), (float)num4 * v.y + 4f, v.x, v.y);
				if (GUI.Button(rect, string.Empty, BlueStonez.gray_background) && !GUITools.IsScrolling && !Singleton<SceneLoader>.Instance.IsLoading && allMap != null && !isPopup)
				{
					if (isPopup)
					{
						return;
					}
					isPopup = true;
					_mapSelected = allMap;
					if (isBlueBoxMap)
					{
						PopupSystem.ShowMessage("Texture Settings", "Please select the map's desired texture!", PopupSystem.AlertType.OKCancel, delegate
						{
							GameState.Current.RoomData.BoxType = GameBoxType.BLUE;
							allMap.View.BoxType = GameBoxType.BLUE;
							LoadMap(allMap);
						}, "Bluebox", delegate
						{
							GameState.Current.RoomData.BoxType = GameBoxType.NORMAL;
							allMap.View.BoxType = GameBoxType.NORMAL;
							LoadMap(allMap);
						}, "Normal");
					}
					else
					{
						GameState.Current.RoomData.BoxType = GameBoxType.NORMAL;
						allMap.View.BoxType = GameBoxType.NORMAL;
						LoadMap(allMap);
					}
				}
				GUI.BeginGroup(rect);
				allMap.Icon.Draw(rect.CenterHorizontally(2f, 100f, 64f));
				Vector2 vector = BlueStonez.label_interparkbold_11pt.CalcSize(new GUIContent(allMap.Name));
				GUI.contentColor = white;
				GUI.Label(rect.CenterHorizontally(rect.height - vector.y, vector.x, vector.y), allMap.Name, BlueStonez.label_interparkbold_11pt);
				GUI.contentColor = Color.white;
				GUI.EndGroup();
				num3++;
			}
		}
		GUITools.EndScrollView();
		GUI.enabled = true;
		Rect rect2 = new Rect(num, 0f, 360f, num2);
		int num6 = 1;
		Array values = Enum.GetValues(typeof(GameFlags.GAME_FLAGS));
		for (int num7 = 1; num7 < (int)values.GetValue(values.Length - 1) + 1; num7 *= 2)
		{
			GameFlags.GAME_FLAGS gAME_FLAGS = (GameFlags.GAME_FLAGS)num7;
			string text = Regex.Replace(gAME_FLAGS.ToString(), "((?<=\\p{Ll})\\p{Lu})|((?!\\A)\\p{Lu}(?>\\p{Ll}))", " $0");
			double num8 = 100.0 + 20.0 * Math.Ceiling((float)num6 / 2f);
			int num9;
			for (num9 = num6; num9 > 2; num9 -= 2)
			{
			}
			float left = 8f + (float)(num9 - 1) * 165f;
			switch (gAME_FLAGS)
			{
			case GameFlags.GAME_FLAGS.QuickSwitch:
			{
				bool flag2 = GUI.Toggle(new Rect(left, (float)num8, 160f, 16f), _gameFlags.QuickSwitch, text, BlueStonez.toggle);
				if (_gameFlags.QuickSwitch != flag2)
				{
					_gameFlags.QuickSwitch = !_gameFlags.QuickSwitch;
				}
				break;
			}
			case GameFlags.GAME_FLAGS.LowGravity:
			{
				bool flag = GUI.Toggle(new Rect(left, (float)num8, 160f, 16f), UberKill.IsLowGravity, text, BlueStonez.toggle);
				if (UberKill.IsLowGravity != flag)
				{
					UberKill.IsLowGravity = !UberKill.IsLowGravity;
				}
				break;
			}
			}
			num6++;
		}
		GUI.EndGroup();
	}

	private static void LoadMap(UberstrikeMap allMap)
	{
		if(allMap.View.MapId == 1) UberKill.BoxLoader(allMap.View.BoxType);
		Singleton<MapManager>.Instance.LoadMap(allMap, delegate
		{
			Singleton<GameStateController>.Instance.SetGameMode(new TrainingRoom((GameFlags.GAME_FLAGS)_gameFlags.ToInt()));
			GameState.Current.Actions.JoinTeam(TeamID.NONE);
		});
	}
}
