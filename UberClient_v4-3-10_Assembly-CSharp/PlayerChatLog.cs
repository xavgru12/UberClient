// Decompiled with JetBrains decompiler
// Type: PlayerChatLog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;

public static class PlayerChatLog
{
  private static Dictionary<int, PlayerChatLog.ChatParticipant> _participants = new Dictionary<int, PlayerChatLog.ChatParticipant>();
  private static Dictionary<int, PlayerChatLog.ChatContext> _privateContexts = new Dictionary<int, PlayerChatLog.ChatContext>();
  private static PlayerChatLog.ChatContext _ingameContext = new PlayerChatLog.ChatContext("InGame", PlayerChatLog.ChatContextType.Game, 100);
  private static PlayerChatLog.ChatContext _lobbyContext = new PlayerChatLog.ChatContext("Lobby", PlayerChatLog.ChatContextType.Lobby, 100);

  public static void AddMessage(string message, PlayerChatLog.ChatContextType type)
  {
    PlayerChatLog.ChatContext chatContext = (PlayerChatLog.ChatContext) null;
    switch (type)
    {
      case PlayerChatLog.ChatContextType.Lobby:
        chatContext = PlayerChatLog._lobbyContext;
        break;
      case PlayerChatLog.ChatContextType.Game:
        chatContext = PlayerChatLog._ingameContext;
        break;
    }
    chatContext?.Add(string.Format("{0}: {3}", (object) DateTime.Now, (object) message));
  }

  public static void AddIncomingMessage(
    int senderCmid,
    string senderName,
    string message,
    PlayerChatLog.ChatContextType type)
  {
    if (!PlayerChatLog._participants.ContainsKey(senderCmid))
      PlayerChatLog._participants.Add(senderCmid, new PlayerChatLog.ChatParticipant(senderCmid, senderName));
    PlayerChatLog.ChatContext chatContext = (PlayerChatLog.ChatContext) null;
    switch (type)
    {
      case PlayerChatLog.ChatContextType.Lobby:
        chatContext = PlayerChatLog._lobbyContext;
        break;
      case PlayerChatLog.ChatContextType.Game:
        chatContext = PlayerChatLog._ingameContext;
        break;
      case PlayerChatLog.ChatContextType.Private:
        if (!PlayerChatLog._privateContexts.TryGetValue(senderCmid, out chatContext))
        {
          chatContext = new PlayerChatLog.ChatContext("Private Chat with " + senderName, PlayerChatLog.ChatContextType.Private, 30);
          PlayerChatLog._privateContexts.Add(senderCmid, chatContext);
          break;
        }
        break;
    }
    string str = string.Empty;
    if (PlayerDataManager.Cmid == senderCmid)
      str = "*** ";
    chatContext?.Add(string.Format("{0}{1} ({2}) \"{3}\": {4}", (object) str, (object) DateTime.Now, (object) senderCmid, (object) senderName, (object) message));
  }

  public static void AddOutgoingPrivateMessage(
    int recieverCmid,
    string recieverName,
    string message,
    PlayerChatLog.ChatContextType type)
  {
    if (!PlayerChatLog._participants.ContainsKey(recieverCmid))
      PlayerChatLog._participants.Add(recieverCmid, new PlayerChatLog.ChatParticipant(recieverCmid, recieverName));
    PlayerChatLog.ChatContext chatContext = (PlayerChatLog.ChatContext) null;
    switch (type)
    {
      case PlayerChatLog.ChatContextType.Lobby:
        chatContext = PlayerChatLog._lobbyContext;
        break;
      case PlayerChatLog.ChatContextType.Game:
        chatContext = PlayerChatLog._ingameContext;
        break;
      case PlayerChatLog.ChatContextType.Private:
        if (!PlayerChatLog._privateContexts.TryGetValue(recieverCmid, out chatContext))
        {
          chatContext = new PlayerChatLog.ChatContext("Private Chat with " + recieverName, PlayerChatLog.ChatContextType.Private, 30);
          PlayerChatLog._privateContexts.Add(recieverCmid, chatContext);
          break;
        }
        break;
    }
    chatContext?.Add(string.Format("*** {0} ({1}) \"{2}\": {3}", (object) DateTime.Now, (object) PlayerDataManager.CmidSecure, (object) PlayerDataManager.NameSecure, (object) message));
  }

  public static int ParticipantsCount => PlayerChatLog._participants.Count;

  public static ICollection<PlayerChatLog.ChatParticipant> AllParticipants => (ICollection<PlayerChatLog.ChatParticipant>) PlayerChatLog._participants.Values;

  public static string DumpLogs()
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (PlayerChatLog._lobbyContext != null)
      stringBuilder.AppendLine(PlayerChatLog._lobbyContext.ToString());
    if (PlayerChatLog._ingameContext != null)
      stringBuilder.AppendLine(PlayerChatLog._ingameContext.ToString());
    foreach (PlayerChatLog.ChatContext chatContext in PlayerChatLog._privateContexts.Values)
      stringBuilder.AppendLine(chatContext.ToString());
    return stringBuilder.ToString();
  }

  public enum ChatContextType
  {
    Lobby,
    Game,
    Private,
    Clan,
  }

  private class ChatContext
  {
    public int MaxMessagesCount = 100;
    public string Title = string.Empty;
    public PlayerChatLog.ChatContextType Type;
    public Queue<string> Messages;

    public ChatContext(string title, PlayerChatLog.ChatContextType type, int messageCap)
    {
      this.Title = title;
      this.Type = type;
      this.Messages = new Queue<string>(messageCap);
      this.MaxMessagesCount = messageCap;
    }

    public void Add(string message)
    {
      this.Messages.Enqueue(message);
      while (this.Messages.Count > this.MaxMessagesCount)
        this.Messages.Dequeue();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(this.Title);
      foreach (string message in this.Messages)
        stringBuilder.AppendLine(message);
      return stringBuilder.ToString();
    }
  }

  public struct ChatParticipant
  {
    public int Cmid;
    public string Name;

    public ChatParticipant(int cmid, string name)
    {
      this.Cmid = cmid;
      this.Name = name;
    }
  }
}
