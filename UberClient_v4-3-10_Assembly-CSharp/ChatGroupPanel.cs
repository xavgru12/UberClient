// Decompiled with JetBrains decompiler
// Type: ChatGroupPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ChatGroupPanel
{
  private readonly List<ChatGroup> _groups;

  public ChatGroupPanel()
  {
    this.SearchText = string.Empty;
    this._groups = new List<ChatGroup>();
  }

  public void AddGroup(UserGroups group, string name, ICollection<CommUser> users) => this._groups.Add(new ChatGroup(group, name, users));

  public Vector2 Scroll { get; set; }

  public string SearchText { get; set; }

  public float ContentHeight { get; set; }

  public IEnumerable<ChatGroup> Groups => (IEnumerable<ChatGroup>) this._groups;
}
