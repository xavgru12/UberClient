using System.Collections;
using System.Collections.Generic;
using UberStrike.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class CompleteAccountPanelGUI : PanelGuiBase
{
	private const int MAX_CHARACTER_NAME_LENGTH = 18;

	private const float NORMAL_HEIGHT = 260f;

	private const float EXTENDED_HEIGHT = 330f;

	private string _characterName = string.Empty;

	private float _height = 260f;

	private float _targetHeight = 260f;

	private bool _checkButtonClicked;

	private string _errorMessage = string.Empty;

	private Dictionary<int, string> _errorMessages;

	private List<string> _availableNames;

	private int _selectedIndex = -1;

	private bool _waitingForWsReturn;

	private Color _feedbackMessageColor = Color.white;

	private float _keyboardOffset;

	private float _targetKeyboardOffset;

	private void Awake()
	{
		_availableNames = new List<string>();
		_errorMessages = new Dictionary<int, string>();
		_errorMessages.Add(3, string.Empty);
		_errorMessages.Add(2, string.Empty);
		_errorMessages.Add(0, string.Empty);
		_errorMessages.Add(4, string.Empty);
		_errorMessages.Add(5, LocalizedStrings.YourAccountHasBeenBanned);
		_targetKeyboardOffset = 0f;
	}

	private void Update()
	{
	}

	private void HideKeyboard()
	{
	}

	private void OnGUI()
	{
		float num = 400f;
		if (Mathf.Abs(_keyboardOffset - _targetKeyboardOffset) > 2f)
		{
			_keyboardOffset = Mathf.Lerp(_keyboardOffset, _targetKeyboardOffset, Time.deltaTime * 4f);
		}
		else
		{
			_keyboardOffset = _targetKeyboardOffset;
		}
		if (_height != _targetHeight)
		{
			_height = Mathf.Lerp(_height, _targetHeight, Time.deltaTime * 5f);
			if (Mathf.Approximately(_height, _targetHeight))
			{
				_height = _targetHeight;
			}
		}
		GUI.depth = 1;
		Rect position = new Rect(((float)Screen.width - num) * 0.5f, ((float)Screen.height - _height) * 0.5f - _keyboardOffset, num, _height);
		GUI.BeginGroup(position, GUIContent.none, BlueStonez.window);
		GUI.Label(new Rect(0f, 0f, position.width, 56f), LocalizedStrings.ChooseCharacterName, BlueStonez.tab_strip);
		Rect position2 = new Rect(20f, 55f, position.width - 40f, position.height - 76f);
		GUI.Label(position2, GUIContent.none, BlueStonez.window_standard_grey38);
		GUI.BeginGroup(position2);
		GUI.Label(new Rect(10f, 8f, position2.width - 20f, 40f), "Please choose your character name.\nThis is the name that will be displayed to other players in game.", BlueStonez.label_interparkbold_11pt);
		GUI.color = new Color(1f, 1f, 1f, 0.3f);
		GUI.Label(new Rect(20f, 66f, 15f, 11f), (18 - _characterName.Length).ToString(), BlueStonez.label_interparkmed_11pt_right);
		GUI.color = Color.white;
		GUI.enabled = !_waitingForWsReturn;
		GUI.changed = false;
		GUI.SetNextControlName("@Name");
		_characterName = GUI.TextField(new Rect(40f, 60f, 180f, 22f), _characterName, 18, BlueStonez.textField);
		_characterName = TextUtilities.Trim(_characterName);
		if (GUI.changed)
		{
			_selectedIndex = -1;
			_checkButtonClicked = false;
		}
		if (string.IsNullOrEmpty(_characterName) && GUI.GetNameOfFocusedControl() != "@Name")
		{
			GUI.color = new Color(1f, 1f, 1f, 0.3f);
			GUI.Label(new Rect(85f, 67f, 180f, 22f), LocalizedStrings.EnterYourName, BlueStonez.label_interparkmed_11pt_left);
			GUI.color = Color.white;
		}
		GUI.enabled = true;
		DrawCheckAvailabilityButton(position2);
		if (_waitingForWsReturn)
		{
			GUI.contentColor = Color.gray;
			GUI.Label(new Rect(165f, 100f, 100f, 20f), LocalizedStrings.PleaseWait, BlueStonez.label_interparkbold_11pt_left);
			GUI.contentColor = Color.white;
			WaitingTexture.Draw(new Vector2(140f, 110f));
		}
		else
		{
			GUI.contentColor = _feedbackMessageColor;
			GUI.Label(new Rect(0f, 100f, position2.width, 20f), _errorMessage, BlueStonez.label_interparkbold_11pt);
			GUI.contentColor = Color.white;
		}
		DrawAvailableNames(new Rect(0f, 120f, position2.width, position2.height - 162f));
		DrawOKButton(position2);
		GUI.EndGroup();
		GUI.EndGroup();
	}

	private void DrawCheckAvailabilityButton(Rect position)
	{
		GUI.enabled = (!string.IsNullOrEmpty(_characterName) && !_checkButtonClicked && !_waitingForWsReturn);
		if (GUITools.Button(new Rect(225f, 60f, 110f, 24f), new GUIContent("Check Availability"), BlueStonez.buttondark_small))
		{
			HideKeyboard();
			_availableNames.Clear();
			_checkButtonClicked = true;
			_targetHeight = 260f;
			if (!ValidationUtilities.IsValidMemberName(_characterName, ApplicationDataManager.CurrentLocale.ToString()))
			{
				_feedbackMessageColor = Color.red;
				_errorMessage = "'" + _characterName + "' is not a valid name!";
			}
			else
			{
				_waitingForWsReturn = true;
				UserWebServiceClient.IsDuplicateMemberName(_characterName, IsDuplicatedNameCallback, delegate
				{
					_waitingForWsReturn = false;
					_feedbackMessageColor = Color.red;
					_errorMessage = "Our server had an error, please try again.";
				});
			}
		}
		GUI.enabled = true;
	}

	private void DrawOKButton(Rect position)
	{
		GUI.enabled = (!_waitingForWsReturn && !string.IsNullOrEmpty(_characterName));
		if (GUITools.Button(new Rect((position.width - 120f) / 2f, position.height - 42f, 120f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button_green))
		{
			HideKeyboard();
			string name = _characterName;
			if (_selectedIndex != -1)
			{
				name = _availableNames[_selectedIndex];
			}
			_waitingForWsReturn = true;
			AuthenticationWebServiceClient.CompleteAccount(PlayerDataManager.Cmid, name, ApplicationDataManager.Channel, ApplicationDataManager.CurrentLocale.ToString(), SystemInfo.deviceUniqueIdentifier, delegate(AccountCompletionResultView ev)
			{
				CompleteAccountCallback(ev, name);
			}, delegate
			{
				_waitingForWsReturn = false;
				_feedbackMessageColor = Color.red;
				_errorMessage = "Webservice error";
			});
		}
		GUI.enabled = true;
	}

	private void DrawAvailableNames(Rect position)
	{
		if (_availableNames.Count == 0)
		{
			return;
		}
		GUI.BeginGroup(position);
		GUI.Label(new Rect(0f, 0f, position.width, 20f), "Here are some suggestions", BlueStonez.label_interparkbold_11pt);
		GUI.enabled = !_waitingForWsReturn;
		for (int i = 0; i < _availableNames.Count; i++)
		{
			if (GUI.Toggle(new Rect(94f, 24 + i * 20, position.width, 18f), i == _selectedIndex, _availableNames[i], BlueStonez.radiobutton))
			{
				_selectedIndex = i;
			}
		}
		GUI.enabled = true;
		GUI.EndGroup();
	}

	private void IsDuplicatedNameCallback(bool isDuplicate)
	{
		if (isDuplicate)
		{
			UserWebServiceClient.GenerateNonDuplicatedMemberNames(_characterName, GetNonDuplicatedNamesCallback, delegate
			{
				_waitingForWsReturn = false;
			});
			return;
		}
		_waitingForWsReturn = false;
		_feedbackMessageColor = Color.green;
		_errorMessage = "'" + _characterName + "' is available!";
	}

	private void GetNonDuplicatedNamesCallback(List<string> names)
	{
		_selectedIndex = -1;
		_targetHeight = 330f;
		_waitingForWsReturn = false;
		_feedbackMessageColor = Color.red;
		_errorMessage = "'" + _characterName + "' is already taken!";
		_availableNames.Clear();
		_availableNames.AddRange(names);
	}

	private void CompleteAccountCallback(AccountCompletionResultView result, string name)
	{
		_selectedIndex = -1;
		_waitingForWsReturn = false;
		switch (result.Result)
		{
		case 1:
		{
			Hide();
			List<IUnityItem> list = new List<IUnityItem>();
			foreach (int key in result.ItemsAttributed.Keys)
			{
				list.Add(Singleton<ItemManager>.Instance.GetItemInShop(key));
			}
			PlayerDataManager.Name = name;
			AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Operations.SendAuthenticationRequest(PlayerDataManager.AuthToken, PlayerDataManager.MagicHash, ApplicationDataManager.IsMac);
			StartCoroutine(StartPreparingNewPlayersLoadout(list));
			break;
		}
		case 2:
			GetNonDuplicatedNamesCallback(result.NonDuplicateNames);
			break;
		case 5:
			_feedbackMessageColor = Color.red;
			_errorMessage = LocalizedStrings.YourAccountHasBeenBanned;
			break;
		case 4:
			_feedbackMessageColor = Color.red;
			_errorMessage = LocalizedStrings.NameInvalidCharsMsg;
			break;
		case 3:
			Hide();
			Singleton<SceneLoader>.Instance.LoadLevel("Menu");
			break;
		}
	}

	private IEnumerator StartPreparingNewPlayersLoadout(List<IUnityItem> items)
	{
		yield return StartCoroutine(Singleton<ItemManager>.Instance.StartGetInventory(showProgress: false));
		IUnityItem unityItem = items.Find((IUnityItem i) => i.View.ID == 1);
		if (unityItem != null)
		{
			InventoryItem inventoryItem = new InventoryItem(unityItem);
			Singleton<LoadoutManager>.Instance.SetLoadoutItem(LoadoutSlotType.WeaponMelee, inventoryItem.Item);
		}
		unityItem = items.Find((IUnityItem i) => i.View.ID == 12);
		if (unityItem != null)
		{
			InventoryItem inventoryItem2 = new InventoryItem(unityItem);
			Singleton<LoadoutManager>.Instance.SetLoadoutItem(LoadoutSlotType.WeaponPrimary, inventoryItem2.Item);
		}
		unityItem = items.Find((IUnityItem i) => i.View.ID == 44);
		if (unityItem != null)
		{
			InventoryItem inventoryItem3 = new InventoryItem(unityItem);
			Singleton<LoadoutManager>.Instance.SetLoadoutItem(LoadoutSlotType.WeaponSecondary, inventoryItem3.Item);
		}
		unityItem = items.Find((IUnityItem i) => i.View.ID == 68);
		if (unityItem != null)
		{
			InventoryItem inventoryItem4 = new InventoryItem(unityItem);
			Singleton<LoadoutManager>.Instance.SetLoadoutItem(LoadoutSlotType.WeaponTertiary, inventoryItem4.Item);
		}
		if (items.Count > 0)
		{
			ItemListPopupDialog popup = new ItemListPopupDialog("New Items", "You're now ready to start kicking ass!\nUse the PLAY button to join or create a game.", items, delegate
			{
				Singleton<AuthenticationManager>.Instance.SetAuthComplete(enabled: true);
				MenuPageManager.Instance.LoadPage(PageType.Home, forceReload: true);
			});
			PopupSystem.Show(popup);
			Debug.Log("You've got new items: " + items.Count.ToString());
		}
	}
}
