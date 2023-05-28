// Decompiled with JetBrains decompiler
// Type: NameChangePanelGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class NameChangePanelGUI : PanelGuiBase
{
  private const int MAX_CHARACTER_NAME_LENGTH = 18;
  private Rect _groupRect = new Rect(1f, 1f, 1f, 1f);
  private string newName = string.Empty;
  private string oldName = string.Empty;
  private IUnityItem nameChangeItem;
  private bool isChangingName;

  private void OnGUI()
  {
    this._groupRect = new Rect((float) (Screen.width - 340) * 0.5f, (float) (Screen.height - 200) * 0.5f, 340f, 200f);
    GUI.depth = 3;
    GUI.skin = BlueStonez.Skin;
    Rect groupRect = this._groupRect;
    GUI.BeginGroup(groupRect, string.Empty, BlueStonez.window_standard_grey38);
    if (this.nameChangeItem != null)
    {
      GUI.Label(new Rect(8f, 8f, 48f, 48f), (Texture) this.nameChangeItem.Icon, BlueStonez.item_slot_large);
      if ((double) BlueStonez.label_interparkbold_32pt_left.CalcSize(new GUIContent(this.nameChangeItem.ItemView.Name)).x > (double) groupRect.width - 72.0)
        GUI.Label(new Rect(64f, 8f, groupRect.width - 72f, 30f), this.nameChangeItem.ItemView.Name, BlueStonez.label_interparkbold_18pt_left);
      else
        GUI.Label(new Rect(64f, 8f, groupRect.width - 72f, 30f), this.nameChangeItem.ItemView.Name, BlueStonez.label_interparkbold_32pt_left);
    }
    GUI.Label(new Rect(64f, 30f, groupRect.width - 72f, 30f), LocalizedStrings.FunctionalItem, BlueStonez.label_interparkbold_16pt_left);
    Rect rect = new Rect(8f, 116f, this._groupRect.width - 16f, (float) ((double) this._groupRect.height - 120.0 - 46.0));
    GUI.BeginGroup(new Rect(rect.xMin, 74f, rect.width, rect.height + 42f), string.Empty, BlueStonez.group_grey81);
    GUI.EndGroup();
    GUI.Label(new Rect(56f, 72f, 227f, 20f), LocalizedStrings.ChooseCharacterName, BlueStonez.label_interparkbold_11pt);
    GUI.SetNextControlName("@ChooseName");
    Rect position = new Rect(56f, 102f, 227f, 24f);
    GUI.changed = false;
    this.newName = GUI.TextField(position, this.newName, 18, BlueStonez.textField);
    this.newName = TextUtilities.Trim(this.newName);
    if (string.IsNullOrEmpty(this.newName) && GUI.GetNameOfFocusedControl() != "@ChooseName")
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(position, LocalizedStrings.EnterYourName, BlueStonez.label_interparkmed_11pt);
      GUI.color = Color.white;
    }
    if (GUITools.Button(new Rect(groupRect.width - 118f, 160f, 110f, 32f), new GUIContent(LocalizedStrings.CancelCaps), BlueStonez.button))
      this.Hide();
    GUI.enabled = !this.isChangingName;
    if (GUITools.Button(new Rect(groupRect.width - 230f, 160f, 110f, 32f), new GUIContent(LocalizedStrings.OkCaps), BlueStonez.button_green) && !this.newName.Equals(this.oldName) && !string.IsNullOrEmpty(this.newName))
    {
      this.isChangingName = true;
      UserWebServiceClient.ChangeMemberName(Singleton<PlayerDataManager>.Instance.ServerLocalPlayerMemberView.PublicProfile.Cmid, this.newName, ((Enum) ApplicationDataManager.CurrentLocale).ToString(), SystemInfo.deviceUniqueIdentifier, (Action<MemberOperationResult>) (t =>
      {
        MemberOperationResult memberOperationResult = t;
        switch (memberOperationResult)
        {
          case MemberOperationResult.Ok:
            PlayerDataManager.NameSecure = this.newName;
            this.StartCoroutine(Singleton<ItemManager>.Instance.StartGetInventory(false));
            CommConnectionManager.CommCenter.SendUpdatedActorInfo();
            this.Hide();
            break;
          case MemberOperationResult.DuplicateName:
            PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.NameInUseMsg);
            break;
          default:
            switch (memberOperationResult - 13)
            {
              case MemberOperationResult.Ok:
                PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.NameInvalidCharsMsg);
                break;
              case MemberOperationResult.DuplicateName:
                PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.OffensiveNameMsg);
                break;
              default:
                Debug.LogError((object) ("Failed to change name: " + (object) t));
                PopupSystem.ShowMessage(LocalizedStrings.Error, LocalizedStrings.Unknown);
                break;
            }
            break;
        }
        this.isChangingName = false;
      }), (Action<Exception>) (ex =>
      {
        this.isChangingName = false;
        this.Hide();
        DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.");
      }));
    }
    GUI.EndGroup();
    GUI.enabled = true;
    if (this.isChangingName)
      WaitingTexture.Draw(new Vector2(groupRect.x + 305f, groupRect.y + 114f));
    GuiManager.DrawTooltip();
  }

  public override void Show()
  {
    base.Show();
    this.nameChangeItem = Singleton<ItemManager>.Instance.GetItemInShop(1294);
    this.oldName = Singleton<PlayerDataManager>.Instance.ServerLocalPlayerMemberView.PublicProfile.Name;
    this.newName = this.oldName;
  }
}
