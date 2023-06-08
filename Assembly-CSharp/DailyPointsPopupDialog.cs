using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

internal class DailyPointsPopupDialog : BaseEventPopup
{
	private DailyBonusView View;
	public DailyPointsPopupDialog(DailyBonusView view)
	{
		View = view;
		Width = 500;
		Height = 400;
	}

	protected override void DrawGUI(Rect rect)
	{
		GUI.color = ColorScheme.HudTeamBlue;
		GUI.DrawTexture(new Rect(-50f, -20f, rect.width + 100f, 100f), HudTextures.WhiteBlur128);
		GUI.color = Color.white;
		GUITools.OutlineLabel(new Rect(0f, 10f, rect.width, 50f), "Daily Reward : Day "+View.Streak, BlueStonez.label_interparkbold_32pt, 1, Color.white, ColorScheme.GuiTeamBlue);
		int num = 230;
		HandleDailyReward(new Rect((rect.width - (float)num) / 2f, rect.height - (float)num - 25f, 50, 50));
		GUI.color = ColorScheme.HudTeamBlue;
		GUI.DrawTexture(new Rect(-50f, 25f, rect.width + 100f, 120f), HudTextures.WhiteBlur128);
		GUI.color = Color.white;
		GUITools.OutlineLabel(new Rect(0f, 35f, rect.width, 100f), View.Label , BlueStonez.label_interparkbold_48pt, 1, Color.white, ColorScheme.GuiTeamBlue);
		GUITools.OutlineLabel(new Rect(0f, rect.height - 50f, rect.width, 50f), $"Come back tomorrow for more rewards!", BlueStonez.label_interparkbold_13pt, 1, new Color(0.9f, 0.9f, 0.9f), ColorScheme.GuiTeamBlue.SetAlpha(0.5f));
	}

	protected void HandleDailyReward(Rect rect)
	{
		if(View.RewardClass == -1)
		{
			GUI.DrawTexture(rect, ShopIcons.Points48x48);
		}
		else if(View.RewardClass == -2)
		{
			GUI.DrawTexture(rect, ShopIcons.CreditsIcon48x48);
		}
		else if(View.RewardClass > 0)
		{
			Singleton<ItemManager>.Instance.GetItemInShop(View.RewardClass)?.DrawIcon(rect);
		}
	}
}
