
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
