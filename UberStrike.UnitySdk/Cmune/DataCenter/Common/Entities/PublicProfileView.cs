
using System;

namespace Cmune.DataCenter.Common.Entities
{
  [Serializable]
  public class PublicProfileView
  {
    public int Cmid { get; set; }

    public string Name { get; set; }

    public bool IsChatDisabled { get; set; }

    public MemberAccessLevel AccessLevel { get; set; }

    public string GroupTag { get; set; }

    public DateTime LastLoginDate { get; set; }

    public EmailAddressStatus EmailAddressStatus { get; set; }

    public PublicProfileView()
    {
      this.Cmid = 0;
      this.Name = string.Empty;
      this.IsChatDisabled = false;
      this.AccessLevel = MemberAccessLevel.Default;
      this.GroupTag = string.Empty;
      this.LastLoginDate = DateTime.Now;
      this.EmailAddressStatus = EmailAddressStatus.Unverified;
    }

    public PublicProfileView(
      int cmid,
      string name,
      MemberAccessLevel accesLevel,
      bool isChatDisabled,
      DateTime lastLoginDate,
      EmailAddressStatus emailAddressStatus)
    {
      this.SetPublicProfile(cmid, name, accesLevel, isChatDisabled, string.Empty, lastLoginDate, emailAddressStatus);
    }

    public PublicProfileView(
      int cmid,
      string name,
      MemberAccessLevel accesLevel,
      bool isChatDisabled,
      string groupTag,
      DateTime lastLoginDate,
      EmailAddressStatus emailAddressStatus)
    {
      this.SetPublicProfile(cmid, name, accesLevel, isChatDisabled, groupTag, lastLoginDate, emailAddressStatus);
    }

    private void SetPublicProfile(
      int cmid,
      string name,
      MemberAccessLevel accesLevel,
      bool isChatDisabled,
      string groupTag,
      DateTime lastLoginDate,
      EmailAddressStatus emailAddressStatus)
    {
      this.Cmid = cmid;
      this.Name = name;
      this.AccessLevel = accesLevel;
      this.IsChatDisabled = isChatDisabled;
      this.GroupTag = groupTag;
      this.LastLoginDate = lastLoginDate;
      this.EmailAddressStatus = emailAddressStatus;
    }

    public override string ToString() => "[Public profile: " + "[Member name:" + this.Name + "][CMID:" + (object) this.Cmid + "][Is banned from chat: " + (object) this.IsChatDisabled + "]" + "[Access level:" + (object) this.AccessLevel + "][Group tag: " + this.GroupTag + "][Last login date: " + (object) this.LastLoginDate + "]]" + "[EmailAddressStatus:" + (object) this.EmailAddressStatus + "]]";
  }
}
