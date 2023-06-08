using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.Core.Models.Views;
using UnityEngine;

public abstract class ItemPriceGUI
{
	protected bool _levelLocked;

	protected string _tooltip = string.Empty;

	protected ItemPrice _selectedPrice;

	protected Action<ItemPrice> _onPriceSelected;

	public int Height
	{
		get;
		protected set;
	}

	public ItemPrice SelectedPriceOption => _selectedPrice;

	public ItemPriceGUI(int levelLock, Action<ItemPrice> onPriceSelected)
	{
		if (levelLock > PlayerDataManager.PlayerLevel)
		{
			_levelLocked = true;
			_tooltip = $"Not so fast, squirt!\n\nYou need to be Level {levelLock} to buy this item using points.\n\nGet fragging!";
		}
		_onPriceSelected = delegate(ItemPrice price)
		{
			_selectedPrice = price;
		};
		_onPriceSelected = (Action<ItemPrice>)Delegate.Combine(_onPriceSelected, onPriceSelected);
	}

	public abstract void Draw(Rect rect);

	protected int DrawPrice(ItemPrice price, float width, int y)
	{
		string text = (price.Price <= 0) ? " FREE" : $" {price.Price:N0}";
		Texture image = (price.Currency != UberStrikeCurrencyType.Points) ? ShopIcons.IconCredits20x20 : ShopIcons.IconPoints20x20;
		GUIContent content = new GUIContent(text, image);
		GUI.Label(new Rect(width, y, width, 20f), content, BlueStonez.label_itemdescription);
		if (price.Price > 0 && price.Discount > 0)
		{
			string text2 = string.Format(LocalizedStrings.DiscountPercentOff, price.Discount);
			GUI.color = ColorScheme.UberStrikeYellow;
			GUI.Label(new Rect(width + 80f, y + 5, width, 20f), text2, BlueStonez.label_itemdescription);
			GUI.color = Color.white;
		}
		return y += 24;
	}
}
