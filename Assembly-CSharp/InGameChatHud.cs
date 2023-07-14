// Decompiled with JetBrains decompiler
// Type: InGameChatHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class InGameChatHud : Singleton<InGameChatHud>
{
  private const int _maxMessageLength = 140;
  private const int InputHeight = 30;
  private bool _isEnabled;
  private bool _canInput;
  private Rect MsgPosition = new Rect(10f, 160f, 300f, 360f);
  private float MsgLifespan = 10f;
  private float MsgFadeSpeed = 1f;
  private string _inputContent = string.Empty;
  private string _muteMessage = "You are not allowed to chat!";
  private string _spamMessage = "Don't spam!";
  private GUIStyle _msgStyleCache;
  private bool _paused;
  private bool _doFocusOnChat;
  private List<InGameChatHud.ChatMessage> _chatMsgs;
  private float _chatTimer;
  private float _muteTimer;
  private float _spamTimer;
  private GUIStyle _textFieldStyle = StormFront.InGameChatBlue;

  private InGameChatHud()
  {
    this._chatMsgs = new List<InGameChatHud.ChatMessage>(10);
    this.ClearAll();
    CmuneEventHandler.AddListener<OnSetPlayerTeamEvent>(new Action<OnSetPlayerTeamEvent>(this.OnTeamChange));
  }

  public bool Enabled
  {
    get => this._isEnabled;
    set => this._isEnabled = value;
  }

  public bool CanInput => this._canInput;

  public bool CanStartChat() => (double) this._spamTimer <= 0.0 && (double) this._chatTimer <= 0.0 && !ClientCommCenter.IsPlayerMuted;

  public void OpenChat()
  {
  }

  public void PushMessage(string input)
  {
    this._inputContent = input;
    this.EndChat();
  }

  public void Update()
  {
    if (!PopupSystem.IsAnyPopupOpen && !AutoMonoBehaviour<InputManager>.Instance.IsAnyDown && (double) this._spamTimer <= 0.0 && ((double) this._chatTimer <= 0.0 || this._canInput) && Input.GetKeyDown(KeyCode.Return))
    {
      if (ClientCommCenter.IsPlayerMuted)
      {
        this._muteTimer = 2f;
      }
      else
      {
        this._canInput = !this._canInput;
        Input.ResetInputAxes();
        if (this._canInput)
          this.BeginChat();
        else
          this.EndChat();
      }
    }
    for (int index = 0; index < this._chatMsgs.Count; ++index)
    {
      this._chatMsgs[index].Timer -= Time.deltaTime * this.MsgFadeSpeed;
      if ((double) this._chatMsgs[index].Timer < 0.0)
        this._chatMsgs.RemoveAt(index);
    }
    this.MsgPosition.height = (float) ((double) Screen.height - (double) this.MsgPosition.y - (double) Screen.height / 9.0);
    if ((double) this._chatTimer > 0.0)
      this._chatTimer -= Time.deltaTime;
    if ((double) this._spamTimer <= 0.0)
      return;
    this._spamTimer -= Time.deltaTime;
  }

  public void Draw()
  {
    GUI.depth = 9;
    if (TabScreenPanelGUI.Enabled)
      return;
    GUI.BeginGroup(this.MsgPosition);
    this.DoChatMessages();
    if (ClientCommCenter.IsPlayerMuted)
      this.DoMuteMessage();
    else if ((double) this._spamTimer > 0.0)
      this.DoSpamMessage();
    else if (this._canInput)
      this.DoChatInput();
    GUI.EndGroup();
    if (!this._doFocusOnChat)
      return;
    GUI.FocusControl("input");
    this._doFocusOnChat = false;
  }

  private void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    switch (ev.TeamId)
    {
      case TeamID.NONE:
      case TeamID.BLUE:
        this._textFieldStyle = StormFront.InGameChatBlue;
        break;
      case TeamID.RED:
        this._textFieldStyle = StormFront.InGameChatRed;
        break;
    }
  }

  private void DoChatMessages()
  {
    this.MsgStyle.wordWrap = true;
    for (int index = 0; index < this._chatMsgs.Count; ++index)
    {
      InGameChatHud.ChatMessage chatMsg = this._chatMsgs[index];
      string text1 = chatMsg.Sender + ": ";
      int height = Mathf.CeilToInt(this.MsgStyle.CalcHeight(new GUIContent(text1), this.MsgPosition.width));
      string text2 = text1 + chatMsg.Content;
      float num = Mathf.Clamp01(chatMsg.Timer);
      GUI.color = chatMsg.Color.SetAlpha(num);
      GUI.Label(new Rect(chatMsg.Position.x + 1f, chatMsg.Position.y + 1f, chatMsg.Position.width, chatMsg.Position.height), text2, this.MsgStyle);
      GUI.color = new Color(1f, 1f, 1f, num);
      GUI.Label(chatMsg.Position, text2, this.MsgStyle);
      GUI.color = new Color(0.0f, 0.0f, 0.0f, num * 0.4f);
      GUI.Label(new Rect(chatMsg.Position.x, chatMsg.Position.y, chatMsg.Position.width, (float) height), text1, this.MsgStyle);
    }
    this.MsgStyle.wordWrap = false;
  }

  private void DoChatInput()
  {
    Rect position = new Rect(44f, this.MsgPosition.height - 30f, this.MsgPosition.width - 44f, 30f);
    GUI.color = Color.white;
    GUI.SetNextControlName("input");
    this._inputContent = GUI.TextField(position, this._inputContent, 140, this._textFieldStyle);
    this._inputContent = this._inputContent.Trim('\n', '\t');
    GUI.color = Color.black;
    GUI.Label(new Rect(1f, (float) ((double) this.MsgPosition.height - 30.0 + 1.0), this.MsgPosition.width, 30f), LocalizedStrings.Chat + ":", this.MsgStyle);
    GUI.color = Color.white;
    GUI.Label(new Rect(0.0f, this.MsgPosition.height - 30f, this.MsgPosition.width, 30f), LocalizedStrings.Chat + ":", this.MsgStyle);
    if (Event.current.keyCode == KeyCode.Return && (Event.current.type == UnityEngine.EventType.KeyDown || Event.current.type == UnityEngine.EventType.KeyUp))
    {
      if ((double) this._chatTimer <= 0.0)
      {
        this._canInput = false;
        GUIUtility.keyboardControl = 0;
        Event.current.Use();
        this.EndChat();
      }
      else
        Event.current.Use();
    }
    if (Event.current.keyCode != KeyCode.Escape)
      return;
    this._inputContent = string.Empty;
    this._canInput = false;
    GUIUtility.keyboardControl = 0;
    Event.current.Use();
    this.EndChat();
  }

  private void DoMuteMessage()
  {
    if ((double) this._muteTimer <= 0.0)
      return;
    GUI.color = Color.red;
    GUI.Label(new Rect(1f, (float) ((double) this.MsgPosition.height - 30.0 + 1.0), this.MsgPosition.width, 30f), this._muteMessage, this.MsgStyle);
    GUI.color = Color.white;
    GUI.Label(new Rect(0.0f, this.MsgPosition.height - 30f, this.MsgPosition.width, 30f), this._muteMessage, this.MsgStyle);
    this._muteTimer -= Time.deltaTime;
  }

  private void DoSpamMessage()
  {
    GUI.color = Color.red;
    GUI.Label(new Rect(1f, (float) ((double) this.MsgPosition.height - 30.0 + 1.0), this.MsgPosition.width, 30f), this._spamMessage, this.MsgStyle);
    GUI.color = Color.white;
    GUI.Label(new Rect(0.0f, this.MsgPosition.height - 30f, this.MsgPosition.width, 30f), this._spamMessage, this.MsgStyle);
  }

  public void AddChatMessage(string sender, string message, MemberAccessLevel accessLevel)
  {
    this.MsgStyle.wordWrap = true;
    int height = Mathf.CeilToInt(this.MsgStyle.CalcHeight(new GUIContent(sender + ": " + message), this.MsgPosition.width));
    InGameChatHud.ChatMessage chatMessage1 = new InGameChatHud.ChatMessage();
    chatMessage1.Sender = sender;
    chatMessage1.Content = message;
    chatMessage1.Timer = this.MsgLifespan;
    chatMessage1.Height = height;
    chatMessage1.Position = new Rect(0.0f, 0.0f, this.MsgPosition.width, (float) height);
    InGameChatHud.ChatMessage chatMessage2 = chatMessage1;
    Color color;
    switch (accessLevel)
    {
      case MemberAccessLevel.Default:
        color = Color.black;
        break;
      case MemberAccessLevel.Admin:
        color = ColorScheme.ChatNameAdminUser;
        break;
      default:
        color = ColorScheme.ChatNameModeratorUser;
        break;
    }
    chatMessage2.Color = color;
    this._chatMsgs.Insert(0, chatMessage1);
    this.UpdateMessagePosition();
  }

  public void ClearHistory() => this._chatMsgs.Clear();

  public void SendChatMessage()
  {
    string message = TextUtilities.Trim(this._inputContent);
    if (message.Length == 0)
      return;
    if (!ClientCommCenter.IsPlayerMuted && !CommConnectionManager.CommCenter.SendInGameChatMessage(message, ChatContext.Player))
      this._spamTimer = 5f;
    this._inputContent = string.Empty;
  }

  public void ClearAll()
  {
    this._canInput = false;
    this._inputContent = string.Empty;
    this._chatMsgs.Clear();
  }

  public void Pause()
  {
    if (!this._canInput)
      return;
    this._paused = true;
  }

  public void OnFullScreen() => this.UpdateMessagePosition();

  private void BeginChat()
  {
    this._doFocusOnChat = true;
    this._chatTimer = 0.5f;
    AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = false;
  }

  private void EndChat()
  {
    this.SendChatMessage();
    this._chatTimer = 0.3f;
    if (this._paused)
    {
      this._paused = false;
      if (GameState.HasCurrentGame && GameState.CurrentGame.IsMatchRunning)
        GameState.LocalPlayer.UnPausePlayer();
    }
    if (!GameState.LocalCharacter.IsAlive)
      Screen.lockCursor = false;
    if (!GameState.HasCurrentGame || !GameState.CurrentGame.IsMatchRunning || GameState.LocalPlayer.IsGamePaused)
      return;
    AutoMonoBehaviour<InputManager>.Instance.IsInputEnabled = true;
  }

  private void UpdateMessagePosition()
  {
    int num = 0;
    int index1 = 0;
    bool flag = false;
    for (int index2 = 0; index2 < this._chatMsgs.Count; ++index2)
    {
      InGameChatHud.ChatMessage chatMsg = this._chatMsgs[index2];
      chatMsg.Position.y = this.MsgPosition.height - 30f - (float) chatMsg.Height - (float) num;
      num += chatMsg.Height;
      if ((double) chatMsg.Position.y < 0.0)
        break;
      if (flag)
        this._chatMsgs.RemoveRange(index1, this._chatMsgs.Count - index1);
    }
  }

  private GUIStyle MsgStyle
  {
    get
    {
      if (this._msgStyleCache == null)
        this._msgStyleCache = BlueStonez.label_ingamechat;
      return this._msgStyleCache;
    }
  }

  private class ChatMessage
  {
    public string Sender;
    public string Content;
    public float Timer;
    public int Height;
    public Rect Position;
    public Color Color;
  }

  private enum State
  {
    Normal,
    Ghost,
    Mute,
  }
}
