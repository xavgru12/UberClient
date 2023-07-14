// Decompiled with JetBrains decompiler
// Type: DebugFacebook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class DebugFacebook : IDebugPage
{
  private Vector2 scroll;
  private string jsCommand = string.Empty;
  private string jsCommandParam1 = string.Empty;
  private string jsCommandParam2 = string.Empty;
  private string jsCommandLog = "Js Command Log.\n";
  private string button1Param = string.Empty;
  private string button2Param = string.Empty;

  public string Title => "Facebook";

  public void Draw()
  {
    this.scroll = GUILayout.BeginScrollView(this.scroll);
    GUILayout.BeginHorizontal();
    this.FBTestPanel();
    this.JSTestConsole();
    GUILayout.EndHorizontal();
    GUILayout.EndScrollView();
  }

  private void FBTestPanel()
  {
    GUILayout.BeginVertical(GUILayout.MinWidth(256f));
    this.TestFbButton("Invite (usHelper.inviteFriends)", new Action(AutoMonoBehaviour<FacebookInterface>.Instance.OpenInviteFbFriends));
    this.TestFbButton("Get Friends (usHelper.getFriends)", new Action(AutoMonoBehaviour<FacebookInterface>.Instance.GetFbFriends));
    this.TestFbButton("Publish Action (usHelper.publishAction)", (Action) (() => AutoMonoBehaviour<FacebookInterface>.Instance.PublishFbAchievement(AchievementType.SharpestShooter)));
    this.button1Param = this.TestFbButton("Level Up (usHelper.publishLevelUp)", (Action) (() => AutoMonoBehaviour<FacebookInterface>.Instance.PublishFbLevelUp(int.Parse(this.button1Param))), this.button1Param);
    this.button2Param = this.TestFbButton("Publish Score (usHelper.postScore)", (Action) (() => AutoMonoBehaviour<FacebookInterface>.Instance.PublishFbScore(int.Parse(this.button2Param))), this.button2Param);
    GUILayout.EndVertical();
  }

  private void StartTask(LotteryPopupDialog dialog)
  {
    LotteryPopupTask lotteryPopupTask = new LotteryPopupTask(dialog);
    PopupSystem.Show((IPopupDialog) dialog);
  }

  private void JSTestConsole()
  {
    GUILayout.BeginVertical(GUILayout.MinWidth(256f));
    GUILayout.Label("JavaScript Test Console");
    GUILayout.Space(4f);
    this.jsCommand = this.GUILayoutLabelTextField("Function", this.jsCommand);
    GUILayout.Space(4f);
    this.jsCommandParam1 = this.GUILayoutLabelTextField("Param 1", this.jsCommandParam1);
    GUILayout.Space(4f);
    this.jsCommandParam2 = this.GUILayoutLabelTextField("Param 2", this.jsCommandParam2);
    GUILayout.Space(4f);
    if (GUILayout.Button("Execute", BlueStonez.buttondark_medium, GUILayout.MinHeight(24f)))
    {
      if (string.IsNullOrEmpty(this.jsCommand))
        this.jsCommandLog += "Nothing to execute.";
      else if (string.IsNullOrEmpty(this.jsCommandParam1))
      {
        Application.ExternalCall(this.jsCommand);
        DebugFacebook debugFacebook = this;
        debugFacebook.jsCommandLog = debugFacebook.jsCommandLog + this.jsCommand + "\n";
      }
      else if (string.IsNullOrEmpty(this.jsCommandParam2) && !string.IsNullOrEmpty(this.jsCommandParam1))
      {
        Application.ExternalCall(this.jsCommand, (object) this.jsCommandParam1);
        DebugFacebook debugFacebook = this;
        debugFacebook.jsCommandLog = debugFacebook.jsCommandLog + this.jsCommand + "('" + this.jsCommandParam1 + "')\n";
      }
      else if (!string.IsNullOrEmpty(this.jsCommandParam1) && !string.IsNullOrEmpty(this.jsCommandParam2))
      {
        Application.ExternalCall(this.jsCommand, (object) this.jsCommandParam1, (object) this.jsCommandParam2);
        DebugFacebook debugFacebook = this;
        debugFacebook.jsCommandLog = debugFacebook.jsCommandLog + this.jsCommand + "('" + this.jsCommandParam1 + "','" + this.jsCommandParam2 + "')\n";
      }
    }
    GUILayout.Space(4f);
    GUILayout.TextArea(this.jsCommandLog, GUILayout.MinHeight(64f));
    if (GUILayout.Button("Clear", BlueStonez.buttondark_medium, GUILayout.MinHeight(24f)))
      this.jsCommandLog = string.Empty;
    GUILayout.EndVertical();
  }

  private string GUILayoutLabelTextField(string label, string text)
  {
    GUILayout.BeginHorizontal();
    GUILayout.Label(label, GUILayout.Width(64f));
    string str = GUILayout.TextField(text, GUILayout.MinHeight(24f));
    GUILayout.EndHorizontal();
    return str;
  }

  private string TestFbButton(string label, Action action, string paramOne = null)
  {
    string str = string.Empty;
    GUILayout.BeginHorizontal();
    if (GUILayout.Button(label, BlueStonez.buttondark_medium, GUILayout.MinHeight(24f)) && action != null)
    {
      DebugFacebook debugFacebook = this;
      debugFacebook.jsCommandLog = debugFacebook.jsCommandLog + "Executed: " + label + "\n";
      action();
    }
    if (paramOne != null)
      str = GUILayout.TextField(paramOne, GUILayout.MinHeight(24f), GUILayout.Width(64f));
    GUILayout.EndHorizontal();
    GUILayout.Space(4f);
    return str;
  }
}
