// Decompiled with JetBrains decompiler
// Type: ChatGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class ChatGroup
{
  public ChatGroup(UserGroups group, string title, ICollection<CommUser> players)
  {
    this.GroupId = group;
    this.Title = title;
    this.Players = players;
  }

  public bool HasUnreadMessages()
  {
    if (this.Players != null)
    {
      foreach (CommUser player in (IEnumerable<CommUser>) this.Players)
      {
        ChatDialog chatDialog;
        if (Singleton<ChatManager>.Instance._dialogsByCmid.TryGetValue(player.Cmid, out chatDialog) && chatDialog != null && chatDialog.HasUnreadMessage)
          return true;
      }
    }
    return false;
  }

  public UserGroups GroupId { get; private set; }

  public string Title { get; private set; }

  public ICollection<CommUser> Players { get; private set; }
}
