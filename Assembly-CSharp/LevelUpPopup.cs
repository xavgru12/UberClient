using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpPopup : IPopupDialog
{
	protected int Width = 650;

	protected int Height = 330;

	private ShopItemGrid _itemGrid;

	private Action _action;

	private int _level;

	public string Text
	{
		get;
		set;
	}

	public string Title
	{
		get;
		set;
	}

	public bool IsWaiting
	{
		get;
		set;
	}

	public GuiDepth Depth => GuiDepth.Event;

	public LevelUpPopup(int level, Action action = null)
		: this(level, level - 1, action)
	{
	}

	public LevelUpPopup(int newLevel, int previousLevel, Action action = null)
	{
		_action = action;
		_level = newLevel;
		Title = "Level Up";
		Text = "Congratulations, you reached level " + _level.ToString() + "!";
		Width = 388;
		Height = 560 - GlobalUIRibbon.Instance.Height() - 10;
		List<ShopItemView> list = new List<ShopItemView>();
		for (int num = newLevel; num > previousLevel; num--)
		{
			list.AddRange(GetItemsUnlocked(num));
		}
		AutoMonoBehaviour<SfxManager>.Instance.Play2dAudioClip(GameAudio.LevelUp, 0uL);
		_itemGrid = new ShopItemGrid(list);
		_itemGrid.Show = true;
	}

	public void OnGUI()
	{
		Rect position = GetPosition();
		GUI.Box(position, GUIContent.none, BlueStonez.window);
		GUITools.PushGUIState();
		GUI.BeginGroup(position);
		DrawPlayGUI(position);
		GUI.EndGroup();
		GUITools.PopGUIState();
		if (IsWaiting)
		{
			WaitingTexture.Draw(position.center);
		}
	}

	private Rect GetPosition()
	{
		float left = (float)(Screen.width - Width) * 0.5f;
		float top = (float)GlobalUIRibbon.Instance.Height() + (float)(Screen.height - GlobalUIRibbon.Instance.Height() - Height) * 0.5f;
		return new Rect(left, top, Width, Height);
	}

	private List<ShopItemView> GetItemsUnlocked(int level)
	{
		List<ShopItemView> list = new List<ShopItemView>();
		if (level > 1)
		{
			foreach (IUnityItem shopItem in Singleton<ItemManager>.Instance.ShopItems)
			{
				if (shopItem.View.LevelLock == level && shopItem.View.IsForSale)
				{
					list.Add(new ShopItemView(shopItem.View.ID));
				}
			}
			return list;
		}
		return list;
	}

	private void DrawPlayGUI(Rect rect)
	{
		GUI.color = ColorScheme.HudTeamBlue;
		Vector2 vector = BlueStonez.label_interparkbold_18pt.CalcSize(new GUIContent(Title));
		float num = vector.x * 2.5f;
		GUI.DrawTexture(new Rect((rect.width - num) * 0.5f, -29f, num, 100f), HudTextures.WhiteBlur128);
		GUI.color = Color.white;
		GUITools.OutlineLabel(new Rect(0f, 10f, rect.width, 30f), Title, BlueStonez.label_interparkbold_18pt, 1, Color.white, ColorScheme.GuiTeamBlue.SetAlpha(0.5f));
		GUI.Label(new Rect(30f, 35f, rect.width - 60f, 40f), Text, BlueStonez.label_interparkbold_16pt);
		int num2 = 288;
		int num3 = (Width - num2 - 6) / 2;
		int num4 = 323;
		int count = _itemGrid.Items.Count;
		GUI.BeginGroup(new Rect(num3, 75f, num2, num4), BlueStonez.item_slot_large);
		Rect position = new Rect((num2 - 282) / 2, (num4 - 317) / 2, 282f, 317f);
		GUI.DrawTexture(position, UberstrikeIcons.LevelUpPopup);
		if (count > 0)
		{
			_itemGrid.Draw(new Rect(0f, 0f, num2, num4));
		}
		GUI.EndGroup();
		if (count > 0)
		{
			GUI.Label(new Rect(30f, rect.height - 107f, rect.width - 60f, 40f), string.Format("You unlocked {0} new item{1}.", count, (count != 1) ? "s" : string.Empty), BlueStonez.label_interparkbold_16pt);
		}
		int num5 = -70;
		if (GUI.Button(new Rect(rect.width * 0.5f + (float)num5, rect.height - 47f, 140f, 30f), "OK", BlueStonez.buttongold_large_price))
		{
			PopupSystem.HideMessage(this);
			if (_action != null)
			{
				_action();
			}
		}
	}

	public void OnHide()
	{
	}
}
