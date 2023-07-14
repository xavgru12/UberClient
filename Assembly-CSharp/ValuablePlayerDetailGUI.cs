// Decompiled with JetBrains decompiler
// Type: ValuablePlayerDetailGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class ValuablePlayerDetailGUI
{
  private StatsSummary _curPlayerStats;
  private List<AchievementType> _achievementList;
  private Texture2D _curBadge;
  private string _curBadgeTitle;
  private string _curBadgeText;
  private int _curAchievementIndex = -1;

  public ValuablePlayerDetailGUI() => this._achievementList = new List<AchievementType>();

  public void SetValuablePlayer(StatsSummary playerStats)
  {
    this._curPlayerStats = playerStats;
    this._curBadgeTitle = string.Empty;
    this._curBadgeText = string.Empty;
    this._achievementList.Clear();
    if (playerStats != null)
    {
      foreach (KeyValuePair<byte, ushort> achievement in this._curPlayerStats.Achievements)
        this._achievementList.Add((AchievementType) achievement.Key);
    }
    MonoRoutine.Start(this.StartBadgeShow());
  }

  public void StopBadgeShow() => Singleton<PreemptiveCoroutineManager>.Instance.IncrementId(new PreemptiveCoroutineManager.CoroutineFunction(this.StartBadgeShow));

  public void Draw(Rect rect)
  {
    GUI.BeginGroup(new Rect(rect.x, rect.y, rect.width, rect.height - 2f), GUIContent.none, StormFront.GrayPanelBox);
    this.DrawPlayerBadge(new Rect((float) (((double) rect.width - 180.0) / 2.0), 10f, 180f, 125f));
    this.DrawStatsDetail(new Rect(0.0f, 140f, rect.width, rect.height - 140f));
    GUI.EndGroup();
  }

  private void DrawPlayerBadge(Rect rect)
  {
    if (!((Object) this._curBadge != (Object) null))
      return;
    GUI.DrawTexture(rect, (Texture) this._curBadge);
  }

  private void DrawStatsDetail(Rect rect)
  {
    if (this._curPlayerStats == null)
      return;
    GUI.BeginGroup(rect);
    GUI.contentColor = ColorScheme.UberStrikeYellow;
    GUI.Label(new Rect(0.0f, 5f, rect.width, 20f), this._curBadgeTitle, BlueStonez.label_interparkbold_16pt);
    GUI.contentColor = Color.white;
    GUI.Label(new Rect(0.0f, 30f, rect.width, 20f), this._curBadgeText, BlueStonez.label_interparkbold_16pt);
    GUI.Label(new Rect(0.0f, 60f, rect.width, 20f), this._curPlayerStats.Name, BlueStonez.label_interparkbold_18pt);
    GUI.EndGroup();
  }

  [DebuggerHidden]
  private IEnumerator StartBadgeShow() => (IEnumerator) new ValuablePlayerDetailGUI.\u003CStartBadgeShow\u003Ec__Iterator1C()
  {
    \u003C\u003Ef__this = this
  };

  private string GetAchievementTitle(AchievementType type)
  {
    switch (type)
    {
      case AchievementType.None:
        return "UBERSTRIKE PLAYER";
      case AchievementType.MostValuable:
        return "MOST VALUABLE";
      case AchievementType.MostAggressive:
        return "MOST AGGRESSIVE";
      case AchievementType.SharpestShooter:
        return "SHARPEST SHOOTER";
      case AchievementType.TriggerHappy:
        return "TRIGGER HAPPY";
      case AchievementType.HardestHitter:
        return "HARDEST HITTER";
      case AchievementType.CostEffective:
        return "COST EFFECTIVE";
      default:
        return string.Empty;
    }
  }

  private void SetCurrentAchievementBadge(AchievementType type, int value)
  {
    this._curBadge = UberstrikeIconsHelper.GetAchievementBadgeTexture(type);
    this._curBadgeTitle = this.GetAchievementTitle(type);
    switch (type)
    {
      case AchievementType.MostValuable:
        this._curBadgeText = string.Format("Best KDR: {0:N1}", (object) (float) ((double) value / 10.0));
        break;
      case AchievementType.MostAggressive:
        this._curBadgeText = string.Format("Total Kills: {0:N0}", (object) value);
        break;
      case AchievementType.SharpestShooter:
        this._curBadgeText = string.Format("Critial Strikes: {0:N0}", (object) value);
        break;
      case AchievementType.TriggerHappy:
        this._curBadgeText = string.Format("Kills in a row: {0:N0}", (object) value);
        break;
      case AchievementType.HardestHitter:
        this._curBadgeText = string.Format("Damage Dealt: {0:N0}", (object) value);
        break;
      case AchievementType.CostEffective:
        this._curBadgeText = string.Format("Accuracy: {0:N1}%", (object) (float) ((double) value / 10.0));
        break;
      default:
        this._curBadgeText = string.Format("KDR: {0:N1}", (object) (float) ((double) value / 10.0));
        break;
    }
  }
}
