using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class HUDDesktopChat : MonoBehaviour
{
	[Serializable]
	public class ItemGfx
	{
		public UILabel Label;
	}

	[Serializable]
	public class Item
	{
		public string From = string.Empty;

		public string ColorMod;

		public MemberAccessLevel accessLevel;

		public Color color;

		public string Message = string.Empty;

		public float TimeEnd;
	}

	[SerializeField]
	private UILabel template;

	[SerializeField]
	private UIVerticalAligner aligner;

	[SerializeField]
	private UILabel inputLabel;

	[SerializeField]
	private UISprite inputLabelBgr;

	[SerializeField]
	private float ySpace = -17f;

	[SerializeField]
	private int maxItems = 8;

	[SerializeField]
	private float displayTime = 5f;

	[SerializeField]
	private UILabel muteLabel;

	[SerializeField]
	private UILabel spamLabel;

	private UIInput textInput;

	private List<ItemGfx> itemsGfx = new List<ItemGfx>();

	private List<Item> items = new List<Item>();

	private float lastSpammingTime;

	private bool skipNextEnter;

	private void Start()
	{
		spamLabel.enabled = false;
		textInput = GetComponent<UIInput>();
		if (textInput == null)
		{
			throw new Exception("Chat: no UIInput attached.");
		}
		GameData.Instance.OnHUDChatMessage.AddEvent(AddMessage, this);
		GameData.Instance.OnHUDChatClear.AddEvent(ClearLog, this);
		PropertyExt.AddEvent(GameData.Instance.OnHUDChatStartTyping, delegate
		{
			ActivateTextInput(enabled: true);
		}, this);
		GameData.Instance.GameState.AddEvent((Action<GameStateId>)delegate
		{
			ActivateTextInput(enabled: false);
		}, (MonoBehaviour)this);
		GameData.Instance.PlayerState.AddEvent((Action<PlayerStateId>)delegate
		{
			ActivateTextInput(enabled: false);
		}, (MonoBehaviour)this);
		AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.IsPlayerMuted.AddEventAndFire(delegate(bool el)
		{
			muteLabel.enabled = el;
		}, this);
		foreach (Transform item in template.transform.parent)
		{
			if (item != template.transform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
		for (int i = 0; i < maxItems; i++)
		{
			GameObject gameObject = GameObjectHelper.Instantiate(template.gameObject, template.transform.parent, new Vector3(0f, (float)i * ySpace, 0f), template.transform.localScale);
			gameObject.name = "Item " + i.ToString();
			itemsGfx.Add(new ItemGfx
			{
				Label = gameObject.GetComponent<UILabel>()
			});
		}
		template.gameObject.SetActive(value: false);
		ApplyChanges();
		ActivateTextInput(enabled: false);
	}

	private void OnEnable()
	{
		spamLabel.enabled = false;
	}

	private void ApplyChanges()
	{
		for (int i = 0; i < itemsGfx.Count; i++)
		{
			ItemGfx itemGfx = itemsGfx[i];
			Item item = (i >= items.Count) ? null : items[i];
			itemGfx.Label.gameObject.SetActive(item != null);
			if (item != null)
			{
				if (item.From != string.Empty)
				{
					itemGfx.Label.color = item.color;
					itemGfx.Label.supportEncoding = (item.accessLevel > MemberAccessLevel.Default);
					itemGfx.Label.text = item.From + ": " + item.Message;
				}
				else
				{
					itemGfx.Label.text = string.Empty;
				}
			}
		}
		aligner.Reposition();
	}

	private void OnSubmit(string text)
	{
		text = NGUITools.StripSymbols(text).Trim();
		text = TextUtilities.Trim(text);
		if (!string.IsNullOrEmpty(text) && !GameState.Current.SendChatMessage(text, ChatContext.Player))
		{
			spamLabel.enabled = true;
			lastSpammingTime = Time.time;
		}
		OnInputChanged();
		ActivateTextInput(enabled: false);
		skipNextEnter = true;
		textInput.text = string.Empty;
	}

	private void OnInputChanged(object input = null)
	{
		textInput.text = textInput.text.Replace(textInput.caratChar, string.Empty);
		inputLabel.text = textInput.text;
		Vector3 size = NGUIMath.CalculateRelativeWidgetBounds(inputLabel.transform).size;
		float y = size.y;
		Vector3 localScale = inputLabel.transform.localScale;
		float num = y * localScale.y;
		Transform transform = inputLabelBgr.transform;
		Vector3 localScale2 = inputLabelBgr.transform.localScale;
		Vector3 localScale3 = inputLabel.transform.localScale;
		transform.localScale = localScale2.SetY(num + localScale3.y);
	}

	public void AddMessage(string from, string message, MemberAccessLevel accessLevel)
	{
		items.Add(new Item
		{
			From = from,
			Message = message,
			ColorMod = GUIUtils.ColorToNGuiModifier(ColorScheme.GetNameColorByAccessLevel(accessLevel)),
			accessLevel = accessLevel,
			color = ColorScheme.GetNameColorByAccessLevel(accessLevel),
			TimeEnd = Time.time + displayTime
		});
		if (items.Count > maxItems)
		{
			items.RemoveAt(0);
		}
		ApplyChanges();
	}

	public void ClearLog()
	{
		items.Clear();
		ApplyChanges();
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Return) && !PopupSystem.IsAnyPopupOpen)
		{
			if (!skipNextEnter)
			{
				ActivateTextInput(!GameData.Instance.HUDChatIsTyping);
			}
			skipNextEnter = false;
		}
		if (GameData.Instance.HUDChatIsTyping && !textInput.selected)
		{
			ActivateTextInput(enabled: false);
		}
		while (items.Count > 0 && Time.time >= items[0].TimeEnd)
		{
			items.RemoveAt(0);
			ApplyChanges();
		}
		if (spamLabel.enabled && Time.time >= lastSpammingTime + 5f)
		{
			spamLabel.enabled = false;
		}
	}

	private void ActivateTextInput(bool enabled)
	{
		enabled &= !AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.IsPlayerMuted;
		GameData.Instance.HUDChatIsTyping = enabled;
		textInput.selected = enabled;
		if (enabled)
		{
			OnInputChanged();
		}
		inputLabel.enabled = enabled;
		inputLabelBgr.enabled = enabled;
		EventHandler.Global.Fire(new GameEvents.ChatWindow
		{
			IsEnabled = enabled
		});
	}
}
