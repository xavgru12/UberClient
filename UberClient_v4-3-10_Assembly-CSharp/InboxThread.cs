// Decompiled with JetBrains decompiler
// Type: InboxThread
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class InboxThread
{
  public const int AdminCmid = 767;
  public const int NameWidth = 100;
  public const int ThreadHeight = 76;
  public Vector2 Scroll;
  private bool _messagesLoaded;
  private MessageThreadView _threadView;
  private SortedList<int, InboxMessage> _messages;
  private int _curPageIndex;

  public InboxThread(MessageThreadView threadView)
  {
    this._threadView = threadView;
    this._messages = new SortedList<int, InboxMessage>(threadView.MessageCount, (IComparer<int>) new InboxThread.MessageSorter());
    this.LastServerUpdate = this._threadView.LastUpdate;
  }

  public static InboxThread Current { get; set; }

  public bool IsLoading { get; set; }

  public DateTime LastServerUpdate { get; private set; }

  public int ThreadId => this._threadView.ThreadId;

  public string Name => this._threadView.ThreadName;

  public DateTime LastMessageDateTime => this._threadView.LastUpdate;

  public IEnumerable<InboxMessage> Messages => (IEnumerable<InboxMessage>) this._messages.Values;

  public bool HasUnreadMessage => this._threadView.HasNewMessages;

  private string Date => this._threadView.LastUpdate.ToString("yyyy MMM ") + " " + this._threadView.LastUpdate.Day.ToString() + " at " + this._threadView.LastUpdate.ToShortTimeString();

  public bool IsAdmin => this.ThreadId == 767;

  public bool Contains(string keyword)
  {
    bool flag = false;
    string lower = keyword.ToLower();
    if (this._threadView.ThreadName.ToLower().Contains(lower))
      return true;
    foreach (InboxMessage inboxMessage in (IEnumerable<InboxMessage>) this._messages.Values)
    {
      if (inboxMessage.Content.ToLower().Contains(lower))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public int DrawThread(int y, int width)
  {
    Rect position = new Rect(8f, (float) (y + 8), (float) (width - 8), 68f);
    if (InboxThread.Current == this)
      GUI.Box(new Rect(4f, (float) (y + 4), (float) width, 76f), GUIContent.none, BlueStonez.box_grey50);
    GUI.BeginGroup(position);
    GUI.Label(new Rect(0.0f, 0.0f, (float) width, 18f), string.Format("{0} ({1})", (object) this._threadView.ThreadName, (object) this._threadView.MessageCount), BlueStonez.label_interparkbold_13pt);
    GUI.color = new Color(1f, 1f, 1f, 0.5f);
    GUI.Label(new Rect(0.0f, 20f, (float) width, 10f), this.Date, BlueStonez.label_interparkmed_10pt_left);
    GUI.color = Color.white;
    GUI.Label(new Rect(0.0f, 50f, (float) width, 18f), this._threadView.LastMessagePreview, BlueStonez.label_interparkmed_10pt_left);
    GUI.EndGroup();
    Rect rect = new Rect((float) (width - 18), (float) (y + 9), 16f, 16f);
    if (GUI.enabled && position.Contains(Event.current.mousePosition))
    {
      GUI.Box(new Rect(4f, (float) (y + 4), (float) width, 76f), GUIContent.none, BlueStonez.group_grey81);
      if (Event.current.type == UnityEngine.EventType.MouseDown && Event.current.button == 0 && !rect.Contains(Event.current.mousePosition))
      {
        InboxThread.Current = this;
        this.Scroll.y = float.MinValue;
        if (!this._messagesLoaded)
        {
          this._messagesLoaded = true;
          Singleton<InboxManager>.Instance.LoadMessagesForThread(this, 0);
        }
        if (this._threadView.HasNewMessages)
        {
          this._threadView.HasNewMessages = false;
          Singleton<InboxManager>.Instance.MarkThreadAsRead(this._threadView.ThreadId);
        }
        Event.current.Use();
      }
    }
    if (this._threadView.HasNewMessages)
      GUI.Label(new Rect((float) (width - 40), (float) (y + 5), 29f, 29f), (Texture) CommunicatorIcons.NewInboxMessage);
    return y + 76 + 8;
  }

  public int DrawMessageList(int y, int scrollRectWidth, float scrollRectHeight, float curScrollY)
  {
    for (int index = this._messages.Values.Count - 1; index >= 0; --index)
    {
      InboxMessage msg = this._messages.Values[index];
      y += this.DrawContent(msg, y + 12, scrollRectWidth) + 16;
    }
    if (this._messages.Count == 0)
    {
      GUI.Label(new Rect(0.0f, (float) y, (float) scrollRectWidth, 100f), "This thread is empty", BlueStonez.label_interparkbold_13pt);
    }
    else
    {
      float max = (float) y - scrollRectHeight;
      float num = Mathf.Clamp(max, 0.0f, max);
      if ((double) curScrollY >= (double) num && this._threadView.MessageCount > this._messages.Count && !this.IsLoading)
      {
        ++this._curPageIndex;
        Singleton<InboxManager>.Instance.LoadMessagesForThread(this, this._curPageIndex);
      }
    }
    if (this.IsLoading)
    {
      GUI.Label(new Rect(0.0f, (float) y, (float) scrollRectWidth, 30f), "Loading messages...", BlueStonez.label_interparkbold_13pt);
      y += 30;
    }
    return y;
  }

  public int DrawContent(InboxMessage msg, int y, int width) => msg.IsMine ? this.DrawMyMessage(msg, 100, y, width - 100) : this.DrawOtherMessage(msg, 0, y, width - 100);

  private int DrawOtherMessage(InboxMessage msg, int x, int y, int width)
  {
    int height = Mathf.RoundToInt(BlueStonez.speechbubble_left.CalcHeight(new GUIContent(msg.Content), (float) width)) + 30;
    Rect position = new Rect((float) x, (float) y, (float) width, (float) height);
    GUI.color = new Color(0.5f, 0.5f, 0.5f);
    int x1 = (int) BlueStonez.label_interparkbold_11pt_left.CalcSize(new GUIContent(msg.SenderName)).x;
    int x2 = (int) BlueStonez.label_interparkmed_10pt_left.CalcSize(new GUIContent(msg.SentDateString)).x;
    GUI.Label(new Rect(position.x + 28f, position.y - 16f, position.width, 12f), msg.SenderName, BlueStonez.label_interparkbold_11pt_left);
    GUI.Label(new Rect((float) ((double) position.x + (double) x1 + 34.0), position.y - 15f, position.width, 12f), msg.SentDateString, BlueStonez.label_interparkmed_10pt_left);
    GUI.color = Color.white;
    GUI.BeginGroup(position);
    GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);
    if (ApplicationDataManager.IsMobile)
      GUI.Label(new Rect(0.0f, 0.0f, position.width, (float) height), msg.Content, BlueStonez.speechbubble_left);
    else
      GUI.TextArea(new Rect(0.0f, 0.0f, position.width, (float) height), msg.Content, BlueStonez.speechbubble_left);
    GUI.backgroundColor = Color.white;
    GUI.EndGroup();
    return height;
  }

  private int DrawMyMessage(InboxMessage msg, int x, int y, int width)
  {
    int height = Mathf.RoundToInt(BlueStonez.speechbubble_right.CalcHeight(new GUIContent(msg.Content), (float) width)) + 30;
    Rect position = new Rect((float) x, (float) y, (float) width, (float) height);
    GUI.color = new Color(0.5f, 0.5f, 0.5f);
    int x1 = (int) BlueStonez.label_interparkbold_11pt_left.CalcSize(new GUIContent(msg.SenderName)).x;
    int x2 = (int) BlueStonez.label_interparkmed_10pt_left.CalcSize(new GUIContent(msg.SentDateString)).x;
    GUI.Label(new Rect(position.x + position.width - (float) (x2 + x1 + 40), position.y - 16f, (float) (x1 + 2), 12f), msg.SenderName, BlueStonez.label_interparkbold_11pt_left);
    GUI.Label(new Rect(position.x + position.width - (float) (x2 + 32), position.y - 15f, (float) (x2 + 2), 12f), msg.SentDateString, BlueStonez.label_interparkmed_10pt_left);
    GUI.color = Color.white;
    GUI.BeginGroup(position);
    GUI.backgroundColor = new Color(0.376f, 0.631f, 0.886f, 0.5f);
    if (ApplicationDataManager.IsMobile)
      GUI.Label(new Rect(position.width - position.width, 0.0f, position.width, (float) height), msg.Content, BlueStonez.speechbubble_right);
    else
      GUI.TextArea(new Rect(position.width - position.width, 0.0f, position.width, (float) height), msg.Content, BlueStonez.speechbubble_right);
    GUI.backgroundColor = Color.white;
    GUI.EndGroup();
    return height;
  }

  internal void UpdateThread(MessageThreadView newThreadView)
  {
    if (newThreadView.MessageCount != this._threadView.MessageCount)
      this._messagesLoaded = false;
    this._threadView = newThreadView;
    this.LastServerUpdate = this._threadView.LastUpdate;
  }

  internal void AddMessage(PrivateMessageView message)
  {
    if (!this._messages.ContainsKey(message.PrivateMessageId))
    {
      this._messages.Add(message.PrivateMessageId, new InboxMessage(message, message.FromCmid != PlayerDataManager.Cmid ? this._threadView.ThreadName : PlayerDataManager.Name));
      ++this._threadView.MessageCount;
      if (!message.IsRead && message.ToCmid == PlayerDataManager.Cmid)
        this._threadView.HasNewMessages = true;
      if (message.DateSent > this._threadView.LastUpdate)
      {
        this._threadView.LastUpdate = message.DateSent;
        this._threadView.LastMessagePreview = TextUtilities.ShortenText(message.ContentText, 25, true);
      }
    }
    this.Scroll.y = float.MinValue;
  }

  internal void AddMessages(List<PrivateMessageView> messages)
  {
    foreach (PrivateMessageView message in messages)
    {
      if (!this._messages.ContainsKey(message.PrivateMessageId))
        this._messages.Add(message.PrivateMessageId, new InboxMessage(message, message.FromCmid != PlayerDataManager.Cmid ? this._threadView.ThreadName : PlayerDataManager.Name));
    }
  }

  private class MessageSorter : IComparer<int>
  {
    public int Compare(int obj1, int obj2) => obj1 - obj2;
  }
}
