// Decompiled with JetBrains decompiler
// Type: InstantMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;

public class InstantMessage
{
  public InstantMessage(
    int cmid,
    int actorID,
    string playerName,
    string messageText,
    MemberAccessLevel level)
    : this(cmid, actorID, playerName, messageText, level, ChatContext.None)
  {
  }

  public InstantMessage(
    int cmid,
    int actorID,
    string playerName,
    string messageText,
    MemberAccessLevel level,
    ChatContext context)
  {
    this.Cmid = cmid;
    this.ActorID = actorID;
    this.PlayerName = playerName;
    this.MessageText = messageText;
    this.AccessLevel = level;
    this.MessageDateTime = DateTime.Now.ToString("t");
    this.Context = context;
    this.IsFriend = PlayerDataManager.IsFriend(this.Cmid);
    this.IsClan = PlayerDataManager.IsClanMember(this.Cmid);
  }

  public InstantMessage(InstantMessage instantMessage)
  {
    this.Cmid = instantMessage.Cmid;
    this.ActorID = instantMessage.ActorID;
    this.PlayerName = instantMessage.PlayerName;
    this.MessageText = instantMessage.MessageText;
    this.MessageDateTime = instantMessage.MessageDateTime;
    this.AccessLevel = instantMessage.AccessLevel;
    this.Context = instantMessage.Context;
    this.IsFriend = PlayerDataManager.IsFriend(this.Cmid);
    this.IsClan = PlayerDataManager.IsClanMember(this.Cmid);
  }

  public int Cmid { get; private set; }

  public int ActorID { get; private set; }

  public string PlayerName { get; private set; }

  public string MessageText { get; private set; }

  public string MessageDateTime { get; private set; }

  public MemberAccessLevel AccessLevel { get; private set; }

  public bool IsFriend { get; private set; }

  public bool IsClan { get; private set; }

  public ChatContext Context { get; private set; }
}
