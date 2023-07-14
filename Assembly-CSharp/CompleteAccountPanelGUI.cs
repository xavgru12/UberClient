// Decompiled with JetBrains decompiler
// Type: CompleteAccountPanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

  private void Awake()
  {
    this._availableNames = new List<string>();
    this._errorMessages = new Dictionary<int, string>();
    this._errorMessages.Add(3, string.Empty);
    this._errorMessages.Add(2, string.Empty);
    this._errorMessages.Add(0, string.Empty);
    this._errorMessages.Add(4, string.Empty);
    this._errorMessages.Add(5, LocalizedStrings.YourAccountHasBeenBanned);
  }

  private void OnGUI()
  {
    float width = 400f;
    if ((double) this._height != (double) this._targetHeight)
    {
      this._height = Mathf.Lerp(this._height, this._targetHeight, Time.deltaTime * 5f);
      if (Mathf.Approximately(this._height, this._targetHeight))
        this._height = this._targetHeight;
    }
    GUI.depth = 1;
    Rect position1 = new Rect((float) (((double) Screen.width - (double) width) * 0.5), (float) (((double) Screen.height - (double) this._height) * 0.5), width, this._height);
    GUI.BeginGroup(position1, GUIContent.none, BlueStonez.window);
    GUI.Label(new Rect(0.0f, 0.0f, position1.width, 56f), LocalizedStrings.ChooseCharacterName, BlueStonez.tab_strip);
    Rect position2 = new Rect(20f, 55f, position1.width - 40f, position1.height - 76f);
    GUI.Label(position2, GUIContent.none, BlueStonez.window_standard_grey38);
    GUI.BeginGroup(position2);
    GUI.Label(new Rect(10f, 8f, position2.width - 20f, 40f), "Please choose your character name.\nThis is the name that will be displayed to other players in game.", BlueStonez.label_interparkbold_11pt);
    GUI.color = new Color(1f, 1f, 1f, 0.3f);
    GUI.Label(new Rect(20f, 66f, 15f, 11f), (18 - this._characterName.Length).ToString(), BlueStonez.label_interparkmed_11pt_right);
    GUI.color = Color.white;
    GUI.enabled = !this._waitingForWsReturn;
    GUI.changed = false;
    GUI.SetNextControlName("@Name");
    this._characterName = GUI.TextField(new Rect(40f, 60f, 180f, 22f), this._characterName, 18, BlueStonez.textField);
    this._characterName = TextUtilities.Trim(this._characterName);
    if (GUI.changed)
    {
      this._selectedIndex = -1;
      this._checkButtonClicked = false;
    }
    if (string.IsNullOrEmpty(this._characterName) && GUI.GetNameOfFocusedControl() != "@Name")
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(new Rect(85f, 67f, 180f, 22f), LocalizedStrings.EnterYourName, BlueStonez.label_interparkmed_11pt_left);
      GUI.color = Color.white;
    }
    GUI.enabled = true;
    this.DrawCheckAvailabilityButton(position2);
    if (this._waitingForWsReturn)
    {
      GUI.contentColor = Color.gray;
      GUI.Label(new Rect(165f, 100f, 100f, 20f), LocalizedStrings.PleaseWait, BlueStonez.label_interparkbold_11pt_left);
      GUI.contentColor = Color.white;
      WaitingTexture.Draw(new Vector2(140f, 110f));
    }
    else
    {
      GUI.contentColor = this._feedbackMessageColor;
      GUI.Label(new Rect(0.0f, 100f, position2.width, 20f), this._errorMessage, BlueStonez.label_interparkbold_11pt);
      GUI.contentColor = Color.white;
    }
    this.DrawAvailableNames(new Rect(0.0f, 120f, position2.width, position2.height - 162f));
    this.DrawOKButton(position2);
    GUI.EndGroup();
    GUI.EndGroup();
  }

  private void DrawCheckAvailabilityButton(Rect position)
  {
    GUI.enabled = !string.IsNullOrEmpty(this._characterName) && !this._checkButtonClicked && !this._waitingForWsReturn;
    if (GUITools.Button(new Rect(225f, 60f, 110f, 24f), new GUIContent("Check Availability"), BlueStonez.buttondark_small))
    {
      this._availableNames.Clear();
      this._checkButtonClicked = true;
      this._targetHeight = 260f;
      if (!ValidationUtilities.IsValidMemberName(this._characterName, ((Enum) ApplicationDataManager.CurrentLocale).ToString()))
      {
        this._feedbackMessageColor = Color.red;
        this._errorMessage = "'" + this._characterName + "' is not a valid name!";
      }
      else
      {
        this._waitingForWsReturn = true;
        UserWebServiceClient.IsDuplicateMemberName(this._characterName, new Action<bool>(this.IsDuplicatedNameCallback), (Action<Exception>) (ex =>
        {
          this._waitingForWsReturn = false;
          this._feedbackMessageColor = Color.red;
          this._errorMessage = "Our server had an error, please try again.";
          DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace);
        }));
      }
    }
    GUI.enabled = true;
  }

  private void DrawOKButton(Rect position)
  {
    GUI.enabled = !this._waitingForWsReturn && !string.IsNullOrEmpty(this._characterName);
    if (GUITools.Button(new Rect((float) (((double) position.width - 120.0) / 2.0), position.height - 42f, 120f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button_green))
    {
      string name = this._characterName;
      if (this._selectedIndex != -1)
        name = this._availableNames[this._selectedIndex];
      this._waitingForWsReturn = true;
      AuthenticationWebServiceClient.CompleteAccount(PlayerDataManager.CmidSecure, name, ApplicationDataManager.Channel, ((Enum) ApplicationDataManager.CurrentLocale).ToString(), SystemInfo.deviceUniqueIdentifier, (Action<AccountCompletionResultView>) (ev => this.CompleteAccountCallback(ev, name)), (Action<Exception>) (ex =>
      {
        this._waitingForWsReturn = false;
        this._feedbackMessageColor = Color.red;
        this._errorMessage = "Webservice error";
        DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.");
      }));
    }
    GUI.enabled = true;
  }

  private void DrawAvailableNames(Rect position)
  {
    if (this._availableNames.Count == 0)
      return;
    GUI.BeginGroup(position);
    GUI.Label(new Rect(0.0f, 0.0f, position.width, 20f), "Here are some suggestions", BlueStonez.label_interparkbold_11pt);
    GUI.enabled = !this._waitingForWsReturn;
    for (int index = 0; index < this._availableNames.Count; ++index)
    {
      if (GUI.Toggle(new Rect(94f, (float) (24 + index * 20), position.width, 18f), index == this._selectedIndex, this._availableNames[index], BlueStonez.radiobutton))
        this._selectedIndex = index;
    }
    GUI.enabled = true;
    GUI.EndGroup();
  }

  private void IsDuplicatedNameCallback(bool isDuplicate)
  {
    if (isDuplicate)
    {
      UserWebServiceClient.GenerateNonDuplicatedMemberNames(this._characterName, new Action<List<string>>(this.GetNonDuplicatedNamesCallback), (Action<Exception>) (ex =>
      {
        this._waitingForWsReturn = false;
        DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.");
      }));
    }
    else
    {
      this._waitingForWsReturn = false;
      this._feedbackMessageColor = Color.green;
      this._errorMessage = "'" + this._characterName + "' is available!";
    }
  }

  private void GetNonDuplicatedNamesCallback(List<string> names)
  {
    this._selectedIndex = -1;
    this._targetHeight = 330f;
    this._waitingForWsReturn = false;
    this._feedbackMessageColor = Color.red;
    this._errorMessage = "'" + this._characterName + "' is already taken!";
    this._availableNames.Clear();
    this._availableNames.AddRange((IEnumerable<string>) names);
  }

  private void CompleteAccountCallback(AccountCompletionResultView result, string name)
  {
    this._selectedIndex = -1;
    this._waitingForWsReturn = false;
    switch (result.Result)
    {
      case 1:
        this.Hide();
        if ((bool) (UnityEngine.Object) GameState.LocalDecorator)
          GameState.LocalDecorator.HudInformation.SetAvatarLabel(name);
        List<IUnityItem> items = new List<IUnityItem>();
        foreach (int key in result.ItemsAttributed.Keys)
          items.Add(Singleton<ItemManager>.Instance.GetItemInShop(key));
        PlayerDataManager.NameSecure = name;
        this.StartCoroutine(this.StartPreparingNewPlayersLoadout(items));
        CommConnectionManager.CommCenter.SendUpdatedActorInfo();
        if (items.Count > 0)
        {
          Singleton<EventPopupManager>.Instance.AddEventPopup((IPopupDialog) new ItemListPopupDialog("New Items", "You're now ready to start kicking ass!\nUse the PLAY button to join or create a game.", items, ShopArea.Shop));
          UnityEngine.Debug.Log((object) ("You've got new items: " + (object) items.Count));
        }
        Singleton<GameStateController>.Instance.LeaveGame();
        break;
      case 2:
        this.GetNonDuplicatedNamesCallback(result.NonDuplicateNames);
        break;
      case 3:
        this.Hide();
        Singleton<SceneLoader>.Instance.LoadLevel("Menu");
        break;
      case 4:
        this._feedbackMessageColor = Color.red;
        this._errorMessage = LocalizedStrings.NameInvalidCharsMsg;
        break;
      case 5:
        this._feedbackMessageColor = Color.red;
        this._errorMessage = LocalizedStrings.YourAccountHasBeenBanned;
        break;
    }
  }

  [DebuggerHidden]
  private IEnumerator StartPreparingNewPlayersLoadout(List<IUnityItem> items) => (IEnumerator) new CompleteAccountPanelGUI.\u003CStartPreparingNewPlayersLoadout\u003Ec__Iterator1D()
  {
    items = items,
    \u003C\u0024\u003Eitems = items,
    \u003C\u003Ef__this = this
  };
}
