// Decompiled with JetBrains decompiler
// Type: SignupPanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class SignupPanelGUI : PanelGuiBase
{
  private const float NORMAL_HEIGHT = 300f;
  private const float EXTENDED_HEIGHT = 340f;
  private string _emailAddress = string.Empty;
  private string _password1 = string.Empty;
  private string _password2 = string.Empty;
  private string _errorMessage = string.Empty;
  private Color _errorMessageColor = Color.red;
  private Dictionary<MemberRegistrationResult, string> _errorMessages;
  private bool _enableGUI = true;
  private float _keyboardOffset;
  private float _targetKeyboardOffset;
  private float _height = 300f;
  private float _targetHeight = 300f;

  private void Awake() => this._errorMessages = new Dictionary<MemberRegistrationResult, string>();

  private void Start()
  {
    this._errorMessages.Add(MemberRegistrationResult.DuplicateEmail, LocalizedStrings.EmailAddressInUseMsg);
    this._errorMessages.Add(MemberRegistrationResult.DuplicateEmailName, LocalizedStrings.EmailAddressAndNameInUseMsg);
    this._errorMessages.Add(MemberRegistrationResult.DuplicateHandle, LocalizedStrings.NameInUseMsg);
    this._errorMessages.Add(MemberRegistrationResult.DuplicateName, LocalizedStrings.NameInUseMsg);
    this._errorMessages.Add(MemberRegistrationResult.InvalidData, LocalizedStrings.InvalidData);
    this._errorMessages.Add(MemberRegistrationResult.InvalidEmail, LocalizedStrings.EmailAddressIsInvalid);
    this._errorMessages.Add(MemberRegistrationResult.InvalidEsns, LocalizedStrings.InvalidData + " (Esns)");
    this._errorMessages.Add(MemberRegistrationResult.InvalidHandle, LocalizedStrings.InvalidData + " (Handle)");
    this._errorMessages.Add(MemberRegistrationResult.InvalidName, LocalizedStrings.NameInvalidCharsMsg);
    this._errorMessages.Add(MemberRegistrationResult.InvalidPassword, LocalizedStrings.PasswordIsInvalid);
    this._errorMessages.Add(MemberRegistrationResult.IsIpBanned, "IP is banned");
    this._errorMessages.Add(MemberRegistrationResult.MemberNotFound, "I can't find that member. Maybe he's hiding. In any case, you'll have to try again.");
    this._errorMessages.Add(MemberRegistrationResult.OffensiveName, LocalizedStrings.OffensiveNameMsg);
  }

  private void HideKeyboard()
  {
  }

  private void Update()
  {
    if ((double) this._height == (double) this._targetHeight)
      return;
    this._height = Mathf.Lerp(this._height, this._targetHeight, 10f * Time.deltaTime);
    if (!Mathf.Approximately(this._height, this._targetHeight))
      return;
    this._height = this._targetHeight;
  }

  private void SetTargetKeyboardOffset() => this._targetKeyboardOffset = (float) (((double) Screen.height - (double) this._height) * 0.5 - ((double) Screen.height * 0.5 - (double) this._height) * 0.5);

  private void OnGUI()
  {
    this._keyboardOffset = (double) Mathf.Abs(this._keyboardOffset - this._targetKeyboardOffset) <= 2.0 ? this._targetKeyboardOffset : Mathf.Lerp(this._keyboardOffset, this._targetKeyboardOffset, Time.deltaTime * 4f);
    Rect position1 = new Rect((float) (Screen.width - 500) * 0.5f, (float) (((double) Screen.height - (double) this._height) * 0.5) - this._keyboardOffset, 500f, this._height);
    GUI.BeginGroup(position1, GUIContent.none, BlueStonez.window);
    GUI.Label(new Rect(0.0f, 0.0f, position1.width, 56f), LocalizedStrings.Welcome, BlueStonez.tab_strip);
    Rect position2 = new Rect(20f, 55f, position1.width - 40f, position1.height - 78f);
    GUI.Label(position2, GUIContent.none, BlueStonez.window_standard_grey38);
    GUI.BeginGroup(position2);
    GUI.Label(new Rect(0.0f, 0.0f, position2.width, 60f), LocalizedStrings.PleaseProvideValidEmailPasswordMsg, BlueStonez.label_interparkbold_18pt);
    GUI.Label(new Rect(0.0f, 76f, 170f, 11f), LocalizedStrings.Email, BlueStonez.label_interparkbold_11pt_right);
    GUI.Label(new Rect(0.0f, 110f, 170f, 11f), LocalizedStrings.Password, BlueStonez.label_interparkbold_11pt_right);
    GUI.Label(new Rect(0.0f, 147f, 170f, 11f), LocalizedStrings.VerifyPassword, BlueStonez.label_interparkbold_11pt_right);
    GUI.enabled = this._enableGUI;
    GUI.SetNextControlName("@Email");
    this._emailAddress = GUI.TextField(new Rect(180f, 69f, 180f, 22f), this._emailAddress, BlueStonez.textField);
    if (string.IsNullOrEmpty(this._emailAddress) && GUI.GetNameOfFocusedControl() != "@Email")
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(new Rect(188f, 75f, 180f, 22f), LocalizedStrings.EnterYourEmailAddress, BlueStonez.label_interparkmed_11pt_left);
      GUI.color = Color.white;
    }
    GUI.SetNextControlName("@Password1");
    this._password1 = GUI.PasswordField(new Rect(180f, 104f, 180f, 22f), this._password1, '*', BlueStonez.textField);
    if (string.IsNullOrEmpty(this._password1) && GUI.GetNameOfFocusedControl() != "@Password1")
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(new Rect(188f, 110f, 172f, 18f), LocalizedStrings.EnterYourPassword, BlueStonez.label_interparkmed_11pt_left);
      GUI.color = Color.white;
    }
    GUI.SetNextControlName("@Password2");
    this._password2 = GUI.PasswordField(new Rect(180f, 140f, 180f, 22f), this._password2, '*', BlueStonez.textField);
    if (string.IsNullOrEmpty(this._password2) && GUI.GetNameOfFocusedControl() != "@Password2")
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(new Rect(188f, 146f, 180f, 22f), LocalizedStrings.RetypeYourPassword, BlueStonez.label_interparkmed_11pt_left);
      GUI.color = Color.white;
    }
    GUI.enabled = true;
    GUI.contentColor = this._errorMessageColor;
    GUI.Label(new Rect(0.0f, 175f, position2.width, 40f), this._errorMessage, BlueStonez.label_interparkbold_11pt);
    GUI.contentColor = Color.white;
    GUI.EndGroup();
    GUI.Label(new Rect(100f, (float) ((double) position1.height - 42.0 - 22.0), 300f, 16f), "By clicking OK you agree to the", BlueStonez.label_interparkbold_11pt);
    if (GUI.Button(new Rect(205f, (float) ((double) position1.height - 30.0 - 22.0), 90f, 20f), "Terms of Service", BlueStonez.label_interparkbold_11pt))
    {
      ApplicationDataManager.OpenUrl("Terms Of Service", "http://www.cmune.com/index.php/terms-of-service/");
      this.HideKeyboard();
    }
    GUI.Label(new Rect(207f, (float) ((double) position1.height - 15.0 - 22.0), 90f, 20f), GUIContent.none, BlueStonez.horizontal_line_grey95);
    GUI.enabled = this._enableGUI;
    if (GUITools.Button(new Rect(position1.width - 150f, (float) ((double) position1.height - 42.0 - 22.0), 120f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button_green))
    {
      this.HideKeyboard();
      if (!ValidationUtilities.IsValidEmailAddress(this._emailAddress))
      {
        this._targetHeight = 340f;
        this._errorMessageColor = Color.red;
        this._errorMessage = LocalizedStrings.EmailAddressIsInvalid;
      }
      else if (this._password1 != this._password2)
      {
        this._targetHeight = 340f;
        this._errorMessageColor = Color.red;
        this._errorMessage = LocalizedStrings.PasswordDoNotMatch;
      }
      else if (!ValidationUtilities.IsValidPassword(this._password1))
      {
        this._targetHeight = 340f;
        this._errorMessageColor = Color.red;
        this._errorMessage = LocalizedStrings.PasswordInvalidCharsMsg;
      }
      else
      {
        this._enableGUI = false;
        this._targetHeight = 340f;
        this._errorMessageColor = Color.grey;
        this._errorMessage = LocalizedStrings.PleaseWait;
        AuthenticationWebServiceClient.CreateUser(this._emailAddress, this._password1, ApplicationDataManager.Channel, ((Enum) ApplicationDataManager.CurrentLocale).ToString(), SystemInfo.deviceUniqueIdentifier, (Action<MemberRegistrationResult>) (result =>
        {
          if (result == MemberRegistrationResult.Ok)
          {
            this.Hide();
            MonoRoutine.Start(Singleton<AuthenticationManager>.Instance.StartLoginMemberEmail(this._emailAddress, this._password1));
            this._targetHeight = 300f;
            this._errorMessage = string.Empty;
            this._emailAddress = string.Empty;
            this._password1 = string.Empty;
            this._password2 = string.Empty;
            this._errorMessageColor = Color.red;
            this._enableGUI = true;
          }
          else
          {
            this._enableGUI = true;
            this._targetHeight = 340f;
            this._errorMessageColor = Color.red;
            this._errorMessages.TryGetValue(result, out this._errorMessage);
          }
        }), (Action<Exception>) (ex =>
        {
          this._enableGUI = true;
          this._targetHeight = 300f;
          this._errorMessage = string.Empty;
          this.ShowSignUpErrorPopup(LocalizedStrings.Error, "Sign Up was unsuccessful. There was an error communicating with the server.");
        }));
      }
    }
    if (GUITools.Button(new Rect(30f, (float) ((double) position1.height - 42.0 - 22.0), 120f, 32f), new GUIContent(LocalizedStrings.BackCaps), BlueStonez.button))
    {
      this.Hide();
      this.HideKeyboard();
      PanelManager.Instance.OpenPanel(PanelType.Login);
    }
    GUI.enabled = true;
    GUI.EndGroup();
  }

  private void ShowSignUpErrorPopup(string title, string message)
  {
    this.Hide();
    PopupSystem.ShowMessage(title, message, PopupSystem.AlertType.OK, (Action) (() =>
    {
      LoginPanelGUI.ErrorMessage = string.Empty;
      this.Show();
    }));
  }
}
