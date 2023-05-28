// Decompiled with JetBrains decompiler
// Type: ChatDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ChatDialog
{
  public ChatDialog.CanShowMessage CanShow;
  public List<float> _msgHeight;
  public Queue<InstantMessage> _msgQueue;
  private bool _reset;
  private string _title;
  private Vector2 _scroll;
  public Vector2 _frameSize;
  public Vector2 _contentSize;
  public float _heightCache;

  public ChatDialog(string title)
  {
    this.Title = title;
    this.UserName = string.Empty;
    this._msgHeight = new List<float>();
    this._msgQueue = new Queue<InstantMessage>();
    this.AddMessage(new InstantMessage(0, 0, "Disclaimer", "Do not share your password or any other confidential information with anybody. The members of Cmune and the Uberstrike Moderators will never ask you to provide such information.", MemberAccessLevel.Admin));
  }

  public ChatDialog(CommUser user, UserGroups group)
    : this(string.Empty)
  {
    this.Group = group;
    if (user == null)
      return;
    this.UserName = user.ShortName;
    this.UserCmid = user.Cmid;
  }

  public void AddMessage(InstantMessage msg)
  {
    this._reset = true;
    while (this._msgQueue.Count > 200)
      this._msgQueue.Dequeue();
    this._msgQueue.Enqueue(msg);
    if (Input.GetMouseButton(0))
      return;
    this._scroll.y = float.MaxValue;
  }

  public void ScrollToEnd() => this._scroll.y = float.PositiveInfinity;

  public void Clear() => this._msgQueue.Clear();

  public bool CanChat => this.UserCmid == 0 || CommConnectionManager.IsPlayerOnline(this.UserCmid);

  public void CheckSize(Rect rect)
  {
    if (!this._reset && (double) rect.width == (double) this._frameSize.x && (double) rect.height == (double) this._frameSize.y)
      return;
    float num1 = 0.0f;
    this._reset = false;
    this._frameSize.x = rect.width;
    this._frameSize.y = rect.height;
    this._contentSize.x = rect.width;
    this._contentSize.y = rect.height;
    this._msgHeight.Clear();
    foreach (InstantMessage msg in this._msgQueue)
    {
      float num2 = BlueStonez.label_interparkbold_11pt_left_wrap.CalcHeight(new GUIContent(msg.MessageText), this._contentSize.x - 8f) + 24f;
      this._msgHeight.Add(num2);
      num1 += num2;
    }
    if ((double) num1 <= (double) rect.height)
      return;
    float num3 = 0.0f;
    this._msgHeight.Clear();
    this._contentSize.x = rect.width - 17f;
    foreach (InstantMessage msg in this._msgQueue)
    {
      float num4 = BlueStonez.label_interparkbold_11pt_left_wrap.CalcHeight(new GUIContent(msg.MessageText), this._contentSize.x - 8f) + 24f;
      this._msgHeight.Add(num4);
      num3 += num4;
    }
    this._contentSize.y = num3;
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine("Title: " + this.Title);
    stringBuilder.AppendLine("Group: " + (object) this.Group);
    stringBuilder.AppendLine("User: " + this.UserName + " " + (object) this.UserCmid);
    stringBuilder.AppendLine("CanChat: " + (object) this.CanChat);
    return stringBuilder.ToString();
  }

  public string Title
  {
    set => this._title = value;
    get
    {
      if (this.UserCmid <= 0)
        return this._title;
      return CommConnectionManager.IsPlayerOnline(this.UserCmid) ? "Chat with " + this.UserName : this.UserName + " is offline";
    }
  }

  public string UserName { get; private set; }

  public int UserCmid { get; private set; }

  public UserGroups Group { get; set; }

  public bool HasUnreadMessage { get; set; }

  public ICollection<InstantMessage> AllMessages => (ICollection<InstantMessage>) new List<InstantMessage>((IEnumerable<InstantMessage>) this._msgQueue.ToArray());

  public delegate bool CanShowMessage(ChatContext c);
}
