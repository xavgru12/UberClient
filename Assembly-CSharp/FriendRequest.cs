// Decompiled with JetBrains decompiler
// Type: FriendRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UnityEngine;

public class FriendRequest
{
  public const int PanelHeight = 50;
  private Action<FriendRequest> _onAccept;
  private Action<FriendRequest> _onIgnore;
  private FriendRequest.CanRespond _canRespond;
  private ContactRequestView _request;

  public FriendRequest(
    ContactRequestView request,
    Action<FriendRequest> onAccept,
    Action<FriendRequest> onIgnore,
    FriendRequest.CanRespond canRespond)
  {
    this._request = request;
    this._onAccept = onAccept;
    this._onIgnore = onIgnore;
    this._canRespond = canRespond;
  }

  public void Draw(int y, int width)
  {
    Rect position1 = new Rect(4f, (float) (y + 4), (float) (width - 1), 50f);
    GUI.BeginGroup(position1);
    Rect position2 = new Rect(0.0f, 0.0f, position1.width, position1.height - 1f);
    if (GUI.enabled && position2.Contains(Event.current.mousePosition))
      GUI.Box(position2, GUIContent.none, BlueStonez.box_grey50);
    GUI.Label(new Rect(120f, 5f, 200f, 20f), string.Format("{0}: {1}", (object) LocalizedStrings.FriendRequest, (object) this.Name), BlueStonez.label_interparkbold_13pt_left);
    GUI.Label(new Rect(120f, 30f, 400f, 20f), this.Message, BlueStonez.label_interparkmed_11pt_left);
    bool enabled = GUI.enabled;
    GUI.enabled = enabled && this._canRespond();
    if (GUITools.Button(new Rect((float) ((double) position1.width - 120.0 - 18.0), 5f, 60f, 20f), new GUIContent(LocalizedStrings.Accept), BlueStonez.buttondark_medium) && this._onAccept != null)
      this._onAccept(this);
    if (GUITools.Button(new Rect((float) ((double) position1.width - 50.0 - 18.0), 5f, 60f, 20f), new GUIContent(LocalizedStrings.Ignore), BlueStonez.buttondark_medium) && this._onIgnore != null)
      this._onIgnore(this);
    GUI.enabled = enabled;
    GUI.EndGroup();
    GUI.Label(new Rect(4f, (float) (y + 50 + 8), (float) width, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
  }

  public string Name => this._request.InitiatorName;

  public string Message => this._request.InitiatorMessage;

  public int RequestId => this._request.RequestId;

  public int Cmid => this._request.InitiatorCmid;

  public delegate bool CanRespond();
}
