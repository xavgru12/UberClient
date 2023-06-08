using System;
using System.Collections.Generic;
using UberStrike.Core.Models;
using UnityEngine;

internal class ValuablePlayerListGUI
{
	private const float HEADER_HEIGHT = 20f;

	private readonly float[] _columnWidthPercent = new float[4]
	{
		0.7f,
		0.1f,
		0.1f,
		0.1f
	};

	private readonly string[] _headingArray = new string[4]
	{
		"NAME",
		"KILL",
		"DEATHS",
		"LEVEL"
	};

	private Vector2 _scroll;

	private int _curSelectedPlayerIndex = -1;

	private float _playerListViewHeight;

	public bool Enabled
	{
		get;
		set;
	}

	public Action<StatsSummary> OnSelectionChange
	{
		get;
		set;
	}

	public float Height => 20f + _playerListViewHeight + 2f;

	public void ClearSelection()
	{
		_curSelectedPlayerIndex = -1;
	}

	public void Draw(Rect rect)
	{
		GUI.BeginGroup(rect, GUIContent.none, BlueStonez.window);
		Rect rect2 = new Rect(0f, 0f, rect.width, 20f);
		Rect rect3 = new Rect(0f, 20f, rect.width, rect.height - 20f - 1f);
		DrawRankingListHeader(rect2, _columnWidthPercent);
		DrawRankingListContent(rect3, _columnWidthPercent);
		GUI.EndGroup();
	}

	public void SetSelection(int index)
	{
		_curSelectedPlayerIndex = index;
		if (OnSelectionChange != null)
		{
			OnSelectionChange(GameState.Current.Statistics.Data.MostValuablePlayers[_curSelectedPlayerIndex]);
		}
	}

	private void DrawRankingListHeader(Rect rect, float[] columnWidthPercent)
	{
		GUI.BeginGroup(rect);
		float num = 0f;
		for (int i = 0; i < _headingArray.Length; i++)
		{
			Rect position = new Rect(num, 0f, rect.width * columnWidthPercent[i], rect.height);
			GUI.Button(position, string.Empty, BlueStonez.box_grey50);
			GUI.Label(position, new GUIContent(_headingArray[i]), BlueStonez.label_interparkmed_11pt);
			num += rect.width * columnWidthPercent[i];
		}
		GUI.EndGroup();
	}

	private void DrawRankingListContent(Rect rect, float[] columnWidthPercent)
	{
		float num = rect.width;
		if (GameState.Current.Statistics.Data.MostValuablePlayers != null)
		{
			_playerListViewHeight = GameState.Current.Statistics.Data.MostValuablePlayers.Count * 32;
			if (_playerListViewHeight > rect.height)
			{
				num -= 20f;
			}
		}
		_scroll = GUITools.BeginScrollView(rect, _scroll, new Rect(0f, 0f, num, _playerListViewHeight));
		float num2 = 0f;
		int num3 = 0;
		while (GameState.Current.Statistics.Data != null && num3 < GameState.Current.Statistics.Data.MostValuablePlayers.Count)
		{
			DrawStatsSummary(new Rect(0f, num2, num, 32f), num3, columnWidthPercent);
			num2 += 32f;
			num3++;
		}
		GUITools.EndScrollView();
	}

	private void DrawStatsSummary(Rect rect, int rank, float[] columnWidthPercent)
	{
		StatsSummary statsSummary = GameState.Current.Statistics.Data.MostValuablePlayers[rank];
		Color contentColor = Color.white;
		if (statsSummary.Cmid != PlayerDataManager.Cmid)
		{
			if (statsSummary.Team == TeamID.BLUE)
			{
				contentColor = ColorScheme.UberStrikeBlue;
			}
			else if (statsSummary.Team == TeamID.RED)
			{
				contentColor = ColorScheme.UberStrikeRed;
			}
		}
		if (_curSelectedPlayerIndex == rank)
		{
			GUI.Label(rect, GUIContent.none, StormFront.GrayPanelBox);
		}
		else
		{
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
		}
		GUI.BeginGroup(rect);
		float num = 0f;
		Vector2 vector = BlueStonez.label_interparkbold_18pt_left.CalcSize(new GUIContent(statsSummary.Name));
		GUI.contentColor = contentColor;
		DrawAchivements(new Rect(num + 16f + vector.x, 0f, rect.width * columnWidthPercent[0] - num - 16f - vector.x, 32f), statsSummary.Achievements);
		GUI.Label(new Rect(num + 10f, 0f, rect.width * columnWidthPercent[0], 32f), statsSummary.Name, BlueStonez.label_interparkbold_18pt_left);
		num += rect.width * columnWidthPercent[0];
		GUI.Label(new Rect(num, 0f, rect.width * columnWidthPercent[1], 32f), statsSummary.Kills.ToString(), BlueStonez.label_interparkbold_18pt);
		num += rect.width * columnWidthPercent[1];
		GUI.Label(new Rect(num, 0f, rect.width * columnWidthPercent[2], 32f), statsSummary.Deaths.ToString(), BlueStonez.label_interparkbold_18pt);
		num += rect.width * columnWidthPercent[2];
		GUI.Label(new Rect(num, 0f, rect.width * columnWidthPercent[3], 32f), statsSummary.Level.ToString(), BlueStonez.label_interparkbold_18pt);
		GUI.color = Color.white;
		GUI.contentColor = Color.white;
		GUI.EndGroup();
		if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition) && _curSelectedPlayerIndex != rank)
		{
			SetSelection(rank);
		}
	}

	private void DrawAchivements(Rect rect, Dictionary<byte, ushort> achievements)
	{
		GUI.BeginGroup(rect);
		float num = 0f;
		foreach (KeyValuePair<byte, ushort> achievement in achievements)
		{
			AchievementType key = (AchievementType)achievement.Key;
			Texture2D achievementBadgeTexture = UberstrikeIconsHelper.GetAchievementBadgeTexture(key);
			float num2 = (rect.height - 2f) / (float)achievementBadgeTexture.height;
			GUI.DrawTexture(new Rect(num, 1f, (float)achievementBadgeTexture.width * num2, rect.height - 2f), achievementBadgeTexture);
			num += (float)achievementBadgeTexture.width * num2;
		}
		GUI.EndGroup();
	}
}
