// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ContactGroupView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class ContactGroupView
  {
    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public List<PublicProfileView> Contacts { get; set; }

    public ContactGroupView()
    {
      this.Contacts = new List<PublicProfileView>(0);
      this.GroupName = string.Empty;
    }

    public ContactGroupView(int groupID, string groupName, List<PublicProfileView> contacts)
    {
      this.GroupId = groupID;
      this.GroupName = groupName;
      this.Contacts = contacts;
    }

    public override string ToString()
    {
      string str = "[Contact group: [Group ID: " + (object) this.GroupId + "][Group Name :" + this.GroupName + "][Contacts: ";
      foreach (PublicProfileView contact in this.Contacts)
        str = str + "[Contact: " + contact.ToString() + "]";
      return str + "]]";
    }
  }
}
