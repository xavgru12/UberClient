// Decompiled with JetBrains decompiler
// Type: DailyPointsPopupDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

internal class DailyPointsPopupDialog : BaseEventPopup
{
  private DailyPointsView _points;

  public DailyPointsPopupDialog(DailyPointsView dailypoints)
  {
    if (dailypoints != null)
      this._points = dailypoints;
    else
      this._points = new DailyPointsView()
      {
        Current = 700,
        PointsTomorrow = 800,
        PointsMax = 1000
      };
    this.Width = 500;
    this.Height = 330;
  }

  protected override void DrawGUI(Rect rect)
  {
    GUI.color = ColorScheme.HudTeamBlue;
    GUI.DrawTexture(new Rect(-50f, -20f, rect.width + 100f, 100f), (Texture) HudTextures.WhiteBlur128);
    GUI.color = Color.white;
    GUITools.OutlineLabel(new Rect(0.0f, 10f, rect.width, 50f), "Daily Reward", BlueStonez.label_interparkbold_32pt, 1, Color.white, ColorScheme.GuiTeamBlue);
    int num = 230;
    GUI.DrawTexture(new Rect((float) (((double) rect.width - (double) num) / 2.0), (float) ((double) rect.height - (double) num - 25.0), (float) num, (float) num), (Texture) ShopIcons.Points48x48);
    GUI.color = ColorScheme.HudTeamBlue;
    GUI.DrawTexture(new Rect(-50f, 25f, rect.width + 100f, 120f), (Texture) HudTextures.WhiteBlur128);
    GUI.color = Color.white;
    GUITools.OutlineLabel(new Rect(0.0f, 35f, rect.width, 100f), this._points.Current.ToString() + " POINTS", BlueStonez.label_interparkbold_48pt, 1, Color.white, ColorScheme.GuiTeamBlue);
    GUITools.OutlineLabel(new Rect(0.0f, rect.height - 50f, rect.width, 50f), string.Format("Come back tomorrow for {0} points!", (object) this.GetPointsForTomorrow()), BlueStonez.label_interparkbold_13pt, 1, new Color(0.9f, 0.9f, 0.9f), ColorScheme.GuiTeamBlue.SetAlpha(0.5f));
  }

  private int GetPointsForTomorrow() => this._points.PointsTomorrow;
}
