using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

internal class WeekendChallengePopupDialog : BaseEventPopup
{
    public WeekendChallengePopupDialog() 
    {
        Width = 500;
        Height = 400;
    }

    protected override void DrawGUI(Rect rect)
    {
		GUI.color = ColorScheme.HudTeamBlue;
		GUI.DrawTexture(new Rect(-50f, -20f, rect.width + 100f, 100f), HudTextures.WhiteBlur128);
		GUI.color = Color.white;
		GUITools.OutlineLabel(new Rect(0f, 10f, rect.width, 50f), "Congratulations!", BlueStonez.label_interparkbold_32pt, 1, Color.white, ColorScheme.GuiTeamBlue);
		int num = 230;
		GUI.color = ColorScheme.HudTeamBlue;
		GUI.DrawTexture(new Rect(-50f, 25f, rect.width + 100f, 120f), HudTextures.WhiteBlur128);
		GUI.color = Color.white;
		GUITools.OutlineLabel(new Rect(0f, 35f, rect.width, 100f),"Weekend Challenge mastered.", BlueStonez.label_interparkbold_13pt, 1, Color.white, ColorScheme.GuiTeamBlue);
		GUITools.OutlineLabel(new Rect(0f, rect.height - 50f, rect.width, 50f), "Relogin and check your inventory!", BlueStonez.label_interparkbold_13pt, 1, new Color(0.9f, 0.9f, 0.9f), ColorScheme.GuiTeamBlue.SetAlpha(0.5f));
	}
}

