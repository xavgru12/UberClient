// Decompiled with JetBrains decompiler
// Type: ValuablePlayerListGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
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

  public bool Enabled { get; set; }

  public Action<StatsSummary> OnSelectionChange { get; set; }

  public float Height => (float) (20.0 + (double) this._playerListViewHeight + 2.0);

  public void ClearSelection() => this._curSelectedPlayerIndex = -1;

  public void Draw(Rect rect)
  {
    GUI.BeginGroup(rect, GUIContent.none, BlueStonez.window);
    Rect rect1 = new Rect(0.0f, 0.0f, rect.width, 20f);
    Rect rect2 = new Rect(0.0f, 20f, rect.width, (float) ((double) rect.height - 20.0 - 1.0));
    this.DrawRankingListHeader(rect1, this._columnWidthPercent);
    this.DrawRankingListContent(rect2, this._columnWidthPercent);
    GUI.EndGroup();
  }

  public void SetSelection(int index)
  {
    this._curSelectedPlayerIndex = index;
    this.OnSelectionChange(Singleton<EndOfMatchStats>.Instance.Data.MostValuablePlayers[this._curSelectedPlayerIndex]);
  }

  private void DrawRankingListHeader(Rect rect, float[] columnWidthPercent)
  {
    GUI.BeginGroup(rect);
    float left = 0.0f;
    for (int index = 0; index < this._headingArray.Length; ++index)
    {
      Rect position = new Rect(left, 0.0f, rect.width * columnWidthPercent[index], rect.height);
      GUI.Button(position, string.Empty, BlueStonez.box_grey50);
      GUI.Label(position, new GUIContent(this._headingArray[index]), BlueStonez.label_interparkmed_11pt);
      left += rect.width * columnWidthPercent[index];
    }
    GUI.EndGroup();
  }

  private void DrawRankingListContent(Rect rect, float[] columnWidthPercent)
  {
    float width = rect.width;
    this._playerListViewHeight = (float) (Singleton<EndOfMatchStats>.Instance.Data.MostValuablePlayers.Count * 32);
    if ((double) this._playerListViewHeight > (double) rect.height)
      width -= 20f;
    this._scroll = GUITools.BeginScrollView(rect, this._scroll, new Rect(0.0f, 0.0f, width, this._playerListViewHeight));
    float top = 0.0f;
    for (int rank = 0; Singleton<EndOfMatchStats>.Instance.Data != null && rank < Singleton<EndOfMatchStats>.Instance.Data.MostValuablePlayers.Count; ++rank)
    {
      this.DrawStatsSummary(new Rect(0.0f, top, width, 32f), rank, columnWidthPercent);
      top += 32f;
    }
    GUITools.EndScrollView();
  }

  private void DrawStatsSummary(Rect rect, int rank, float[] columnWidthPercent)
  {
    StatsSummary mostValuablePlayer = Singleton<EndOfMatchStats>.Instance.Data.MostValuablePlayers[rank];
    Color color = Color.white;
    if (mostValuablePlayer.Cmid != PlayerDataManager.Cmid)
    {
      if (mostValuablePlayer.Team == TeamID.BLUE)
        color = ColorScheme.UberStrikeBlue;
      else if (mostValuablePlayer.Team == TeamID.RED)
        color = ColorScheme.UberStrikeRed;
    }
    if (this._curSelectedPlayerIndex == rank)
      GUI.Label(rect, GUIContent.none, StormFront.GrayPanelBox);
    else
      GUI.color = new Color(1f, 1f, 1f, 0.5f);
    GUI.BeginGroup(rect);
    float num = 0.0f;
    Vector2 vector2 = BlueStonez.label_interparkbold_18pt_left.CalcSize(new GUIContent(mostValuablePlayer.Name));
    GUI.contentColor = color;
    this.DrawAchivements(new Rect(num + 16f + vector2.x, 0.0f, (float) ((double) rect.width * (double) columnWidthPercent[0] - (double) num - 16.0) - vector2.x, 32f), mostValuablePlayer.Achievements);
    GUI.Label(new Rect(num + 10f, 0.0f, rect.width * columnWidthPercent[0], 32f), mostValuablePlayer.Name, BlueStonez.label_interparkbold_18pt_left);
    float left1 = num + rect.width * columnWidthPercent[0];
    GUI.Label(new Rect(left1, 0.0f, rect.width * columnWidthPercent[1], 32f), mostValuablePlayer.Kills.ToString(), BlueStonez.label_interparkbold_18pt);
    float left2 = left1 + rect.width * columnWidthPercent[1];
    GUI.Label(new Rect(left2, 0.0f, rect.width * columnWidthPercent[2], 32f), mostValuablePlayer.Deaths.ToString(), BlueStonez.label_interparkbold_18pt);
    GUI.Label(new Rect(left2 + rect.width * columnWidthPercent[2], 0.0f, rect.width * columnWidthPercent[3], 32f), mostValuablePlayer.Level.ToString(), BlueStonez.label_interparkbold_18pt);
    GUI.color = Color.white;
    GUI.contentColor = Color.white;
    GUI.EndGroup();
    if (Event.current.type != UnityEngine.EventType.MouseDown || !rect.Contains(Event.current.mousePosition) || this._curSelectedPlayerIndex == rank)
      return;
    this.SetSelection(rank);
  }

  private void DrawAchivements(Rect rect, Dictionary<byte, ushort> achievements)
  {
    GUI.BeginGroup(rect);
    float left = 0.0f;
    foreach (KeyValuePair<byte, ushort> achievement in achievements)
    {
      Texture2D achievementBadgeTexture = UberstrikeIconsHelper.GetAchievementBadgeTexture((AchievementType) achievement.Key);
      float num = (rect.height - 2f) / (float) achievementBadgeTexture.height;
      GUI.DrawTexture(new Rect(left, 1f, (float) achievementBadgeTexture.width * num, rect.height - 2f), (Texture) achievementBadgeTexture);
      left += (float) achievementBadgeTexture.width * num;
    }
    GUI.EndGroup();
  }
}
