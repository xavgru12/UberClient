// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.ContactGroupView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
      string str = "[Contact group: [Group ID: " + this.GroupId.ToString() + "][Group Name :" + this.GroupName + "][Contacts: ";
      foreach (PublicProfileView contact in this.Contacts)
        str = str + "[Contact: " + contact.ToString() + "]";
      return str + "]]";
    }
  }
}
